using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Model.Implementation;
using TopModel.Generator.Jpa.ClassGeneration.Utils;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JdbcEntityGenerator(ILogger<JdbcEntityGenerator> logger, IFileWriterProvider writerProvider)
    : JavaClassGeneratorBase(logger, writerProvider)
{
    private JavaEnumGeneratorHelper? _javaEnumConstructorGenerator;

    public override string Name => "JdbcEntityGen";

    protected override JavaEnumGeneratorHelper JavaConstructorGenerator
    {
        get
        {
            _javaEnumConstructorGenerator ??= new JavaEnumGeneratorHelper(Config);
            return _javaEnumConstructorGenerator;
        }
    }

    protected override bool FilterClass(Class classe)
    {
        return classe.Type != ClassType.Interface && classe.IsPersistent && classe.Enum != EnumMode.Enum;
    }

    protected override IEnumerable<JavaAnnotation> GetAnnotations(Class classe, string tag)
    {
        var annotations = base.GetAnnotations(classe, tag).ToList();
        var tableAnnotation = new JavaAnnotation(
            "Table",
            imports: "org.springframework.data.relational.core.mapping.Table"
        ).AddAttribute("name", @$"""{Config.GetSqlName(classe, tag)}""");
        annotations.Add(tableAnnotation);
        return annotations;
    }

    protected override IEnumerable<JavaConstructor> GetConstuctors(Class classe, string tag)
    {
        if (classe.Enum == EnumMode.Class && classe.Readonly)
        {
            var allArgsConstructor = JavaConstructorGenerator.GetAllArgsConstructor(classe, tag);
            if (allArgsConstructor.Parameters.Count > 0)
            {
                allArgsConstructor.Visibility = "private";
                yield return allArgsConstructor;
            }
        }
    }

    protected override IEnumerable<JavaField> GetFields(Class classe, string tag)
    {
        if (classe.Enum == EnumMode.Class && classe.Readonly)
        {
            foreach (var javaFinalField in JavaConstructorGenerator.GetConstFields(classe, tag))
            {
                javaFinalField.Add(new JavaAnnotation("Transient", imports: "jakarta.persistence.Transient"));
                yield return javaFinalField;
            }
            yield return JavaConstructorGenerator.GetStaticValuesList(classe);
        }

        foreach (var property in Config.GetAvailableProperties(classe))
        {
            yield return JpaModelPropertyGenerator.GetField(property, tag);
        }
    }

    protected override IEnumerable<JavaEnumValue> GetFieldsEnumValues(Class classe, string tag)
    {
        return Config
            .GetAvailableProperties(classe)
            .Select(prop =>
            {
                string name = prop.NameCamel.ToConstantCase();
                var javaType = Config.GetType(prop);
                javaType = javaType.Split("<")[0];
                return new JavaEnumValue(name)
                {
                    Parameters = { $"{javaType}.class" },
                    Imports = prop.GetTypeImports(Config, tag).ToList(),
                };
            });
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Config.GetClassFileName(classe, tag);
    }

    protected override IEnumerable<JavaMethod> GetGetters(Class classe, string tag)
    {
        var properties = Config.GetAvailableProperties(classe);
        foreach (var property in properties)
        {
            yield return JpaModelPropertyGenerator!.GetGetter(tag, property);
        }
    }

    protected override IEnumerable<JavaClass> GetInnerClasses(Class classe, string tag)
    {
        if (Config.FieldsEnum.Contains(AnnotationConstraint.Persisted) && Config.GetAvailableProperties(classe).Any())
        {
            var fieldEnum = GetFieldsEnum(classe, tag);
            yield return fieldEnum;
        }
    }

    protected override IEnumerable<JavaMethod> GetSetters(Class classe, string tag)
    {
        var properties = Config.GetAvailableProperties(classe);
        if (classe.Enum == null)
        {
            foreach (var property in properties)
            {
                yield return JpaModelPropertyGenerator!.GetSetter(tag, property);
            }
        }
    }
}
