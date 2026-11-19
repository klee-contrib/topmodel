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
        if (classe.IsPersistent)
        {
            yield return ("main", Config.GetClassesFilePath(tag, classe));
        }
    }

    protected override async Task HandleFile(
        string fileType,
        string fileName,
        string tag,
        IEnumerable<Class> classes,
        CancellationToken ct = default
    )
    {
        var classList = classes.OrderBy(c => c.ModelFile.Name + c.Namespace.Module + c.Name).ToList();
        if (classList.Count == 0)
        {
            return;
        }

        var title = Config.ClassesMode switch
        {
            DocumentationGenerationMode.All => Config.GetMessage(DocumentationMessage.SchemaDataDictionary),
            DocumentationGenerationMode.File => Config.GetMessage(
                DocumentationMessage.SchemaDataDictionaryFile,
                classList[0].ModelFile.Name
            ),
            _ => Config.GetMessage(DocumentationMessage.SchemaDataDictionaryModule, classList[0].Namespace.Module),
        };

        await using var md = new MarkdownWriter(OpenFileWriter(fileName, new() { EnableHeader = false }));
        md.WriteTitle(1, title);
        md.WriteLine();

        var table = new MarkdownTable() { Columns = GetColumns() };
        var schema = Config.Schemas.Where(s => s.Value.Contains(tag)).Select(s => s.Key).FirstOrDefault(string.Empty);

        foreach (var classe in classList)
        {
            var tableName = Config.GetSqlName(classe, tag);
            foreach (var property in Config.GetProperties(classe).Where(p => !p.IsReverseProperty))
            {
                List<string> constraints = [];
                if (property is { Association: not null })
                {
                    constraints.Add(Config.GetMessage(DocumentationMessage.SchemaForeignKey));
                }

                if (property.PrimaryKey)
                {
                    constraints.Add(Config.GetMessage(DocumentationMessage.SchemaPrimaryKey));
                }

                table.AddRow([
                    schema,
                    tableName,
                    property.Name,
                    Config.GetSqlName(property, tag),
                    property.Label,
                    Config.GetType(property, tag),
                    $"{property.Domain?.Length}",
                    property.Comment,
                    string.Join("<br>", constraints),
                    property.Required ? Config.GetMessage(DocumentationMessage.Yes) : string.Empty,
                    string.Join(
                        ", ",
                        property.Class.Values.Select(v =>
                            v.Value.TryGetValue(property, out var val) ? val : string.Empty
                        )
                    ),
                ]);

                schema = string.Empty;
                tableName = string.Empty;
            }
        }

        md.WriteTable(table);
    }

    private string[] GetColumns()
    {
        return
        [
            Config.GetMessage(DocumentationMessage.SchemaSchema),
            Config.GetMessage(DocumentationMessage.SchemaTable),
            Config.GetMessage(DocumentationMessage.SchemaDesignation),
            Config.GetMessage(DocumentationMessage.SchemaSqlName),
            Config.GetMessage(DocumentationMessage.SchemaLabel),
            Config.GetMessage(DocumentationMessage.SchemaSqlType),
            Config.GetMessage(DocumentationMessage.SchemaLength),
            Config.GetMessage(DocumentationMessage.EndpointHeaderDescription),
            Config.GetMessage(DocumentationMessage.SchemaConstraints),
            Config.GetMessage(DocumentationMessage.SchemaRequired),
            Config.GetMessage(DocumentationMessage.SchemaPossibleValues),
        ];
    }
}
