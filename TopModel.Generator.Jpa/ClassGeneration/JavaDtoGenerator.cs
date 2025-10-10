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
        return !classe.Abstract && !classe.IsPersistent && !Config.CanClassUseEnums(classe, Classes);
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Path.Combine(
            Config.OutputDirectory,
            Config.ResolveVariables(Config.DtosPath, tag, module: classe.Namespace.Module).ToFilePath(),
            $"{classe.NamePascal}.java"
        );
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        var packageName = Config.GetPackageName(classe, tag);

        var javaClass = base.InitClass(classe, tag);
        javaClass.Implements.Add("Serializable");
        javaClass.Imports.Add("java.io.Serializable");

        if (Config.FieldsEnum.Contains(AnnotationConstraint.NonPersisted))
        {
            var fieldEnum = GetFieldsEnum(classe, tag);
            javaClass.InnerClasses.Add(fieldEnum);
        }

        using var fw = this.OpenJavaWriter(fileName, packageName, codePage: null);
        fw.Write(0, javaClass);
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
}
