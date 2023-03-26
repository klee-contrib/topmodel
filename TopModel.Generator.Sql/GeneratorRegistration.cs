using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Generator.Sql.Procedural;
using TopModel.Generator.Sql.Ssdt;

using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.Sql;

public class GeneratorRegistration : IGeneratorRegistration<SqlConfig>
{
    public void Register(IServiceCollection services, string dn, IEnumerable<SqlConfig>? configs)
    {
        GeneratorUtils.HandleConfigs(dn, configs, (config, number) =>
        {
            if (config.Ssdt != null)
            {
                CombinePath(config.OutputDirectory, config.Ssdt, c => c.InitListScriptFolder);
                CombinePath(config.OutputDirectory, config.Ssdt, c => c.TableScriptFolder);
                CombinePath(config.OutputDirectory, config.Ssdt, c => c.TableTypeScriptFolder);

                services.AddSingleton<IModelWatcher>(p =>
                    new SsdtGenerator(p.GetRequiredService<ILogger<SsdtGenerator>>(), config) { Number = number });
            }

            if (config.Procedural != null)
            {
                CombinePath(config.OutputDirectory, config.Procedural, c => c.CrebasFile);
                CombinePath(config.OutputDirectory, config.Procedural, c => c.IndexFKFile);
                CombinePath(config.OutputDirectory, config.Procedural, c => c.InitListFile);
                CombinePath(config.OutputDirectory, config.Procedural, c => c.TypeFile);
                CombinePath(config.OutputDirectory, config.Procedural, c => c.UniqueKeysFile);

                services.AddSingleton<IModelWatcher>(p =>
                    new ProceduralSqlGenerator(p.GetRequiredService<ILogger<ProceduralSqlGenerator>>(), config) { Number = number });
            }
        });
    }
}
