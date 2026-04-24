namespace TopModel.Generator.Documentation;

using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

/// <summary>
/// Générateur de documentation Markdown décrivant le dictionnaire de données.
/// </summary>
public class DocumentationClassDocGenerator(
    ILogger<DocumentationClassDocGenerator> logger,
    IFileWriterProvider writerProvider
) : ClassGroupGeneratorBase<DocumentationConfig>(logger, writerProvider)
{
    public override string Name => "DocClassGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (!classe.IsPersistent || classe.Abstract)
        {
            yield break;
        }

        yield return ("main", Config.GetClassesFilePath(tag, classe));
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        var classList = classes.OrderBy(c => c.ModelFile.Name + c.Namespace.Module + c.Name).ToList();
        if (classList.Count == 0)
        {
            return;
        }

        var title = Config.ClassesMode switch
        {
            DocumentationGenerationMode.All => "Dictionnaire de données",
            DocumentationGenerationMode.File => $"Dictionnaire de données du fichier {classList[0].ModelFile.Name}",
            _ => $"Dictionnaire de données du module {classList[0].Namespace.Module}",
        };

        using var md = new MarkdownWriter(OpenFileWriter(fileName));
        md.WriteTitle(1, title);
        md.WriteLine();

        var table = new MarkdownTable() { Columns = GetColumns() };
        var schema = Config.Schemas.Where(s => s.Value.Contains(tag)).Select(s => s.Key).FirstOrDefault(string.Empty);

        foreach (var classe in classList)
        {
            var tableName = classe.SqlName;
            foreach (var property in classe.Properties.Where(p => !p.IsReverseProperty))
            {
                List<string> constraints = [];
                if (property is { Association: not null })
                {
                    constraints.Add("Clé étrangère");
                }

                if (property.PrimaryKey)
                {
                    constraints.Add("Clé primaire");
                }

                table.AddRow(
                    [
                        schema,
                        tableName,
                        property.Name,
                        property.SqlName,
                        property.Label,
                        Config.GetType(property),
                        $"{property.Domain.Length}",
                        property.Comment,
                        string.Join("<br>", constraints),
                        property.Required ? "Oui" : string.Empty,
                        string.Join(
                            ", ",
                            property.Class.Values.Select(v =>
                                v.Value.TryGetValue(property, out var val) ? val : string.Empty
                            )
                        ),
                    ]
                );

                schema = string.Empty;
                tableName = string.Empty;
            }
        }

        md.WriteTable(table);
    }

    private static string[] GetColumns()
    {
        return
        [
            "Schéma",
            "Table",
            "Désignation",
            "Nom SQL",
            "Libellé",
            "Type SQL",
            "Longueur",
            "Description",
            "Contraintes",
            "Obligatoire",
            "Valeurs possibles",
        ];
    }
}
