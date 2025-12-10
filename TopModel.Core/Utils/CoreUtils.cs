using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Utils;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Utils;

#pragma warning disable KTA1200, S2325 // Jusqu'à ce qu'on supporte les blocs d'extension...

public static class CoreUtils
{
    extension(IProperty prop)
    {
        /// <summary>
        /// Si la propriété est une association, classe cible de l'association.
        /// </summary>
        public Class? Association =>
            prop switch
            {
                AssociationProperty { Association: Class a } => a,
                AliasProperty { Property: AssociationProperty { Association: Class a } } => a,
                _ => null,
            };

        /// <summary>
        /// Si la propriété est une association, propriété de la classe cible sur laquelle porte l'association.
        /// </summary>
        public IProperty? AssociationProperty =>
            prop switch
            {
                AssociationProperty ap => ap.Property,
                AliasProperty { Property: AssociationProperty ap } => ap.Property,
                _ => null,
            };

        /// <summary>
        /// Si la propriété est une association, rôle de l'association.
        /// </summary>
        public string? AssociationRole =>
            prop switch
            {
                AssociationProperty ap => ap.Role,
                AliasProperty { Property: AssociationProperty ap } => ap.Role,
                _ => null,
            };

        /// <summary>
        /// Si la propriété est une association, type de l'association.
        /// </summary>
        public AssociationType? AssociationType =>
            prop switch
            {
                AssociationProperty ap => ap.Type,
                AliasProperty { Property: AssociationProperty ap } => ap.Type,
                _ => null,
            };

        /// <summary>
        /// Si la propriété est une composition, classe cible de la composition.
        /// </summary>
        public Class? Composition =>
            prop switch
            {
                CompositionProperty { Composition: Class c } => c,
                AliasProperty { Composition: Class c } => c,
                _ => null,
            };

        /// <summary>
        /// Si la propriété est une composition, clé primaire de la classe cible de la composition.
        /// </summary>
        public IProperty? CompositionPrimaryKey
        {
            get
            {
                if (prop.Composition == null)
                {
                    return null;
                }

                var cpPks = prop.Composition!.ExtendedProperties.Where(p => p.PrimaryKeyish);
                return cpPks.Count() == 1 ? cpPks.Single() : null;
            }
        }

        /// <summary>
        /// Hiérarchie des domaines de la propriété.
        /// </summary>
        /// <remarks>(Une propriété peut être construite à partir de plusieurs domaines avec des alias 'as' ou des associations 'toMany'.)</remarks>
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

        /// <summary>
        /// Propriété utilisée pour la détermination du type enum de la propriété.
        /// </summary>
        /// <remarks>(Cela peut être la propriété cible d'une association, où la propriété originale d'un alias.)</remarks>
        public IProperty? EnumProperty =>
            prop switch
            {
                { AssociationProperty: IProperty ap } => ap,
                AliasProperty alp => alp.Property,
                { Composition: not null } => null,
                _ => prop,
            };

        /// <summary>
        /// Si la propriété est une association ManyToMany, retourne la classe de liaison implicite entre les deux classes associées.
        /// </summary>
        public Class? ManyToManyClass
        {
            get
            {
                if (prop.AssociationType == AssociationType.ManyToMany)
                {
                    var traClass = new Class
                    {
                        Comment = prop.Comment,
                        Label = prop.Label,
                        SqlName =
                            $"{prop.Class.SqlName}_{prop.Association!.SqlName}{(prop.AssociationRole != null ? $"_{prop.AssociationRole!.ToConstantCase()}" : string.Empty)}",
                        ModelFile = prop.Class.ModelFile,
                    };

                    traClass.Properties.Add(
                        new AssociationProperty
                        {
                            Association = prop.Class,
                            Class = traClass,
                            Comment = prop.Comment,
                            Type = AssociationType.ManyToOne,
                            PrimaryKey = true,
                            Required = true,
                            Role = prop.AssociationRole,
                            DefaultValue = prop.DefaultValue,
                            Label = prop.Label,
                            Trigram = prop.Class.PrimaryKey.Single().Trigram,
                        }
                    );

                    traClass.Properties.Add(
                        new AssociationProperty
                        {
                            Association = prop.Association,
                            Class = traClass,
                            Comment = prop.Comment,
                            Type = AssociationType.ManyToOne,
                            PrimaryKey = true,
                            Required = true,
                            Role = prop.AssociationRole,
                            DefaultValue = prop.DefaultValue,
                            Label = prop.Label,
                            Trigram = new LocatedString(new Scalar(prop.FinalTrigram ?? string.Empty)),
                        }
                    );

                    return traClass;
                }

                return null;
            }
        }

        /// <summary>
        /// Classe persistante de laquelle est issue la propriété.
        /// </summary>
        /// <remarks>(Pour un alias de propriété persistée sur une classe non persistée, il s'agira de la classe de la propriété originale.)</remarks>
        public Class? PersistentClass =>
            prop switch
            {
                { Class: { IsPersistent: true } c } => c,
                AliasProperty { PersistentProperty.Class: Class c } => c,
                _ => null,
            };

        /// <summary>
        /// Si la propriété doit être considérée comme une clé primaire.
        /// </summary>
        /// <remarks>Cela permet de considérer des alias de clé primaire comme des clés primaires.</remarks>
        public bool PrimaryKeyish =>
            prop.PrimaryKey
            || prop
                is AliasProperty
                {
                    PreservePrimaryKey: false,
                    Prefix: null,
                    Suffix: null,
                    OriginalProperty.PrimaryKeyish: true
                };

        /// <summary>
        /// Classe de référence de laquelle la propriété est la clé primaire.
        /// </summary>
        /// <remarks>(Pour un alias, on regardera aussi la propriété originale et sa classe.)</remarks>
        public Class? ReferenceClass =>
            prop switch
            {
                { PrimaryKey: true, Class: { Reference: true } c } => c,
                AliasProperty { Property: { PrimaryKey: true, Class: { Reference: true } c } } => c,
                _ => null,
            };

        /// <summary>
        /// Vérifie si la propriété est une association de type 'toMany'.
        /// </summary>
        /// <returns>Oui/non.</returns>
        public bool IsAssociationToMany()
        {
            return prop.AssociationType == AssociationType.OneToMany
                || prop.AssociationType == AssociationType.ManyToMany;
        }
    }

    extension(Class classe)
    {
        /// <summary>
        /// Association vers la classe parente pour une classe dérivée.
        /// </summary>
        public IProperty? ParentAssociationProperty =>
            classe.Extends != null
                ? new AssociationProperty
                {
                    Association = classe.Extends,
                    Class = classe,
                    Comment = "Association vers la clé primaire de la classe parente",
                    Required = true,
                    PrimaryKey = !classe.PrimaryKey.Any(),
                }
                : null;
    }

    /// <summary>
    /// Vérifie si le type d'association est un type 'toMany'.
    /// </summary>
    /// <returns>Oui/non.</returns>
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
