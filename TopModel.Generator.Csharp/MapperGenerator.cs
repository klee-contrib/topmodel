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
        using var w = new CSharpWriter(fileName, _logger, Config.UseLatestCSharp);

        var sampleFromMapper = fromMappers.FirstOrDefault();
        var sampleToMapper = toMappers.FirstOrDefault();

        var (mapperNs, modelPath) = sampleFromMapper != default
            ? Config.GetMapperLocation(sampleFromMapper, tag)
            : Config.GetMapperLocation(sampleToMapper, tag);

        var ns = Config.GetNamespace(mapperNs, modelPath, tag);

        var usings = fromMappers.SelectMany(m => m.Mapper.Params.Select(p => p.Class).Concat(new[] { m.Classe }))
            .Concat(toMappers.SelectMany(m => new[] { m.Classe, m.Mapper.Class }))
            .Select(c => Config.GetNamespace(c, GetClassTags(c).Contains(tag) ? tag : GetClassTags(c).Intersect(Config.Tags).First()))
            .Where(@using => !ns.Contains(@using))
            .Distinct()
            .ToArray();

        if (usings.Any())
        {
            w.WriteUsings(usings);
            w.WriteLine();
        }

        w.WriteNamespace(ns);
        w.WriteSummary(1, $"Mappers pour le module '{mapperNs.Module}'.");
        w.WriteLine(1, $"public static class {Config.GetMapperName(mapperNs, modelPath)}");
        w.WriteLine(1, "{");

        foreach (var fromMapper in fromMappers)
        {
            var (classe, mapper) = fromMapper;

            w.WriteSummary(2, $"Crée une nouvelle instance de '{classe}'{(mapper.Comment != null ? $"\n{mapper.Comment}" : string.Empty)}");
            foreach (var param in mapper.Params)
            {
                if (param.Comment != null)
                {
                    w.WriteParam(param.Name, param.Comment);
                }
                else
                {
                    w.WriteParam(param.Name, $"Instance de '{param.Class}'");
                }
            }

            w.WriteReturns(2, $"Une nouvelle instance de '{classe}'");

            if (classe.Abstract)
            {
                w.Write(2, $"public static T Create{classe}<T>");
            }
            else
            {
                w.Write(2, $"public static {classe} Create{classe}");
            }

            w.WriteLine($"({string.Join(", ", mapper.Params.Select(p => $"{(p.Class.Abstract ? "I" : string.Empty)}{p.Class} {p.Name}{(!p.Required ? " = null" : string.Empty)}"))})");

            if (classe.Abstract)
            {
                w.WriteLine(3, $"where T : I{classe}");
            }

            w.WriteLine(2, "{");

            foreach (var param in mapper.Params.Where(p => p.Required))
            {
                w.WriteLine(3, $"if ({param.Name} is null)");
                w.WriteLine(3, "{");
                w.WriteLine(4, $"throw new ArgumentNullException(nameof({param.Name}));");
                w.WriteLine(3, "}");
                w.WriteLine();
            }

            if (classe.Abstract)
            {
                w.WriteLine(3, $"return (T)T.Create(");
            }
            else
            {
                w.WriteLine(3, $"return new {classe}");
                w.WriteLine(3, "{");
            }

            if (mapper.ParentMapper != null)
            {
                foreach (var param in mapper.ParentMapper.Params)
                {
                    var mappings = param.Mappings.ToList();
                    foreach (var mapping in mappings)
                    {
                        w.Write(4, $"{mapping.Key.NamePascal} = ");

                        if (mapping.Value == null)
                        {
                            w.Write(param.Name);
                        }
                        else
                        {
                            var value = $"{mapper.Params[mapper.ParentMapper.Params.IndexOf(param)].Name}{(!param.Required && mapping.Key is not CompositionProperty ? "?" : string.Empty)}.{mapping.Value.NamePascal}";

                            if (mapping.Key is CompositionProperty cp)
                            {
                                w.Write($"{(!param.Required ? $"{param.Name} is null ? null : " : string.Empty)}new() {{ {cp.Composition.PrimaryKey.SingleOrDefault()?.NamePascal} = ");
                            }
                            else
                            {
                                value = Config.GetConvertedValue(value, mapping.Value?.Domain, (mapping.Key as IFieldProperty)?.Domain);
                            }

                            w.Write(value);

                            if (mapping.Key is CompositionProperty)
                            {
                                w.Write("}");
                            }
                        }

                        if (mapper.Params.IndexOf(param) < mapper.Params.Count - 1 || mappings.IndexOf(mapping) < mappings.Count - 1)
                        {
                            w.Write(",");
                        }

                        w.WriteLine();
                    }
                }
            }

            foreach (var param in mapper.Params)
            {
                var mappings = param.Mappings.ToList();
                foreach (var mapping in mappings)
                {
                    if (classe.Abstract)
                    {
                        w.Write(4, $"{mapping.Key.NameCamel}: ");
                    }
                    else
                    {
                        w.Write(4, $"{mapping.Key.NamePascal} = ");
                    }

                    if (mapping.Value == null)
                    {
                        w.Write(param.Name);
                    }
                    else
                    {
                        var value = $"{param.Name}{(!param.Required && mapping.Key is not CompositionProperty ? "?" : string.Empty)}.{mapping.Value.NamePascal}";

                        if (mapping.Key is CompositionProperty cp)
                        {
                            w.Write($"{(!param.Required ? $"{param.Name} is null ? null : " : string.Empty)}new() {{ {cp.Composition.PrimaryKey.SingleOrDefault()?.NamePascal} = ");
                        }
                        else
                        {
                            value = Config.GetConvertedValue(value, mapping.Value?.Domain, (mapping.Key as IFieldProperty)?.Domain);
                        }

                        w.Write(value);

                        if (mapping.Key is CompositionProperty)
                        {
                            w.Write(" }");
                        }
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
            }

            if (!classe.Abstract)
            {
                w.WriteLine(3, "};");
            }

            w.WriteLine(2, "}");

            if (toMappers.Any() || fromMappers.IndexOf(fromMapper) < fromMappers.Count - 1)
            {
                w.WriteLine();
            }
        }

        foreach (var toMapper in toMappers)
        {
            var (classe, mapper) = toMapper;

            w.WriteSummary(2, $"Mappe '{classe}' vers '{mapper.Class}'{(mapper.Comment != null ? $"\n{mapper.Comment}" : string.Empty)}");
            w.WriteParam("source", $"Instance de '{classe}'");
            if (!mapper.Class.Abstract)
            {
                w.WriteParam("dest", $"Instance pré-existante de '{mapper.Class}'. Une nouvelle instance sera créée si non spécifié.");
            }

            w.WriteReturns(2, $"Une instance de '{mapper.Class}'");

            if (mapper.Class.Abstract)
            {
                w.WriteLine(2, $"public static T {mapper.Name}<T>(this {classe} source)");
                w.WriteLine(3, $"where T : I{mapper.Class}");
            }
            else
            {
                w.WriteLine(2, $"public static {mapper.Class} {mapper.Name}(this {(classe.Abstract ? "I" : string.Empty)}{classe} source, {mapper.Class} dest = null)");
            }

            w.WriteLine(2, "{");

            if (mapper.Class.Abstract)
            {
                w.WriteLine(3, $"return (T)T.Create(");
            }
            else
            {
                w.WriteLine(3, $"dest ??= new {mapper.Class}();");
            }

            static string GetSourceMapping(IProperty property)
            {
                if (property is CompositionProperty cp)
                {
                    return $"{cp.NamePascal}?.{cp.Composition.PrimaryKey.SingleOrDefault()?.NamePascal}";
                }
                else
                {
                    return property.NamePascal;
                }
            }

            var mappings = (mapper.ParentMapper?.Mappings ?? new Dictionary<IProperty, IFieldProperty?>()).Concat(mapper.Mappings).ToList();
            foreach (var mapping in mappings)
            {
                var value = Config.GetConvertedValue($"source.{GetSourceMapping(mapping.Key)}", (mapping.Key as IFieldProperty)?.Domain, mapping.Value?.Domain);

                if (mapper.Class.Abstract)
                {
                    w.Write(4, $"{mapping.Value?.NameCamel}: {value}");

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
                    w.WriteLine(3, $"dest.{mapping.Value?.NamePascal} = {value};");
                }
            }

            if (!mapper.Class.Abstract)
            {
                w.WriteLine(3, "return dest;");
            }

            w.WriteLine(2, "}");

            if (toMappers.IndexOf(toMapper) < toMappers.Count - 1)
            {
                w.WriteLine();
            }
        }

        w.WriteLine(1, "}");
        w.WriteNamespaceEnd();
    }

    protected override bool IsPersistent(Class classe)
    {
        return GetClassTags(classe).Intersect(Config.MapperTagsOverrides).Any() || classe.IsPersistent;
    }
}