using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JpaModelPropertyGenerator
{
    private readonly JpaConfig _config;
    private readonly IDictionary<string, ModelFile> _files;

    public JpaModelPropertyGenerator(JpaConfig config, IDictionary<string, ModelFile> files)
    {
        _config = config;
        this._files = files;
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

    public void WriteProperties(JavaWriter fw, Class classe)
    {
        foreach (var property in classe.Properties)
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
        var fk = property.Property.SqlName + (property.Role is not null ? "_" + ModelUtils.ConvertCsharp2Bdd(property.Role) : string.Empty);
        var apk = property.Property.SqlName;
        fw.WriteLine(1, @$"@{property.Type}(fetch = FetchType.LAZY, optional = {(property.Required ? "false" : "true")}, targetEntity = {property.Association.Name}.class)");
        fw.WriteLine(1, @$"@JoinColumn(name = ""{fk}"", referencedColumnName = ""{apk}"")");
    }

    private void WriteOneToOne(JavaWriter fw, Class classe, AssociationProperty property)
    {
        var fk = property.Property.SqlName + (property.Role is not null ? "_" + ModelUtils.ConvertCsharp2Bdd(property.Role) : string.Empty);
        var apk = property.Property.SqlName;
        fw.WriteLine(1, @$"@{property.Type}(fetch = FetchType.LAZY, cascade = CascadeType.ALL, orphanRemoval = true, optional = {(property.Required ? "false" : "true")})");
        fw.WriteLine(1, @$"@JoinColumn(name = ""{fk}"", referencedColumnName = ""{apk}"", unique = true)");
    }

    private void WriteManyToMany(JavaWriter fw, Class classe, AssociationProperty property)
    {
        var fk = (property.Role is not null ? ModelUtils.ConvertCsharp2Bdd(property.Role) + "_" : string.Empty) + property.Property.SqlName;
        var pk = classe.PrimaryKey!.SqlName;
        if (property is JpaAssociationProperty jap)
        {
            fw.WriteLine(1, @$"@{property.Type}(fetch = FetchType.LAZY, mappedBy = ""{jap.ReverseProperty.GetJavaName()}"", cascade = {{ CascadeType.PERSIST, CascadeType.MERGE }})");
        }
        else
        {
            var hasRerverse = property.Class.Namespace.Module.Split('.').First() == property.Association.Namespace.Module.Split('.').First() && !property.Association.Reference;
            fw.WriteLine(1, @$"@{property.Type}(fetch = FetchType.LAZY, cascade = {{ CascadeType.PERSIST, CascadeType.MERGE }})");
            if (!hasRerverse)
            {
                fw.WriteLine(1, @$"@JoinTable(name = ""{property.Class.SqlName}_{property.Association.SqlName}{(property.Role != null ? "_" + ModelUtils.ConvertCsharp2Bdd(property.Role): string.Empty)}"", joinColumns = @JoinColumn(name = ""{pk}""), inverseJoinColumns = @JoinColumn(name = ""{fk}""))");
            }
        }
    }

    private void WriteOneToMany(JavaWriter fw, Class classe, AssociationProperty property)
    {
        var pk = classe.PrimaryKey!.SqlName;
        if (property is JpaAssociationProperty jap)
        {
            fw.WriteLine(1, @$"@{property.Type}(cascade = CascadeType.ALL, orphanRemoval = true, fetch = FetchType.LAZY, mappedBy = ""{jap.ReverseProperty.GetJavaName()}"")");
        }
        else
        {
            var hasRerverse = property.Class.Namespace.Module.Split('.').First() == property.Association.Namespace.Module.Split('.').First();
            fw.WriteLine(1, @$"@{property.Type}(cascade = CascadeType.ALL, orphanRemoval = true, fetch = FetchType.LAZY{(hasRerverse ? @$", mappedBy = ""{property.Class.Name.ToFirstLower()}{property.Role ?? string.Empty}""" : string.Empty)})");
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
                var seqName = $"SEQ_{classe.SqlName}";
                fw.WriteLine(1, @$"@SequenceGenerator(name = ""{seqName}"", sequenceName = ""{seqName}"", initialValue = 1000)");
                fw.WriteLine(1, @$"@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = ""{seqName}"")");
            }
        }

        if (classe.IsPersistent)
        {
            var column = @$"@Column(name = ""{property.SqlName}"", nullable = {(!property.Required).ToString().ToFirstLower()}{(classe.Reference ? ", updatable = false" : string.Empty)}";
            if (property.Domain.Length != null)
            {
                if (property.Domain.Java!.Type == "String" || property.Domain.Java.Type == "string")
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

        if (property.PrimaryKey && classe.Reference)
        {
            fw.WriteLine(1, "@Enumerated(EnumType.STRING)");
        }

        if (property.Domain.Java is not null && property.Domain.Java.Annotations is not null)
        {
            foreach (var annotation in property.Domain.Java.Annotations)
            {
                fw.WriteLine(1, $"{(annotation.StartsWith("@") ? string.Empty : '@')}{annotation}");
            }
        }

        fw.WriteLine(1, $"private {property.GetJavaType()} {property.GetJavaName()};");
    }
}