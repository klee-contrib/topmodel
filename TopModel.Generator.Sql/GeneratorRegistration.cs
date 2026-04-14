using Microsoft.Extensions.DependencyInjection;
using TopModel.Generator.Core;
using TopModel.Generator.Sql.Procedural;
using TopModel.Generator.Sql.Procedural.Oracle;
using TopModel.Generator.Sql.Procedural.Postgres;
using TopModel.Generator.Sql.Procedural.SqlServer;
using TopModel.Generator.Sql.Ssdt;
using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.Sql;

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
                services.AddGenerator<SsdtReferenceListGenerator, SqlConfig>(config, number);

                if (config.Ssdt.InitListMainScriptName != null)
                {
                    services.AddGenerator<SsdtMainReferenceListGenerator, SqlConfig>(config, number);
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

            if (config.TargetDBMS == TargetDBMS.Oracle)
            {
                services.AddGenerator<OracleTablesGenerator, SqlConfig>(config, number);
                services.AddGenerator<OracleValuesGenerator, SqlConfig>(config, number);
            }

            if (config.TargetDBMS == TargetDBMS.Postgre)
            {
                services.AddGenerator<PostgresTablesGenerator, SqlConfig>(config, number);
                services.AddGenerator<PostgresValuesGenerator, SqlConfig>(config, number);
            }

            if (config.TargetDBMS == TargetDBMS.Sqlserver)
            {
                services.AddGenerator<SqlServerTablesGenerator, SqlConfig>(config, number);
                services.AddGenerator<SqlServerTypesGenerator, SqlConfig>(config, number);
                services.AddGenerator<SqlServerValuesGenerator, SqlConfig>(config, number);
            }

            services.AddGenerator<SqlIndexesKeysGenerator, SqlConfig>(config, number);
            services.AddGenerator<SqlResourcesGenerator, SqlConfig>(config, number);

            if (config.Procedural.CommentsFileName != null)
            {
                services.AddGenerator<SqlCommentsGenerator, SqlConfig>(config, number);
            }
        }
    }
}
