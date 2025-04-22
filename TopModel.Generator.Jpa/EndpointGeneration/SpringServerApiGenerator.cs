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

        AddImports(endpoints, fw, tag);
        fw.WriteLine();
        var javaxOrJakarta = Config.PersistenceMode.ToString().ToLower();
        fw.WriteAnnotations(0, GetClassAnnotations(endpoints.First().ModelFile));
        fw.WriteLine($"public interface {className} {{");

        foreach (var endpoint in endpoints)
        {
            WriteEndpoint(fw, endpoint, tag);
        }

        fw.WriteLine("}");
    }

    protected virtual void WriteEndpoint(JavaWriter fw, Endpoint endpoint, string tag)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, endpoint.Description);

        foreach (var param in endpoint.Params)
        {
            fw.WriteLine(1, $" * @param {param.GetParamName()} {param.Comment}");
        }

        if (endpoint.Returns != null)
        {
            fw.WriteLine(1, $" * @return {endpoint.Returns.Comment}");
        }

        fw.WriteLine(1, " */");
        var returnType = "void";

        if (endpoint.Returns != null)
        {
            returnType = Config.GetType(endpoint.Returns);
        }

        var method = new JavaMethod(returnType, endpoint.NameCamel);

        var mappingAnnotation = new JavaAnnotation($@"@{endpoint.Method.ToPascalCase(true)}Mapping", imports: $"org.springframework.web.bind.annotation.{endpoint.Method.ToPascalCase(true)}Mapping")
            .AddAttribute("path", $@"""{endpoint.Route}""");
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
            fw.WriteLine(1, $"{(annotation.StartsWith('@') ? string.Empty : "@")}{annotation}");
        }

        method.AddAnnotation(mappingAnnotation);

        foreach (var routeParam in endpoint.GetRouteParams())
        {
            var param = new JavaMethodParameter(Config.GetType(routeParam), routeParam.GetParamName());
            var pathParamAnnotation = new JavaAnnotation("PathVariable", imports: "org.springframework.web.bind.annotation.PathVariable", value: @$"""{routeParam.GetParamName()}""");
            param.AddAnnotation(pathParamAnnotation);
            param.Imports.AddRange(routeParam.GetTypeImports(Config, tag));
            method.AddParameter(param);
            foreach (var (a, i) in Config.GetDomainAnnotationsAndImports(routeParam, tag))
            {
                param.AddAnnotation(new JavaAnnotation(a, imports: i.ToArray()));
            }
        }

        foreach (var queryParam in endpoint.GetQueryParams())
        {
            var ann = string.Empty;
            var param = new JavaMethodParameter(Config.GetType(queryParam), queryParam.GetParamName());
            var queryParamAnnotation = new JavaAnnotation("RequestParam", imports: "org.springframework.web.bind.annotation.RequestParam", value: @$"""{queryParam.GetParamName()}""")
                .AddAttribute("required", queryParam.Required.ToString().ToFirstLower());
            param.AddAnnotation(queryParamAnnotation);
            param.Imports.AddRange(queryParam.GetTypeImports(Config, tag));
            foreach (var (a, i) in Config.GetDomainAnnotationsAndImports(queryParam, tag))
            {
                param.AddAnnotation(new JavaAnnotation(a, imports: i.ToArray()));
            }

            method.AddParameter(param);
        }

        if (endpoint.IsMultipart)
        {
            foreach (var param in endpoint.Params.Where(param => param is CompositionProperty || (param.Domain?.BodyParam ?? false) || (param.Domain?.IsMultipart ?? false)))
            {
                var ann = string.Empty;
                JavaAnnotation annotation;
                if (!(param.Domain?.IsMultipart ?? false))
                {
                    annotation = new JavaAnnotation("ModelAttribute", imports: "org.springframework.web.bind.annotation.ModelAttribute");
                }
                else
                {
                    annotation = new JavaAnnotation("RequestPart", imports: "org.springframework.web.bind.annotation.RequestPart", value: @$"""{param.GetParamName()}""")
                        .AddAttribute("required", param.Required.ToString().ToFirstLower());
                }

                var parameter = new JavaMethodParameter(Config.GetType(param), param.GetParamName());
                parameter.AddAnnotation(annotation);
                parameter.Imports.AddRange(param.GetTypeImports(Config, tag));
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
                parameter.Imports.AddRange(bodyParam.GetTypeImports(Config, tag));
                foreach (var (a, i) in Config.GetDomainAnnotationsAndImports(bodyParam, tag))
                {
                    parameter.AddAnnotation(new JavaAnnotation(a, imports: i.ToArray()));
                }

                parameter.AddAnnotation(new JavaAnnotation("Valid", imports: Config.JavaxOrJakarta + ".validation.Valid"));
                method.AddParameter(parameter);
            }
        }

        if (method.ReturnType == "void" || method.ReturnType == "Void")
        {
            method.AddAnnotation(new JavaAnnotation("ResponseStatus", imports: ["org.springframework.web.bind.annotation.ResponseStatus", "org.springframework.http.HttpStatus"], value: "HttpStatus.NO_CONTENT"));
        }

        fw.AddImports(method.Imports);
        fw.Write(1, method);
    }
}
