using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.Model;
using TopModel.Core.Utils;
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
            _jpaModelPropertyGenerator ??= Config.UseJdbc
                ? new JdbcModelPropertyGenerator(Config, new Dictionary<string, string>())
                : new JpaModelPropertyGenerator(Config, new Dictionary<string, string>());
            return _jpaModelPropertyGenerator;
        }
    }

    protected override string GetFileName((Class Classe, FromMapper Mapper) mapper, string tag)
    {
        return Config.GetMapperFilePath(mapper, Config.GetBestClassTag(mapper.Classe, tag));
    }

    protected override string GetFileName((Class Classe, ClassMappings Mapper) mapper, string tag)
    {
        return Config.GetMapperFilePath(mapper, Config.GetBestClassTag(mapper.Classe, tag));
    }

    protected virtual JavaMethod GetFromMapperNoTarget(Class classe, FromMapper mapper, string tag)
    {
        var fromMapperMethod = new JavaMethod(classe.NamePascal, $"create{classe.NamePascal}")
        {
            Static = true,
            Visibility = "public",
            ReturnComment =
                $"Une nouvelle instance de '{classe.NamePascal}' sur laquelle les champs sources ont été mappés",
            Comment = $"Crée une nouvelle instance de la classe '{classe.NamePascal}' en mappant les champs sources.",
        };

        fromMapperMethod.AddParameters(GetFromMappersParameters(classe, mapper, tag));

        fromMapperMethod.AddBodyLine(
            $"return map{classe.NamePascal}({string.Join(", ", fromMapperMethod.Parameters.Select(p => p.Name))}, new {classe.NamePascal}());"
        );
        return fromMapperMethod;
    }

    protected virtual JavaMethod GetFromMapperWithTarget(Class classe, FromMapper mapper, string tag)
    {
        var fromMapperMethod = new JavaMethod(classe.NamePascal, $"map{classe.NamePascal}")
        {
            Static = true,
            Visibility = "public",
            ReturnComment =
                $"L'instance de '{classe.NamePascal}' passée en paramètres sur lesquels les champs sources ont été mappés",
            Comment = $"Mappe les champs sources sur l'instance de la classe '{classe.NamePascal}' passée en paramètre",
        };

        fromMapperMethod.AddParameters(GetFromMappersParameters(classe, mapper, tag));
        fromMapperMethod.AddParameter(
            new JavaMethodParameter(classe.NamePascal, "target")
            {
                Comment = $"Instance de '{classe.NamePascal}' cible",
            }
        );

        fromMapperMethod.AddBodyLine("if (target == null) {");
        fromMapperMethod.AddBodyLine(1, $"throw new IllegalArgumentException(\"target cannot be null\");");
        fromMapperMethod.AddBodyLine("}");

        fromMapperMethod.AddBodyLine();

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
                fromMapperMethod.AddBodyLine($"if ({param.Name.ToCamelCase()} == null) {{");
                fromMapperMethod.AddBodyLine(
                    1,
                    $"throw new IllegalArgumentException(\"{param.Name} cannot be null\");"
                );
                fromMapperMethod.AddBodyLine("}");
                fromMapperMethod.AddBodyLine();
            }
        }

        foreach (var param in mapper.PropertyParams)
        {
            if (param.Property.Required && !classe.Abstract)
            {
                if (param.TargetProperty is { Association.IsPersistent: true } && classe.IsPersistent)
                {
                    continue;
                }

                fromMapperMethod.AddBodyLine($"if ({param.Property.NameCamel} == null) {{");
                fromMapperMethod.AddBodyLine(
                    1,
                    $"throw new IllegalArgumentException(\"{param.Property.NameCamel} cannot be null\");"
                );
                fromMapperMethod.AddBodyLine("}");
                fromMapperMethod.AddBodyLine();
            }
        }

        foreach (var param in mapper.ClassParams.Where(p => p.Mappings.Count > 0))
        {
            var mappings = param.Mappings.ToList();
            var indent = 0;
            if (!param.Required)
            {
                fromMapperMethod.AddBodyLine(indent, $"if ({param.Name.ToCamelCase()} != null) {{");
                indent++;
            }

            foreach (var mapping in mappings.Where(mapping => FilterMapping(mapping.Key, mapping.Value)))
            {
                var propertyTarget = mapping.Key;
                var propertySource = mapping.Value;
                var (getter, checkSourceNull, imports) = GetSourceGetter(
                    propertySource,
                    propertyTarget,
                    classe,
                    param.Name.ToCamelCase(),
                    tag
                );
                fromMapperMethod.Imports.AddRange(imports);
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
                        hydrate +=
                            $"{param.Name}.{JpaModelPropertyGenerator.GetGetterName(propertyTarget)}() != null ? {getter} : null";
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
                            fromMapperMethod.AddBodyLine(
                                indent,
                                $"if ({param.Name}.{JpaModelPropertyGenerator.GetGetterName(propertySource)}() != null) {{"
                            );
                        }

                        fromMapperMethod.AddBodyLine(
                            indent + (checkSourceNull ? 1 : 0),
                            $"target.{JpaModelPropertyGenerator.GetSetterName(propertyTarget)}({getter});"
                        );

                        if (checkSourceNull)
                        {
                            fromMapperMethod.AddBodyLine(indent, $"}} else {{");
                            fromMapperMethod.AddBodyLine(
                                indent + 1,
                                $"target.{JpaModelPropertyGenerator.GetSetterName(propertyTarget)}(null);"
                            );
                            fromMapperMethod.AddBodyLine(indent, $"}}");
                            fromMapperMethod.AddBodyLine();
                        }
                    }
                }
            }

            if (!param.Required)
            {
                fromMapperMethod.AddBodyLine(indent - 1, "}");
                fromMapperMethod.AddBodyLine();
            }
        }

        foreach (var param in mapper.PropertyParams)
        {
            if (param.TargetProperty is { Association.IsPersistent: true } && classe.IsPersistent)
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
                fromMapperMethod.AddBodyLine(
                    $"target.{JpaModelPropertyGenerator.GetSetterName(param.TargetProperty)}({param.Property.NameCamel});"
                );
            }
        }

        if (classe.Abstract)
        {
            hydrate += ");";
            fromMapperMethod.AddBodyLine(1, hydrate);
        }

        fromMapperMethod.AddBodyLine("return target;");

        return fromMapperMethod;
    }

    protected virtual IEnumerable<JavaMethod> GetFromMappers(Class classe, FromMapper mapper, string tag)
    {
        if (Config.CanClassUseEnums(classe))
        {
            _logger.LogWarning($"La classe {classe.Name} ne peut pas être mappée car c'est une enum");
        }
        else
        {
            yield return GetFromMapperNoTarget(classe, mapper, tag);
            yield return GetFromMapperWithTarget(classe, mapper, tag);
        }
    }

    protected virtual (string Getter, bool CheckSourceNull, IEnumerable<string> Imports) GetSourceGetter(
        IProperty propertySource,
        IProperty propertyTarget,
        Class classe,
        string sourceName,
        string tag
    )
    {
        var getter = string.Empty;
        var imports = new List<string>();
        var getterName = JpaModelPropertyGenerator.GetGetterName(propertySource);
        var converter = propertySource.Domain.GetConverter(propertyTarget.Domain);
        var targetType = Config.GetType(propertyTarget);
        var collector = $"Collectors.to{targetType.Split('<')[0]}()";
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
            return (
                Getter: Config.GetConvertedValue(getter, propertySource.Domain, propertyTarget.Domain),
                CheckSourceNull: false,
                Imports: imports
            );
        }

        var checkSourceNull = false;
        if (
            (propertySource.Class.IsPersistent == propertyTarget.Class.IsPersistent)
            || propertySource.Association == null && propertyTarget.Association == null
        )
        {
            getter = $"{sourceName}.{getterName}()";
        }
        else if (
            propertySource.Class.IsPersistent
            && (!propertyTarget.Class.IsPersistent || propertyTarget.Association == null)
            && (
                propertySource is
                { Association: { IsPersistent: true } aSource, AssociationProperty: IProperty apSource }
            )
        )
        {
            checkSourceNull = true;
            if (propertyTarget is { Composition: Class cpc })
            {
                if (propertySource.Class.ToMappers.Any(t => t.Class == cpc))
                {
                    var cpMapper = propertySource.Class.ToMappers.Single(t => t.Class == cpc)!;
                    var (cpMapperNs, cpMapperModelPath) = Config.GetMapperLocation((cpMapper.Class, cpMapper));

                    getter =
                        $"{Config.GetMapperName(cpMapperNs, cpMapperModelPath)}.{cpMapper.Name.Value.ToCamelCase()}({sourceName}.{getterName}(), target.{JpaModelPropertyGenerator.GetGetterName(propertySource)}())";
                    imports.Add(Config.GetMapperImport(cpMapperNs, cpMapperModelPath, tag)!);
                }
                else if (cpc.FromMappers.Any(f => f.Params.Count == 1 && f.ClassParams.First().Class == aSource))
                {
                    var cpMapper = cpc.FromMappers.Single(f =>
                        f.Params.Count == 1 && f.ClassParams.First().Class == aSource
                    )!;
                    var (cpMapperNs, cpMapperModelPath) = Config.GetMapperLocation((cpc, cpMapper));

                    getter = $"{sourceName}.{getterName}()";
                    var mapperName = Config.GetMapperName(cpMapperNs, cpMapperModelPath);
                    var target = $"target.{JpaModelPropertyGenerator.GetGetterName(propertyTarget)}()";
                    getter =
                        $"{target} != null ? {mapperName}.map{cpc}({getter}, {target}) : {mapperName}.create{cpc}({getter})";

                    imports.Add(Config.GetMapperImport(cpMapperNs, cpMapperModelPath, tag)!);
                }
                else
                {
                    throw new ModelException(
                        classe,
                        $"La propriété {propertySource.Name} ne peut pas être mappée avec la propriété {propertyTarget.Name} car il n'existe pas de mapper {cpc.Name} -> {aSource.Name}"
                    );
                }
            }
            else
            {
                if (!propertySource.AssociationMultiple)
                {
                    if (Config.EnumsAsEnums && Config.CanClassUseEnums(aSource, apSource))
                    {
                        getter = $"{sourceName}.{getterName}()";
                        checkSourceNull = false;
                    }
                    else
                    {
                        getter = $"{sourceName}.{getterName}().{JpaModelPropertyGenerator.GetGetterName(apSource)}()";
                    }
                }
                else
                {
                    checkSourceNull = true;
                    imports.Add("java.util.stream.Collectors");
                    imports.Add("java.util.Objects");
                    if (Config.EnumsAsEnums && Config.CanClassUseEnums(aSource, apSource))
                    {
                        getter = $"{sourceName}.{getterName}().stream().filter(Objects::nonNull).collect({collector})";
                    }
                    else if (aSource.PrimaryKey.Count() == 1)
                    {
                        var targetGetterName =
                            JpaModelPropertyGenerator
                                .GetMapIdPropertyGetter(aSource, Config.GetBestClassTag(aSource, tag))
                                ?.Name ?? JpaModelPropertyGenerator.GetGetterName(apSource);

                        getter =
                            $"{sourceName}.{getterName}().stream().filter(Objects::nonNull).map({aSource.NamePascal}::{targetGetterName}).collect({collector})";
                        imports.Add(aSource.GetImport(Config, tag));
                    }
                    else if (aSource.PrimaryKey.Count() > 1)
                    {
                        throw new ModelException(
                            classe,
                            $"La propriété {propertySource.Name} ne peut pas être mappée avec la propriété {propertyTarget.Name} car la classe cible de l'association possède une clé composite"
                        );
                    }
                }
            }
        }
        else if (
            (!propertySource.Class.IsPersistent || propertySource.Association == null)
            && propertyTarget.Class.IsPersistent
            && (
                propertyTarget is
                { Association: { IsPersistent: true } aTarget, AssociationProperty: IProperty apTarget }
            )
        )
        {
            if (Config.CanClassUseEnums(apTarget.Class))
            {
                if (!propertySource.Class.IsPersistent)
                {
                    if (Config.EnumsAsEnums)
                    {
                        getter = $@"{sourceName}.{getterName}()";
                        imports.Add(aTarget.GetImport(Config, tag));
                        checkSourceNull = false;
                    }
                    else
                    {
                        checkSourceNull = true;
                        getter = $"new {aTarget.NamePascal}({sourceName}.{getterName}())";
                        imports.Add(aTarget.GetImport(Config, tag));
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
            else if (propertySource is { Composition: Class cpc })
            {
                if (cpc.ToMappers.Any(t => t.Class == aTarget))
                {
                    var cpMapper = cpc.ToMappers.Single(t => t.Class == aTarget);
                    var (cpMapperNs, cpMapperModelPath) = Config.GetMapperLocation((cpMapper.Class, cpMapper));

                    checkSourceNull = true;
                    getter =
                        $"{Config.GetMapperName(cpMapperNs, cpMapperModelPath)}.{cpMapper.Name.Value.ToCamelCase()}({sourceName}.{getterName}(), target.{JpaModelPropertyGenerator.GetGetterName(apTarget)}())";
                    imports.Add(Config.GetMapperImport(cpMapperNs, cpMapperModelPath, tag)!);
                }
                else
                {
                    throw new ModelException(
                        classe,
                        $"La propriété {propertySource.Name} ne peut pas être mappée avec la propriété {propertyTarget.Name} car il n'existe pas de mapper {cpc.Name} -> {aTarget.Name}"
                    );
                }
            }
        }
        else
        {
            getter = $"{sourceName}.{getterName}()";
        }

        return (
            Getter: Config.GetConvertedValue(getter, propertySource.Domain, propertyTarget.Domain),
            CheckSourceNull: checkSourceNull,
            Imports: imports
        );
    }

    protected virtual JavaMethod GetToMapperMethodNoTarget(Class classe, ClassMappings mapper, string tag)
    {
        var toMapperMethod = new JavaMethod(mapper.Class.NamePascal, mapper.Name.Value.ToCamelCase())
        {
            Visibility = "public",
            Static = true,
            Comment = $"Mappe '{mapper.Class.NamePascal}' vers une nouvelle instance de '{classe}'",
            ReturnComment = $"Nouvelle instance de '{classe}' mappée depuis '{mapper.Class.NameCamel}'",
        }.AddParameter(
            new JavaMethodParameter(classe.GetImport(Config, tag), classe.NamePascal, "source")
            {
                Comment = $"Instance de '{classe.NamePascal}' à mapper",
            }
        );

        toMapperMethod.AddBodyLine(
            1,
            $"return {mapper.Name.Value.ToCamelCase()}(source, new {mapper.Class.NamePascal}());"
        );
        return toMapperMethod;
    }

    protected virtual JavaMethod GetToMapperMethodWithTarget(Class classe, ClassMappings mapper, string tag)
    {
        var toMapperMethod = GetToMapperMethodNoTarget(classe, mapper, tag);
        toMapperMethod.AddParameter(
            new JavaMethodParameter(mapper.Class.GetImport(Config, tag), mapper.Class.NamePascal, "target")
            {
                Comment = $"Instance de '{mapper.Class.NamePascal}' sur laquelle mapper",
            }
        );

        toMapperMethod.Body.Clear();
        toMapperMethod.Comment =
            $"Mappe '{mapper.Class.NamePascal}' vers une nouvelle instance ou bien sur l'instance passée en paramètres";
        toMapperMethod.ReturnComment =
            $"Nouvelle instance ou bien l'instance passée en paramètres mappée depuis '{mapper.Class.NameCamel}'";
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
        foreach (
            var mapping in mapper
                .Mappings.Where(mapping => FilterMapping(mapping.Key, mapping.Value))
                .OrderBy(m => m.Key.Class.Properties.IndexOf(m.Key))
        )
        {
            var propertyTarget = mapping.Value;
            var propertySource = mapping.Key;
            var (getter, checkSourceNull, imports) = GetSourceGetter(
                propertySource,
                propertyTarget!,
                classe,
                "source",
                tag
            );
            toMapperMethod.Imports.AddRange(imports);
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
                    hydrate +=
                        $"source.{JpaModelPropertyGenerator.GetGetterName(propertyTarget)}() != null ? {getter} : null";
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
                        toMapperMethod.AddBodyLine(
                            $"if (source.{JpaModelPropertyGenerator.GetGetterName(propertySource)}() != null) {{"
                        );
                    }

                    toMapperMethod.AddBodyLine(
                        checkSourceNull ? 1 : 0,
                        $"target.{JpaModelPropertyGenerator.GetSetterName(propertyTarget)}({getter});"
                    );

                    if (checkSourceNull)
                    {
                        toMapperMethod.AddBodyLine($"}} else {{");
                        toMapperMethod.AddBodyLine(
                            1,
                            $"target.{JpaModelPropertyGenerator.GetSetterName(propertyTarget)}(null);"
                        );
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

    protected virtual IEnumerable<JavaMethod> GetToMappers(Class classe, ClassMappings mapper, string tag)
    {
        if (Config.CanClassUseEnums(mapper.Class))
        {
            _logger.LogWarning($"La classe {mapper.Class.Name} ne peut pas être mappée car c'est une enum");
        }
        else
        {
            yield return GetToMapperMethodNoTarget(classe, mapper, tag);
            yield return GetToMapperMethodWithTarget(classe, mapper, tag);
        }
    }

    protected override void HandleFile(
        string fileName,
        string tag,
        IList<(Class Classe, FromMapper Mapper)> fromMappers,
        IList<(Class Classe, ClassMappings Mapper)> toMappers
    )
    {
        var mapperClass = GetMapperClass(tag, fromMappers, toMappers);
        using var fw = this.OpenJavaWriter(fileName, mapperClass.Package ?? "", codePage: null);
        fw.Write(0, mapperClass);
    }

    private bool FilterMapping(IProperty propertySource, IProperty propertyTarget)
    {
        return !(
            Config.UseJdbc
            && (
                propertyTarget.Class.IsPersistent && propertyTarget.AssociationMultiple
                || propertySource.Class.IsPersistent && propertySource.AssociationMultiple
                || propertySource.Composition != null
                || propertyTarget.Composition != null
            )
        );
    }

    private IEnumerable<JavaMethodParameter> GetFromMappersParameters(Class classe, FromMapper mapper, string tag)
    {
        foreach (var param in mapper.ClassParams)
        {
            var methodParameter = new JavaMethodParameter(param.Class.NamePascal, param.Name.ToCamelCase())
            {
                Comment = param.Comment ?? $"Instance de '{param.Class.NamePascal}' source",
            };
            methodParameter.Imports.Add(param.Class.GetImport(Config, tag));
            methodParameter.Imports.Add(classe.GetImport(Config, tag));
            yield return methodParameter;
        }

        foreach (var param in mapper.PropertyParams)
        {
            var methodParameter = new JavaMethodParameter(Config.GetType(param.Property), param.Property.NameCamel)
            {
                Comment = param.Property.Comment,
            };
            methodParameter.Imports.AddRange(param.Property.GetTypeImports(Config, tag));
            yield return methodParameter;
        }
    }

    private JavaClass GetMapperClass(
        string tag,
        IList<(Class Classe, FromMapper Mapper)> fromMappers,
        IList<(Class Classe, ClassMappings Mapper)> toMappers
    )
    {
        var sampleFromMapper = fromMappers.FirstOrDefault();
        var sampleToMapper = toMappers.FirstOrDefault();

        var (mapperNs, modelPath) =
            sampleFromMapper != default
                ? Config.GetMapperLocation(sampleFromMapper)
                : Config.GetMapperLocation(sampleToMapper);

        var package = Config.GetPackageName(
            mapperNs,
            modelPath,
            Config.GetBestClassTag(sampleFromMapper.Classe ?? sampleToMapper.Classe, tag)
        );
        var mapperClass = new JavaClass(Config.GetMapperName(mapperNs, modelPath))
        {
            Package = package,
            Visibility = "public",
        };
        if (Config.GeneratedHint)
        {
            mapperClass.Add(Config.GeneratedAnnotation);
        }

        var emptyConstructor = new JavaConstructor(Config.GetMapperName(mapperNs, modelPath))
        {
            Visibility = "private",
        };
        emptyConstructor.AddBodyLine("// private constructor to hide implicite public one");

        mapperClass.Add(emptyConstructor);
        foreach (var (classe1, mapper) in fromMappers)
        {
            mapperClass.AddRange(GetFromMappers(classe1, mapper, Config.GetBestClassTag(classe1, tag)));
        }

        foreach (var (classe, mapper1) in toMappers)
        {
            mapperClass.AddRange(GetToMappers(classe, mapper1, Config.GetBestClassTag(classe, tag)));
        }

        return mapperClass;
    }
}
