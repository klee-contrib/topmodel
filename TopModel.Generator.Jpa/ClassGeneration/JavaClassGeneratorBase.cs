using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public abstract class JavaClassGeneratorBase(ILogger<JavaClassGeneratorBase> logger, IFileWriterProvider writerProvider)
    : ClassGeneratorBase<JpaConfig>(logger, writerProvider)
{
    private JavaConstructorGenerator? _jpaModelConstructorGenerator;
    private JpaModelPropertyGenerator? _jpaModelPropertyGenerator;

    protected static IDictionary<string, string> NewableTypes =>
        new Dictionary<string, string>() { ["List"] = "ArrayList", ["Set"] = "HashSet" };

    protected virtual JavaConstructorGenerator ConstructorGenerator
    {
        get
        {
            _jpaModelConstructorGenerator ??= new JavaConstructorGenerator(Config);
            return _jpaModelConstructorGenerator;
        }
    }

    protected virtual JpaModelPropertyGenerator JpaModelPropertyGenerator
    {
        get
        {
            _jpaModelPropertyGenerator ??= Config.UseJdbc
                ? new JdbcModelPropertyGenerator(Config, NewableTypes)
                : new JpaModelPropertyGenerator(Config, NewableTypes);
            return _jpaModelPropertyGenerator;
        }
    }

    protected virtual IEnumerable<JavaAnnotation> GetAnnotations(Class classe, string tag)
    {
        if (Config.GeneratedHint)
        {
            yield return Config.GeneratedAnnotation;
        }

        var annotations = Config
            .GetAnnotations(classe, tag)
            .Select(a => new JavaAnnotation(a.Annotation, imports: a.Imports.ToArray()));
        foreach (var a in annotations)
        {
            yield return a;
        }
    }

    protected virtual IEnumerable<JavaMethod> GetConstuctors(Class classe, string tag)
    {
        if (
            Config.MappersInClass
            && classe.FromMappers.Any(c => c.ClassParams.All(p => Config.AvailableClasses.Contains(p.Class)))
        )
        {
            yield return ConstructorGenerator.GetNoArgConstructor(classe, tag);
        }

        if (Config.MappersInClass)
        {
            foreach (var constructor in ConstructorGenerator.GetFromMappers(classe, tag))
            {
                yield return constructor;
            }
        }
    }

    protected virtual IEnumerable<JavaField> GetFields(Class classe, string tag)
    {
        return JpaModelPropertyGenerator.GetFields(classe, tag);
    }

    protected virtual JavaEnum GetFieldsEnum(Class classe, string tag)
    {
        var javaEnum = new JavaEnum("Fields")
        {
            Comment =
                $"Enumération des champs de la classe {{@link {classe.GetImport(Config, tag)} {classe.NamePascal}}}",
        };

        if (Config.FieldsEnumInterface != null)
        {
            javaEnum.Imports.Add(Config.FieldsEnumInterface.Replace("<>", string.Empty));
        }

        if (Config.FieldsEnumInterface != null)
        {
            javaEnum.Implements.Add(
                $" implements {Config.FieldsEnumInterface.Split(".")[^1].Replace("<>", $"<{classe.NamePascal}>")}"
            );
        }

        var enumValues = GetFieldsEnumValues(classe, tag);
        javaEnum.AddRange(enumValues);
        var classField = new JavaField("Class<?>", "type") { Final = true };
        javaEnum.Add(new JavaField("Class<?>", "type") { Final = true });
        javaEnum.Add(
            new JavaMethod(
                classField.Type,
                classField.Name.ToPascalCase().WithPrefix(classField.Type == "boolean" ? "is" : "get")
            )
            {
                Comment = $"Getter for {classField.Name}",
                Body =
                {
                    new WriterLine() { Line = $"return this.{classField.Name};", Indent = 0 },
                },
                ReturnComment = $"value of {{@link #{classField.Name} {classField.Name}}}",
                Visibility = "public",
            }
        );
        javaEnum.Constructors.Add(javaEnum.GetAllArgsConstructor());
        return javaEnum;
    }

    protected virtual IEnumerable<JavaEnumValue> GetFieldsEnumValues(Class classe, string tag)
    {
        return JpaModelPropertyGenerator
            .GetAvailableProperties(classe)
            .Select(prop =>
            {
                string name = JpaModelPropertyGenerator.GetPropertyName(prop).ToConstantCase();
                var javaType = JpaModelPropertyGenerator.GetPropertyType(prop);
                javaType = javaType.Split("<")[0];
                return new JavaEnumValue(name)
                {
                    Parameters = { $"{javaType}.class" },
                    Imports = prop.GetTypeImports(Config, tag).ToList(),
                };
            });
    }

    protected virtual IEnumerable<JavaMethod> GetGetters(Class classe, string tag)
    {
        if (!(Config.HasAnnotation(classe, "Getter") || Config.HasAnnotation(classe, "Data")))
        {
            foreach (var property in JpaModelPropertyGenerator.GetAvailableProperties(classe))
            {
                if (!Config.HasAnnotation(property, "Getter") || Config.HasAnnotation(classe, "Data"))
                {
                    yield return JpaModelPropertyGenerator.GetGetter(tag, property);
                }
            }
        }
    }

    protected virtual IEnumerable<JavaClass> GetInnerClasses(Class classe, string tag)
    {
        return [];
    }

    protected virtual IEnumerable<JavaMethod> GetMethods(Class classe, string tag)
    {
        foreach (var method in GetGetters(classe, tag))
        {
            yield return method;
        }
        foreach (var method in GetSetters(classe, tag))
        {
            yield return method;
        }
        if (Config.MappersInClass)
        {
            foreach (var method in GetToMappers(classe, tag))
            {
                yield return method;
            }
        }
    }

    protected virtual IEnumerable<JavaMethod> GetSetters(Class classe, string tag)
    {
        if (!(Config.HasAnnotation(classe, "Setter") || Config.HasAnnotation(classe, "Data")))
        {
            foreach (var property in JpaModelPropertyGenerator.GetAvailableProperties(classe))
            {
                if (!(Config.HasAnnotation(property, "Setter") || Config.HasAnnotation(classe, "Data")))
                {
                    yield return JpaModelPropertyGenerator.GetSetter(tag, property);
                }
            }
        }
    }

    protected virtual IEnumerable<JavaMethod> GetToMappers(Class classe, string tag)
    {
        var toMappers = classe
            .ToMappers.Where(p => Config.AvailableClasses.Contains(p.Class))
            .Select(m => (classe, m))
            .OrderBy(m => m.m.Name)
            .ToList();

        foreach (var toMapper in toMappers)
        {
            var (_, mapper) = toMapper;
            var method = new JavaMethod(mapper.Class.NamePascal, mapper.Name.Value.ToCamelCase())
            {
                Visibility = "public",
                Comment = $"Mappe '{classe}' vers '{mapper.Class.NamePascal}'",
            };
            method.Imports.Add(mapper.Class.GetImport(Config, tag));
            if (mapper.Comment != null)
            {
                method.Comment += $"{mapper.Comment}";
            }

            method.AddParameter(
                new JavaMethodParameter(mapper.Class.NamePascal, "target")
                {
                    Comment =
                        $"Instance pré-existante de '{mapper.Class.NamePascal}'. Une nouvelle instance sera créée si non spécifié.",
                }
            );
            method.ReturnComment = $"Une instance de '{mapper.Class.NamePascal}'";
            var (mapperNs, mapperModelPath) = Config.GetMapperLocation(toMapper);
            method.AddBodyLine(
                @$"return {Config.GetMapperName(mapperNs, mapperModelPath)}.{mapper.Name.Value.ToCamelCase()}(this, target);"
            );
            method.Imports.Add(Config.GetMapperImport(mapperNs, mapperModelPath, tag));
            yield return method;
        }
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        var packageName = Config.GetPackageName(classe, tag);

        var javaClass = InitClass(classe, tag);

        using var fw = this.OpenJavaWriter(fileName, packageName, codePage: null);
        fw.Write(0, javaClass);
    }

    protected virtual JavaClass InitClass(Class classe, string tag)
    {
        var javaClass = new JavaClass(classe.NamePascal) { Comment = classe.Comment };
        javaClass.AddRange(GetAnnotations(classe, tag));
        var extends = Config.GetClassExtends(classe, tag);
        if (classe.Extends is not null)
        {
            javaClass.Imports.Add(classe.Extends.GetImport(Config, Config.GetBestClassTag(classe.Extends, tag)));
        }

        javaClass.Extends = extends;
        var implements = Config.GetClassImplements(classe, tag).ToList();
        javaClass.Implements.AddRange(implements);
        javaClass.Imports.AddRange(Config.GetDecoratorImports(classe, tag));
        javaClass.AddRange(GetConstuctors(classe, tag));
        javaClass.AddRange(GetFields(classe, tag));
        javaClass.AddRange(GetMethods(classe, tag));
        javaClass.AddRange(GetInnerClasses(classe, tag));

        return javaClass;
    }
}
