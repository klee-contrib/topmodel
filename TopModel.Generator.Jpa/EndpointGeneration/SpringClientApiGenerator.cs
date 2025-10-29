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

    protected static string GetClassName(string fileName)
    {
        return $"{fileName.ToPascalCase()}Client";
    }

    protected override bool FilterTag(string tag)
    {
        return Config.ResolveVariables(Config.ApiGeneration!, tag) == ApiGeneration.Client;
    }

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Path.Combine(Config.GetApiPath(file, tag), $"{GetClassName(file.Options.Endpoints.FileName)}.java");
    }

    protected virtual IEnumerable<string> GetTypeImports(IEnumerable<Endpoint> endpoints, string tag)
    {
        var properties = endpoints
            .SelectMany(endpoint => endpoint.Params)
            .Concat(endpoints.Where(endpoint => endpoint.Returns is not null).Select(endpoint => endpoint.Returns));
        return properties
            .SelectMany(property => property!.GetTypeImports(Config, tag))
            .Concat(
                endpoints
                    .Where(endpoint => endpoint.Returns is not null)
                    .Select(e => e.Returns)
                    .OfType<CompositionProperty>()
                    .SelectMany(c => c.GetKindImports(Config, tag))
            );
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        var className = GetClassName(fileName);
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

        if (endpoint.Returns != null)
        {
            returnType = Config.GetType(endpoint.Returns);
        }
        var method = new JavaMethod(
            "org.springframework.http.ResponseEntity",
            @$"ResponseEntity<{returnType}>",
            endpoint.NameCamel
        )
        {
            Comment = endpoint.Description,
            ReturnComment = endpoint.Returns != null ? endpoint.Returns.Comment : "Aucun retour",
        };
        var javaAnnotations = Config
            .GetAnnotations(endpoint, tag)
            .Select(a => new JavaAnnotation(a.Annotation, imports: a.Imports.ToArray()));
        method.AddAnnotations(javaAnnotations);

        var exchangeAnnotation = new JavaAnnotation(
            $"{endpoint.Method.ToPascalCase(strict: true)}Exchange",
            imports: $"org.springframework.web.service.annotation.{endpoint.Method.ToPascalCase(strict: true)}Exchange"
        ).AddAttribute($@"""{endpoint.Route}""");
        if (endpoint.Returns != null && endpoint.Returns.Domain?.MediaType != null)
        {
            exchangeAnnotation.AddAttribute("accept", $@"{{ ""{endpoint.Returns.Domain.MediaType}"" }}");
        }

        if (endpoint.Params.Any(p => p.Domain?.MediaType != null))
        {
            exchangeAnnotation.AddAttribute(
                "contentType",
                string.Join(
                    ", ",
                    endpoint
                        .Params.Where(p => p.Domain?.MediaType != null)
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

        foreach (var param in endpoint.GetQueryParams())
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
                var param in endpoint.Params.Where(param =>
                    param is CompositionProperty
                    || (param.Domain?.BodyParam ?? false)
                    || (param.Domain?.IsMultipart ?? false)
                )
            )
            {
                if (param is CompositionProperty)
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
            var bodyParam = endpoint.GetJsonBodyParam();
            if (bodyParam != null)
            {
                var validAnnotation = new JavaAnnotation("Valid", imports: $"{Config.JavaxOrJakarta}.validation.Valid");
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
