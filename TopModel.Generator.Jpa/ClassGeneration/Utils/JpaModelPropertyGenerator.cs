using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration.Utils;

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

        if (!property.Class.IsPersistent && !property.AssociationMultiple && property.Domain != null)
        {
            var propertyType = Config.GetType(property);
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
        var javaField = new JavaField(
            Config.GetType(property, forceAssociationPropertyType: Config.UseJdbc),
            property.NameCamel
        )
        {
            Comment = { property.Comment },
        };

        if (
            property is { OriginalProperty: IProperty op }
            && Config.AvailableClasses.Contains(op.Class)
            && (op.Class.Enum != EnumMode.Enum || op != op.Class.EnumKey)
        )
        {
            var getter = $"#{GetGetterName(op)}()";
            javaField.Comment.Add(
                $"Alias of {{@link {op.Class.GetImport(Config, tag)}{getter} {op.Class.NamePascal}{getter}}}"
            );
        }
        var annotations = GetAnnotations(property, tag);

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
        var availableProperties = Config.GetAvailableProperties(classe);
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
        var propertyName = property.NameCamel;
        var propertyType = Config.GetType(property);
        var getterPrefix = propertyType == "boolean" ? "is" : "get";
        if (property.Class.PreservePropertyCasing)
        {
            return propertyName.ToFirstUpper().WithPrefix(getterPrefix);
        }

        return propertyName.ToPascalCase().WithPrefix(getterPrefix);
    }

    public IEnumerable<JavaAnnotation> GetJpaAssociationAnnotations(IProperty property, string tag)
    {
        if (property.AssociationMultiple)
        {
            return GetOneToManyAnnotations(property);
        }
        else if (property.Unique)
        {
            return GetOneToOneAnnotations(property);
        }

        return GetManyToOneAnnotations(property, tag);
    }

    public virtual JavaMethod? GetMapIdPropertyGetter(Class classe, string tag)
    {
        if (
            classe.PrimaryKey.Count() == 1
            && classe.PrimaryKey.FirstOrDefault()
                is { AssociationProperty: IProperty ap, UseClassForAssociation: true } pk
        )
        {
            var propertyType = Config.GetType(ap);
            string getterName = $"get{pk.PropertyNamePascal}";
            var method = new JavaMethod(propertyType, getterName)
            {
                Visibility = "public",
                Comment = $"Getter for {pk.PropertyNameCamel}",
                ReturnComment =
                    $"value of {{@link {classe.GetImport(Config, tag)}#{pk.PropertyNameCamel} {pk.PropertyNameCamel}}}",
            };
            method.AddBodyLine(@$"return this.{pk.PropertyNameCamel};");
            return method;
        }
        return null;
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
        var propertyName = property.NameCamel;
        if (property.Class.PreservePropertyCasing)
        {
            return propertyName.WithPrefix("set");
        }

        return propertyName.ToPascalCase().WithPrefix("set");
    }

    public virtual bool ShouldWriteEnumAnnotation(IProperty property)
    {
        return property is { EnumProperty: IProperty ep }
            && property.Class.IsPersistent
            && Config.AvailableClasses.Contains(ep.Class);
    }

    protected virtual IEnumerable<JavaAnnotation> GetAnnotations(IProperty property, string tag)
    {
        var shouldWriteAssociation =
            !Config.UseJdbc
            && property.Class.IsPersistent
            && (property.Association?.IsPersistent ?? false)
            && property.EnumProperty?.Class.Enum != EnumMode.Enum
            && Config.AvailableClasses.Contains(property.Association)
            && property.UseClassForAssociation;

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
                property.AssociationMultiple
                && property.Association?.OrderProperty != null
                && Config.GetType(property).Contains("List")
            )
            {
                yield return new JavaAnnotation(
                    "OrderBy",
                    $@"""{property.Association!.OrderProperty!.NameCamel} ASC""",
                    "jakarta.persistence.OrderBy"
                );
            }
        }
        else
        {
            if (
                (property.Class.IsPersistent || Config.UseJdbc && property.Composition == null)
                && !(property.PrimaryKey && property.Class.PrimaryKey.Count() > 1)
                && !property.AssociationMultiple
            )
            {
                yield return GetColumnAnnotation(property);
            }

            if (ShouldWriteEnumAnnotation(property))
            {
                yield return EnumAnnotation;
            }

            if (
                !Config.UseJdbc
                && property.Class.IsPersistent
                && property.AssociationMultiple
                && !property.UseClassForAssociation
            )
            {
                // Association réciproque multiple non matérialisée (ex. useClass: false) : il n'existe aucune
                // colonne correspondante en base, on l'exclut donc du mapping JPA pour éviter une colonne fantôme.
                yield return new JavaAnnotation("Transient", imports: "jakarta.persistence.Transient");
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
        return defaultValue != "null" ? defaultValue : string.Empty;
    }

    protected virtual IEnumerable<string> GetDefaultValueImports(IProperty property, string tag)
    {
        var defaultValue = Config.GetValue(property);

        if (
            defaultValue != "null"
            && property is { EnumProperty: IProperty ep }
            && Config.UniqueValueGeneration != UniqueValueGenerationMode.None
        )
        {
            return
            [
                $"{Config.GetEnumPackageName(ep.Class, Config.GetBestClassTag(property.Class, tag))}.{Config.GetEnumType(ep)}",
            ];
        }
        else if (
            defaultValue != "null"
            && property is { UniqueValuedProperty: IProperty uvp }
            && Config.UniqueValueGeneration.CanConst
        )
        {
            return
            [
                $"{Config.GetEnumPackageName(uvp.Class, Config.GetBestClassTag(property.Class, tag))}.{Config.GetEnumType(uvp)}",
            ];
        }
        else
        {
            return Config.GetValueImports(property, tag);
        }
    }

    protected virtual IEnumerable<JavaAnnotation> GetIdAnnotations(IProperty property)
    {
        if (property.Association != null && property.Class?.PrimaryKey.Count() == 1)
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

    protected virtual IEnumerable<JavaAnnotation> GetManyToOneAnnotations(IProperty property, string tag)
    {
        var association = new JavaAnnotation("ManyToOne", imports: "jakarta.persistence.ManyToOne")
            .AddAttribute("fetch", "FetchType.LAZY", "jakarta.persistence.FetchType")
            .AddAttribute("optional", property.Required ? "false" : "true")
            .AddAttribute(
                "targetEntity",
                $"{property.Association!.NamePascal}.class",
                property.Association!.GetImport(Config, Config.GetBestClassTag(property.Association!, tag))
            );
        Config.CascadeTypes.TryGetValue(AssociationType.ManyToOne, out var cascadeTypes);
        if (cascadeTypes != null && cascadeTypes.Any())
        {
            association.AddAttribute(
                "cascade",
                $@"{(cascadeTypes.Count() > 1 ? "{ " : string.Empty)}{string.Join(", ", cascadeTypes?.Select(ct => $"CascadeType.{ct.ToString().ToUpper()}") ?? [])}{(cascadeTypes!.Count() > 1 ? " }" : string.Empty)}",
                "jakarta.persistence.CascadeType"
            );
        }
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
        var association = new JavaAnnotation("OneToMany", imports: "jakarta.persistence.OneToMany");
        Config.CascadeTypes.TryGetValue(AssociationType.OneToMany, out var cascadeTypes);
        if (cascadeTypes != null && cascadeTypes.Any())
        {
            association.AddAttribute(
                "cascade",
                $@"{(cascadeTypes.Count() > 1 ? "{ " : string.Empty)}{string.Join(", ", cascadeTypes?.Select(ct => $"CascadeType.{ct.ToString().ToUpper()}") ?? [])}{(cascadeTypes!.Count() > 1 ? " }" : string.Empty)}",
                "jakarta.persistence.CascadeType"
            );
        }
        association.AddAttribute("fetch", "FetchType.LAZY", "jakarta.persistence.FetchType");

        if (property.ReverseProperty != null)
        {
            association.AddAttribute("mappedBy", @$"""{property.ReverseProperty!.NameCamel}""");
        }
        else
        {
            var pk = property.Class.PrimaryKey.Single().SqlName;
            var joinColumn = new JavaAnnotation("JoinColumn", imports: "jakarta.persistence.JoinColumn")
                .AddAttribute("name", $@"""{pk}""")
                .AddAttribute("referencedColumnName", $@"""{pk}""");
            yield return joinColumn;
        }

        yield return association;
    }

    protected virtual IEnumerable<JavaAnnotation> GetOneToOneAnnotations(IProperty property)
    {
        var fk = property.SqlName;
        var apk = property.AssociationProperty!.SqlName;
        var association = new JavaAnnotation("OneToOne", imports: $"jakarta.persistence.OneToOne")
            .AddAttribute("fetch", "FetchType.LAZY", "jakarta.persistence.FetchType")
            .AddAttribute("optional", (!property.Required).ToString().ToLower());

        Config.CascadeTypes.TryGetValue(AssociationType.OneToOne, out var cascadeTypes);
        if (cascadeTypes != null && cascadeTypes.Any())
        {
            association.AddAttribute(
                "cascade",
                $@"{(cascadeTypes.Count() > 1 ? "{ " : string.Empty)}{string.Join(", ", cascadeTypes?.Select(ct => $"CascadeType.{ct.ToString().ToUpper()}") ?? [])}{(cascadeTypes!.Count() > 1 ? " }" : string.Empty)}",
                "jakarta.persistence.CascadeType"
            );
        }
        if (property is { ReverseProperty: not null } && property is { IsReverseProperty: true })
        {
            association.AddAttribute("mappedBy", $@"""{property.ReverseProperty!.NameCamel}""");
        }
        yield return association;
        if (property is { IsReverseProperty: false })
        {
            var joinColumn = new JavaAnnotation("JoinColumn", imports: "jakarta.persistence.JoinColumn")
                .AddAttribute("name", $@"""{fk}""")
                .AddAttribute("referencedColumnName", $@"""{apk}""")
                .AddAttribute("unique", "true");
            yield return joinColumn;
        }
    }
}
