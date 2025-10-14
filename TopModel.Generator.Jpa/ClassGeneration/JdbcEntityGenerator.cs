using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Model.Implementation;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JdbcEntityGenerator(ILogger<JdbcEntityGenerator> logger, IFileWriterProvider writerProvider)
    : JavaClassGeneratorBase(logger, writerProvider)
{
    private JavaEnumConstructorGenerator? _javaEnumConstructorGenerator;

    public override string Name => "JdbcEntityGen";

    protected override JavaEnumConstructorGenerator ConstructorGenerator
    {
        get
        {
            _javaEnumConstructorGenerator ??= new JavaEnumConstructorGenerator(Config);
            return _javaEnumConstructorGenerator;
        }
    }

    protected override bool FilterClass(Class classe)
    {
        return !classe.Abstract && classe.IsPersistent;
    }

    protected override IEnumerable<JavaAnnotation> GetAnnotations(Class classe, string tag)
    {
        var annotations = base.GetAnnotations(classe, tag).ToList();
        var tableAnnotation = new JavaAnnotation(
            "Table",
            imports: "org.springframework.data.relational.core.mapping.Table"
        ).AddAttribute("name", @$"""{classe.SqlName.ToLower()}""");
        annotations.Add(tableAnnotation);
        return annotations;
    }

    protected override IEnumerable<JavaField> GetFields(Class classe, string tag)
    {
        if (Config.CanClassUseEnums(classe))
        {
            var codeProperty = classe.EnumKey!;
            foreach (var refValue in classe.Values.OrderBy(x => x.Name, StringComparer.Ordinal))
            {
                var code = refValue.Value[codeProperty];

                yield return new JavaField(classe.NamePascal, code)
                {
                    Static = true,
                    Final = true,
                    DefaultValue = $"new {classe.NamePascal}({Config.GetEnumName(codeProperty, classe)}.{code})",
                }.Add(new JavaAnnotation("Transient", imports: $"{JavaxOrJakarta}.persistence.Transient"));
            }
        }
        foreach (var property in JpaModelPropertyGenerator.GetAvailableProperties(classe))
        {
            yield return JpaModelPropertyGenerator.GetField(property, tag);
        }
    }

    protected override IEnumerable<JavaEnumValue> GetFieldsEnumValues(Class classe, string tag)
    {
        return JpaModelPropertyGenerator
            .GetAvailableProperties(classe)
            .Select(prop =>
            {
                string name = JpaModelPropertyGenerator.GetPropertyName(prop).ToConstantCase();
                var javaType = Config.GetType(
                    prop,
                    useClassForAssociation: classe.IsPersistent
                        && !Config.UseJdbc
                        && prop is AssociationProperty asp
                        && asp.Association.IsPersistent
                );
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
        var properties = JpaModelPropertyGenerator.GetAvailableProperties(classe);
        foreach (var property in properties)
        {
            yield return JpaModelPropertyGenerator!.GetGetter(tag, property);
        }
    }

    protected override IEnumerable<JavaClass> GetInnerClasses(Class classe, string tag)
    {
        if (
            Config.FieldsEnum.Contains(AnnotationConstraint.Persisted)
            && JpaModelPropertyGenerator.GetAvailableProperties(classe).Any()
        )
        {
            var fieldEnum = GetFieldsEnum(classe, tag);
            yield return fieldEnum;
        }
    }

    protected override IEnumerable<JavaMethod> GetSetters(Class classe, string tag)
    {
        var properties = JpaModelPropertyGenerator.GetAvailableProperties(classe);
        if (!Config.CanClassUseEnums(classe))
        {
            foreach (var property in properties)
            {
                yield return JpaModelPropertyGenerator!.GetSetter(tag, property);
            }
        }
    }
}
