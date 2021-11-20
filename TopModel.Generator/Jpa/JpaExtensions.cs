using TopModel.Core;

namespace TopModel.Generator.Jpa;

public static class JpaExtensions
{
    public static string GetAssociationName(this AssociationProperty ap)
    {
        return $"{ap.Association.Name.ToFirstLower()}{ap.Role?.ToFirstUpper() ?? string.Empty}{(ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany ? "List" : string.Empty)}";
    }

    public static List<string> getImports(this AssociationProperty ap, JpaConfig _config)
    {
        var imports = new List<string>();
        imports.Add($"javax.persistence.{ap.Type}");
        switch (ap.Type)
        {
            case AssociationType.OneToMany:
                imports.Add("java.util.Set");
                imports.Add("java.util.HashSet");
                imports.Add("javax.persistence.FetchType");
                imports.Add("javax.persistence.CascadeType");
                break;
            case AssociationType.ManyToOne:
                imports.Add("javax.persistence.FetchType");
                imports.Add("javax.persistence.JoinColumn");
                break;
            case AssociationType.ManyToMany:
                imports.Add("java.util.Set");
                imports.Add("java.util.HashSet");
                imports.Add("javax.persistence.FetchType");
                imports.Add("javax.persistence.JoinColumn");
                imports.Add("javax.persistence.JoinTable");
                break;
        }
        if (ap.Association.Namespace.Module != ap.Class.Namespace.Module)
        {
            var entityDto = ap.Class.IsPersistent ? "entities" : "dtos";
            var packageName = $"{_config.DaoPackageName}.{entityDto}.{ap.Association.Namespace.Module.ToLower()}";
            imports.Add($"{packageName}.{ap.Association.Name}");
        }
        return imports;
    }
}