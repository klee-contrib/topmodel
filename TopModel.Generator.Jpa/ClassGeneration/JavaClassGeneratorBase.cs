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

        var mapIdGetter = GetMapIdPropertyGetter(classe, tag);
        if (mapIdGetter != null)
        {
            yield return mapIdGetter;
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

        if (Config.FieldsEnumInterface != null)
        {
            fw.AddImport(Config.FieldsEnumInterface.Replace("<>", string.Empty));
        }

        fw.WriteLine();
        fw.WriteDocStart(
            1,
            $"Enumération des champs de la classe {{@link {classe.GetImport(Config, tag)} {classe.NamePascal}}}"
        );
        fw.WriteDocEnd(1);
        string enumDeclaration = @$"public enum Fields";
        if (Config.FieldsEnumInterface != null)
        {
            enumDeclaration +=
                $" implements {Config.FieldsEnumInterface.Split(".")[^1].Replace("<>", $"<{classe.NamePascal}>")}";
        }

        enumDeclaration += " {";
        fw.WriteLine(1, enumDeclaration);

        var props = classe.Properties.Select(prop =>
        {
            string name;
            if (prop is AssociationProperty ap && ap.Association.IsPersistent && !Config.UseJdbc)
            {
                name = ap.NameByClassCamel.ToConstantCase();
            }
            else
            {
                name = prop.NameCamel.ToConstantCase();
            }

            var javaType = Config.GetType(
                prop,
                useClassForAssociation: classe.IsPersistent
                    && !Config.UseJdbc
                    && prop is AssociationProperty asp
                    && asp.Association.IsPersistent
            );
            javaType = javaType.Split("<")[0];
            return $"        {name}({javaType}.class)";
        });

        fw.WriteLine(string.Join(", //\n", props) + ";");

        fw.WriteLine();

        fw.WriteLine(2, "private final Class<?> type;");
        fw.WriteLine();
        fw.WriteLine(2, "Fields(Class<?> type) {");
        fw.WriteLine(3, "this.type = type;");
        fw.WriteLine(2, "}");

        fw.WriteLine();

        fw.WriteLine(2, "public Class<?> getType() {");
        fw.WriteLine(3, "return this.type;");
        fw.WriteLine(2, "}");

        fw.WriteLine(1, "}");
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

        var enumValues = classe.Properties.Select(prop =>
        {
            string name;
            if (prop is AssociationProperty ap && ap.Association.IsPersistent && !Config.UseJdbc)
            {
                name = ap.NameByClassCamel.ToConstantCase();
            }
            else
            {
                name = prop.NameCamel.ToConstantCase();
            }

            var javaType = Config.GetType(
                prop,
                useClassForAssociation: classe.IsPersistent
                    && !Config.UseJdbc
                    && prop is AssociationProperty asp
                    && asp.Association.IsPersistent
            );
            javaType = javaType.Split("<")[0];
            return new JavaEnumValue(name) { Parameters = { $"{javaType}.class" } };
        });
        javaEnum.Values.AddRange(enumValues);
        var classField = new JavaField("Class<?>", "type") { Final = true };
        javaEnum.Add(new JavaField("Class<?>", "type") { Final = true });
        javaEnum.Add(classField.DefaultGetter);
        javaEnum.Constructors.Add(javaEnum.GetAllArgsConstructor());
        return javaEnum;
    }

    protected virtual void WriteGetters(JavaWriter fw, Class classe, string tag)
    {
        if (!Config.HasAnnotation(classe, "Getter"))
        {
            foreach (var property in classe.Properties)
            {
                if (!Config.HasAnnotation(property, "Getter"))
                {
                    JpaModelPropertyGenerator.WriteGetter(fw, tag, property);
                }
            }
        }

        WriteMapIdPropertyGetter(fw, classe, tag);
    }

    protected virtual void WriteSetters(JavaWriter fw, Class classe, string tag)
    {
        if (!Config.HasAnnotation(classe, "Setter"))
        {
            foreach (var property in classe.Properties)
            {
                if (!Config.HasAnnotation(property, "Setter"))
                {
                    JpaModelPropertyGenerator.WriteSetter(fw, tag, property);
                }
            }
        }

        WriteMapIdPropertySetter(fw, classe, tag);
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

    private JavaMethod? GetMapIdPropertyGetter(Class classe, string tag)
    {
        if (classe.PrimaryKey.Count() == 1 && classe.PrimaryKey.FirstOrDefault() is AssociationProperty ap)
        {
            var propertyType = JpaModelPropertyGenerator.GetPropertyType(ap.Property);
            string getterName = $"get{ap.NamePascal}";
            var method = new JavaMethod(propertyType, getterName)
            {
                Visibility = "public",
                Comment = $"Getter for {ap.NameCamel}",
                ReturnComment = $"value of {{@link {classe.GetImport(Config, tag)}#{ap.NameCamel} {ap.NameCamel}}}",
            };
            method.AddBodyLine(@$"return this.{ap.NameCamel};");
            return method;
        }
        return null;
    }

    private void WriteMapIdPropertyGetter(JavaWriter fw, Class classe, string tag)
    {
        var method = GetMapIdPropertyGetter(classe, tag);
        if (method != null)
        {
            fw.Write(1, method);
        }
    }

    private void WriteMapIdPropertySetter(JavaWriter fw, Class classe, string tag)
    {
        if (classe.PrimaryKey.Count() == 1 && classe.PrimaryKey.FirstOrDefault() is AssociationProperty ap)
        {
            var propertyName = classe.PrimaryKey.First().NameCamel;
            var propertyType = JpaModelPropertyGenerator.GetPropertyType(ap.Property);
            fw.WriteLine();
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
            fw.Write(1, method);
        }
    }
}
