using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Generator.Jpa.ClassGeneration;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JpaMapperGenerator(ILogger<JpaMapperGenerator> logger, IFileWriterProvider writerProvider)
    : MapperGeneratorBase<JpaConfig>(logger, writerProvider)
{
    private readonly ILogger<JpaMapperGenerator> _logger = logger;

    private JpaModelPropertyGenerator? _jpaModelPropertyGenerator;

    public override string Name => "JpaMapperGenerator";

    protected virtual JpaModelPropertyGenerator JpaModelPropertyGenerator
    {
        get
        {
            _jpaModelPropertyGenerator ??= Config.UseJdbc ? new JdbcModelPropertyGenerator(Config, Classes, []) : new JpaModelPropertyGenerator(Config, Classes, []);
            return _jpaModelPropertyGenerator;
        }
    }

    protected override string GetFileName((Class Classe, FromMapper Mapper) mapper, string tag)
    {
        return Config.GetMapperFilePath(mapper, GetBestClassTag(mapper.Classe, tag));
    }

    protected override string GetFileName((Class Classe, ClassMappings Mapper) mapper, string tag)
    {
        return Config.GetMapperFilePath(mapper, GetBestClassTag(mapper.Classe, tag));
    }

    protected virtual JavaMethod GetFromMapperNoTarget(Class classe, FromMapper mapper, string tag)
    {
        var fromMapperMethod = new JavaMethod(classe.NamePascal, $"create{classe.NamePascal}")
        {
            Static = true,
            Visibility = "public",
            ReturnComment = $"Une nouvelle instance de '{classe.NamePascal}' sur laquelle les champs sources ont été mappée",
            Comment = "Mapper les champs sources sur une nouvelle instance de la classe",
        };

        foreach (var param in mapper.ClassParams)
        {
            var methodParameter = new JavaMethodParameter(param.Class.NamePascal, param.Name.ToCamelCase())
            {
                Comment = param.Comment ?? $"Instance de '{param.Class.NamePascal}'"
            };
            methodParameter.Imports.Add(param.Class.GetImport(Config, tag));
            fromMapperMethod.AddParameter(methodParameter);
        }

        foreach (var param in mapper.PropertyParams)
        {
            var methodParameter = new JavaMethodParameter(Config.GetType(param.Property, Classes, useClassForAssociation: UseClassForAssociation(param.Property, classe)), param.Property.NameCamel)
            {
                Comment = param.Property.Comment
            };
            methodParameter.Imports.AddRange(param.Property.GetTypeImports(Config, tag));
            fromMapperMethod.AddParameter(methodParameter);
        }

        fromMapperMethod.AddBodyLine($"return create{classe.NamePascal}({string.Join(", ", fromMapperMethod.Parameters.Select(p => p.Name))}, new {classe.NamePascal}());");
        return fromMapperMethod;
    }

    protected virtual JavaMethod GetFromMapperWithTarget(Class classe, FromMapper mapper, string tag)
    {
        var mapperWithTarget = GetFromMapperNoTarget(classe, mapper, tag);
        if (!Config.MappersInClass)
        {
            mapperWithTarget.Visibility = "private";
        }

        mapperWithTarget.Body.Clear();
        mapperWithTarget.Parameters.Add(new JavaMethodParameter(classe.NamePascal, "target"));
        mapperWithTarget.ReturnComment = $"Une nouvelle instance de '{classe.NamePascal}' ou bien l'instance passée en paramètres sur lesquels les champs sources ont été mappée";
        mapperWithTarget.Comment = "Mapper les champs sources sur une nouvelle instance de la classe ou bien sur l'instance passée en paramètres";
        if (Config.MappersInClass)
        {
            mapperWithTarget.AddBodyLine("if (target == null) {");
            mapperWithTarget.AddBodyLine(1, $"throw new IllegalArgumentException(\"target cannot be null\");");
            mapperWithTarget.AddBodyLine("}");
        }

        mapperWithTarget.AddBodyLine();

        var hydrate = string.Empty;
        if (classe.Abstract)
        {
            hydrate = "target.hydrate(";
        }

        var isFirst = true;

        foreach (var param in mapper.ClassParams.Where(p => p.Mappings.Count > 0))
        {
            if (param.Required && !classe.Abstract)
            {
                mapperWithTarget.AddBodyLine($"if ({param.Name.ToCamelCase()} == null) {{");
                mapperWithTarget.AddBodyLine(1, $"throw new IllegalArgumentException(\"{param.Name} cannot be null\");");
                mapperWithTarget.AddBodyLine("}");
                mapperWithTarget.AddBodyLine();
            }
        }

        foreach (var param in mapper.PropertyParams)
        {
            if (param.Property.Required && !classe.Abstract)
            {
                if (param.TargetProperty is AssociationProperty atg && atg.Association.IsPersistent && classe.IsPersistent)
                {
                    continue;
                }

                mapperWithTarget.AddBodyLine($"if ({param.Property.NameCamel} == null) {{");
                mapperWithTarget.AddBodyLine(1, $"throw new IllegalArgumentException(\"{param.Property.NameCamel} cannot be null\");");
                mapperWithTarget.AddBodyLine("}");
                mapperWithTarget.AddBodyLine();
            }
        }

        foreach (var param in mapper.ClassParams.Where(p => p.Mappings.Count > 0))
        {
            var mappings = param.Mappings.ToList();
            var indent = 0;
            if (!param.Required)
            {
                mapperWithTarget.AddBodyLine(indent, $"if ({param.Name.ToCamelCase()} != null) {{");
                indent++;
            }

            foreach (var mapping in mappings)
            {
                var propertyTarget = mapping.Key;
                var propertySource = mapping.Value!;
                var (getter, checkSourceNull, imports) = GetSourceGetter(propertySource, propertyTarget, classe, param.Name.ToCamelCase(), tag);
                mapperWithTarget.Imports.AddRange(imports);
                var propertyTargetName = JpaModelPropertyGenerator.GetPropertyName(propertyTarget);
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
                        hydrate += $"{param.Name}.{JpaModelPropertyGenerator.GetGetterName(propertyTarget)}() != null ? {getter} : null";
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
                            mapperWithTarget.AddBodyLine(indent, $"if ({param.Name}.{JpaModelPropertyGenerator.GetGetterName(propertySource)}() != null) {{");
                        }

                        mapperWithTarget.AddBodyLine(indent + (checkSourceNull ? 1 : 0), $"target.{JpaModelPropertyGenerator.GetSetterName(propertyTarget)}({getter});");

                        if (checkSourceNull)
                        {
                            mapperWithTarget.AddBodyLine(indent, $"}}");
                            mapperWithTarget.AddBodyLine();
                        }
                    }
                }
            }

            if (!param.Required)
            {
                mapperWithTarget.AddBodyLine(indent - 1, "}");
                mapperWithTarget.AddBodyLine();
            }
        }

        foreach (var param in mapper.PropertyParams)
        {
            var propertyTargetName = Config.UseJdbc ? param.TargetProperty.NamePascal : param.TargetProperty.NameByClassPascal;
            if (param.TargetProperty is AssociationProperty apTg && apTg.Association.IsPersistent && classe.IsPersistent)
            {
                continue;
            }

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

                hydrate += param.Property.NameCamel;
            }
            else
            {
                mapperWithTarget.AddBodyLine($"target.{JpaModelPropertyGenerator.GetSetterName(param.TargetProperty)}({param.Property.NameCamel});");
            }
        }

        if (classe.Abstract)
        {
            hydrate += ");";
            mapperWithTarget.AddBodyLine(1, hydrate);
        }

        mapperWithTarget.AddBodyLine("return target;");

        return mapperWithTarget;
    }

    protected virtual (string Getter, bool CheckSourceNull, IEnumerable<string> Imports) GetSourceGetter(IProperty propertySource, IProperty propertyTarget, Class classe, string sourceName, string tag)
    {
        var getter = string.Empty;
        var imports = new List<string>();
        var getterName = JpaModelPropertyGenerator.GetGetterName(propertySource);
        var converter = Config.GetConverter(propertySource.Domain, propertyTarget.Domain);
        var targetType = JpaModelPropertyGenerator.GetPropertyType(propertyTarget);
        var collector = $"Collectors.to{targetType.Split('<').First()}()";
        if (converter != null && Config.GetImplementation(converter) != null)
        {
            var impl = Config.GetImplementation(converter);
            if (impl != null)
            {
                imports.AddRange(impl.Imports);
            }
        }

        if (Config.UseJdbc)
        {
            getter = $"{sourceName}.{getterName}()";
            return (Getter: Config.GetConvertedValue(
                getter,
                propertySource.Domain,
                propertyTarget.Domain), CheckSourceNull: false, Imports: imports);
        }

        var checkSourceNull = false;
        if (
            (propertySource.Class.IsPersistent == propertyTarget.Class.IsPersistent)
             || !(propertySource is AssociationProperty
                || propertySource is AliasProperty psAlp && psAlp.Property is AssociationProperty
                || propertyTarget is AssociationProperty
                || propertySource is AliasProperty ptAlp && ptAlp.Property is AssociationProperty))
        {
            getter = $"{sourceName}.{getterName}()";
        }
        else if (propertySource.Class.IsPersistent && (!propertyTarget.Class.IsPersistent || propertyTarget is not AssociationProperty) && (propertySource is AssociationProperty apSource && apSource.Association.IsPersistent || propertySource is AliasProperty alpSource && alpSource.Property is AssociationProperty apSource2 && apSource2.Association.IsPersistent))
        {
            apSource = propertySource is AssociationProperty ap ? ap : (AssociationProperty)((AliasProperty)propertySource).Property;
            checkSourceNull = true;
            if (propertyTarget is CompositionProperty cp)
            {
                if (propertySource.Class.ToMappers.Any(t => t.Class == cp.Composition))
                {
                    var cpMapper = propertySource.Class.ToMappers.Find(t => t.Class == cp.Composition)!;
                    var (cpMapperNs, cpMapperModelPath) = Config.GetMapperLocation((cpMapper.Class, cpMapper));

                    getter = $"{Config.GetMapperName(cpMapperNs, cpMapperModelPath)}.{cpMapper.Name.Value.ToCamelCase()}({sourceName}.{getterName}(), target.get{apSource.NameByClassPascal}())";
                    imports.Add(Config.GetMapperImport(cpMapperNs, cpMapperModelPath, tag)!);
                }
                else if (cp.Composition.FromMappers.Any(f => f.Params.Count == 1 && f.ClassParams.First().Class == apSource.Association))
                {
                    var cpMapper = cp.Composition.FromMappers.Find(f => f.Params.Count == 1 && f.ClassParams.First().Class == apSource.Association)!;
                    var (cpMapperNs, cpMapperModelPath) = Config.GetMapperLocation((cp.Composition, cpMapper));

                    getter = $"{sourceName}.{getterName}()";
                    if (apSource.Type.IsToMany())
                    {
                        getter = $"{getter}.stream().map(item -> {Config.GetMapperName(cpMapperNs, cpMapperModelPath)}.create{cp.Composition}(item, null)).collect({collector})";
                        imports.Add("java.util.stream.Collectors");
                    }
                    else
                    {
                        getter = $"{Config.GetMapperName(cpMapperNs, cpMapperModelPath)}.create{cp.Composition}({getter}, target.get{propertyTarget.NameByClassPascal}())";
                    }

                    imports.Add(Config.GetMapperImport(cpMapperNs, cpMapperModelPath, tag)!);
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
                    if (Config.EnumsAsEnums && Config.CanClassUseEnums(apSource.Association, prop: apSource.Property, availableClasses: Classes))
                    {
                        getter = $"{sourceName}.{getterName}()";
                        checkSourceNull = false;
                    }
                    else
                    {
                        getter = $"{sourceName}.{getterName}().get{apSource.Property.NameByClassPascal}()";
                    }
                }
                else
                {
                    checkSourceNull = true;
                    imports.Add("java.util.stream.Collectors");
                    imports.Add("java.util.Objects");
                    if (Config.EnumsAsEnums && Config.CanClassUseEnums(apSource.Association, prop: apSource.Property, availableClasses: Classes))
                    {
                        getter = $"{sourceName}.{getterName}().stream().filter(Objects::nonNull).collect({collector})";
                    }
                    else
                    {
                        getter = $"{sourceName}.{getterName}().stream().filter(Objects::nonNull).map({apSource.Association.NamePascal}::get{apSource.Property.NameByClassPascal}).collect({collector})";
                        imports.Add(apSource.Association.GetImport(Config, tag));
                    }
                }
            }
        }
        else if ((!propertySource.Class.IsPersistent || propertySource is not AssociationProperty) && propertyTarget.Class.IsPersistent && (propertyTarget is AssociationProperty apTarget && apTarget.Association.IsPersistent || propertyTarget is AliasProperty ptAp && ptAp.Property is AssociationProperty ptApAss && ptApAss.Association.IsPersistent))
        {
            apTarget = propertyTarget is AssociationProperty ap ? ap : (AssociationProperty)((AliasProperty)propertyTarget).Property;
            if (Config.CanClassUseEnums(apTarget.Property.Class))
            {
                if (!propertySource.Class.IsPersistent)
                {
                    if (Config.EnumsAsEnums)
                    {
                        if (apTarget.Type.IsToMany())
                        {
                            checkSourceNull = true;
                            getter = $@"{sourceName}.{getterName}().stream().collect({collector})";
                            imports.Add("java.util.stream.Collectors");
                        }
                        else
                        {
                            getter = $@"{sourceName}.{getterName}()";
                            imports.Add(apTarget.Association.GetImport(Config, tag));
                            checkSourceNull = false;
                        }
                    }
                    else
                    {
                        checkSourceNull = true;
                        if (apTarget.Type.IsToMany())
                        {
                            getter = $@"{sourceName}.{getterName}().stream().map({apTarget.Association.NamePascal}::new).collect({collector})";
                            imports.Add("java.util.stream.Collectors");
                        }
                        else
                        {
                            getter = $"new {apTarget.Association.NamePascal}({sourceName}.{getterName}())";
                            imports.Add(apTarget.Association.GetImport(Config, tag));
                        }
                    }
                }
                else
                {
                    getter = $@"{sourceName}.{getterName}()";
                }
            }
            else if (propertyTarget.Class.IsPersistent && propertySource.Class.IsPersistent)
            {
                getter = $"{sourceName}.{getterName}()";
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
                        checkSourceNull = !propertySource.Class.IsPersistent;
                        getter = $@"{sourceName}.{getterName}(){(!propertySource.Class.IsPersistent ? $".stream().map(src -> {Config.GetMapperName(cpMapperNs, cpMapperModelPath)}.{cpMapper.Name.ToCamelCase()}(src, null)).collect({collector})" : string.Empty)}";
                        imports.Add("java.util.stream.Collectors");
                    }
                    else
                    {
                        checkSourceNull = true;
                        getter = $"{Config.GetMapperName(cpMapperNs, cpMapperModelPath)}.{cpMapper.Name.Value.ToCamelCase()}({sourceName}.{getterName}(), target.get{apTarget.NameByClassPascal}())";
                        imports.Add(Config.GetMapperImport(cpMapperNs, cpMapperModelPath, tag)!);
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
            getter = $"{sourceName}.{getterName}()";
        }

        return (Getter: Config.GetConvertedValue(
                getter,
                propertySource.Domain,
                propertyTarget.Domain), CheckSourceNull: checkSourceNull, Imports: imports);
    }

    protected virtual JavaMethod GetToMapperMethodNoTarget(Class classe, ClassMappings mapper, string tag)
    {
        var toMapperMethod = new JavaMethod(mapper.Class.NamePascal, mapper.Name.Value.ToCamelCase())
        {
            Visibility = "public",
            Static = true,
            Comment = $"Mappe '{mapper.Class.NamePascal}' vers une nouvelle instance de '{classe}'",
            ReturnComment = $"Nouvelle instance de '{classe}' mappée depuis '{mapper.Class.NameCamel}'"
        }
       .AddParameter(new JavaMethodParameter(classe.GetImport(Config, tag), classe.NamePascal, "source")
       {
           Comment = $"Instance de '{classe.NamePascal}' à mapper"
       });

        toMapperMethod.AddBodyLine(1, $"return {mapper.Name.Value.ToCamelCase()}(source, new {mapper.Class.NamePascal}());");
        return toMapperMethod;
    }

    protected virtual JavaMethod GetToMapperMethodWithTarget(Class classe, ClassMappings mapper, string tag)
    {
        var toMapperMethod = GetToMapperMethodNoTarget(classe, mapper, tag);
        toMapperMethod.AddParameter(new JavaMethodParameter(mapper.Class.GetImport(Config, tag), mapper.Class.NamePascal, "target")
        {
            Comment = $"Instance de '{mapper.Class.NamePascal}' sur laquelle mapper"
        });

        toMapperMethod.Body.Clear();
        toMapperMethod.Comment = $"Mappe '{mapper.Class.NamePascal}' vers une nouvelle instance ou bien sur l'instance passée en paramètres";
        toMapperMethod.ReturnComment = $"Nouvelle instance ou bien l'instance passée en paramètres mappée depuis '{mapper.Class.NameCamel}'";
        toMapperMethod.AddBodyLine("if (source == null) {");
        toMapperMethod.AddBodyLine(1, $"throw new IllegalArgumentException(\"source cannot be null\");");
        toMapperMethod.AddBodyLine("}");
        toMapperMethod.AddBodyLine();
        toMapperMethod.AddBodyLine("if (target == null) {");
        toMapperMethod.AddBodyLine(1, $"throw new IllegalArgumentException(\"target cannot be null\");");
        toMapperMethod.AddBodyLine("}");
        toMapperMethod.AddBodyLine();
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
            var (getter, checkSourceNull, imports) = GetSourceGetter(propertySource, propertyTarget!, classe, "source", tag);
            toMapperMethod.Imports.AddRange(imports);
            var propertyTargetName = Config.UseJdbc || propertyTarget is AssociationProperty asp && !asp.Association.IsPersistent ? propertyTarget!.NamePascal : propertyTarget!.NameByClassPascal;
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
                        toMapperMethod.AddBodyLine($"if (source.{propertySource.NameByClassPascal.WithPrefix(getterPrefix)}() != null) {{");
                    }

                    toMapperMethod.AddBodyLine(checkSourceNull ? 1 : 0, $"target.{JpaModelPropertyGenerator.GetSetterName(propertyTarget)}({getter});");

                    if (checkSourceNull)
                    {
                        toMapperMethod.AddBodyLine($"}}");
                        toMapperMethod.AddBodyLine();
                    }
                }
            }
        }

        if (mapper.Class.Abstract)
        {
            hydrate += ");";
            toMapperMethod.AddBodyLine(hydrate);
        }

        toMapperMethod.AddBodyLine("return target;");
        return toMapperMethod;
    }

    protected override void HandleFile(string fileName, string tag, IList<(Class Classe, FromMapper Mapper)> fromMappers, IList<(Class Classe, ClassMappings Mapper)> toMappers)
    {
        var sampleFromMapper = fromMappers.FirstOrDefault();
        var sampleToMapper = toMappers.FirstOrDefault();

        var (mapperNs, modelPath) = sampleFromMapper != default
            ? Config.GetMapperLocation(sampleFromMapper)
            : Config.GetMapperLocation(sampleToMapper);

        var package = Config.GetPackageName(mapperNs, modelPath, GetBestClassTag(sampleFromMapper.Classe ?? sampleToMapper.Classe, tag));

        using var fw = this.OpenJavaWriter(fileName, package, null);

        var imports = fromMappers.SelectMany(m => m.Mapper.ClassParams.Select(p => p.Class).Concat([m.Classe]))
            .Concat(toMappers.SelectMany(m => new[] { m.Classe, m.Mapper.Class }))
            .Where(c => Classes.Contains(c))
            .Select(c => c.GetImport(Config, c.Tags.Contains(tag) ? tag : c.Tags.Intersect(Config.Tags).First()))
            .Distinct()
            .ToArray();

        fw.AddImports(imports);
        fw.WriteLine();
        if (Config.GeneratedHint)
        {
            fw.WriteLine(0, Config.GeneratedAnnotation);
        }

        fw.WriteLine($@"public class {Config.GetMapperName(mapperNs, modelPath)} {{");

        fw.WriteLine();
        fw.WriteLine(1, $@"private {Config.GetMapperName(mapperNs, modelPath)}() {{");
        fw.WriteLine(2, "// private constructor to hide implicite public one");
        fw.WriteLine(1, "}");

        foreach (var (classe1, mapper) in fromMappers)
        {
            WriteFromMappers(classe1, mapper, fw, GetBestClassTag(classe1, tag));
        }

        foreach (var (classe, mapper1) in toMappers)
        {
            WriteToMapper(classe, mapper1, fw, GetBestClassTag(classe, tag));
        }

        fw.WriteLine("}");
    }

    protected virtual bool UseClassForAssociation(IProperty p, Class classe) => classe.IsPersistent && !Config.UseJdbc && p is AssociationProperty ap && ap.Association.IsPersistent;

    protected virtual void WriteFromMappers(Class classe, FromMapper mapper, JavaWriter fw, string tag)
    {
        if (Config.CanClassUseEnums(classe, Classes))
        {
            _logger.LogWarning($"La classe {classe.Name} ne peut pas être mappée car c'est une enum");
            return;
        }

        fw.WriteLine();
        fw.Write(1, GetFromMapperNoTarget(classe, mapper, tag));
        fw.WriteLine();
        fw.Write(1, GetFromMapperWithTarget(classe, mapper, tag));
    }

    protected virtual void WriteToMapper(Class classe, ClassMappings mapper, JavaWriter fw, string tag)
    {
        if (Config.CanClassUseEnums(mapper.Class, Classes))
        {
            _logger.LogWarning($"La classe {mapper.Class.Name} ne peut pas être mappée car c'est une enum");
            return;
        }

        fw.WriteLine();
        fw.Write(1, GetToMapperMethodNoTarget(classe, mapper, tag));
        fw.WriteLine();
        fw.Write(1, GetToMapperMethodWithTarget(classe, mapper, tag));
    }
}