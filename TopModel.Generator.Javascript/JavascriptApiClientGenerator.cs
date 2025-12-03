using Microsoft.Extensions.Logging;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class JavascriptApiClientGenerator(
    ILogger<JavascriptApiClientGenerator> logger,
    IFileWriterProvider writerProvider
) : EndpointsGeneratorBase<JavascriptConfig>(logger, writerProvider)
{
    public override string Name => "JSApiClientGen";

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Config.GetEndpointsFileName(file, tag);
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        using var fw = OpenFileWriter(filePath, encoderShouldEmitUTF8Identifier: false);

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

            fw.WriteLine($@"import fetch from ""{fetchImport}"";");
        }

        var imports = Config.GetEndpointImports(filePath, endpoints, tag);
        if (imports.Any())
        {
            if (Config.FetchPath != null)
            {
                fw.WriteLine();
            }

            foreach (var (import, path) in imports)
            {
                fw.WriteLine($@"import {{{import}}} from ""{path}"";");
            }
        }

        foreach (var endpoint in endpoints)
        {
            string GetSafeVariableName(string varName)
            {
                while (endpoint.Params.Any(p => p.NameCamel == varName))
                {
                    varName = $"_{varName}";
                }

                return varName;
            }

            var options = GetSafeVariableName("options");
            var query = GetSafeVariableName("query");
            var body = GetSafeVariableName("body");
            var response = GetSafeVariableName("response");

            fw.WriteLine();
            fw.WriteLine("/**");
            fw.WriteLine($" * {endpoint.Description}");

            foreach (var param in endpoint.Params)
            {
                fw.WriteLine($" * @param {param.GetParamName()} {param.Comment}");
            }

            fw.WriteLine($" * @param {options} Options pour 'fetch'.");

            if (endpoint.Returns != null)
            {
                fw.WriteLine($" * @returns {endpoint.Returns.Comment}");
            }

            fw.WriteLine(" */");
            fw.Write($"export async function {endpoint.NameCamel}(");

            foreach (var param in endpoint.Params)
            {
                var defaultValue = Config.GetValue(param);
                fw.Write(
                    $"{param.GetParamName()}{(param.IsQueryParam() && !endpoint.IsMultipart && defaultValue == "undefined" ? "?" : string.Empty)}: {Config.GetType(param)}{(defaultValue != "undefined" ? $" = {defaultValue}" : string.Empty)}, "
                );
            }

            fw.Write($"{options}: RequestInit = {{}}): Promise<");
            if (endpoint.Returns == null)
            {
                fw.Write("void");
            }
            else
            {
                fw.Write(Config.GetType(endpoint.Returns));
                if (!endpoint.Returns.Required)
                {
                    fw.Write(" | undefined");
                }
            }

            fw.WriteLine("> {");

            if (endpoint.GetQueryParams().Any())
            {
                fw.WriteLine(1, $"const {query} = new URLSearchParams();");

                foreach (var qParam in endpoint.GetQueryParams())
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
                        var item = GetSafeVariableName("item");
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

            if (endpoint.IsMultipart)
            {
                fw.WriteLine(1, $"const {body} = new FormData();");
                fw.WriteLine(1, "fillFormData(");
                fw.WriteLine(2, "{");

                foreach (var param in endpoint.Params.Where(p => !p.IsRouteParam() && !p.IsQueryParam()))
                {
                    if (param is not CompositionProperty and not AliasProperty { Property: CompositionProperty })
                    {
                        fw.Write(3, $@"{param.GetParamName()}");
                    }
                    else
                    {
                        fw.Write(3, $@"...{param.GetParamName()}");
                    }

                    if (endpoint.Params.IndexOf(param) < endpoint.Params.Count - 1)
                    {
                        fw.WriteLine(",");
                    }
                    else
                    {
                        fw.WriteLine();
                    }
                }

                fw.WriteLine(2, "},");
                fw.WriteLine(2, body);
                fw.WriteLine(1, ");");
            }

            fw.WriteLine(
                1,
                $@"{(endpoint.Returns != null ? $"const {response} = " : string.Empty)}await fetch(`./{endpoint.FullRoute.Replace("{", "${")}{(endpoint.GetQueryParams().Any() ? $"?${{{query}}}" : string.Empty)}`, {{"
            );
            fw.WriteLine(2, $"...{options},");
            fw.Write(2, $"method: \"{endpoint.Method}\"");

            if (endpoint.GetJsonBodyParam() != null)
            {
                fw.WriteLine(",");
                fw.WriteLine(2, $"body: JSON.stringify({endpoint.GetJsonBodyParam()!.GetParamName()}),");
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
            if (endpoint.Returns != null)
            {
                if (!endpoint.Returns.Required)
                {
                    fw.WriteLine(1, $"if ({response}.status === 204) {{");
                    fw.WriteLine(2, $"return undefined;");
                    fw.WriteLine(1, "}");
                }

                var type =
                    Config.GetType(endpoint.Returns) == "Blob" ? "blob"
                    : endpoint.Returns is CompositionProperty or { Domain.BodyParam: true } ? "json"
                    : "text";

                var domainType = Config.GetImplementation(endpoint.Returns.Domain)?.Type;
                fw.WriteLine(
                    1,
                    $"return {(domainType == "number" ? "+" : string.Empty)}await {response}.{type}(){(domainType == "boolean" ? " === \"true\"" : string.Empty)};"
                );
            }
            fw.WriteLine("}");
        }

        if (
            endpoints.Any(endpoint =>
                endpoint.Params.Any(p =>
                    p is not CompositionProperty and not AliasProperty { Property: CompositionProperty }
                    && Config.GetType(p).Contains("File")
                )
            )
        )
        {
            fw.WriteLine(
                @"
function fillFormData(data: any, formData: FormData, prefix = """") {
    if (Array.isArray(data)) {
        for (const [i, item] of data.entries()) {
            fillFormData(item, formData, prefix + (typeof item === ""object"" && !(item instanceof File) ? `[${i}]` : """"));
        }
    } else if (typeof data === ""object"" && !(data instanceof File)) {
        for (const key in data) {
            fillFormData(data[key], formData, (prefix ? `${prefix}.` : """") + key);
        }
    } else {
        formData.append(prefix, data);
    }
}"
            );
        }
    }
}
