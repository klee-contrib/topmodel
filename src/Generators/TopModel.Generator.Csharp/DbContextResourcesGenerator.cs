using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Csharp;

public class DbContextResourcesGenerator(
    ILogger<DbContextResourcesGenerator> logger,
    TranslationStore translationStore,
    IFileWriterProvider writerProvider
) : TranslationGeneratorBase<CsharpConfig>(logger, translationStore, writerProvider)
{
    private readonly TranslationStore _translationStore = translationStore;

    public override string Name => "CSharpDbCxtResGen";

    protected override string? GetResourceFilePath(IProperty property, string tag, string lang)
    {
        if (
            Config.AvailableClasses.Any(c => c.Translation)
            && (
                Config.PersistedPropertiesResources
                    && (property.ResourceProperty.Label != null || _translationStore.AllowPropertyLabelFallback)
                || Config.PersistedReferencesResources
                    && property.ResourceProperty.Class
                        is { DefaultProperty: not null, Enum: not null, Values.Count: > 0 }
            )
        )
        {
            return Config
                .GetDbContextFilePath(tag, property.Parent.Namespace.Module)
                .Replace(".cs", $".resources.{lang}.cs")
                .Replace("..", ".");
        }

        return null;
    }

    protected override async Task HandleResourceFile(
        string filePath,
        string tag,
        string lang,
        IEnumerable<IProperty> properties,
        CancellationToken ct = default
    )
    {
        await using var w = this.OpenCSharpWriter(filePath);

        var module = properties.First().Parent.Namespace.Module;
        var dbContextName = Config.GetDbContextName(tag, module);
        var contextNs = Config.GetDbContextNamespace(tag, module);

        w.WriteNamespace(contextNs);

        w.AddUsing("Microsoft.EntityFrameworkCore");

        w.WriteSummary(Config.GetMessage(CsharpMessage.EfTranslationsPartial));
        w.WriteClassDeclaration(dbContextName);
        w.WriteLine(1, $"partial void Add{lang.ToPascalCase()}Resources(ModelBuilder modelBuilder)");
        w.WriteLine(1, "{");

        foreach (var translationClass in Config.AvailableClasses.Where(c => c.Translation))
        {
            w.AddUsing(Config.GetNamespace(translationClass, Config.GetBestClassTag(translationClass, tag)));

            w.WriteLine(2, $"modelBuilder.Entity<{Config.GetTypeName(translationClass)}>().HasData(");
            var containers = properties.GroupBy(prop => prop.Parent);

            var orderedContainers = containers
                .OrderBy(c => c.Key.NamePascal)
                .Select(container =>
                    (
                        container,
                        values: container.Key is Class { Enum: not null, DefaultProperty: not null } classe
                        && Config.PersistedReferencesResources
                            ? classe.Values.OrderBy(p => p.ResourceKey, StringComparer.Ordinal).ToList()
                            : []
                    )
                )
                .ToList();

            var lastContainer = orderedContainers[^1].container;
            var lastContainerWithValues = orderedContainers.LastOrDefault(c => c.values.Any()).container;

            foreach (var (container, values) in orderedContainers)
            {
                if (Config.PersistedPropertiesResources)
                {
                    var orderedProperties = container.OrderBy(p => p.NamePascal, StringComparer.Ordinal).ToList();
                    foreach (var property in orderedProperties)
                    {
                        if (property.Label != null || _translationStore.AllowPropertyLabelFallback)
                        {
                            w.Write(
                                3,
                                $"new {Config.GetTypeName(translationClass)} {{ {translationClass.PrimaryKey.Single(p => p != translationClass.LocaleProperty).NamePascal} = \"{property.ResourceKey}\"{(translationClass.LocaleProperty != null ? $", {translationClass.LocaleProperty.NamePascal} = \"{lang}\"" : string.Empty)}, {translationClass.DefaultProperty!.NamePascal} = \"{_translationStore.GetTranslation(property, lang)}\" }}"
                            );

                            if (property == orderedProperties[^1] && container == lastContainer && values.Count == 0)
                            {
                                w.WriteLine();
                            }
                            else
                            {
                                w.WriteLine(",");
                            }
                        }
                    }
                }

                foreach (var value in values)
                {
                    w.Write(
                        3,
                        $"new {Config.GetTypeName(translationClass)} {{ {translationClass.PrimaryKey.Single(p => p != translationClass.LocaleProperty).NamePascal} = \"{value.ResourceKey}\"{(translationClass.LocaleProperty != null ? $", {translationClass.LocaleProperty.NamePascal} = \"{lang}\"" : string.Empty)}, {translationClass.DefaultProperty!.NamePascal} = \"{_translationStore.GetTranslation(value, lang)}\" }}"
                    );

                    if (
                        value == values[^1]
                        && (
                            container == lastContainer && Config.PersistedPropertiesResources
                            || container == lastContainerWithValues && !Config.PersistedPropertiesResources
                        )
                    )
                    {
                        w.WriteLine();
                    }
                    else
                    {
                        w.WriteLine(",");
                    }
                }
            }

            w.WriteLine(2, ");");
        }

        w.WriteLine(1, "}");
        w.WriteLine("}");
    }
}
