using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

public class SqlResourceGenerator(
    ILogger<SqlResourceGenerator> logger,
    TranslationStore translationStore,
    IFileWriterProvider writerProvider
) : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SqlResourceGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.IsPersistent && !classe.Abstract)
        {
            yield return ("resource", Config.Procedural!.ResourceFile!);
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        using var writer = this.OpenSqlWriter(fileName);

        var appName = classes.First().Namespace.App;

        writer.WriteSqlFileHeader(
            appName,
            fileName.Split('/')[^1],
            "Script de création des resources (libellés traduits)."
        );

        var propertiesMap = classes
            .Where(c => c != null && c.Properties != null)
            .OrderBy(c => c.SqlName)
            .SelectMany(c => c.Properties)
            .Where(p =>
                p.ResourceProperty.Parent.Namespace.Module != null
                && p.Label != null
                && p.ResourceProperty != null
                && p.Class != null
            )
            .DistinctBy(property => property.ResourceKey)
            .GroupBy(property => property.Class)
            .ToDictionary(g => g.Key, g => g.Select(t => t));

        foreach (var modelClass in propertiesMap.Keys)
        {
            if (propertiesMap.TryGetValue(modelClass, out var properties))
            {
                if (
                    Config.TranslateProperties == true
                    && properties.Any(p => p.Label != null)
                    && modelClass.ModelFile != null
                )
                {
                    writer.WriteLine();
                    writer.WriteLine(
                        "/**\t\tInitialisation des traductions des propriétés de la table "
                            + modelClass.SqlName
                            + "\t\t**/"
                    );

                    foreach (var lang in translationStore.Translations.Keys)
                    {
                        foreach (
                            var property in properties
                                .Where(p => p.Label != null)
                                .DistinctBy(property => property.ResourceKey)
                        )
                        {
                            foreach (var classe in Config.AvailableClasses.Where(c => c.Translation))
                            {
                                writer.WriteLine(
                                    $@"INSERT INTO {classe.SqlName}({classe.PrimaryKey.Single(p => p != classe.LocaleProperty).SqlName}{(classe.LocaleProperty != null ? $", {classe.LocaleProperty!.SqlName}" : string.Empty)}, {classe.DefaultProperty!.SqlName}) VALUES({SingleQuote(property.ResourceKey)}{(classe.LocaleProperty == null ? string.Empty : @$", {SingleQuote(lang)}")}, {SingleQuote(translationStore.GetTranslation(property, lang))});"
                                );
                            }
                        }
                    }
                }

                if (
                    modelClass.DefaultProperty != null
                    && modelClass.Values.Count > 0
                    && Config.TranslateReferences == true
                )
                {
                    writer.WriteLine();
                    writer.WriteLine(
                        "/**\t\tInitialisation des traductions des valeurs de la table "
                            + modelClass.SqlName
                            + "\t\t**/"
                    );
                    foreach (var lang in translationStore.Translations.Keys)
                    {
                        foreach (var val in modelClass.Values)
                        {
                            foreach (var classe in Config.AvailableClasses.Where(c => c.Translation))
                            {
                                writer.WriteLine(
                                    $@"INSERT INTO {classe.SqlName}({classe.PrimaryKey.Single(p => p != classe.LocaleProperty).SqlName}{(classe.LocaleProperty != null ? $", {classe.LocaleProperty!.SqlName}" : string.Empty)}, {classe.DefaultProperty!.SqlName}) VALUES({SingleQuote(val.ResourceKey)}{(classe.LocaleProperty == null ? string.Empty : @$", {SingleQuote(lang)}")}, {SingleQuote(translationStore.GetTranslation(val, lang))});"
                                );
                            }
                        }
                    }
                }
            }
        }
    }

    private static string SingleQuote(string name)
    {
        return $@"'{name.Replace("'", "''")}'";
    }
}
