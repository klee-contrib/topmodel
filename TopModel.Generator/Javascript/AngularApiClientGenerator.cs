using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class AngularApiClientGenerator : GeneratorBase
{
    private readonly JavascriptConfig _config;
    private readonly ILogger<AngularApiClientGenerator> _logger;

    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

    public AngularApiClientGenerator(ILogger<AngularApiClientGenerator> logger, JavascriptConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "AngularApiClientGen";

    public override List<string> GeneratedFiles => _files.Values.Where(f => f.Endpoints.Any()).Select(GetFileName).ToList();

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            _files[file.Name] = file;
            GenerateClientFile(file);
        }
    }

    private string GetFileName(ModelFile file)
    {
        var fileSplit = file.Name.Split("/");
        var modulePath = string.Join('\\', file.Module.Split('.').Select(m => m.ToDashCase()));
        var filePath = _config.ApiClientFilePath.Replace("{module}", modulePath) + '/';
        var fileName = string.Join('_', fileSplit.Last().Split("_").Skip(fileSplit.Last().Contains('_') ? 1 : 0)).ToDashCase();

        if (file.Options?.Endpoints?.FileName != null)
        {
            fileName = file.Options?.Endpoints?.FileName.ToDashCase();
        }

        return $"{_config.ApiClientOutputDirectory}\\{filePath}\\{fileName}.ts";
    }

    private void GenerateClientFile(ModelFile file)
    {
        if (!file.Endpoints.Any())
        {
            return;
        }

        var modulePath = string.Join('/', file.Module.Split('.').Select(m => m.ToDashCase()));
        var fileName = GetFileName(file);

        var relativePath = _config.ApiClientFilePath.Length > 0 ? string.Join(string.Empty, _config.ApiClientFilePath.Replace("{module}", modulePath).Split('/').Select(s => "../")) : string.Empty;

        using var fw = new FileWriter(fileName, _logger, false);
        var imports = GetImports(file, relativePath);
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

        fw.WriteLine(@$"export class {GetClassName(file)} {{");
        fw.WriteLine();
        fw.WriteLine(1, "constructor(private http: HttpClient) {}");
        foreach (var endpoint in file.Endpoints)
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
        fw.Write(1, $"{endpoint.Name.ToFirstLower()}(");

        var hasForm = endpoint.Params.Any(p => p is IFieldProperty fp && fp.Domain.TS!.Type.Contains("File"));
        var hasProperty = false;
        foreach (var param in endpoint.Params)
        {
            hasProperty = true;
            fw.Write($"{param.GetParamName()}{(param.IsQueryParam() && !hasForm ? "?" : string.Empty)}: {param.GetPropertyTypeName()} ");
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
            returnType = endpoint.Returns.GetPropertyTypeName();
        }

        fw.Write(returnType);
        fw.WriteLine("> {");

        if (endpoint.GetQueryParams().Any())
        {
            fw.WriteLine(2, "const httpParams = new HttpParams({fromObject : queryParams});");
            fw.WriteLine(2, "const httpOptions = { params: httpParams }");
            foreach (var qParam in endpoint.GetQueryParams())
            {
                fw.WriteLine(2, @$"if({qParam.GetParamName()} !== null) {{");
                fw.WriteLine(3, $"httpOptions.params.set('{qParam.GetParamName()}', {qParam.GetParamName()})");
                fw.WriteLine(2, @$"}}");
            }

            fw.WriteLine();
        }

        fw.Write(2, $@"return this.http.{endpoint.Method.ToLower()}<{returnType}>(`/{endpoint.FullRoute.Replace("{", "${")}`");

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

    private string GetClassName(ModelFile file)
    {
        if (file.Options?.Endpoints?.FileName != null)
        {
            return $"{file.Options.Endpoints.FileName.ToFirstUpper()}Service";
        }

        var filePath = file.Name.Split("/").Last();
        return $"{string.Join('_', filePath.Split("_").Skip(filePath.Contains('_') ? 1 : 0)).ToFirstUpper()}Service";
    }

    private IList<(string Import, string Path)> GetImports(ModelFile file, string relativePath)
    {
        var properties = file.Endpoints
            .SelectMany(endpoint => endpoint.Params.Concat(new[] { endpoint.Returns }))
            .Where(p => p != null) as IEnumerable<IProperty>;

        var types = properties.OfType<CompositionProperty>().Select(property => property.Composition);

        var modelPath = Path.GetRelativePath(_config.ApiClientOutputDirectory!, _config.ModelOutputDirectory!).Replace("\\", "/");

        var imports = types.Select(type =>
        {
            var name = type.Name.Value;
            var module = $"{modelPath}/{string.Join('/', type.Namespace.Module.Split('.').Select(m => m.ToDashCase()))}";
            return (Import: name, Path: $"{relativePath}{module}/{name.ToDashCase()}");
        }).Distinct().ToList();
        imports.AddRange(new List<(string Import, string Path)>()
        {
            (Import: "Injectable", Path: "@angular/core"),
            (Import: "HttpClient", Path: "@angular/common/http"),
            (Import: "Observable", Path: "rxjs"),
        });

        if (file.Endpoints.Any(e => e.GetQueryParams().Any()))
        {
            imports.Add((Import: "HttpParams", Path: "@angular/common/http"));
        }

        imports.AddRange(JavascriptUtils.GetImportsForProperties(properties, string.Empty).Select(i => (i.Import, Path: $"{i.Path.Replace("../", $"{relativePath}{modelPath}/")}")));
        return imports.GroupAndSort();
    }
}