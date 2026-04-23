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

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        if (endpoints.Count == 0)
        {
            return;
        }

        var file = endpoints[0].ModelFile;
        var title = Config.EndpointsMode switch
        {
            DocumentationGenerationMode.All => "Liste des endpoints",
            DocumentationGenerationMode.File => $"Liste des endpoints du fichier {file.Name}",
            _ => $"Liste des endpoints du module {file.Namespace.Module}",
        };

        using var md = new MarkdownWriter(OpenFileWriter(filePath));
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

    private static string[] GetColumns(bool includeModule, bool includeAuthorization)
    {
        var columns = new List<string>();

        if (includeModule)
        {
            columns.Add("Module");
        }

        columns.Add("Nom");
        columns.Add("Méthode");
        columns.Add("Route");
        columns.Add("Description");

        if (includeAuthorization)
        {
            columns.Add("Autorisation");
        }

        return [.. columns];
    }
}
