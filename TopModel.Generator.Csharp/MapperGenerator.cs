﻿using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;

namespace TopModel.Generator.Csharp;

public class MapperGenerator : MapperGeneratorBase<CsharpConfig>
{
    private readonly ILogger<MapperGenerator> _logger;

    public MapperGenerator(ILogger<MapperGenerator> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override string Name => "CSharpMapperGen";

    protected override string GetFileName((Class Classe, FromMapper Mapper) mapper, string tag)
    {
        return Config.GetMapperFilePath(mapper, tag);
    }

    protected override string GetFileName((Class Classe, ClassMappings Mapper) mapper, string tag)
    {
        return Config.GetMapperFilePath(mapper, tag);
    }

    protected override void HandleFile(string fileName, string tag, IList<(Class Classe, FromMapper Mapper)> fromMappers, IList<(Class Classe, ClassMappings Mapper)> toMappers)
    {
        using var w = new CSharpWriter(fileName, _logger);

        var sampleFromMapper = fromMappers.FirstOrDefault();
        var sampleToMapper = toMappers.FirstOrDefault();

        var (mapperNs, modelPath) = sampleFromMapper != default
            ? Config.GetMapperLocation(sampleFromMapper, tag)
            : Config.GetMapperLocation(sampleToMapper, tag);

        var ns = Config.GetNamespace(mapperNs, modelPath, tag);

        var usings = fromMappers.SelectMany(m => m.Mapper.ClassParams.Select(p => p.Class).Concat(new[] { m.Classe }))
            .Concat(toMappers.SelectMany(m => new[] { m.Classe, m.Mapper.Class }))
            .Select(c => Config.GetNamespace(c, GetBestClassTag(c, tag)))
            .ToList();

        foreach (var mapping in fromMappers.SelectMany(fm => fm.Mapper.PropertyParams))
        {
            usings.AddRange(Config.GetDomainImports(mapping.Property, tag));

            if (mapping.Property is IFieldProperty fp)
            {
                usings.AddRange(Config.GetValueImports(fp));
            }

            switch (mapping.Property)
            {
                case AssociationProperty ap when Classes.Contains(ap.Association) && Config.CanClassUseEnums(ap.Association, Classes, ap.Property):
                    usings.Add(Config.GetNamespace(ap.Association, GetBestClassTag(ap.Association, tag)));
                    break;
                case AliasProperty { Property: AssociationProperty ap2 } when Classes.Contains(ap2.Association) && Config.CanClassUseEnums(ap2.Association, Classes, ap2.Property):
                    usings.Add(Config.GetNamespace(ap2.Association, GetBestClassTag(ap2.Association, tag)));
                    break;
                case AliasProperty { Property: RegularProperty rp } alp when Classes.Contains(rp.Class) && Config.CanClassUseEnums(rp.Class, Classes, rp):
                    usings.Add(Config.GetNamespace(rp.Class, GetBestClassTag(rp.Class, tag)));
                    break;
                case CompositionProperty cp:
                    usings.Add(Config.GetNamespace(cp.Composition, GetBestClassTag(cp.Composition, tag)));
                    break;
            }
        }

        if (usings.Any(@using => !ns.Contains(@using)))
        {
            w.WriteUsings(usings.Where(@using => !ns.Contains(@using)).Distinct().ToArray());
            w.WriteLine();
        }

        w.WriteNamespace(ns);
        w.WriteSummary($"Mappers pour le module '{mapperNs.Module}'.");
        w.WriteLine($"public static class {Config.GetMapperName(mapperNs, modelPath)}");
        w.WriteLine("{");

        foreach (var fromMapper in fromMappers)
        {
            var (classe, mapper) = fromMapper;

            var requiredNonNullable = Config.RequiredNonNullable(GetBestClassTag(classe, tag));

            w.WriteSummary(1, $"Crée une nouvelle instance de '{classe.NamePascal}'{(mapper.Comment != null ? $"\n{mapper.Comment}" : string.Empty)}");
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

            w.WriteLine($"({string.Join(", ", mapper.Params.Select(mp => mp.Match(
                c => $"{(c.Class.Abstract ? "I" : string.Empty)}{c.Class.NamePascal}{(!c.Required && Config.NullableEnable ? "?" : string.Empty)} {c.Name}{(!c.Required ? " = null" : string.Empty)}",
                p => $"{Config.GetType(p.Property, nonNullable: mp.GetRequired() || Config.GetValue(p.Property, Classes) != "null")} {p.Property.NameCamel}{(!mp.GetRequired() ? $" = {Config.GetValue(p.Property, Classes)}" : string.Empty)}")))})");

            if (classe.Abstract)
            {
                w.WriteLine(2, $"where T : I{classe.NamePascal}");
            }

            w.WriteLine(1, "{");

            var requiredParams = mapper.Params.Where(p => p.GetRequired() && (p.IsT0 || !Config.AllValueTypes.Contains(Config.GetImplementation(p.AsT1.Property.Domain)?.Type ?? string.Empty)));
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
                    foreach (var mapping in param.Mappings.Where(m => m.Key.Required && (!param.Required || !m.Value.Required)))
                    {
                        w.WriteLine(2, $"ArgumentNullException.ThrowIfNull({param.Name}{(!param.Required ? "?" : string.Empty)}.{mapping.Value.NamePascal});");
                        wlForCheck = true;
                    }
                }

                foreach (var param in mapper.PropertyParams.Where(p => p.TargetProperty.Required && !p.Property.Required))
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

                            var value = $"{param.Name}{(!param.Required && mapping.Key is not CompositionProperty ? "?" : string.Empty)}.{mapping.Value.NamePascal}";

                            if (mapping.Key is CompositionProperty cp)
                            {
                                w.Write($"{(!param.Required ? $"{param.Name} is null ? null : " : string.Empty)}new() {{ {cp.CompositionPrimaryKey?.NamePascal} = ");
                            }
                            else
                            {
                                var isValueType = Config.IsValueType(mapping.Value, Classes);

                                if (isValueType && requiredNonNullable && mapping.Key.Required && !param.Required)
                                {
                                    var type = Config.GetType(mapping.Value, Classes, nonNullable: true);
                                    var enumType = Config.GetEnumType((IFieldProperty)mapping.Value);
                                    var cast = enumType.Contains($".{type}") ? enumType : type;

                                    value = $"({cast}){value}";
                                }
                                else if (isValueType && requiredNonNullable && mapping.Key.Required && !mapping.Value.Required)
                                {
                                    value += ".Value";
                                }

                                value = Config.GetConvertedValue(value, mapping.Value.Domain, (mapping.Key as IFieldProperty)?.Domain, isValueType && (!requiredNonNullable || !mapping.Value.Required));
                            }

                            w.Write(value);

                            if (mapping.Key is CompositionProperty)
                            {
                                w.Write(" }");
                            }

                            if (mapper.Params.IndexOf(param) < mapper.Params.Count - 1 || mappings.IndexOf(mapping) < mappings.Count - 1)
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

                        if (Config.IsValueType(param.Property, Classes))
                        {
                            if (requiredNonNullable && param.TargetProperty.Required && !param.Property.Required)
                            {
                                value += ".Value";
                            }
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
                    });
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

            var rrnSource = Config.RequiredNonNullable(GetBestClassTag(classe, tag));
            var rrnTarget = Config.RequiredNonNullable(GetBestClassTag(mapper.Class, tag));

            var hasNewToMapper = !rrnTarget ||
                !mapper.Class.Properties.OfType<IFieldProperty>()
                    .Where(p =>
                        p.Required
                        && p.DefaultValue == null
                        && !(p.Class.IsPersistent && p.PrimaryKey && p.Class.PrimaryKey.Count() == 1)
                        && !p.Domain.AutoGeneratedValue)
                    .Except(mapper.Mappings.Select(m => m.Value))
                    .Any();
            var hasExistingToMapper = !mapper.Class.Abstract;

            if (hasNewToMapper)
            {
                w.WriteSummary(1, $"Mappe '{classe.NamePascal}' vers '{mapper.Class.NamePascal}'{(mapper.Comment != null ? $"\n{mapper.Comment}" : string.Empty)}");
                w.WriteParam("source", $"Instance de '{classe.NamePascal}'");
                w.WriteReturns(1, $"Une nouvelle instance de '{mapper.Class.NamePascal}'");

                if (mapper.Class.Abstract)
                {
                    w.WriteLine(1, $"public static T {mapper.Name}<T>(this {classe.NamePascal} source)");
                    w.WriteLine(2, $"where T : I{mapper.Class.NamePascal}");
                }
                else
                {
                    w.WriteLine(1, $"public static {mapper.Class.NamePascal} {mapper.Name}(this {(classe.Abstract ? "I" : string.Empty)}{classe.NamePascal} source)");
                }

                w.WriteLine(1, "{");

                if (rrnTarget)
                {
                    var requiredMappings = mapper.Mappings.Where(m => (!rrnSource || !m.Key.Required) && m.Value.Required).ToList();
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

                    var isValueType = Config.IsValueType(mapping.Key, Classes);
                    if (isValueType && rrnTarget && (!rrnSource || !mapping.Key.Required) && mapping.Value.Required)
                    {
                        value += ".Value";
                    }

                    value = Config.GetConvertedValue(value, (mapping.Key as IFieldProperty)?.Domain, mapping.Value.Domain, isValueType && (!rrnSource || !mapping.Key.Required));

                    if (mapper.Class.Abstract)
                    {
                        w.Write(3, $"{mapping.Value.NameCamel}: {value}");
                    }
                    else
                    {
                        w.Write(3, $"{mapping.Value.NamePascal} = {value}");
                    }

                    if (mappings.IndexOf(mapping) < mappings.Count - 1)
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
            }

            if (hasExistingToMapper && hasNewToMapper)
            {
                w.WriteLine();
            }

            if (hasExistingToMapper)
            {
                w.WriteSummary(1, $"Mappe '{classe.NamePascal}' vers '{mapper.Class.NamePascal}'{(mapper.Comment != null ? $"\n{mapper.Comment}" : string.Empty)}");
                w.WriteParam("source", $"Instance de '{classe.NamePascal}'");
                w.WriteParam("dest", $"Instance pré-existante de '{mapper.Class.NamePascal}'.");
                w.WriteReturns(1, $"L'instance pré-existante de '{mapper.Class.NamePascal}'");
                w.WriteLine(1, $"public static {mapper.Class.NamePascal} {mapper.Name}(this {(classe.Abstract ? "I" : string.Empty)}{classe.NamePascal} source, {mapper.Class.NamePascal} dest)");
                w.WriteLine(1, "{");

                if (rrnTarget)
                {
                    var requiredMappings = mapper.Mappings.Where(m => (!rrnSource || !m.Key.Required) && m.Value.Required).ToList();
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

                    var isValueType = Config.IsValueType(mapping.Key, Classes);
                    if (isValueType && rrnTarget && (!rrnSource || !mapping.Key.Required) && mapping.Value.Required)
                    {
                        value += ".Value";
                    }

                    value = Config.GetConvertedValue(value, (mapping.Key as IFieldProperty)?.Domain, mapping.Value.Domain, isValueType && (!rrnSource || !mapping.Key.Required));

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
            && (Config.PersistentModelPath == Config.PersistentReferencesModelPath || !classe.Reference);
    }

    private static string GetSourceMapping(IProperty property)
    {
        if (property is CompositionProperty cp)
        {
            return $"{cp.NamePascal}?.{cp.CompositionPrimaryKey?.NamePascal}";
        }
        else
        {
            return property.NamePascal;
        }
    }
}