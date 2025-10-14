using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

public class JpaMetaModelGenerator(ILogger<JavaClassGeneratorBase> logger, IFileWriterProvider writerProvider)
    : ClassGeneratorBase<JpaConfig>(logger, writerProvider)
{
    private JpaModelPropertyGenerator _jpaModelConstructorGenerator;
    protected virtual JpaModelPropertyGenerator jpaModelPropertyGenerator
    {
        get
        {
            _jpaModelConstructorGenerator ??= new JpaModelPropertyGenerator(Config, new Dictionary<string, string>());
            return _jpaModelConstructorGenerator;
        }
    }
    public override string Name => "JpaMetaModelGen";

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
                "StaticMetaModel",
                imports: $"{Config.JavaxOrJakarta}.persistence.StaticMetaModel"
            ).AddAttribute($"{classe.NamePascal}.class")
        );
        if (Config.GeneratedHint)
        {
            javaClass.Add(Config.GeneratedAnnotation);
        }

        foreach (var property in jpaModelPropertyGenerator.GetAvailableProperties(classe))
        {
            javaClass.Add(
                new JavaField("String", jpaModelPropertyGenerator.GetPropertyName(property).ToConstantCase())
                {
                    Static = true,
                    Final = true,
                    DefaultValue = $"\"{jpaModelPropertyGenerator.GetPropertyName(property)}\"",
                }
            );
        }

        fw.Write(0, javaClass);
    }
}
