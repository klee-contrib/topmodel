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
    ILogger<JavaUniqueValuedPropertyGenerator> logger,
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

    protected bool FilterClass(Class classe)
    {
        return classe.Type != ClassType.Interface
            && classe.Enum != EnumMode.Enum
            && Config.UniqueValueGeneration.CanConst;
    }

    protected string GetFileName(IProperty property, Class classe, string tag)
    {
        return Config.GetEnumFileName(classe, tag, property);
    }

    protected virtual IEnumerable<IProperty> GetUniqueValuedProperties(Class classe)
    {
        return Config
            .GetProperties(classe)
            .Where(e =>
                e.UniqueValuedProperty == e
                && (e.EnumProperty == null || Config.UniqueValueGeneration == UniqueValueGenerationMode.ConstOnly)
            );
    }

    protected async Task HandleClass(Class classe, string tag)
    {
        foreach (var p in GetUniqueValuedProperties(classe))
        {
            await WriteStaticClass(p, classe, tag);
        }
    }

    protected override async Task HandleFiles(IEnumerable<ModelFile> files, CancellationToken ct = default)
    {
        foreach (var file in files)
        {
            foreach (var classe in file.Classes.Where(FilterClass))
            {
                foreach (var tag in Config.Tags.Intersect(classe.Tags))
                {
                    await HandleClass(classe, tag);
                }
            }
        }
    }

    protected virtual async Task WriteStaticClass(IProperty property, Class classe, string tag)
    {
        var packageName = Config.GetEnumPackageName(classe, tag);
        await using var fw = this.OpenJavaWriter(
            Config.GetEnumFileName(classe, tag, property),
            packageName,
            codePage: null
        );
        var staticClass = GetStaticClass(property, classe, tag);
        fw.Write(staticClass);
    }

    private JavaClass GetStaticClass(IProperty property, Class classe, string tag)
    {
        var javaClass = new JavaClass(Config.GetEnumType(property))
        {
            Comment = $"Valeurs connues de la propriété {property.NamePascal} de la classe {classe.NamePascal}",
        };

        var emptyConstructor = new JavaConstructor(Config.GetEnumType(property)) { Visibility = "private" };
        emptyConstructor.AddBodyLine("// private constructor to hide implicite public one");
        javaClass.Add(emptyConstructor);

        var i = 0;
        var refs = Config.GetAllValues(classe).OrderBy(x => x.Name, StringComparer.Ordinal).ToList();

        foreach (var value in refs)
        {
            i++;

            var staticField = new JavaField(
                Config.GetType(property, tag),
                value.Name.ToPascalCase(strictIfUppercase: true)
            )
            {
                Visibility = "public",
                Static = true,
                Final = true,
                DefaultValue = Config.FormatValue(property, value.Value[property], tag),
            };
            if (classe.DefaultProperty != null)
            {
                staticField.AddCommentLine(value.Value[classe.DefaultProperty]);
            }

            javaClass.Add(staticField);
        }

        return javaClass;
    }
}
