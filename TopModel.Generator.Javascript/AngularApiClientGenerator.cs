using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class AngularApiClientGenerator : EndpointsGeneratorBase<JavascriptConfig>
{
    private readonly ILogger<AngularApiClientGenerator> _logger;

    public AngularApiClientGenerator(ILogger<AngularApiClientGenerator> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override string Name => "JSNGApiClientGen";

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Config.GetEndpointsFileName(file, tag);
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        using var fw = new FileWriter(filePath, _logger, false);
        var imports = Config.GetEndpointImports(filePath, endpoints, tag, Classes);

        imports.AddRange(new List<(string Import, string Path)>
        {
            (Import: "Injectable", Path: "@angular/core"),
            (Import: "HttpClient", Path: "@angular/common/http"),
            (Import: "Observable", Path: "rxjs"),
        });

        if (endpoints.Any(e => e.GetQueryParams().Any()))
        {
            imports.Add((Import: "HttpParams", Path: "@angular/common/http"));
        }

        imports = imports.GroupAndSort();

        if (imports.Any())
        {
            fw.WriteLine();

            foreach (var (import, path) in imports)
            {
                fw.WriteLine($@"import {{{import}}} from ""{path}"";");
            }
        }

        fw.WriteLine("@Injectable({");
        fw.WriteLine(1, "providedIn: 'root'");
        fw.WriteLine("})");

        fw.WriteLine(@$"export class {fileName.ToPascalCase()}Service {{");
        fw.WriteLine();
        fw.WriteLine(1, "constructor(private http: HttpClient) {}");
        foreach (var endpoint in endpoints)
        {
            WriteEndpoint(endpoint, fw);
        }

        if (endpoints.Any(e => e.IsMultipart))
        {
            fw.WriteLine(@"
    private fillFormData(data: any, formData: FormData, prefix = """") {
        if (Array.isArray(data)) {
            data.forEach((item, i) => this.fillFormData(item, formData, prefix + (typeof item === ""object"" && !(item instanceof File) ? `[${i}]` : """")));
        } else if (typeof data === ""object"" && !(data instanceof File)) {
            for (const key in data) {
                this.fillFormData(data[key], formData, (prefix ? `${prefix}.` : """") + key);
            }
        } else {
            formData.append(prefix, data);
        }
    }");
        }

        fw.WriteLine("}");
    }

    private void WriteEndpoint(Endpoint endpoint, FileWriter fw)
    {
        fw.WriteLine();
        fw.WriteLine(1, "/**");
        fw.WriteLine(1, $" * @description {endpoint.Description}");

        foreach (var param in endpoint.Params)
        {
            fw.WriteLine(1, $" * @param {param.GetParamName()} {param.Comment}");
        }

        if (endpoint.Returns != null)
        {
            fw.WriteLine(1, $" * @returns {endpoint.Returns.Comment}");
        }

        fw.WriteLine(1, " */");
        fw.Write(1, $"{endpoint.NameCamel}(");

        var hasProperty = false;
        foreach (var param in endpoint.Params)
        {
            if (hasProperty)
            {
                fw.Write(", ");
            }

            hasProperty = true;
            var defaultValue = Config.GetValue(param, Classes);
            fw.Write($"{param.GetParamName()}{(param.IsQueryParam() && !endpoint.IsMultipart && defaultValue == "undefined" ? "?" : string.Empty)}: {Config.GetType(param, Classes)}{(defaultValue != "undefined" ? $" = {defaultValue}" : string.Empty)}");
        }

        if (endpoint.GetQueryParams().Any() && !endpoint.IsMultipart)
        {
            if (hasProperty)
            {
                fw.Write(", ");
            }

            fw.Write("queryParams: any = {}");
        }

        fw.Write("): Observable<");

        string returnType;
        if (endpoint.Returns == null)
        {
            returnType = "void";
        }
        else
        {
            returnType = Config.GetType(endpoint.Returns, Classes);
        }

        fw.Write(returnType);
        fw.WriteLine("> {");

        if (endpoint.IsMultipart)
        {
            fw.WriteLine("    const formData = new FormData();");
            fw.WriteLine("    this.fillFormData(");
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
            fw.WriteLine("        formData");
            fw.WriteLine("    );");

            fw.WriteLine(2, $@"return this.http.{endpoint.Method.ToLower()}<{returnType}>(`/{endpoint.FullRoute}`, formData);");
            fw.WriteLine("}");
            return;
        }

        if (endpoint.GetQueryParams().Any())
        {
            foreach (var qParam in endpoint.GetQueryParams())
            {
                fw.WriteLine(2, @$"if({qParam.GetParamName()}) {{");
                fw.WriteLine(3, $"queryParams['{qParam.GetParamName()}'] = {qParam.GetParamName()}");
                fw.WriteLine(2, @$"}}");
                fw.WriteLine();
            }

            fw.WriteLine(2, "const httpParams = new HttpParams({fromObject : queryParams});");
            fw.WriteLine(2, "const httpOptions = { params: httpParams }");

            fw.WriteLine();
        }

        if (returnType == "string")
        {
            fw.Write(2, $@"return this.http.{endpoint.Method.ToLower()}(`/{endpoint.FullRoute.Replace("{", "${")}`");
        }
        else
        {
            fw.Write(2, $@"return this.http.{endpoint.Method.ToLower()}<{returnType}>(`/{endpoint.FullRoute.Replace("{", "${")}`");
        }

        if (endpoint.GetJsonBodyParam() != null)
        {
            fw.Write($", {endpoint.GetJsonBodyParam()!.GetParamName()}");
        }
        else if (endpoint.Method != "OPTIONS" && endpoint.Method != "GET" && endpoint.Method != "DELETE")
        {
            fw.Write(", {}");
        }

        if (returnType == "string")
        {
            fw.Write($", {{ responseType: 'text' }}");
        }

        if (endpoint.GetQueryParams().Any())
        {
            fw.Write(", httpOptions");
        }

        fw.WriteLine(");");
        fw.WriteLine(1, "}");
    }
}