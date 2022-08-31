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

    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

    public SpringServerApiGenerator(ILogger<SpringServerApiGenerator> logger, JpaConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "SpringServerApiGenerator";

    public override IEnumerable<string> GeneratedFiles => _files.Select(f => GetFilePath(f.Value));

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            _files[file.Name] = file;
            GenerateController(file);
        }
    }

    private string GetDestinationFolder(ModelFile file)
    {
        return Path.Combine(_config.ApiServerOutputDirectory!, Path.Combine(_config.ApiServerOutputDirectory!.ToLower().Split(".")), Path.Combine(_config.ApiServerPackageName.Split('.')), Path.Combine(file.Module.ToLower().Split(".")));
    }

    private string GetClassName(ModelFile file)
    {
        if (file.Options?.Endpoints?.FileName != null)
        {
            return $"I{file.Options.Endpoints.FileName.ToFirstUpper()}Controller";
        }

        var filePath = file.Name.Split("/").Last();
        return $"I{string.Join('_', filePath.Split("_").Skip(filePath.Contains('_') ? 1 : 0)).ToFirstUpper()}Controller";
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

    private void GenerateController(ModelFile file)
    {
        if (!file.Endpoints.Any() || _config.ApiServerOutputDirectory == null)
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

        fw.WriteLine($"package {_config.ApiServerPackageName}.{file.Module.ToLower()};");

        WriteImports(file, fw);
        fw.WriteLine();
        if (file.Options?.Endpoints.Prefix != null)
        {
            fw.WriteLine($@"@RequestMapping(""{file.Options.Endpoints.Prefix}"")");
        }

        fw.WriteLine("@Generated(\"TopModel : https://github.com/klee-contrib/topmodel\")");
        fw.WriteLine($"public interface {GetClassName(file)} {{");

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
        }

        var hasForm = endpoint.Params.Any(p => p is IFieldProperty { Domain.Java.Type: "MultipartFile" });
        fw.WriteLine(1, @$"@{endpoint.Method.ToLower().ToFirstUpper()}Mapping(path = ""{endpoint.Route}""{(hasForm ? ", consumes = { MediaType.MULTIPART_FORM_DATA_VALUE }" : string.Empty)})");

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

    private void WriteImports(ModelFile file, JavaWriter fw)
    {
        var imports = file.Endpoints.Select(e => $"org.springframework.web.bind.annotation.{e.Method.ToLower().ToFirstUpper()}Mapping").ToList();
        imports.AddRange(GetTypeImports(file));
        imports.Add("javax.annotation.Generated");
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

        var hasForm = file.Endpoints.Any(endpoint => endpoint.Params.Any(p => p is IFieldProperty { Domain.Java.Type: "MultipartFile" }));
        if (hasForm)
        {
            imports.Add("org.springframework.http.MediaType");
        }

        if (file.Options?.Endpoints.Prefix != null)
        {
            imports.Add("org.springframework.web.bind.annotation.RequestMapping");
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
                throw new ModelException(endpoint, $"Le endpoint {endpoint.Route} ne peut pas contenir d'association");
            }
        }

        if (endpoint.Returns != null && endpoint.Returns is AssociationProperty)
        {
            throw new ModelException(endpoint, $"Le retour du endpoint {endpoint.Route} ne peut pas être une association");
        }
    }
}