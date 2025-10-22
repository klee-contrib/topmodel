using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Csharp;

public class DbContextResourcesGenerator(
    ILogger<TranslationGeneratorBase<CsharpConfig>> logger,
    TranslationStore translationStore,
    IFileWriterProvider writerProvider
) : TranslationGeneratorBase<CsharpConfig>(logger, translationStore, writerProvider)
{
    private readonly TranslationStore _translationStore = translationStore;

    public override string Name => "CSharpDbCxtResGen";

    protected override string? GetResourceFilePath(IProperty property, string tag, string lang)
    {
        if (Config.AvailableClasses.Any(c => c.Translation))
        {
            return Config.GetDbContextFilePath(tag).Replace(".cs", $".resources.{lang}.cs").Replace("..", ".");
        }

        return null;
    }

    protected override void HandleResourceFile(
        string filePath,
        string tag,
        string lang,
        IEnumerable<IProperty> properties
    )
    {
        using var cw = this.OpenCSharpWriter(filePath);

        var dbContextName = Config.GetDbContextName(tag);
        var usings = new List<string> { "Microsoft.EntityFrameworkCore" };
        var contextNs = Config.GetDbContextNamespace(tag);

        foreach (var classe in Config.AvailableClasses.Where(c => c.Translation))
        {
            usings.Add(Config.GetNamespace(classe, Config.GetBestClassTag(classe, tag)));
        }

        cw.AddUsings(usings);

        cw.WriteNamespace(contextNs);

        cw.WriteSummary("Partial pour ajouter les traductions dans EF.");
        cw.WriteLine($"public partial class {dbContextName} : DbContext");
        cw.WriteLine("{");
        cw.WriteLine(1, $"partial void Add{lang.ToPascalCase()}Resources(ModelBuilder modelBuilder)");
        cw.WriteLine(1, "{");

        foreach (var translationClass in Config.AvailableClasses.Where(c => c.Translation))
        {
            cw.WriteLine(2, $"modelBuilder.Entity<{translationClass.NamePascal}>().HasData(");
            var containers = properties.GroupBy(prop => prop.Parent);

            var orderedContainers = containers
                .OrderBy(c => c.Key.NamePascal)
                .Select(container =>
                    (
                        container,
                        values: container.Key is Class classe
                        && classe.DefaultProperty != null
                        && Config.PersistedReferencesResources
                            ? classe.Values.OrderBy(p => p.ResourceKey, StringComparer.Ordinal).ToList()
                            : []
                    )
                )
                .ToList();

            var lastContainer = orderedContainers[^1].container;
            var lastContainerWithValues = orderedContainers.Where(c => c.values.Any()).ToList()[^1].container;

            foreach (var (container, values) in orderedContainers)
            {
                if (Config.PersistedPropertiesResources)
                {
                    var orderedProperties = container.OrderBy(p => p.NamePascal, StringComparer.Ordinal).ToList();
                    foreach (var property in orderedProperties)
                    {
                        if (property.Label != null)
                        {
                            cw.Write(
                                3,
                                $"new {translationClass.NamePascal} {{ {translationClass.PrimaryKey.Single(p => p != translationClass.LocaleProperty).NamePascal} = \"{property.ResourceKey}\"{(translationClass.LocaleProperty != null ? $", {translationClass.LocaleProperty.NamePascal} = \"{lang}\"" : string.Empty)}, {translationClass.DefaultProperty!.NamePascal} = \"{_translationStore.GetTranslation(property, lang)}\" }}"
                            );

                            if (property == orderedProperties[^1] && container == lastContainer && values.Count == 0)
                            {
                                cw.WriteLine();
                            }
                            else
                            {
                                cw.WriteLine(",");
                            }
                        }
                    }
                }

                foreach (var value in values)
                {
                    cw.Write(
                        3,
                        $"new {translationClass.NamePascal} {{ {translationClass.PrimaryKey.Single(p => p != translationClass.LocaleProperty).NamePascal} = \"{value.ResourceKey}\"{(translationClass.LocaleProperty != null ? $", {translationClass.LocaleProperty.NamePascal} = \"{lang}\"" : string.Empty)}, {translationClass.DefaultProperty!.NamePascal} = \"{_translationStore.GetTranslation(value, lang)}\" }}"
                    );

                    if (
                        value == values[^1]
                        && (
                            container == lastContainer && Config.PersistedPropertiesResources
                            || container == lastContainerWithValues && !Config.PersistedPropertiesResources
                        )
                    )
                    {
                        cw.WriteLine();
                    }
                    else
                    {
                        cw.WriteLine(",");
                    }
                }
            }

            cw.WriteLine(2, ");");
        }

        cw.WriteLine(1, "}");
        cw.WriteLine("}");
    }
}
