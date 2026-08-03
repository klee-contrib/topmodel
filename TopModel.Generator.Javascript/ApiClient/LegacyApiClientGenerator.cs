using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Generator.Javascript.ApiClient;

/// <summary>
/// Générateur de clients d'API JavaScript legacy.
/// </summary>
public class LegacyApiClientGenerator(ILogger<LegacyApiClientGenerator> logger, IFileWriterProvider writerProvider)
    : BaseApiClientGenerator(logger, writerProvider)
{
    public override string Name => "JSLApiClientGen";

    protected override string EndpointDefinitionPrefix => "export function ";

    protected override bool UndefinedInReturnType => false;

    private string FetchPath => Config.FetchPath ?? "@focus4/core";

    private string Fetch => FetchPath != "@focus4/core" ? "fetch" : "coreFetch";

    protected override IEnumerable<(string Import, string Path)> GetFrameworkImports(
        string filePath,
        string tag,
        IList<Endpoint> endpoints
    )
    {
        yield return (
            Fetch,
            FetchPath.StartsWith('@') || !FetchPath.StartsWith('.')
                ? Config.ResolveVariables(FetchPath, tag)
                : Path.GetRelativePath(
                        string.Join('/', filePath.Split('/').SkipLast(1)),
                        Path.Combine(Config.OutputDirectory, Config.ResolveVariables(FetchPath, tag))
                    )
                    .Replace('\\', '/')
        );
    }

    protected override void WriteEndpoint(IFileWriter fw, Endpoint endpoint)
    {
        var options = GetSafeVariableName(endpoint, "options");

        WriteEndpointSignature(fw, endpoint, (options, "Options pour 'fetch'.", "RequestInit", "{}"));

        var body = GetSafeVariableName(endpoint, "body");

        WriteFormDataBody(fw, endpoint, body);

        fw.Write(1, $@"return {Fetch}(""{endpoint.Method}"", `./{endpoint.FullRoute.Replace("{", "${")}`, {{");

        var bodyParam = endpoint.GetJsonBodyParam(Config)?.GetParamName() ?? (endpoint.IsMultipart ? body : null);

        if (bodyParam != null)
        {
            fw.Write(bodyParam == "body" ? "body" : $"body: {bodyParam}");

            if (endpoint.GetQueryParams(Config).Any())
            {
                fw.Write(", ");
            }
        }

        if (endpoint.GetQueryParams(Config).Any())
        {
            fw.Write("query: {");

            foreach (var qParam in endpoint.GetQueryParams(Config))
            {
                fw.Write(qParam.GetParamName());

                if (qParam != endpoint.GetQueryParams(Config).Last())
                {
                    fw.Write(", ");
                }
            }

            fw.Write("}");
        }

        fw.WriteLine($"}}, {options});");
        fw.WriteLine("}");
    }
}
