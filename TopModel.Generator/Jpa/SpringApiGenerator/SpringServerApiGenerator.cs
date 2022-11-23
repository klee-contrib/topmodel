using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class SpringServerApiGenerator : GeneratorBase
{
    private readonly JpaConfig _config;
    private readonly ILogger<SpringServerApiGenerator> _logger;

    public SpringServerApiGenerator(ILogger<SpringServerApiGenerator> logger, JpaConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "SpringApiServerGen";

    public override IEnumerable<string> GeneratedFiles => Files.Values.Where(f => f.Endpoints.Any()).Select(f => GetFilePath(f.Options.Endpoints.FileName, f.Module)).Distinct();

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files.GroupBy(file => new { file.Options.Endpoints.FileName, file.Module }))
        {
            GenerateController(file.Key.FileName, file.Key.Module);
        }
    }

    private string GetDestinationFolder(string module)
    {
        return Path.Combine(_config.OutputDirectory, Path.Combine(_config.ApiRootPath!.ToLower().Split(".")), Path.Combine(_config.ApiPackageName.Split('.')), Path.Combine(module.ToLower().Split(".")));
    }

    private string GetClassName(string fileName)
    {
        return $"{fileName.ToFirstUpper()}Controller";
    }

    private string GetFileName(string fileName)
    {
        return $"{GetClassName(fileName)}.java";
    }

    private string GetFilePath(string fileName, string module)
    {
        return Path.Combine(GetDestinationFolder(module), GetFileName(fileName));
    }

    private void GenerateController(string fileName, string module)
    {
        var files = Files.Values
            .Where(file => file.Options.Endpoints.FileName == fileName && file.Module == module);

        var endpoints = files
            .SelectMany(file => file.Endpoints)
            .OrderBy(endpoint => endpoint.Name, StringComparer.Ordinal)
            .ToList();

        if (!endpoints.Any() || _config.ApiRootPath == null)
        {
            return;
        }

        foreach (var endpoint in endpoints)
        {
            CheckEndpoint(endpoint);
        }

        var destFolder = GetDestinationFolder(module);
        Directory.CreateDirectory(destFolder);
        var packageName = $"{_config.ApiPackageName}.{module.ToLower()}";
        using var fw = new JavaWriter($"{GetFilePath(fileName, module)}", _logger, packageName, null);

        WriteImports(files, fw);
        fw.WriteLine();
        if (files.First().Options.Endpoints.Prefix != null)
        {
            fw.WriteLine($@"@RequestMapping(""{files.First().Options.Endpoints.Prefix}"")");
        }

        fw.WriteLine("@Generated(\"TopModel : https://github.com/klee-contrib/topmodel\")");
        fw.WriteLine($"public interface {GetClassName(fileName)} {{");

        fw.WriteLine();

        foreach (var endpoint in endpoints)
        {
            WriteEndPoint(fw, endpoint);
        }

        fw.WriteLine("}");
    }

    private void WriteEndPoint(JavaWriter fw, Endpoint endpoint)
    {
        fw.WriteLine();
        WriteEndPointMethod(fw, endpoint);
    }

    private void WriteEndPointMethod(JavaWriter fw, Endpoint endpoint)
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
            if (endpoint.Returns is CompositionProperty cp && cp.Composition.Decorators.Any(d => d.Decorator.Java?.GenerateInterface == true))
            {
                fw.AddImport(cp.Composition.GetImport(_config).Replace(cp.Composition.Name, "interfaces.I" + cp.Composition.Name));
                returnType = endpoint.Returns.GetJavaType().Replace(cp.Composition.Name, "I" + cp.Composition.Name);
            }
        }

        var hasForm = endpoint.Params.Any(p => p is IFieldProperty { Domain.Java.Type: "MultipartFile" });
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

            foreach (var d in endpoint.Decorators)
            {
                foreach (var a in d.Decorator.Java?.Annotations ?? Array.Empty<string>())
                {
                    fw.WriteLine(1, $"{(a.StartsWith("@") ? string.Empty : "@")}{a.ParseTemplate(endpoint, d.Parameters)}");
                }
            }

            fw.WriteLine(1, @$"@{endpoint.Method.ToLower().ToFirstUpper()}Mapping(path = ""{endpoint.Route}""{consumes}{produces})");
        }

        var methodParams = new List<string>();
        foreach (var param in endpoint.GetRouteParams())
        {
            var ann = string.Empty;
            ann += @$"@PathVariable(""{param.GetParamName()}"") ";

            methodParams.Add($"{ann}{param.GetJavaType()} {param.GetParamName()}");
        }

        foreach (var param in endpoint.GetQueryParams())
        {
            var ann = string.Empty;
            ann += @$"@RequestParam(value = ""{param.GetParamName()}"", required = {(param is IFieldProperty fp ? fp.Required : true).ToString().ToFirstLower()}) ";

            methodParams.Add($"{ann}{param.GetJavaType()} {param.GetParamName()}");
        }

        var bodyParam = endpoint.GetBodyParam();
        if (bodyParam != null)
        {
            var ann = string.Empty;
            ann += @$"@RequestBody @Valid ";

            methodParams.Add($"{ann}{bodyParam.GetJavaType()} {bodyParam.GetParamName()}");
        }

        fw.WriteLine(1, $"{returnType} {endpoint.Name.ToFirstLower()}({string.Join(", ", methodParams)});");

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
    }

    private void WriteImports(IEnumerable<ModelFile> files, JavaWriter fw)
    {
        var endpoints = files.SelectMany(file => file.Endpoints);

        var imports = endpoints.Select(e => $"org.springframework.web.bind.annotation.{e.Method.ToLower().ToFirstUpper()}Mapping").ToList();
        imports.AddRange(GetTypeImports(files));
        imports.Add(_config.PersistenceMode.ToString().ToLower() + ".annotation.Generated");
        if (endpoints.Any(e => e.GetRouteParams().Any()))
        {
            imports.Add("org.springframework.web.bind.annotation.PathVariable");
        }

        if (endpoints.Any(e => e.GetQueryParams().Any()))
        {
            imports.Add("org.springframework.web.bind.annotation.RequestParam");
        }

        if (endpoints.Any(e => e.GetBodyParam() != null))
        {
            imports.Add("org.springframework.web.bind.annotation.RequestBody");
            imports.Add(_config.PersistenceMode.ToString().ToLower() + ".validation.Valid");
        }

        if (files.First().Options?.Endpoints.Prefix != null)
        {
            imports.Add("org.springframework.web.bind.annotation.RequestMapping");
        }

        imports.AddRange(endpoints.SelectMany(e => e.Decorators.SelectMany(d => (d.Decorator.Java?.Imports ?? Array.Empty<string>()).Select(i => i.ParseTemplate(e, d.Parameters)))).Distinct());

        fw.AddImports(imports);
    }

    private IEnumerable<string> GetTypeImports(IEnumerable<ModelFile> files)
    {
        var properties = files.SelectMany(file => file.Endpoints).SelectMany(endpoint => endpoint.Params)
            .Concat(files.SelectMany(file => file.Endpoints).Where(endpoint => endpoint.Returns is not null && !(endpoint.Returns is CompositionProperty cp && cp.Composition.Decorators.Any(d => d.Decorator.Java?.GenerateInterface == true)))
            .Select(endpoint => endpoint.Returns));
        return properties.SelectMany(property => property!.GetTypeImports(_config))
                .Concat(files.SelectMany(file => file.Endpoints).Where(endpoint => endpoint.Returns is not null && (endpoint.Returns is CompositionProperty cp && cp.Composition.Decorators.Any(d => d.Decorator.Java?.GenerateInterface == true)))
                .Select(e => e.Returns).OfType<CompositionProperty>()
                .SelectMany(c => c.GetKindImports(_config)));
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