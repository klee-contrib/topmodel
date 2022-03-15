using TopModel.Core;

namespace TopModel.Generator.Javascript;

public static class JavascriptUtils
{
    public static string GetPropertyTypeName(this IProperty prop)
    {
        return prop switch
        {
            CompositionProperty cp => cp.Kind switch
            {
                "object" => cp.Composition.Name,
                "list" or "async-list" => $"{cp.Composition.Name}[]",
                string _ when cp.DomainKind!.TS!.Type.Contains("{class}") => cp.DomainKind.TS.Type.Replace("{class}", cp.Composition.Name),
                string _ => $"{cp.DomainKind.TS.Type}<{cp.Composition.Name}>"
            },
            AliasProperty al => ((IFieldProperty)al).TS.Type + (al.Property is AssociationProperty ap && (ap.Type == AssociationType.ManyToMany || ap.Type == AssociationType.OneToMany) ? "[]" : string.Empty),
            IFieldProperty fp => fp.TS.Type,
            _ => string.Empty
        };
    }

    public static IEnumerable<(string Import, string Path)> GetPropertyImports(IEnumerable<IProperty> properties)
    {
        return properties.OfType<IFieldProperty>()
            .Where(p => p.Domain.TS?.Import != null)
            .Select(p => (p.Domain.TS!.Type, p.Domain.TS.Import!))
        .Concat(properties.OfType<CompositionProperty>()
            .Where(p => p.DomainKind?.TS?.Import != null)
            .Select(p => (p.DomainKind!.TS!.Type.Split('<').First(), p.DomainKind.TS.Import!)))
        .Distinct();
    }

    public static IEnumerable<(string Code, string Module)> GetReferencesToImport(IEnumerable<IProperty> properties)
    {
        return properties
            .Select(p => p is AliasProperty alp ? alp.Property : p)
            .OfType<IFieldProperty>()
            .Select(prop => (prop, classe: prop is AssociationProperty ap ? ap.Association : prop.Class))
            .Where(pc => pc.prop.TS.Type != pc.prop.Domain.TS!.Type && pc.prop.Domain.TS.Type == "string" && pc.classe.Reference)
            .Select(pc => (pc.prop.TS.Type, pc.classe.Namespace.Module))
            .Distinct();
    }

    public static IList<(string Import, string Path)> GroupAndSortImports(IEnumerable<(string Import, string Path)> imports)
    {
        return imports
             .GroupBy(i => i.Path)
             .Select(i => (Import: string.Join(", ", i.Select(l => l.Import)), Path: i.Key))
             .OrderBy(i => i.Path.StartsWith(".") ? i.Path : $"...{i.Path}")
             .ToList();
    }
}
