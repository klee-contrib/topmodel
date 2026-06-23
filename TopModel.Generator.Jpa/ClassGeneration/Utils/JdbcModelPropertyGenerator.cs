using TopModel.Core.Model;

namespace TopModel.Generator.Jpa.ClassGeneration.Utils;

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

    public override bool ShouldWriteEnumAnnotation(IProperty property)
    {
        return false;
    }

    protected override string GetDefaultValue(IProperty property)
    {
        var defaultValue = Config.GetValue(property);
        var suffix = defaultValue != "null" ? $"{defaultValue}" : string.Empty;
        return suffix;
    }

    protected override IEnumerable<JavaAnnotation> GetIdAnnotations(IProperty property, string tag)
    {
        yield return IdAnnotation;
    }
}
