using Microsoft.Extensions.Logging;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.EndpointGeneration;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class SpringClientApiGenerator(ILogger<SpringClientApiGenerator> logger, IFileWriterProvider writerProvider)
    : EndpointsGeneratorBase<JpaConfig>(logger, writerProvider)
{
    public override string Name => "SpringApiClientGen";
    private static string DefaultApiClassName => "{fileName}Client";

    protected override bool FilterTag(string tag)
    {
        return Config.GetApiGenerationMode(tag) == ApiGenerationMode.Client;
    }

    protected string GetClassName(string fileName, string tag)
    {
        return Config.GetApiClassName(DefaultApiClassName, fileName, tag);
    }

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Path.Combine(Config.GetApiPath(file, tag), $"{GetClassName(file.Options.Endpoints.FileName, tag)}.java");
    }

    protected virtual IEnumerable<string> GetTypeImports(IEnumerable<Endpoint> endpoints, string tag)
    {
        return endpoints.SelectMany(p => p.Properties).SelectMany(c => c.GetTypeImports(Config, tag));
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        var className = GetClassName(fileName, tag);
        var packageName = Config.GetPackageName(endpoints[0], tag);
        using var fw = this.OpenJavaWriter(filePath, packageName, codePage: null);

        WriteImports(endpoints, fw, tag);
        fw.WriteLine();
        if (endpoints[0].ModelFile.Options.Endpoints.Prefix != null)
        {
            var exchangeAnnotation = new JavaAnnotation(
                "HttpExchange",
                imports: "org.springframework.web.service.annotation.HttpExchange"
            ).AddAttribute($@"""{endpoints[0].ModelFile.Options.Endpoints.Prefix}""");
            fw.WriteLine(0, exchangeAnnotation);
        }

        if (Config.GeneratedHint)
        {
            fw.WriteLine(0, Config.GeneratedAnnotation);
        }

        fw.WriteLine($"public interface {className} {{");

        fw.WriteLine();

        foreach (var endpoint in endpoints)
        {
            WriteEndpoint(fw, endpoint, tag);
        }

        fw.WriteLine("}");
    }

    protected virtual void WriteEndpoint(JavaWriter fw, Endpoint endpoint, string tag)
    {
        var returnType = "Void";

        var returns = Config.GetReturns(endpoint);

        if (returns != null)
        {
            returnType = Config.GetType(returns);
        }
        var method = new JavaMethod(
            "org.springframework.http.ResponseEntity",
            @$"ResponseEntity<{returnType}>",
            endpoint.NameCamel
        )
        {
            Comment = endpoint.Description,
            ReturnComment = returns != null ? returns.Comment : "Aucun retour",
        };
        var javaAnnotations = Config
            .GetAnnotations(endpoint, tag)
            .Select(a => new JavaAnnotation(a.Annotation, imports: a.Imports.ToArray()));
        method.AddAnnotations(javaAnnotations);

        var exchangeAnnotation = new JavaAnnotation(
            $"{endpoint.Method.ToPascalCase(strict: true)}Exchange",
            imports: $"org.springframework.web.service.annotation.{endpoint.Method.ToPascalCase(strict: true)}Exchange"
        ).AddAttribute($@"""{endpoint.Route}""");
        if (returns != null && returns.Domain?.MediaType != null)
        {
            exchangeAnnotation.AddAttribute("accept", $@"{{ ""{returns.Domain.MediaType}"" }}");
        }

        if (Config.GetParams(endpoint).Any(p => p.Domain?.MediaType != null))
        {
            exchangeAnnotation.AddAttribute(
                "contentType",
                string.Join(
                    ", ",
                    Config
                        .GetParams(endpoint)
                        .Where(p => p.Domain?.MediaType != null)
                        .Select(p => $@"""{p.Domain.MediaType}""")
                        .First()
                )
            );
        }

        method.AddAnnotation(exchangeAnnotation);

        foreach (var param in endpoint.GetRouteParams())
        {
            var pathParamAnnotation = new JavaAnnotation(
                "PathVariable",
                imports: "org.springframework.web.bind.annotation.PathVariable"
            ).AddAttribute(@$"""{param.GetParamName()}""");
            var parameter = new JavaMethodParameter(Config.GetType(param), param.GetParamName())
            {
                Comment = param.Comment,
            }
                .AddRange(Config.GetDomainJavaAnnotations(param, tag))
                .Add(pathParamAnnotation);
            parameter.Imports.AddRange(Config.GetDomainImports(param, tag));
            method.AddParameter(parameter);
        }

        foreach (var param in endpoint.GetQueryParams(Config))
        {
            var requestParamAnnotation = new JavaAnnotation(
                "RequestParam",
                imports: "org.springframework.web.bind.annotation.RequestParam"
            )
                .AddAttribute(@$"""{param.GetParamName()}""")
                .AddAttribute("required", param.Required.ToString().ToFirstLower());
            var parameter = new JavaMethodParameter(Config.GetType(param), param.GetParamName())
            {
                Comment = param.Comment,
            }
                .AddRange(Config.GetDomainJavaAnnotations(param, tag))
                .Add(requestParamAnnotation);
            method.AddParameter(parameter);
        }

        if (endpoint.IsMultipart)
        {
            foreach (
                var param in Config
                    .GetParams(endpoint)
                    .Where(param =>
                        param is { Composition: not null }
                        || (param.Domain?.BodyParam ?? false)
                        || (param.Domain?.IsMultipart ?? false)
                    )
            )
            {
                if (param is { Composition: not null })
                {
                    var requestPartAnnotation = new JavaAnnotation(
                        "RequestPart",
                        imports: "org.springframework.web.bind.annotation.RequestPart"
                    )
                        .AddAttribute(@$"""{param.GetParamName()}""")
                        .AddAttribute("required", param.Required.ToString().ToFirstLower());

                    var parameter = new JavaMethodParameter(
                        "org.springframework.util.MultiValueMap",
                        "MultiValueMap<K, V>",
                        param.GetParamName()
                    )
                    {
                        Comment = param.Comment,
                    }
                        .AddRange(Config.GetDomainJavaAnnotations(param, tag))
                        .Add(requestPartAnnotation);
                    method.AddParameter(parameter).AddGenericType("K").AddGenericType("V");
                }
                else
                {
                    var requestPartAnnotation = new JavaAnnotation(
                        "RequestPart",
                        imports: "org.springframework.web.bind.annotation.RequestPart"
                    )
                        .AddAttribute(@$"""{param.Name}""")
                        .AddAttribute("required", param.Required.ToString().ToFirstLower());

                    var parameter = new JavaMethodParameter(Config.GetType(param), param.GetParamName())
                    {
                        Comment = param.Comment,
                    }
                        .AddRange(Config.GetDomainJavaAnnotations(param, tag))
                        .Add(requestPartAnnotation);
                    method.AddParameter(parameter);
                }
            }
        }
        else
        {
            var bodyParam = endpoint.GetJsonBodyParam(Config);
            if (bodyParam != null)
            {
                var validAnnotation = new JavaAnnotation("Valid", imports: "jakarta.validation.Valid");
                var requestBodyAnnotation = new JavaAnnotation(
                    "RequestBody",
                    imports: "org.springframework.web.bind.annotation.RequestBody"
                );
                var parameter = new JavaMethodParameter(Config.GetType(bodyParam), bodyParam.GetParamName())
                {
                    Comment = bodyParam.Comment,
                }
                    .Add(requestBodyAnnotation)
                    .Add(validAnnotation);
                method.AddParameter(parameter);
            }
        }

        fw.Write(1, method);
    }

    protected virtual void WriteImports(IEnumerable<Endpoint> endpoints, JavaWriter fw, string tag)
    {
        fw.AddImports(GetTypeImports(endpoints, tag));
        fw.AddImports(endpoints.SelectMany(e => Config.GetDecoratorImports(e, tag)));
    }
}
