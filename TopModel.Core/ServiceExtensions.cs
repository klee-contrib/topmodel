using Microsoft.Extensions.DependencyInjection;
using TopModel.Core.Loaders;

namespace TopModel.Core;

public static class ServiceExtensions
{
    public static IServiceCollection AddModelStore(
        this IServiceCollection services,
        FileChecker fileChecker,
        ModelConfig config
    )
    {
        services
            .AddLocalization()
            .AddMemoryCache()
            .AddSingleton(fileChecker)
            .AddSingleton<AnnotationLoader>()
            .AddSingleton<ClassLoader>()
            .AddSingleton<ConverterLoader>()
            .AddSingleton<DataFlowLoader>()
            .AddSingleton<DecoratorLoader>()
            .AddSingleton<DomainLoader>()
            .AddSingleton<EndpointLoader>()
            .AddSingleton<PropertyLoader>()
            .AddSingleton<ModelFileLoader>()
            .AddSingleton<ModelConfig>()
            .AddSingleton<TranslationStore>()
            .AddSingleton<ModelStore>()
            .AddSingleton(config);

        return services;
    }
}
