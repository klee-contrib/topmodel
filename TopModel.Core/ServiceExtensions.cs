using Microsoft.Extensions.DependencyInjection;
using TopModel.Core.Loaders;

namespace TopModel.Core;

public static class ServiceExtensions
{
    public static IServiceCollection AddModelFileLoader(this IServiceCollection services, FileChecker fileChecker)
    {
        return services
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
            .AddSingleton<ModelFileLoader>();
    }

    public static IServiceCollection AddModelStore(this IServiceCollection services, ModelConfig config)
    {
        return services
            .AddLocalization()
            .AddMemoryCache()
            .AddSingleton<ModelConfig>()
            .AddSingleton<TranslationStore>()
            .AddSingleton<ModelStore>()
            .AddSingleton(config);
    }
}
