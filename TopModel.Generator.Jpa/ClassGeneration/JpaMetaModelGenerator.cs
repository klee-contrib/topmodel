using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Generator.Jpa.ClassGeneration.Utils;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

public class JpaMetaModelGenerator(ILogger<JavaClassGeneratorBase> logger, IFileWriterProvider writerProvider)
    : ClassGeneratorBase<JpaConfig>(logger, writerProvider)
{
    private JpaModelPropertyGenerator? _jpaModelConstructorGenerator;

    public override string Name => "JpaMetaModelGen";

    protected virtual JpaModelPropertyGenerator JpaModelPropertyGenerator
    {
        get
        {
            _jpaModelConstructorGenerator ??= new JpaModelPropertyGenerator(Config, new Dictionary<string, string>());
            return _jpaModelConstructorGenerator;
        }
    }

    protected override bool FilterClass(Class classe)
    {
        return classe.IsPersistent && classe.Type != ClassType.Interface && classe.Enum != EnumMode.Enum;
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return $"{Config.GetClassFileName(classe, tag).Split(".java")[0]}_.java";
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        var packageName = Config.GetPackageName(classe, tag);
        using var fw = this.OpenJavaWriter(fileName, packageName, codePage: null);
        var javaClass = new JavaClass($"{classe.NamePascal}_");
        javaClass.Add(
            new JavaAnnotation(
                "StaticMetamodel",
                imports: "jakarta.persistence.metamodel.StaticMetamodel"
            ).AddAttribute($"{classe.NamePascal}.class")
        );
        if (Config.GeneratedHint)
        {
            javaClass.Add(Config.GeneratedAnnotation);
        }

        if (classe.Extends is not null)
        {
            javaClass.Imports.Add(classe.Extends.GetImport(Config, Config.GetBestClassTag(classe.Extends, tag)) + "_");
            javaClass.Extends = classe.Extends.NamePascal + "_";
        }

        foreach (var property in Config.GetAvailableProperties(classe))
        {
            var javaType = Config.GetType(property);
            var genericType = javaType.Split("<")[0];
            var attributeType = genericType switch
            {
                "List" => "ListAttribute",
                "Collection" => "CollectionAttribute",
                "Set" => "SetAttribute",
                "Map" => "MapAttribute",
                _ => "SingularAttribute",
            };

            var propertyType = Config.GetType(property);

            if (javaType != genericType)
            {
                propertyType = javaType.Split('<')[1].Split('>')[0];
            }

            var javaField = new JavaField($"{attributeType}<{classe.NamePascal}, {propertyType}>", property.NameCamel)
            {
                Static = true,
                Visibility = "public",
                Volatile = true,
            };

            var imports = property.GetTypeImports(Config, tag, skipDomainImports: attributeType != "SingularAttribute");
            javaField.Imports.Add($"jakarta.persistence.metamodel.{attributeType}");

            javaField.Imports.AddRange(imports);

            javaClass.Add(javaField);
        }

        foreach (var property in Config.GetAvailableProperties(classe))
        {
            javaClass.Add(
                new JavaField("String", property.NameCamel.ToConstantCase())
                {
                    Static = true,
                    Final = true,
                    Visibility = "public",
                    DefaultValue = $"\"{property.NameCamel}\"",
                }
            );
        }

        fw.Write(0, javaClass);
    }
}
