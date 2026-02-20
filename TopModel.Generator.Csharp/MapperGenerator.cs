using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Csharp;

public class MapperGenerator(ILogger<MapperGenerator> logger, IFileWriterProvider writerProvider)
    : MapperGeneratorBase<CsharpConfig>(logger, writerProvider)
{
    public override string Name => "CSharpMapperGen";

    protected override string GetFileName((Class Classe, FromMapper Mapper) mapper, string tag)
    {
        return Config.GetMapperFilePath(mapper, tag);
    }

    protected override string GetFileName((Class Classe, ClassMappings Mapper) mapper, string tag)
    {
        return Config.GetMapperFilePath(mapper, tag);
    }

    protected virtual string GetSourceMapping(IProperty property)
    {
        if (property.MappingType.TryPickT1(out var t1, out _) && t1.Property != null)
        {
            return $"{property.NamePascal}?.{t1.Property.NamePascal}";
        }
        else
        {
            return property.NamePascal;
        }
    }

    protected override void HandleFile(
        string fileName,
        string tag,
        IList<(Class Classe, FromMapper Mapper)> fromMappers,
        IList<(Class Classe, ClassMappings Mapper)> toMappers
    )
    {
        using var w = this.OpenCSharpWriter(fileName);

        var sampleFromMapper = fromMappers.FirstOrDefault();
        var sampleToMapper = toMappers.FirstOrDefault();

        var (mapperName, mapperNs, mapperModule) =
            sampleFromMapper != default
                ? Config.GetMapperInfo(sampleFromMapper, tag)
                : Config.GetMapperInfo(sampleToMapper, tag);

        var usings = fromMappers
            .SelectMany(m => m.Mapper.ClassParams.Select(p => p.Class).Concat([m.Classe]))
            .Concat(toMappers.SelectMany(m => new[] { m.Classe, m.Mapper.Class }))
            .Select(c => Config.GetNamespace(c, Config.GetBestClassTag(c, tag)))
            .ToList();

        foreach (
            var property in fromMappers.SelectMany(fm =>
                fm.Mapper.PropertyParams.Select(pp => pp.Property)
                    .Concat(toMappers.SelectMany(tm => tm.Mapper.MissingRequiredProperties))
            )
        )
        {
            usings.AddRange(Config.GetDomainImports(property, tag));
            usings.AddRange(Config.GetValueImports(property));

            switch (property)
            {
                case { EnumProperty: IProperty ep } when Config.AvailableClasses.Contains(ep.Class):
                    usings.Add(Config.GetNamespace(ep.Class, Config.GetBestClassTag(ep.Class, tag)));
                    break;
                case { Composition: Class cpc } when Config.AvailableClasses.Contains(cpc):
                    usings.Add(Config.GetNamespace(cpc, Config.GetBestClassTag(cpc, tag)));
                    break;
            }
        }

        foreach (var mapping in fromMappers.SelectMany(fm => fm.Mapper.ClassParams.Select(c => c.Mappings)))
        {
            usings.AddRange(mapping.SelectMany(m => Config.GetConverterImports(m.Value.Domain, m.Key.Domain)));
        }

        foreach (var mapping in toMappers.Select(fm => fm.Mapper.Mappings))
        {
            usings.AddRange(mapping.SelectMany(m => Config.GetConverterImports(m.Key.Domain, m.Value.Domain)));
        }

        if (usings.Exists(@using => !mapperNs.Contains(@using)))
        {
            w.AddUsings(usings.Where(@using => !mapperNs.Contains(@using)));
        }

        string GetValue(
            string? paramName,
            bool paramRequired,
            IProperty source,
            IProperty target,
            bool rrnSource,
            bool rrnTarget
        )
        {
            string GetMappedValue(string value, Class sourceClass, Class targetClass, bool nullCheck)
            {
                var mapper = sourceClass.GetMapperTo(targetClass)!.Value;
                return mapper.Match(
                    fromMapper =>
                    {
                        var targetTag = Config.GetBestClassTag(sourceClass, tag);
                        var (targetMapperName, targetMapperNs, _) = Config.GetMapperInfo(
                            (sourceClass, fromMapper),
                            targetTag
                        );

                        if (targetMapperNs != mapperNs)
                        {
                            w.AddUsing(targetMapperNs);
                        }

                        var mapped =
                            $"{(targetMapperNs != mapperNs || targetMapperName != mapperName ? $"{targetMapperName}." : string.Empty)}Create{targetClass.NamePascal}({value})";
                        return
                            nullCheck
                            && (!rrnSource && !rrnTarget && paramName != null || !source.Required || !paramRequired)
                            ? $"{value} != null ? {mapped} : {(!rrnTarget && target.Required && target.Composition != null ? "new()" : "null")}"
                            : mapped;
                    },
                    toMapper =>
                    {
                        var targetTag = Config.GetBestClassTag(sourceClass, tag);
                        var (_, targetMapperNs, _) = Config.GetMapperInfo((targetClass, toMapper), targetTag);

                        if (targetMapperNs != mapperNs)
                        {
                            w.AddUsing(targetMapperNs);
                        }

                        value =
                            $"{value}{(nullCheck && (!rrnSource && !rrnTarget || !source.Required) ? "?" : string.Empty)}.To{targetClass}()";
                        if (nullCheck && !rrnTarget && target.Required && target.Composition != null)
                        {
                            value += " ?? new()";
                        }
                        return value;
                    }
                );
            }

            var value =
                paramName == null
                    ? source.NameCamel
                    : $"{paramName}{(!paramRequired ? "?" : string.Empty)}.{source.NamePascal}";

            if (source.MappingType.TryPickT2(out var st2, out _))
            {
                if (target.MappingType.IsT0 && st2.Property != null)
                {
                    var innerProp = st2.Property.NamePascal;
                    if (st2.Property.MappingType.TryPickT1(out var t1, out _) && t1.Property != null)
                    {
                        innerProp +=
                            $"{(Config.NullableEnable && st2.Property.Required ? "!" : string.Empty)}.{t1.Property.NamePascal}";
                    }

                    value +=
                        $"{(rrnSource && !source.Required ? "?" : string.Empty)}.Select(p => {HandleConversion($"p.{innerProp}", t1?.Property ?? st2.Property, target, rrnSource, rrnTarget, paramRequired: true, collection: true)}).{Config.GetCollector(st2.Domain)}";
                }
                else if (target.MappingType.TryPickT2(out var tt2, out _))
                {
                    var selector = $"p => {GetMappedValue("p", st2.Class, tt2.Class, nullCheck: false)}";
                    if (!selector.StartsWith("p => p."))
                    {
                        selector = selector[5..^3];
                    }
                    value =
                        $"{value}{(rrnSource && !source.Required ? "?" : string.Empty)}.Select({selector}).{Config.GetCollector(st2.Domain)}";
                }
            }
            else if (
                source.MappingType.TryPickT1(out var st1, out _)
                && target.MappingType.TryPickT1(out var tt1, out _)
                && st1.Class != tt1.Class
            )
            {
                value = GetMappedValue(value, st1.Class, tt1.Class, nullCheck: true);
            }
            else
            {
                if (source.MappingType.TryPickT1(out var t1, out _) && t1.Property != null && target.MappingType.IsT0)
                {
                    source = t1.Property;
                    value += $"{(!rrnSource || !source.Required ? "?" : string.Empty)}.{t1.Property.NamePascal}";
                }

                value = HandleConversion(value, source, target, rrnSource, rrnTarget, paramRequired);
            }

            return value;
        }

        string HandleConversion(
            string value,
            IProperty source,
            IProperty target,
            bool rrnSource,
            bool rrnTarget,
            bool paramRequired,
            bool collection = false
        )
        {
            var isValueType = Config.IsValueType(source);

            var targetType = Config.GetType(target, skipChain: collection ? 1 : 0, nonNullable: true);
            var sourceType = Config.GetType(source, nonNullable: true);
            if (
                !sourceType.EndsWith(targetType)
                && !targetType.EndsWith(sourceType)
                && Config.GetEnumType(target).EndsWith(targetType)
            )
            {
                var enumType = Config.GetEnumType(target);
                value = $"<{enumType}>({value}";

                w.AddUsing(Config.GetEnumTypeNamespace(target, Config.GetBestClassTag(target.Class, tag)));

                if (!rrnSource || !target.Required && !source.Required)
                {
                    value = $"Enum.TryParse{value}, out var {source.NameCamel}) ? {source.NameCamel} : null";
                }
                else
                {
                    value = $"Enum.Parse{value})";
                }
            }
            else if (
                !sourceType.EndsWith(targetType)
                && !targetType.EndsWith(sourceType)
                && Config.GetEnumType(source).EndsWith(sourceType)
            )
            {
                if (!rrnSource || !target.Required && !source.Required)
                {
                    value = $"{value} != null ? Enum.GetName({value}.Value) : null";
                }
                else
                {
                    if (rrnSource && !source.Required)
                    {
                        value += ".Value";
                    }

                    value = $"Enum.GetName({value})";
                }
            }
            else if (isValueType && rrnSource && target.Required && !paramRequired)
            {
                var type = Config.GetType(source, nonNullable: true);
                var enumType = Config.GetEnumType(source);
                var cast = enumType.Contains($".{type}") ? enumType : type;

                value = $"({cast}){value}";
            }
            else if (
                isValueType
                && (
                    rrnSource && target.Required && !source.Required
                    || collection && source.Required
                    || rrnTarget && (!rrnSource || !source.Required) && target.Required
                )
            )
            {
                if (Config.NullableEnable && !rrnSource && !rrnTarget && source.Required)
                {
                    value += "!";
                }

                value += ".Value";
            }

            return Config.GetConvertedValue(
                value,
                source.Domain,
                target.Domain,
                isValueType && (!rrnSource || !source.Required)
            );
        }

        w.WriteNamespace(mapperNs);
        w.WriteSummary($"Mappers pour le module '{mapperModule}'.");
        w.WriteLine($"public static class {mapperName}");
        w.WriteLine("{");

        foreach (var fromMapper in fromMappers)
        {
            var (classe, mapper) = fromMapper;

            var rrnTarget = Config.RequiredNonNullable(Config.GetBestClassTag(classe, tag));

            w.WriteSummary(
                1,
                $"Crée une nouvelle instance de '{Config.GetTypeName(classe)}'{(mapper.Comment != null ? $"\n{mapper.Comment}" : string.Empty)}"
            );
            foreach (var param in mapper.Params)
            {
                w.WriteParam(param.GetNameCamel(), param.GetComment());
            }

            w.WriteReturns(1, $"Une nouvelle instance de '{Config.GetTypeName(classe)}'");

            w.Write(
                1,
                $"public static {Config.GetTypeName(classe)} Create{classe.NamePascal}{(classe.Abstract ? "<T>" : string.Empty)}"
            );

            w.WriteLine(
                $"({string.Join(", ", mapper.Params.Select(mp => mp.Match(
                c => $"{Config.GetTypeName(c.Class)}{(!c.Required && Config.NullableEnable ? "?" : string.Empty)} {c.Name}{(!c.Required ? " = null" : string.Empty)}",
                p => $"{Config.GetType(p.Property, nonNullable: mp.GetRequired() || Config.GetValue(p.Property) != "null")} {p.Property.NameCamel}{(!mp.GetRequired() ? $" = {Config.GetValue(p.Property)}" : string.Empty)}")))})"
            );

            if (classe.Abstract)
            {
                w.WriteLine(2, $"where T : {Config.GetTypeName(classe)}, new()");
            }

            w.WriteLine(1, "{");

            var requiredParams = mapper.Params.Where(p =>
                p.GetRequired()
                && (
                    p.IsT0
                    || !Config.AllValueTypes.Contains(
                        Config.GetImplementation(p.AsT1.Property.Domain)?.Type ?? string.Empty
                    )
                )
            );
            var wlForCheck = false;

            foreach (var param in requiredParams)
            {
                w.WriteLine(2, $"ArgumentNullException.ThrowIfNull({param.GetNameCamel()});");
                wlForCheck = true;
            }

            if (rrnTarget)
            {
                foreach (var param in mapper.ClassParams)
                {
                    foreach (
                        var mapping in param.Mappings.Where(m =>
                            m.Key.Required && (!param.Required || !m.Value.Required)
                        )
                    )
                    {
                        w.WriteLine(
                            2,
                            $"ArgumentNullException.ThrowIfNull({param.Name}{(!param.Required ? "?" : string.Empty)}.{mapping.Value.NamePascal});"
                        );
                        wlForCheck = true;
                    }
                }

                foreach (
                    var param in mapper.PropertyParams.Where(p => p.TargetProperty.Required && !p.Property.Required)
                )
                {
                    w.WriteLine(2, $"ArgumentNullException.ThrowIfNull({param.NameCamel});");
                    wlForCheck = true;
                }
            }

            if (wlForCheck)
            {
                w.WriteLine();
            }

            w.WriteLine(2, $"return new {(classe.Abstract ? "T" : Config.GetTypeName(mapper.Class))}");
            w.WriteLine(2, "{");

            foreach (var p in mapper.Params)
            {
                p.Switch(
                    param =>
                    {
                        var rrnSource = Config.RequiredNonNullable(Config.GetBestClassTag(param.Class, tag));

                        var mappings = param.Mappings.ToList();
                        foreach (var mapping in mappings)
                        {
                            var value = GetValue(
                                param.Name,
                                param.Required,
                                mapping.Value,
                                mapping.Key,
                                rrnSource,
                                rrnTarget
                            );

                            w.Write(3, $"{mapping.Key.NamePascal} = {value}");

                            if (
                                mapper.Params.IndexOf(param) < mapper.Params.Count - 1
                                || mappings.IndexOf(mapping) < mappings.Count - 1
                            )
                            {
                                w.Write(",");
                            }

                            w.WriteLine();
                        }
                    },
                    param =>
                    {
                        var value = GetValue(
                            paramName: null,
                            param.Property.Required,
                            param.Property,
                            param.TargetProperty,
                            rrnTarget,
                            rrnTarget
                        );

                        w.Write(3, $"{param.TargetProperty.NamePascal} = {value}");

                        if (mapper.Params.IndexOf(param) < mapper.Params.Count - 1)
                        {
                            w.Write(",");
                        }

                        w.WriteLine();
                    }
                );
            }

            w.WriteLine(2, "};");
            w.WriteLine(1, "}");

            if (toMappers.Any() || fromMappers.IndexOf(fromMapper) < fromMappers.Count - 1)
            {
                w.WriteLine();
            }
        }

        foreach (var toMapper in toMappers)
        {
            var (classe, mapper) = toMapper;
            var mappings = mapper.Mappings.ToList();

            var rrnSource = Config.RequiredNonNullable(Config.GetBestClassTag(classe, tag));
            var rrnTarget = Config.RequiredNonNullable(Config.GetBestClassTag(mapper.Class, tag));

            w.WriteSummary(
                1,
                $"Mappe '{Config.GetTypeName(classe)}' vers '{Config.GetTypeName(mapper.Class)}'{(mapper.Comment != null ? $"\n{mapper.Comment}" : string.Empty)}"
            );
            w.WriteParam("source", $"Instance de '{Config.GetTypeName(classe)}'");

            var missingRequiredProperties = mapper
                .MissingRequiredProperties.Where(mrp =>
                    mrp is not { Composition: Class cpc } || Config.AvailableClasses.Contains(cpc)
                )
                .ToList();

            foreach (var mrp in missingRequiredProperties)
            {
                w.WriteParam(mrp.NameCamel, mrp.Comment);
            }

            w.WriteReturns(1, $"Une nouvelle instance de '{Config.GetTypeName(mapper.Class)}'");

            var extraParams = string.Empty;
            if (missingRequiredProperties.Count > 0)
            {
                extraParams =
                    $", {string.Join(", ", missingRequiredProperties.Select(mrp => $"{Config.GetType(mrp, nonNullable: rrnTarget)} {mrp.NameCamel.Verbatim()}{(!rrnTarget ? " = null" : string.Empty)}"))}";
            }

            w.WriteLine(
                1,
                $"public static {Config.GetTypeName(mapper.Class)} {mapper.Name}{(mapper.Class.Abstract ? "<T>" : string.Empty)}(this {Config.GetTypeName(classe)} source{extraParams})"
            );

            if (mapper.Class.Abstract)
            {
                w.WriteLine(2, $"where T : {Config.GetTypeName(mapper.Class)}, new()");
            }

            w.WriteLine(1, "{");

            if (rrnTarget)
            {
                var requiredMappings = mapper
                    .Mappings.Where(m => (!rrnSource || !m.Key.Required) && m.Value.Required)
                    .ToList();
                foreach (var mapping in requiredMappings)
                {
                    w.WriteLine(2, $"ArgumentNullException.ThrowIfNull(source.{GetSourceMapping(mapping.Key)});");
                }

                if (requiredMappings.Count > 0)
                {
                    w.WriteLine();
                }
            }

            w.WriteLine(2, $"return new {(mapper.Class.Abstract ? "T" : Config.GetTypeName(mapper.Class))}");
            w.WriteLine(2, "{");

            foreach (var mapping in mapper.Mappings)
            {
                var value = GetValue("source", paramRequired: true, mapping.Key, mapping.Value, rrnSource, rrnTarget);

                w.Write(3, $"{mapping.Value.NamePascal} = {value}");

                if (mappings.IndexOf(mapping) < mappings.Count - 1 || missingRequiredProperties.Count > 0)
                {
                    w.WriteLine(",");
                }
                else
                {
                    w.WriteLine();
                }
            }

            foreach (var mrp in missingRequiredProperties)
            {
                w.Write(3, $"{mrp.NamePascal} = {mrp.NameCamel.Verbatim()}");

                if (missingRequiredProperties.IndexOf(mrp) < missingRequiredProperties.Count - 1)
                {
                    w.WriteLine(",");
                }
                else
                {
                    w.WriteLine();
                }
            }

            w.WriteLine(2, "};");
            w.WriteLine(1, "}");

            if (!mapper.Class.Abstract)
            {
                w.WriteLine();
                w.WriteSummary(
                    1,
                    $"Mappe '{Config.GetTypeName(classe)}' vers '{Config.GetTypeName(mapper.Class)}'{(mapper.Comment != null ? $"\n{mapper.Comment}" : string.Empty)}"
                );
                w.WriteParam("source", $"Instance de '{Config.GetTypeName(classe)}'");
                w.WriteParam("dest", $"Instance pré-existante de '{Config.GetTypeName(mapper.Class)}'.");
                w.WriteReturns(1, $"L'instance pré-existante de '{Config.GetTypeName(mapper.Class)}'");
                w.WriteLine(
                    1,
                    $"public static {Config.GetTypeName(mapper.Class)} {mapper.Name}(this {Config.GetTypeName(classe)} source, {Config.GetTypeName(mapper.Class)} dest)"
                );
                w.WriteLine(1, "{");

                if (rrnTarget)
                {
                    var requiredMappings = mapper
                        .Mappings.Where(m => (!rrnSource || !m.Key.Required) && m.Value.Required)
                        .ToList();
                    foreach (var mapping in requiredMappings)
                    {
                        w.WriteLine(2, $"ArgumentNullException.ThrowIfNull(source.{GetSourceMapping(mapping.Key)});");
                    }

                    if (requiredMappings.Count > 0)
                    {
                        w.WriteLine();
                    }
                }

                foreach (var mapping in mapper.Mappings.Where(m => !m.Value.Readonly))
                {
                    var value = GetValue(
                        "source",
                        paramRequired: true,
                        mapping.Key,
                        mapping.Value,
                        rrnSource,
                        rrnTarget
                    );
                    w.WriteLine(2, $"dest.{mapping.Value.NamePascal} = {value};");
                }

                w.WriteLine(2, "return dest;");
                w.WriteLine(1, "}");
            }

            if (toMappers.IndexOf(toMapper) < toMappers.Count - 1)
            {
                w.WriteLine();
            }
        }

        w.WriteLine("}");
    }

    protected override bool IsPersistent(Class classe)
    {
        return (classe.Tags.Intersect(Config.MapperTagsOverrides).Any() || classe.IsPersistent)
            && (Config.ReferencesModelPath == null || !classe.Reference);
    }
}
