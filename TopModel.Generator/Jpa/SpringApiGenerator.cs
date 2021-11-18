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

        var destFolder = Path.Combine(_config.ApiOutputDirectory, Path.Combine(_config.ApiPackageName.Split(".")), "controller", file.Module.ToLower());
        Directory.CreateDirectory(destFolder);

        var fileSplit = file.Name.Split("/");
        var filePath = fileSplit.Length > 1 ? string.Join("/", fileSplit[1..]) : file.Name;

        var fileName = $"Abstract{filePath.ToFirstUpper()}Controller.java";

        using var fw = new JavaWriter($"{destFolder}/{fileName}", _logger, null);

        fw.WriteLine($"package {_config.ApiPackageName}.controller.{file.Module.ToLower()};");

        WriteImports(file, fw);
        fw.WriteLine();
        fw.WriteLine("@RestController");
        fw.WriteLine(@$"@RequestMapping(""{file.Module.ToLower()}"")");
        fw.WriteLine($"public abstract class Abstract{filePath}Controller {{");

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
            fw.WriteLine(1, $" * @returns {endpoint.Returns.Comment}");
        }

        fw.WriteLine(1, " */");
        var returnType = "void";

        if (endpoint.Returns is IFieldProperty fp)
        {
            returnType = fp.Domain.Java!.Type;
        }
        else if (endpoint.Returns is CompositionProperty cp)
        {
            returnType = cp.Composition.Name;
        }

        if (writeAnnotation)
        {
            fw.WriteLine(1, @$"@{endpoint.Method.ToLower().ToFirstUpper()}Mapping(""{endpoint.Route.Replace(endpoint.ModelFile.Module.ToLower(), string.Empty).ToLower()}"")");
        }

        var methodParams = string.Empty;
        var isFirstMethodParam = true;
        foreach (var param in endpoint.GetRouteParams())
        {
            if (!isFirstMethodParam)
            {
                methodParams += ", ";
            }

            if (writeAnnotation)
            {
                methodParams += @$"@PathVariable(""{param.GetParamName()}"") ";
            }

            if (param is IFieldProperty fpe)
            {
                methodParams += $"{fpe.Domain.Java!.Type} {param.GetParamName()}";
            }

            isFirstMethodParam = false;
        }

        foreach (var param in endpoint.GetQueryParams())
        {
            if (!isFirstMethodParam)
            {
                methodParams += ", ";
            }

            if (writeAnnotation)
            {
                methodParams += @$"@RequestParam(""{param.GetParamName()}"") ";
            }

            if (param is IFieldProperty fpe)
            {
                methodParams += $"{fpe.Domain.Java!.Type} {param.GetParamName()}";
            }

            isFirstMethodParam = false;
        }

        var bodyParam = endpoint.GetBodyParam();
        if (bodyParam != null)
        {
            if (!isFirstMethodParam)
            {
                methodParams += ", ";
            }

            if (writeAnnotation)
            {
                methodParams += @$"@RequestBody @Valid ";
            }

            if (bodyParam is CompositionProperty cp)
            {
                methodParams += $"{cp.Composition.Name} {bodyParam.GetParamName()}";
            }
        }

        if (writeAnnotation)
        {
            fw.WriteLine(1, $"public  {returnType} {endpoint.Name.ToFirstLower()}{(writeAnnotation ? "Mapping" : string.Empty)}({methodParams}) {{");
        }
        else
        {
            fw.WriteLine(1, $"public abstract {returnType} {endpoint.Name.ToFirstLower()}({methodParams});");
        }

        if (writeAnnotation)
        {
            var methodCallParams = string.Empty;
            var isFirstParam = true;
            foreach (var param in endpoint.GetRouteParams())
            {
                if (param is IFieldProperty fpe)
                {
                    if (!isFirstParam)
                    {
                        methodCallParams += ", ";
                    }

                    methodCallParams += $"{param.GetParamName()}";
                    isFirstParam = false;
                }
            }

            foreach (var param in endpoint.GetQueryParams())
            {
                if (!isFirstParam)
                {
                    methodCallParams += ", ";
                }

                if (param is IFieldProperty fpe)
                {
                    methodCallParams += $"{param.GetParamName()}";
                }

                isFirstParam = false;
            }

            if (bodyParam != null)
            {
                if (!isFirstParam)
                {
                    methodCallParams += ", ";
                }

                if (bodyParam is CompositionProperty)
                {
                    methodCallParams += $"{bodyParam.GetParamName()}";
                }
            }

            fw.WriteLine(2, $"return this.{endpoint.Name.ToFirstLower()}({methodCallParams});");
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

        imports.Sort();
        foreach (var import in imports.Distinct())
        {
            fw.WriteLine($"import {import};");
        }
    }

    private IEnumerable<string> GetTypeImports(ModelFile file)
    {
        var properties = file.Endpoints.SelectMany(endpoint => endpoint.Params.Concat(new[] { endpoint.Returns }));
        var types = properties.OfType<CompositionProperty>().Select(property => property.Composition);
        return types.Select(type =>
        {
            var name = type.Name;
            var import = $"{_config.DaoPackageName}.dtos.{type.Namespace.Module.ToLower()}.{name}";
            return import;
        }).Distinct();
    }
}