using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core.Model;

public static class PropertyExtensions
{
    extension(IProperty prop)
    {
        /// <summary>
        /// Nom de la propriété.
        /// </summary>
        public string Name => prop.NameSegments.Name;

        /// <summary>
        /// Nom de la propriété en PascalCase.
        /// </summary>
        public string NamePascal => prop.Parent.PreservePropertyCasing ? prop.Name : prop.NameSegments.NamePascal;

        /// <summary>
        /// Nom de la propriété en camelCase.
        /// </summary>
        public string NameCamel => prop.Parent.PreservePropertyCasing ? prop.Name : prop.NamePascal.ToFirstLower();

        /// <summary>
        /// Nom de la propriété (y compris avec useClass: true) en PascalCase.
        /// </summary>
        public string PropertyNamePascal =>
            prop.Parent.PreservePropertyCasing ? prop.Name : prop.PropertyNameSegments.NamePascal;

        /// <summary>
        /// Nom de la propriété (y compris avec useClass: true) en camelCase.
        /// </summary>
        public string PropertyNameCamel =>
            prop.Parent.PreservePropertyCasing ? prop.Name : prop.PropertyNamePascal.ToFirstLower();

        /// <summary>
        /// Conteneur source de la propriété.
        /// </summary>
        public IPropertyContainer? SourceContainer => prop.SourceProperty?.Parent;

        /// <summary>
        /// Parent de la propriété.
        /// </summary>
        public IPropertyContainer Parent =>
            prop.Class
            ?? (IPropertyContainer)prop.Endpoint!
            ?? (IPropertyContainer)prop.Decorator!
            ?? prop.PropertyMapping!;

        /// <summary>
        /// Propriété à utiliser pour le libellé de la propriété.
        /// </summary>
        public IProperty ResourceProperty =>
            prop.SourceContainer != null
                ? prop.SourceContainer!.Properties.First(p => p.Name == prop.Name).ResourceProperty
            : prop is AliasProperty { OverridesLabel: false, OriginalProperty: IProperty op } ? op.ResourceProperty
            : prop;

        /// <summary>
        /// Clé de ressource pour le libellé de la propriété.
        /// </summary>
        public string ResourceKey =>
            $"{prop.ResourceProperty.Parent.Namespace.ModuleCamel}.{prop.ResourceProperty.Parent.NameCamel}.{prop.ResourceProperty.PropertyNameCamel}";

        /// <summary>
        /// Propriété à utiliser pour le commentaire de la propriété.
        /// </summary>
        public IProperty CommentResourceProperty =>
            prop.SourceContainer != null
                ? prop.SourceContainer!.Properties.First(p => p.Name == prop.Name).CommentResourceProperty
            : prop is AliasProperty { OverridesComment: false, OriginalProperty: IProperty op }
                ? op.CommentResourceProperty
            : prop;

        /// <summary>
        /// Clé de ressource pour le commentaire de la propriété.
        /// </summary>
        public string CommentResourceKey =>
            $"comments.{prop.CommentResourceProperty.Parent.Namespace.ModuleCamel}.{prop.CommentResourceProperty.Parent.NameCamel}.{prop.CommentResourceProperty.PropertyNameCamel}";

        /// <summary>
        /// Définition de valeur générée pour la propriété.
        /// </summary>
        public GeneratedValueDefinition? GeneratedValue =>
            prop is not AliasProperty { PreservePrimaryKey: true } && prop.Association == null
                ? prop.Domain?.GeneratedValue
                : null;

        /// <summary>
        /// Si la propriété est une association, classe cible de l'association.
        /// </summary>
        public Class? Association =>
            prop switch
            {
                { Composition: not null } => null,
                AssociationProperty { Association: Class a } => a,
                AliasProperty { DiscardAssociations: false, Property: AssociationProperty { Association: Class a } } =>
                    a,
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
        public LocatedString? AssociationRole =>
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
                AliasProperty
                {
                    DiscardAssociations: true,
                    Property: AssociationProperty { Association.Enum: var e }
                } => e == EnumMode.Enum,
                AliasProperty { Property: AssociationProperty } alp => alp.UseClass,
                _ => false,
            };

        /// <summary>
        /// Si la propriété est une association vers une classe enum readonly, retourne cette classe.
        /// </summary>
        public Class? ReadonlyEnumClassAssociation =>
            prop switch
            {
                AssociationProperty { Association: Class { Readonly: true, Enum: EnumMode.Class } a, UseClass: true } =>
                    a,
                AliasProperty
                {
                    Property: AssociationProperty { Association: Class { Readonly: true, Enum: EnumMode.Class } a },
                    UseClass: true
                } => a,
                _ => null,
            };

