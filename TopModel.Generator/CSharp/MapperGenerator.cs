using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.CSharp;

public class MapperGenerator : GeneratorBase
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

    public override IEnumerable<string> GeneratedFiles => GetMapperModules(Files.Values)
        .Select(module => _config.GetMapperFilePath(module))
        .Where(f => f != null)!;

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var classes in GetMapperModules(files))
        {
            Generate(classes, Classes.ToList());
        }
    }

    private IEnumerable<IEnumerable<Class>> GetMapperModules(IEnumerable<ModelFile> files)
    {
        return files
            .SelectMany(f => f.Classes.Select(c => c.Namespace.Module))
            .Distinct()
            .Select(
                module => Classes
                    .Where(c =>
                        c.Namespace.Module == module
                        && c.FromMappers.SelectMany(m => m.Params).Concat(c.ToMappers)
                            .Any(m => Files.SelectMany(f => f.Value.Classes).Contains(c))));
    }

    /// <summary>
    /// Génère les mappers pour un namespace.
    /// </summary>
    /// <param name="classes">Classes.</param>
    /// <param name="availableClasses">Classes disponibles dans le générateur.</param>
    private void Generate(IEnumerable<Class> classes, List<Class> availableClasses)
    {
        if (!classes.Any())
        {
            return;
        }

        var firstClass = classes.First();

        using var w = new CSharpWriter(_config.GetMapperFilePath(classes)!, _logger, _config.UseLatestCSharp);

        var ns = $"{_config.GetNamespace(firstClass)}.Mappers";

        var usings = classes.SelectMany(classe =>
            classe.FromMappers.SelectMany(m => m.Params).Concat(classe.ToMappers)
                .Select(m => m.Class)
                .Where(c => availableClasses.Contains(c))
                .Select(_config.GetNamespace)
                .Where(@using => !ns.Contains(@using))
                .Distinct())
            .ToArray();

        if (usings.Any())
        {
            w.WriteUsings(usings);
            w.WriteLine();
        }

        w.WriteNamespace(ns);
        w.WriteSummary(1, $"Mappers pour le module '{firstClass.Namespace.Module}'.");
        w.WriteLine(1, $"public static class {firstClass.Namespace.Module}Mappers");
        w.WriteLine(1, "{");

        var classList = classes.OrderBy(c => c.Name, StringComparer.Ordinal).ToList();

        var fromMappers = classes
            .SelectMany(classe => classe.FromMappers.Where(c => c.Params.All(p => availableClasses.Contains(p.Class))).Select(m => (classe, m)))
            .OrderBy(m => m.classe.Name, StringComparer.Ordinal)
            .ToList();
        var toMappers = classes
            .SelectMany(classe => classe.ToMappers.Where(p => availableClasses.Contains(p.Class)).Select(m => (classe, m)))
            .OrderBy(m => m.m.Name, StringComparer.Ordinal)
            .ToList();

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

            w.WriteLine(2, $"public static {classe} Create{classe}({string.Join(", ", mapper.Params.Select(p => $"{p.Class} {p.Name}{(!p.Required ? " = null" : string.Empty)}"))})");
            w.WriteLine(2, "{");

            foreach (var param in mapper.Params.Where(p => p.Required))
            {
                w.WriteLine(3, $"if ({param.Name} is null)");
                w.WriteLine(3, "{");
                w.WriteLine(4, $"throw new ArgumentNullException(nameof({param.Name}));");
                w.WriteLine(3, "}");
                w.WriteLine();
            }

            w.WriteLine(3, $"return new {classe}");
            w.WriteLine(3, "{");

            if (mapper.ParentMapper != null)
            {
                foreach (var param in mapper.ParentMapper.Params)
                {
                    var mappings = param.Mappings.ToList();
                    foreach (var mapping in mappings)
                    {
                        w.Write(4, $"{mapping.Key.Name} = ");

                        if (mapping.Value == null)
                        {
                            w.Write(param.Name);
                        }
                        else
                        {
                            if (mapping.Key is CompositionProperty cp)
                            {
                                w.Write($"{(!param.Required ? $"{param.Name} is null ? null : " : string.Empty)}new() {{ {cp.Composition.PrimaryKey?.Name} = ");
                            }

                            w.Write($"{mapper.Params[mapper.ParentMapper.Params.IndexOf(param)].Name}{(!param.Required && mapping.Key is not CompositionProperty ? "?" : string.Empty)}.{mapping.Value.Name}");

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
                    w.Write(4, $"{mapping.Key.Name} = ");

                    if (mapping.Value == null)
                    {
                        w.Write(param.Name);
                    }
                    else
                    {
                        if (mapping.Key is CompositionProperty cp)
                        {
                            w.Write($"{(!param.Required ? $"{param.Name} is null ? null : " : string.Empty)}new() {{ {cp.Composition.PrimaryKey?.Name} = ");
                        }

                        w.Write($"{param.Name}{(!param.Required && mapping.Key is not CompositionProperty ? "?" : string.Empty)}.{mapping.Value.Name}");

                        if (mapping.Key is CompositionProperty)
                        {
                            w.Write(" }");
                        }
                    }

                    if (mapper.Params.IndexOf(param) < mapper.Params.Count - 1 || mappings.IndexOf(mapping) < mappings.Count - 1)
                    {
                        w.Write(",");
                    }

                    w.WriteLine();
                }
            }

            w.WriteLine(3, "};");
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
            w.WriteParam("dest", $"Instance pré-existante de '{mapper.Class}'. Une nouvelle instance sera créée si non spécifié.");
            w.WriteReturns(2, $"Une instance de '{mapper.Class}'");

            w.WriteLine(2, $"public static {mapper.Class} {mapper.Name}(this {classe} source, {mapper.Class} dest = null)");
            w.WriteLine(2, "{");
            w.WriteLine(3, $"dest ??= new {mapper.Class}();");

            static string GetSourceMapping(IProperty property)
            {
                if (property is CompositionProperty cp)
                {
                    return $"{cp.Name}?.{cp.Composition.PrimaryKey?.Name}";
                }
                else
                {
                    return property.Name;
                }
            }

            if (mapper.ParentMapper != null)
            {
                foreach (var mapping in mapper.ParentMapper.Mappings)
                {
                    w.WriteLine(3, $"dest.{mapping.Value?.Name} = source.{GetSourceMapping(mapping.Key)};");
                }
            }

            foreach (var mapping in mapper.Mappings)
            {
                w.WriteLine(3, $"dest.{mapping.Value?.Name} = source.{GetSourceMapping(mapping.Key)};");
            }

            w.WriteLine(3, "return dest;");

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