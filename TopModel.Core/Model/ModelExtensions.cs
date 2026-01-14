using System.Text;
using TopModel.Utils;

namespace TopModel.Core.Model;

#pragma warning disable KTA1200, S2325 // Jusqu'à ce qu'on supporte les blocs d'extension...

public static class ModelExtensions
{
    extension(IProperty prop)
    {
        /// <summary>
        /// Si la propriété est une association, classe cible de l'association.
        /// </summary>
        public Class? Association =>
            prop switch
            {
                { Composition: not null } => null,
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
                { Composition: not null } => null,
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
                { Composition: not null } => null,
                AssociationProperty ap => ap.Role,
                AliasProperty { Property: AssociationProperty ap } => ap.Role,
                _ => null,
            };

        /// <summary>
        /// Vérifie si la propriété est une association multiple (oneToMany);
        /// </summary>
        /// <returns>Oui/non.</returns>
        public bool AssociationMultiple =>
            prop switch
            {
                { Composition: not null } => false,
                AssociationProperty ap => ap.Multiple,
                AliasProperty { Property: AssociationProperty ap } => ap.Multiple,
                _ => false,
            };

        /// <summary>
        /// Type l'association avec la classe cible plutôt qu'avec celui de la propriété cible.
        /// </summary>
        public bool UseClassForAssociation =>
            prop switch
            {
                AssociationProperty ap => ap.UseClass,
                AliasProperty { Property: AssociationProperty } alp => alp.UseClass,
                _ => false,
            };

        /// <summary>
        /// Vérifie si la propriété est la cible d'une contrainte d'unicité.
        /// </summary>
        /// <returns>Oui/non.</returns>
#pragma warning disable S2190
        public bool Unique =>
            prop is ReverseAssociationProperty { ReverseProperty.Multiple: false, ReverseProperty.Unique: true }
            || prop.Class != null
                && !prop.AssociationMultiple
                && (
                    prop.Class.PrimaryKey.Count() == 1 && prop.Class.PrimaryKey.First() == prop
                    || prop.Class.UniqueKeys.Any(uk => uk.Count == 1 && uk.Single() == prop)
                );
#pragma warning restore S2190

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
                    yield return (prop.Domain, prop is AliasProperty { As: not null } or { AssociationMultiple: true });
                }

                if (op != null)
                {
                    foreach (var d in op.DomainChain)
                    {
                        yield return d;
                    }
                }
                else if (prop is AssociationProperty { Multiple: true } ap && ap.Property.Domain != prop.Domain)
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
        /// Pour un alias, la propriété à partir de laquelle l'alias a été construit.
        /// </summary>
        public IProperty? OriginalProperty => (prop as AliasProperty)?.OriginalProperty;

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
        /// Si la propriété est une association, retourne son association réciproque.
        /// </summary>
        /// <remarks>(Un alias d'association n'hérite pas de son association réciproque.)</remarks>
        public IProperty? ReverseProperty => (prop as AssociationProperty)?.ReverseProperty;

        /// <summary>
        /// Si la propriété est une association réciproque.
        /// </summary>
        public bool IsReverseProperty =>
            prop is ReverseAssociationProperty || prop is AliasProperty ap && ap.Property is ReverseAssociationProperty;

        /// <summary>
        /// Calcule le nom d'une propriété d'association.
        /// </summary>
        /// <param name="pascalCase">Si on veut le nom en PascalCase.</param>
        /// <param name="forcePropertyName">Si le nom doit inclure le nom de la propriété cible, indépendemment de la valeur de `UseClass`.</param>
        /// <returns></returns>
        internal string GetAssociationName(bool pascalCase = false, bool forcePropertyName = false)
        {
            if (prop.Association == null)
            {
                return string.Empty;
            }

            var name = new StringBuilder();

            var className = prop switch
            {
                AssociationProperty ap => ap.ClassName,
                AliasProperty { Property: AssociationProperty ap } => ap.ClassName,
                _ => null,
            };

            if (className != null)
            {
                name.Append(pascalCase ? className.ToPascalCase(strictIfUppercase: true) : className);
            }
            else if (prop.AssociationMultiple)
            {
                name.Append(pascalCase ? prop.Association?.PluralNamePascal : prop.Association?.PluralName);
            }
            else if (prop.Association?.Extends == null || !(prop.Association?.PrimaryKey.Any() ?? false))
            {
                name.Append(pascalCase ? prop.Association?.NamePascal : prop.Association?.Name);
            }

            if (!prop.AssociationMultiple && (!prop.UseClassForAssociation || forcePropertyName))
            {
                name.Append(pascalCase ? prop.AssociationProperty?.NamePascal : prop.AssociationProperty?.Name);
            }

            if (!string.IsNullOrWhiteSpace(prop.AssociationRole))
            {
                var role = prop.AssociationRole?.Replace(" ", string.Empty);
                name.Append(pascalCase ? role?.ToPascalCase(strictIfUppercase: true) : role);
            }

            return name.ToString();
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
}
