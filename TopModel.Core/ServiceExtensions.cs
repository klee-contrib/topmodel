using TopModel.Core.Loaders;
using Microsoft.Extensions.DependencyInjection;

namespace TopModel.Core
{
    using static ModelUtils;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddModelStore(this IServiceCollection services, FileChecker fileChecker, ModelConfig? config = null, string? rootDir = null)
        {
            services
                .AddMemoryCache()
                .AddSingleton(fileChecker)
                .AddSingleton<ModelFileLoader>()
                .AddSingleton<DomainFileLoader>()
                .AddSingleton<ModelStore>();

            if (config != null && rootDir != null)
            {
                config.ModelRoot ??= string.Empty;
                config.Domains ??= "domains.yml";

                CombinePath(rootDir, config, c => c.ModelRoot);
                CombinePath(rootDir, config, c => c.Domains);
                CombinePath(rootDir, config, c => c.StaticLists);
                services.AddSingleton(config);
            }

            return services;
        }
    }
}