        /// <summary>
        /// Si la propriété est une association vers une classe enum: true, retourne cette classe.
        /// </summary>
        public Class? TrueEnumClassAssociation =>
            prop switch
            {
                AssociationProperty { Association: Class { Enum: EnumMode.Enum } a } => a,
                AliasProperty { Property: AssociationProperty { Association: Class { Enum: EnumMode.Enum } a } } => a,
                _ => null,
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
                    || prop.Class.Indexes.Any(idx =>
                        idx.Unique && idx.Properties.Count == 1 && idx.Properties.Single() == prop
                    )
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
        /// Hiérarchie des domaines de la propriété.
        /// </summary>
        /// <remarks>(Une propriété peut être construite à partir de plusieurs domaines avec des alias 'as' ou des associations multiple.)</remarks>
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
                        prop.Domain.Domain,
                        prop is AliasProperty { As: not null } or { AssociationMultiple: true }
                    );
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
                    yield return (ap.Property.Domain.Domain, false);
                }
            }
        }

        /// <summary>
        /// Si la propriété est une enum, retourne la propriété qui la définit.
        /// </summary>
        public IProperty? EnumProperty
        {
            get
            {
                var elp = prop.UniqueValuedProperty;
                return
                    elp?.Class.Enum != null
                    && elp.Class.Values.All(r => r.Value.ContainsKey(elp) && r.Value[elp].IsEnumNameValid())
                    ? elp
                    : null;
            }
        }

        /// <summary>
        /// Si la propriété devrait être une enum mais ne peut pas l'être à cause d'un identifiant invalide, retourne la propriété qui la définit.
        /// </summary>
        public IProperty? EnumLikeProperty
        {
            get
            {
                var elp = prop.UniqueValuedProperty;
                return elp?.Class.Enum != null && elp.Class.Values.All(r => r.Value.ContainsKey(elp)) ? elp : null;
            }
        }

        /// <summary>
        /// Si la propriété fait référence à une propriété avec contrainte d'unicité dans une classe avec des valeurs, la propriété en question.
        /// </summary>
        public IProperty? UniqueValuedProperty
        {
            get
            {
                var sourceProp = prop switch
                {
                    { AssociationProperty: IProperty ap } => ap,
                    AliasProperty alp => alp.Property,
                    { Composition: not null } => null,
                    _ => prop,
                };

                var sourceClass = sourceProp?.Class;

                if (sourceProp == null || sourceClass == null)
                {
                    return null;
                }

                if (
                    sourceClass.Values.Any()
                    && (
                        sourceProp == sourceClass.ReferenceKey
                        || sourceClass
                            .Indexes.Where(idx => idx.Unique && idx.Properties.Count == 1)
                            .Select(idx => idx.Properties.Single())
                            .Contains(sourceProp)
                    )
                )
                {
                    return sourceProp;
                }

                return null;
            }
        }

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
            prop.Class?.PrimaryKey.Count() == 1 && prop.Class.PrimaryKey.Single() == prop
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
                { Association: Class { Reference: true } c, UseClassForAssociation: false } => c,
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
        /// Détermine le type de la propriété pour déterminer comment elle doit être mappée.
        /// </summary>
        public MappingType MappingType
        {
            get
            {
                var propMappingType =
                    prop.Domain != null
                        ? new PropertyMappingType(
                            prop.Domain.Domain,
                            prop.Domain?.Collection == true
                            && prop.DomainChain.Count() > 1
                            && prop.DomainChain.First().Generic
                                ? prop.DomainChain.ElementAt(1).Domain
                                : null
                        )
                        : null;

                var cpk =
                    prop.Composition != null && prop.Composition!.ExtendedProperties.Count(p => p.PrimaryKeyish) == 1
                        ? prop.Composition!.ExtendedProperties.Single(p => p.PrimaryKeyish)
                        : null;

                return prop switch
                {
                    { Composition: Class c, Domain.Collection: true } => new ClassCollectionMappingType(
                        c,
                        prop.Domain.Domain,
                        cpk
                    ),
                    { Composition: Class c } => new ClassMappingType(c, prop.Domain?.Domain, cpk),
                    { EnumProperty.Class.Enum: EnumMode.Enum } => propMappingType,
                    {
                        Association: Class c,
                        AssociationProperty: IProperty ap,
                        UseClassForAssociation: true,
                        AssociationMultiple: true,
                        Domain: not null
                    } => new ClassCollectionMappingType(c, prop.Domain.Domain, ap),
                    { Association: Class c, AssociationProperty: IProperty ap, UseClassForAssociation: true } =>
                        new ClassMappingType(c, prop.Domain?.Domain, ap),
                    _ => propMappingType,
                };
            }
        }

        /// <summary>
        /// Calcule les segments du nom d'une propriété d'association.
        /// </summary>
        /// <returns>Les segments (vide si ce n'est pas une association).</returns>
        internal IList<NameSegment> GetAssociationNameSegments(bool forcePropertyName = false)
        {
            var association = prop switch
            {
                { Composition: not null } => null,
                AssociationProperty { Association: Class a } => a,
                AliasProperty { Property: AssociationProperty { Association: Class a } } => a,
                _ => null,
            };

            if (association == null)
            {
                return [];
            }

            var nameSegments = new List<NameSegment>();

            var className = prop switch
            {
                AssociationProperty ap => ap.ClassName,
                AliasProperty { Property: AssociationProperty ap } => ap.ClassName,
                _ => null,
            };

            if (className != null)
            {
                nameSegments.Add(new NameSegment.Text(className));
            }
            else if (prop.AssociationMultiple)
            {
                nameSegments.Add(new NameSegment.ClassName(association, Plural: true));
            }
            else if (association.Extends == null || !association.PrimaryKey.Any())
            {
                nameSegments.Add(new NameSegment.ClassName(association));
            }

            if (
                !prop.AssociationMultiple
                && (!prop.UseClassForAssociation || forcePropertyName)
                && association.Enum != EnumMode.Enum
            )
            {
                if (prop.AssociationProperty?.Association != null)
                {
                    nameSegments.AddRange(
                        prop.AssociationProperty!.GetAssociationNameSegments(forcePropertyName: true)
                    );
                }
                else
                {
                    nameSegments.Add(new NameSegment.PropertyName(prop.AssociationProperty));
                }
            }

            if (!string.IsNullOrWhiteSpace(prop.AssociationRole))
            {
                nameSegments.Add(new NameSegment.Text(prop.AssociationRole!));
            }

            return nameSegments;
        }
    }
}
