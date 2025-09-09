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

        var props = classe
            .GetProperties(Classes)
            .Select(prop =>
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

    protected virtual void WriteGetters(JavaWriter fw, Class classe, string tag)
    {
        if (!Config.HasAnnotation(classe, "Getter"))
        {
            foreach (var property in classe.GetProperties(Classes))
            {
                JpaModelPropertyGenerator.WriteGetter(fw, tag, property);
            }
        }

        WriteMapIdPropertyGetter(fw, classe, tag);
    }

    protected virtual void WriteSetters(JavaWriter fw, Class classe, string tag)
    {
        if (!Config.HasAnnotation(classe, "Setter"))
        {
            foreach (var property in classe.GetProperties(Classes))
            {
                JpaModelPropertyGenerator.WriteSetter(fw, tag, property);
            }
        }

        WriteMapIdPropertySetter(fw, classe, tag);
    }

    protected virtual void WriteToMappers(JavaWriter fw, Class classe, string tag)
    {
        var toMappers = classe
            .ToMappers.Where(p => Classes.Contains(p.Class))
            .Select(m => (classe, m))
            .OrderBy(m => m.m.Name)
            .ToList();

        foreach (var toMapper in toMappers)
        {
            var (_, mapper) = toMapper;
            fw.AddImport(mapper.Class.GetImport(Config, tag));
            fw.WriteLine();
            fw.WriteDocStart(1, $"Mappe '{classe}' vers '{mapper.Class.NamePascal}'");
            if (mapper.Comment != null)
            {
                fw.WriteLine(1, $" * {mapper.Comment}");
            }

            fw.WriteParam(
                "target",
                $"Instance pré-existante de '{mapper.Class.NamePascal}'. Une nouvelle instance sera créée si non spécifié."
            );
            fw.WriteReturns(1, $"Une instance de '{mapper.Class.NamePascal}'");

            fw.WriteDocEnd(1);
            var (mapperNs, mapperModelPath) = Config.GetMapperLocation(toMapper);

            fw.WriteLine(
                1,
                $"public {mapper.Class.NamePascal} {mapper.Name.Value.ToCamelCase()}({mapper.Class.NamePascal} target) {{"
            );
            fw.WriteLine(
                2,
                $"return {Config.GetMapperName(mapperNs, mapperModelPath)}.{mapper.Name.Value.ToCamelCase()}(this, target);"
            );
            fw.AddImport(Config.GetMapperImport(mapperNs, mapperModelPath, tag)!);
            fw.WriteLine(1, "}");

            if (toMappers.IndexOf(toMapper) < toMappers.Count - 1)
            {
                fw.WriteLine();
            }
        }
    }

    private void WriteMapIdPropertyGetter(JavaWriter fw, Class classe, string tag)
    {
        if (classe.PrimaryKey.Count() == 1 && classe.PrimaryKey.FirstOrDefault() is AssociationProperty ap)
        {
            var propertyType = JpaModelPropertyGenerator.GetPropertyType(ap.Property);
            fw.WriteLine();
            string getterName = $"get{ap.NamePascal}";
            var method = new JavaMethod(propertyType, getterName)
            {
                Visibility = "public",
                Comment = $"Getter for {ap.NameCamel}",
                ReturnComment = $"value of {{@link {classe.GetImport(Config, tag)}#{ap.NameCamel} {ap.NameCamel}}}",
            };
            method.AddBodyLine(@$"return this.{ap.NameCamel};");
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
