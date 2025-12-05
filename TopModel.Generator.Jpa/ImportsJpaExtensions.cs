using TopModel.Core.Model;
using TopModel.Core.Utils;

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

    public static IEnumerable<string> GetTypeImports(this IProperty p, JpaConfig config, string tag)
    {
        foreach (var di in config.GetDomainImports(p, tag))
        {
            yield return di;
        }

        if (p is IProperty { Composition: Class cpc })
        {
            yield return cpc.GetImport(config, config.GetBestClassTag(cpc, tag));
        }

        if (p.Class != null && config.CanClassUseEnums(p.Class, p))
        {
            yield return $"{config.GetEnumPackageName(p.Class, config.GetBestClassTag(p.Class, tag))}.{config.GetEnumName(p, p.Class)}";
        }

        if (p.Class != null && p is AliasProperty { Property: IProperty tp } && config.CanClassUseEnums(tp.Class, tp))
        {
            if (config.EnumsAsEnums)
            {
                yield return $"{config.GetEnumValuePackageName(tp.Class.EnumKey!.Class, config.GetBestClassTag(tp.Class.EnumKey!.Class, tag))}.{tp.Class.NamePascal}";
            }
            else
            {
                yield return $"{config.GetEnumPackageName(tp.Class, config.GetBestClassTag(tp.Class, tag))}.{config.GetEnumName(tp, tp.Class)}";
            }
        }

        var ap = (p as AssociationProperty) ?? (p as AliasProperty)?.Property as AssociationProperty;

        if (ap != null)
        {
            if (config.CanClassUseEnums(ap.Association, prop: ap.Property))
            {
                if (config.EnumsAsEnums)
                {
                    yield return $"{config.GetEnumValuePackageName(ap.Association.EnumKey!.Class, config.GetBestClassTag(ap.Association.EnumKey!.Class, tag))}.{ap.Association.NamePascal}";
                }
                else if (p.Class?.IsPersistent != true)
                {
                    yield return $"{config.GetEnumPackageName(ap.Property.Class, config.GetBestClassTag(ap.Property.Class, tag))}.{config.GetEnumName(ap.Property, ap.Property.Class)}";
                }
                else if (!config.UseJdbc && p.Class != null && ap.Association.IsPersistent && p.Class.IsPersistent)
                {
                    yield return ap.Association.GetImport(config, config.GetBestClassTag(ap.Association, tag));
                }
            }
            else if (!config.UseJdbc && p.Class != null && ap.Association.IsPersistent && p.Class.IsPersistent)
            {
                yield return ap.Association.GetImport(config, config.GetBestClassTag(ap.Association, tag));
            }
        }
    }
}
