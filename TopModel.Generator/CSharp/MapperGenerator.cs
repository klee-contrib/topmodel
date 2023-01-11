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

    public override IEnumerable<string> GeneratedFiles => Mappers
        .Select(g => _config.GetMapperFilePath(Classes.OrderBy(c => c.Reference ? 1 : 0).FirstOrDefault(c => c.Namespace.Module == g.Module && c.IsPersistent == g.IsPersistant)))
        .Where(f => f != null)!;

    private IDictionary<(string Module, bool IsPersistant), IEnumerable<(Class Classe, FromMapper Mapper)>> FromMappers => Classes
        .SelectMany(classe => classe.FromMappers.Select(mapper => (classe, mapper)))
        .Where(mapper => mapper.mapper.Params.All(p => Classes.Contains(p.Class)))
        .Select(c => (c.classe.Namespace.Module, isPersistant: c.classe.IsPersistent || c.mapper.Params.Any(p => p.Class.IsPersistent), c.classe, c.mapper))
        .GroupBy(c => (c.Module, c.isPersistant))
        .ToDictionary(g => g.Key, g => g.Select(c => (c.classe, c.mapper)));

    private IDictionary<(string Module, bool IsPersistant), IEnumerable<(Class Classe, ClassMappings Mapper)>> ToMappers => Classes
        .SelectMany(classe => classe.ToMappers.Select(mapper => (classe, mapper))
        .Where(mapper => Classes.Contains(mapper.mapper.Class)))
        .Select(c => (c.classe.Namespace.Module, isPersistant: c.classe.IsPersistent || c.mapper.Class.IsPersistent, c.classe, c.mapper))
        .GroupBy(c => (c.Module, c.isPersistant))
        .ToDictionary(g => g.Key, g => g.Select(c => (c.classe, c.mapper)));

    private IEnumerable<(string Module, bool IsPersistant)> Mappers => FromMappers.Select(c => c.Key).Concat(ToMappers.Select(c => c.Key));

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var (module, isPersistant) in Mappers)
        {
            Generate(module, isPersistant);
        }
    }

    /// <summary>
    /// Génère les mappers.
    /// </summary>
    /// <param name="module">Module.</param>
    /// <param name="isPersistant">Mappers à générer avec les classes persistées (ou non).</param>
    private void Generate(string module, bool isPersistant)
    {
        var sampleClass = Classes.OrderBy(c => c.Reference ? 1 : 0).First(c => c.Namespace.Module == module && c.IsPersistent == isPersistant);
        using var w = new CSharpWriter(_config.GetMapperFilePath(sampleClass)!, _logger, _config.UseLatestCSharp);

        var ns = _config.GetNamespace(sampleClass);

        FromMappers.TryGetValue((module, isPersistant), out var fm);
        ToMappers.TryGetValue((module, isPersistant), out var tm);

        var fromMappers = (fm ?? Array.Empty<(Class, FromMapper)>())
            .OrderBy(m => $"{m.Classe.Name} {string.Join(',', m.Mapper.Params.Select(p => p.Name))}", StringComparer.Ordinal)
            .ToList();
        var toMappers = (tm ?? Array.Empty<(Class, ClassMappings)>())
            .OrderBy(m => $"{m.Mapper.Name} {m.Classe.Name}", StringComparer.Ordinal)
            .ToList();

        var usings = fromMappers.SelectMany(m => m.Mapper.Params.Select(p => p.Class).Concat(new[] { m.Classe }))
            .Concat(toMappers.SelectMany(m => new[] { m.Classe, m.Mapper.Class }))
            .Where(c => Classes.Contains(c))
            .Select(_config.GetNamespace)
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
        w.WriteLine(1, $"public static class {sampleClass.Namespace.Module}{(sampleClass.IsPersistent ? string.Empty : "DTO")}Mappers");
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