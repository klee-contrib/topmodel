using TopModel.Core.Model;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JdbcModelPropertyGenerator(JpaConfig config, IDictionary<string, string> newableTypes)
    : JpaModelPropertyGenerator(config, newableTypes)
{
    private static new JavaAnnotation IdAnnotation => new("Id", imports: "org.springframework.data.annotation.Id");

    public override JavaAnnotation GetColumnAnnotation(IProperty property)
    {
        return new JavaAnnotation(
            "Column",
            imports: "org.springframework.data.relational.core.mapping.Column"
        ).AddAttribute("value", $@"""{property.SqlName.ToLower()}""");
    }

    public override string GetPropertyName(IProperty property)
    {
        return property.NameCamel;
    }

    public override string GetPropertyType(IProperty property)
    {
        return Config.GetType(property, useClassForAssociation: false);
    }

    protected override IEnumerable<JavaAnnotation> GetAnnotations(AliasProperty property, string tag)
    {
        if (property.PrimaryKey && property.Class.IsPersistent)
        {
            yield return IdAnnotation;
        }

        yield return GetColumnAnnotation(property);

        if (property.Required && !property.PrimaryKey && (!property.Class.IsPersistent || Config.UseJdbc))
        {
            yield return NotNullAnnotation;
        }
    }

    protected override IEnumerable<JavaAnnotation> GetAnnotations(AssociationProperty property, string tag)
    {
        if (property.Class.IsPersistent)
        {
            if (property.PrimaryKey && property.Class.PrimaryKey.Count() <= 1)
            {
                yield return IdAnnotation;
            }

            yield return GetColumnAnnotation(property);
        }
    }

    protected override IEnumerable<JavaAnnotation> GetAnnotations(IProperty property)
    {
        if (property.PrimaryKey && property.Class.IsPersistent)
        {
            yield return IdAnnotation;
        }

        yield return GetColumnAnnotation(property);

        if (property.Required && !property.PrimaryKey)
        {
            yield return NotNullAnnotation;
        }
    }
}
