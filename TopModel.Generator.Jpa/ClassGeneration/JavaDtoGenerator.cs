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

        var javaClass = new JavaClass(classe.NamePascal) { Comment = classe.Comment };
        javaClass.AddRange(GetAnnotations(classe, tag));
        var extends = Config.GetClassExtends(classe, tag);
        if (classe.Extends is not null)
        {
            javaClass.Extends = extends;
            javaClass.Imports.Add(classe.Extends.GetImport(Config, Config.GetBestClassTag(classe.Extends, tag)));
        }

        var implements = Config.GetClassImplements(classe, tag).ToList();
        implements.Add("Serializable");
        javaClass.Implements.AddRange(implements);
        javaClass.Imports.Add("java.io.Serializable");
        javaClass.Add(
            new JavaField("long", "serialVersionUID")
            {
                Static = true,
                Final = true,
                Comment = { "Serial ID" },
                DefaultValue = "1L",
            }.Add(new JavaAnnotation("Serial", imports: "java.io.Serial"))
        );
        javaClass.AddRange(GetConstuctors(classe, tag));
        javaClass.AddRange(JpaModelPropertyGenerator.GetProperties(classe, tag));
        javaClass.AddRange(GetGetters(classe, tag));
        javaClass.AddRange(GetSetters(classe, tag));

        if (Config.MappersInClass)
        {
            javaClass.AddRange(GetToMappers(classe, tag));
        }

        if (Config.FieldsEnum.Contains(AnnotationConstraint.NonPersisted))
        {
            var fieldEnum = GetFieldsEnum(classe, tag);
            javaClass.InnerClasses.Add(fieldEnum);
        }

        using var fw = this.OpenJavaWriter(fileName, packageName, codePage: null);
        fw.Write(0, javaClass);
    }

    protected virtual void WriteConstuctors(JavaWriter fw, Class classe, string tag)
    {
        if (
            Config.MappersInClass && classe.FromMappers.Any(c => c.ClassParams.All(p => Classes.Contains(p.Class)))
            || Classes.Any(c => c.Extends == classe)
            || Config.GetClassExtends(classe, tag) != null
        )
        {
            ConstructorGenerator.WriteNoArgConstructor(fw, classe, tag);
        }

        if (Config.MappersInClass)
        {
            ConstructorGenerator.WriteFromMappers(fw, classe, Classes, tag);
        }
    }

    protected virtual IEnumerable<JavaMethod> GetConstuctors(Class classe, string tag)
    {
        if (
            Config.MappersInClass && classe.FromMappers.Any(c => c.ClassParams.All(p => Classes.Contains(p.Class)))
            || Classes.Any(c => c.Extends == classe)
            || Config.GetClassExtends(classe, tag) != null
        )
        {
            yield return ConstructorGenerator.GetNoArgConstructor(classe, tag);
        }

        if (Config.MappersInClass)
        {
            foreach (var constructor in ConstructorGenerator.GetFromMappers(classe, Classes, tag))
            {
                yield return constructor;
            }
        }
    }

    protected virtual void WriteStaticMembers(JavaWriter fw, Class classe) { }
}
