using TopModel.Core.Model;

namespace TopModel.Generator.Jpa;

public static class ImportsJpaExtensions
{
    public static string GetImport(this Class classe, JpaConfig config, string tag)
    {
        if (config.EnumsAsEnums && config.CanClassUseEnums(classe))
        {
            return $"{config.GetEnumValuePackageName(classe, config.GetBestClassTag(classe, tag))}.{classe.NamePascal}";
        }

        return $"{config.GetPackageName(classe, config.GetBestClassTag(classe, tag))}.{classe.NamePascal}";
    }

    public static IEnumerable<string> GetTypeImports(
        this IProperty p,
        JpaConfig config,
        string tag,
        bool forceAssociationPropertyType = false
    )
    {
        foreach (var di in config.GetDomainImports(p, tag))
        {
            yield return di;
        }

        if (p is { Composition: Class cpc })
        {
            yield return cpc.GetImport(config, config.GetBestClassTag(cpc, tag));
        }

        if (
            p is { EnumProperty: IProperty ep, Association: null }
            && ep.Class != null
            && config.CanClassUseEnums(ep.Class, ep)
        )
        {
            if (config.EnumsAsEnums)
            {
                yield return $"{config.GetEnumValuePackageName(ep.Class.EnumKey!.Class, config.GetBestClassTag(ep.Class.EnumKey!.Class, tag))}.{ep.Class.NamePascal}";
            }
            else
            {
                yield return $"{config.GetEnumPackageName(ep.Class, config.GetBestClassTag(ep.Class, tag))}.{config.GetEnumName(ep, ep.Class)}";
            }
        }

        if (p is { Association: Class association, AssociationProperty: IProperty ap } && !forceAssociationPropertyType)
        {
            if (config.CanClassUseEnums(association, ap))
            {
                if (config.EnumsAsEnums)
                {
                    yield return $"{config.GetEnumValuePackageName(association.EnumKey!.Class, config.GetBestClassTag(association.EnumKey!.Class, tag))}.{association.NamePascal}";
                }
                else if (p.Class?.IsPersistent != true)
                {
                    yield return $"{config.GetEnumPackageName(ap.Class, config.GetBestClassTag(ap.Class, tag))}.{config.GetEnumName(ap, association)}";
                }
                else if (!config.UseJdbc && p.Class != null && association.IsPersistent && p.Class.IsPersistent)
                {
                    yield return association.GetImport(config, config.GetBestClassTag(association, tag));
                }
            }
            else if (!config.UseJdbc && p.Class != null && association.IsPersistent && p.Class.IsPersistent)
            {
                yield return association.GetImport(config, config.GetBestClassTag(association, tag));
            }
        }
    }
}
