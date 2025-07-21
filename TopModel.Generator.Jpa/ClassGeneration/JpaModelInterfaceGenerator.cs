using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de DAOs JPA.
/// </summary>
public class JpaModelInterfaceGenerator(ILogger<JpaModelInterfaceGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGeneratorBase<JpaConfig>(logger, writerProvider)
{
    public override string Name => "JpaInterfaceGen";

    protected override bool FilterClass(Class classe)
    {
        return classe.Abstract;
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Config.GetClassFileName(classe, tag);
    }

    protected IEnumerable<JavaMethod> GetGetters(Class classe, string tag)
    {
        foreach (var property in classe.Properties.Where(p => !(p is AssociationProperty apo && apo.Association.Reference && (apo.Type == AssociationType.OneToOne || apo.Type == AssociationType.ManyToOne))))
        {
            var getterPrefix = Config.GetType(property) == "boolean" ? "is" : "get";
            yield return new JavaMethod(Config.GetType(property), property.NameByClassPascal.WithPrefix(getterPrefix))
            {
                Comment = $"Getter for {property.NameByClassCamel}",
                ReturnComment = $"value of {{@link {classe.GetImport(Config, tag)}#{property.NameByClassCamel} {property.NameByClassCamel}}}"
            };
        }
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        var packageName = Config.GetPackageName(classe, tag);
        using var fw = this.OpenJavaWriter(fileName, packageName, null);
        var javaxOrJakarta = Config.PersistenceMode.ToString().ToLower();

        WriteImports(fw, classe, tag);
        fw.WriteLine();

        var extends = Config.GetClassExtends(classe, tag);
        var implements = Config.GetClassImplements(classe, tag);

        if (Config.GeneratedHint)
        {
            fw.WriteLine(0, Config.GeneratedAnnotation);
        }

        fw.WriteLine($"public interface {classe.NamePascal} {{");

        foreach (var getter in GetGetters(classe, tag))
        {
            fw.Write(1, getter);
            fw.WriteLine();
        }

        if (classe.Properties.Any(p => !p.Readonly))
        {
            WriteHydrate(fw, classe);
        }

        fw.WriteLine("}");
    }

    protected virtual void WriteHydrate(JavaWriter fw, Class classe)
    {
        var properties = classe.Properties
            .Where(p => !p.Readonly)
            .Where(p => !(p is AssociationProperty apo && apo.Association.Reference && (apo.Type == AssociationType.OneToOne || apo.Type == AssociationType.ManyToOne)));

        if (!properties.Any())
        {
            return;
        }

        fw.WriteDocStart(1, $"hydrate values of instance");
        foreach (var property in properties)
        {
            var propertyName = property.NameByClassCamel;
            fw.WriteLine(1, $" * @param {propertyName} value to set");
        }

        fw.WriteDocEnd(1);
        var signature = string.Join(", ", properties.Select(property =>
            {
                var propertyName = property.NameByClassCamel;
                return $@"{Config.GetType(property)} {property.NameByClassCamel}";
            }));

        fw.WriteLine(1, $"void hydrate({signature});");
    }

    protected virtual void WriteImports(JavaWriter fw, Class classe, string tag)
    {
        var imports = new List<string>
            {
                Config.PersistenceMode.ToString().ToLower() + ".annotation.Generated",
            };
        foreach (var property in classe.Properties)
        {
            imports.AddRange(property.GetTypeImports(Config, tag));

            if (property is CompositionProperty cp && cp.Composition.Namespace.Module == cp.Class?.Namespace.Module)
            {
                imports.Add(cp.Composition.GetImport(Config, tag));
            }
        }

        if (classe.Extends != null)
        {
            foreach (var property in classe.Extends.Properties)
            {
                imports.AddRange(property.GetTypeImports(Config, tag));
            }
        }

        fw.AddImports(imports);
    }
}