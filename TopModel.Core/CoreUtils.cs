using TopModel.Core.FileModel;

namespace TopModel.Core;

public static class CoreUtils
{
    public static IList<T> Sort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> getDependencies)
            where T : notnull
    {
        var sorted = new List<T>();
        var visited = new Dictionary<T, bool>();

        foreach (var item in source)
        {
            Visit(item, getDependencies, sorted, visited);
        }

        return sorted;
    }

    private static void Visit<T>(T item, Func<T, IEnumerable<T>> getDependencies, List<T> sorted, Dictionary<T, bool> visited)
        where T : notnull
    {
        var alreadyVisited = visited.TryGetValue(item, out var inProcess);

        if (alreadyVisited)
        {
            if (inProcess)
            {
                throw new ModelException(
                    item,
                    $"Dépendance circulaire détectée : {visited.Last().Key} ne peut pas référencer {item}.",
                    (item as ModelFile)?.Uses.FirstOrDefault(u => u.ReferenceName == (visited.Last().Key as ModelFile)?.Name));
            }
        }
        else
        {
            visited[item] = true;

            foreach (var dependency in getDependencies(item))
            {
                Visit(dependency, getDependencies, sorted, visited);
            }

            visited[item] = false;
            sorted.Add(item);
        }
    }

    public static bool IsToMany(this AssociationType associationType)
    {
        return associationType == AssociationType.ManyToMany || associationType == AssociationType.OneToMany;
    }
}
