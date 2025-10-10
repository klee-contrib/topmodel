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

    protected string JavaxOrJakarta => Config.JavaxOrJakarta;

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
                ? new JdbcModelPropertyGenerator(Config, Classes, NewableTypes)
                : new JpaModelPropertyGenerator(Config, Classes, NewableTypes);
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

    protected virtual JavaClass InitClass(Class classe, string tag)
    {
        var packageName = Config.GetPackageName(classe, tag);

        var javaClass = new JavaClass(classe.NamePascal) { Comment = classe.Comment };
        javaClass.AddRange(GetAnnotations(classe, tag));
        var extends = Config.GetClassExtends(classe, tag);
        if (classe.Extends is not null)
        {
            javaClass.Extends = extends;
            javaClass.Imports.Add(classe.Extends.GetImport(Config, Config.GetBestClassTag(classe.Extends, tag)));
        }

        var implements = Config.GetClassImplements(classe, tag).ToList();
        javaClass.Implements.AddRange(implements);
        javaClass.Imports.AddRange(Config.GetDecoratorImports(classe, tag));
        javaClass.AddRange(GetConstuctors(classe, tag));
        javaClass.AddRange(GetFields(classe, tag));
        javaClass.AddRange(GetGetters(classe, tag));
        javaClass.AddRange(GetSetters(classe, tag));
        if (Config.MappersInClass)
        {
            javaClass.AddRange(GetToMappers(classe, tag));
        }

        return javaClass;
    }

    protected virtual IEnumerable<JavaMethod> GetConstuctors(Class classe, string tag)
    {
        if (
            Config.MappersInClass && classe.FromMappers.Any(c => c.ClassParams.All(p => Classes.Contains(p.Class)))
            || Classes.Any(c => c.Extends == classe)
            || Config.GetClassExtends(classe, tag) != null
        )
        {
            yield return ConstructorGenerator.GetNoArgConstructor(classe, tag);
        }

        if (Config.MappersInClass)
        {
            foreach (var constructor in ConstructorGenerator.GetFromMappers(classe, Classes, tag))
            {
                yield return constructor;
            }
        }
    }

    protected virtual IEnumerable<JavaField> GetFields(Class classe, string tag)
    {
        return JpaModelPropertyGenerator.GetProperties(classe, tag);
    }

    protected virtual IEnumerable<JavaMethod> GetGetters(Class classe, string tag)
    {
        if (!Config.HasAnnotation(classe, "Getter"))
        {
            foreach (var property in classe.Properties)
            {
                if (!Config.HasAnnotation(property, "Getter"))
                {
                    yield return JpaModelPropertyGenerator.GetGetter(tag, property);
                }
            }
        }
    }

    protected virtual JavaMethod? GetMapIdPropertySetter(Class classe, string tag)
    {
        if (classe.PrimaryKey.Count() == 1 && classe.PrimaryKey.FirstOrDefault() is AssociationProperty ap)
        {
            var propertyName = classe.PrimaryKey.First().NameCamel;
            var propertyType = JpaModelPropertyGenerator.GetPropertyType(ap.Property);
            string setterName = $"set{ap.NamePascal}";
            var method = new JavaMethod("void", setterName)
            {
                Visibility = "public",
                Comment = $"Setter for {propertyName}",
            }.AddParameter(
                new JavaMethodParameter(propertyType, propertyName)
                {
                    Comment =
                        $"Set the value of {{@link {classe.GetImport(Config, tag)}#{propertyName} {propertyName}}}",
                }
            );
            method.Imports.AddRange(Config.GetDomainImports(ap.Property, tag));
            method.AddBodyLine(@$"this.{propertyName} = {propertyName};");
            return method;
        }
        return null;
    }

    protected virtual IEnumerable<JavaMethod> GetSetters(Class classe, string tag)
    {
        if (!Config.HasAnnotation(classe, "Setter"))
        {
            foreach (var property in classe.Properties)
            {
                if (!Config.HasAnnotation(property, "Setter"))
                {
                    yield return JpaModelPropertyGenerator.GetSetter(tag, property);
                }
            }
        }

        var mapIdSetter = GetMapIdPropertySetter(classe, tag);
        if (mapIdSetter != null)
        {
            yield return mapIdSetter;
        }
    }

    protected virtual IEnumerable<JavaMethod> GetToMappers(Class classe, string tag)
    {
        var toMappers = classe
            .ToMappers.Where(p => Classes.Contains(p.Class))
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

    protected virtual void WriteAnnotations(JavaWriter fw, Class classe, string tag)
    {
        fw.AddImports(Config.GetDecoratorImports(classe, tag).ToList());
        fw.Write(0, GetAnnotations(classe, tag));
    }

    protected virtual void WriteClassComment(JavaWriter fw, Class classe, string tag)
    {
        fw.WriteDocStart(0, classe.Comment);
        fw.WriteDocEnd(0);
    }

    protected virtual void WriteFieldsEnum(JavaWriter fw, Class classe, string tag)
    {
        if (!classe.Properties.Any())
        {
            return;
        }

        fw.Write(1, GetFieldsEnum(classe, tag));
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
        javaEnum.Add(classField.DefaultGetter);
        javaEnum.Constructors.Add(javaEnum.GetAllArgsConstructor());
        return javaEnum;
    }

    protected virtual IEnumerable<JavaEnumValue> GetFieldsEnumValues(Class classe, string tag)
    {
        return classe.Properties.Select(prop =>
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

    protected virtual void WriteGetters(JavaWriter fw, Class classe, string tag)
    {
        var getters = GetGetters(classe, tag);
        foreach (var getter in getters)
        {
            fw.Write(1, getter);
        }
    }

    protected virtual void WriteSetters(JavaWriter fw, Class classe, string tag)
    {
        var setters = GetSetters(classe, tag);
        foreach (var setter in setters)
        {
            fw.Write(1, setter);
        }
    }

    protected virtual void WriteToMappers(JavaWriter fw, Class classe, string tag)
    {
        var toMappers = classe.ToMappers.Where(p => Classes.Contains(p.Class)).ToList();
        if (!toMappers.Any())
        {
            return;
        }

        foreach (var method in GetToMappers(classe, tag))
        {
            fw.Write(1, method);
            fw.WriteLine();
        }
    }
}
