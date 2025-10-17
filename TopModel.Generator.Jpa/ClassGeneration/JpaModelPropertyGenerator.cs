using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JpaModelPropertyGenerator(JpaConfig config, IDictionary<string, string> newableTypes)
{
    public JavaAnnotation EnumAnnotation =>
        new JavaAnnotation("Enumerated", imports: $"{JavaxOrJakarta}.persistence.Enumerated").AddAttribute(
            "value",
            "EnumType.STRING",
            $"{JavaxOrJakarta}.persistence.EnumType"
        );

    public virtual JavaAnnotation IdAnnotation => new("Id", imports: $"{JavaxOrJakarta}.persistence.Id");

    public virtual JavaAnnotation MapsIdAnnotation => new("MapsId", imports: $"{JavaxOrJakarta}.persistence.MapsId");

    protected JpaConfig Config { get; } = config;

    protected IDictionary<string, string> NewableTypes { get; } = newableTypes;

    protected virtual string JavaxOrJakarta => Config.JavaxOrJakarta;

    protected virtual JavaAnnotation NotNullAnnotation =>
        new("NotNull", imports: $"{JavaxOrJakarta}.validation.constraints.NotNull");

    protected virtual JavaAnnotation ValidAnnotation => new("Valid", imports: $"{JavaxOrJakarta}.validation.Valid");

    public virtual IEnumerable<IProperty> GetAvailableProperties(Class classe)
    {
        return classe.Properties.Where(p =>
            !(p is CompositionProperty cp && !Config.AvailableClasses.Contains(cp.Composition))
            && !(
                p is AliasProperty ap
                && ap.Property is CompositionProperty cp2
                && !Config.AvailableClasses.Contains(cp2.Composition)
            )
        );
    }

    public virtual JavaAnnotation GetColumnAnnotation(IProperty property)
    {
        JavaAnnotation column = new JavaAnnotation(
            "Column",
            imports: $"{JavaxOrJakarta}.persistence.Column"
        ).AddAttribute("name", $@"""{property.SqlName}""");
        if (property.Required)
        {
            column.AddAttribute("nullable", "false");
        }

        if (property.Domain != null)
        {
            if (property.Domain.Length != null)
            {
                if (Config.GetImplementation(property.Domain)?.Type?.ToUpper() == "STRING")
                {
                    column.AddAttribute("length", $"{property.Domain.Length}");
                }
                else
                {
                    column.AddAttribute("precision", $"{property.Domain.Length}");
                }
            }

            if (property.Domain.Scale != null)
            {
                column.AddAttribute("scale", $"{property.Domain.Scale}");
            }

            if (property.Domain.Implementations.TryGetValue("sql", out var value))
            {
                column.AddAttribute("columnDefinition", @$"""{value.Type}""");
            }
        }

        if (property is CompositionProperty && property.Domain is null)
        {
            column.AddAttribute("columnDefinition", @$"""jsonb""");
        }

        return column;
    }

    public virtual IEnumerable<JavaAnnotation> GetDomainAnnotations(IProperty property, string tag)
    {
        foreach (var (annotation, imports) in Config.GetAnnotations(property, tag))
        {
            yield return new JavaAnnotation(name: annotation, imports: imports.ToArray());
        }

        if (
            !property.Class.IsPersistent
            && !(property is AssociationProperty ap && ap.Type.IsToMany())
            && property.Domain != null
        )
        {
            var propertyType = GetPropertyType(property);
            List<string> sizePropertyValidateTypes = ["String", "CharSequence", "Set", "Map", "List", "Collection"];
            var shouldAddSizeAnnotation =
                property.Domain.Length != null
                && (sizePropertyValidateTypes.Contains(propertyType.Split("<")[0]) || propertyType.EndsWith("[]"));

            if (shouldAddSizeAnnotation)
            {
                yield return new JavaAnnotation(
                    name: "Size",
                    imports: $"{JavaxOrJakarta}.validation.constraints.Size"
                ).AddAttribute("max", value: property.Domain.Length.ToString()!);
            }

            // Techniquement Digit peut aussi être mis sur des chaînes de caractères, mais ce n'est pas forcément l'intention de l'utilisateurs
            List<string> digitPropertyValidateTypes =
            [
                "BigDecimal",
                "BigInteger",
                "byte",
                "short",
                "int",
                "long",
                "Byte",
                "Short",
                "Integer",
                "Long",
                "double",
                "Double",
            ];
            var shouldAddDigitsAnnotation =
                property.Domain.Length != null
                && property.Domain.Scale != null
                && digitPropertyValidateTypes.Contains(propertyType);
            if (shouldAddDigitsAnnotation)
            {
                var digitsAnnotation = new JavaAnnotation(
                    name: "Digits",
                    imports: $"{JavaxOrJakarta}.validation.constraints.Digits"
                )
                    .AddAttribute("integer", value: property.Domain.Length.ToString()!)
                    .AddAttribute("fraction", value: property.Domain.Scale.ToString()!);

                yield return digitsAnnotation;
            }
        }
    }

    public virtual JavaField GetField(IProperty property, string tag)
    {
        var javaField = new JavaField(GetPropertyType(property), GetPropertyName(property))
        {
            Comment = { property.Comment },
        };

        if (
            property is AliasProperty ap
            && Config.AvailableClasses.Contains(ap.Property.Class)
            && !(Config.EnumsAsEnums && Config.CanClassUseEnums(ap.Property.Class))
        )
        {
            var getter = $"#{GetGetterName(ap.Property)}()";
            javaField.Comment.Add(
                $"Alias of {{@link {ap.Property.Class.GetImport(Config, tag)}{getter} {ap.Property.Class.NamePascal}{getter}}}"
            );
        }
        IEnumerable<JavaAnnotation> annotations = GetAnnotations(property, tag);

        if (!property.PrimaryKey || property.Class.PrimaryKey.Count() <= 1)
        {
            annotations = GetDomainAnnotations(property, tag).Concat(annotations).ToList();
        }

        javaField.AddRange(annotations);
        javaField.DefaultValue = GetDefaultValue(property);
        javaField.Imports.AddRange(GetDefaultValueImports(property, tag));
        javaField.Imports.AddRange(property.GetTypeImports(Config, tag));
        return javaField;
    }

    public virtual IEnumerable<JavaField> GetFields(Class classe, string tag)
    {
        var availableProperties = GetAvailableProperties(classe);
        foreach (var property in availableProperties)
        {
            yield return GetField(property, tag);
        }
    }

    public virtual JavaMethod GetGetter(string tag, IProperty property)
    {
        var field = GetField(property, tag);
        var method = field.DefaultGetter;
        var genericType = field.Type.Split('<')[0];
        method.Imports.AddRange(property.GetTypeImports(Config, tag));
        if (NewableTypes.TryGetValue(genericType, out var newableType) && property.Class.IsPersistent)
        {
            method.Imports.Add($"java.util.{newableType}");
            method.Body.Clear();
            method
                .AddBodyLine($"if (this.{field.Name} == null) {{")
                .AddBodyLine(1, $"this.{field.Name} = new {newableType}<>();")
                .AddBodyLine($"}}");
            method.AddBodyLine(@$"return this.{field.Name};");
        }

        return method;
    }

    public virtual string GetGetterName(IProperty property)
    {
        var propertyName = GetPropertyName(property);
        var propertyType = GetPropertyType(property);
        var getterPrefix = propertyType == "boolean" ? "is" : "get";
        if (property.Class.PreservePropertyCasing)
        {
            return propertyName.ToFirstUpper().WithPrefix(getterPrefix);
        }

        return propertyName.ToPascalCase().WithPrefix(getterPrefix);
    }

    public IEnumerable<JavaAnnotation> GetJpaAssociationAnnotations(AssociationProperty property, string tag)
    {
        return property.Type switch
        {
            AssociationType.ManyToOne => GetManyToOneAnnotations(property, tag),
            AssociationType.OneToMany => GetOneToManyAnnotations(property),
            AssociationType.ManyToMany => GetManyToManyAnnotations(property),
            AssociationType.OneToOne => GetOneToOneAnnotations(property),
            _ => [],
        };
    }

    public virtual string GetPropertyName(IProperty property)
    {
        return UseClassForAssociation(property) ? property.NameByClassCamel : property.NameCamel;
    }

    public virtual string GetPropertyType(IProperty property)
    {
        return Config.GetType(property, UseClassForAssociation(property));
    }

    public virtual JavaMethod GetSetter(string tag, IProperty property)
    {
        return GetField(property, tag).DefaultSetter;
    }

    public virtual string GetSetterName(IProperty property)
    {
        var propertyName = GetPropertyName(property);
        if (property.Class.PreservePropertyCasing)
        {
            return propertyName.WithPrefix("set");
        }

        return propertyName.ToPascalCase().WithPrefix("set");
    }

    public bool ShouldWriteEnumAnnotation(IProperty property)
    {
        if (property is AliasProperty ap)
        {
            return Config.CanClassUseEnums(ap.Property.Class, ap.Property) && property.Class.IsPersistent;
        }
        else if (property is AssociationProperty asp && Config.EnumsAsEnums)
        {
            return Config.CanClassUseEnums(asp.Association) && property.Class.IsPersistent;
        }
        else
        {
            return Config.CanClassUseEnums(property.Class, property) && property.Class.IsPersistent;
        }
    }

    public virtual bool UseClassForAssociation(IProperty property)
    {
        var isAssociationToNotAvailableClass =
            property is AssociationProperty asp && !Config.AvailableClasses.Contains(asp.Association);
        var isAliasToAssociationToNotAvailableClass =
            property is AliasProperty alp
            && alp.Property is AssociationProperty asop
            && !Config.AvailableClasses.Contains(asop.Association);
        var isAssociationNotPersistent = property is AssociationProperty apr && !apr.Association.IsPersistent;
        return property.Class.IsPersistent
            && !isAssociationNotPersistent
            && !isAssociationToNotAvailableClass
            && !isAliasToAssociationToNotAvailableClass;
    }

    protected virtual IEnumerable<JavaAnnotation> GetAnnotations(CompositionProperty property, string tag)
    {
        if (property.Class.IsPersistent)
        {
            yield return GetConvertAnnotation(property, tag);
            yield return GetColumnAnnotation(property);
        }
        else
        {
            yield return ValidAnnotation;
            if (property.Required && !property.PrimaryKey)
            {
                yield return NotNullAnnotation;
            }
        }

        foreach (var a in GetDomainAnnotations(property, tag))
        {
            yield return a;
        }
    }

    protected virtual IEnumerable<JavaAnnotation> GetAnnotations(AliasProperty property, string tag)
    {
        if (property.Class.IsPersistent)
        {
            var shouldWriteAssociation =
                property.Property is AssociationProperty ap
                && ap.Association.IsPersistent
                && Config.AvailableClasses.Contains(ap.Association);
            if (property.PrimaryKey && property.Class.IsPersistent)
            {
                foreach (var a in GetIdAnnotations(property))
                {
                    yield return a;
                }
            }

            if (shouldWriteAssociation)
            {
                foreach (var a in GetJpaAssociationAnnotations((AssociationProperty)property.Property, tag))
                {
                    yield return a;
                }
            }
            else if (!(property.PrimaryKey && property.Class.PrimaryKey.Count() > 1))
            {
                yield return GetColumnAnnotation(property);
            }

            if (property.Property is CompositionProperty cp)
            {
                foreach (var a in GetAnnotations(cp, tag))
                {
                    yield return a;
                }
            }

            if (Config.CanClassUseEnums(property.Property.Class, property.Property))
            {
                yield return EnumAnnotation;
            }
        }
        else if (property.Required && !property.PrimaryKey)
        {
            yield return NotNullAnnotation;
        }
    }

    protected virtual IEnumerable<JavaAnnotation> GetAnnotations(AssociationProperty property, string tag)
    {
        if (property.Class.IsPersistent)
        {
            if (
                property.Association.IsPersistent
                && !(Config.EnumsAsEnums && Config.CanClassUseEnums(property.Property.Class, property.Property))
                && Config.AvailableClasses.Contains(property.Association)
            )
            {
                if (!property.PrimaryKey || property.Class.PrimaryKey.Count() <= 1)
                {
                    foreach (var a in GetJpaAssociationAnnotations(property, tag))
                    {
                        yield return a;
                    }
                }

                if (
                    (property.Type == AssociationType.ManyToMany || property.Type == AssociationType.OneToMany)
                    && property.Association.OrderProperty != null
                    && GetPropertyType(property).Contains("List")
                )
                {
                    yield return new JavaAnnotation(
                        "OrderBy",
                        $@"""{property.Association.OrderProperty.NameByClassCamel} ASC""",
                        $"{JavaxOrJakarta}.persistence.OrderBy"
                    );
                }
            }
            else
            {
                yield return GetColumnAnnotation(property);
                if (ShouldWriteEnumAnnotation(property))
                {
                    yield return EnumAnnotation;
                }
            }

            if (property.PrimaryKey)
            {
                foreach (var a in GetIdAnnotations(property))
                {
                    yield return a;
                }
            }
        }
    }

    protected virtual IEnumerable<JavaAnnotation> GetAnnotations(IProperty property)
    {
        if (property.Class.IsPersistent)
        {
            if (property.PrimaryKey)
            {
                foreach (var a in GetIdAnnotations(property))
                {
                    yield return a;
                }
            }

            if (!(property.PrimaryKey && property.Class.PrimaryKey.Count() > 1))
            {
                yield return GetColumnAnnotation(property);
            }

            if (ShouldWriteEnumAnnotation(property))
            {
                yield return EnumAnnotation;
            }
        }
        else if (property.Required && !property.PrimaryKey)
        {
            yield return NotNullAnnotation;
        }
    }

    protected virtual IEnumerable<JavaAnnotation> GetAnnotations(IProperty property, string tag)
    {
        return property switch
        {
            AliasProperty alp => GetAnnotations(alp, tag),
            AssociationProperty ap => GetAnnotations(ap, tag),
            CompositionProperty cp => GetAnnotations(cp, tag),
            IProperty ip => GetAnnotations(ip),
            _ => [],
        };
    }

    protected virtual IEnumerable<JavaAnnotation> GetAutogeneratedAnnotations(Class classe)
    {
        var autoGenerated = new JavaAnnotation(
            "GeneratedValue",
            imports: $"{JavaxOrJakarta}.persistence.GeneratedValue"
        );
        if (Config.Identity.Mode == IdentityMode.UUID)
        {
            autoGenerated.AddAttribute(
                "strategy",
                "GenerationType.UUID",
                $"{JavaxOrJakarta}.persistence.GenerationType"
            );
        }
        else if (Config.Identity.Mode == IdentityMode.IDENTITY)
        {
            autoGenerated.AddAttribute(
                "strategy",
                "GenerationType.IDENTITY",
                $"{JavaxOrJakarta}.persistence.GenerationType"
            );
        }
        else if (Config.Identity.Mode == IdentityMode.SEQUENCE)
        {
            var seqName = $"SEQ_{classe.SqlName}";
            autoGenerated
                .AddAttribute("strategy", "GenerationType.SEQUENCE", $"{JavaxOrJakarta}.persistence.GenerationType")
                .AddAttribute("generator", $@"""{seqName}""");
            var sequenceGenerator = new JavaAnnotation(
                "SequenceGenerator",
                imports: $"{JavaxOrJakarta}.persistence.SequenceGenerator"
            )
                .AddAttribute("sequenceName", $@"""{seqName}""")
                .AddAttribute("name", $@"""{seqName}""");
            if (Config.Identity.Start != null)
            {
                sequenceGenerator.AddAttribute("initialValue", $"{Config.Identity.Start}");
            }

            if (Config.Identity.Increment != null)
            {
                sequenceGenerator.AddAttribute("allocationSize", $"{Config.Identity.Increment}");
            }

            yield return sequenceGenerator;
        }

        yield return autoGenerated;
    }

    protected virtual JavaAnnotation GetConvertAnnotation(CompositionProperty property, string tag)
    {
        var convert = new JavaAnnotation("Convert", imports: $"{JavaxOrJakarta}.persistence.Convert");
        var import = Config
            .CompositionConverterCanonicalName.Replace("{class}", property.Composition.Name)
            .Replace(
                "{package}",
                Config.GetPackageName(property.Composition, Config.GetBestClassTag(property.Composition, tag))
            );
        convert.AddAttribute(
            "converter",
            $"{Config.CompositionConverterSimpleName.Replace("{class}", property.Composition.Name)}.class",
            import
        );
        return convert;
    }

    protected virtual string GetDefaultValue(IProperty property)
    {
        var defaultValue = Config.GetValue(property);
        if (property is AssociationProperty ap)
        {
            if (
                ap.Association.PrimaryKey.Count() == 1
                && Config.CanClassUseEnums(ap.Association, prop: ap.Association.PrimaryKey.Single())
                && defaultValue != "null"
            )
            {
                if (Config.EnumsAsEnums)
                {
                    return $"{defaultValue}";
                }
                else
                {
                    return $"new {ap.Association.NamePascal}({defaultValue})";
                }
            }

            return string.Empty;
        }
        else
        {
            var suffix = defaultValue != "null" ? $"{defaultValue}" : string.Empty;
            return suffix;
        }
    }

    protected virtual IEnumerable<string> GetDefaultValueImports(IProperty property, string tag)
    {
        var defaultValue = Config.GetValue(property);
        if (property is AssociationProperty ap)
        {
            if (
                ap.Association.PrimaryKey.Count() == 1
                && Config.CanClassUseEnums(ap.Association, prop: ap.Association.PrimaryKey.Single())
                && defaultValue != "null"
            )
            {
                return
                [
                    $"{Config.GetEnumPackageName(property.Class, Config.GetBestClassTag(property.Class, tag))}.{GetPropertyType(ap.Association.PrimaryKey.Single())}",
                ];
            }

            return [];
        }
        else
        {
            return Config.GetValueImports(property, tag);
        }
    }

    protected virtual IEnumerable<JavaAnnotation> GetIdAnnotations(IProperty property)
    {
        if (property is AssociationProperty && property.Class?.PrimaryKey.Count() == 1)
        {
            yield return MapsIdAnnotation;
        }
        else
        {
            yield return IdAnnotation;
            if (property.Domain.AutoGeneratedValue && property.Class?.PrimaryKey.Count() == 1)
            {
                foreach (var a in GetAutogeneratedAnnotations(property.Class))
                {
                    yield return a;
                }
            }
        }
    }

    protected virtual IEnumerable<JavaAnnotation> GetManyToManyAnnotations(AssociationProperty property)
    {
        var role = property.Role is not null ? "_" + property.Role.ToConstantCase() : string.Empty;
        var fk = ((IProperty)property).SqlName;
        var pk = property.Class.PrimaryKey.Single().SqlName + role;
        var association = new JavaAnnotation(
            $"{property.Type}",
            imports: $"{JavaxOrJakarta}.persistence.{property.Type}"
        );
        if (property.Type == AssociationType.ManyToOne || property.Type == AssociationType.OneToOne)
        {
            association.AddAttribute("fetch", "FetchType.LAZY", $"{JavaxOrJakarta}.persistence.FetchType");
        }

        if (!Config.CanClassUseEnums(property.Association))
        {
            association.AddAttribute(
                "cascade",
                "{ CascadeType.PERSIST, CascadeType.MERGE }",
                $"{JavaxOrJakarta}.persistence.CascadeType"
            );
        }

        if (property is ReverseAssociationProperty rap)
        {
            association.AddAttribute("mappedBy", $@"""{rap.ReverseProperty.NameByClassCamel}""");
        }

        yield return association;

        if (property is not ReverseAssociationProperty)
        {
            var joinColumns = new JavaAnnotation(
                "JoinColumn",
                imports: $"{JavaxOrJakarta}.persistence.JoinColumn"
            ).AddAttribute("name", $@"""{pk}""");
            var inverseJoinColumns = new JavaAnnotation(
                "JoinColumn",
                imports: $"{JavaxOrJakarta}.persistence.JoinColumn"
            ).AddAttribute("name", $@"""{fk}""");
            var joinTable = new JavaAnnotation("JoinTable", imports: $"{JavaxOrJakarta}.persistence.JoinTable")
                .AddAttribute(
                    "name",
                    $@"""{property.Class.SqlName}_{property.Association.SqlName}{(property.Role != null ? "_" + property.Role.ToConstantCase() : string.Empty)}"""
                )
                .AddAttribute("joinColumns", joinColumns)
                .AddAttribute("inverseJoinColumns", inverseJoinColumns);
            yield return joinTable;
        }
    }

    protected virtual IEnumerable<JavaAnnotation> GetManyToOneAnnotations(AssociationProperty property, string tag)
    {
        var association = new JavaAnnotation(
            @$"{property.Type}",
            imports: $"{JavaxOrJakarta}.persistence.{property.Type}"
        )
            .AddAttribute("fetch", "FetchType.LAZY", $"{JavaxOrJakarta}.persistence.FetchType")
            .AddAttribute("optional", property.Required ? "false" : "true")
            .AddAttribute(
                "targetEntity",
                $"{property.Association.NamePascal}.class",
                property.Association.GetImport(Config, Config.GetBestClassTag(property.Association, tag))
            );
        yield return association;

        var fk = ((IProperty)property).SqlName;
        var apk = property.Property.SqlName;
        var joinColumn = new JavaAnnotation("JoinColumn", imports: $"{JavaxOrJakarta}.persistence.JoinColumn")
            .AddAttribute("name", $@"""{fk}""")
            .AddAttribute("referencedColumnName", $@"""{apk}""");
        yield return joinColumn;
    }

    protected virtual IEnumerable<JavaAnnotation> GetOneToManyAnnotations(AssociationProperty property)
    {
        var association = new JavaAnnotation(
            @$"{property.Type}",
            imports: $"{JavaxOrJakarta}.persistence.{property.Type}"
        );
        association
            .AddAttribute("cascade", "CascadeType.ALL", $"{JavaxOrJakarta}.persistence.CascadeType")
            .AddAttribute("fetch", "FetchType.LAZY", $"{JavaxOrJakarta}.persistence.FetchType");

        if (property is ReverseAssociationProperty rap)
        {
            association.AddAttribute("mappedBy", $@"""{rap.ReverseProperty.NameByClassCamel}""");
        }
        else
        {
            var pk = property.Class.PrimaryKey.Single().SqlName;

            if (property.ReverseProperty != null)
            {
                association.AddAttribute("mappedBy", @$"""{property.Class.NameCamel}{property.Role ?? string.Empty}""");
            }
            else
            {
                var joinColumn = new JavaAnnotation("JoinColumn", imports: $"{JavaxOrJakarta}.persistence.JoinColumn")
                    .AddAttribute("name", $@"""{pk}""")
                    .AddAttribute("referencedColumnName", $@"""{pk}""");
                yield return joinColumn;
            }
        }

        yield return association;
    }

    protected virtual IEnumerable<JavaAnnotation> GetOneToOneAnnotations(AssociationProperty property)
    {
        var fk = ((IProperty)property).SqlName;
        var apk = property.Property.SqlName;
        var association = new JavaAnnotation(
            @$"{property.Type}",
            imports: $"{JavaxOrJakarta}.persistence.{property.Type}"
        )
            .AddAttribute("fetch", "FetchType.LAZY", $"{JavaxOrJakarta}.persistence.FetchType")
            .AddAttribute("cascade", @"CascadeType.ALL", $"{JavaxOrJakarta}.persistence.CascadeType")
            .AddAttribute("optional", (!property.Required).ToString().ToLower());
        yield return association;

        var joinColumn = new JavaAnnotation("JoinColumn", imports: $"{JavaxOrJakarta}.persistence.JoinColumn")
            .AddAttribute("name", $@"""{fk}""")
            .AddAttribute("referencedColumnName", $@"""{apk}""")
            .AddAttribute("unique", "true");
        yield return joinColumn;
    }
}
