using TopModel.Core.Model;

namespace TopModel.Generator.Jpa.ClassGeneration.Utils;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JdbcModelPropertyGenerator(JpaConfig config, IDictionary<string, string> newableTypes)
    : JpaModelPropertyGenerator(config, newableTypes)
{
    private static new JavaAnnotation IdAnnotation => new("Id", imports: "org.springframework.data.annotation.Id");

    public override JavaAnnotation GetColumnAnnotation(IProperty property, string tag)
    {
        return new JavaAnnotation(
            "Column",
            imports: "org.springframework.data.relational.core.mapping.Column"
        ).AddAttribute("value", $@"""{Config.GetSqlName(property, tag)}""");
    }

    public override bool ShouldWriteEnumAnnotation(IProperty property)
    {
        return false;
    }

    protected override IEnumerable<JavaAnnotation> GetIdAnnotations(IProperty property, string tag)
    {
        yield return IdAnnotation;
    }
}
