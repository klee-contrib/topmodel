namespace TopModel.Generator.Documentation;

using Microsoft.Extensions.Logging;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

/// <summary>
/// Générateur de documentation Markdown listant les endpoints.
/// </summary>
public class DocumentationEndpointDocGenerator(
    ILogger<DocumentationEndpointDocGenerator> logger,
    IFileWriterProvider writerProvider
) : EndpointsGeneratorBase<DocumentationConfig>(logger, writerProvider)
{
    public override string Name => "DocEndpointGen";

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Config.GetEndpointsFilePath(tag, file);
    }

    protected override async Task HandleFile(
        string filePath,
        string fileName,
        string tag,
        IList<Endpoint> endpoints,
        CancellationToken ct = default
    )
    {
        if (endpoints.Count == 0)
        {
            return;
        }

        var file = endpoints[0].ModelFile;
        var title = Config.EndpointsMode switch
        {
            DocumentationGenerationMode.All => Config.GetMessage(DocumentationMessage.SchemaEndpointList),
            DocumentationGenerationMode.File => Config.GetMessage(
                DocumentationMessage.SchemaEndpointListFile,
                file.Name
            ),
            _ => Config.GetMessage(DocumentationMessage.SchemaEndpointListModule, file.Namespace.Module),
        };

        await using var md = new MarkdownWriter(OpenFileWriter(filePath, new() { EnableHeader = false }));
        md.WriteTitle(1, title);
        md.WriteLine();

        var includeAuthorization = Config.AuthorizationAnnotations.Count > 0;
        var includeModule = Config.EndpointsMode == DocumentationGenerationMode.All;
        var authorizationAnnotations = new HashSet<string>(Config.AuthorizationAnnotations, StringComparer.Ordinal);

        var table = new MarkdownTable() { Columns = GetColumns(includeModule, includeAuthorization) };
        foreach (var endpoint in endpoints.OrderBy(e => e.ModelFile.Namespace.Module.ToLower(), StringComparer.Ordinal))
        {
            var row = new List<string>();

            if (includeModule)
            {
                row.Add(endpoint.ModelFile.Namespace.Module);
            }

            row.Add(endpoint.Name);
            row.Add(endpoint.Method);
            row.Add($"/{endpoint.FullRoute}");
            row.Add(endpoint.Description);

            if (includeAuthorization)
            {
                row.Add(
                    string.Join(
                        ", ",
                        endpoint
                            .Annotations.Where(a => authorizationAnnotations.Contains(a.Annotation.Name))
                            .SelectMany(a => a.Parameters.Select(p => p.Value))
                    )
                );
            }

            table.AddRow([.. row]);
        }

        md.WriteTable(table);
    }

    private string[] GetColumns(bool includeModule, bool includeAuthorization)
    {
        var columns = new List<string>();

        if (includeModule)
        {
            columns.Add(Config.GetMessage(DocumentationMessage.EndpointHeaderModule));
        }

        columns.Add(Config.GetMessage(DocumentationMessage.EndpointHeaderName));
        columns.Add(Config.GetMessage(DocumentationMessage.EndpointHeaderMethod));
        columns.Add(Config.GetMessage(DocumentationMessage.EndpointHeaderRoute));
        columns.Add(Config.GetMessage(DocumentationMessage.EndpointHeaderDescription));

        if (includeAuthorization)
        {
            columns.Add(Config.GetMessage(DocumentationMessage.EndpointHeaderAuthorization));
        }

        return [.. columns];
    }
}
