using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

public class SqlResourcesGenerator(
    ILogger<SqlResourcesGenerator> logger,
    TranslationStore translationStore,
    IFileWriterProvider writerProvider
) : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SqlResourcesGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (
            Config.AvailableClasses.Any(c => c.Translation)
            && classe.HasTable
            && (
                Config.TranslateReferences == true && classe.DefaultProperty != null && classe.Values.Any()
                || Config.TranslateProperties == true && classe.Properties.Any(c => c.Label != null)
            )
        )
        {
            yield return ("resources", Path.Combine(Config.OutputDirectory, Config.Procedural!.ResourcesFileName));
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        using var writer = this.OpenSqlWriter(fileName);

        var appName = classes.First().Namespace.App;

        writer.WriteSqlFileHeader(
            appName,
            fileName.Split('/')[^1],
            "Scripts d'insertion des ressources (libellés traduits)."
        );

        if (Config.TranslateProperties != null)
        {
            var propertiesMap = classes
                .OrderBy(c => c.SqlName)
                .SelectMany(c => c.Properties)
                .Where(p => p.Label != null)
                .DistinctBy(property => property.ResourceKey)
                .GroupBy(property => property.Class)
                .ToDictionary(g => g.Key);

            foreach (var modelClass in propertiesMap.Keys)
            {
                if (propertiesMap.TryGetValue(modelClass, out var properties))
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
            }
        }

        if (Config.TranslateReferences == true)
        {
            foreach (
                var modelClass in classes.Where(modelClass =>
                    modelClass.DefaultProperty != null && modelClass.Values.Count > 0
                )
            )
            {
                writer.WriteLine();
                writer.WriteLine(
                    "/**\t\tInitialisation des traductions des valeurs de la table " + modelClass.SqlName + "\t\t**/"
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

    private static string SingleQuote(string name)
    {
        return $@"'{name.Replace("'", "''")}'";
    }
}
