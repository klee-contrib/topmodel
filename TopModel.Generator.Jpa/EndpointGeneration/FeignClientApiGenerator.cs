using Microsoft.Extensions.Logging;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.EndpointGeneration;

/// <summary>
/// Générateur des objets de traduction javascripts.
/// </summary>
public class FeignClientApiGenerator(ILogger<FeignClientApiGenerator> logger, IFileWriterProvider writerProvider)
    : SpringServerApiGenerator(logger, writerProvider)
{
    public override string Name => "FeignClientApiGen";
    private static string DefaultApiClassName => "{fileName}Api";

    protected override bool FilterTag(string tag)
    {
        return Config.ResolveVariables(Config.ApiGeneration!, tag) == ApiGeneration.Client
            && Config.ResolveVariables(Config.ClientApiGeneration!, tag) == ClientApiMode.FeignClient;
    }

    protected override IEnumerable<JavaAnnotation> GetClassAnnotations(ModelFile file, string tag)
    {
        var fileName = file.Options.Endpoints.FileName;
        foreach (var a in base.GetClassAnnotations(file, tag).Where(a => a.Name != "RequestMapping"))
        {
            yield return a;
        }

        var feignClientAnnotation = new JavaAnnotation(
            "FeignClient",
            imports: "org.springframework.cloud.openfeign.FeignClient"
        )
            .AddAttribute("name", $@"""{Config.GetRootModule(file.Namespace)}""")
            .AddAttribute("contextId", $@"""{GetClassName(fileName, tag)}""");
        yield return feignClientAnnotation;
    }

    protected override string GetClassName(string fileName, string tag)
    {
        return Config.GetApiClassName(DefaultApiClassName, fileName, tag);
    }

    protected override string GetRoute(Endpoint endpoint)
    {
        return endpoint.FullRoute;
    }
}
