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
public class LegacyApiClientGenerator(ILogger<LegacyApiClientGenerator> logger, IFileWriterProvider writerProvider)
    : EndpointsGeneratorBase<JavascriptConfig>(logger, writerProvider)
{
    public override string Name => "JSLApiClientGen";

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Config.GetEndpointsFileName(file, tag);
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        var fetchPath = Config.FetchPath ?? "@focus4/core";

        var fetch = fetchPath != "@focus4/core" ? "fetch" : "coreFetch";
        var fetchImport =
            fetchPath.StartsWith('@') || !fetchPath.StartsWith('.')
                ? Config.ResolveVariables(fetchPath, tag)
                : Path.GetRelativePath(
                        string.Join('/', filePath.Split('/').SkipLast(1)),
                        Path.Combine(Config.OutputDirectory, Config.ResolveVariables(fetchPath, tag))
                    )
                    .Replace('\\', '/');

        using var fw = OpenFileWriter(filePath, encoderShouldEmitUTF8Identifier: false);

        fw.WriteLine($@"import {{{fetch}}} from ""{fetchImport}"";");

        var imports = Config.GetEndpointImports(filePath, endpoints, tag);
        if (imports.Any())
        {
            fw.WriteLine();

            foreach (var (import, path) in imports)
            {
                fw.WriteLine($@"import {{{import}}} from ""{path}"";");
            }
        }

        foreach (var endpoint in endpoints)
        {
            fw.WriteLine();
            fw.WriteLine("/**");
            fw.WriteLine($" * {endpoint.Description}");

            foreach (var param in Config.GetParams(endpoint))
            {
                fw.WriteLine($" * @param {param.GetParamName()} {param.Comment}");
            }

            fw.WriteLine(" * @param options Options pour 'fetch'.");

            var returns = Config.GetReturns(endpoint);

            if (returns != null)
            {
                fw.WriteLine($" * @returns {returns.Comment}");
            }

            fw.WriteLine(" */");
            fw.Write($"export function {endpoint.NameCamel}(");

            foreach (var param in Config.GetParams(endpoint))
            {
                var defaultValue = Config.GetValue(param);
                fw.Write(
                    $"{param.GetParamName()}{(param.IsQueryParam(Config) && !endpoint.IsMultipart && defaultValue == "undefined" ? "?" : string.Empty)}: {Config.GetType(param)}{(defaultValue != "undefined" ? $" = {defaultValue}" : string.Empty)}, "
                );
            }

            fw.Write("options: RequestInit = {}): Promise<");
            if (returns == null)
            {
                fw.Write("void");
            }
            else
            {
                fw.Write(Config.GetType(returns));
            }

            fw.WriteLine("> {");

            if (endpoint.IsMultipart)
            {
                fw.WriteLine(1, "const body = new FormData();");
                fw.WriteLine(1, "fillFormData(");
                fw.WriteLine(2, "{");

                foreach (
                    var param in Config.GetParams(endpoint).Where(p => !p.IsRouteParam() && !p.IsQueryParam(Config))
                )
                {
                    if (param is not { Composition: not null })
                    {
                        fw.Write(3, $@"{param.GetParamName()}");
                    }
                    else
                    {
                        fw.Write(3, $@"...{param.GetParamName()}");
                    }

                    if (Config.GetParams(endpoint).Last() != param)
                    {
                        fw.WriteLine(",");
                    }
                    else
                    {
                        fw.WriteLine();
                    }
                }

                fw.WriteLine(2, "},");
                fw.WriteLine(2, "body");
                fw.WriteLine(1, ");");
            }

            fw.Write(1, $@"return {fetch}(""{endpoint.Method}"", `./{endpoint.FullRoute.Replace("{", "${")}`, {{");

            if (endpoint.GetJsonBodyParam(Config) != null)
            {
                fw.Write($"body: {endpoint.GetJsonBodyParam(Config)!.GetParamName()}");
            }
            else if (endpoint.IsMultipart)
            {
                fw.Write("body");
            }

            if (
                (endpoint.GetJsonBodyParam(Config) != null || endpoint.IsMultipart)
                && endpoint.GetQueryParams(Config).Any()
            )
            {
                fw.Write(", ");
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

            fw.WriteLine("}, options);");
            fw.WriteLine("}");
        }

        if (
            endpoints.Any(endpoint =>
                Config
                    .GetParams(endpoint)
                    .Any(p => p is not { Composition: not null } && Config.GetType(p).Contains("File"))
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
