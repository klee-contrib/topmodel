using Microsoft.Extensions.Logging;
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
                c => $"{(c.Class.Abstract ? "I" : string.Empty)}{c.Class.NamePascal}{(!c.Required && Config.IsNullableEnabled ? "?" : string.Empty)} {c.Name}{(!c.Required ? " = null" : string.Empty)}",
                p => $"{Config.GetType(p.Property, nonNullable: mp.GetRequired() || Config.GetValue(p.Property, Classes) != "null")} {p.Property.NameCamel}{(!mp.GetRequired() ? $" = {Config.GetValue(p.Property, Classes)}" : string.Empty)}")))})");

            if (classe.Abstract)
            {
                w.WriteLine(2, $"where T : I{classe.NamePascal}");
            }

            w.WriteLine(1, "{");

            foreach (var param in mapper.Params.Where(p => p.GetRequired() && (p.IsT0 || !Config.NonNullableTypes.Contains(Config.GetImplementation(p.AsT1.Property.Domain)?.Type ?? string.Empty))))
            {
                w.WriteLine(2, $"ArgumentNullException.ThrowIfNull({param.GetNameCamel()});");
            }

            if (mapper.Params.Any(p => p.GetRequired()))
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

            foreach (var lol in mapper.Params)
            {
                lol.Switch(
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
                                value = Config.GetConvertedValue(value, mapping.Value.Domain, (mapping.Key as IFieldProperty)?.Domain);
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
                        if (classe.Abstract)
                        {
                            w.Write(3, $"{param.TargetProperty.NameCamel}: {param.NameCamel}");
                        }
                        else
                        {
                            w.Write(3, $"{param.TargetProperty.NamePascal} = {param.NameCamel}");
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

            w.WriteSummary(1, $"Mappe '{classe.NamePascal}' vers '{mapper.Class.NamePascal}'{(mapper.Comment != null ? $"\n{mapper.Comment}" : string.Empty)}");
            w.WriteParam("source", $"Instance de '{classe.NamePascal}'");
            if (!mapper.Class.Abstract)
            {
                w.WriteParam("dest", $"Instance pré-existante de '{mapper.Class.NamePascal}'. Une nouvelle instance sera créée si non spécifié.");
            }

            w.WriteReturns(1, $"Une instance de '{mapper.Class.NamePascal}'");

            if (mapper.Class.Abstract)
            {
                w.WriteLine(1, $"public static T {mapper.Name}<T>(this {classe.NamePascal} source)");
                w.WriteLine(2, $"where T : I{mapper.Class.NamePascal}");
            }
            else
            {
                w.WriteLine(1, $"public static {mapper.Class.NamePascal} {mapper.Name}(this {(classe.Abstract ? "I" : string.Empty)}{classe.NamePascal} source, {mapper.Class.NamePascal}{(Config.IsNullableEnabled ? "?" : string.Empty)} dest = null)");
            }

            w.WriteLine(1, "{");

            if (mapper.Class.Abstract)
            {
                w.WriteLine(2, $"return (T)T.Create(");
            }
            else
            {
                w.WriteLine(2, $"dest ??= new {mapper.Class.NamePascal}();");
            }

            static string GetSourceMapping(IProperty property)
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

            var mappings = mapper.Mappings.ToList();
            foreach (var mapping in mapper.Mappings)
            {
                var value = Config.GetConvertedValue($"source.{GetSourceMapping(mapping.Key)}", (mapping.Key as IFieldProperty)?.Domain, mapping.Value.Domain);

                if (mapper.Class.Abstract)
                {
                    w.Write(3, $"{mapping.Value.NameCamel}: {value}");

                    if (mappings.IndexOf(mapping) < mappings.Count - 1)
                    {
                        w.WriteLine(",");
                    }
                    else
                    {
                        w.WriteLine(");");
                    }
                }
                else
                {
                    w.WriteLine(2, $"dest.{mapping.Value.NamePascal} = {value};");
                }
            }

            if (!mapper.Class.Abstract)
            {
                w.WriteLine(2, "return dest;");
            }

            w.WriteLine(1, "}");

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
}