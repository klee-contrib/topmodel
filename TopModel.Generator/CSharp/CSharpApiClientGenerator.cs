using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopModel.Core.FileModel;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.CSharp
{
    public class CSharpApiClientGenerator : GeneratorBase
    {
        private readonly CSharpConfig _config;
        private readonly ILogger<CSharpApiClientGenerator> _logger;

        public CSharpApiClientGenerator(ILogger<CSharpApiClientGenerator> logger, CSharpConfig config)
            : base(logger, config)
        {
            _config = config;
            _logger = logger;
        }

        public override string Name => "CSharpApiClientGen";

        protected override void HandleFiles(IEnumerable<ModelFile> files)
        {
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
            var className = $"{file.Module}Client";
            var apiPath = _config.ApiPath.Replace("{app}", file.Endpoints.First().Namespace.App).Replace("{module}", file.Module);
            var filePath = $"{_config.OutputDirectory}/{apiPath}{path}/generated/{className}.cs";

            using var fw = new CSharpWriter(filePath, _logger);

            var usings = new List<string> { "System.Net.Http", "System.Text", "System.Text.Json", "System.Threading.Tasks" };

            foreach (var property in file.Endpoints.SelectMany(e => e.Params.Concat(new[] { e.Returns }).Where(p => p != null)))
            {
                if (property is IFieldProperty fp)
                {
                    foreach (var @using in fp.Domain.CSharp!.Usings)
                    {
                        usings.Add(@using);
                    }
                }

                switch (property)
                {
                    case AssociationProperty ap when !ap.AsAlias:
                        usings.Add(_config.GetNamespace(ap.Association));
                        break;
                    case AliasProperty { Property: AssociationProperty ap2 }:
                        usings.Add(_config.GetNamespace(ap2.Association));
                        break;
                    case AliasProperty { PrimaryKey: false, Property: RegularProperty { PrimaryKey: true } rp }:
                        usings.Add(_config.GetNamespace(rp.Class));
                        break;
                    case CompositionProperty cp:
                        usings.Add(_config.GetNamespace(cp.Composition));
                        if (cp.DomainKind != null)
                        {
                            usings.AddRange(cp.DomainKind.CSharp!.Usings);
                        }

                        break;
                }
            }

            fw.WriteUsings(usings.Distinct().ToArray());
            fw.WriteLine();

            fw.WriteNamespace(apiPath.Replace("/", "."));

            fw.WriteSummary(1, $"Client {file.Module}");
            fw.WriteClassDeclaration(className, null);

            fw.WriteLine(2, "private readonly HttpClient _client;");
            fw.WriteLine();

            fw.WriteSummary(2, "Constructeur");
            fw.WriteParam("client", "HttpClient injecté.");
            fw.WriteLine(2, $"public {className}(HttpClient client)");
            fw.WriteLine(2, "{");
            fw.WriteLine(3, "_client = client;");
            fw.WriteLine(2, "}");

            foreach (var endpoint in file.Endpoints)
            {
                fw.WriteLine();
                fw.WriteSummary(2, endpoint.Description);

                foreach (var param in endpoint.Params)
                {
                    fw.WriteParam(param.Name.ToFirstLower(), param.Comment);
                }

                if (endpoint.Returns != null)
                {
                    fw.WriteReturns(2, endpoint.Returns.Comment);
                }

                fw.Write("        public async Task");

                if (endpoint.Returns != null)
                {
                    fw.Write($"<{GetPropertyTypeName(endpoint.Returns)}>");
                }

                fw.Write($" {endpoint.Name}(");

                foreach (var param in endpoint.Params)
                {
                    fw.Write($"{GetPropertyTypeName(param)} {param.Name.ToFirstLower()}");
                    if (endpoint.Params.Last() != param)
                    {
                        fw.Write(", ");
                    }
                }

                fw.WriteLine(")");
                fw.WriteLine(2, "{");

                var bodyParam = endpoint.GetBodyParam();
                fw.WriteLine(3, $"var res = await _client.{endpoint.Method.ToLower().ToFirstUpper()}Async($\"{endpoint.Route}\"{(bodyParam != null ? $", GetBody({bodyParam.Name})" : string.Empty)});");

                fw.WriteLine(3, $"await HandleErrors(res);");

                if (endpoint.Returns != null)
                {
                    fw.WriteLine(3, $"return await Deserialize<{GetPropertyTypeName(endpoint.Returns)}>(res);");
                }

                fw.WriteLine(2, "}");
            }

            fw.WriteLine();
            fw.WriteSummary(2, "Déserialize le contenu d'une réponse HTTP.");
            fw.WriteTypeParam("T", "Type de destination.");
            fw.WriteParam("response", "Réponse HTTP");
            fw.WriteReturns(2, "Contenu.");
            fw.WriteLine(2, "private async Task<T> Deserialize<T>(HttpResponseMessage response)");
            fw.WriteLine(2, "{");
            fw.WriteLine(3, "return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { IgnoreNullValues = true });");
            fw.WriteLine(2, "}");

            fw.WriteLine();
            fw.WriteSummary(2, "Récupère le body d'une requête pour l'objet donné.");
            fw.WriteTypeParam("T", "Type source.");
            fw.WriteParam("input", "Entrée");
            fw.WriteReturns(2, "Contenu.");
            fw.WriteLine(2, "private StringContent GetBody<T>(T input)");
            fw.WriteLine(2, "{");
            fw.WriteLine(3, "return new StringContent(JsonSerializer.Serialize(input, new JsonSerializerOptions { IgnoreNullValues = true }), Encoding.UTF8, \"application/json\");");
            fw.WriteLine(2, "}");

            fw.WriteLine();
            fw.WriteSummary(2, "Gère les erreurs éventuelles retournées par l'API appelée.");
            fw.WriteParam("response", "Réponse HTTP");
            fw.WriteLine(2, "private partial Task HandleErrors(HttpResponseMessage response);");

            fw.WriteLine(1, "}");
            fw.WriteLine("}");
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
                        throw new Exception($"Le endpoint '{endpoint.Name}' définit un paramètre '{routeParamName}' dans sa route qui n'existe pas dans la liste des paramètres.");
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
