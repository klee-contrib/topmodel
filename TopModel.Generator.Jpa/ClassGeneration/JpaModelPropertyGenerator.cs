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
    public static JavaAnnotation EnumAnnotation =>
        new JavaAnnotation("Enumerated", imports: "jakarta.persistence.Enumerated").AddAttribute(
            "value",
            "EnumType.STRING",
            "jakarta.persistence.EnumType"
        );

    public virtual JavaAnnotation IdAnnotation => new("Id", imports: "jakarta.persistence.Id");

    public virtual JavaAnnotation MapsIdAnnotation => new("MapsId", imports: "jakarta.persistence.MapsId");

    protected JpaConfig Config { get; } = config;

    protected IDictionary<string, string> NewableTypes { get; } = newableTypes;

    protected virtual JavaAnnotation NotNullAnnotation =>
        new("NotNull", imports: "jakarta.validation.constraints.NotNull");

    protected virtual JavaAnnotation ValidAnnotation => new("Valid", imports: "jakarta.validation.Valid");

    public virtual IEnumerable<IProperty> GetAvailableProperties(Class classe)
    {
        return classe.Properties.Where(p =>
            !(p is { Composition: Class cpc } && !Config.AvailableClasses.Contains(cpc))
            && !(
                p is AliasProperty ap
                && ap.Property is { Composition: Class cpc2 }
                && !Config.AvailableClasses.Contains(cpc2)
            )
        );
    }

    public virtual JavaAnnotation GetColumnAnnotation(IProperty property)
    {
        JavaAnnotation column = new JavaAnnotation("Column", imports: "jakarta.persistence.Column").AddAttribute(
            "name",
            $@"""{property.SqlName}"""
        );
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

        if (property is { Composition: not null, Domain: null })
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
                    imports: "jakarta.validation.constraints.Size"
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
                    imports: "jakarta.validation.constraints.Digits"
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
        var method = new JavaMethod(field.Type, GetGetterName(property))
        {
            Comment = $"Getter for {field.Name}",
            Body =
            {
                new WriterLine() { Line = $"return this.{field.Name};", Indent = 0 },
            },
            ReturnComment = $"value of {{@link #{field.Name} {field.Name}}}",
            Visibility = "public",
        };
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

    public IEnumerable<JavaAnnotation> GetJpaAssociationAnnotations(IProperty property, string tag)
    {
        return property.AssociationType switch
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
        var field = GetField(property, tag);
        return new("void", GetSetterName(property))
        {
            Comment = $@"Set the value of {{@link #{field.Name} {field.Name}}}",
            Parameters = { new JavaMethodParameter(field.Type, field.Name) { Comment = $"value to set" } },
            Body =
            {
                new WriterLine() { Line = $"this.{field.Name} = {field.Name};", Indent = 0 },
            },
            Visibility = "public",
        };
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
        return property.EnumProperty != null
            && Config.CanClassUseEnums(property.EnumProperty!.Class, property.EnumProperty)
            && property.Class.IsPersistent;
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

    protected virtual IEnumerable<JavaAnnotation> GetAnnotations(IProperty property, string tag)
    {
        var shouldWriteAssociation =
            !Config.UseJdbc
            && property.Class.IsPersistent
            && (property.Association?.IsPersistent ?? false)
            && !(
                Config.EnumsAsEnums
                && property.EnumProperty != null
                && Config.CanClassUseEnums(property.EnumProperty!.Class, property.EnumProperty)
            )
            && Config.AvailableClasses.Contains(property.Association);

        if (shouldWriteAssociation && property.Association != null)
        {
            if (!property.PrimaryKey || property.Class.PrimaryKey.Count() <= 1)
            {
                foreach (var a in GetJpaAssociationAnnotations(property, tag))
                {
                    yield return a;
                }
            }

            if (
                (
                    property.AssociationType == AssociationType.ManyToMany
                    || property.AssociationType == AssociationType.OneToMany
                )
                && property.Association?.OrderProperty != null
                && GetPropertyType(property).Contains("List")
            )
            {
                yield return new JavaAnnotation(
                    "OrderBy",
                    $@"""{property.Association!.OrderProperty!.NameByClassCamel} ASC""",
                    "jakarta.persistence.OrderBy"
                );
            }
        }
        else
        {
            if (
                (property.Class.IsPersistent || Config.UseJdbc && property.Composition == null)
                && !(property.PrimaryKey && property.Class.PrimaryKey.Count() > 1)
            )
            {
                yield return GetColumnAnnotation(property);
            }

            if (ShouldWriteEnumAnnotation(property))
            {
                yield return EnumAnnotation;
            }
        }

        if (property.PrimaryKey && property.Class.IsPersistent)
        {
            foreach (var a in GetIdAnnotations(property))
            {
                yield return a;
            }
        }

        if ((!property.Class.IsPersistent || Config.UseJdbc) && property.Required && !property.PrimaryKey)
        {
            yield return NotNullAnnotation;
        }

        if (property is { Composition: Class cpc })
        {
            if (property.Class.IsPersistent)
            {
                yield return GetConvertAnnotation(cpc, tag);
            }
            else
            {
                yield return ValidAnnotation;
            }
        }
    }

    protected virtual IEnumerable<JavaAnnotation> GetAutogeneratedAnnotations(Class classe)
    {
        var autoGenerated = new JavaAnnotation("GeneratedValue", imports: "jakarta.persistence.GeneratedValue");
        if (Config.Identity.Mode == IdentityMode.UUID)
        {
            autoGenerated.AddAttribute("strategy", "GenerationType.UUID", "jakarta.persistence.GenerationType");
        }
        else if (Config.Identity.Mode == IdentityMode.IDENTITY)
        {
            autoGenerated.AddAttribute("strategy", "GenerationType.IDENTITY", "jakarta.persistence.GenerationType");
        }
        else if (Config.Identity.Mode == IdentityMode.SEQUENCE)
        {
            var seqName = $"SEQ_{classe.SqlName}";
            autoGenerated
                .AddAttribute("strategy", "GenerationType.SEQUENCE", "jakarta.persistence.GenerationType")
                .AddAttribute("generator", $@"""{seqName}""");
            var sequenceGenerator = new JavaAnnotation(
                "SequenceGenerator",
                imports: "jakarta.persistence.SequenceGenerator"
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

    protected virtual JavaAnnotation GetConvertAnnotation(Class cpc, string tag)
    {
        var convert = new JavaAnnotation("Convert", imports: "jakarta.persistence.Convert");
        var import = Config
            .CompositionConverterCanonicalName.Replace("{class}", cpc.Name)
            .Replace("{package}", Config.GetPackageName(cpc, Config.GetBestClassTag(cpc, tag)));
        convert.AddAttribute(
            "converter",
            $"{Config.CompositionConverterSimpleName.Replace("{class}", cpc.Name)}.class",
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
                else if (property.Class.IsPersistent)
                {
                    return $"new {ap.Association.NamePascal}({defaultValue})";
                }
                else
                {
                    return defaultValue;
                }
            }

            return string.Empty;
        }
        else if (property is AliasProperty { Property: AssociationProperty ap2 })
        {
            if (
                ap2.Association.PrimaryKey.Count() == 1
                && Config.CanClassUseEnums(ap2.Association, prop: ap2.Association.PrimaryKey.Single())
                && defaultValue != "null"
            )
            {
                if (Config.EnumsAsEnums)
                {
                    return $"{defaultValue}";
                }
                else if (property.Class.IsPersistent)
                {
                    return $"new {ap2.Association.NamePascal}({defaultValue})";
                }
                else
                {
                    return defaultValue;
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
                    $"{Config.GetEnumPackageName(ap.Association, Config.GetBestClassTag(property.Class, tag))}.{GetPropertyType(ap.Association.PrimaryKey.Single())}",
                ];
            }

            return [];
        }
        else if (property is AliasProperty asp && asp.Property is AssociationProperty ap2)
        {
            if (
                ap2.Association.PrimaryKey.Count() == 1
                && Config.CanClassUseEnums(ap2.Association, prop: ap2.Association.PrimaryKey.Single())
                && defaultValue != "null"
            )
            {
                return
                [
                    $"{Config.GetEnumPackageName(ap2.Association, Config.GetBestClassTag(property.Class, tag))}.{GetPropertyType(ap2.Association.PrimaryKey.Single())}",
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
            if (property.AutoGeneratedValue && property.Class?.PrimaryKey.Count() == 1)
            {
                foreach (var a in GetAutogeneratedAnnotations(property.Class))
                {
                    yield return a;
                }
            }
        }
    }

    protected virtual IEnumerable<JavaAnnotation> GetManyToManyAnnotations(IProperty property)
    {
        var role = property.AssociationRole != null ? "_" + property.AssociationRole!.ToConstantCase() : string.Empty;
        var fk = property.SqlName;
        var pk = property.Class.PrimaryKey.Single().SqlName + role;
        var association = new JavaAnnotation(
            $"{property.AssociationType}",
            imports: $"jakarta.persistence.{property.AssociationType}"
        );
        if (
            property.AssociationType == AssociationType.ManyToOne
            || property.AssociationType == AssociationType.OneToOne
        )
        {
            association.AddAttribute("fetch", "FetchType.LAZY", "jakarta.persistence.FetchType");
        }

        if (!Config.CanClassUseEnums(property.Association!))
        {
            association.AddAttribute(
                "cascade",
                "{ CascadeType.PERSIST, CascadeType.MERGE }",
                "jakarta.persistence.CascadeType"
            );
        }

        if (property is ReverseAssociationProperty rap)
        {
            association.AddAttribute("mappedBy", $@"""{rap.ReverseProperty.NameByClassCamel}""");
        }

        yield return association;

        if (property is not ReverseAssociationProperty)
        {
            var joinColumns = new JavaAnnotation("JoinColumn", imports: "jakarta.persistence.JoinColumn").AddAttribute(
                "name",
                $@"""{pk}"""
            );
            var inverseJoinColumns = new JavaAnnotation(
                "JoinColumn",
                imports: "jakarta.persistence.JoinColumn"
            ).AddAttribute("name", $@"""{fk}""");
            var joinTable = new JavaAnnotation("JoinTable", imports: "jakarta.persistence.JoinTable")
                .AddAttribute(
                    "name",
                    $@"""{property.Class.SqlName}_{property.Association!.SqlName}{(property.AssociationRole != null ? "_" + property.AssociationRole!.ToConstantCase() : string.Empty)}"""
                )
                .AddAttribute("joinColumns", joinColumns)
                .AddAttribute("inverseJoinColumns", inverseJoinColumns);
            yield return joinTable;
        }
    }

    protected virtual IEnumerable<JavaAnnotation> GetManyToOneAnnotations(IProperty property, string tag)
    {
        var association = new JavaAnnotation(
            @$"{property.AssociationType}",
            imports: $"jakarta.persistence.{property.AssociationType}"
        )
            .AddAttribute("fetch", "FetchType.LAZY", "jakarta.persistence.FetchType")
            .AddAttribute("optional", property.Required ? "false" : "true")
            .AddAttribute(
                "targetEntity",
                $"{property.Association!.NamePascal}.class",
                property.Association!.GetImport(Config, Config.GetBestClassTag(property.Association!, tag))
            );
        yield return association;

        var fk = property.SqlName;
        var apk = property.AssociationProperty!.SqlName;
        var joinColumn = new JavaAnnotation("JoinColumn", imports: "jakarta.persistence.JoinColumn")
            .AddAttribute("name", $@"""{fk}""")
            .AddAttribute("referencedColumnName", $@"""{apk}""");
        yield return joinColumn;
    }

    protected virtual IEnumerable<JavaAnnotation> GetOneToManyAnnotations(IProperty property)
    {
        var association = new JavaAnnotation(
            @$"{property.AssociationType}",
            imports: $"jakarta.persistence.{property.AssociationType}"
        );
        association
            .AddAttribute("cascade", "CascadeType.ALL", "jakarta.persistence.CascadeType")
            .AddAttribute("fetch", "FetchType.LAZY", "jakarta.persistence.FetchType");

        if (property is ReverseAssociationProperty rap)
        {
            association.AddAttribute("mappedBy", $@"""{rap.ReverseProperty.NameByClassCamel}""");
        }
        else
        {
            var pk = property.Class.PrimaryKey.Single().SqlName;

            if (property is AssociationProperty { ReverseProperty: not null })
            {
                association.AddAttribute(
                    "mappedBy",
                    @$"""{property.Class.NameCamel}{property.AssociationRole ?? string.Empty}"""
                );
            }
            else
            {
                var joinColumn = new JavaAnnotation("JoinColumn", imports: "jakarta.persistence.JoinColumn")
                    .AddAttribute("name", $@"""{pk}""")
                    .AddAttribute("referencedColumnName", $@"""{pk}""");
                yield return joinColumn;
            }
        }

        yield return association;
    }

    protected virtual IEnumerable<JavaAnnotation> GetOneToOneAnnotations(IProperty property)
    {
        var fk = property.SqlName;
        var apk = property.AssociationProperty!.SqlName;
        var association = new JavaAnnotation(
            @$"{property.AssociationType}",
            imports: $"jakarta.persistence.{property.AssociationType}"
        )
            .AddAttribute("fetch", "FetchType.LAZY", "jakarta.persistence.FetchType")
            .AddAttribute("cascade", @"CascadeType.ALL", "jakarta.persistence.CascadeType")
            .AddAttribute("optional", (!property.Required).ToString().ToLower());
        yield return association;

        var joinColumn = new JavaAnnotation("JoinColumn", imports: "jakarta.persistence.JoinColumn")
            .AddAttribute("name", $@"""{fk}""")
            .AddAttribute("referencedColumnName", $@"""{apk}""")
            .AddAttribute("unique", "true");
        yield return joinColumn;
    }
}
