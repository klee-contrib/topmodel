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
public class SpringServerApiGenerator(ILogger<SpringServerApiGenerator> logger, IFileWriterProvider writerProvider)
    : EndpointsGeneratorBase<JpaConfig>(logger, writerProvider)
{
    public override string Name => "SpringApiServerGen";
    private static string DefaultApiClassName => "{fileName}Controller";

    protected override bool FilterTag(string tag)
    {
        return Config.GetApiGenerationMode(tag) == ApiGenerationMode.Server;
    }

    protected virtual JavaAnnotation GetBodyParamNotMultipartAnnotation()
    {
        return new("ModelAttribute", imports: "org.springframework.web.bind.annotation.ModelAttribute");
    }

    protected virtual IEnumerable<JavaAnnotation> GetClassAnnotations(ModelFile file, string tag)
    {
        if (Config.GeneratedHint)
        {
            yield return Config.GeneratedAnnotation;
        }

        if (file.Options.Endpoints.Prefix != null)
        {
            yield return new JavaAnnotation(
                "RequestMapping",
                $@"""{file.Options.Endpoints.Prefix}""",
                "org.springframework.web.bind.annotation.RequestMapping"
            );
        }
    }

    protected virtual string GetClassName(string fileName, string tag)
    {
        return Config.GetApiClassName(DefaultApiClassName, fileName, tag);
    }

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Path.Combine(Config.GetApiPath(file, tag), $"{GetClassName(file.Options.Endpoints.FileName, tag)}.java");
    }

    protected virtual JavaMethod GetMethod(Endpoint endpoint, string tag)
    {
        var returnType = "void";

        var returns = Config.GetReturns(endpoint);

        if (returns != null)
        {
            returnType = Config.GetType(returns);
        }

        var method = new JavaMethod(returnType, endpoint.NameCamel) { Comment = endpoint.Description };

        if (returns != null)
        {
            method.ReturnComment = returns.Comment;
            method.AddImports(returns.GetTypeImports(Config, tag));
        }

        var mappingAnnotation = new JavaAnnotation(
            $@"@{endpoint.Method.ToPascalCase(strict: true)}Mapping",
            imports: $"org.springframework.web.bind.annotation.{endpoint.Method.ToPascalCase(strict: true)}Mapping"
        ).AddAttribute("path", $@"""{GetRoute(endpoint)}""");
        if (returns != null && returns.Domain?.MediaType != null)
        {
            mappingAnnotation.AddAttribute("produces", @$"""{returns.Domain.MediaType}""");
        }

        if (Config.GetParams(endpoint).Any(p => p.Domain?.MediaType != null))
        {
            mappingAnnotation.AddAttribute(
                "consumes",
                @$"{{ {string.Join(", ", Config.GetParams(endpoint).Where(p => p.Domain?.MediaType != null).Select(p => $@"""{p.Domain.MediaType}"""))} }}"
            );
        }

        foreach (var (annotation, imports) in Config.GetAnnotations(endpoint, tag))
        {
            method.AddAnnotation(new JavaAnnotation(annotation, imports.ToArray()));
        }

        method.AddAnnotation(mappingAnnotation);
        foreach (var param in Config.GetParams(endpoint))
        {
            if (param.ParamLocation == ParamLocation.Route)
            {
                method.AddParameter(GetRouteParam(tag, param));
            }
            else if (param.ParamLocation == ParamLocation.Query)
            {
                method.AddParameter(GetQueryParam(tag, param));
            }
            else
            {
                method.AddParameter(GetBodyParam(tag, param));
            }
        }

        if (method.ReturnType == "void" || method.ReturnType == "Void")
        {
            method.AddAnnotation(
                new JavaAnnotation(
                    "ResponseStatus",
                    imports:
                    [
                        "org.springframework.web.bind.annotation.ResponseStatus",
                        "org.springframework.http.HttpStatus",
                    ],
                    value: "HttpStatus.NO_CONTENT"
                )
            );
        }

        if (Config.OpenApiAnnotations)
        {
            var operationAnnotation = new JavaAnnotation(
                "Operation",
                imports: "io.swagger.v3.oas.annotations.Operation"
            ).AddAttribute("description", @$"""{endpoint.Description}""");
            method.AddAnnotation(operationAnnotation);
        }

        return method;
    }

    protected virtual IEnumerable<JavaMethod> GetMethods(IEnumerable<Endpoint> endpoints, string tag)
    {
        foreach (var endpoint in endpoints)
        {
            yield return GetMethod(endpoint, tag);
        }
    }

    protected virtual string GetRoute(Endpoint endpoint)
    {
        return endpoint.Route.Trim('/');
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

        var javaInterface = new JavaClass(className) { ClassType = "interface", Package = packageName };
        var annotations = GetClassAnnotations(endpoints[0].ModelFile, tag);
        javaInterface.AddRange(annotations);
        javaInterface.AddRange(GetMethods(endpoints, tag));
        fw.Write(javaInterface);
    }

    private JavaMethodParameter GetBodyParam(string tag, IProperty bodyParam)
    {
        var parameter = new JavaMethodParameter(Config.GetType(bodyParam), bodyParam.GetParamName());
        if (bodyParam.Endpoint!.IsMultipart)
        {
            if (bodyParam.Composition != null)
            {
                parameter.Add(GetBodyParamNotMultipartAnnotation());
                parameter.Add(new JavaAnnotation("Valid", imports: "jakarta.validation.Valid"));
            }
            else
            {
                parameter.Add(
                    new JavaAnnotation(
                        "RequestPart",
                        @$"""{bodyParam.GetParamName()}""",
                        "org.springframework.web.bind.annotation.RequestPart"
                    ).AddAttribute("required", bodyParam.Required.ToString().ToFirstLower())
                );
            }

            parameter.Imports.AddRange(bodyParam.GetTypeImports(Config, tag));
        }
        else
        {
            var annotation = new JavaAnnotation(
                "RequestBody",
                imports: "org.springframework.web.bind.annotation.RequestBody"
            );
            parameter.Add(new JavaAnnotation("Valid", imports: "jakarta.validation.Valid"));
            parameter.Add(annotation);
            parameter.Comment = bodyParam.Comment;
            parameter.Imports.AddRange(bodyParam.GetTypeImports(Config, tag));
            foreach (var (a, i) in Config.GetAnnotations(bodyParam, tag))
            {
                parameter.Add(new JavaAnnotation(a, imports: i.ToArray()));
            }
        }

        if (Config.OpenApiAnnotations)
        {
            parameter.Add(
                new JavaAnnotation("io.swagger.v3.oas.annotations.parameters.RequestBody").AddAttribute(
                    "description",
                    @$"""{bodyParam.Comment}"""
                )
            );
        }

        return parameter;
    }

    private JavaMethodParameter GetQueryParam(string tag, IProperty queryParam)
    {
        var param = new JavaMethodParameter(Config.GetType(queryParam), queryParam.GetParamName());
        var queryParamAnnotation = new JavaAnnotation(
            "RequestParam",
            imports: "org.springframework.web.bind.annotation.RequestParam",
            value: @$"""{queryParam.GetParamName()}"""
        ).AddAttribute("required", queryParam.Required.ToString().ToFirstLower());
        param.Add(queryParamAnnotation);
        param.Comment = queryParam.Comment;
        param.Imports.AddRange(queryParam.GetTypeImports(Config, tag));
        foreach (var (a, i) in Config.GetAnnotations(queryParam, tag))
        {
            param.Add(new JavaAnnotation(a, imports: i.ToArray()));
        }

        if (Config.OpenApiAnnotations)
        {
            param.Add(
                new JavaAnnotation("Parameter", imports: "io.swagger.v3.oas.annotations.Parameter").AddAttribute(
                    "description",
                    @$"""{queryParam.Comment}"""
                )
            );
        }

        return param;
    }

    private JavaMethodParameter GetRouteParam(string tag, IProperty routeParam)
    {
        var param = new JavaMethodParameter(Config.GetType(routeParam), routeParam.GetParamName());
        var pathParamAnnotation = new JavaAnnotation(
            "PathVariable",
            @$"""{routeParam.GetParamName()}""",
            "org.springframework.web.bind.annotation.PathVariable"
        );
        param.Add(pathParamAnnotation);
        param.Comment = routeParam.Comment;
        param.Imports.AddRange(routeParam.GetTypeImports(Config, tag));
        foreach (var (a, i) in Config.GetAnnotations(routeParam, tag))
        {
            param.Add(new JavaAnnotation(a, imports: i.ToArray()));
        }

        if (Config.OpenApiAnnotations)
        {
            param.Add(
                new JavaAnnotation("Parameter", imports: "io.swagger.v3.oas.annotations.Parameter").AddAttribute(
                    "description",
                    @$"""{routeParam.Comment}"""
                )
            );
        }

        return param;
    }
}
