using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Generator.Jpa;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JpaMapperGenerator : GeneratorBase
{
    private readonly JpaConfig _config;
    private readonly ILogger<JpaMapperGenerator> _logger;

    public JpaMapperGenerator(ILogger<JpaMapperGenerator> logger, JpaConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "JpaMapperGenerator";

    public override IEnumerable<string> GeneratedFiles => Mappers
        .Select(g => _config.GetMapperFilePath(Classes.OrderBy(c => c.Reference ? 1 : 0).FirstOrDefault(c => c.Namespace.Module == g.Module && c.IsPersistent == g.IsPersistant)))
        .Where(f => f != null)!;

    private IDictionary<(string Module, bool IsPersistant), IEnumerable<(Class Classe, FromMapper Mapper)>> FromMappers => Classes
        .SelectMany(classe => classe.FromMappers.Select(mapper => (classe, mapper)))
        .Where(mapper => mapper.mapper.Params.All(p => Classes.Contains(p.Class)))
        .Select(c => (c.classe.Namespace.Module, isPersistant: c.classe.IsPersistent || c.mapper.Params.Any(p => p.Class.IsPersistent), c.classe, c.mapper))
        .GroupBy(c => (c.Module.Split('.').First(), c.isPersistant))
        .ToDictionary(g => g.Key, g => g.Select(c => (c.classe, c.mapper)));

    private IDictionary<(string Module, bool IsPersistant), IEnumerable<(Class Classe, ClassMappings Mapper)>> ToMappers => Classes
        .SelectMany(classe => classe.ToMappers.Select(mapper => (classe, mapper))
        .Where(mapper => Classes.Contains(mapper.mapper.Class)))
        .Select(c => (c.classe.Namespace.Module, isPersistant: c.classe.IsPersistent || c.mapper.Class.IsPersistent, c.classe, c.mapper))
        .GroupBy(c => (c.Module.Split('.').First(), c.isPersistant))
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
        var package = _config.MappersPackageName;
        var destFolder = Path.Combine(_config.OutputDirectory, string.Join('/', package.Split('.')));
        var dirInfo = Directory.CreateDirectory(destFolder);
        using var fw = new JavaWriter(_config.GetMapperFilePath(sampleClass)!, _logger, package, null);


        FromMappers.TryGetValue((module, isPersistant), out var fm);
        ToMappers.TryGetValue((module, isPersistant), out var tm);

        var fromMappers = (fm ?? Array.Empty<(Class, FromMapper)>())
            .OrderBy(m => $"{m.Classe.Name} {string.Join(',', m.Mapper.Params.Select(p => p.Name))}", StringComparer.Ordinal)
            .ToList();
        var toMappers = (tm ?? Array.Empty<(Class, ClassMappings)>())
            .OrderBy(m => $"{m.Mapper.Name} {m.Classe.Name}", StringComparer.Ordinal)
            .ToList();

        var imports = fromMappers.SelectMany(m => m.Mapper.Params.Select(p => p.Class).Concat(new[] { m.Classe }))
            .Concat(toMappers.SelectMany(m => new[] { m.Classe, m.Mapper.Class }))
            .Where(c => Classes.Contains(c))
            .Select(c => c.GetImport(_config))
            .Distinct()
            .ToArray();

        if (imports.Any())
        {
            fw.AddImports(imports);
            fw.WriteLine();
        }

        fw.WriteLine($@"public static class {_config.GetMapperClassName(sampleClass)} {{");

        foreach (var fromMapper in fromMappers)
        {
            WriteFromMapper(fromMapper.Classe, fromMapper.Mapper, fw);
        }

        foreach (var toMapper in toMappers)
        {
            WriteToMapper(toMapper.Classe, toMapper.Mapper, fw);
        }

        fw.WriteLine("}");
    }

    private void WriteToMapper(Class classe, ClassMappings mapper, JavaWriter fw)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, $"Mappe '{classe}' vers '{mapper.Class}'");
        if (mapper.Comment != null)
        {
            fw.WriteLine(1, $" * {mapper.Comment}");
        }

        fw.WriteParam("source", $"Instance de '{classe}'");
        fw.WriteParam("target", $"Instance pré-existante de '{mapper.Class}'. Une nouvelle instance sera créée si non spécifié.");

        fw.WriteDocEnd(1);

        fw.WriteLine(1, $"public void {mapper.Name.ToFirstLower()}({classe} source, {mapper.Class} target) {{");
        fw.WriteLine(2, "if (source == null) {");
        fw.WriteLine(3, $"throw new IllegalArgumentException(\"source cannot be null\");");
        fw.WriteLine(2, "}");
        fw.WriteLine();
        fw.WriteLine(2, "if (target == null) {");
        fw.WriteLine(3, $"throw new IllegalArgumentException(\"target cannot be null\");");
        fw.WriteLine(2, "}");
        fw.WriteLine();
        if (mapper.ParentMapper != null)
        {
            fw.AddImport(_config.GetMapperImport(mapper.ParentMapper.Class)!);
            fw.WriteLine(2, $"{_config.GetMapperClassName(mapper.ParentMapper.Class)}{mapper.ParentMapper.Name}.{mapper.ParentMapper.Name.ToFirstLower()}(source, target);");
        }

        foreach (var mapping in mapper.Mappings)
        {
            var getterPrefix = mapping.Value!.GetJavaType().ToUpper() == "BOOLEAN" ? "is" : "get";
            if (mapping.Value is AssociationProperty ap)
            {
                if (ap.Property.IsEnum())
                {
                    var isMultiple = ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany;
                    if (isMultiple)
                    {
                        var getter = $@"source.{getterPrefix}{mapping.Key.GetJavaName().ToFirstUpper()}(){(!mapping.Key.Class.IsPersistent ? $".stream().map({ap.Association.PrimaryKey.Single().GetJavaType()}::getEntity).collect(Collectors.toList())" : string.Empty)}";
                        fw.WriteLine(2, $"target.set{mapping.Value.GetJavaName().ToFirstUpper()}({getter});");
                        fw.AddImport("java.util.stream.Collectors");
                    }
                    else
                    {
                        fw.WriteLine(2, $"if (source.{getterPrefix}{mapping.Key.GetJavaName().ToFirstUpper()}() != null) {{");
                        fw.WriteLine(3, $"target.set{mapping.Value.GetJavaName().ToFirstUpper()}(source.{getterPrefix}{mapping.Key.GetJavaName().ToFirstUpper()}(){(!mapping.Key.Class.IsPersistent ? ".getEntity()" : string.Empty)});");
                        fw.WriteLine(2, $"}}");
                    }
                }
                else if (mapping.Value.Class.IsPersistent && mapping.Key.Class.IsPersistent)
                {
                    fw.WriteLine(2, $"target.set{mapping.Value.GetJavaName().ToFirstUpper()}(source.{getterPrefix}{mapping.Key.GetJavaName().ToFirstUpper()}());");
                }
                else if (mapping.Key is CompositionProperty cp)
                {
                    if (cp.Composition.ToMappers.Any(t => t.Class == ap.Association))
                    {
                        var cpMapper = cp.Composition.ToMappers.Find(t => t.Class == ap.Association)!;
                        fw.WriteLine(2, $@"if (source.get{cp.GetJavaName().ToFirstUpper()}() != null) {{");
                        fw.WriteLine(3, $@"target.set{mapping.Value.GetJavaName().ToFirstUpper()}(source.{getterPrefix}{cp.GetJavaName().ToFirstUpper()}().{cpMapper.Name.ToFirstLower()}(target.get{ap.GetJavaName().ToFirstUpper()}()));");
                        fw.WriteLine(2, $@"}}");
                        fw.WriteLine();
                    }
                    else
                    {
                        throw new ModelException(classe, $"La propriété {mapping.Key.Name} ne peut pas être mappée avec la propriété {mapping.Value.Name} car il n'existe pas de mapper {cp.Composition.Name} -> {ap.Association.Name}");
                    }
                }
            }
            else
            {
                if (mapping.Key is IFieldProperty ifpTo && mapping.Value is IFieldProperty ifpFrom && ifpFrom.Domain != ifpTo.Domain)
                {
                    var converter = ifpFrom.Domain.ConvertersFrom.FirstOrDefault(c => c.To.Any(t => t == ifpTo.Domain));
                    string conversion = $@"source.{getterPrefix}{mapping.Key.GetJavaName().ToFirstUpper()}()";
                    if (converter != null && converter.Java?.Text != null)
                    {
                        var convert = converter.Java.Text;
                        conversion = convert.Replace("{value}", conversion)
                            .ParseTemplate(ifpFrom.Domain, "java", "from.")
                            .ParseTemplate(ifpTo.Domain, "java", "to.");
                    }

                    fw.WriteLine(2, $"source.{mapping.Key.GetJavaName()} = {conversion};");
                }
                else
                {
                    fw.WriteLine(2, $"target.set{mapping.Value!.GetJavaName().ToFirstUpper()}(source.{getterPrefix}{mapping.Key.GetJavaName().ToFirstUpper()}());");
                }
            }
        }

        fw.WriteLine();
        fw.WriteLine(2, "return target;");

        fw.WriteLine(1, "}");
    }

    private void WriteFromMapper(Class classe, FromMapper mapper, JavaWriter fw)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, $"Map les champs des classes passées en paramètre dans l'objet target'");
        fw.WriteParam("target", $"Instance de '{classe}'");
        foreach (var param in mapper.Params)
        {
            if (param.Comment != null)
            {
                fw.WriteParam(param.Name.ToFirstLower(), param.Comment);
            }
            else
            {
                fw.WriteParam(param.Name.ToFirstLower(), $"Instance de '{param.Class}'");
            }
        }

        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"public static void map({classe} target, {string.Join(", ", mapper.Params.Select(p => $"{p.Class} {p.Name.ToFirstLower()}"))}) {{");
        fw.WriteLine(2, "if (target == null) {");
        fw.WriteLine(3, $"throw new IllegalArgumentException(\"target cannot be null\");");
        fw.WriteLine(2, "}");
        fw.WriteLine();
        if (classe.Extends != null)
        {
            if (mapper.ParentMapper != null)
            {
                fw.WriteLine(2, $"{_config.GetMapperClassName(classe.Extends)}.map(target, {string.Join(", ", mapper.Params.Take(mapper.ParentMapper.Params.Count).Select(p => p.Name))});");
            }
        }

        foreach (var param in mapper.Params.Where(p => p.Mappings.Count > 0))
        {
            fw.WriteLine(2, $"if ({param.Name.ToFirstLower()} != null) {{");
            var mappings = param.Mappings.ToList();
            foreach (var mapping in mappings)
            {
                var getterPrefix = mapping.Value!.GetJavaType().ToUpper() == "BOOLEAN" ? "is" : "get";
                if (mapping.Value is AssociationProperty ap)
                {
                    if (!classe.IsPersistent)
                    {
                        if (mapping.Key is IFieldProperty)
                        {
                            fw.WriteLine(3, $"if ({param.Name.ToFirstLower()}.{getterPrefix}{mapping.Value.GetJavaName().ToFirstUpper()}() != null) {{");
                            if (ap.Type == AssociationType.OneToOne || ap.Type == AssociationType.ManyToOne)
                            {
                                fw.WriteLine(4, $"target.set{mapping.Key.GetJavaName().ToPascalCase()}({param.Name.ToFirstLower()}.{getterPrefix}{ap.GetJavaName().ToFirstUpper()}().{getterPrefix}{ap.Property.Name.ToFirstUpper()}());");
                            }
                            else
                            {
                                fw.WriteLine(4, $"target.set{mapping.Key.GetJavaName().ToPascalCase()}({param.Name.ToFirstLower()}.{getterPrefix}{ap.GetJavaName().ToFirstUpper()}().stream().filter(t -> t != null).map({ap.GetJavaName().ToFirstLower()} -> {ap.GetJavaName().ToFirstLower()}.{getterPrefix}{ap.Property.Name.ToFirstUpper()}()).collect(Collectors.toList()));");
                                fw.AddImport("java.util.stream.Collectors");
                            }

                            fw.WriteLine(3, "}");

                            if (mappings.IndexOf(mapping) < mappings.Count - 1)
                            {
                                fw.WriteLine();
                            }
                        }
                        else if (mapping.Key is CompositionProperty cp)
                        {
                            if (mapping.Value == null)
                            {
                                fw.WriteLine(3, $"target.{mapping.Key.GetJavaName()} = {param.Name};");
                            }
                            else if (cp.Composition.FromMappers.Any(f => f.Params.Count == 1 && f.Params.First().Class == ap.Association))
                            {
                                var cpMapper = cp.Composition.FromMappers.Find(f => f.Params.Count == 1 && f.Params.First().Class == ap.Association);
                                var getter = $"{param.Name.ToFirstLower()}.{getterPrefix}{ap.GetJavaName().ToFirstUpper()}()";
                                if (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany)
                                {
                                    fw.WriteLine(3, $"target.{mapping.Key.GetJavaName()} = {getter} == null ? null : {getter}.stream().map({cp.Composition.Name.ToFirstUpper()}::new).collect(Collectors.toList());");
                                    fw.AddImport("java.util.stream.Collectors");
                                }
                                else
                                {
                                    fw.WriteLine(3, $"target.{mapping.Key.GetJavaName()} = {getter} == null ? null : new {cp.Composition.Name.ToFirstUpper()}({getter});");
                                }
                            }
                            else
                            {
                                throw new ModelException(classe, $"La propriété {mapping.Key.Name} ne peut pas être mappée avec la propriété {mapping.Value.Name} car il n'existe pas de mapper {ap.Association.Name} -> {cp.Composition.Name}");
                            }
                        }
                    }
                    else
                    {
                        fw.WriteLine(3, $"target.{mapping.Key.GetJavaName()} = {param.Name.ToFirstLower()}.{getterPrefix}{mapping.Value.GetJavaName().ToFirstUpper()}();");
                    }
                }
                else
                {
                    if (mapping.Key is IFieldProperty ifpFrom && mapping.Value is IFieldProperty ifpTo && ifpFrom.Domain != ifpTo.Domain)
                    {
                        var converter = ifpFrom.Domain.ConvertersFrom.FirstOrDefault(c => c.To.Any(t => t == ifpTo.Domain));
                        string conversion = $@"{param.Name.ToFirstLower()}.{getterPrefix}{mapping.Value!.Name.ToFirstUpper()}()";
                        if (converter != null && converter.Java?.Text != null)
                        {
                            var convert = converter.Java.Text;
                            conversion = convert.Replace("{value}", conversion)
                                .ParseTemplate(ifpFrom.Domain, "java", "from.")
                                .ParseTemplate(ifpTo.Domain, "java", "to.");
                        }

                        fw.WriteLine(3, $"target.{mapping.Key.GetJavaName()} = {conversion};");
                    }
                    else
                    {
                        fw.WriteLine(3, $"target.{mapping.Key.GetJavaName()} = {param.Name.ToFirstLower()}.{getterPrefix}{mapping.Value!.Name.ToFirstUpper()}();");
                    }
                }
            }

            if (param.Required)
            {
                fw.WriteLine(2, "} else {");
                fw.WriteLine(3, $"throw new IllegalArgumentException(\"{param.Name} cannot be null\");");
            }

            fw.WriteLine(2, "}");

            if (mapper.Params.IndexOf(param) < mapper.Params.Count - 1)
            {
                fw.WriteLine();
            }
        }

        fw.WriteLine(1, "}");
    }
}