using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using TopModel.Utils;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace TopModel.Generator.Csharp;

public class CSharpApiServerGenerator(ILogger<CSharpApiServerGenerator> logger, IFileWriterProvider writerProvider)
    : EndpointsGeneratorBase<CsharpConfig>(logger, writerProvider)
{
    public override string Name => "CSharpApiServerGen";

    protected override bool FilterTag(string tag)
    {
        return Config.GetApiGenerationMode(tag) == ApiGenerationMode.Server;
    }

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Path.Combine(
            Config.GetApiPath(file, tag, withControllers: true),
            $"{file.Options.Endpoints.FileName.ToPascalCase()}Controller.cs"
        );
    }

    protected virtual string GetParam(IProperty param, string tag)
    {
        var sb = new StringBuilder();

        var defaultValue = Config.GetDefaultValue(param, tag);

        var isFormParam = param.Endpoint.IsMultipart && !param.IsQueryParam(Config) && !param.IsRouteParam();

        var type = Config.GetType(
            param,
            nonNullable: param.Required && !isFormParam && !param.IsQueryParam(Config)
                || param.IsRouteParam()
                || defaultValue != "null"
        );

        var hasAnnotation = false;

        if (isFormParam && !type.StartsWith("IFormFile"))
        {
            sb.Append("[FromForm]");
            hasAnnotation = true;
        }
        else if (param.IsJsonBodyParam(Config))
        {
            sb.Append("[FromBody]");
            hasAnnotation = true;
        }
        else if (type.EndsWith("[]"))
        {
            sb.Append("[FromQuery]");
            hasAnnotation = true;
        }

        if (param.Required && defaultValue == "null" && (param.IsQueryParam(Config) || isFormParam))
        {
            sb.Append("[Required]");
            hasAnnotation = true;
        }

        if (hasAnnotation)
        {
            sb.Append(' ');
        }

        sb.Append($@"{type} {param.GetParamName().Verbatim()}");

        if (param.IsQueryParam(Config) || isFormParam)
        {
            sb.Append($" = {defaultValue}");
        }

        return sb.ToString();
    }

    protected virtual string GetRoute(Endpoint endpoint)
    {
        var split = endpoint.FullRoute.Split("/");

        for (var i = 0; i < split.Length; i++)
        {
            if (split[i].StartsWith('{'))
            {
                var routeParamName = split[i][1..^1];
                var param = Config.GetParams(endpoint).SingleOrDefault(param => param.GetParamName() == routeParamName);

                var paramType =
                    param != null
                        ? Config.GetType(param) switch
                        {
                            "int" => "int",
                            "int?" => "int",
                            "Guid" => "guid",
                            "Guid?" => "guid",
                            _ => null,
                        }
                        : null;
                if (paramType != null)
                {
                    split[i] = $"{{{routeParamName}:{paramType}}}";
                }
            }
        }

        return string.Join('/', split);
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        var className = $"{fileName.ToPascalCase()}Controller";
        var ns = Config.GetNamespace(endpoints[0], tag);

        var text = File.Exists(filePath)
            ? File.ReadAllText(filePath)
            : $@"using Microsoft.AspNetCore.Mvc;

namespace {ns};

public class {className} : Controller
{{

}}";

        var syntaxTree = CSharpSyntaxTree.ParseText(text, cancellationToken: default);
        var existingController = syntaxTree.GetRoot(default).DescendantNodes().OfType<ClassDeclarationSyntax>().First();

        var controller = existingController;

        var indent = "    ";

        foreach (var endpoint in endpoints)
        {
            string GetSafeVariableName(string varName)
            {
                while (Config.GetParams(endpoint).Any(p => p.NameCamel == varName))
                {
                    varName = $"_{varName}";
                }

                return varName;
            }

            var ct = GetSafeVariableName("ct");

            var wd = new StringBuilder();

            wd.AppendLine();

            wd.AppendLine($"{indent}/// <summary>");
            wd.AppendLine($"{indent}/// {endpoint.Description}");
            wd.AppendLine($"{indent}/// </summary>");

            foreach (var param in Config.GetParams(endpoint))
            {
                wd.AppendLine($@"{indent}/// <param name=""{param.GetParamName()}"">{param.Comment}</param>");
            }

            if (Config.UseCancellationTokens)
            {
                wd.AppendLine(
                    $@"{indent}/// <param name=""{ct}"">CancellationToken (HttpContext.RequestAborted).</param>"
                );
            }

            var returns = Config.GetReturns(endpoint);

            if (!Config.NoAsyncControllers || returns != null)
            {
                wd.AppendLine($"{indent}/// <returns>{(returns != null ? returns.Comment : "Task.")}</returns>");
            }

            if (returns is { Domain.MediaType: string mediaType })
            {
                wd.AppendLine($@"{indent}[Produces(""{mediaType}"")]");
            }

            foreach (var (annotation, _) in Config.GetAnnotations(endpoint, tag))
            {
                wd.AppendLine($"{indent}[{annotation}]");
            }

            wd.AppendLine($@"{indent}[Http{endpoint.Method.ToPascalCase(strict: true)}(""{GetRoute(endpoint)}"")]");
            wd.AppendLine(
                $"{indent}public {Config.GetReturnTypeName(returns)} {endpoint.NamePascal}({string.Join(", ", Config.GetParams(endpoint).Select(p => GetParam(p, tag)))}{(Config.UseCancellationTokens ? $"{(Config.GetParams(endpoint).Any() ? ", " : string.Empty)}CancellationToken {ct} = default" : string.Empty)})"
            );
            wd.AppendLine($"{indent}{{");
            wd.AppendLine();
            wd.AppendLine($"{indent}}}");

            var method = (MethodDeclarationSyntax)ParseMemberDeclaration(wd.ToString())!;

            var existingMethod = controller
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .SingleOrDefault(method => method.Identifier.Text == endpoint.NamePascal);
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
                controller = controller.WithMembers(
                    List(
                        controller
                            .Members.Take(start + index)
                            .Concat([method])
                            .Concat(controller.Members.Skip(start + index))
                    )
                );
            }
        }

        foreach (var method in controller.DescendantNodes().OfType<MethodDeclarationSyntax>())
        {
            if (
                method.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword))
                && !endpoints.Any(endpoint => endpoint.NamePascal == method.Identifier.Text)
            )
            {
                controller = controller.WithMembers(
                    List(
                        controller.Members.Where(member =>
                            ((member as MethodDeclarationSyntax)?.Identifier.Text ?? string.Empty)
                            != method.Identifier.Text
                        )
                    )
                );
            }
        }

        if (controller.OpenBraceToken.HasTrailingTrivia && !controller.Members.OfType<FieldDeclarationSyntax>().Any())
        {
            controller = controller.WithOpenBraceToken(controller.OpenBraceToken.WithoutTrivia());
        }

        using var fw = OpenFileWriter(filePath);
        fw.HeaderMessage = "ATTENTION, CE FICHIER EST PARTIELLEMENT GENERE AUTOMATIQUEMENT !";
        fw.Write(syntaxTree.GetRoot(default).ReplaceNode(existingController, controller).ToString());
    }
}
