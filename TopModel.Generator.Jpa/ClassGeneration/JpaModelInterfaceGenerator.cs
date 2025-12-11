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
        return classe.Abstract;
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
        foreach (var property in classe.Properties)
        {
            var getter = JpaModelPropertyGenerator.GetGetter(tag, property);
            getter.Body.Clear();
            getter.ReturnComment = string.Empty;
            getter.Comment = property.Comment;
            getter.Visibility = string.Empty;
            yield return getter;
        }
    }

    protected virtual JavaMethod? GetHydrate(Class classe, string tag)
    {
        var properties = classe.Properties.Where(p => !p.Readonly);

        if (!properties.Any())
        {
            return null;
        }
        var hydrate = new JavaMethod("void", "hydrate") { Comment = "Hydrate values of instance" };
        foreach (var property in properties)
        {
            var parameter = new JavaMethodParameter(Config.GetType(property), property.NameByClassCamel)
            {
                Comment = $"value to set",
            };
            parameter.Imports.AddRange(property.GetTypeImports(Config, tag));
            hydrate.AddParameter(parameter);
        }

        return hydrate;
    }

    protected override IEnumerable<JavaMethod> GetMethods(Class classe, string tag)
    {
        foreach (var method in GetGetters(classe, tag))
        {
            yield return method;
        }
        var hydrate = GetHydrate(classe, tag);
        if (hydrate != null)
        {
            yield return hydrate;
        }
    }

    protected override JavaClass InitClass(Class classe, string tag)
    {
        var javaClass = base.InitClass(classe, tag);
        javaClass.ClassType = "interface";
        return javaClass;
    }
}
