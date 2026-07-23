using Microsoft.Extensions.Logging;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

/// <summary>
/// Générateur de clients d'API Nuxt.
/// </summary>
public class NuxtApiClientGenerator(ILogger<NuxtApiClientGenerator> logger, IFileWriterProvider writerProvider)
    : EndpointsGeneratorBase<JavascriptConfig>(logger, writerProvider)
{
    public override string Name => "JSApiClientGen";

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Config.GetEndpointsFileName(file, tag);
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        using var fw = OpenFileWriter(filePath, encoderShouldEmitUTF8Identifier: false);

        fw.WriteLine($@"import {{AsyncData, AsyncDataOptions}} from ""nuxt/app"";");

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

            var fetchReturnType = returns == null ? "void" : Config.GetType(returns);
            fw.WriteLine(
                $"options: AsyncDataOptions<{fetchReturnType}> = {{}}): AsyncData<{fetchReturnType} | null, Error | null> {{"
            );

            if (endpoint.IsMultipart)
            {
                fw.WriteLine("    const body = new FormData();");
                fw.WriteLine("    fillFormData(");
                fw.WriteLine("        {");

                foreach (
                    var param in Config.GetParams(endpoint).Where(p => !p.IsRouteParam() && !p.IsQueryParam(Config))
                )
                {
                    if (param is not { Composition: not null })
                    {
                        fw.Write($@"            {param.GetParamName()}");
                    }
                    else
                    {
                        fw.Write($@"            ...{param.GetParamName()}");
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

                fw.WriteLine("        },");
                fw.WriteLine("        body");
                fw.WriteLine("    );");
            }

            var fetchRoute = $@"`/{endpoint.FullRoute.Replace("{", "${")}`";

            fw.WriteLine(1, $@"return useAsyncData({fetchRoute}, () => ");
            fw.WriteLine(2, $@"$fetch<{fetchReturnType}>({fetchRoute}, {{");
            fw.WriteLine(3, $@"method: '{endpoint.Method}',");

            if (endpoint.IsMultipart)
            {
                fw.WriteLine(3, "body,");
            }
            else if (endpoint.GetJsonBodyParam(Config) != null)
            {
                fw.WriteLine(3, $@"body: {endpoint.GetJsonBodyParam(Config)!.GetParamName()},");
            }

            if (endpoint.GetQueryParams(Config).Any())
            {
                fw.WriteLine(4, "query: {");

                foreach (var qParam in endpoint.GetQueryParams(Config))
                {
                    fw.WriteLine(5, $@"{qParam.GetParamName()},");
                }

                fw.WriteLine(4, "}");
            }

            fw.WriteLine(2, "}), options);");
            fw.WriteLine("}");
        }

        if (endpoints.Any(endpoint => endpoint.IsMultipart))
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
