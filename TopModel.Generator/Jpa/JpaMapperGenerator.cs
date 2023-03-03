using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JpaMapperGenerator : MapperGeneratorBase
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

    protected override string GetFileName(Class classe, bool isPersistant, string tag)
    {
        return _config.GetMapperFilePath(classe, isPersistant, tag);
    }

    protected override void HandleFile(bool isPersistant, string fileName, string tag, IEnumerable<Class> classes)
    {
        var sampleClass = classes.First();
        var package = _config.GetMapperPackage(sampleClass, isPersistant, tag);
        using var fw = new JavaWriter(fileName, _logger, package, null);

        var fm = FromMappers.Where(fm => fm.IsPersistant == isPersistant && classes.Contains(fm.Classe));
        var tm = ToMappers.Where(fm => fm.IsPersistant == isPersistant && classes.Contains(fm.Classe));

        var fromMappers = (fm ?? Array.Empty<(Class, FromMapper, bool)>())
            .OrderBy(m => $"{m.Classe.NamePascal} {string.Join(',', m.Mapper.Params.Select(p => p.Name))}", StringComparer.Ordinal)
            .ToList();
        var toMappers = (tm ?? Array.Empty<(Class, ClassMappings, bool)>())
            .OrderBy(m => $"{m.Mapper.Name} {m.Classe.NamePascal}", StringComparer.Ordinal)
            .ToList();

        var imports = fromMappers.SelectMany(m => m.Mapper.Params.Select(p => p.Class).Concat(new[] { m.Classe }))
            .Concat(toMappers.SelectMany(m => new[] { m.Classe, m.Mapper.Class }))
            .Where(c => Classes.Contains(c))
            .Select(c => c.GetImport(_config, tag))
            .Distinct()
            .ToArray();

        if (imports.Any())
        {
            fw.AddImports(imports);
            fw.WriteLine();
        }

        fw.WriteLine($@"public class {sampleClass.GetMapperClassName(isPersistant)} {{");

        foreach (var fromMapper in fromMappers)
        {
            WriteFromMapper(fromMapper.Classe, fromMapper.Mapper, fw, tag);
        }

        foreach (var toMapper in toMappers)
        {
            WriteToMapper(toMapper.Classe, toMapper.Mapper, fw, tag);
        }

        fw.WriteLine("}");
    }

    private void WriteToMapper(Class classe, ClassMappings mapper, JavaWriter fw, string tag)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, $"Mappe '{classe}' vers '{mapper.Class.NamePascal}'");
        if (mapper.Comment != null)
        {
            fw.WriteLine(1, $" * {mapper.Comment}");
        }

        fw.WriteParam("source", $"Instance de '{classe}'");
        fw.WriteParam("target", $"Instance pré-existante de '{mapper.Class.NamePascal}'. Une nouvelle instance sera créée si non spécifié.");

        fw.WriteReturns(1, $"Une nouvelle instance de '{mapper.Class.NamePascal}' ou bien l'instance passée en paramètre dont les champs ont été surchargés");
        fw.WriteDocEnd(1);

        fw.WriteLine(1, $"public static {mapper.Class.NamePascal} {mapper.Name.Value.ToCamelCase()}({classe} source, {mapper.Class.NamePascal} target) {{");
        fw.WriteLine(2, "if (source == null) {");
        fw.WriteLine(3, $"throw new IllegalArgumentException(\"source cannot be null\");");
        fw.WriteLine(2, "}");
        fw.WriteLine();
        fw.WriteLine(2, "if (target == null) {");
        if (mapper.Class.Abstract)
        {
            fw.WriteLine(3, $"throw new IllegalArgumentException(\"target cannot be null\");");
        }
        else
        {
            fw.WriteLine(3, $"target = new {mapper.Class.NamePascal}();");
        }

        fw.WriteLine(2, "}");
        fw.WriteLine();
        if (mapper.ParentMapper != null)
        {
            fw.AddImport(_config.GetMapperImport(classe.Extends!, mapper.ParentMapper, tag)!);
            fw.WriteLine(2, $"{classe.Extends!.GetMapperClassName(mapper)}.{mapper.ParentMapper.Name.Value.ToCamelCase()}(({classe.Extends!.NamePascal}) source, ({mapper.ParentMapper.Class.NamePascal}) target);");
        }

        var hydrate = string.Empty;
        if (mapper.Class.Abstract)
        {
            hydrate = "target.hydrate(";
        }

        var isFirst = true;
        foreach (var mapping in mapper.Mappings.OrderBy(m => m.Key.Class.Properties.IndexOf(m.Key)))
        {
            var propertyTarget = mapping.Value;
            var propertySource = mapping.Key;
            var getterPrefix = propertyTarget!.GetJavaType() == "boolean" ? "is" : "get";
            var getter = GetSourceGetter(propertySource, propertyTarget!, classe, fw, "source", tag);
            if (mapper.Class.Abstract)
            {
                if (!isFirst)
                {
                    hydrate += ", ";
                }
                else
                {
                    isFirst = false;
                }

                if (getter.CheckSourceNull)
                {
                    hydrate += $"source.{getterPrefix}{propertySource.GetJavaName(true)}() != null ? {getter.Getter} : null";
                }
                else
                {
                    hydrate += getter.Getter;
                }
            }
            else
            {
                if (getter.Getter != string.Empty)
                {
                    if (getter.CheckSourceNull)
                    {
                        fw.WriteLine(2, $"if (source.{getterPrefix}{propertySource.GetJavaName(true)}() != null) {{");
                    }

                    fw.WriteLine(2 + (getter.CheckSourceNull ? 1 : 0), $"target.set{propertyTarget!.GetJavaName(true)}({getter.Getter});");

                    if (getter.CheckSourceNull)
                    {
                        fw.WriteLine(2, $"}}");
                        fw.WriteLine();
                    }
                }
            }
        }

        if (mapper.Class.Abstract)
        {
            hydrate += ");";
            fw.WriteLine(2, hydrate);
        }

        fw.WriteLine(2, "return target;");
        fw.WriteLine(1, "}");
    }

    private (string Getter, bool CheckSourceNull) GetSourceGetter(IProperty propertySource, IProperty propertyTarget, Class classe, JavaWriter fw, string sourceName, string tag)
    {
        var getterPrefix = propertyTarget!.GetJavaType() == "boolean" ? "is" : "get";
        var getter = string.Empty;
        var checkSourceNull = false;
        if ((!propertySource.Class.IsPersistent && !propertyTarget.Class.IsPersistent)
            || !(propertySource is AssociationProperty || propertyTarget is AssociationProperty))
        {
            getter = $"{sourceName}.{getterPrefix}{propertySource.GetJavaName(true)}()";
        }
        else if (propertySource.Class.IsPersistent && !propertyTarget.Class.IsPersistent && propertySource is AssociationProperty apSource)
        {
            checkSourceNull = true;
            if (propertyTarget is CompositionProperty cp)
            {
                if (propertySource.Class.ToMappers.Any(t => t.Class == cp.Composition))
                {
                    var cpMapper = propertySource.Class.ToMappers.Find(t => t.Class == cp.Composition)!;
                    getter = $"{cpMapper.Class.GetMapperClassName(cpMapper)}.{cpMapper.Name.Value.ToCamelCase()}({sourceName}.{getterPrefix}{cp.GetJavaName(true)}(), target.get{apSource.GetJavaName(true)}())";
                    fw.AddImport(_config.GetMapperImport(cpMapper.Class, cpMapper, tag)!);
                }
                else if (cp.Composition.FromMappers.Any(f => f.Params.Count == 1 && f.Params.First().Class == apSource.Association))
                {
                    var cpMapper = cp.Composition.FromMappers.Find(f => f.Params.Count == 1 && f.Params.First().Class == apSource.Association)!;
                    getter = $"{sourceName}.{getterPrefix}{apSource.GetJavaName(true)}()";
                    if (apSource.Type == AssociationType.OneToMany || apSource.Type == AssociationType.ManyToMany)
                    {
                        getter = $"{getter}.stream().map(item -> {cp.Composition.GetMapperClassName(cpMapper)}.create{cp.Composition}(item, null)).collect(Collectors.toList())";
                        fw.AddImport("java.util.stream.Collectors");
                    }
                    else
                    {
                        getter = $"{cp.Composition.GetMapperClassName(cpMapper)}.create{cp.Composition}({getter}, target.get{propertyTarget.GetJavaName(true)}())";
                    }

                    fw.AddImport(_config.GetMapperImport(cp.Composition, cpMapper!, tag)!);
                }
                else
                {
                    throw new ModelException(classe, $"La propriété {propertySource.Name} ne peut pas être mappée avec la propriété {propertyTarget.Name} car il n'existe pas de mapper {cp.Composition.Name} -> {apSource.Association.Name}");
                }
            }
            else
            {
                if (apSource.Type == AssociationType.OneToOne || apSource.Type == AssociationType.ManyToOne)
                {
                    getter = $"{sourceName}.{getterPrefix}{propertySource.GetJavaName(true)}().get{apSource.Property.GetJavaName(true)}()";
                }
                else
                {
                    getter = $"{sourceName}.{getterPrefix}{propertySource.GetJavaName(true)}().stream().filter(Objects::nonNull).map({apSource.Association.NamePascal}::get{apSource.Property.GetJavaName(true)}).collect(Collectors.toList())";
                    fw.AddImport("java.util.stream.Collectors");
                    fw.AddImport("java.util.Objects");
                    fw.AddImport(apSource.Association.GetImport(_config, tag));
                }
            }
        }
        else if (!propertySource.Class.IsPersistent && propertyTarget.Class.IsPersistent && propertyTarget is AssociationProperty apTarget)
        {
            if (apTarget.Property.IsEnum())
            {
                var isMultiple = apTarget.Type == AssociationType.OneToMany || apTarget.Type == AssociationType.ManyToMany;
                if (isMultiple)
                {
                    getter = $@"{sourceName}.{getterPrefix}{propertySource.GetJavaName(true)}(){(!propertySource.Class.IsPersistent ? $".stream().map({apTarget.Association.PrimaryKey.Single().GetJavaType()}::getEntity).collect(Collectors.toList())" : string.Empty)}";
                    fw.AddImport("java.util.stream.Collectors");
                }
                else
                {
                    getter = $"{sourceName}.{getterPrefix}{propertySource.GetJavaName(true)}(){(!propertySource.Class.IsPersistent ? ".getEntity()" : string.Empty)}";
                    checkSourceNull = true;
                }
            }
            else if (propertyTarget.Class.IsPersistent && propertySource.Class.IsPersistent)
            {
                getter = $"{sourceName}.{getterPrefix}{propertySource.GetJavaName(true)}()";
            }
            else if (propertySource is CompositionProperty cp)
            {
                if (cp.Composition.ToMappers.Any(t => t.Class == apTarget.Association))
                {
                    var cpMapper = cp.Composition.ToMappers.Find(t => t.Class == apTarget.Association)!;
                    getter = $"{cpMapper.Class.GetMapperClassName(cpMapper)}.{cpMapper.Name.Value.ToCamelCase()}({sourceName}.{getterPrefix}{cp.GetJavaName(true)}(), target.get{apTarget.GetJavaName(true)}())";
                    fw.AddImport(_config.GetMapperImport(cpMapper.Class, cpMapper, tag)!);
                    checkSourceNull = true;
                }
                else
                {
                    throw new ModelException(classe, $"La propriété {propertySource.Name} ne peut pas être mappée avec la propriété {propertyTarget.Name} car il n'existe pas de mapper {cp.Composition.Name} -> {apTarget.Association.Name}");
                }
            }
        }
        else
        {
            if (propertySource is IFieldProperty ifpTo && propertyTarget is IFieldProperty ifpFrom && ifpFrom.Domain != ifpTo.Domain)
            {
                var converter = ifpFrom.Domain.ConvertersFrom.FirstOrDefault(c => c.To.Any(t => t == ifpTo.Domain));
                string conversion = $@"{sourceName}.{getterPrefix}{propertySource.GetJavaName(true)}()";
                if (converter != null && converter.Java?.Text != null)
                {
                    var convert = converter.Java.Text;
                    getter = convert.Replace("{value}", conversion)
                        .ParseTemplate(ifpFrom.Domain, "java", "from.")
                        .ParseTemplate(ifpTo.Domain, "java", "to.");
                }
                else
                {
                    getter = $"{sourceName}.{propertySource.GetJavaName(true)}()";
                }
            }
            else
            {
                getter = $"{sourceName}.{getterPrefix}{propertySource.GetJavaName(true)}()";
            }
        }

        return (Getter: getter, CheckSourceNull: checkSourceNull);
    }

    private void WriteFromMapper(Class classe, FromMapper mapper, JavaWriter fw, string tag)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, $"Map les champs des classes passées en paramètre dans l'objet target'");
        fw.WriteParam("target", $"Instance de '{classe}' (ou null pour créer une nouvelle instance)");
        foreach (var param in mapper.Params)
        {
            if (param.Comment != null)
            {
                fw.WriteParam(param.Name.ToCamelCase(), param.Comment);
            }
            else
            {
                fw.WriteParam(param.Name.ToCamelCase(), $"Instance de '{param.Class}'");
            }
        }

        fw.WriteReturns(1, $"Une nouvelle instance de '{classe}' ou bien l'instance passée en paramètres sur lesquels les champs sources ont été mappée");
        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"public static {classe.NamePascal} create{classe}({string.Join(", ", mapper.Params.Select(p => $"{p.Class} {p.Name.ToCamelCase()}"))}, {classe} target) {{");
        fw.WriteLine(2, "if (target == null) {");
        if (classe.Abstract)
        {
            fw.WriteLine(3, $"throw new IllegalArgumentException(\"target cannot be null\");");
        }
        else
        {
            fw.WriteLine(3, $"target = new {classe.NamePascal}();");
        }

        fw.WriteLine(2, "}");
        fw.WriteLine();
        var hydrate = string.Empty;
        if (classe.Abstract)
        {
            hydrate = "target.hydrate(";
        }

        var isFirst = true;
        if (classe.Extends != null)
        {
            if (mapper.ParentMapper != null)
            {
                fw.WriteLine(2, $"{classe.Extends!.GetMapperClassName(mapper.ParentMapper)}.create{classe.Extends}({string.Join(", ", mapper.Params.Take(mapper.ParentMapper.Params.Count).Select(p => $"({p.Class.NamePascal}) {p.Name}"))}, target);");
            }
        }

        foreach (var param in mapper.Params.Where(p => p.Mappings.Count > 0))
        {
            fw.WriteLine(2, $"if ({param.Name.ToCamelCase()} != null) {{");
            var mappings = param.Mappings.ToList();

            foreach (var mapping in mappings)
            {
                var propertyTarget = mapping.Key;
                var propertySource = mapping.Value!;
                var getterPrefix = propertyTarget!.GetJavaType() == "boolean" ? "is" : "get";
                var getter = GetSourceGetter(propertySource, propertyTarget, classe, fw, param.Name.ToCamelCase(), tag);
                if (classe.Abstract)
                {
                    if (!isFirst)
                    {
                        hydrate += ", ";
                    }
                    else
                    {
                        isFirst = false;
                    }

                    if (getter.CheckSourceNull)
                    {
                        hydrate += $"{param.Name}.{getterPrefix}{propertySource.GetJavaName(true)}() != null ? {getter.Getter} : null";
                    }
                    else
                    {
                        hydrate += getter.Getter;
                    }
                }
                else
                {
                    if (getter.Getter != string.Empty)
                    {
                        if (getter.CheckSourceNull)
                        {
                            fw.WriteLine(3, $"if ({param.Name}.{getterPrefix}{propertySource.GetJavaName(true)}() != null) {{");
                        }

                        fw.WriteLine(3 + (getter.CheckSourceNull ? 1 : 0), $"target.set{propertyTarget!.GetJavaName(true)}({getter.Getter});");

                        if (getter.CheckSourceNull)
                        {
                            fw.WriteLine(3, $"}}");
                            fw.WriteLine();
                        }
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

        if (classe.Abstract)
        {
            hydrate += ");";
            fw.WriteLine(2, hydrate);
        }

        fw.WriteLine(2, "return target;");
        fw.WriteLine(1, "}");
    }
}