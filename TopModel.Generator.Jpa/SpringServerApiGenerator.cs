using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class SpringServerApiGenerator : EndpointsGeneratorBase<JpaConfig>
{
    private readonly ILogger<SpringServerApiGenerator> _logger;

    public SpringServerApiGenerator(ILogger<SpringServerApiGenerator> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override string Name => "SpringApiServerGen";

    protected override bool FilterTag(string tag)
    {
        return Config.ResolveVariables(Config.ApiGeneration!, tag) == ApiGeneration.Server;
    }

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Path.Combine(Config.GetApiPath(file, tag), $"{GetClassName(file.Options.Endpoints.FileName)}.java");
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        var className = GetClassName(fileName);
        var packageName = Config.GetPackageName(endpoints.First(), tag);
        using var fw = new JavaWriter(filePath, _logger, packageName, null);

        WriteImports(endpoints, fw, tag);
        fw.WriteLine();
        if (endpoints.First().ModelFile.Options.Endpoints.Prefix != null)
        {
            fw.WriteLine($@"@RequestMapping(""{endpoints.First().ModelFile.Options.Endpoints.Prefix}"")");
            fw.AddImport("org.springframework.web.bind.annotation.RequestMapping");
        }

        var javaOrJakarta = Config.PersistenceMode.ToString().ToLower();
        fw.AddImport($"{javaOrJakarta}.annotation.Generated");
        fw.WriteLine("@Generated(\"TopModel : https://github.com/klee-contrib/topmodel\")");
        fw.WriteLine($"public interface {className} {{");

        fw.WriteLine();

        foreach (var endpoint in endpoints)
        {
            WriteEndpoint(fw, endpoint, tag);
        }

        fw.WriteLine("}");
    }

    private static string GetClassName(string fileName)
    {
        return $"{fileName.ToPascalCase()}Controller";
    }

    private IEnumerable<string> GetTypeImports(IEnumerable<Endpoint> endpoints, string tag)
    {
        var properties = endpoints.SelectMany(endpoint => endpoint.Params)
            .Concat(endpoints.Where(endpoint => endpoint.Returns is not null)
            .Select(endpoint => endpoint.Returns));
        return properties.SelectMany(property => property!.GetTypeImports(Config, tag))
                .Concat(endpoints.Where(endpoint => endpoint.Returns is not null)
                .Select(e => e.Returns).OfType<CompositionProperty>()
                .SelectMany(c => c.GetKindImports(Config, tag)));
    }

    private void WriteEndpoint(JavaWriter fw, Endpoint endpoint, string tag)
    {
        fw.WriteLine();
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
        var returnType = "void";

        if (endpoint.Returns != null)
        {
            returnType = Config.GetType(endpoint.Returns);
        }

        {
            var produces = string.Empty;
            if (endpoint.Returns != null && endpoint.Returns is IFieldProperty fp && fp.Domain.MediaType != null)
            {
                produces = @$", produces = {{ ""{fp.Domain.MediaType}"" }}";
            }

            var consumes = string.Empty;
            if (endpoint.Params.Any(p => p is IFieldProperty fdp && fdp.Domain.MediaType != null))
            {
                consumes = @$", consumes = {{ {string.Join(", ", endpoint.Params.Where(p => p is IFieldProperty fdp && fdp.Domain.MediaType != null).Select(p => $@"""{((IFieldProperty)p).Domain.MediaType}"""))} }}";
            }

            foreach (var annotation in Config.GetDecoratorAnnotations(endpoint, tag))
            {
                fw.WriteLine(1, $"{(annotation.StartsWith("@") ? string.Empty : "@")}{annotation}");
            }

            fw.WriteLine(1, @$"@{endpoint.Method.ToPascalCase(true)}Mapping(path = ""{endpoint.Route}""{consumes}{produces})");
        }

        var methodParams = new List<string>();
        foreach (var param in endpoint.GetRouteParams())
        {
            var pathParamAnnotation = @$"@PathVariable(""{param.GetParamName()}"")";

            fw.AddImport("org.springframework.web.bind.annotation.PathVariable");
            fw.AddImports(Config.GetDomainImports(param, tag));
            var decoratorAnnotations = string.Join(' ', Config.GetDomainAnnotations(param, tag).Select(a => a.StartsWith("@") ? a : "@" + a));
            methodParams.Add($"{pathParamAnnotation}{(decoratorAnnotations.Length > 0 ? $" {decoratorAnnotations}" : string.Empty)} {Config.GetType(param)} {param.GetParamName()}");
        }

        foreach (var param in endpoint.GetQueryParams())
        {
            var ann = string.Empty;
            ann += @$"@RequestParam(value = ""{param.GetParamName()}"", required = {(param is not IFieldProperty fp || fp.Required).ToString().ToFirstLower()}) ";
            fw.AddImport("org.springframework.web.bind.annotation.RequestParam");
            fw.AddImports(Config.GetDomainImports(param, tag));
            var decoratorAnnotations = string.Join(' ', Config.GetDomainAnnotations(param, tag).Select(a => a.StartsWith("@") ? a : "@" + a));
            methodParams.Add($"{ann}{(decoratorAnnotations.Length > 0 ? $" {decoratorAnnotations}" : string.Empty)}{Config.GetType(param)} {param.GetParamName()}");
        }

        var bodyParam = endpoint.GetBodyParam();
        var formParam = endpoint.Params.FirstOrDefault(p => p.Domain?.MediaType == "multipart/form-data", null);
        if (bodyParam != null)
        {
            var ann = string.Empty;
            if (formParam != null)
            {
                if (formParam != bodyParam)
                {
                    ann += @$"@ModelAttribute ";
                    fw.AddImport("org.springframework.web.bind.annotation.ModelAttribute");
                }
                else
                {
                    ann += @$"@RequestPart(value = ""{bodyParam.GetParamName()}"", required = {(bodyParam is not IFieldProperty fp || fp.Required).ToString().ToFirstLower()}) ";
                    fw.AddImport("org.springframework.web.bind.annotation.RequestPart");
                }
            }
            else
            {
                ann += @$"@RequestBody @Valid ";
                fw.AddImport("org.springframework.web.bind.annotation.RequestBody");
                fw.AddImport(Config.PersistenceMode.ToString().ToLower() + ".validation.Valid");
            }

            methodParams.Add($"{ann}{Config.GetType(bodyParam)} {bodyParam.GetParamName()}");
        }

        fw.WriteLine(1, $"{returnType} {endpoint.NameCamel}({string.Join(", ", methodParams)});");
    }

    private void WriteImports(IEnumerable<Endpoint> endpoints, JavaWriter fw, string tag)
    {
        fw.AddImports(endpoints.Select(e => $"org.springframework.web.bind.annotation.{e.Method.ToPascalCase(true)}Mapping"));
        fw.AddImports(GetTypeImports(endpoints, tag));
        fw.AddImports(endpoints.SelectMany(e => Config.GetDecoratorImports(e, tag)));
    }
}
