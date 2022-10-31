using TopModel.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JpaModelPropertyGenerator
{
    private readonly JpaConfig _config;

    public JpaModelPropertyGenerator(JpaConfig config)
    {
        _config = config;
    }

    public void WriteProperty(JavaWriter fw, Class classe, IProperty property)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, property.Comment);
        switch (property)
        {
            case CompositionProperty cp:
                WriteProperty(fw, classe, cp);
                break;
            case AssociationProperty { Association.IsPersistent: true } ap:
                WriteProperty(fw, classe, ap);
                break;
            case IFieldProperty fp:
                WriteProperty(fw, classe, fp);
                break;
        }
    }

    public void WriteProperty(JavaWriter fw, Class classe, CompositionProperty property)
    {
        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"private {property.GetJavaType()} {property.Name.ToFirstLower()};");
    }

    public void WriteProperties(JavaWriter fw, Class classe, List<Class> availableClasses)
    {
        foreach (var property in classe.GetProperties(_config, availableClasses))
        {
            WriteProperty(fw, classe, property);
        }
    }

    private void WriteProperty(JavaWriter fw, Class classe, AssociationProperty property)
    {
        fw.WriteDocEnd(1);
        switch (property.Type)
        {
            case AssociationType.ManyToOne:
                WriteManyToOne(fw, classe, property);
                break;
            case AssociationType.OneToMany:
                WriteOneToMany(fw, classe, property);
                break;
            case AssociationType.ManyToMany:
                WriteManyToMany(fw, classe, property);
                break;
            case AssociationType.OneToOne:
                WriteOneToOne(fw, classe, property);
                break;
        }

        fw.WriteLine(1, $"private {property.GetJavaType()} {property.GetAssociationName()};");
    }

    private void WriteManyToOne(JavaWriter fw, Class classe, AssociationProperty property)
    {
        var fk = ((IFieldProperty)property).SqlName;
        var apk = property.Association.PrimaryKey!.SqlName;
        fw.WriteLine(1, @$"@{property.Type}(fetch = FetchType.LAZY, optional = {(property.Required ? "false" : "true")}, targetEntity = {property.Association.Name}.class)");
        fw.WriteLine(1, @$"@JoinColumn(name = ""{fk}"", referencedColumnName = ""{apk}"")");
    }

    private void WriteOneToOne(JavaWriter fw, Class classe, AssociationProperty property)
    {
        var fk = ((IFieldProperty)property).SqlName;
        var apk = property.Association.PrimaryKey!.SqlName;
        fw.WriteLine(1, @$"@{property.Type}(fetch = FetchType.LAZY, cascade = CascadeType.ALL, orphanRemoval = true, optional = {(property.Required ? "false" : "true")})");
        fw.WriteLine(1, @$"@JoinColumn(name = ""{fk}"", referencedColumnName = ""{apk}"", unique = true)");
    }

    private void WriteManyToMany(JavaWriter fw, Class classe, AssociationProperty property)
    {
        var role = property.Role is not null ? "_" + property.Role.ToConstantCase() : string.Empty;
        var fk = ((IFieldProperty)property).SqlName;
        var pk = classe.PrimaryKey!.SqlName + role;
        var cascade = property.Association.IsStatic() ? string.Empty : $", cascade = {{ CascadeType.PERSIST, CascadeType.MERGE }}";
        if (property is JpaAssociationProperty jap)
        {
            fw.WriteLine(1, @$"@{property.Type}(fetch = FetchType.LAZY, mappedBy = ""{jap.ReverseProperty.GetJavaName()}""{cascade})");
        }
        else
        {
            fw.WriteLine(1, @$"@{property.Type}(fetch = FetchType.LAZY{cascade})");
            fw.WriteLine(1, @$"@JoinTable(name = ""{property.Class.SqlName}_{property.Association.SqlName}{(property.Role != null ? "_" + property.Role.ToConstantCase() : string.Empty)}"", joinColumns = @JoinColumn(name = ""{pk}""), inverseJoinColumns = @JoinColumn(name = ""{fk}""))");
        }
    }

    private void WriteOneToMany(JavaWriter fw, Class classe, AssociationProperty property)
    {
        var pk = classe.PrimaryKey!.SqlName;
        if (property is JpaAssociationProperty jap)
        {
            fw.WriteLine(1, @$"@{property.Type}(cascade = {{CascadeType.PERSIST, CascadeType.MERGE}}, orphanRemoval = {property.Required.ToString().ToLower()}, fetch = FetchType.LAZY, mappedBy = ""{jap.ReverseProperty.GetJavaName()}"")");
        }
        else
        {
            var hasRerverse = property.Class.Namespace.Module.Split('.').First() == property.Association.Namespace.Module.Split('.').First();
            fw.WriteLine(1, @$"@{property.Type}(cascade = CascadeType.ALL, orphanRemoval = {property.Required.ToString().ToLower()}, fetch = FetchType.LAZY{(hasRerverse ? @$", mappedBy = ""{property.Class.Name.ToFirstLower()}{property.Role ?? string.Empty}""" : string.Empty)})");
            if (!hasRerverse)
            {
                fw.WriteLine(1, @$"@JoinColumn(name = ""{pk}"", referencedColumnName = ""{pk}"")");
            }
        }
    }

    private void WriteProperty(JavaWriter fw, Class classe, IFieldProperty property)
    {
        if (property is AliasProperty alp)
        {
            fw.WriteLine(1, $" * Alias of {{@link {alp.Property.Class.GetImport(_config)}#get{alp.Property.Name.ToFirstUpper()}() {alp.Property.Class.Name}#get{alp.Property.Name.ToFirstUpper()}()}} ");
        }

        fw.WriteDocEnd(1);
        if (property.PrimaryKey && classe.IsPersistent)
        {
            fw.WriteLine(1, "@Id");
            if (
                property.Domain.Java!.Type == "Long"
                || property.Domain.Java.Type == "long"
                || property.Domain.Java.Type == "int"
                || property.Domain.Java.Type == "Integer")
            {
                if (_config.Identity.Mode == IdentityMode.IDENTITY)
                {
                    fw.WriteLine(1, @$"@GeneratedValue(strategy = GenerationType.IDENTITY)");
                }
                else if (_config.Identity.Mode == IdentityMode.SEQUENCE)
                {
                    var seqName = $"SEQ_{classe.SqlName}";
                    var initialValue = _config.Identity.Start != null ? $", initialValue = {_config.Identity.Start}" : string.Empty;
                    var increment = _config.Identity.Increment != null ? $", allocationSize = {_config.Identity.Increment}" : string.Empty;
                    fw.WriteLine(1, @$"@SequenceGenerator(name = ""{seqName}"", sequenceName = ""{seqName}""{initialValue}{increment})");
                    fw.WriteLine(1, @$"@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = ""{seqName}"")");
                }
            }
        }

        if (classe.IsPersistent && !property.Domain.Java!.Annotations
        .Where(i =>
                classe.IsPersistent && (Target.Persisted & i.Target) > 0
            || !classe.IsPersistent && (Target.Dto & i.Target) > 0)
            .Any(a => a.Text.Replace("@", string.Empty).StartsWith("Column")))
        {
            var column = @$"@Column(name = ""{property.SqlName}"", nullable = {(!property.Required).ToString().ToFirstLower()}";
            if (property.Domain.Length != null)
            {
                if (property.Domain.Java!.Type.ToUpper() == "STRING")
                {
                    column += $", length = {property.Domain.Length}";
                }
                else
                {
                    column += $", precision = {property.Domain.Length}";
                }
            }

            if (property.Domain.Scale != null)
            {
                column += $", scale = {property.Domain.Scale}";
            }

            column += ")";
            fw.WriteLine(1, column);
        }
        else if (property.Required && !property.PrimaryKey)
        {
            fw.WriteLine(1, @$"@NotNull");
        }

        if (property.PrimaryKey && classe.Reference && classe.ReferenceValues.Any())
        {
            var javaOrJakarta = _config.PersistenceMode.ToString().ToLower();
            fw.AddImports(new List<string>
            {
                $"{javaOrJakarta}.persistence.Enumerated",
                $"{javaOrJakarta}.persistence.EnumType",
            });
            fw.WriteLine(1, "@Enumerated(EnumType.STRING)");
        }

        if (property.Domain.Java is not null && property.Domain.Java.Annotations is not null)
        {
            foreach (var annotation in property.Domain.Java.Annotations.Where(a =>
                classe.IsPersistent && (Target.Persisted & a.Target) > 0
            || !classe.IsPersistent && (Target.Dto & a.Target) > 0))
            {
                fw.AddImports(annotation.Imports.Select(i => i.ParseTemplate(property)));
                fw.WriteLine(1, $"{(annotation.Text.StartsWith("@") ? string.Empty : '@')}{annotation.Text.ParseTemplate(property)}");
            }
        }

        fw.WriteLine(1, $"private {property.GetJavaType()} {property.GetJavaName()};");
    }
}