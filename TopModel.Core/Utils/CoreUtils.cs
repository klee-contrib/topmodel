using TopModel.Core.FileModel;
using TopModel.Core.Model;

namespace TopModel.Core.Utils;

public static class CoreUtils
{
    public static bool IsAssociationToMany(this IProperty property)
    {
        return property
            is AssociationProperty { Type: AssociationType.OneToMany or AssociationType.ManyToMany }
                or AliasProperty
            {
                Property: AssociationProperty { Type: AssociationType.OneToMany or AssociationType.ManyToMany }
            };
    }

    public static bool IsToMany(this AssociationType associationType)
    {
        return associationType == AssociationType.ManyToMany || associationType == AssociationType.OneToMany;
    }

    public static IList<T> Sort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> getDependencies)
        where T : notnull
    {
        var sorted = new List<T>();
        var visited = new Dictionary<T, bool>();

        void Visit(T item)
        {
            var alreadyVisited = visited.TryGetValue(item, out var inProcess);

            if (alreadyVisited)
            {
                if (inProcess)
                {
                    throw new ModelException(
                        item,
                        $"Dépendance circulaire détectée : {visited.Last().Key} ne peut pas référencer {item}.",
                        (item as ModelFile)?.Uses.FirstOrDefault(u =>
                            u.ReferenceName == (visited.Last().Key as ModelFile)?.Name
                        )
                    );
                }
            }
            else
            {
                visited[item] = true;

                foreach (var dependency in getDependencies(item))
                {
                    Visit(dependency);
                }

                visited[item] = false;
                sorted.Add(item);
            }
        }

        foreach (var item in source)
        {
            Visit(item);
        }

        return sorted;
    }

    public static IList<IList<ModelFile>> SortWithCycles(
        IEnumerable<ModelFile> source,
        Func<ModelFile, IEnumerable<ModelFile>> getDependencies
    )
    {
        var indexMap = new Dictionary<ModelFile, int>();
        var lowLinkMap = new Dictionary<ModelFile, int>();
        var pending = new Stack<ModelFile>();

        IList<IList<ModelFile>> sorted = [];

        int index = 0;

        void Visit(ModelFile item)
        {
            indexMap[item] = index;
            lowLinkMap[item] = index;
            index++;
            pending.Push(item);

            foreach (var dep in getDependencies(item))
            {
                if (!indexMap.TryGetValue(dep, out var visited))
                {
                    Visit(dep);
                    lowLinkMap[item] = Math.Min(lowLinkMap[item], lowLinkMap[dep]);
                }
                else if (pending.Contains(dep))
                {
                    lowLinkMap[item] = Math.Min(lowLinkMap[item], visited);
                }
            }

            if (lowLinkMap[item] == indexMap[item])
            {
                var cycle = new List<ModelFile>();
                ModelFile w;
                do
                {
                    w = pending.Pop();
                    cycle.Add(w);
                } while (!w.Equals(item));

                sorted.Add(cycle.OrderBy(f => f.Name).ToList());
            }
        }

        foreach (var item in source)
        {
            if (!indexMap.ContainsKey(item))
            {
                Visit(item);
            }
        }

        return sorted;
    }
}
