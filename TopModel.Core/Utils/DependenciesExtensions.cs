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
            .Where(p => p.Association != null && p.Association != currentClass && p.UseClassForAssociation)
            .Select(p => new ClassDependency(p.Association!, p))
            .Concat(
                properties
                    .Where(p => p.Composition != null && p.Composition != currentClass)
                    .Select(p => new ClassDependency(p.Composition!, p))
            )
            .Concat(
                properties
                    .Where(p =>
                        p.EnumLikeProperty != null
                        && p.EnumLikeProperty!.Class != currentClass
                        && !p.UseClassForAssociation
                    )
                    .Select(p => new ClassDependency(p.EnumLikeProperty!.Class, p))
            )
            .Where(d => d != null)!;
    }
}
