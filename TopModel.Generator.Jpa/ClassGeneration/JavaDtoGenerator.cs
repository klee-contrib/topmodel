using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Model.Implementation;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JavaDtoGenerator(ILogger<JavaDtoGenerator> logger, IFileWriterProvider writerProvider)
    : JavaClassGeneratorBase(logger, writerProvider)
{
    public override string Name => "JavaDtoGen";

    protected override bool FilterClass(Class classe)
    {
        return !classe.Abstract && !classe.IsPersistent && classe.Enum == null;
    }

    protected override IEnumerable<JavaField> GetFields(Class classe, string tag)
    {
        yield return new JavaField("long", "serialVersionUID")
        {
            Static = true,
            Final = true,
            Comment = { "Serial ID" },
            DefaultValue = "1L",
        }.Add(new JavaAnnotation("Serial", imports: "java.io.Serial"));

        foreach (var property in base.GetFields(classe, tag))
        {
            yield return property;
        }
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Path.Combine(
            Config.OutputDirectory,
            Config.ResolveVariables(Config.DtosPath, tag, module: classe.Namespace.Module).ToFilePath(),
            $"{classe.NamePascal}.java"
        );
    }

    protected override IEnumerable<JavaClass> GetInnerClasses(Class classe, string tag)
    {
        if (
            Config.FieldsEnum.Contains(AnnotationConstraint.NonPersisted)
            && JpaModelPropertyGenerator.GetAvailableProperties(classe).Any()
        )
        {
            var fieldEnum = GetFieldsEnum(classe, tag);
            yield return fieldEnum;
        }
    }

    protected override JavaClass InitClass(Class classe, string tag)
    {
        var javaClass = base.InitClass(classe, tag);
        javaClass.Implements.Add("Serializable");
        javaClass.Imports.Add("java.io.Serializable");
        return javaClass;
    }
}
