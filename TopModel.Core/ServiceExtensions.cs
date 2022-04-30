using Microsoft.Extensions.DependencyInjection;
using TopModel.Core.Loaders;
using TopModel.Utils;

namespace TopModel.Core;

public static class ServiceExtensions
{
    public static IServiceCollection AddModelStore(this IServiceCollection services, FileChecker fileChecker, ModelConfig? config = null, string? rootDir = null)
    {
        services
            .AddMemoryCache()
            .AddSingleton(fileChecker)
            .AddSingleton<ClassLoader>()
            .AddSingleton<DecoratorLoader>()
            .AddSingleton<ModelFileLoader>()
            .AddSingleton<ModelStore>();

        if (config != null && rootDir != null)
        {
            config.ModelRoot ??= string.Empty;

            ModelUtils.CombinePath(rootDir, config, c => c.ModelRoot);
            services.AddSingleton(config);
        }

        return services;
    }
}