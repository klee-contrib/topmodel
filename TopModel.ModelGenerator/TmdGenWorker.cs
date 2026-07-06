using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopModel.ModelGenerator.Database;
using TopModel.ModelGenerator.OpenApi;
using TopModel.Utils;
using TopModel.Utils.Cli;

namespace TopModel.ModelGenerator;

public class TmdGenWorker : TopModelWorker<ModelGeneratorConfig, TmdGenFileChecker>
{
    private readonly Dictionary<string, string> _passwords = [];

    public override void Init()
    {
        Services
            .AddLogging(builder => builder.AddProvider(LoggerProvider))
            .AddSingleton<IFileWriterProvider>(new GeneratedFileWriterProvider(Config));

        foreach (var conf in Config.OpenApi)
        {
            ModelUtils.TrimSlashes(conf, c => c.OutputDirectory);
            Services.AddSingleton<TmdGenerator>(p => new OpenApiTmdGenerator(
                p.GetRequiredService<ILogger<OpenApiTmdGenerator>>(),
                conf,
                p.GetRequiredService<IFileWriterProvider>()
            )
            {
                DirectoryName = ConfigDirectoryName,
                ModelRoot = Config.ModelRoot,
                Number = Config.OpenApi.IndexOf(conf) + 1,
            });
        }

        foreach (var conf in Config.Database)
        {
            ModelUtils.TrimSlashes(conf, c => c.OutputDirectory);
            if (conf.Source.DbType == DbType.ORACLE)
            {
                Services.AddSingleton<TmdGenerator>(p => new DatabaseOraTmdGenerator(
                    p.GetRequiredService<ILogger<DatabaseOraTmdGenerator>>(),
                    conf,
                    p.GetRequiredService<IFileWriterProvider>()
                )
                {
                    DirectoryName = ConfigDirectoryName,
                    ModelRoot = Config.ModelRoot,
                    Number = Config.Database.IndexOf(conf) + 1,
                    Passwords = _passwords,
                });
            }
            else if (conf.Source.DbType == DbType.POSTGRESQL)
            {
                Services.AddSingleton<TmdGenerator>(p => new DatabasePgTmdGenerator(
                    p.GetRequiredService<ILogger<DatabasePgTmdGenerator>>(),
                    conf,
                    p.GetRequiredService<IFileWriterProvider>()
                )
                {
                    DirectoryName = ConfigDirectoryName,
                    ModelRoot = Config.ModelRoot,
                    Number = Config.Database.IndexOf(conf) + 1,
                    Passwords = _passwords,
                });
            }
            else if (conf.Source.DbType == DbType.MYSQL)
            {
                Services.AddSingleton<TmdGenerator>(p => new DatabaseMySqlTmdGenerator(
                    p.GetRequiredService<ILogger<DatabaseMySqlTmdGenerator>>(),
                    conf,
                    p.GetRequiredService<IFileWriterProvider>()
                )
                {
                    DirectoryName = ConfigDirectoryName,
                    ModelRoot = Config.ModelRoot,
                    Number = Config.Database.IndexOf(conf) + 1,
                    Passwords = _passwords,
                });
            }
            else if (conf.Source.DbType == DbType.MSSQL)
            {
                Services.AddSingleton<TmdGenerator>(p => new DatabaseMsSqlTmdGenerator(
                    p.GetRequiredService<ILogger<DatabaseMsSqlTmdGenerator>>(),
                    conf,
                    p.GetRequiredService<IFileWriterProvider>()
                )
                {
                    DirectoryName = ConfigDirectoryName,
                    ModelRoot = Config.ModelRoot,
                    Number = Config.Database.IndexOf(conf) + 1,
                    Passwords = _passwords,
                });
            }
        }
    }

    public override async Task Run(CancellationToken cancellationToken)
    {
        var mainLogger = ServiceProvider.GetRequiredService<ILogger<TmdGenerator>>();
        using var scope = mainLogger.BeginScope(StoreConfig);

        var generators = ServiceProvider.GetRequiredService<IEnumerable<TmdGenerator>>();

        mainLogger.LogInformation(string.Empty);
        mainLogger.LogInformation(
            ModelGeneratorMessage.RegisteredGenerators,
            $"{Environment.NewLine}                          {string.Join($"{Environment.NewLine}                          ", generators.Select(g => $"- {g.Name}@{{{g.Number}}}"))}"
        );

        var tmdLock = new TopModelLock(Config, mainLogger);
        var generatedFiles = new List<string>();

        foreach (var generator in generators)
        {
            generatedFiles.AddRange(await generator.Generate(StoreConfig, cancellationToken));
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            tmdLock.UpdateFiles(generatedFiles);
            mainLogger.LogInformation(ModelGeneratorMessage.UpdateCompleted);
        }
    }

    public override Task WaitForFinished(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
