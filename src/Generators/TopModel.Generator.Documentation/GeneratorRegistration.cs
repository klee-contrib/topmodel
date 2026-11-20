namespace TopModel.Generator.Documentation;

using Microsoft.Extensions.DependencyInjection;
using TopModel.Generator.Core;
using static TopModel.Utils.ModelUtils;

public class GeneratorRegistration : IGeneratorRegistration<DocumentationConfig>
{
    /// <inheritdoc cref="IGeneratorRegistration{T}.Register" />
    public void Register(IServiceCollection services, DocumentationConfig config, int number)
    {
        TrimSlashes(config, c => c.EndpointsFilePath);
        TrimSlashes(config, c => c.ClassesFilePath);
        TrimSlashes(config, c => c.MermaidFilePath);

        services.AddGenerator<DocumentationClassDocGenerator, DocumentationConfig>(config, number);
        services.AddGenerator<DocumentationEndpointDocGenerator, DocumentationConfig>(config, number);
        services.AddGenerator<DocumentationMermaidGenerator, DocumentationConfig>(config, number);
    }
}
