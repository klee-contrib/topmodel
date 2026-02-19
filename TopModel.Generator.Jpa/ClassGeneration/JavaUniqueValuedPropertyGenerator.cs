using Microsoft.Extensions.Logging;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JavaUniqueValuedPropertyGenerator(
    ILogger<JavaEnumClassPropertyGenerator> logger,
    IFileWriterProvider writerProvider
) : GeneratorBase<JpaConfig>(logger, writerProvider)
{
    public override string Name => "JavaUniqValPropGen";

    public override IEnumerable<string> GeneratedFiles =>
        Config
            .Files.Values.SelectMany(f => f.Classes.Where(FilterClass))
            .SelectMany(c =>
                Config
                    .Tags.Intersect(c.Tags)
                    .SelectMany(tag => GetUniqueValuedProperties(c).Select(p => GetFileName(p, c, tag)))
            )
            .Distinct();

    protected static bool FilterClass(Class classe)
    {
        return !classe.Abstract && classe.Enum != EnumMode.Enum;
    }

    protected string GetFileName(IProperty property, Class classe, string tag)
    {
        return Config.GetEnumFileName(classe, tag, property);
    }

    protected virtual IEnumerable<IProperty> GetUniqueValuedProperties(Class classe)
    {
        return classe.Properties.Where(e => e.UniqueValuedProperty == e && e.EnumProperty == null);
    }

    protected void HandleClass(Class classe, string tag)
    {
        foreach (var p in GetUniqueValuedProperties(classe))
        {
            WriteStaticClass(p, classe, tag);
        }
    }

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            foreach (var classe in file.Classes.Where(FilterClass))
            {
                foreach (var tag in Config.Tags.Intersect(classe.Tags))
                {
                    HandleClass(classe, tag);
                }
            }
        }
    }

    protected virtual void WriteStaticClass(IProperty property, Class classe, string tag)
    {
        var packageName = Config.GetEnumPackageName(classe, tag);
        using var fw = this.OpenJavaWriter(Config.GetEnumFileName(classe, tag, property), packageName, codePage: null);
        var staticClass = GetStaticClass(property, classe);
        fw.Write(0, staticClass);
    }

    private JavaClass GetStaticClass(IProperty property, Class classe)
    {
        var javaClass = new JavaClass(Config.GetEnumType(property))
        {
            Comment = $"Valeurs connues de la propriété {property.NamePascal} de la classe {classe.NamePascal}",
        };
        var i = 0;
        var refs = Config.GetAllValues(classe).OrderBy(x => x.Name, StringComparer.Ordinal).ToList();

        foreach (var value in refs)
        {
            i++;
            var staticField = new JavaField(Config.GetType(property), value.Name.ToPascalCase(strictIfUppercase: true))
            {
                Visibility = "public",
                Static = true,
                Final = true,
                DefaultValue = Config.ShouldQuoteValue(property)
                    ? $"\"{value.Value[property]}\""
                    : value.Value[property],
            };
            if (classe.DefaultProperty != null)
            {
                staticField.Comment = [value.Value[classe.DefaultProperty]];
            }

            javaClass.Add(staticField);
        }

        return javaClass;
    }
}
