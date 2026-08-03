using System.Reflection;
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

#nullable disable

    private ILogger _logger;

#nullable enable

    public bool SchemaMode { get; set; }

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
        _logger = ServiceProvider.GetRequiredService<ILogger<TmdGenerator>>();
        using var scope = _logger.BeginScope(StoreConfig);

        if (SchemaMode)
        {
            await WriteSchema(cancellationToken);
        }

        var generators = ServiceProvider.GetRequiredService<IEnumerable<TmdGenerator>>();

        _logger.LogInformation(string.Empty);
        _logger.LogInformation(
            ModelGeneratorMessage.RegisteredGenerators,
            $"{Environment.NewLine}                          {string.Join($"{Environment.NewLine}                          ", generators.Select(g => $"- {g.Name}@{{{g.Number}}}"))}"
        );

        var tmdLock = new TopModelLock(Config, _logger);
        var generatedFiles = new List<string>();

        foreach (var generator in generators)
        {
            generatedFiles.AddRange(await generator.Generate(StoreConfig, cancellationToken));
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            tmdLock.UpdateFiles(generatedFiles);
            _logger.LogInformation(ModelGeneratorMessage.UpdateCompleted);
        }
    }

    public override Task WaitForFinished(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task WriteSchema(CancellationToken cancellationToken)
    {
        _logger.LogInformation(CliMessage.GeneratingConfigSchema);

        var schema = await File.ReadAllTextAsync(
            Assembly.GetExecutingAssembly().GetFilePath("schema.tmdgen.config.json"),
            cancellationToken
        );
        await File.WriteAllTextAsync(ConfigFullName + ".schema.json", schema, cancellationToken);

        var configFile = await File.ReadAllTextAsync(ConfigFullName, cancellationToken);
        if (!configFile.StartsWith("# yaml-language-server"))
        {
            var relativePath = ConfigFullName.ToRelative(Config.ConfigRoot);
            configFile =
                $"# yaml-language-server: $schema={relativePath}.schema.json{Environment.NewLine}" + configFile;
            await File.WriteAllTextAsync(ConfigFullName, configFile, cancellationToken);
        }

        _logger.LogInformation(CliMessage.ConfigSchemaGenerated);
    }
}
