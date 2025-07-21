using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
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

    protected virtual void AddImports(IEnumerable<Endpoint> endpoints, JavaWriter fw, string tag)
    {
        fw.AddImports(GetTypeImports(endpoints, tag));
        fw.AddImports(endpoints.SelectMany(e => Config.GetDecoratorImports(e, tag)));
    }

    protected override bool FilterTag(string tag)
    {
        return Config.ResolveVariables(Config.ApiGeneration!, tag) == ApiGeneration.Server;
    }

    protected virtual IEnumerable<JavaAnnotation> GetClassAnnotations(ModelFile file)
    {
        if (Config.GeneratedHint)
        {
            yield return Config.GeneratedAnnotation;
        }

        if (file.Options.Endpoints.Prefix != null)
        {
            yield return new JavaAnnotation("RequestMapping", $@"""{file.Options.Endpoints.Prefix}""", "org.springframework.web.bind.annotation.RequestMapping");
        }
    }

    protected virtual string GetClassName(string fileName)
    {
        return $"{fileName.ToPascalCase()}Controller";
    }

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Path.Combine(Config.GetApiPath(file, tag), $"{GetClassName(file.Options.Endpoints.FileName)}.java");
    }

    protected virtual JavaMethod GetMethod(Endpoint endpoint, string tag)
    {
        var returnType = "void";

        if (endpoint.Returns != null)
        {
            returnType = Config.GetType(endpoint.Returns);
        }

        var method = new JavaMethod(returnType, endpoint.NameCamel)
        {
            Comment = endpoint.Description
        };

        if (endpoint.Returns != null)
        {
            method.ReturnComment = endpoint.Returns.Comment;
            method.Imports.AddRange(endpoint.Returns.GetTypeImports(Config, tag));
        }

        var mappingAnnotation = new JavaAnnotation($@"@{endpoint.Method.ToPascalCase(true)}Mapping", imports: $"org.springframework.web.bind.annotation.{endpoint.Method.ToPascalCase(true)}Mapping")
            .AddAttribute("path", $@"""{endpoint.Route.Trim('/')}""");
        if (endpoint.Returns != null && endpoint.Returns.Domain?.MediaType != null)
        {
            mappingAnnotation.AddAttribute("produces", @$"""{endpoint.Returns.Domain.MediaType}""");
        }

        var consumes = string.Empty;
        if (endpoint.Params.Any(p => p.Domain?.MediaType != null))
        {
            mappingAnnotation.AddAttribute("consumes", @$"{{ {string.Join(", ", endpoint.Params.Where(p => p.Domain?.MediaType != null).Select(p => $@"""{p.Domain.MediaType}"""))} }}");
        }

        foreach (var annotation in Config.GetDecoratorAnnotations(endpoint, tag))
        {
            var imports = Config.GetDecoratorImports(endpoint, tag).ToArray();
            method.AddAnnotation(new JavaAnnotation(annotation, imports));
        }

        method.AddAnnotation(mappingAnnotation);

        foreach (var routeParam in endpoint.GetRouteParams())
        {
            var param = new JavaMethodParameter(Config.GetType(routeParam), routeParam.GetParamName());
            var pathParamAnnotation = new JavaAnnotation("PathVariable", imports: "org.springframework.web.bind.annotation.PathVariable", value: @$"""{routeParam.GetParamName()}""");
            param.AddAnnotation(pathParamAnnotation);
            param.Comment = routeParam.Comment;
            param.Imports.AddRange(routeParam.GetTypeImports(Config, tag));
            foreach (var (a, i) in Config.GetDomainAnnotations(routeParam, tag))
            {
                param.AddAnnotation(new JavaAnnotation(a, imports: i.ToArray()));
            }

            if (Config.OpenApiAnnotations)
            {
                param.AddAnnotation(new JavaAnnotation("Parameter", imports: ["io.swagger.v3.oas.annotations.Parameter"])
                    .AddAttribute("description", @$"""{routeParam.Comment}"""));
            }

            method.AddParameter(param);
        }

        foreach (var queryParam in endpoint.GetQueryParams())
        {
            var ann = string.Empty;
            var param = new JavaMethodParameter(Config.GetType(queryParam), queryParam.GetParamName());
            var queryParamAnnotation = new JavaAnnotation("RequestParam", imports: "org.springframework.web.bind.annotation.RequestParam", value: @$"""{queryParam.GetParamName()}""")
                .AddAttribute("required", queryParam.Required.ToString().ToFirstLower());
            param.AddAnnotation(queryParamAnnotation);
            param.Comment = queryParam.Comment;
            param.Imports.AddRange(queryParam.GetTypeImports(Config, tag));
            foreach (var (a, i) in Config.GetDomainAnnotations(queryParam, tag))
            {
                param.AddAnnotation(new JavaAnnotation(a, imports: i.ToArray()));
            }

            if (Config.OpenApiAnnotations)
            {
                param.AddAnnotation(new JavaAnnotation("Parameter", imports: ["io.swagger.v3.oas.annotations.Parameter"])
                    .AddAttribute("description", @$"""{queryParam.Comment}"""));
            }

            method.AddParameter(param);
        }

        if (endpoint.IsMultipart)
        {
            foreach (var param in endpoint.Params.Where(param => param is CompositionProperty || (param.Domain?.BodyParam ?? false) || (param.Domain?.IsMultipart ?? false)))
            {
                var parameter = new JavaMethodParameter(Config.GetType(param), param.GetParamName());
                parameter.Comment = parameter.Comment;
                if (!(param.Domain?.IsMultipart ?? false))
                {
                    parameter.AddAnnotation(new JavaAnnotation("ModelAttribute", imports: "org.springframework.web.bind.annotation.ModelAttribute"));
                    parameter.AddAnnotation(new JavaAnnotation("Valid", imports: Config.JavaxOrJakarta + ".validation.Valid"));
                }
                else
                {
                    parameter.AddAnnotation(new JavaAnnotation("RequestPart", imports: "org.springframework.web.bind.annotation.RequestPart", value: @$"""{param.GetParamName()}""")
                        .AddAttribute("required", param.Required.ToString().ToFirstLower()));
                }

                parameter.Imports.AddRange(param.GetTypeImports(Config, tag));
                if (Config.OpenApiAnnotations)
                {
                    parameter.AddAnnotation(new JavaAnnotation("Parameter", imports: ["io.swagger.v3.oas.annotations.Parameter"])
                        .AddAttribute("description", @$"""{param.Comment}"""));
                }

                method.AddParameter(parameter);
            }
        }
        else
        {
            var bodyParam = endpoint.GetJsonBodyParam();
            if (bodyParam != null)
            {
                var ann = string.Empty;
                var annotation = new JavaAnnotation("RequestBody", imports: "org.springframework.web.bind.annotation.RequestBody");
                var parameter = new JavaMethodParameter(Config.GetType(bodyParam), bodyParam.GetParamName());
                parameter.AddAnnotation(annotation);
                parameter.Comment = bodyParam.Comment;
                parameter.Imports.AddRange(bodyParam.GetTypeImports(Config, tag));
                foreach (var (a, i) in Config.GetDomainAnnotations(bodyParam, tag))
                {
                    parameter.AddAnnotation(new JavaAnnotation(a, imports: i.ToArray()));
                }

                parameter.AddAnnotation(new JavaAnnotation("Valid", imports: Config.JavaxOrJakarta + ".validation.Valid"));

                if (Config.OpenApiAnnotations)
                {
                    parameter.AddAnnotation(new JavaAnnotation("io.swagger.v3.oas.annotations.parameters.RequestBody")
                        .AddAttribute("description", @$"""{bodyParam.Comment}"""));
                }

                method.AddParameter(parameter);
            }
        }

        if (method.ReturnType == "void" || method.ReturnType == "Void")
        {
            method.AddAnnotation(new JavaAnnotation("ResponseStatus", imports: ["org.springframework.web.bind.annotation.ResponseStatus", "org.springframework.http.HttpStatus"], value: "HttpStatus.NO_CONTENT"));
        }

        if (Config.OpenApiAnnotations)
        {
            var operationAnnotation = new JavaAnnotation("Operation", imports: ["io.swagger.v3.oas.annotations.Operation"])
                .AddAttribute("description", @$"""{endpoint.Description}""");
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

    protected virtual IEnumerable<string> GetTypeImports(IEnumerable<Endpoint> endpoints, string tag)
    {
        var properties = endpoints.SelectMany(endpoint => endpoint.Params)
            .Concat(endpoints.Where(endpoint => endpoint.Returns is not null)
            .Select(endpoint => endpoint.Returns));
        return properties.SelectMany(property => property!.GetTypeImports(Config, tag))
                .Concat(endpoints.Where(endpoint => endpoint.Returns is not null)
                .Select(e => e.Returns).OfType<CompositionProperty>()
                .SelectMany(c => c.GetKindImports(Config, tag)));
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        var className = GetClassName(fileName);
        var packageName = Config.GetPackageName(endpoints.First(), tag);
        using var fw = this.OpenJavaWriter(filePath, packageName, null);

        var javaInterface = new JavaClass(className)
        {
            Interface = true,
            Package = packageName,
        };
        var annotations = GetClassAnnotations(endpoints.First().ModelFile);
        javaInterface.AddRange(annotations);
        javaInterface.AddRange(GetMethods(endpoints, tag));
        fw.Write(0, javaInterface);
    }

    protected virtual void WriteMethods(JavaWriter fw, IEnumerable<Endpoint> endpoints, string tag)
    {
        foreach (var method in GetMethods(endpoints, tag))
        {
            fw.Write(1, method);
        }
    }
}
