using TopModel.Core;
using TopModel.Core.Model.Implementation;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JpaModelPropertyGenerator
{
    private readonly IEnumerable<Class> _classes;
    private readonly JpaConfig _config;

    public JpaModelPropertyGenerator(JpaConfig config, IEnumerable<Class> classes)
    {
        _classes = classes;
        _config = config;
    }

    public void WriteCompositePrimaryKeyClass(JavaWriter fw, Class classe)
    {
        if (classe.PrimaryKey.Count() <= 1)
        {
            return;
        }

        fw.WriteLine();
        fw.AddImport("java.io.Serializable");
        fw.WriteLine(1, @$"public class {classe.NamePascal}Id implements Serializable {{");
        foreach (var pk in classe.PrimaryKey)
        {
            fw.WriteLine(2, $"private {_config.GetType(pk, _classes, true)} {pk.NameByClassCamel};");
            fw.WriteLine();
        }

        fw.WriteLine(2, "public boolean equals(Object o) {");
        fw.WriteLine(3, $@"if(!(o instanceof {classe.NamePascal}Id)) {{");
        fw.WriteLine(4, "return false;");
        fw.WriteLine(3, "}");
        fw.WriteLine();
        fw.WriteLine(3, $"{classe.NamePascal}Id oId = ({classe.NamePascal}Id) o;");
        fw.WriteLine(3, $@"return !({string.Join("\n || ", classe.PrimaryKey.Select(pk => $@"!this.{pk.NameByClassCamel}.equals(oId.{pk.NameByClassCamel})"))});");
        fw.WriteLine(2, "}");

        fw.WriteLine();
        fw.WriteLine(2, "@Override");
        fw.WriteLine(2, "public int hashCode() {");
        fw.WriteLine(3, $"return Objects.hash({string.Join(", ", classe.PrimaryKey.Select(pk => pk.NameByClassCamel))});");
        fw.AddImport("java.util.Objects");
        fw.WriteLine(2, "}");
        fw.WriteLine(1, "}");
    }

    public void WriteProperties(JavaWriter fw, Class classe, string tag)
    {
        foreach (var property in classe.GetProperties(_classes))
        {
            WriteProperty(fw, classe, property, tag);
        }
    }

    public void WriteProperty(JavaWriter fw, CompositionProperty property)
    {
        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"private {_config.GetType(property)} {property.NameCamel};");
    }

    public void WriteProperty(JavaWriter fw, Class classe, IProperty property, string tag)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, property.Comment);
        switch (property)
        {
            case CompositionProperty cp:
                WriteProperty(fw, cp);
                break;
            case AssociationProperty { Association.IsPersistent: true } ap:
                WriteProperty(fw, classe, ap);
                break;
            case IFieldProperty fp:
                WriteProperty(fw, classe, fp, tag);
                break;
        }
    }

    private void WriteManyToMany(JavaWriter fw, Class classe, AssociationProperty property)
    {
        var role = property.Role is not null ? "_" + property.Role.ToConstantCase() : string.Empty;
        var fk = ((IFieldProperty)property).SqlName;
        var pk = classe.PrimaryKey.Single().SqlName + role;
        var javaOrJakarta = _config.PersistenceMode.ToString().ToLower();
        if (!_config.CanClassUseEnums(property.Association))
        {
            fw.AddImport($"{javaOrJakarta}.persistence.CascadeType");
        }

        var cascade = _config.CanClassUseEnums(property.Association) ? string.Empty : $", cascade = {{ CascadeType.PERSIST, CascadeType.MERGE }}";
        if (property is ReverseAssociationProperty rap)
        {
            fw.WriteLine(1, @$"@{property.Type}(fetch = FetchType.LAZY, mappedBy = ""{rap.ReverseProperty.NameByClassCamel}""{cascade})");
        }
        else
        {
            fw.AddImport($"{javaOrJakarta}.persistence.JoinTable");
            fw.WriteLine(1, @$"@{property.Type}(fetch = FetchType.LAZY{cascade})");
            fw.WriteLine(1, @$"@JoinTable(name = ""{property.Class.SqlName}_{property.Association.SqlName}{(property.Role != null ? "_" + property.Role.ToConstantCase() : string.Empty)}"", joinColumns = @JoinColumn(name = ""{pk}""), inverseJoinColumns = @JoinColumn(name = ""{fk}""))");
            fw.AddImport($"{javaOrJakarta}.persistence.JoinColumn");
        }
    }

    private void WriteManyToOne(JavaWriter fw, AssociationProperty property)
    {
        var fk = ((IFieldProperty)property).SqlName;
        var apk = property.Property.SqlName;
        var javaOrJakarta = _config.PersistenceMode.ToString().ToLower();
        fw.WriteLine(1, @$"@{property.Type}(fetch = FetchType.LAZY, optional = {(property.Required ? "false" : "true")}, targetEntity = {property.Association.NamePascal}.class)");
        fw.WriteLine(1, @$"@JoinColumn(name = ""{fk}"", referencedColumnName = ""{apk}"")");
        fw.AddImport($"{javaOrJakarta}.persistence.JoinColumn");
    }

    private void WriteOneToMany(JavaWriter fw, Class classe, AssociationProperty property)
    {
        var pk = classe.PrimaryKey.Single().SqlName;
        var javaOrJakarta = _config.PersistenceMode.ToString().ToLower();
        fw.AddImport($"{javaOrJakarta}.persistence.CascadeType");
        if (property is ReverseAssociationProperty rap)
        {
            fw.WriteLine(1, @$"@{property.Type}(cascade = {{CascadeType.PERSIST, CascadeType.MERGE}}, fetch = FetchType.LAZY, mappedBy = ""{rap.ReverseProperty.NameByClassCamel}"")");
        }
        else
        {
            var hasReverse = property.Class.Namespace.RootModule == property.Association.Namespace.RootModule;
            fw.WriteLine(1, @$"@{property.Type}(cascade = CascadeType.ALL, fetch = FetchType.LAZY{(hasReverse ? @$", mappedBy = ""{property.Class.NameCamel}{property.Role ?? string.Empty}""" : string.Empty)})");
            if (!hasReverse)
            {
                fw.WriteLine(1, @$"@JoinColumn(name = ""{pk}"", referencedColumnName = ""{pk}"")");
                fw.AddImport($"{javaOrJakarta}.persistence.JoinColumn");
            }
        }
    }

    private void WriteOneToOne(JavaWriter fw, AssociationProperty property)
    {
        var fk = ((IFieldProperty)property).SqlName;
        var apk = property.Property.SqlName;
        var javaOrJakarta = _config.PersistenceMode.ToString().ToLower();
        fw.AddImport($"{javaOrJakarta}.persistence.CascadeType");
        fw.WriteLine(1, @$"@{property.Type}(fetch = FetchType.LAZY, cascade = CascadeType.ALL, optional = {(!property.Required).ToString().ToLower()})");
        fw.WriteLine(1, @$"@JoinColumn(name = ""{fk}"", referencedColumnName = ""{apk}"", unique = true)");
        fw.AddImport($"{javaOrJakarta}.persistence.JoinColumn");
    }

    private void WriteProperty(JavaWriter fw, Class classe, AssociationProperty property)
    {
        var javaOrJakarta = _config.PersistenceMode.ToString().ToLower();
        fw.WriteDocEnd(1);
        fw.AddImport($"{javaOrJakarta}.persistence.FetchType");
        fw.AddImport($"{javaOrJakarta}.persistence.{property.Type}");
        switch (property.Type)
        {
            case AssociationType.ManyToOne:
                WriteManyToOne(fw, property);
                break;
            case AssociationType.OneToMany:
                WriteOneToMany(fw, classe, property);
                break;
            case AssociationType.ManyToMany:
                WriteManyToMany(fw, classe, property);
                break;
            case AssociationType.OneToOne:
                WriteOneToOne(fw, property);
                break;
        }

        if (property.Type == AssociationType.ManyToMany || property.Type == AssociationType.OneToMany)
        {
            if (property.Association.OrderProperty != null)
            {
                fw.WriteLine(1, @$"@OrderBy(""{property.Association.OrderProperty.NameByClassCamel} ASC"")");
                fw.AddImport($"{javaOrJakarta}.persistence.OrderBy");
            }
        }

        var suffix = string.Empty;

        if (_config.CanClassUseEnums(property.Association))
        {
            var defaultValue = _config.GetDefaultValue(property, _classes);
            if (defaultValue != "null")
            {
                suffix = $" = {defaultValue}.getEntity()";
            }
        }

        if (property.PrimaryKey)
        {
            fw.AddImport($"{javaOrJakarta}.persistence.Id");
            fw.WriteLine(1, "@Id");
        }

        fw.WriteLine(1, $"private {_config.GetType(property, useClassForAssociation: classe.IsPersistent)} {property.NameByClassCamel}{suffix};");
    }

    private void WriteProperty(JavaWriter fw, Class classe, IFieldProperty property, string tag)
    {
        var javaOrJakarta = _config.PersistenceMode.ToString().ToLower();
        if (property is AliasProperty alp)
        {
            fw.WriteLine(1, $" * Alias of {{@link {alp.Property.Class.GetImport(_config, tag)}#get{alp.Property.NameCamel.ToFirstUpper()}() {alp.Property.Class.NamePascal}#get{alp.Property.NameCamel.ToFirstUpper()}()}} ");
        }

        fw.WriteDocEnd(1);
        if (property.PrimaryKey && classe.IsPersistent)
        {
            fw.WriteLine(1, "@Id");
            fw.AddImport($"{javaOrJakarta}.persistence.Id");
            if (property.Domain.AutoGeneratedValue && classe.PrimaryKey.Count() == 1)
            {
                fw.AddImports(new List<string>
                {
                    $"{javaOrJakarta}.persistence.GeneratedValue",
                    $"{javaOrJakarta}.persistence.GenerationType"
                });

                if (_config.Identity.Mode == IdentityMode.IDENTITY)
                {
                    fw.WriteLine(1, @$"@GeneratedValue(strategy = GenerationType.IDENTITY)");
                }
                else if (_config.Identity.Mode == IdentityMode.SEQUENCE)
                {
                    fw.AddImport($"{javaOrJakarta}.persistence.SequenceGenerator");
                    var seqName = $"SEQ_{classe.SqlName}";
                    var initialValue = _config.Identity.Start != null ? $", initialValue = {_config.Identity.Start}" : string.Empty;
                    var increment = _config.Identity.Increment != null ? $", allocationSize = {_config.Identity.Increment}" : string.Empty;
                    fw.WriteLine(1, @$"@SequenceGenerator(name = ""{seqName}"", sequenceName = ""{seqName}""{initialValue}{increment})");
                    fw.WriteLine(1, @$"@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = ""{seqName}"")");
                }
            }
        }

        if (classe.IsPersistent && !_config.GetImplementation(property.Domain)!.Annotations
        .Where(i =>
                classe.IsPersistent && (Target.Persisted & i.Target) > 0
            || !classe.IsPersistent && (Target.Dto & i.Target) > 0)
            .Any(a => a.Text.Replace("@", string.Empty).StartsWith("Column")))
        {
            var column = @$"@Column(name = ""{property.SqlName}"", nullable = {(!property.Required).ToString().ToFirstLower()}";
            if (property.Domain.Length != null)
            {
                if (_config.GetImplementation(property.Domain)!.Type.ToUpper() == "STRING")
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
            fw.AddImport($"{javaOrJakarta}.persistence.Column");
            fw.WriteLine(1, column);
        }
        else if (property.Required && !property.PrimaryKey && !classe.IsPersistent)
        {
            fw.WriteLine(1, @$"@NotNull");
            fw.AddImport($"{javaOrJakarta}.validation.constraints.NotNull");
        }

        if (property.PrimaryKey && classe.Reference && classe.Values.Any())
        {
            fw.AddImports(new List<string>
            {
                $"{javaOrJakarta}.persistence.Enumerated",
                $"{javaOrJakarta}.persistence.EnumType",
            });
            fw.WriteLine(1, "@Enumerated(EnumType.STRING)");
        }

        foreach (var annotation in _config.GetDomainAnnotations(property, tag))
        {
            fw.WriteLine(1, $"{(annotation.StartsWith("@") ? string.Empty : '@')}{annotation}");
        }

        var defaultValue = _config.GetDefaultValue(property, _classes);
        var suffix = defaultValue != "null" ? $" = {defaultValue}" : string.Empty;
        fw.WriteLine(1, $"private {_config.GetType(property, useClassForAssociation: classe.IsPersistent)} {property.NameByClassCamel}{suffix};");
    }
}
