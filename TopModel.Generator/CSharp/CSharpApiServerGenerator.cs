using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace TopModel.Generator.CSharp;

public class CSharpApiServerGenerator : GeneratorBase
{
    private readonly CSharpConfig _config;
    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();
    private readonly ILogger<CSharpApiServerGenerator> _logger;

    public CSharpApiServerGenerator(ILogger<CSharpApiServerGenerator> logger, CSharpConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "CSharpApiServerGen";

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            _files[file.Name] = file;
            HandleFile(file);
        }
    }

    private void HandleFile(ModelFile file)
    {
        if (!file.Endpoints.Any())
        {
            return;
        }

        var fileSplit = file.Name.Split("/");
        var path = string.Join("/", fileSplit.Skip(fileSplit.Length > 1 ? 1 : 0).SkipLast(1));
        var className = $"{fileSplit.Last()}Controller";
        var apiPath = _config.ApiPath.Replace("{app}", file.Endpoints.First().Namespace.App).Replace("{module}", file.Module);
        var filePath = $"{_config.OutputDirectory}/{apiPath}/Controllers{(!string.IsNullOrEmpty(path) ? "/" : string.Empty)}{path}/{className}.cs";

        var text = File.Exists(filePath)
            ? File.ReadAllText(filePath)
            : _config.UseLatestCSharp
            ? $@"using Microsoft.AspNetCore.Mvc;

namespace {apiPath};

public class {className} : Controller
{{

}}"
            : $@"using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace {apiPath}
{{
    public class {className} : Controller
    {{

    }}
}}";

        var syntaxTree = CSharpSyntaxTree.ParseText(text);
        var existingController = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();

        var controller = existingController;

        var indent = _config.UseLatestCSharp ? "    " : "        ";

        foreach (var endpoint in file.Endpoints.Where(endpoint => endpoint.ModelFile == file || !_files.ContainsKey(endpoint.ModelFile.Name)))
        {
            var method = (MethodDeclarationSyntax)ParseMemberDeclaration($@"
{indent}/// <summary>
{indent}/// {endpoint.Description}
{indent}/// </summary>{string.Join(Environment.NewLine, new[] { string.Empty }.Concat(endpoint.Params.Select(param => $@"{indent}/// <param name=""{param.GetParamName()}"">{param.Comment}</param>")))}
{indent}/// <returns>{(endpoint.Returns != null ? endpoint.Returns.Comment : "Task.")}</returns>
{indent}[Http{endpoint.Method.ToLower().ToFirstUpper()}(""{GetRoute(endpoint)}"")]
{indent}public {_config.GetReturnTypeName(endpoint.Returns)} {endpoint.Name}({string.Join(", ", endpoint.Params.Select(GetParam))})
{indent}{{

{indent}}}
")!;

            var existingMethod = controller.DescendantNodes().OfType<MethodDeclarationSyntax>().SingleOrDefault(method => method.Identifier.Text == endpoint.Name);
            if (existingMethod != null)
            {
                method = method.WithBody(existingMethod.Body);
                controller = controller.ReplaceNode(existingMethod, method);
            }
            else
            {
                var index = file.Endpoints.IndexOf(endpoint);
                var firstMethod = controller.Members.OfType<MethodDeclarationSyntax>().FirstOrDefault();
                var start = firstMethod != null ? controller.Members.IndexOf(firstMethod) : 0;
                controller = controller.WithMembers(List(controller.Members.Take(start + index).Concat(new[] { method }).Concat(controller.Members.Skip(start + index))));
            }
        }

        foreach (var method in controller.DescendantNodes().OfType<MethodDeclarationSyntax>())
        {
            if (method.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword)) && !file.Endpoints.Where(endpoint => endpoint.ModelFile == file || !_files.ContainsKey(endpoint.ModelFile.Name)).Any(endpoint => endpoint.Name == method.Identifier.Text))
            {
                controller = controller.WithMembers(List(controller.Members.Where(member => ((member as MethodDeclarationSyntax)?.Identifier.Text ?? string.Empty) != method.Identifier.Text)));
            }
        }

        using var fw = new FileWriter(filePath, _logger, true) { HeaderMessage = "ATTENTION, CE FICHIER EST PARTIELLEMENT GENERE AUTOMATIQUEMENT !" };
        fw.Write(syntaxTree.GetRoot().ReplaceNode(existingController, controller).ToString());
    }

    private string GetRoute(Endpoint endpoint)
    {
        var split = endpoint.Route.Split("/");

        for (var i = 0; i < split.Length; i++)
        {
            if (split[i].StartsWith("{"))
            {
                var routeParamName = split[i][1..^1];
                var param = endpoint.Params.OfType<IFieldProperty>().SingleOrDefault(param => param.GetParamName() == routeParamName);

                if (param == null)
                {
                    throw new ModelException(endpoint.ModelFile, $"Le endpoint '{endpoint.Name}' définit un paramètre '{routeParamName}' dans sa route qui n'existe pas dans la liste des paramètres.");
                }

                var paramType = param.Domain.CSharp!.Type switch
                {
                    "int" => "int",
                    "int?" => "int",
                    "Guid" => "guid",
                    "Guid?" => "guid",
                    _ => null
                };
                if (paramType != null)
                {
                    split[i] = $"{{{routeParamName}:{paramType}}}";
                }
            }
        }

        return string.Join("/", split);
    }

    private string GetParam(IProperty param)
    {
        var sb = new StringBuilder();

        var hasForm = param.Endpoint.Params.Any(p => p is IFieldProperty { Domain.CSharp.Type: "IFormFile" });

        if (param.IsBodyParam())
        {
            if (hasForm)
            {
                sb.Append("[FromForm] ");
            }
            else
            {
                sb.Append("[FromBody] ");
            }
        }

        sb.Append($@"{_config.GetPropertyTypeName(param, param.IsRouteParam())} {param.GetParamName()}");

        if (param.IsQueryParam() && !hasForm)
        {
            sb.Append(" = null");
        }

        return sb.ToString();
    }
}