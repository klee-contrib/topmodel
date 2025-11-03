using System.Numerics;
using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

public class JpaMetaModelGenerator(ILogger<JavaClassGeneratorBase> logger, IFileWriterProvider writerProvider)
    : ClassGeneratorBase<JpaConfig>(logger, writerProvider)
{
    private JpaModelPropertyGenerator? _jpaModelConstructorGenerator;
    protected virtual JpaModelPropertyGenerator jpaModelPropertyGenerator
    {
        get
        {
            _jpaModelConstructorGenerator ??= new JpaModelPropertyGenerator(Config, new Dictionary<string, string>());
            return _jpaModelConstructorGenerator;
        }
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return $"{Config.GetClassFileName(classe, tag).Split(".java")[0]}_.java";
    }

    protected override bool FilterClass(Class classe)
    {
        return classe.IsPersistent && !classe.Abstract && !(Config.EnumsAsEnums && Config.CanClassUseEnums(classe));
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        var packageName = Config.GetPackageName(classe, tag);
        using var fw = this.OpenJavaWriter(fileName, packageName, codePage: null);
        var javaClass = new JavaClass($"{classe.NamePascal}_");
        javaClass.Add(
            new JavaAnnotation(
                "StaticMetamodel",
                imports: $"{Config.JavaxOrJakarta}.persistence.metamodel.StaticMetamodel"
            ).AddAttribute($"{classe.NamePascal}.class")
        );
        if (Config.GeneratedHint)
        {
            javaClass.Add(Config.GeneratedAnnotation);
        }

        if (classe.Extends is not null)
        {
            javaClass.Imports.Add(classe.Extends.GetImport(Config, Config.GetBestClassTag(classe.Extends, tag)) + "_");
            javaClass.Extends = classe.Extends + "_";
        }

        foreach (var property in jpaModelPropertyGenerator.GetAvailableProperties(classe))
        {
            var javaType = jpaModelPropertyGenerator.GetPropertyType(property);
            var genericType = javaType.Split("<")[0];
            var attributeType = genericType switch
            {
                "List" => "ListAttribute",
                "Collection" => "CollectionAttribute",
                "Set" => "SetAttribute",
                "Map" => "MapAttribute",
                _ => "SingularAttribute",
            };

            var propertyType = jpaModelPropertyGenerator.GetPropertyType(property);

            if (javaType != genericType)
            {
                propertyType = javaType.Split('<')[1].Split('>')[0];
            }

            var javaField = new JavaField(
                $"{attributeType}<{classe.NamePascal}, {propertyType}>",
                jpaModelPropertyGenerator.GetPropertyName(property)
            )
            {
                Static = true,
                Visibility = "public",
                Volatile = true,
            };

            var imports = property.GetTypeImports(Config, tag);
            javaField.Imports.Add($"jakarta.persistence.metamodel.{attributeType}");

            foreach (var import in imports.ToList())
            {
                javaField.Imports.Add(import);
            }

            javaClass.Add(javaField);
        }

        foreach (var property in jpaModelPropertyGenerator.GetAvailableProperties(classe))
        {
            javaClass.Add(
                new JavaField("String", jpaModelPropertyGenerator.GetPropertyName(property).ToConstantCase())
                {
                    Static = true,
                    Final = true,
                    Visibility = "public",
                    DefaultValue = $"\"{jpaModelPropertyGenerator.GetPropertyName(property)}\"",
                }
            );
        }

        fw.Write(0, javaClass);
    }

    public override string Name => "JpaMetaModelGen";
}
