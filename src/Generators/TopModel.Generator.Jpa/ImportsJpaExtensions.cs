using TopModel.Core.Model;
using TopModel.Generator.Core;

namespace TopModel.Generator.Jpa;

public static class ImportsJpaExtensions
{
    public static string GetImport(this Class classe, JpaConfig config, string tag)
    {
        if (classe.Enum == EnumMode.Enum)
        {
            return $"{config.GetEnumPackageName(classe, config.GetBestClassTag(classe, tag))}.{classe.NamePascal}";
        }

        return $"{config.GetPackageName(classe, config.GetBestClassTag(classe, tag))}.{classe.NamePascal}";
    }

    public static IEnumerable<string> GetTypeImports(
        this IProperty p,
        JpaConfig config,
        string tag,
        bool forceAssociationPropertyType = false,
        bool skipDomainImports = false
    )
    {
        if (!skipDomainImports)
        {
            foreach (var di in config.GetDomainImports(p, tag))
            {
                yield return di;
            }
        }

        if (p is { Composition: Class cpc })
        {
            yield return cpc.GetImport(config, config.GetBestClassTag(cpc, tag));
        }

        if (
            p is { EnumProperty: IProperty ep, Association: null }
            && config.AvailableClasses.Contains(ep.Class)
            && (config.UniqueValueGeneration.CanEnum || ep.Class.Enum == EnumMode.Enum)
        )
        {
            yield return $"{config.GetEnumPackageName(ep.Class, config.GetBestClassTag(ep.Class, tag))}.{config.GetEnumType(ep)}";
        }

        if (
            p is { Association: Class association, AssociationProperty: IProperty ap }
            && config.AvailableClasses.Contains(association)
        )
        {
            if (
                association.Enum == EnumMode.Enum
                || association.Enum == EnumMode.Class
                    && ap.EnumProperty != null
                    && (!p.UseClassForAssociation || config.UseJdbc)
                    && config.UniqueValueGeneration.CanEnum
            )
            {
                yield return $"{config.GetEnumPackageName(ap.Class, config.GetBestClassTag(ap.Class, tag))}.{config.GetEnumType(ap)}";
            }
            else if (
                p.UseClassForAssociation
                && p.Class != null
                && association.IsPersistent
                && p.Class.IsPersistent
                && !forceAssociationPropertyType
            )
            {
                yield return association.GetImport(config, config.GetBestClassTag(association, tag));
            }
        }
    }
}
