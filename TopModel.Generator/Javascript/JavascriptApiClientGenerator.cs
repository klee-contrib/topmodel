using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class JavascriptApiClientGenerator : GeneratorBase
{
    private readonly JavascriptConfig _config;
    private readonly ILogger<JavascriptApiClientGenerator> _logger;

    public JavascriptApiClientGenerator(ILogger<JavascriptApiClientGenerator> logger, JavascriptConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "JSApiClientGen";

    public override List<string> GeneratedFiles => Files.Values.Where(f => f.Endpoints.Any())
        .SelectMany(file => _config.Tags.Intersect(file.Tags).Select(tag => _config.GetEndpointsFileName(file, tag)))
        .Distinct()
        .ToList();

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files.GroupBy(file => new { file.Options.Endpoints.FileName, file.Module }))
        {
            GenerateClientFile(file.First(), file.SelectMany(f => f.Tags).Distinct());
        }
    }

    private void GenerateClientFile(ModelFile file, IEnumerable<string> tags)
    {
        foreach (var (tag, fileName) in _config.Tags.Intersect(tags)
           .Select(tag => (tag, fileName: _config.GetEndpointsFileName(file, tag)))
           .DistinctBy(t => t.fileName))
        {
            var files = Files.Values.Where(f => f.Options.Endpoints.FileName == file.Options.Endpoints.FileName && f.Module == file.Module && f.Tags.Contains(tag));

            var endpoints = files
                .SelectMany(f => f.Endpoints)
                .OrderBy(e => e.Name, StringComparer.Ordinal)
                .ToList();

            if (!endpoints.Any())
            {
                continue;
            }

            var fetch = _config.FetchImportPath != "@focus4/core" ? "fetch" : "coreFetch";
            var fetchImport = _config.FetchImportPath.StartsWith("@")
                ? _config.FetchImportPath
                : Path.GetRelativePath(string.Join('/', fileName.Split('/').SkipLast(1)), Path.Combine(_config.OutputDirectory, _config.ApiClientRootPath!.Replace("{tag}", tag.ToKebabCase()), _config.FetchImportPath)).Replace("\\", "/");

            using var fw = new FileWriter(fileName, _logger, false);

            fw.WriteLine($@"import {{{fetch}}} from ""{fetchImport}"";");

            var imports = _config.GetEndpointImports(files, tag, Classes);
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

                foreach (var param in endpoint.Params)
                {
                    fw.WriteLine($" * @param {param.GetParamName()} {param.Comment}");
                }

                fw.WriteLine(" * @param options Options pour 'fetch'.");

                if (endpoint.Returns != null)
                {
                    fw.WriteLine($" * @returns {endpoint.Returns.Comment}");
                }

                fw.WriteLine(" */");
                fw.Write($"export function {endpoint.Name.ToFirstLower()}(");

                var hasForm = endpoint.Params.Any(p => p is IFieldProperty fp && fp.Domain.TS!.Type.Contains("File"));

                foreach (var param in endpoint.Params)
                {
                    var defaultValue = _config.GetDefaultValue(param);
                    fw.Write($"{param.GetParamName()}{(param.IsQueryParam() && !hasForm && defaultValue == "undefined" ? "?" : string.Empty)}: {param.GetPropertyTypeName(Classes)}{(defaultValue != "undefined" ? $" = {defaultValue}" : string.Empty)}, ");
                }

                fw.Write("options: RequestInit = {}): Promise<");
                if (endpoint.Returns == null)
                {
                    fw.Write("void");
                }
                else
                {
                    fw.Write(endpoint.Returns.GetPropertyTypeName(Classes));
                }

                fw.WriteLine("> {");

                if (hasForm)
                {
                    fw.WriteLine("    const body = new FormData();");
                    fw.WriteLine("    fillFormData(");
                    fw.WriteLine("        {");
                    foreach (var param in endpoint.Params)
                    {
                        if (param is IFieldProperty)
                        {
                            fw.Write($@"            {param.GetParamName()}");
                        }
                        else
                        {
                            fw.Write($@"            ...{param.GetParamName()}");
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

                    fw.WriteLine("        },");
                    fw.WriteLine("        body");
                    fw.WriteLine("    );");

                    fw.WriteLine($@"    return {fetch}(""{endpoint.Method}"", `./{endpoint.FullRoute.Replace("{", "${")}`, {{body}}, options);");
                    fw.WriteLine("}");
                    continue;
                }

                fw.Write($@"    return {fetch}(""{endpoint.Method}"", `./{endpoint.FullRoute.Replace("{", "${")}`, {{");

                if (endpoint.GetBodyParam() != null)
                {
                    fw.Write($"body: {endpoint.GetBodyParam()!.GetParamName()}");
                }

                if (endpoint.GetBodyParam() != null && endpoint.GetQueryParams().Any())
                {
                    fw.Write(", ");
                }

                if (endpoint.GetQueryParams().Any())
                {
                    fw.Write("query: {");

                    foreach (var qParam in endpoint.GetQueryParams())
                    {
                        fw.Write(qParam.GetParamName());

                        if (qParam != endpoint.GetQueryParams().Last())
                        {
                            fw.Write(", ");
                        }
                    }

                    fw.Write("}");
                }

                fw.WriteLine("}, options);");
                fw.WriteLine("}");
            }

            if (endpoints.Any(endpoint => endpoint.Params.Any(p => p is IFieldProperty fp && fp.Domain.TS!.Type.Contains("File"))))
            {
                fw.WriteLine(@"
function fillFormData(data: any, formData: FormData, prefix = """") {
    if (Array.isArray(data)) {
        data.forEach((item, i) => fillFormData(item, formData, prefix + (typeof item === ""object"" && !(item instanceof File) ? `[${i}]` : """")));
    } else if (typeof data === ""object"" && !(data instanceof File)) {
        for (const key in data) {
            fillFormData(data[key], formData, (prefix ? `${prefix}.` : """") + key);
        }
    } else {
        formData.append(prefix, data);
    }
}");
            }
        }
    }
}