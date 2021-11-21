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
                imports.Add("javax.persistence.JoinColumn");
                imports.Add("javax.persistence.JoinTable");
                break;
            case AssociationType.OneToOne:
                imports.Add("javax.persistence.FetchType");
                break;
        }
        if (ap.Association.Namespace.Module != ap.Class.Namespace.Module)
        {
            imports.Add(ap.Association.getImport(_config));
        }
        return imports;
    }

    public static List<string> getImports(this CompositionProperty cp, JpaConfig _config)
    {
        var imports = new List<string>();
        if (cp.Composition.Namespace.Module != cp.Class.Namespace.Module)
        {
            imports.Add(cp.Composition.getImport(_config));
        }
        return imports;
    }

    public static List<string> getImports(this IFieldProperty rp, JpaConfig _config)
    {
        var imports = new List<string>();
        if (rp.Class.IsPersistent)
        {
            imports.Add("javax.persistence.Column");
        }
        if (rp.Domain.Java != null)
        {
            if (rp.Domain.Java.Import != null)
            {
                imports.Add($"{rp.Domain.Java.Import}.{rp.Domain.Java.Type}");
            }
            if (rp.Domain.Java.Annotations != null)
            {
                foreach (var annotation in rp.Domain.Java.Annotations.Where(a => a.Imports is not null))
                {
                    imports.AddRange(annotation.Imports!);
                }
            }
        }
        if (rp.PrimaryKey)
        {
            imports.Add("javax.persistence.Id");
            if (
                           rp.Domain.Java!.Type == "Long"
                       || rp.Domain.Java.Type == "long"
                       || rp.Domain.Java.Type == "int"
                       || rp.Domain.Java.Type == "Integer")
            {
                imports.Add("javax.persistence.GeneratedValue");
                imports.Add("javax.persistence.SequenceGenerator");
                imports.Add("javax.persistence.GenerationType");
            }
        }
        return imports;
    }

    public static string getImport(this Class classe, JpaConfig _config)
    {
        var entityDto = classe.IsPersistent ? "entities" : "dtos";
        var packageName = $"{_config.DaoPackageName}.{entityDto}.{classe.Namespace.Module.ToLower()}";
        return $"{packageName}.{classe.Name}";
    }
    public static List<string> getImports(this Class classe, JpaConfig _config)
    {
        var imports = new List<string>
            {
                "lombok.NoArgsConstructor",
                "lombok.Builder",
                "lombok.Setter",
                "lombok.ToString",
                "lombok.EqualsAndHashCode",
                "lombok.AllArgsConstructor",
                "java.io.Serializable"
            };
        if (classe.IsPersistent)
        {
            imports.Add("javax.persistence.Entity");
            imports.Add("javax.persistence.Table");
        }
        else
        {
            if (classe.Properties.Any(p => p is IFieldProperty { Required: true })
                || classe.Properties.Any(p => p is AliasProperty { Required: true }))
            {
                imports.Add("javax.validation.constraints.NotNull");
            }

            imports.Sort();
            return imports;
        }
        if (classe.Reference)
        {
            imports.Add("javax.persistence.Enumerated");
            imports.Add("javax.persistence.EnumType");
            imports.Add("org.hibernate.annotations.Cache");
            imports.Add("org.hibernate.annotations.Cache");
            imports.Add("org.hibernate.annotations.Immutable");
            imports.Add("org.hibernate.annotations.CacheConcurrencyStrategy");
            imports.Add($"{_config.DaoPackageName}.references.{classe.Namespace.Module.ToLower()}.{classe.Name}Code");
        }

        if (classe.UniqueKeys?.Count > 0)
        {
            imports.Add("javax.persistence.UniqueConstraint");
        }
        return imports;
    }
}