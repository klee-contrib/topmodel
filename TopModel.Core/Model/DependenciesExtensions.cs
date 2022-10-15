namespace TopModel.Core;

internal static class DependenciesExtensions
{
    internal static IEnumerable<ClassDependency> GetClassDependencies(this IEnumerable<IProperty> properties, Class? currentClass = null)
    {
        return properties.OfType<AssociationProperty>().Select(p => new ClassDependency(p.Association, p))
            .Concat(properties.OfType<AliasProperty>().Select(p => p.Property is AssociationProperty ap ? new ClassDependency(ap.Association, p) : null))
            .Concat(properties.OfType<AliasProperty>().Select(p => p.Property is RegularProperty { ReferenceKey: true } rp ? new ClassDependency(rp.Class, p) : null))
            .Concat(properties.OfType<CompositionProperty>().Where(p => p.Composition != currentClass).Select(p => new ClassDependency(p.Composition, p)))
            .Where(d => d != null)!;
    }

    internal static IEnumerable<Domain> GetDomainDependencies(this IEnumerable<IProperty> properties)
    {
        return properties.OfType<IFieldProperty>().Select(p => p is AliasProperty { ListDomain: Domain ld } ? ld : p.Domain)
            .Concat(properties.OfType<CompositionProperty>().Select(p => p.DomainKind != null ? p.DomainKind : null))
            .Distinct()
            .Where(d => d != null)!;
    }
}
