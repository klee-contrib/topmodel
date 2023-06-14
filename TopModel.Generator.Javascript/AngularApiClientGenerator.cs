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
        var imports = Config.GetEndpointImports(endpoints, tag, Classes, GetClassTags);

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

        fw.WriteLine("}");
    }

    private void WriteEndpoint(Endpoint endpoint, FileWriter fw)
    {
        fw.WriteLine();
        fw.WriteLine(1, "/**");
        fw.WriteLine(1, $" * {endpoint.Description}");

        foreach (var param in endpoint.Params)
        {
            fw.WriteLine(1, $" * @param {param.GetParamName()} {param.Comment}");
        }

        fw.WriteLine(1, " * @param options Options pour 'fetch'.");

        if (endpoint.Returns != null)
        {
            fw.WriteLine(1, $" * @returns {endpoint.Returns.Comment}");
        }

        fw.WriteLine(1, " */");
        fw.Write(1, $"{endpoint.NameCamel}(");

        var hasForm = endpoint.Params.Any(p => p is IFieldProperty fp && Config.GetType(fp).Contains("File"));
        var hasProperty = false;
        foreach (var param in endpoint.Params)
        {
            if (hasProperty)
            {
                fw.Write(", ");
            }

            hasProperty = true;
            var defaultValue = Config.GetValue(param, Classes);
            fw.Write($"{param.GetParamName()}{(param.IsQueryParam() && !hasForm && defaultValue == "undefined" ? "?" : string.Empty)}: {Config.GetType(param, Classes)}{(defaultValue != "undefined" ? $" = {defaultValue}" : string.Empty)}");
        }

        string returnType;
        if (endpoint.GetQueryParams().Any())
        {
            if (hasProperty)
            {
                fw.Write(", ");
            }

            fw.Write("queryParams: any = {}");
        }

        fw.Write("): Observable<");

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
            fw.Write(2, $@"return this.http.{endpoint.Method.ToLower()}(`/{endpoint.FullRoute.Replace("{", "${")}`, {{responseType: 'text'}}");
        }
        else
        {
            fw.Write(2, $@"return this.http.{endpoint.Method.ToLower()}<{returnType}>(`/{endpoint.FullRoute.Replace("{", "${")}`");
        }

        if (endpoint.GetBodyParam() != null)
        {
            fw.Write($", {endpoint.GetBodyParam()!.GetParamName()}");
        }

        if (endpoint.GetQueryParams().Any())
        {
            fw.Write(", httpOptions");
        }

        fw.WriteLine(");");
        fw.WriteLine(1, "}");
    }
}