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
        if (property is { CompositionPrimaryKey: IProperty cpk })
        {
            return $"{property.NamePascal}?.{cpk.NamePascal}";
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

        var (mapperNs, modelPath) =
            sampleFromMapper != default
                ? Config.GetMapperLocation(sampleFromMapper, tag)
                : Config.GetMapperLocation(sampleToMapper, tag);

        var ns = Config.GetNamespace(mapperNs, modelPath, tag);

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

        if (usings.Exists(@using => !ns.Contains(@using)))
        {
            w.AddUsings(usings.Where(@using => !ns.Contains(@using)));
        }

        w.WriteNamespace(ns);
        w.WriteSummary($"Mappers pour le module '{mapperNs.Module}'.");
        w.WriteLine($"public static class {Config.GetMapperName(mapperNs)}");
        w.WriteLine("{");

        foreach (var fromMapper in fromMappers)
        {
            var (classe, mapper) = fromMapper;

            var requiredNonNullable = Config.RequiredNonNullable(Config.GetBestClassTag(classe, tag));

            w.WriteSummary(
                1,
                $"Crée une nouvelle instance de '{classe.NamePascal}'{(mapper.Comment != null ? $"\n{mapper.Comment}" : string.Empty)}"
            );
            foreach (var param in mapper.Params)
            {
                w.WriteParam(param.GetNameCamel(), param.GetComment());
            }

            w.WriteReturns(1, $"Une nouvelle instance de '{classe.NamePascal}'");

            if (classe.Abstract)
            {
                w.Write(1, $"public static T Create{classe.NamePascal}<T>");
            }
            else
            {
                w.Write(1, $"public static {classe.NamePascal} Create{classe.NamePascal}");
            }

            w.WriteLine(
                $"({string.Join(", ", mapper.Params.Select(mp => mp.Match(
                c => $"{(c.Class.Abstract ? "I" : string.Empty)}{c.Class.NamePascal}{(!c.Required && Config.NullableEnable ? "?" : string.Empty)} {c.Name}{(!c.Required ? " = null" : string.Empty)}",
                p => $"{Config.GetType(p.Property, nonNullable: mp.GetRequired() || Config.GetValue(p.Property) != "null")} {p.Property.NameCamel}{(!mp.GetRequired() ? $" = {Config.GetValue(p.Property)}" : string.Empty)}")))})"
            );

            if (classe.Abstract)
            {
                w.WriteLine(2, $"where T : I{classe.NamePascal}");
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

            if (requiredNonNullable)
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

            if (classe.Abstract)
            {
                w.WriteLine(2, $"return (T)T.Create(");
            }
            else
            {
                w.WriteLine(2, $"return new {classe.NamePascal}");
                w.WriteLine(2, "{");
            }

            foreach (var p in mapper.Params)
            {
                p.Switch(
                    param =>
                    {
                        var mappings = param.Mappings.ToList();
                        foreach (var mapping in mappings)
                        {
                            if (classe.Abstract)
                            {
                                w.Write(3, $"{mapping.Key.NameCamel}: ");
                            }
                            else
                            {
                                w.Write(3, $"{mapping.Key.NamePascal} = ");
                            }

                            var value =
                                $"{param.Name}{(!param.Required && mapping.Key is not { Composition: not null } ? "?" : string.Empty)}.{mapping.Value.NamePascal}";

                            if (
                                mapping.Key is { CompositionPrimaryKey: IProperty cpk }
                                && mapping.Value is { Composition: null }
                            )
                            {
                                w.Write(
                                    $"{(!param.Required ? $"{param.Name} is null ? null : " : string.Empty)}new() {{ {cpk.NamePascal} = "
                                );
                            }
                            else
                            {
                                var isValueType = Config.IsValueType(mapping.Value);

                                var targetType = Config.GetType(mapping.Key, nonNullable: true);
                                var sourceType = Config.GetType(mapping.Value, nonNullable: true);
                                if (
                                    !sourceType.EndsWith(targetType)
                                    && !targetType.EndsWith(sourceType)
                                    && Config.GetEnumType(mapping.Key).EndsWith(targetType)
                                )
                                {
                                    var enumType = Config.GetEnumType(mapping.Key);
                                    value = $"<{enumType}>({value}";

                                    w.AddUsing(
                                        Config.GetEnumTypeNamespace(
                                            mapping.Key,
                                            Config.GetBestClassTag(mapping.Key.Class, tag)
                                        )
                                    );

                                    if (!requiredNonNullable || !mapping.Key.Required && !mapping.Value.Required)
                                    {
                                        value =
                                            $"Enum.TryParse{value}, out var {mapping.Value.NameCamel}) ? {mapping.Value.NameCamel} : null";
                                    }
                                    else
                                    {
                                        value = $"Enum.Parse{value})";
                                    }
                                }
                                else if (
                                    !sourceType.EndsWith(targetType)
                                    && !targetType.EndsWith(sourceType)
                                    && Config.GetEnumType(mapping.Value).EndsWith(sourceType)
                                )
                                {
                                    if (!requiredNonNullable || !mapping.Key.Required && !mapping.Value.Required)
                                    {
                                        value = $"{value} != null ? Enum.GetName({value}.Value) : null";
                                    }
                                    else
                                    {
                                        if (requiredNonNullable && !mapping.Value.Required)
                                        {
                                            value += ".Value";
                                        }

                                        value = $"Enum.GetName({value})";
                                    }
                                }
                                else if (isValueType && requiredNonNullable && mapping.Key.Required && !param.Required)
                                {
                                    var type = Config.GetType(mapping.Value, nonNullable: true);
                                    var enumType = Config.GetEnumType(mapping.Value);
                                    var cast = enumType.Contains($".{type}") ? enumType : type;

                                    value = $"({cast}){value}";
                                }
                                else if (
                                    isValueType
                                    && requiredNonNullable
                                    && mapping.Key.Required
                                    && !mapping.Value.Required
                                )
                                {
                                    value += ".Value";
                                }

                                value = Config.GetConvertedValue(
                                    value,
                                    mapping.Value.Domain,
                                    mapping.Key.Domain,
                                    isValueType && (!requiredNonNullable || !mapping.Value.Required)
                                );
                            }

                            w.Write(value);

                            if (mapping.Key is { Composition: not null } && mapping.Value is { Composition: null })
                            {
                                w.Write(" }");
                            }

                            if (
                                mapper.Params.IndexOf(param) < mapper.Params.Count - 1
                                || mappings.IndexOf(mapping) < mappings.Count - 1
                            )
                            {
                                w.Write(",");
                            }
                            else if (classe.Abstract)
                            {
                                w.Write(");");
                            }

                            w.WriteLine();
                        }
                    },
                    param =>
                    {
                        var value = param.NameCamel;

                        if (
                            Config.IsValueType(param.Property)
                            && requiredNonNullable
                            && param.TargetProperty.Required
                            && !param.Property.Required
                        )
                        {
                            value += ".Value";
                        }

                        if (classe.Abstract)
                        {
                            w.Write(3, $"{param.TargetProperty.NameCamel}: {value}");
                        }
                        else
                        {
                            w.Write(3, $"{param.TargetProperty.NamePascal} = {value}");
                        }

                        if (mapper.Params.IndexOf(param) < mapper.Params.Count - 1)
                        {
                            w.Write(",");
                        }
                        else if (classe.Abstract)
                        {
                            w.Write(");");
                        }

                        w.WriteLine();
                    }
                );
            }

            if (!classe.Abstract)
            {
                w.WriteLine(2, "};");
            }

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
                $"Mappe '{classe.NamePascal}' vers '{mapper.Class.NamePascal}'{(mapper.Comment != null ? $"\n{mapper.Comment}" : string.Empty)}"
            );
            w.WriteParam("source", $"Instance de '{classe.NamePascal}'");

            var missingRequiredProperties = mapper
                .MissingRequiredProperties.Where(mrp =>
                    mrp is not { Composition: Class cpc } || Config.AvailableClasses.Contains(cpc)
                )
                .ToList();

            foreach (var mrp in missingRequiredProperties)
            {
                w.WriteParam(mrp.NameCamel, mrp.Comment);
            }

            w.WriteReturns(1, $"Une nouvelle instance de '{mapper.Class.NamePascal}'");

            var extraParams = string.Empty;
            if (missingRequiredProperties.Count > 0)
            {
                extraParams =
                    $", {string.Join(", ", missingRequiredProperties.Select(mrp => $"{Config.GetType(mrp, nonNullable: rrnTarget)} {mrp.NameCamel.Verbatim()}{(!rrnTarget ? " = null" : string.Empty)}"))}";
            }

            if (mapper.Class.Abstract)
            {
                w.WriteLine(1, $"public static T {mapper.Name}<T>(this {classe.NamePascal} source{extraParams})");
                w.WriteLine(2, $"where T : I{mapper.Class.NamePascal}");
            }
            else
            {
                w.WriteLine(
                    1,
                    $"public static {mapper.Class.NamePascal} {mapper.Name}(this {(classe.Abstract ? "I" : string.Empty)}{classe.NamePascal} source{extraParams})"
                );
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

            if (mapper.Class.Abstract)
            {
                w.WriteLine(2, $"return (T)T.Create(");
            }
            else
            {
                w.WriteLine(2, $"return new {mapper.Class.NamePascal}");
                w.WriteLine(2, "{");
            }

            foreach (var mapping in mapper.Mappings)
            {
                var value = $"source.{GetSourceMapping(mapping.Key)}";

                var isValueType = Config.IsValueType(mapping.Key);
                if (isValueType && rrnTarget && (!rrnSource || !mapping.Key.Required) && mapping.Value.Required)
                {
                    value += ".Value";
                }

                var sourceType = Config.GetType(mapping.Key, nonNullable: true);
                var targetType = Config.GetType(mapping.Value, nonNullable: true);
                if (
                    !sourceType.EndsWith(targetType)
                    && !targetType.EndsWith(sourceType)
                    && Config.GetEnumType(mapping.Key).EndsWith(sourceType)
                )
                {
                    if (!rrnSource || !mapping.Key.Required && !mapping.Value.Required)
                    {
                        value = $"{value} != null ? Enum.GetName({value}.Value) : null";
                    }
                    else
                    {
                        value = $"Enum.GetName({value})";
                    }
                }
                else if (
                    !sourceType.EndsWith(targetType)
                    && !targetType.EndsWith(sourceType)
                    && Config.GetEnumType(mapping.Value).EndsWith(targetType)
                )
                {
                    var enumType = Config.GetEnumType(mapping.Value);
                    value = $"<{enumType}>({value}";

                    w.AddUsing(
                        Config.GetEnumTypeNamespace(mapping.Value, Config.GetBestClassTag(mapping.Value.Class, tag))
                    );

                    if (!rrnSource || !mapping.Key.Required && !mapping.Value.Required)
                    {
                        value =
                            $"Enum.TryParse{value}, out var {mapping.Key.NameCamel}) ? {mapping.Key.NameCamel} : null";
                    }
                    else
                    {
                        value = $"Enum.Parse{value})";
                    }
                }

                value = Config.GetConvertedValue(
                    value,
                    mapping.Key.Domain,
                    mapping.Value.Domain,
                    isValueType && (!rrnSource || !mapping.Key.Required)
                );

                if (mapper.Class.Abstract)
                {
                    w.Write(3, $"{mapping.Value.NameCamel}: {value}");
                }
                else
                {
                    w.Write(3, $"{mapping.Value.NamePascal} = {value}");
                }

                if (mappings.IndexOf(mapping) < mappings.Count - 1 || missingRequiredProperties.Count > 0)
                {
                    w.WriteLine(",");
                }
                else if (mapper.Class.Abstract)
                {
                    w.WriteLine(");");
                }
                else
                {
                    w.WriteLine();
                }
            }

            foreach (var mrp in missingRequiredProperties)
            {
                if (mapper.Class.Abstract)
                {
                    w.Write(3, $"{mrp.NameCamel}: {mrp.NameCamel.Verbatim()}");
                }
                else
                {
                    w.Write(3, $"{mrp.NamePascal} = {mrp.NameCamel.Verbatim()}");
                }

                if (missingRequiredProperties.IndexOf(mrp) < missingRequiredProperties.Count - 1)
                {
                    w.WriteLine(",");
                }
                else if (mapper.Class.Abstract)
                {
                    w.WriteLine(");");
                }
                else
                {
                    w.WriteLine();
                }
            }

            if (!mapper.Class.Abstract)
            {
                w.WriteLine(2, "};");
            }

            w.WriteLine(1, "}");

            if (!mapper.Class.Abstract)
            {
                w.WriteLine();
                w.WriteSummary(
                    1,
                    $"Mappe '{classe.NamePascal}' vers '{mapper.Class.NamePascal}'{(mapper.Comment != null ? $"\n{mapper.Comment}" : string.Empty)}"
                );
                w.WriteParam("source", $"Instance de '{classe.NamePascal}'");
                w.WriteParam("dest", $"Instance pré-existante de '{mapper.Class.NamePascal}'.");
                w.WriteReturns(1, $"L'instance pré-existante de '{mapper.Class.NamePascal}'");
                w.WriteLine(
                    1,
                    $"public static {mapper.Class.NamePascal} {mapper.Name}(this {(classe.Abstract ? "I" : string.Empty)}{classe.NamePascal} source, {mapper.Class.NamePascal} dest)"
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

                foreach (var mapping in mapper.Mappings)
                {
                    var value = $"source.{GetSourceMapping(mapping.Key)}";

                    var isValueType = Config.IsValueType(mapping.Key);
                    if (isValueType && rrnTarget && (!rrnSource || !mapping.Key.Required) && mapping.Value.Required)
                    {
                        value += ".Value";
                    }

                    var sourceType = Config.GetType(mapping.Key, nonNullable: true);
                    var targetType = Config.GetType(mapping.Value, nonNullable: true);
                    if (
                        !sourceType.EndsWith(targetType)
                        && !targetType.EndsWith(sourceType)
                        && Config.GetEnumType(mapping.Key).EndsWith(sourceType)
                    )
                    {
                        if (!rrnSource || !mapping.Key.Required && !mapping.Value.Required)
                        {
                            value = $"{value} != null ? Enum.GetName({value}.Value) : null";
                        }
                        else
                        {
                            value = $"Enum.GetName({value})";
                        }
                    }
                    else if (
                        !sourceType.EndsWith(targetType)
                        && !targetType.EndsWith(sourceType)
                        && Config.GetEnumType(mapping.Value).EndsWith(targetType)
                    )
                    {
                        var enumType = Config.GetEnumType(mapping.Value);
                        value = $"<{enumType}>({value}";

                        w.AddUsing(
                            Config.GetEnumTypeNamespace(mapping.Value, Config.GetBestClassTag(mapping.Value.Class, tag))
                        );

                        if (!rrnSource || !mapping.Key.Required && !mapping.Value.Required)
                        {
                            value =
                                $"Enum.TryParse{value}, out var {mapping.Key.NameCamel}) ? {mapping.Key.NameCamel} : null";
                        }
                        else
                        {
                            value = $"Enum.Parse{value})";
                        }
                    }

                    value = Config.GetConvertedValue(
                        value,
                        mapping.Key.Domain,
                        mapping.Value.Domain,
                        isValueType && (!rrnSource || !mapping.Key.Required)
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
