using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class SpringApiGenerator : GeneratorBase
{
    private readonly JpaConfig _config;
    private readonly ILogger<SpringApiGenerator> _logger;

    public SpringApiGenerator(ILogger<SpringApiGenerator> logger, JpaConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "SpringApiGenerator";

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            GenerateController(file);
        }
    }

    private void GenerateController(ModelFile file)
    {
        if (!file.Endpoints.Any() || _config.ApiOutputDirectory == null)
        {
            return;
        }

        foreach (var endpoint in file.Endpoints)
        {
            CheckEndpoint(endpoint);
        }

        var destFolder = Path.Combine(_config.ApiOutputDirectory, Path.Combine(_config.ApiPackageName.Split(".")), "controller", file.Module.ToLower());
        Directory.CreateDirectory(destFolder);

        var fileSplit = file.Name.Split("/");
        var filePath = fileSplit.Length > 1 ? string.Join("/", fileSplit[1..]) : file.Name;

        var fileName = $"I{filePath.ToFirstUpper()}Controller.java";

        using var fw = new JavaWriter($"{destFolder}/{fileName}", _logger, null);

        fw.WriteLine($"package {_config.ApiPackageName}.controller.{file.Module.ToLower()};");

        WriteImports(file, fw);
        fw.WriteLine();
        fw.WriteLine("@RestController");
        fw.WriteLine(@$"@RequestMapping(""{file.Module.ToLower()}"")");
        fw.WriteLine($"public interface I{filePath}Controller {{");

        fw.WriteLine();

        foreach (var endpoint in file.Endpoints)
        {
            WriteEndPoint(fw, endpoint);
        }

        fw.WriteLine("}");
    }

    private void WriteEndPoint(JavaWriter fw, Endpoint endpoint)
    {
        fw.WriteLine();
        WriteEndPointMethod(fw, endpoint, true);
        fw.WriteLine();
        WriteEndPointMethod(fw, endpoint, false);
    }

    private void WriteEndPointMethod(JavaWriter fw, Endpoint endpoint, bool writeAnnotation)
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
        var returnType = "void";

        if (endpoint.Returns != null)
        {
            returnType = endpoint.Returns.GetJavaType();
        }

        if (writeAnnotation)
        {
            fw.WriteLine(1, @$"@{endpoint.Method.ToLower().ToFirstUpper()}Mapping(""{endpoint.Route.Replace(endpoint.ModelFile.Module.ToLower(), string.Empty)}"")");
        }

        var methodParams = new List<string>();
        foreach (var param in endpoint.GetRouteParams())
        {
            var ann = string.Empty;
            if (writeAnnotation)
            {
                ann += @$"@PathVariable(""{param.GetParamName()}"") ";
            }

            methodParams.Add($"{ann}{param.GetJavaType()} {param.GetParamName()}");
        }

        foreach (var param in endpoint.GetQueryParams())
        {
            var ann = string.Empty;
            if (writeAnnotation)
            {
                ann += @$"@RequestParam(""{param.GetParamName()}"") ";
            }

            methodParams.Add($"{ann}{param.GetJavaType()} {param.GetParamName()}");
        }

        var bodyParam = endpoint.GetBodyParam();
        if (bodyParam != null)
        {
            var ann = string.Empty;
            if (writeAnnotation)
            {
                ann += @$"@RequestBody @Valid ";
            }

            methodParams.Add($"{ann}{bodyParam.GetJavaType()} {bodyParam.GetParamName()}");
        }

        if (writeAnnotation)
        {
            fw.WriteLine(1, $"default {returnType} {endpoint.Name.ToFirstLower()}{(writeAnnotation ? "Mapping" : string.Empty)}({string.Join(", ", methodParams)}) {{");
        }
        else
        {
            fw.WriteLine(1, $"{returnType} {endpoint.Name.ToFirstLower()}({string.Join(", ", methodParams)});");
        }

        if (writeAnnotation)
        {
            var methodCallParams = new List<string>();
            foreach (var param in endpoint.GetRouteParams().OfType<IFieldProperty>())
            {
                methodCallParams.Add($"{param.GetParamName()}");
            }

            foreach (var param in endpoint.GetQueryParams().OfType<IFieldProperty>())
            {
                methodCallParams.Add($"{param.GetParamName()}");
            }

            if (bodyParam != null && bodyParam is CompositionProperty)
            {
                methodCallParams.Add($"{bodyParam.GetParamName()}");
            }

            fw.WriteLine(2, @$"{(returnType != "void" ? "return " : string.Empty)}this.{endpoint.Name.ToFirstLower()}({string.Join(", ", methodCallParams)});");
            fw.WriteLine(1, "}");
        }
    }

    private void WriteImports(ModelFile file, JavaWriter fw)
    {
        var imports = file.Endpoints.Select(e => $"org.springframework.web.bind.annotation.{e.Method.ToLower().ToFirstUpper()}Mapping").ToList();
        imports.AddRange(GetTypeImports(file));
        imports.Add("org.springframework.web.bind.annotation.RequestMapping");
        imports.Add("org.springframework.web.bind.annotation.RestController");
        if (file.Endpoints.Any(e => e.GetRouteParams().Any()))
        {
            imports.Add("org.springframework.web.bind.annotation.PathVariable");
        }

        if (file.Endpoints.Any(e => e.GetQueryParams().Any()))
        {
            imports.Add("org.springframework.web.bind.annotation.RequestParam");
        }

        if (file.Endpoints.Any(e => e.GetBodyParam() != null))
        {
            imports.Add("org.springframework.web.bind.annotation.RequestBody");
            imports.Add("javax.validation.Valid");
        }

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
                throw new ModelException(endpoint.ModelFile, $"Le endpoint {endpoint.Route} ne peut pas contenir d'association");
            }
        }

        if (endpoint.Returns != null && endpoint.Returns is AssociationProperty)
        {
            throw new ModelException(endpoint.ModelFile, $"Le retour du endpoint {endpoint.Route} ne peut pas être une association");
        }
    }
}