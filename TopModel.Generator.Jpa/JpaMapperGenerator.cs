using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JpaMapperGenerator : MapperGeneratorBase<JpaConfig>
{
    private readonly ILogger<JpaMapperGenerator> _logger;

    public JpaMapperGenerator(ILogger<JpaMapperGenerator> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override string Name => "JpaMapperGenerator";

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
        var sampleFromMapper = fromMappers.FirstOrDefault();
        var sampleToMapper = toMappers.FirstOrDefault();

        var (mapperNs, modelPath) = sampleFromMapper != default
            ? Config.GetMapperLocation(sampleFromMapper)
            : Config.GetMapperLocation(sampleToMapper);

        var package = Config.GetPackageName(mapperNs, modelPath, tag);

        using var fw = new JavaWriter(fileName, _logger, package, null);

        var imports = fromMappers.SelectMany(m => m.Mapper.Params.Select(p => p.Class).Concat(new[] { m.Classe }))
            .Concat(toMappers.SelectMany(m => new[] { m.Classe, m.Mapper.Class }))
            .Where(c => Classes.Contains(c))
            .Select(c => c.GetImport(Config, c.Tags.Contains(tag) ? tag : c.Tags.Intersect(Config.Tags).First()))
            .Distinct()
            .ToArray();

        if (imports.Any())
        {
            fw.AddImports(imports);
            fw.WriteLine();
        }

        var javaOrJakarta = Config.PersistenceMode.ToString().ToLower();
        fw.AddImport($"{javaOrJakarta}.annotation.Generated");
        fw.WriteLine("@Generated(\"TopModel : https://github.com/klee-contrib/topmodel\")");
        fw.WriteLine($@"public class {Config.GetMapperName(mapperNs, modelPath)} {{");

        foreach (var (classe1, mapper) in fromMappers)
        {
            WriteFromMapper(classe1, mapper, fw, tag);
        }

        foreach (var (classe, mapper1) in toMappers)
        {
            WriteToMapper(classe, mapper1, fw, tag);
        }

        fw.WriteLine("}");
    }

    private (string Getter, bool CheckSourceNull) GetSourceGetter(IProperty propertySource, IProperty propertyTarget, Class classe, JavaWriter fw, string sourceName, string tag)
    {
        var getterPrefix = Config.GetType(propertyTarget!) == "boolean" ? "is" : "get";
        var getter = string.Empty;
        var converter = Config.GetConverter((propertySource as IFieldProperty)?.Domain, (propertyTarget as IFieldProperty)?.Domain);
        if (converter != null)
        {
            fw.AddImports(Config.GetImplementation(converter)!.Imports);
        }

        if (Config.UseJdbc)
        {
            getter = $"{sourceName}.{getterPrefix}{propertySource.NamePascal.ToFirstUpper()}()";
            return (Getter: Config.GetConvertedValue(
                getter,
                (propertySource as IFieldProperty)?.Domain,
                (propertyTarget as IFieldProperty)?.Domain), CheckSourceNull: false);
        }

        var checkSourceNull = false;
        if ((!propertySource.Class.IsPersistent && !propertyTarget.Class.IsPersistent)
            || !(propertySource is AssociationProperty || propertyTarget is AssociationProperty))
        {
            getter = $"{sourceName}.{propertySource.NameByClassPascal.WithPrefix(getterPrefix)}()";
        }
        else if (propertySource.Class.IsPersistent && !propertyTarget.Class.IsPersistent && propertySource is AssociationProperty apSource)
        {
            checkSourceNull = true;
            if (propertyTarget is CompositionProperty cp)
            {
                if (propertySource.Class.ToMappers.Any(t => t.Class == cp.Composition))
                {
                    var cpMapper = propertySource.Class.ToMappers.Find(t => t.Class == cp.Composition)!;
                    var (cpMapperNs, cpMapperModelPath) = Config.GetMapperLocation((cpMapper.Class, cpMapper));

                    getter = $"{Config.GetMapperName(cpMapperNs, cpMapperModelPath)}.{cpMapper.Name.Value.ToCamelCase()}({sourceName}.{cp.NameByClassPascal.WithPrefix(getterPrefix)}(), target.get{apSource.NameByClassPascal}())";
                    fw.AddImport(Config.GetMapperImport(cpMapperNs, cpMapperModelPath, tag)!);
                }
                else if (cp.Composition.FromMappers.Any(f => f.Params.Count == 1 && f.Params.First().Class == apSource.Association))
                {
                    var cpMapper = cp.Composition.FromMappers.Find(f => f.Params.Count == 1 && f.Params.First().Class == apSource.Association)!;
                    var (cpMapperNs, cpMapperModelPath) = Config.GetMapperLocation((cp.Composition, cpMapper));

                    getter = $"{sourceName}.{apSource.NameByClassPascal.WithPrefix(getterPrefix)}()";
                    if (apSource.Type.IsToMany())
                    {
                        getter = $"{getter}.stream().map(item -> {Config.GetMapperName(cpMapperNs, cpMapperModelPath)}.create{cp.Composition}(item, null)).collect(Collectors.toList())";
                        fw.AddImport("java.util.stream.Collectors");
                    }
                    else
                    {
                        getter = $"{Config.GetMapperName(cpMapperNs, cpMapperModelPath)}.create{cp.Composition}({getter}, target.get{propertyTarget.NameByClassPascal}())";
                    }

                    fw.AddImport(Config.GetMapperImport(cpMapperNs, cpMapperModelPath, tag)!);
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
                    getter = $"{sourceName}.{propertySource.NameByClassPascal.WithPrefix(getterPrefix)}().get{apSource.Property.NameByClassPascal}()";
                }
                else
                {
                    getter = $"{sourceName}.{propertySource.NameByClassPascal.WithPrefix(getterPrefix)}().stream().filter(Objects::nonNull).map({apSource.Association.NamePascal}::get{apSource.Property.NameByClassPascal}).collect(Collectors.toList())";
                    fw.AddImport("java.util.stream.Collectors");
                    fw.AddImport("java.util.Objects");
                    fw.AddImport(apSource.Association.GetImport(Config, tag));
                }
            }
        }
        else if (!propertySource.Class.IsPersistent && propertyTarget.Class.IsPersistent && propertyTarget is AssociationProperty apTarget)
        {
            if (Config.CanClassUseEnums(apTarget.Property.Class))
            {
                if (apTarget.Type.IsToMany())
                {
                    getter = $@"{sourceName}.{propertySource.NameByClassPascal.WithPrefix(getterPrefix)}(){(!propertySource.Class.IsPersistent ? $".stream().map({Config.GetType(apTarget.Property)}::getEntity).collect(Collectors.toList())" : string.Empty)}";
                    fw.AddImport("java.util.stream.Collectors");
                }
                else
                {
                    getter = $"{sourceName}.{propertySource.NameByClassPascal.WithPrefix(getterPrefix)}(){(!propertySource.Class.IsPersistent ? ".getEntity()" : string.Empty)}";
                    checkSourceNull = true;
                }
            }
            else if (propertyTarget.Class.IsPersistent && propertySource.Class.IsPersistent)
            {
                getter = $"{sourceName}.{propertySource.NameByClassPascal.WithPrefix(getterPrefix)}()";
            }
            else if (propertySource is CompositionProperty cp)
            {
                if (cp.Composition.ToMappers.Any(t => t.Class == apTarget.Association))
                {
                    var cpMapper = cp.Composition.ToMappers.Find(t => t.Class == apTarget.Association)!;
                    var (cpMapperNs, cpMapperModelPath) = Config.GetMapperLocation((cpMapper.Class, cpMapper));

                    var isMultiple = apTarget.Type == AssociationType.OneToMany || apTarget.Type == AssociationType.ManyToMany;
                    if (isMultiple)
                    {
                        getter = $@"{sourceName}.{propertySource.NameByClassPascal.WithPrefix(getterPrefix)}(){(!propertySource.Class.IsPersistent ? $".stream().map(src -> {Config.GetMapperName(cpMapperNs, cpMapperModelPath)}.{cpMapper.Name.ToCamelCase()}(src, null)).collect(Collectors.toList())" : string.Empty)}";
                        fw.AddImport("java.util.stream.Collectors");
                    }
                    else
                    {
                        getter = $"{Config.GetMapperName(cpMapperNs, cpMapperModelPath)}.{cpMapper.Name.Value.ToCamelCase()}({sourceName}.{cp.NameByClassPascal.WithPrefix(getterPrefix)}(), target.get{apTarget.NameByClassPascal}())";
                        checkSourceNull = true;
                        fw.AddImport(Config.GetMapperImport(cpMapperNs, cpMapperModelPath, tag)!);
                    }
                }
                else
                {
                    throw new ModelException(classe, $"La propriété {propertySource.Name} ne peut pas être mappée avec la propriété {propertyTarget.Name} car il n'existe pas de mapper {cp.Composition.Name} -> {apTarget.Association.Name}");
                }
            }
        }
        else
        {
            getter = $"{sourceName}.{propertySource.NameByClassPascal.WithPrefix(getterPrefix)}()";
        }

        return (Getter: Config.GetConvertedValue(
                getter,
                (propertySource as IFieldProperty)?.Domain,
                (propertyTarget as IFieldProperty)?.Domain), CheckSourceNull: checkSourceNull);
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

        fw.WriteReturns(1, $"Une nouvelle instance de '{classe.NamePascal}' ou bien l'instance passée en paramètres sur lesquels les champs sources ont été mappée");
        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"public static {classe.NamePascal} create{classe.NamePascal}({string.Join(", ", mapper.Params.Select(p => $"{p.Class} {p.Name.ToCamelCase()}"))}, {classe.NamePascal} target) {{");
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
                var (parentMapperNs, parentMapperModelPath) = Config.GetMapperLocation((classe.Extends!, mapper.ParentMapper));
                fw.WriteLine(2, $"{Config.GetMapperName(parentMapperNs, parentMapperModelPath)}.create{classe.Extends}({string.Join(", ", mapper.Params.Take(mapper.ParentMapper.Params.Count).Select(p => $"({p.Class.NamePascal}) {p.Name}"))}, target);");
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
                var getterPrefix = Config.GetType(propertyTarget!) == "boolean" ? "is" : "get";
                var (getter, checkSourceNull) = GetSourceGetter(propertySource, propertyTarget, classe, fw, param.Name.ToCamelCase(), tag);
                var propertyTargetName = Config.UseJdbc ? propertyTarget.NamePascal : propertyTarget.NameByClassPascal;
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

                    if (checkSourceNull)
                    {
                        hydrate += $"{param.Name}.{propertyTargetName.WithPrefix(getterPrefix)}() != null ? {getter} : null";
                    }
                    else
                    {
                        hydrate += getter;
                    }
                }
                else
                {
                    if (getter != string.Empty)
                    {
                        if (checkSourceNull)
                        {
                            fw.WriteLine(3, $"if ({param.Name}.{propertySource.NameByClassPascal.WithPrefix(getterPrefix)}() != null) {{");
                        }

                        fw.WriteLine(3 + (checkSourceNull ? 1 : 0), $"target.{propertyTargetName!.WithPrefix("set")}({getter});");

                        if (checkSourceNull)
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
            var (parentMapperNs, parentMapperModelPath) = Config.GetMapperLocation((classe.Extends!, mapper.ParentMapper));
            fw.AddImport(Config.GetMapperImport(parentMapperNs, parentMapperModelPath, tag)!);
            fw.WriteLine(2, $"{Config.GetMapperName(parentMapperNs, parentMapperModelPath)}.{mapper.ParentMapper.Name.Value.ToCamelCase()}(({classe.Extends!.NamePascal}) source, ({mapper.ParentMapper.Class.NamePascal}) target);");
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
            var getterPrefix = Config.GetType(propertyTarget!) == "boolean" ? "is" : "get";
            var (getter, checkSourceNull) = GetSourceGetter(propertySource, propertyTarget!, classe, fw, "source", tag);
            var propertyTargetName = Config.UseJdbc ? propertyTarget!.NamePascal : propertyTarget!.NameByClassPascal;
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

                if (checkSourceNull)
                {
                    hydrate += $"source.{propertyTargetName.WithPrefix(getterPrefix)}() != null ? {getter} : null";
                }
                else
                {
                    hydrate += getter;
                }
            }
            else
            {
                if (getter != string.Empty)
                {
                    if (checkSourceNull)
                    {
                        fw.WriteLine(2, $"if (source.{propertySource.NameByClassPascal.WithPrefix(getterPrefix)}() != null) {{");
                    }

                    fw.WriteLine(2 + (checkSourceNull ? 1 : 0), $"target.{propertyTargetName.WithPrefix("set")}({getter});");

                    if (checkSourceNull)
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
}