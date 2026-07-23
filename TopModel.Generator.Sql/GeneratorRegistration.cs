using Microsoft.Extensions.DependencyInjection;
using TopModel.Generator.Core;
using TopModel.Generator.Sql.Procedural;
using TopModel.Generator.Sql.Ssdt;
using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.Sql;

/// <summary>
/// Enregistre les générateurs SQL.
/// </summary>
public class GeneratorRegistration : IGeneratorRegistration<SqlConfig>
{
    /// <inheritdoc cref="IGeneratorRegistration{T}.Register" />
    public void Register(IServiceCollection services, SqlConfig config, int number)
    {
        if (config.Ssdt != null)
        {
            CombinePath(config.OutputDirectory, config.Ssdt, c => c.InitListScriptFolder);
            CombinePath(config.OutputDirectory, config.Ssdt, c => c.TableScriptFolder);
            CombinePath(config.OutputDirectory, config.Ssdt, c => c.TableTypeScriptFolder);

            if (config.Ssdt.TableScriptFolder != null)
            {
                services.AddGenerator<SsdtTableGenerator, SqlConfig>(config, number);
            }

            if (config.Ssdt.TableTypeScriptFolder != null)
            {
                services.AddGenerator<SsdtTableTypeGenerator, SqlConfig>(config, number);
            }

            if (config.Ssdt.InitListScriptFolder != null)
            {
                services.AddGenerator<SsdtValuesGenerator, SqlConfig>(config, number);

                if (config.Ssdt.InitListMainScriptName != null)
                {
                    services.AddGenerator<SsdtMainValuesGenerator, SqlConfig>(config, number);
                }
            }
        }

        if (config.Procedural != null)
        {
            CombinePath(config.OutputDirectory, config.Procedural, c => c.TablesFileName);
            CombinePath(config.OutputDirectory, config.Procedural, c => c.IndexesAndKeysFileName);
            CombinePath(config.OutputDirectory, config.Procedural, c => c.ValuesFileName);
            CombinePath(config.OutputDirectory, config.Procedural, c => c.ResourcesFileName);
            CombinePath(config.OutputDirectory, config.Procedural, c => c.TypesFileName);
            CombinePath(config.OutputDirectory, config.Procedural, c => c.ResourcesFileName);

            services.AddGenerator<SqlTablesGenerator, SqlConfig>(config, number);
            services.AddGenerator<SqlIndexesKeysGenerator, SqlConfig>(config, number);
            services.AddGenerator<SqlValuesGenerator, SqlConfig>(config, number);
            services.AddGenerator<SqlResourcesGenerator, SqlConfig>(config, number);

            if (config.TargetDBMS == TargetDBMS.Sqlserver)
            {
                services.AddGenerator<SqlServerTypesGenerator, SqlConfig>(config, number);
            }

            if (config.Procedural.CommentsFileName != null)
            {
                services.AddGenerator<SqlCommentsGenerator, SqlConfig>(config, number);
            }
        }
    }
}
