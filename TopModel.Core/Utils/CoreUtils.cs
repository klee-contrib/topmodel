using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Core.Utils;

#pragma warning disable KTA1200, S2325 // Jusqu'à ce qu'on supporte les blocs d'extension...

public static class CoreUtils
{
    extension(IProperty prop)
    {
        public Class? Association =>
            prop switch
            {
                AssociationProperty { Association: Class a } => a,
                AliasProperty { Property: AssociationProperty { Association: Class a } } => a,
                _ => null,
            };

        public IProperty? AssociationProperty =>
            prop switch
            {
                AssociationProperty ap => ap.Property,
                AliasProperty { Property: AssociationProperty ap } => ap.Property,
                _ => null,
            };

        public string? AssociationRole =>
            prop switch
            {
                AssociationProperty ap => ap.Role,
                AliasProperty { Property: AssociationProperty ap } => ap.Role,
                _ => null,
            };

        public AssociationType? AssociationType =>
            prop switch
            {
                AssociationProperty ap => ap.Type,
                AliasProperty { Property: AssociationProperty ap } => ap.Type,
                _ => null,
            };

        public Class? Composition =>
            prop switch
            {
                CompositionProperty { Composition: Class c } => c,
                AliasProperty { Composition: Class c } => c,
                _ => null,
            };

        public IProperty? CompositionPrimaryKey
        {
            get
            {
                if (prop.Composition == null)
                {
                    return null;
                }

                var cpPks = prop.Composition!.ExtendedProperties.Where(p => p.PrimaryKey);
                if (!cpPks.Any())
                {
                    cpPks = prop.Composition!.ExtendedProperties.OfType<AliasProperty>()
                        .Where(p => p.AliasedPrimaryKey);
                }

                return cpPks.Count() == 1 ? cpPks.Single() : null;
            }
        }

        public IEnumerable<(Domain Domain, bool Generic)> DomainChain
        {
            get
            {
                var op = (prop as AliasProperty)?.OriginalProperty;

                if (
                    prop.Domain != null
                    && (op == null || op.Domain != prop.Domain || prop is AliasProperty { As: not null })
                )
                {
                    yield return (
                        prop.Domain,
                        prop
                            is AliasProperty { As: not null }
                                or { AssociationType: AssociationType.OneToMany or AssociationType.ManyToMany }
                    );
                }

                if (op != null)
                {
                    foreach (var d in op.DomainChain)
                    {
                        yield return d;
                    }
                }
                else if (
                    prop is AssociationProperty ap
                    && ap.IsAssociationToMany()
                    && ap.Property.Domain != prop.Domain
                )
                {
                    yield return (ap.Property.Domain, false);
                }
            }
        }

        public IProperty? EnumProperty =>
            prop switch
            {
                AssociationProperty a => a.Property,
                AliasProperty { Property: AssociationProperty a } => a.Property,
                AliasProperty alp => alp.Property,
                { Composition: not null } => null,
                _ => prop,
            };

        public bool IsAssociationToMany()
        {
            return prop.AssociationType == AssociationType.OneToMany
                || prop.AssociationType == AssociationType.ManyToMany;
        }
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

    internal static string GetSqlName(IProperty? property)
    {
        return property switch
        {
            { Class.Extends: not null, PrimaryKey: true } when property.Name.StartsWith(property.Class.Name) => property
                .Name[property.Class.Name.Length..]
                .ToConstantCase(),
            AssociationProperty ap => ap.RawSqlName,
            AliasProperty { Property: AssociationProperty ap } => ap.RawSqlName,
            _ => (property?.Name ?? string.Empty).ToConstantCase(),
        };
    }

    internal static string GetSqlTrigram(string? trigram)
    {
        return (!string.IsNullOrWhiteSpace(trigram) ? $"{trigram}_" : string.Empty).ToConstantCase();
    }
}
