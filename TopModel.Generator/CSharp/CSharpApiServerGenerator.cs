using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TopModel.Core.FileModel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.CSharp
{
    using static SyntaxFactory;

    public class CSharpApiServerGenerator : GeneratorBase
    {
        private readonly CSharpConfig _config;
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
            if (_config.ServerApiOutputDirectory == null)
            {
                return;
            }

            foreach (var file in files)
            {
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
            var path = $"/{string.Join("/", fileSplit.Skip(fileSplit.Length > 1 ? 1 : 0).SkipLast(1))}";
            var className = $"{fileSplit.Last()}Controller";
            var filePath = $"{_config.ServerApiOutputDirectory}{path}/{className}.cs";

            var text = File.Exists(filePath)
                ? File.ReadAllText(filePath)
                : $@"using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace {_config.ApiNamespace}
{{
    public class {className} : Controller
    {{

    }}
}}";

            var syntaxTree = CSharpSyntaxTree.ParseText(text);
            var existingController = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();

            var controller = existingController;

            foreach (var endpoint in file.Endpoints)
            {
                var method = (MethodDeclarationSyntax)ParseMemberDeclaration($@"
        /// <summary>
        /// {endpoint.Description}
        /// </summary>{string.Join(Environment.NewLine, new[] { string.Empty }.Concat(endpoint.Params.Select(param => $@"        /// <param name=""{param.GetParamName()}"">{param.Comment}</param>")))}
        /// <returns>{(endpoint.Returns != null ? endpoint.Returns.Comment : "Task.")}</returns>
        [Http{endpoint.Method.ToLower().ToFirstUpper()}(""{GetRoute(endpoint)}"")]
        public async Task{(endpoint.Returns != null ? $"<{GetPropertyTypeName(endpoint.Returns, true)}>" : string.Empty)} {endpoint.Name}({string.Join(", ", endpoint.Params.Select(GetParam))})
        {{

        }}
");

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
                if (method.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword)) && !file.Endpoints.Any(endpoint => endpoint.Name == method.Identifier.Text))
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
                    var param = endpoint.Params.OfType<IFieldProperty>().Single(param => param.GetParamName() == routeParamName);
                    var paramType = param.Domain.CSharp!.Type switch
                    {
                        "int" => "int",
                        "int?" => "int",
                        "string" => "string",
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

        private string GetPropertyTypeName(IProperty prop, bool nonNullable = false)
        {
            var type = prop switch
            {
                IFieldProperty fp => fp.Domain.CSharp?.Type ?? string.Empty,
                CompositionProperty cp => cp.Kind switch
                {
                    "object" => cp.Composition.Name,
                    "list" => $"IEnumerable<{cp.Composition.Name}>",
                    string _ => $"{cp.DomainKind!.CSharp!.Type}<{cp.Composition.Name}>"
                },
                _ => string.Empty
            };

            return nonNullable && type.EndsWith("?") ? type[0..^1] : type;
        }

        private string GetParam(IProperty param)
        {
            var sb = new StringBuilder();
            if (param.IsBodyParam())
            {
                sb.Append("[FromBody] ");
            }

            sb.Append($@"{GetPropertyTypeName(param, param.IsRouteParam())} {param.GetParamName()}");

            if (param.IsQueryParam())
            {
                sb.Append(" = null");
            }

            return sb.ToString();
        }
    }
}
