using TopModel.Core;
using TopModel.Core.Model.Implementation;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Php;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class PhpModelPropertyGenerator
{
    private readonly PhpConfig _config;

    public PhpModelPropertyGenerator(PhpConfig config)
    {
        _config = config;
    }

    public void WriteProperty(PhpWriter fw, Class classe, IProperty property, string tag)
    {
        switch (property)
        {
            case CompositionProperty cp:
                WriteProperty(fw, cp);
                break;
            case AssociationProperty { Association.IsPersistent: true } ap:
                WriteProperty(fw, classe, ap, tag);
                break;
            case IFieldProperty fp:
                WriteProperty(fw, classe, fp, tag);
                break;
        }
    }

    public void WriteProperty(PhpWriter fw, CompositionProperty property)
    {
        fw.WriteLine(1, $"private {_config.GetPhpType(property)} ${property.NameCamel};");
    }

    public void WriteProperties(PhpWriter fw, Class classe, IEnumerable<Class> availableClasses, string tag)
    {
        var isFirst = true;
        foreach (var property in classe.GetProperties(availableClasses, tag))
        {
            if (!isFirst)
            {
                fw.WriteLine();
            }

            isFirst = false;
            WriteProperty(fw, classe, property, tag);
        }
    }

    private void WriteProperty(PhpWriter fw, Class classe, AssociationProperty property, string tag)
    {
        if (property.Type == AssociationType.ManyToMany || property.Type == AssociationType.OneToMany)
        {
            fw.WriteDocStart(1, @$"@var Collection<{property.Association}>");
            fw.WriteDocEnd(1);
        }

        fw.AddImport(property.Association.GetImport(_config, tag));
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

        var defaultValue = string.Empty;
        if (!(property.DefaultValue == null || property.DefaultValue == "null" || property.DefaultValue == "undefined"))
        {
            defaultValue += " = ";
            var quote = string.Empty;
            if (property.GetPhpType() == "String")
            {
                quote = @"""";
            }

            defaultValue += quote + property.DefaultValue + quote;
        }

        fw.WriteLine(1, $"private {property.GetPhpType()} {property.GetAssociationName()}{defaultValue};");
    }

    private void WriteManyToOne(PhpWriter fw, Class classe, AssociationProperty property)
    {
        var fk = ((IFieldProperty)property).SqlName;
        var apk = property.Property.SqlName;
        fw.AddImport(@$"Doctrine\ORM\Mapping\ManyToOne");
        fw.AddImport(@$"Doctrine\ORM\Mapping\JoinColumn");
        fw.WriteLine(1, @$"#[ManyToOne(targetEntity: {property.Association}::class)]");
        fw.WriteLine(1, @$"#[JoinColumn(name: '{fk}', referencedColumnName: '{apk}')]");
    }

    private void WriteOneToOne(PhpWriter fw, Class classe, AssociationProperty property)
    {
        var fk = ((IFieldProperty)property).SqlName;
        var apk = property.Property.SqlName;
        fw.AddImport(@$"Doctrine\ORM\Mapping\OneToOne");
        fw.AddImport(@$"Doctrine\ORM\Mapping\JoinColumn");
        fw.WriteLine(1, @$"#[OneToOne(targetEntity: {property.Association.NamePascal}::class)]");
        fw.WriteLine(1, @$"#[JoinColumn(name: '{fk}', referencedColumnName: '{apk}')]");
    }

    private void WriteManyToMany(PhpWriter fw, Class classe, AssociationProperty property)
    {
        fw.AddImport(@$"Doctrine\ORM\Mapping\ManyToMany");
        fw.AddImport(@$"Doctrine\ORM\Mapping\InverseJoinColumn");
        fw.AddImport(@$"Doctrine\ORM\Mapping\JoinColumn");

        var role = property.Role is not null ? "_" + property.Role.ToConstantCase() : string.Empty;
        var fk = property.Property.SqlName;
        var pk = classe.PrimaryKey.Single().SqlName + role;

        fw.WriteLine(1, @$"#[JoinColumn(name: '{pk}', referencedColumnName: '{pk}')]");
        fw.WriteLine(1, @$"#[InverseJoinColumn(name: '{fk}', referencedColumnName: '{fk}')]");
        fw.WriteLine(1, @$"#[ManyToMany(targetEntity: {property.Association.NamePascal}::class)]");
    }

    private void WriteOneToMany(PhpWriter fw, Class classe, AssociationProperty property)
    {
        fw.AddImport(@$"Doctrine\ORM\Mapping\OneToMany");
        fw.WriteLine(1, @$"#[Doctrine\ORM\Mapping\OneToMany(mappedBy: '{(property is ReverseAssociationProperty rap ? rap.ReverseProperty.GetPhpName() : @$"{property.Class.NameCamel}{property.Role ?? string.Empty}")}', targetEntity: {property.Association.Name}::class)]");
    }

    private void WriteProperty(PhpWriter fw, Class classe, IFieldProperty property, string tag)
    {
        if (classe.IsPersistent)
        {
            if (property.PrimaryKey)
            {
                fw.WriteLine(1, @"#[Doctrine\ORM\Mapping\Id]");
                fw.AddImport(@$"Doctrine\ORM\Mapping\Id");
                var javaDomain = _config.GetImplementation(property.Domain)!;
                if (property.Domain.AutoGeneratedValue)
                {
                    fw.AddImport(@$"Doctrine\ORM\Mapping\GeneratedValue");
                    fw.WriteLine(1, @$"#[ORM\GeneratedValue]");
                }
            }

            fw.AddImport(@$"Doctrine\ORM\Mapping\Column");
            fw.WriteLine(1, $@"#[Doctrine\ORM\Mapping\Column(name: '{property.SqlName}')]");
        }
        else
        {
            if (property.Required)
            {
                fw.WriteLine(1, $@"#[Symfony\Component\Validator\Constraints\NotNull]");
                fw.AddImport(@$"Symfony\Component\Validator\Constraints\NotNull");
            }

            if (property.Domain.Length != null)
            {
                fw.WriteLine(1, $@"#[Symfony\Component\Validator\Constraints\Length]");
                fw.AddImport(@$"Symfony\Component\Validator\Constraints\Length(max: {property.Domain.Length})]");
            }
        }

        var phpType = _config.GetImplementation(property.Domain);
        if (phpType is not null && phpType.Annotations is not null)
        {
            foreach (var annotation in phpType.Annotations.Where(a =>
                classe.IsPersistent && (Target.Persisted & a.Target) > 0
            || !classe.IsPersistent && (Target.Dto & a.Target) > 0))
            {
                fw.AddImports(annotation.Imports.Select(i => i.ParseTemplate(property)));
                fw.WriteLine(1, $"{annotation.Text.ParseTemplate(property)}");
            }
        }

        var defaultValue = string.Empty;
        if (!(property.DefaultValue == null || property.DefaultValue == "null" || property.DefaultValue == "undefined"))
        {
            defaultValue += " = ";
            var quote = string.Empty;
            if (_config.GetPhpType(property) == "String")
            {
                quote = @"""";
            }

            defaultValue += quote + property.DefaultValue + quote;
        }

        fw.WriteLine(1, $"private {_config.GetPhpType(property)} ${property.GetPhpName()}{defaultValue};");
    }
}