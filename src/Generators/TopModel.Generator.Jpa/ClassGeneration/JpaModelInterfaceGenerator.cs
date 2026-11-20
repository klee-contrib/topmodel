using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de DAOs JPA.
/// </summary>
public class JpaModelInterfaceGenerator(ILogger<JpaModelInterfaceGenerator> logger, IFileWriterProvider writerProvider)
    : JavaClassGeneratorBase(logger, writerProvider)
{
    public override string Name => "JpaInterfaceGen";

    protected override bool FilterClass(Class classe)
    {
        return classe.Type == ClassType.Interface;
    }

    protected override IEnumerable<JavaField> GetFields(Class classe, string tag)
    {
        return [];
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Config.GetClassFileName(classe, tag);
    }

    protected override IEnumerable<JavaMethod> GetGetters(Class classe, string tag)
    {
        foreach (var property in Config.GetProperties(classe))
        {
            var getter = JpaModelPropertyGenerator.GetGetter(tag, property);
            getter.Body.Clear();
            getter.ReturnComment = string.Empty;
            getter.Comment = property.Comment;
            getter.Visibility = string.Empty;
            yield return getter;
        }
    }

    protected override IEnumerable<JavaMethod> GetSetters(Class classe, string tag)
    {
        foreach (var property in Config.GetProperties(classe).Where(p => !p.Readonly))
        {
            var getter = JpaModelPropertyGenerator.GetSetter(tag, property);
            getter.Body.Clear();
            getter.ReturnComment = string.Empty;
            getter.Comment = property.Comment;
            getter.Visibility = string.Empty;
            yield return getter;
        }
    }

    protected override JavaClass InitClass(Class classe, string tag)
    {
        var javaClass = base.InitClass(classe, tag);
        javaClass.ClassType = "interface";
        return javaClass;
    }
}
