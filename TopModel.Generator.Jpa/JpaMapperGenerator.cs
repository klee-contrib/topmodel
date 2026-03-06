using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using TopModel.Generator.Jpa.ClassGeneration.Utils;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JpaMapperGenerator(ILogger<JpaMapperGenerator> logger, IFileWriterProvider writerProvider)
    : MapperGeneratorBase<JpaConfig>(logger, writerProvider)
{
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

        foreach (var param in mapper.ClassParams.Where(p => p.Mappings.Count > 0))
        {
            if (param.Required)
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
            if (param.Property.Required)
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
                    param.Name.ToCamelCase(),
                    tag
                );
                fromMapperMethod.Imports.AddRange(imports);

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

            fromMapperMethod.AddBodyLine(
                $"target.{JpaModelPropertyGenerator.GetSetterName(param.TargetProperty)}({param.Property.NameCamel});"
            );
        }

        fromMapperMethod.AddBodyLine("return target;");

        return fromMapperMethod;
    }

    protected virtual IEnumerable<JavaMethod> GetFromMappers(Class classe, FromMapper mapper, string tag)
    {
        if (!classe.Abstract)
        {
            yield return GetFromMapperNoTarget(classe, mapper, tag);
        }

        yield return GetFromMapperWithTarget(classe, mapper, tag);
    }

    protected virtual (string Getter, bool CheckSourceNull, IEnumerable<string> Imports) GetSourceGetter(
        IProperty source,
        IProperty target,
        string paramName,
        string tag
    )
    {
        var imports = new List<string>();
        var checkSourceNull = false;
        var getter = $"{paramName}.{JpaModelPropertyGenerator.GetGetterName(source)}()";

        var converter = source.Domain.GetConverter(target.Domain);
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
            return (
                Getter: Config.GetConvertedValue(getter, source.Domain, target.Domain),
                CheckSourceNull: false,
                Imports: imports
            );
        }

        string GetMappedValue(string value, Class sourceClass, Class targetClass, IProperty? targetProperty = null)
        {
            var mapper = sourceClass.GetMapperTo(targetClass)!.Value;
            return mapper.Match(
                fromMapper =>
                {
                    var (targetMapperNs, targetMapperModelPath) = Config.GetMapperLocation((targetClass, fromMapper));
                    imports.Add(Config.GetMapperImport(targetMapperNs, targetMapperModelPath, tag)!);

                    var createMapper =
                        $"{Config.GetMapperName(targetMapperNs, targetMapperModelPath)}.create{targetClass.NamePascal}({value})";
                    if (targetProperty != null)
                    {
                        var targetGetter = $"target.{JpaModelPropertyGenerator.GetGetterName(targetProperty)}()";
                        return $"{targetGetter} != null ? {Config.GetMapperName(targetMapperNs, targetMapperModelPath)}.map{targetClass.NamePascal}({value}, {targetGetter}) : {createMapper}";
                    }
                    else
                    {
                        return createMapper;
                    }
                },
                toMapper =>
                {
                    var (targetMapperNs, targetMapperModelPath) = Config.GetMapperLocation((sourceClass, toMapper));
                    imports.Add(Config.GetMapperImport(targetMapperNs, targetMapperModelPath, tag)!);
                    var mapperTo =
                        $"{Config.GetMapperName(targetMapperNs, targetMapperModelPath)}.to{targetClass.NamePascal}";

                    if (targetProperty != null)
                    {
                        var targetGetter = $"target.{JpaModelPropertyGenerator.GetGetterName(targetProperty)}()";
                        return $"{targetGetter} != null ? {mapperTo}({value}, {targetGetter}) : {mapperTo}({value})";
                    }
                    else
                    {
                        return $"{mapperTo}({value})";
                    }
                }
            );
        }

        if (source.MappingType.TryPickT2(out var st2, out _))
        {
            if (target.MappingType.IsT0 && st2.Property != null)
            {
                checkSourceNull = true;
                getter += $".stream().filter(Objects::nonNull)";

                var sourceProp = st2.Property;
                if (st2.Property.MappingType.TryPickT1(out var t1, out _) && t1.Property != null)
                {
                    imports.Add(sourceProp.Class.GetImport(Config, tag));
                    getter +=
                        $".map({sourceProp.Class.NamePascal}::{JpaModelPropertyGenerator.GetGetterName(sourceProp)}).filter(Objects::nonNull)";
                    sourceProp = t1.Property;
                }

                imports.Add("java.util.stream.Collectors");
                imports.Add("java.util.Objects");

                var selector =
                    $"p -> {HandleConversion($"p.{JpaModelPropertyGenerator.GetGetterName(sourceProp)}()", sourceProp, target, collection: true)}";
                if (selector.StartsWith("p -> p") && selector.Count(c => c == '.') == 1)
                {
                    imports.Add(sourceProp.Class.GetImport(Config, tag));
                    selector = $"{sourceProp.Class.NamePascal}::{selector[7..^2]}";
                }

                getter += $".map({selector}).{Config.GetCollector(st2.Domain)}";
            }
            else if (target.MappingType.TryPickT2(out var tt2, out _) && st2.Class != tt2.Class)
            {
                checkSourceNull = true;
                imports.Add("java.util.stream.Collectors");
                imports.Add("java.util.Objects");

                var selector = $"p -> {GetMappedValue("p", st2.Class, tt2.Class)}";
                if (selector.EndsWith("(p)"))
                {
                    selector = selector[5..^3].Replace("Mappers.", "Mappers::");
                }

                getter += $".stream().filter(Objects::nonNull).map({selector}).{Config.GetCollector(st2.Domain)}";
            }
        }
        else if (
            source.MappingType.TryPickT1(out var st1, out _)
            && target.MappingType.TryPickT1(out var tt1, out _)
            && st1.Class != tt1.Class
        )
        {
            checkSourceNull = true;
            getter = GetMappedValue(getter, st1.Class, tt1.Class, target);
        }
        else if (source.MappingType.IsT0 && target.MappingType.TryPickT1(out var tt1rec, out _))
        {
            checkSourceNull = true;
            imports.Add(
                $"{Config.GetPackageName(tt1rec.Class, Config.GetBestClassTag(tt1rec.Class, tag))}.{tt1rec.Class.NamePascal}"
            );

            getter =
                $"new {Config.GetTypeName(tt1rec.Class)}({HandleConversion(getter, source, tt1rec.Class.EnumKey!)})";
        }
        else if (source.MappingType.IsT0 && target.MappingType.TryPickT2(out var tt2rec, out _))
        {
            checkSourceNull = true;
            imports.Add("java.util.stream.Collectors");
            imports.Add("java.util.Objects");
            imports.Add(
                $"{Config.GetPackageName(tt2rec.Class, Config.GetBestClassTag(tt2rec.Class, tag))}.{tt2rec.Class.NamePascal}"
            );

            getter +=
                $".stream().filter(Objects::nonNull).map({tt2rec.Class.NamePascal}::new).{Config.GetCollector(tt2rec.Domain)}";
        }
        else
        {
            if (source.MappingType.TryPickT1(out var t1, out _) && t1.Property != null && target.MappingType.IsT0)
            {
                checkSourceNull = true;
                source = t1.Property;
                getter += $".{JpaModelPropertyGenerator.GetGetterName(t1.Property)}()";
            }

            getter = HandleConversion(getter, source, target);
        }

        return (Getter: getter, CheckSourceNull: checkSourceNull, Imports: imports);
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

        foreach (
            var mapping in mapper
                .Mappings.Where(mapping => FilterMapping(mapping.Key, mapping.Value))
                .OrderBy(m => m.Key.Class.Properties.IndexOf(m.Key))
        )
        {
            var propertyTarget = mapping.Value;
            var propertySource = mapping.Key;
            var (getter, checkSourceNull, imports) = GetSourceGetter(propertySource, propertyTarget!, "source", tag);
            toMapperMethod.Imports.AddRange(imports);

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

        toMapperMethod.AddBodyLine("return target;");
        return toMapperMethod;
    }

    protected virtual IEnumerable<JavaMethod> GetToMappers(Class classe, ClassMappings mapper, string tag)
    {
        if (!mapper.Class.Abstract)
        {
            yield return GetToMapperMethodNoTarget(classe, mapper, tag);
        }

        yield return GetToMapperMethodWithTarget(classe, mapper, tag);
    }

    protected virtual string HandleConversion(string value, IProperty source, IProperty target, bool collection = false)
    {
        return Config.GetConvertedValue(value, source.Domain, target.Domain);
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
