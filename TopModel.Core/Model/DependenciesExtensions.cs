namespace TopModel.Core;

internal static class DependenciesExtensions
{
    internal static IEnumerable<ClassDependency> GetClassDependencies(this IEnumerable<IProperty> properties, Class? currentClass = null)
    {
        return properties.OfType<AssociationProperty>().Select(p => new ClassDependency(p.Association, p))
            .Concat(properties.OfType<AliasProperty>().Select(p => p.Property is AssociationProperty ap ? new ClassDependency(ap.Association, p) : null))
            .Concat(properties.OfType<AliasProperty>().Select(p => p.Property == p.Property.Class.EnumKey || p.Property.Class.UniqueKeys.Where(uk => uk.Count == 1).Select(uk => uk.Single()).Contains(p.Property) ? new ClassDependency(p.Property.Class, p) : null))
            .Concat(properties.OfType<CompositionProperty>().Where(p => p.Composition != currentClass).Select(p => new ClassDependency(p.Composition, p)))
            .Concat(properties.OfType<AliasProperty>().Where(p => p.Property is CompositionProperty cp && cp.Composition != currentClass).Select(p => p is AliasProperty { Property: CompositionProperty cp } ? new ClassDependency(cp.Composition, p) : null))
            .Where(d => d != null)!;
    }
}
