namespace TopModel.Core;

internal static class DependenciesExtensions
{
    internal static IEnumerable<ClassDependency> GetClassDependencies(this IEnumerable<IProperty> properties, Class? currentClass = null)
    {
        return properties.OfType<AssociationProperty>().Select(p => new ClassDependency(p.Association, p))
            .Concat(properties.OfType<AliasProperty>().Select(p => p.Property is AssociationProperty ap ? new ClassDependency(ap.Association, p) : null))
            .Concat(properties.OfType<AliasProperty>().Select(p => p.Property == p.Property.Class.EnumKey ? new ClassDependency(p.Property.Class, p) : null))
            .Concat(properties.OfType<CompositionProperty>().Where(p => p.Composition != currentClass).Select(p => new ClassDependency(p.Composition, p)))
            .Where(d => d != null)!;
    }

    internal static IEnumerable<DomainDependency> GetDomainDependencies(this IEnumerable<IProperty> properties)
    {
        return properties.OfType<IFieldProperty>().Select(p => new DomainDependency(p.Domain, p))
            .Concat(properties.OfType<CompositionProperty>().Select(p => p.DomainKind != null ? new DomainDependency(p.DomainKind, p) : null))
            .Where(d => d != null)!;
    }
}
