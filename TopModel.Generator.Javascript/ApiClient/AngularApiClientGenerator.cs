using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Generator.Javascript.ApiClient;

/// <summary>
/// Générateur de clients d'API Angular.
/// </summary>
public class AngularApiClientGenerator(ILogger<AngularApiClientGenerator> logger, IFileWriterProvider writerProvider)
    : BaseApiClientGenerator(logger, writerProvider)
{
    public override string Name => "JSNGApiClientGen";

    protected override string EndpointDefinitionPrefix => string.Empty;

    protected override int EndpointIndent => 1;

    protected override string EndpointReturnType =>
        Config.ApiMode == TargetFramework.ANGULAR ? "Observable" : "Promise";

    protected override string ImportSpacing => " ";

    protected override bool UndefinedInReturnType => false;

    private static string OptionsType
    {
        get
        {
            var options = new List<string>
            {
                "headers?: HttpHeaders | {[header: string]: string | string[]}",
                "context?: HttpContext",
                "params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}",
                "withCredentials?: boolean",
                "reportProgress?: boolean",
                "transferCache?: {includeHeaders?: string[]} | boolean",
            };
            return @$"{{{string.Join("; ", options)}}}";
        }
    }

    protected override IEnumerable<(string Import, string Path)> GetFrameworkImports(
        string filePath,
        string tag,
        IList<Endpoint> endpoints
    )
    {
        yield return ("Injectable", "@angular/core");
        yield return ("inject", "@angular/core");
        yield return ("HttpClient", "@angular/common/http");

        if (Config.ApiMode == TargetFramework.ANGULAR)
        {
            yield return ("Observable", "rxjs");
        }
        else
        {
            yield return ("lastValueFrom", "rxjs");
        }

        if (endpoints.Any(e => e.GetQueryParams(Config).Any()))
        {
            yield return ("HttpParams", "@angular/common/http");
        }

        yield return ("HttpHeaders", "@angular/common/http");
        yield return ("HttpParams", "@angular/common/http");
        yield return ("HttpContext", "@angular/common/http");
    }

    protected override void WriteClassDeclarationEnd(IFileWriter fw)
    {
        fw.WriteLine("}");
    }

    protected override void WriteClassDeclarationStart(IFileWriter fw, string fileName)
    {
        fw.WriteLine();
        fw.WriteLine("@Injectable({");
        fw.WriteLine(1, "providedIn: 'root'");
        fw.WriteLine("})");

        fw.WriteLine(@$"export class {fileName.ToPascalCase()}Service {{");
        fw.WriteLine();
        fw.WriteLine(1, "private readonly http = inject(HttpClient);");
    }

    protected override void WriteEndpoint(IFileWriter fw, Endpoint endpoint)
    {
        var options = GetSafeVariableName(endpoint, "options");

        WriteEndpointSignature(fw, endpoint, (options, null, OptionsType, "{}"));

        WriteFormDataBody(fw, endpoint, "body");

        if (endpoint.GetQueryParams(Config).Any())
        {
            fw.WriteLine(
                2,
                @"const addParam = (key: string, value: any) => {
  if (value !== null && value !== undefined) {
    if (options.params instanceof HttpParams) {
      options.params = options.params.append(key, value);
    } else {
      if (!options.params) {
        options.params = {};
      }
      options.params[key] = value;
    }
  }
};"
            );

            foreach (var qParam in endpoint.GetQueryParams(Config))
            {
                fw.WriteLine(2, $"addParam('{qParam.GetParamName()}', {qParam.GetParamName()});");
            }

            fw.WriteLine();
        }

        var observe = "body";
        var returnType = GetReturnType(GetReturns(endpoint));
        var genericType = returnType.Split('<')[0];
        if (genericType == "HttpEvent")
        {
            observe = "events";
        }
        else if (genericType == "HttpResponse")
        {
            observe = "response";
        }
        var needResponseType = returnType == "string" || returnType == "Blob" || returnType == "ArrayBuffer";
        var getter = $"{endpoint.Method.ToLower()}<{returnType}>";

        if (observe != "body")
        {
            getter = $"{endpoint.Method.ToLower()}<{returnType.Split('<')[1].Split('>')[0]}>";
        }

        if (needResponseType)
        {
            getter = $"{endpoint.Method.ToLower()}";
        }

        var fullRoute = endpoint.FullRoute.Replace("{", "${");
        fw.Write(
            2,
            $@"return {(Config.ApiMode == TargetFramework.ANGULAR_PROMISE ? "lastValueFrom(" : string.Empty)}this.http.{getter}(`/{fullRoute}`"
        );
        if (endpoint.GetJsonBodyParam(Config) != null && endpoint.Method != "DELETE")
        {
            fw.Write($", {endpoint.GetJsonBodyParam(Config)!.GetParamName()}");
        }
        else if (endpoint.IsMultipart)
        {
            fw.Write($", body");
        }
        else if (endpoint.Method != "GET" && endpoint.Method != "DELETE")
        {
            fw.Write(", {}");
        }

        var optionsValue = new Dictionary<string, string> { ["observe"] = $"'{observe}'" };
        if (needResponseType)
        {
            var responseType = returnType == "string" ? "text" : returnType.ToLower();
            optionsValue["responseType"] = $"'{responseType}'";
        }

        if (endpoint.GetJsonBodyParam(Config) != null && endpoint.Method == "DELETE")
        {
            optionsValue["body"] = endpoint.GetJsonBodyParam(Config)!.GetParamName();
        }

        fw.Write(
            @$", {{{string.Join(", ", optionsValue.OrderBy(o => o.Key).Select(o => $"{o.Key}: {o.Value}"))}, ...options}}"
        );
        fw.WriteLine($"{(Config.ApiMode == TargetFramework.ANGULAR_PROMISE ? ")" : string.Empty)});");
        fw.WriteLine(1, "}");
    }
}
