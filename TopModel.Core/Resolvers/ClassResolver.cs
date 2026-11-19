using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Resolvers;

internal class ClassResolver(
    IList<ModelFile> modelFiles,
    IDictionary<string, Class> referencedClasses,
    TranslationStore translationStore
) : BaseResolver
{
    /// <summary>
    /// Effectue les vérifications de cohérence sur le résultat de la résolution des classes.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> CheckResult()
    {
        foreach (var modelFile in modelFiles)
        {
            foreach (var classe in modelFile.Classes)
            {
                if (classe.IsPersistent)
                {
                    if (
                        classe.Type == ClassType.Abstract
                        && classe.InheritanceStrategy == InheritanceStrategy.JoinedTables
                    )
                    {
                        yield return ModelError.Error(ErrorType.TMD3024, [classe.Name], classe);
                    }

                    if (
                        classe.PrimaryKey.Count() == 1
                        && classe.PrimaryKey.Single().GeneratedValue != null
                        && classe.PrimaryKey.Single().GeneratedValue?.SequenceName == null
                        && classe.InheritanceStrategy == InheritanceStrategy.DistinctTables
                    )
                    {
                        yield return ModelError.Error(ErrorType.TMD3025, [classe.Name], classe);
                    }

                    if (classe.Extends != null && classe.Extends.ExtendedProperties.Count(p => p.PrimaryKey) != 1)
                    {
                        yield return ModelError.Error(
                            ErrorType.TMD3009,
                            [classe.Name, classe.Extends.Name],
                            classe,
                            classe.ExtendsReference
                        );
                    }
                    // Doublons de propriétés.
                    foreach (var property in classe.ExtendedProperties.GetDuplicates(p => p.NamePascal))
                    {
                        yield return ModelError.Error(
                            ErrorType.TMD0001,
                            [property.Name],
                            modelFile,
                            property.SourceContainer != null
                                ? classe
                                    .DecoratorReferences.Cast<Reference>()
                                    .Concat(classe.ImplementReferences)
                                    .FirstOrDefault(r => r.Value == property.SourceContainer!.Name)
                                : property.Declaration
                        );
                    }

                    // Doublons d'associations.
                    foreach (
                        var property in classe
                            .Properties.OfType<AssociationProperty>()
                            .Where(p =>
                                (p.Association == classe || p.Association == classe.Extends)
                                && string.IsNullOrEmpty(p.Role)
                            )
                    )
                    {
                        yield return ModelError.Error(
                            ErrorType.TMD3005,
                            [classe.Name],
                            modelFile,
                            property.Decorator is not null
                                ? classe.DecoratorReferences.FirstOrDefault(dr => dr.Value == property.Decorator.Name)
                                : property.Declaration
                        );
                    }

                    // Vérification de la cohérence des enums.
                    if (classe.Enum == EnumMode.Enum)
                    {
                        foreach (
                            var prop in classe.Properties.Where(c =>
                                c.Composition != null && c.Composition?.Enum != EnumMode.Enum
                                || c.Association != null && c.Association?.Enum != EnumMode.Enum
                            )
                        )
                        {
                            yield return ModelError.Error(
                                ErrorType.TMD3020,
                                [(prop.Composition ?? prop.Association)!.Name, classe.Name],
                                prop
                            );
                        }
                    }
                    else if (classe.Enum == EnumMode.Class && classe.Readonly)
                    {
                        foreach (
                            var prop in classe.Properties.Where(c =>
                                c.Composition != null
                                    && (c.Composition?.Enum == null || c.Composition?.Readonly != true)
                                || c.Association != null
                                    && c.UseClassForAssociation
                                    && (c.Association?.Enum == null || c.Association?.Readonly != true)
                            )
                        )
                        {
                            yield return ModelError.Error(
                                ErrorType.TMD3020,
                                [(prop.Composition ?? prop.Association)!.Name, classe.Name],
                                prop
                            );
                        }
                    }

                    // Vérification des valeurs.
                    foreach (
                        var property in classe.Properties.Where(p =>
                            p.DefaultValue != null
                            && p.DefaultValue != ClassValue.Null
                            && p.DefaultValue != ClassValue.Undefined
                            && (
                                p.AssociationMultiple
                                || p.UseClassForAssociation
                                    && p.ReadonlyEnumClassAssociation == null
                                    && p.TrueEnumClassAssociation == null
                            )
                        )
                    )
                    {
                        yield return ModelError.Error(
                            ErrorType.TMD9013,
                            [property.Name, classe.Name],
                            classe,
                            property.Declaration
                        );
                    }

                    foreach (var value in classe.Values)
                    {
                        foreach (var (property, v) in value.Value)
                        {
                            if (
                                v != ClassValue.Null
                                && v != ClassValue.Undefined
                                && (
                                    property.AssociationMultiple
                                    || classe.Enum != null
                                        && property.UseClassForAssociation
                                        && property.ReadonlyEnumClassAssociation == null
                                        && property.TrueEnumClassAssociation == null
                                )
                            )
                            {
                                yield return ModelError.Error(
                                    ErrorType.TMD9013,
                                    [property.Name, classe.Name],
                                    classe,
                                    classe
                                        .ValueReferences[value.Reference]
                                        .Keys.SingleOrDefault(r => r.Value == property.Name)
                                );
                            }
                        }
                    }

                    // Doublons d'indexes
                    foreach (
                        var index in classe.Indexes.Where(
                            (idx1, i) =>
                                classe
                                    .Indexes.Where(
                                        (idx2, j) =>
                                            idx2.Properties.OrderBy(p => p.Name)
                                                .SequenceEqual(idx1.Properties.OrderBy(p => p.Name))
                                            && j < i
                                    )
                                    .Any()
                        )
                    )
                    {
                        yield return ModelError.Error(
                            ErrorType.TMD3021,
                            [classe.Name, string.Join(", ", index.Properties.Select(p => p.Name))],
                            modelFile,
                            index.PropertyReferences.ElementAtOrDefault(0) as Location ?? classe.Declaration
                        );
                    }
                }
            }

            // Vérification des contraintes d'unicités
            foreach (
                var classe in modelFiles
                    .SelectMany(mf => mf.Classes)
                    .Where(c => c.Values.Count > 0 && (c.IsPersistent || c.Indexes.Any(idx => idx.Unique)))
            )
            {
                var uks = new List<IEnumerable<IProperty>>();
                uks.AddRange(classe.Indexes.Where(idx => idx.Unique).Select(idx => idx.Properties));
                if (classe.IsPersistent)
                {
                    uks.Add(classe.PrimaryKey.Any() ? classe.PrimaryKey : (classe.Extends?.PrimaryKey ?? []));
                }

                foreach (var uk in uks)
                {
                    if (!classe.Values.All(value => uk.All(p => value.Value.ContainsKey(p))))
                    {
                        continue;
                    }

                    var ukValues = classe
                        .Values.Select(value => string.Concat(uk.Select(p => value.Value[p])))
                        .ToList();
                    for (int i = 0; i < classe.Values.Count; i++)
                    {
                        var ukValue = string.Concat(uk.Select(p => classe.Values[i].Value[p]));
                        if (ukValues.IndexOf(ukValue) < i)
                        {
                            var duplicateValue = classe.Values[i];
                            yield return ModelError.Error(
                                ErrorType.TMD3012,
                                [string.Join(", ", uk.Select(u => u.Name))],
                                duplicateValue,
                                duplicateValue.Reference
                            );
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Vérifie `reference` et `enum`.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveEnums()
    {
        foreach (var classe in modelFiles.SelectMany(mf => mf.Classes))
        {
            if (classe.Reference && classe.ReferenceKey == null)
            {
                yield return ModelError.Error(ErrorType.TMD3002, [classe.Name], classe);
            }

            if (classe.EnumOverride != null)
            {
                if (classe.EnumOverride != "false" && classe.Values.Count == 0)
                {
                    yield return ModelError.Error(
                        ErrorType.TMD3003,
                        [classe.Name],
                        classe,
                        classe.EnumOverride.Declaration
                    );
                }
                else
                {
                    classe.Enum = classe.EnumOverride.Value switch
                    {
                        "false" => null,
                        "class" => EnumMode.Class,
                        "true" => EnumMode.Enum,
                        _ => throw new InvalidOperationException(),
                    };
                }
            }
            else
            {
                classe.Enum =
                    classe.Values.Count > 0 && classe.ReferenceKey != null && classe.ReferenceKey.GeneratedValue == null
                        ? EnumMode.Class
                        : null;
            }

            if (classe.Extends != null && classe.Enum != classe.Extends.Enum)
            {
                yield return ModelError.Error(ErrorType.TMD3010, [classe.Name, classe.Extends.Name], classe);
            }

            if (classe.Enum == EnumMode.Enum && classe.ReferenceKey?.EnumProperty == null)
            {
                yield return ModelError.Error(ErrorType.TMD3019, [classe.Name], classe);
            }

            if (classe.Enum == EnumMode.Enum)
            {
                classe.Readonly = true;
            }
        }
    }

    /// <summary>
    /// Résout les "extends".
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveExtends()
    {
        foreach (var classe in modelFiles.SelectMany(mf => mf.Classes).Where(c => c.ExtendsReference != null))
        {
            if (!referencedClasses.TryGetValue(classe.ExtendsReference!.Value, out var extends))
            {
                yield return ModelError.Error(
                    ErrorType.TMD0002,
                    [classe.ExtendsReference.Value],
                    classe,
                    classe.ExtendsReference
                );
                continue;
            }

            if (
                (classe.Type == ClassType.Interface || extends.Type == ClassType.Interface)
                && extends.Type != classe.Type
            )
            {
                yield return ModelError.Error(
                    ErrorType.TMD3006,
                    [classe.Name, classe.ExtendsReference.Value],
                    classe,
                    classe.ExtendsReference
                );
                continue;
            }

            classe.Extends = extends;
        }
    }

    public IEnumerable<ModelError> ResolveImplements()
    {
        foreach (var classe in modelFiles.SelectMany(mf => mf.Classes))
        {
            classe.Implements.Clear();

            foreach (var implementReference in classe.ImplementReferences)
            {
                if (!referencedClasses.TryGetValue(implementReference.Value, out var implements))
                {
                    yield return ModelError.Error(
                        ErrorType.TMD0002,
                        [implementReference.Value],
                        classe,
                        implementReference
                    );
                    continue;
                }

                if (implements.Type != ClassType.Interface)
                {
                    yield return ModelError.Error(
                        ErrorType.TMD3007,
                        [implementReference.Value],
                        classe,
                        implementReference
                    );
                    continue;
                }

                if (classe.Type == ClassType.Interface)
                {
                    yield return ModelError.Error(
                        ErrorType.TMD3008,
                        [implementReference.Value],
                        classe,
                        implementReference
                    );
                    continue;
                }

                var existing = new List<Class>(classe.Implements);
                var parentClass = classe.Extends;
                while (parentClass != null)
                {
                    existing.AddRange(parentClass.Implements);
                    parentClass = parentClass.Extends;
                }

                if (existing.Contains(implements))
                {
                    yield return ModelError.Error(
                        ErrorType.TMD3022,
                        [implementReference.Value, classe.Name],
                        classe,
                        implementReference
                    );
                    continue;
                }

                classe.Implements.Add(implements);
            }
        }
    }

    /// <summary>
    /// Résout les clés d'unicité.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveIndexes()
    {
        foreach (var classe in modelFiles.SelectMany(mf => mf.Classes).Where(c => c.Indexes.Count > 0))
        {
            foreach (var index in classe.Indexes)
            {
                index.Properties.Clear();

                foreach (var propRef in index.PropertyReferences)
                {
                    var property = classe.Properties.FirstOrDefault(p => p.Name == propRef.Value);

                    if (property == null)
                    {
                        yield return ModelError.Error(ErrorType.TMD0004, [propRef.Value, classe.Name], classe, propRef);
                    }
                    else
                    {
                        index.Properties.Add(property);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Résout les propriétés spéciales de classe.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveSpecialProperties()
    {
        foreach (var classe in modelFiles.SelectMany(mf => mf.Classes))
        {
            var properties = classe
                .ExtendedProperties.GroupBy(p => p.NamePascal)
                .ToDictionary(p => p.Key, p => p.First());

            IProperty? TryGetProperty(params IEnumerable<string> names)
            {
                return names
                    .Select(name => properties.TryGetValue(name, out var p) ? p : null)
                    .FirstOrDefault(n => n != null);
            }

            if (classe.DefaultPropertyReference != null)
            {
                classe.DefaultProperty = TryGetProperty(classe.DefaultPropertyReference.Value.ToPascalCase());

                if (classe.DefaultProperty == null)
                {
                    yield return ModelError.Error(
                        ErrorType.TMD0004,
                        [classe.DefaultPropertyReference.Value, classe.Name],
                        classe,
                        classe.DefaultPropertyReference
                    );
                }
            }
            else
            {
                // Si la classe a une propriété "Label" ou "Libelle", alors on la considère par défaut (sic) comme propriété par défaut.
                classe.DefaultProperty = TryGetProperty("Label", "Libelle");
            }

            if (classe.OrderPropertyReference != null)
            {
                classe.OrderProperty = TryGetProperty(classe.OrderPropertyReference.Value.ToPascalCase());

                if (classe.OrderProperty == null)
                {
                    yield return ModelError.Error(
                        ErrorType.TMD0004,
                        [classe.OrderPropertyReference.Value, classe.Name],
                        classe,
                        classe.OrderPropertyReference
                    );
                }
            }
            else
            {
                // Si la classe a une propriété "Order" ou "Ordre", alors on la considère par défaut comme propriété d'ordre.
                classe.OrderProperty = TryGetProperty("Order", "Ordre");
            }

            if (classe.FlagPropertyReference != null)
            {
                classe.FlagProperty = TryGetProperty(classe.FlagPropertyReference.Value.ToPascalCase());

                if (classe.FlagProperty == null)
                {
                    yield return ModelError.Error(
                        ErrorType.TMD0004,
                        [classe.FlagPropertyReference.Value, classe.Name],
                        classe,
                        classe.FlagPropertyReference
                    );
                }
            }
            else
            {
                // Si la classe a une propriété "Flag", alors on la considère par défaut comme propriété de flag.
                classe.FlagProperty = TryGetProperty("Flag");
            }

            if (classe.LocalePropertyReference != null)
            {
                classe.LocaleProperty = TryGetProperty(classe.LocalePropertyReference.Value.ToPascalCase());

                if (classe.LocaleProperty == null)
                {
                    yield return ModelError.Error(
                        ErrorType.TMD0004,
                        [classe.LocalePropertyReference.Value, classe.Name],
                        classe,
                        classe.LocalePropertyReference
                    );
                }
            }
            else
            {
                // Si la classe a une propriété "Locale", alors on la considère par défaut comme propriété de locale.
                classe.LocaleProperty = TryGetProperty("Locale");
            }

            if (classe.InheritanceStrategy == InheritanceStrategy.SingleTable)
            {
                if (classe.DiscriminatorPropertyReference != null)
                {
                    classe.DiscriminatorProperty = TryGetProperty(
                        classe.DiscriminatorPropertyReference.Value.ToPascalCase()
                    );

                    if (classe.DiscriminatorProperty == null)
                    {
                        yield return ModelError.Error(
                            ErrorType.TMD0004,
                            [classe.DiscriminatorPropertyReference.Value, classe.Name],
                            classe,
                            classe.DiscriminatorPropertyReference
                        );
                    }
                }
                else
                {
                    classe.DiscriminatorProperty = new RegularProperty(new Declaration())
                    {
                        OwnName = new LocatedString(new Scalar("Discriminator")),
                        Class = classe,
                        Required = true,
                        Comment = CoreMessage.DiscriminatorPropertyComment.GetLocalizedMessage(
                            classe.ModelFile.Language
                        ),
                    };
                }
            }

            if (classe.Translation)
            {
                if (classe.DefaultProperty == null)
                {
                    yield return ModelError.Error(ErrorType.TMD3014, [], classe);
                }

                if (classe.LocaleProperty != null)
                {
                    if (!classe.LocaleProperty.PrimaryKey || classe.PrimaryKey.Count() != 2)
                    {
                        yield return ModelError.Error(ErrorType.TMD3015, [], classe);
                    }
                }
                else
                {
                    if (classe.PrimaryKey.Count() != 1)
                    {
                        yield return ModelError.Error(ErrorType.TMD3016, [], classe);
                    }

                    if (translationStore.Translations.Keys.Count > 1)
                    {
                        yield return ModelError.Error(ErrorType.TMD3017, [], classe);
                    }
                }

                if (
                    classe.Properties.Any(p =>
                        p.Required && !p.PrimaryKey && p != classe.DefaultProperty && p != classe.LocaleProperty
                    )
                )
                {
                    yield return ModelError.Error(ErrorType.TMD3018, [], classe);
                }
            }
        }
    }

    /// <summary>
    /// Résout les traductions.
    /// </summary>
    /// <param name="translationStore">Store de traduction.</param>
    /// <param name="defaultLang">Langue par défaut.</param>
    public void ResolveTranslations(TranslationStore translationStore, string defaultLang)
    {
        foreach (var classe in modelFiles.SelectMany(mf => mf.Classes))
        {
            foreach (var p in classe.Properties.Where(p => p.Label != null))
            {
                translationStore.AddTranslation(defaultLang, p.ResourceKey, p.Label!);
            }

            if (classe.DefaultProperty != null)
            {
                foreach (var r in classe.Values)
                {
                    if (r.Value.TryGetValue(classe.DefaultProperty, out var labelProperty))
                    {
                        translationStore.AddTranslation(defaultLang, r.ResourceKey, labelProperty);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Résout les valeurs de classes.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveValues()
    {
        foreach (var classe in modelFiles.SelectMany(mf => mf.Classes).Where(c => c.ValueReferences.Count > 0))
        {
            if (classe.Type != ClassType.Regular)
            {
                yield return ModelError.Error(ErrorType.TMD3026, [classe.Name], classe);
                continue;
            }

            classe.Values.Clear();

            foreach (var valueRef in classe.ValueReferences)
            {
                var classValue = new ClassValue
                {
                    Name = valueRef.Key.Value,
                    Class = classe,
                    Reference = valueRef.Key,
                };
                classe.Values.Add(classValue);

                foreach (var value in valueRef.Value)
                {
                    var property = classe.ExtendedProperties.FirstOrDefault(p => p.Name == value.Key.Value);

                    if (property == null)
                    {
                        yield return ModelError.Error(
                            ErrorType.TMD0004,
                            [value.Key.Value, classe.Name],
                            classe,
                            value.Key
                        );
                    }
                    else
                    {
                        classValue.Value.Add(property, value.Value);
                    }
                }

                var missingRequiredProperties = classe.ExtendedProperties.Where(p =>
                    p.Required
                    && (p.GeneratedValue == null || p.Association != null)
                    && !p.AssociationMultiple
                    && !valueRef.Value.Any(v => v.Key.Value == p.Name)
                );

                if (missingRequiredProperties.Any())
                {
                    yield return ModelError.Error(
                        ErrorType.TMD3011,
                        [valueRef.Key.Value, string.Join(", ", missingRequiredProperties.Select(p => p.Name))],
                        classe,
                        valueRef.Key
                    );
                }
            }
        }
    }
}
