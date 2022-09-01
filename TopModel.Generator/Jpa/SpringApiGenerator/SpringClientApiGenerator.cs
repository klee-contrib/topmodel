using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class SpringClientApiGenerator : GeneratorBase
{
    private readonly JpaConfig _config;
    private readonly ILogger<SpringClientApiGenerator> _logger;

    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

    public SpringClientApiGenerator(ILogger<SpringClientApiGenerator> logger, JpaConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "SpringClientApiGenerator";

    public override IEnumerable<string> GeneratedFiles => _files.Select(f => GetFilePath(f.Value));

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            _files[file.Name] = file;
            GenerateClient(file);
        }
    }

    private string GetDestinationFolder(ModelFile file)
    {
        return Path.Combine(_config.ApiClientOutputDirectory!, Path.Combine(_config.ApiClientOutputDirectory!.ToLower().Split(".")), Path.Combine(_config.ApiClientPackageName.Split('.')), Path.Combine(file.Module.ToLower().Split(".")));
    }

    private string GetClassName(ModelFile file)
    {
        if (file.Options?.Endpoints?.FileName != null)
        {
            return $"Abstract{file.Options.Endpoints.FileName.ToFirstUpper()}Client";
        }

        var filePath = file.Name.Split("/").Last();
        return $"Abstract{string.Join('_', filePath.Split("_").Skip(filePath.Contains('_') ? 1 : 0)).ToFirstUpper()}Client";
    }

    private string GetFileName(ModelFile file)
    {
        var filePath = file.Name.Split("/").Last();
        return $"{GetClassName(file)}.java";
    }

    private string GetFilePath(ModelFile file)
    {
        return $"{GetDestinationFolder(file)}\\{GetFileName(file)}";
    }

    private void GenerateClient(ModelFile file)
    {
        if (!file.Endpoints.Any() || _config.ApiClientOutputDirectory == null)
        {
            return;
        }

        foreach (var endpoint in file.Endpoints)
        {
            CheckEndpoint(endpoint);
        }

        var destFolder = GetDestinationFolder(file);
        Directory.CreateDirectory(destFolder);

        using var fw = new JavaWriter($"{GetFilePath(file)}", _logger, null);

        fw.WriteLine($"package {_config.ApiClientPackageName}.{file.Module.ToLower()};");

        WriteImports(file, fw);
        fw.WriteLine();

        fw.WriteLine("@Generated(\"TopModel : https://github.com/klee-contrib/topmodel\")");
        fw.WriteLine($"public abstract class {GetClassName(file)} {{");

        fw.WriteLine();

        fw.WriteLine(1, $"protected RestTemplate restTemplate;");
        fw.WriteLine(1, $"protected String host;");

        fw.WriteLine();
        fw.WriteDocStart(1, "Constructeur par paramètres");
        fw.WriteLine(1, " * @param restTemplate");
        fw.WriteLine(1, " * @param host");
        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"protected {GetClassName(file)}(RestTemplate restTemplate, String host) {{");
        fw.WriteLine(2, $"this.restTemplate = restTemplate;");
        fw.WriteLine(2, $"this.host = host;");
        fw.WriteLine(1, $"}}");

        GetClassName(file);

        foreach (var endpoint in file.Endpoints)
        {
            WriteEndPoint(fw, endpoint);
        }

        fw.WriteLine("}");
    }

    private void WriteEndPoint(JavaWriter fw, Endpoint endpoint)
    {
        fw.WriteLine();
        WriteUriBuilderMethod(fw, endpoint);
        fw.WriteLine();
        WriteEndpointCallMethod(fw, endpoint);
    }

    private List<string> GetMethodParams(Endpoint endpoint, bool withType = true, bool withBody = true)
    {
        var methodParams = new List<string>();
        foreach (var param in endpoint.GetRouteParams())
        {
            if (withType)
            {
                methodParams.Add($"{param.GetJavaType()} {param.GetParamName()}");
            }
            else
            {
                methodParams.Add(param.GetParamName());
            }
        }

        foreach (var param in endpoint.GetQueryParams())
        {
            if (withType)
            {
                methodParams.Add($"{param.GetJavaType()} {param.GetParamName()}");
            }
            else
            {
                methodParams.Add(param.GetParamName());
            }
        }

        var bodyParam = endpoint.GetBodyParam();
        if (bodyParam != null && withBody)
        {
            if (withType)
            {
                methodParams.Add($"{bodyParam.GetJavaType()} {bodyParam.GetParamName()}");
            }
            else
            {
                methodParams.Add(bodyParam.GetParamName());
            }
        }

        return methodParams;
    }

    private void WriteUriBuilderMethod(JavaWriter fw, Endpoint endpoint)
    {
        fw.WriteDocStart(1, $"UriComponentsBuilder pour la méthode {endpoint.Name}");

        foreach (var param in endpoint.GetRouteParams().Concat(endpoint.GetQueryParams()))
        {
            fw.WriteLine(1, $" * @param {param.GetParamName()} {param.Comment}");
        }

        if (endpoint.Returns != null)
        {
            fw.WriteLine(1, $" * @return uriBuilder avec les query params remplis");
        }

        fw.WriteLine(1, " */");
        var returnType = "UriComponentsBuilder";
        var methodParams = GetMethodParams(endpoint, true, false);

        fw.WriteLine(1, $"protected {returnType} {endpoint.Name.ToFirstLower()}UriComponentsBuilder({string.Join(", ", methodParams)}) {{");
        var fullRoute = endpoint.FullRoute;
        foreach (IProperty p in endpoint.GetRouteParams())
        {
            fullRoute = fullRoute.Replace(@$"{{{p.GetParamName()}}}", "%s");
        }

        if (endpoint.GetRouteParams().Any())
        {
            fullRoute = $@"""{fullRoute}"".formatted({string.Join(", ", endpoint.GetRouteParams().Select(p => p.GetParamName()))});";
        }
        else
        {
            fullRoute = $@"""{fullRoute}""";
        }

        fw.WriteLine(2, @$"String uri = host + {fullRoute};");
        if (!endpoint.GetQueryParams().Any())
        {
            fw.WriteLine(2, @$"return UriComponentsBuilder.fromUri(URI.create(uri));");
            fw.WriteLine(1, "}");
            return;
        }

        fw.WriteLine(2, @$"UriComponentsBuilder uriBuilder = UriComponentsBuilder.fromUri(URI.create(uri));");
        foreach (IProperty p in endpoint.GetQueryParams())
        {
            var indentLevel = 2;
            var isRequired = p is IFieldProperty fp && fp.Required;
            if (!isRequired)
            {
                fw.WriteLine(2, @$"if ({p.GetParamName()} != null) {{");
                indentLevel++;
            }

            fw.WriteLine(indentLevel, @$"uriBuilder.queryParam(""{p.GetParamName()}"", {p.GetParamName()});");
            if (!isRequired)
            {
                fw.WriteLine(2, @$"}}");
                fw.WriteLine();
            }
        }

        fw.WriteLine(2, $"return uriBuilder;");
        fw.WriteLine(1, "}");
    }

    private void WriteEndpointCallMethod(JavaWriter fw, Endpoint endpoint)
    {
        fw.WriteDocStart(1, endpoint.Description);

        foreach (var param in endpoint.Params)
        {
            fw.WriteLine(1, $" * @param {param.GetParamName()} {param.Comment}");
        }

        if (endpoint.Returns != null)
        {
            fw.WriteLine(1, $" * @return {endpoint.Returns.Comment}");
        }

        fw.WriteLine(1, " */");
        var returnType = "ResponseEntity";
        var returnClass = "(Class<?>) null";
        if (endpoint.Returns != null)
        {
            returnType = $"ResponseEntity<{endpoint.Returns.GetJavaType().Split('<').First()}>";
            returnClass = $"{endpoint.Returns.GetJavaType().Split('<').First()}.class";
            if (endpoint.Returns.GetJavaType().Split('<').First() == "ResponseEntity" && endpoint.Returns.GetJavaType().Split('<').Count() > 1)
            {
                returnType = $"ResponseEntity<{endpoint.Returns.GetJavaType().Split('<')[1].Split('>').First()}>";
                returnClass = $"{endpoint.Returns.GetJavaType().Split('<')[1].Split('>').First()}.class";
            }
        }

        var methodParams = GetMethodParams(endpoint, true, false);
        fw.WriteLine(1, $"public {returnType} {endpoint.Name.ToFirstLower()}({string.Join(", ", GetMethodParams(endpoint).Concat(new List<string>() { "HttpHeaders headers" }))}){{");
        fw.WriteLine(2, $"UriComponentsBuilder uri = this.{endpoint.Name.ToFirstLower()}UriComponentsBuilder({string.Join(", ", GetMethodParams(endpoint, false, false))});");
        var body = $"new HttpEntity<>({(endpoint.GetBodyParam()?.GetParamName() != null ? $"{endpoint.GetBodyParam()?.GetParamName()}, " : string.Empty)}headers)";
        if (endpoint.Returns != null)
        {
            fw.WriteLine(2, $"return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.{endpoint.Method}, {body}, {returnClass});");
        }
        else
        {
            fw.WriteLine(2, $"return this.restTemplate.exchange(uri.build().toUri(), HttpMethod.{endpoint.Method}, {body}, {returnClass});");
        }

        fw.WriteLine(1, "}");
    }

    private void WriteImports(ModelFile file, JavaWriter fw)
    {
        var imports = new List<string>();
        imports.AddRange(GetTypeImports(file));
        imports.Add("javax.annotation.Generated");
        imports.Add("org.springframework.web.util.UriComponentsBuilder");
        imports.Add("org.springframework.web.client.RestTemplate");
        imports.Add("java.net.URI");
        imports.Add("org.springframework.http.HttpMethod");
        imports.Add("org.springframework.http.HttpEntity");
        imports.Add("org.springframework.http.HttpHeaders");
        imports.Add("org.springframework.http.ResponseEntity");
        fw.WriteImports(imports.Distinct().ToArray());
    }

    private IEnumerable<string> GetTypeImports(ModelFile file)
    {
        var properties = file.Endpoints.SelectMany(endpoint => endpoint.Params).Concat(file.Endpoints.Where(endpoint => endpoint.Returns is not null).Select(endpoint => endpoint.Returns));
        return properties.SelectMany(property => property!.GetImports(_config));
    }

    private void CheckEndpoint(Endpoint endpoint)
    {
        foreach (var q in endpoint.GetQueryParams().Concat(endpoint.GetRouteParams()))
        {
            if (q is AssociationProperty ap)
            {
                throw new ModelException(endpoint, $"Le endpoint {endpoint.Route} ne peut pas contenir d'association");
            }
        }

        if (endpoint.Returns != null && endpoint.Returns is AssociationProperty)
        {
            throw new ModelException(endpoint, $"Le retour du endpoint {endpoint.Route} ne peut pas être une association");
        }
    }
}