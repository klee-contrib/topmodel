using Microsoft.Extensions.Logging;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.EndpointGeneration;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class SpringRestTemplateApiGenerator(
    ILogger<SpringRestTemplateApiGenerator> logger,
    IFileWriterProvider writerProvider
) : EndpointsGeneratorBase<JpaConfig>(logger, writerProvider)
{
    public override string Name => "SpringRestTemplateGen";
    private static string DefaultApiClassName => "Abstract{fileName}Client";

    protected override bool FilterTag(string tag)
    {
        return Config.GetApiGenerationMode(tag) == ApiGenerationMode.Client;
    }

    protected string GetClassName(string fileName, string tag)
    {
        return Config.GetApiClassName(DefaultApiClassName, fileName, tag);
    }

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Path.Combine(Config.GetApiPath(file, tag), $"{GetClassName(file.Options.Endpoints.FileName, tag)}.java");
    }

    protected virtual IList<string> GetMethodParams(Endpoint endpoint, bool withType = true, bool withBody = true)
    {
        var methodParams = new List<string>();
        foreach (var param in endpoint.GetRouteParams())
        {
            if (withType)
            {
                methodParams.Add($"{Config.GetType(param)} {param.GetParamName()}");
            }
            else
            {
                methodParams.Add(param.GetParamName());
            }
        }

        foreach (var param in endpoint.GetQueryParams(Config))
        {
            if (withType)
            {
                methodParams.Add($"{Config.GetType(param)} {param.GetParamName()}");
            }
            else
            {
                methodParams.Add(param.GetParamName());
            }
        }

        var bodyParam = endpoint.GetJsonBodyParam(Config);
        if (bodyParam != null && withBody)
        {
            if (withType)
            {
                methodParams.Add($"{Config.GetType(bodyParam)} {bodyParam.GetParamName()}");
            }
            else
            {
                methodParams.Add(bodyParam.GetParamName());
            }
        }

        return methodParams;
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        var className = GetClassName(fileName, tag);
        var packageName = Config.GetPackageName(endpoints[0], tag);
        using var fw = this.OpenJavaWriter(filePath, packageName, codePage: null);

        WriteImports(endpoints, fw, tag);
        fw.WriteLine();

        if (Config.GeneratedHint)
        {
            fw.Write(Config.GeneratedAnnotation);
        }

        fw.WriteLine($"public abstract class {className} {{");

        fw.WriteLine();

        fw.WriteLine(1, $"protected RestTemplate restTemplate;");
        fw.WriteLine(1, $"protected String host;");

        fw.WriteLine();
        fw.WriteDocStart(1, "Constructeur par paramètres");
        fw.WriteLine(1, " * @param restTemplate");
        fw.WriteLine(1, " * @param host");
        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"protected {className}(RestTemplate restTemplate, String host) {{");
        fw.WriteLine(2, $"this.restTemplate = restTemplate;");
        fw.WriteLine(2, $"this.host = host;");
        fw.WriteLine(1, $"}}");

        fw.WriteLine();
        fw.WriteDocStart(1, "Méthode de récupération des headers");
        fw.WriteLine(1, " * @return les headers à ajouter à la requête");
        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"protected abstract HttpHeaders getHeaders();");

        foreach (var endpoint in endpoints)
        {
            WriteEndpoint(fw, endpoint, tag);
        }

        fw.WriteLine("}");
    }

    protected virtual void WriteEndpoint(JavaWriter fw, Endpoint endpoint, string tag)
    {
        fw.WriteLine();
        WriteUriBuilderMethod(fw, endpoint);
        fw.WriteLine();
        WriteEndpointCallMethod(fw, endpoint, tag);
    }

    protected virtual void WriteEndpointCallMethod(JavaWriter fw, Endpoint endpoint, string tag)
    {
        fw.WriteDocStart(1, endpoint.Description);

        foreach (var param in Config.GetParams(endpoint))
        {
            fw.WriteLine(1, $" * @param {param.GetParamName()} {param.Comment}");
        }

        var returns = Config.GetReturns(endpoint);

        if (returns != null)
        {
            fw.WriteLine(1, $" * @return {returns.Comment}");
            fw.AddImports(returns.GetTypeImports(Config, tag));
        }

        AddMethodParamsImports(fw, endpoint, tag);

        fw.WriteLine(1, " */");
        var returnType = "ResponseEntity";
        var returnClass = "(Class<?>) null";
        if (returns != null)
        {
            if (Config.GetType(returns) == "ResponseEntity" && Config.GetType(returns).Split('<').Length > 1)
            {
                returnType = $"ResponseEntity<{Config.GetType(returns).Split('<')[1].Split('>')[0]}>";
                returnClass = $"{Config.GetType(returns).Split('<')[1].Split('>')[0]}.class";
            }
            else if (Config.GetType(returns).Contains('<'))
            {
                returnType = $"ResponseEntity<{Config.GetType(returns)}>";
                returnClass = @$"new ParameterizedTypeReference<{Config.GetType(returns)}>() {{}}";
                fw.AddImport("org.springframework.core.ParameterizedTypeReference");
            }
            else
            {
                returnType = $"ResponseEntity<{Config.GetType(returns)}>";
                returnClass = $"{Config.GetType(returns)}.class";
            }
        }

        fw.WriteLine(1, $"public {returnType} {endpoint.NameCamel}({string.Join(", ", GetMethodParams(endpoint))}){{");
        fw.WriteLine(2, $"HttpHeaders headers = this.getHeaders();");
        fw.WriteLine(
            2,
            $"UriComponentsBuilder uri = this.{endpoint.NameCamel}UriComponentsBuilder({string.Join(", ", GetMethodParams(endpoint, withType: false, withBody: false))});"
        );
        var body =
            $"new HttpEntity<>({(endpoint.GetJsonBodyParam(Config)?.GetParamName() != null ? $"{endpoint.GetJsonBodyParam(Config)?.GetParamName()}, " : string.Empty)}headers)";

        fw.WriteLine(
            2,
            $"return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.{endpoint.Method}, {body}, {returnClass});"
        );
        fw.WriteLine(1, "}");
    }

    protected virtual void AddMethodParamsImports(JavaWriter fw, Endpoint endpoint, string tag)
    {
        foreach (var param in endpoint.GetRouteParams().Concat(endpoint.GetQueryParams(Config)))
        {
            fw.AddImports(param.GetTypeImports(Config, tag));
        }

        if (endpoint.GetJsonBodyParam(Config) is IProperty bodyParam)
        {
            fw.AddImports(bodyParam.GetTypeImports(Config, tag));
        }
    }

    protected virtual void WriteImports(IEnumerable<Endpoint> endpoints, JavaWriter fw, string tag)
    {
        fw.AddImports(
            [
                "jakarta.annotation.Generated",
                "org.springframework.web.util.UriComponentsBuilder",
                "org.springframework.web.client.RestTemplate",
                "java.net.URI",
                "org.springframework.http.HttpMethod",
                "org.springframework.http.HttpEntity",
                "org.springframework.http.HttpHeaders",
                "org.springframework.http.ResponseEntity",
            ]
        );
    }

    protected virtual void WriteUriBuilderMethod(JavaWriter fw, Endpoint endpoint)
    {
        fw.WriteDocStart(1, $"UriComponentsBuilder pour la méthode {endpoint.NameCamel}");

        foreach (var param in endpoint.GetRouteParams().Concat(endpoint.GetQueryParams(Config)))
        {
            fw.WriteLine(1, $" * @param {param.GetParamName()} {param.Comment}");
        }

        if (Config.GetReturns(endpoint) != null)
        {
            fw.WriteLine(1, $" * @return uriBuilder avec les query params remplis");
        }

        fw.WriteLine(1, " */");
        var returnType = "UriComponentsBuilder";
        var methodParams = GetMethodParams(endpoint, withType: true, withBody: false);

        fw.WriteLine(
            1,
            $"protected {returnType} {endpoint.NameCamel}UriComponentsBuilder({string.Join(", ", methodParams)}) {{"
        );
        var fullRoute = endpoint.FullRoute;
        fullRoute = "/" + fullRoute;
        foreach (var p in endpoint.GetRouteParams())
        {
            fullRoute = fullRoute.Replace(@$"{{{p.GetParamName()}}}", "%s");
        }

        if (endpoint.GetRouteParams().Any())
        {
            fullRoute =
                $@"""{fullRoute}"".formatted({string.Join(", ", endpoint.GetRouteParams().Select(p => p.GetParamName()))})";
        }
        else
        {
            fullRoute = $@"""{fullRoute}""";
        }

        fw.WriteLine(2, @$"String uri = host + {fullRoute};");
        if (!endpoint.GetQueryParams(Config).Any())
        {
            fw.WriteLine(2, @$"return UriComponentsBuilder.fromUri(URI.create(uri));");
            fw.WriteLine(1, "}");
            return;
        }

        fw.WriteLine(2, @$"UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));");
        foreach (var p in endpoint.GetQueryParams(Config))
        {
            var indentLevel = 2;
            if (!p.Required)
            {
                fw.WriteLine(2, @$"if ({p.GetParamName()} != null) {{");
                indentLevel++;
            }

            if (Config.GetType(p).StartsWith("List"))
            {
                fw.AddImport("java.util.stream.Collectors");
                fw.WriteLine(
                    indentLevel,
                    @$"uriBuilder.queryParam(""{p.GetParamName()}"", {p.GetParamName()}.stream().collect(Collectors.joining("","")));"
                );
            }
            else
            {
                fw.WriteLine(indentLevel, @$"uriBuilder.queryParam(""{p.GetParamName()}"", {p.GetParamName()});");
            }

            if (!p.Required)
            {
                fw.WriteLine(2, @$"}}");
                fw.WriteLine();
            }
        }

        fw.WriteLine(2, $"return uriBuilder;");
        fw.WriteLine(1, "}");
    }
}
