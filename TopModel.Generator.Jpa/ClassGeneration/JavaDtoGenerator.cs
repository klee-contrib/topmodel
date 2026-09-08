using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Model.Implementation;
using TopModel.Generator.Jpa.ClassGeneration.Utils;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JavaDtoGenerator(ILogger<JavaDtoGenerator> logger, IFileWriterProvider writerProvider)
    : JavaClassGeneratorBase(logger, writerProvider)
{
    private JavaEnumGeneratorHelper? _javaEnumGeneratorHelper;
    public override string Name => "JavaDtoGen";

    protected override JavaEnumGeneratorHelper JavaConstructorGenerator
    {
        get
        {
            _javaEnumGeneratorHelper ??= new JavaEnumGeneratorHelper(Config);
            return _javaEnumGeneratorHelper;
        }
    }

    protected override bool FilterClass(Class classe)
    {
        return classe.Type != ClassType.Interface && !classe.IsPersistent && classe.Enum != EnumMode.Enum;
    }

    protected override IEnumerable<JavaConstructor> GetConstuctors(Class classe, string tag)
    {
        if (Config.IsRecord(classe, tag))
        {
            return [];
        }

        if (classe.Enum == EnumMode.Class && classe.Readonly)
        {
            var allArgsConstructor = JavaConstructorGenerator.GetAllArgsConstructor(classe, tag);
            allArgsConstructor.Visibility = "private";
            if (allArgsConstructor.Parameters.Count > 0)
            {
                return [allArgsConstructor];
            }

            return [];
        }

        return base.GetConstuctors(classe, tag);
    }

    protected override IEnumerable<JavaField> GetFields(Class classe, string tag)
    {
        if (classe.Enum == EnumMode.Class && classe.Readonly)
        {
            foreach (var javaFinalField in JavaConstructorGenerator.GetConstFields(classe, tag))
            {
                yield return javaFinalField;
            }
            yield return JavaConstructorGenerator.GetStaticValuesList(classe);
        }

        if (!Config.IsRecord(classe, tag))
        {
            yield return new JavaField("long", "serialVersionUID")
            {
                Static = true,
                Final = true,
                DefaultValue = "1L",
            }
                .AddCommentLine("Serial ID")
                .Add(new JavaAnnotation("Serial", imports: "java.io.Serial"));
        }

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
            Config.FieldsEnum.Contains(AnnotationConstraint.NonPersisted) && Config.GetAvailableProperties(classe).Any()
        )
        {
            var fieldEnum = GetFieldsEnum(classe, tag);
            yield return fieldEnum;
        }
    }

    protected override IEnumerable<JavaMethod> GetMethods(Class classe, string tag)
    {
        foreach (var method in base.GetMethods(classe, tag))
        {
            yield return method;
        }

        if (classe.Enum == EnumMode.Class && classe.Readonly && classe.EnumKey != null)
        {
            yield return JavaConstructorGenerator.GetGetValueStaticMethod(classe);
        }
    }

    protected override IEnumerable<JavaMethod> GetSetters(Class classe, string tag)
    {
        if (classe.Enum == EnumMode.Class && classe.Readonly || Config.IsRecord(classe, tag))
        {
            return [];
        }

        return base.GetSetters(classe, tag);
    }

    protected override IEnumerable<JavaMethod> GetGetters(Class classe, string tag)
    {
        if (classe.Enum == EnumMode.Class && classe.Readonly || Config.IsRecord(classe, tag))
        {
            return [];
        }

        return base.GetGetters(classe, tag);
    }

    protected override JavaClass InitClass(Class classe, string tag)
    {
        var javaClass = base.InitClass(classe, tag);
        if (!Config.IsRecord(classe, tag))
        {
            javaClass.Implements.Add("Serializable");
            javaClass.AddImports("java.io.Serializable");
        }
        return javaClass;
    }
}
