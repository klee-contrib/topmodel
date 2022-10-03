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

    public override List<string> GeneratedFiles => Files.Values.Where(f => f.Endpoints.Any()).Select(GetFileName).ToList();

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            GenerateClientFile(file);
        }
    }

    private string GetFileName(ModelFile file)
    {
        var fileSplit = file.Name.Split("/");
        var modulePath = Path.Combine(file.Module.Split('.').Select(m => m.ToDashCase()).ToArray());
        var filePath = _config.ApiClientFilePath.Replace("{module}", modulePath);
        var fileName = string.Join('_', fileSplit.Last().Split("_").Skip(fileSplit.Last().Contains('_') ? 1 : 0)).ToDashCase();

        if (file.Options?.Endpoints?.FileName != null)
        {
            fileName = file.Options?.Endpoints?.FileName.ToDashCase();
        }

        return Path.Combine(_config.OutputDirectory, _config.ApiClientRootPath!, filePath, $"{fileName}.ts");
    }

    private void GenerateClientFile(ModelFile file)
    {
        if (!file.Endpoints.Any())
        {
            return;
        }

        var modulePath = string.Join('/', file.Module.Split('.').Select(m => m.ToDashCase()));
        var fileName = GetFileName(file);

        var relativePath = _config.ApiClientFilePath.Length > 0 ? string.Join(string.Empty, _config.ApiClientFilePath.Replace("{module}", modulePath).Split('/').Select(s => "../")) : string.Empty;
        var fetch = _config.FetchImportPath != null ? "fetch" : "coreFetch";

        using var fw = new FileWriter(fileName, _logger, false);
        fw.WriteLine($@"import {{{fetch}}} from ""{((_config.FetchImportPath == null || _config.FetchImportPath.StartsWith('@')) ? string.Empty : relativePath)}{_config.FetchImportPath ?? "@focus4/core"}"";");

        var imports = GetImports(file, relativePath);
        if (imports.Any())
        {
            fw.WriteLine();

            foreach (var (import, path) in imports)
            {
                fw.WriteLine($@"import {{{import}}} from ""{path}"";");
            }
        }

        foreach (var endpoint in file.Endpoints)
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
                fw.Write($"{param.GetParamName()}{(param.IsQueryParam() && !hasForm ? "?" : string.Empty)}: {param.GetPropertyTypeName()}, ");
            }

            fw.Write("options: RequestInit = {}): Promise<");
            if (endpoint.Returns == null)
            {
                fw.Write("void");
            }
            else
            {
                fw.Write(endpoint.Returns.GetPropertyTypeName());
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

        if (file.Endpoints.Any(endpoint => endpoint.Params.Any(p => p is IFieldProperty fp && fp.Domain.TS!.Type.Contains("File"))))
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

    private IList<(string Import, string Path)> GetImports(ModelFile file, string relativePath)
    {
        var properties = file.Endpoints
            .SelectMany(endpoint => endpoint.Params.Concat(new[] { endpoint.Returns }))
            .Where(p => p != null) as IEnumerable<IProperty>;

        var types = properties.OfType<CompositionProperty>().Select(property => property.Composition);

        var modelPath = Path.GetRelativePath(_config.ApiClientRootPath!, _config.ModelRootPath!).Replace("\\", "/");

        var imports = types.Select(type =>
        {
            var name = type.Name.Value;
            var module = $"{modelPath}/{string.Join('/', type.Namespace.Module.Split('.').Select(m => m.ToDashCase()))}";
            return (Import: name, Path: $"{relativePath}{module}/{name.ToDashCase()}");
        }).Distinct().ToList();

        imports.AddRange(JavascriptUtils.GetImportsForProperties(properties, string.Empty).Select(i => (i.Import, Path: $"{i.Path.Replace("../", $"{relativePath}{modelPath}/")}")));
        return imports.GroupAndSort();
    }
}