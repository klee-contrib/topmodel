using TopModel.Core.Model;

namespace TopModel.Core.Utils;

internal static class DependenciesExtensions
{
    internal static IEnumerable<ClassDependency> GetClassDependencies(
        this IEnumerable<IProperty> properties,
        Class? currentClass = null
    )
    {
        return properties
            .Where(p => p.Association != null && p.Association != currentClass)
            .Select(p => new ClassDependency(p.Association!, p))
            .Concat(
                properties
                    .Where(p => p.Composition != null && p.Composition != currentClass)
                    .Select(p => new ClassDependency(p.Composition!, p))
            )
            .Concat(
                properties
                    .OfType<AliasProperty>()
                    .Select(p =>
                        p.Property.Class != null
                        && (
                            p.Property == p.Property.Class.EnumKey
                            || p.Property.Class.UniqueKeys.Where(uk => uk.Count == 1)
                                .Select(uk => uk.Single())
                                .Contains(p.Property)
                        )
                            ? new ClassDependency(p.Property.Class, p)
                            : null
                    )
            )
            .Where(d => d != null)!;
    }
}
