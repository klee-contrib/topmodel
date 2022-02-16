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

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            GenerateClientFile(file);
        }
    }

    private void GenerateClientFile(ModelFile file)
    {
        if (!file.Endpoints.Any())
        {
            return;
        }

        var fileSplit = file.Name.Split("/");
        var modulePath = string.Join('/', file.Module.Split('.').Select(m => m.ToDashCase()));
        var filePath = _config.ApiClientFilePath.Replace("{module}", modulePath) + '/' + string.Join('_', fileSplit.Last().Split("_").Skip(fileSplit.Last().Contains('_') ? 1 : 0)).ToDashCase();
        var fileName = $"{_config.ApiClientOutputDirectory}/{filePath}.ts";

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

            var hasForm = endpoint.Params.Any(p => p is IFieldProperty { Domain.TS.Type: "File" });

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
                fw.Write("    const body = new FormData();\r\n");
                foreach (var param in endpoint.Params)
                {
                    if (param is IFieldProperty { Domain.TS.Type: "File" })
                    {
                        fw.Write($@"    body.append(""{param.Name}"", {param.Name});");
                    }
                    else if (param is CompositionProperty)
                    {
                        fw.Write($@"    for (const key in {param.Name}) {{
        body.append(key, ({param.Name} as any)[key]);
    }}");
                    }
                    else
                    {
                        fw.Write($@"    body.append(""{param.GetParamName()}"", {param.GetParamName()}?.toString());");
                    }

                    fw.WriteLine();
                }

                fw.WriteLine($@"    return {fetch}(""{endpoint.Method}"", `./{endpoint.Route.Replace("{", "${")}`, {{body}}, options);");
                fw.WriteLine("}");
                continue;
            }

            fw.Write($@"    return {fetch}(""{endpoint.Method}"", `./{endpoint.Route.Replace("{", "${")}`, {{");

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
    }

    private IList<(string Import, string Path)> GetImports(ModelFile file, string relativePath)
    {
        var properties = file.Endpoints
            .SelectMany(endpoint => endpoint.Params.Concat(new[] { endpoint.Returns }))
            .Where(p => p != null) as IEnumerable<IProperty>;

        var types = properties.OfType<CompositionProperty>().Select(property => property.Composition);

        var modelPath = Path.GetRelativePath(_config.ApiClientOutputDirectory!, _config.ModelOutputDirectory!).Replace("\\", "/");

        var imports = types.Select(type =>
        {
            var name = type.Name.Value;
            var module = $"{modelPath}/{type.Namespace.Module.Replace(".", "/").ToDashCase()}";
            return (Import: name, Path: $"{relativePath}{module}/{name.ToDashCase()}");
        }).Distinct().ToList();

        var references = JavascriptUtils.GetReferencesToImport(properties);

        if (references.Any())
        {
            var referenceTypeMap = references.GroupBy(t => t.Module);
            foreach (var refModule in referenceTypeMap)
            {
                var module = $"{modelPath}/{refModule.Key.Replace('.', '/').ToLower()}";
                imports.Add((string.Join(", ", refModule.Select(r => r.Code).OrderBy(x => x)), $"{relativePath}{module}/references"));
            }
        }

        imports.AddRange(JavascriptUtils.GetPropertyImports(properties));
        return JavascriptUtils.GroupAndSortImports(imports);
    }
}