using Microsoft.Extensions.Logging;
using TopModel.Core;

namespace TopModel.Generator.CSharp;

public class MapperGenerator : MapperGeneratorBase
{
    private readonly CSharpConfig _config;
    private readonly ILogger<MapperGenerator> _logger;

    public MapperGenerator(ILogger<MapperGenerator> logger, CSharpConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "CSharpMapperGen";

    protected override string GetFileName(Class classe, bool isPersistant, string tag)
    {
        return _config.GetMapperFilePath(classe, isPersistant, tag);
    }

    protected override void HandleFile(bool isPersistant, string fileName, string tag, IEnumerable<Class> classes)
    {
        var sampleClass = classes.First();
        using var w = new CSharpWriter(fileName, _logger, _config.UseLatestCSharp);

        var ns = _config.GetNamespace(sampleClass, tag, isPersistant);

        var fm = FromMappers.Where(fm => fm.IsPersistant == isPersistant && classes.Contains(fm.Classe));
        var tm = ToMappers.Where(fm => fm.IsPersistant == isPersistant && classes.Contains(fm.Classe));

        var fromMappers = (fm ?? Array.Empty<(Class, FromMapper, bool)>())
            .OrderBy(m => $"{m.Classe.NamePascal} {string.Join(',', m.Mapper.Params.Select(p => p.Name))}", StringComparer.Ordinal)
            .ToList();
        var toMappers = (tm ?? Array.Empty<(Class, ClassMappings, bool)>())
            .OrderBy(m => $"{m.Mapper.Name} {m.Classe.NamePascal}", StringComparer.Ordinal)
            .ToList();

        var usings = fromMappers.SelectMany(m => m.Mapper.Params.Select(p => p.Class).Concat(new[] { m.Classe }))
            .Concat(toMappers.SelectMany(m => new[] { m.Classe, m.Mapper.Class }))
            .Where(c => Classes.Contains(c))
            .Select(c => _config.GetNamespace(c, tag))
            .Where(@using => !ns.Contains(@using))
            .Distinct()
            .ToArray();

        if (usings.Any())
        {
            w.WriteUsings(usings);
            w.WriteLine();
        }

        w.WriteNamespace(ns);
        w.WriteSummary(1, $"Mappers pour le module '{sampleClass.Namespace.Module}'.");
        w.WriteLine(1, $"public static class {sampleClass.GetMapperName(isPersistant)}");
        w.WriteLine(1, "{");

        foreach (var fromMapper in fromMappers)
        {
            var (classe, mapper, _) = fromMapper;

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
                                var fromDomain = ((IFieldProperty)mapping.Key).Domain;
                                var toDomain = mapping.Value.Domain;
                                if (fromDomain != toDomain)
                                {
                                    var converter = fromDomain.ConvertersFrom.FirstOrDefault(c => c.From.Contains(fromDomain) && c.To.Contains(toDomain));
                                    if (converter?.CSharp?.Text != null)
                                    {
                                        value = converter.CSharp.Text.Replace("{value}", value)
                                            .ParseTemplate(fromDomain, "csharp", "from.")
                                            .ParseTemplate(toDomain, "csharp", "to.");
                                    }
                                }
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
                            var fromDomain = ((IFieldProperty)mapping.Key).Domain;
                            var toDomain = mapping.Value.Domain;
                            if (fromDomain != toDomain)
                            {
                                var converter = fromDomain.ConvertersFrom.FirstOrDefault(c => c.From.Contains(fromDomain) && c.To.Contains(toDomain));
                                if (converter?.CSharp?.Text != null)
                                {
                                    value = converter.CSharp.Text.Replace("{value}", value)
                                        .ParseTemplate(fromDomain, "csharp", "from.")
                                        .ParseTemplate(toDomain, "csharp", "to.");
                                }
                            }
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
            var (classe, mapper, _) = toMapper;

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
                var value = $"source.{GetSourceMapping(mapping.Key)}";

                var fromDomain = mapping.Value?.Domain;
                var toDomain = (mapping.Key as IFieldProperty)?.Domain;
                if (fromDomain != null && toDomain != null && fromDomain != toDomain)
                {
                    var converter = fromDomain.ConvertersFrom.FirstOrDefault(c => c.From.Contains(fromDomain) && c.To.Contains(toDomain));
                    if (converter?.CSharp?.Text != null)
                    {
                        value = converter.CSharp.Text.Replace("{value}", value)
                            .ParseTemplate(fromDomain, "csharp", "from.")
                            .ParseTemplate(toDomain, "csharp", "to.");
                    }
                }

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
}