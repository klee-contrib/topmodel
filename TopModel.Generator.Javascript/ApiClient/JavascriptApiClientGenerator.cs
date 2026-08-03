using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Generator.Javascript.ApiClient;

/// <summary>
/// Générateur de clients d'API JavaScript basés sur fetch.
/// </summary>
public class JavascriptApiClientGenerator(
    ILogger<JavascriptApiClientGenerator> logger,
    IFileWriterProvider writerProvider
) : BaseApiClientGenerator(logger, writerProvider)
{
    public override string Name => "JSApiClientGen";

    protected override IEnumerable<(string Import, string Path)> GetFrameworkImports(
        string filePath,
        string tag,
        IList<Endpoint> endpoints
    )
    {
        if (Config.FetchPath != null)
        {
            var fetchImport =
                Config.FetchPath.StartsWith('@') || !Config.FetchPath.StartsWith('.')
                    ? Config.ResolveVariables(Config.FetchPath, tag)
                    : Path.GetRelativePath(
                            string.Join('/', filePath.Split('/').SkipLast(1)),
                            Path.Combine(Config.OutputDirectory, Config.ResolveVariables(Config.FetchPath, tag))
                        )
                        .Replace('\\', '/');

            yield return ("*fetch", fetchImport);
        }
    }

    protected override void WriteEndpoint(IFileWriter fw, Endpoint endpoint)
    {
        var options = GetSafeVariableName(endpoint, "options");
        WriteEndpointSignature(fw, endpoint, (options, "Options pour 'fetch'.", "RequestInit", "{}"));

        var query = GetSafeVariableName(endpoint, "query");
        var body = GetSafeVariableName(endpoint, "body");
        var response = GetSafeVariableName(endpoint, "response");

        if (endpoint.GetQueryParams(Config).Any())
        {
            fw.WriteLine(1, $"const {query} = new URLSearchParams();");

            foreach (var qParam in endpoint.GetQueryParams(Config))
            {
                var name = qParam.GetParamName();
                fw.WriteLine(1, $"if ({name} !== undefined) {{");
                var isArray = Config.GetType(qParam).EndsWith("[]");
                var isString =
                    Config.GetImplementation(qParam.Domain)?.Type?.Value.TrimEnd(']').TrimEnd('[') == "string";

                void Append(int indent, string value)
                {
                    fw.WriteLine(indent, $"{query}.append(\"{name}\", {(isString ? value : $"`${{{value}}}`")})");
                }

                if (isArray)
                {
                    var item = GetSafeVariableName(endpoint, "item");
                    fw.WriteLine(2, $"for (const {item} of {name}) {{");
                    Append(3, item);
                    fw.WriteLine(2, "}");
                }
                else
                {
                    Append(2, name);
                }

                fw.WriteLine(1, "}");
            }
        }

        WriteFormDataBody(fw, endpoint, body);

        var returns = GetReturns(endpoint);
        var returnType = GetReturnType(returns);
        var hasReturns = HasReturns(returnType);

        fw.WriteLine(
            1,
            $@"{(hasReturns ? $"const {response} = " : string.Empty)}await fetch(`./{endpoint.FullRoute.Replace("{", "${")}{(endpoint.GetQueryParams(Config).Any() ? $"?${{{query}}}" : string.Empty)}`, {{"
        );
        fw.WriteLine(2, $"...{options},");
        fw.Write(2, $"method: \"{endpoint.Method}\"");

        if (endpoint.GetJsonBodyParam(Config) != null)
        {
            fw.WriteLine(",");
            fw.WriteLine(2, $"body: JSON.stringify({endpoint.GetJsonBodyParam(Config)!.GetParamName()}),");
            fw.WriteLine(2, $"headers: {{...{options}.headers, \"Content-Type\": \"application/json\"}}");
        }
        else if (endpoint.IsMultipart)
        {
            fw.WriteLine(",");
            fw.WriteLine(2, body == "body" ? body : $"body: {body}");
        }
        else
        {
            fw.WriteLine();
        }

        fw.WriteLine(1, "});");
        if (hasReturns)
        {
            if (!returns!.Required)
            {
                fw.WriteLine(1, $"if ({response}.status === 204) {{");
                fw.WriteLine(2, $"return undefined;");
                fw.WriteLine(1, "}");
            }

            if (returnType == "Response")
            {
                fw.WriteLine(1, $"return {response};");
            }
            else
            {
                var domainType = Config.GetImplementation(returns.Domain)?.Type;

                if (domainType == "string")
                {
                    fw.WriteLine(1, $"if ({response}.headers.get(\"Content-Type\")?.includes(\"text/plain\")) {{");
                    fw.WriteLine(
                        2,
                        $"return await {response}.text(){(returnType != domainType ? $" as {returnType}" : string.Empty)};"
                    );
                    fw.WriteLine(1, "}");
                }

                fw.WriteLine(1, $"return await {response}.{(returnType == "Blob" ? "blob" : "json")}();");
            }
        }

        fw.WriteLine("}");
    }
}
