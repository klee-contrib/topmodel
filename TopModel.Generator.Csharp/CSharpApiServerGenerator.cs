using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Generator.Core;
using TopModel.Utils;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace TopModel.Generator.Csharp;

public class CSharpApiServerGenerator : EndpointsGeneratorBase<CsharpConfig>
{
    private readonly ILogger<CSharpApiServerGenerator> _logger;

    public CSharpApiServerGenerator(ILogger<CSharpApiServerGenerator> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override string Name => "CSharpApiServerGen";

    protected override bool FilterTag(string tag)
    {
        return Config.ResolveVariables(Config.ApiGeneration!, tag) == ApiGeneration.Server;
    }

    protected override string GetFileName(ModelFile file, string tag)
    {
        return Path.Combine(Config.GetApiPath(file, tag, withControllers: true), $"{file.Options.Endpoints.FileName.ToPascalCase()}Controller.cs");
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        var className = $"{fileName.ToPascalCase()}Controller";
        var ns = Config.GetNamespace(endpoints.First(), tag);

        var text = File.Exists(filePath)
            ? File.ReadAllText(filePath)
            : Config.UseLatestCSharp
            ? $@"using Microsoft.AspNetCore.Mvc;

namespace {ns};

public class {className} : Controller
{{

}}"
            : $@"using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace {ns}
{{
    public class {className} : Controller
    {{

    }}
}}";

        var syntaxTree = CSharpSyntaxTree.ParseText(text);
        var existingController = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();

        var controller = existingController;

        var indent = Config.UseLatestCSharp ? "    " : "        ";

        foreach (var endpoint in endpoints)
        {
            var wd = new StringBuilder();
            wd.AppendLine();
            wd.AppendLine($"{indent}/// <summary>");
            wd.AppendLine($"{indent}/// {endpoint.Description}");
            wd.AppendLine($"{indent}/// </summary>");

            foreach (var param in endpoint.Params)
            {
                wd.AppendLine($@"{indent}/// <param name=""{param.GetParamName()}"">{param.Comment}</param>");
            }

            if (!Config.NoAsyncControllers || endpoint.Returns != null)
            {
                wd.AppendLine($"{indent}/// <returns>{(endpoint.Returns != null ? endpoint.Returns.Comment : "Task.")}</returns>");
            }

            if (endpoint.Returns is IFieldProperty { Domain.MediaType: string mediaType })
            {
                wd.AppendLine($@"{indent}[Produces(""{mediaType}"")]");
            }

            foreach (var d in endpoint.Decorators)
            {
                foreach (var a in Config.GetImplementation(d.Decorator)?.Annotations ?? Array.Empty<string>())
                {
                    wd.AppendLine($"{indent}[{a.ParseTemplate(endpoint, d.Parameters)}]");
                }
            }

            wd.AppendLine($@"{indent}[Http{endpoint.Method.ToPascalCaseStrict()}(""{GetRoute(endpoint)}"")]");
            wd.AppendLine($"{indent}public {Config.GetReturnTypeName(endpoint.Returns)} {endpoint.NamePascal}({string.Join(", ", endpoint.Params.Select(GetParam))})");
            wd.AppendLine($"{indent}{{");
            wd.AppendLine();
            wd.AppendLine($"{indent}}}");

            var method = (MethodDeclarationSyntax)ParseMemberDeclaration(wd.ToString())!;

            var existingMethod = controller.DescendantNodes().OfType<MethodDeclarationSyntax>().SingleOrDefault(method => method.Identifier.Text == endpoint.NamePascal);
            if (existingMethod != null)
            {
                method = method.WithBody(existingMethod.Body);
                controller = controller.ReplaceNode(existingMethod, method);
            }
            else
            {
                var index = endpoints.IndexOf(endpoint);
                var firstMethod = controller.Members.OfType<MethodDeclarationSyntax>().FirstOrDefault();
                var start = firstMethod != null ? controller.Members.IndexOf(firstMethod) : 0;
                controller = controller.WithMembers(List(controller.Members.Take(start + index).Concat(new[] { method }).Concat(controller.Members.Skip(start + index))));
            }
        }

        foreach (var method in controller.DescendantNodes().OfType<MethodDeclarationSyntax>())
        {
            if (method.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword)) && !endpoints.Any(endpoint => endpoint.NamePascal == method.Identifier.Text))
            {
                controller = controller.WithMembers(List(controller.Members.Where(member => ((member as MethodDeclarationSyntax)?.Identifier.Text ?? string.Empty) != method.Identifier.Text)));
            }
        }

        using var fw = new FileWriter(filePath, _logger, true) { HeaderMessage = "ATTENTION, CE FICHIER EST PARTIELLEMENT GENERE AUTOMATIQUEMENT !" };
        fw.Write(syntaxTree.GetRoot().ReplaceNode(existingController, controller).ToString());
    }

    private string GetParam(IProperty param)
    {
        var sb = new StringBuilder();

        var hasForm = param.Endpoint.Params.Any(p => Config.GetType(p).Contains("IFormFile"));

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

        sb.Append($@"{Config.GetType(param, nonNullable: param.IsRouteParam() || param.IsQueryParam() && !hasForm && Config.GetDefaultValue(param, Classes) != "null")} {param.GetParamName().Verbatim()}");

        if (param.IsQueryParam() && !hasForm)
        {
            sb.Append($" = {Config.GetDefaultValue(param, Classes)}");
        }

        return sb.ToString();
    }

    private string GetRoute(Endpoint endpoint)
    {
        var split = endpoint.FullRoute.Split("/");

        for (var i = 0; i < split.Length; i++)
        {
            if (split[i].StartsWith("{"))
            {
                var routeParamName = split[i][1..^1];
                var param = endpoint.Params.OfType<IFieldProperty>().Single(param => param.GetParamName() == routeParamName);

                var paramType = Config.GetType(param) switch
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
}