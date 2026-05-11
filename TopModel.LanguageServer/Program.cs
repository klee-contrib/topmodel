using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Server;
using TopModel.Core;
using TopModel.Core.Loaders;
using TopModel.LanguageServer;
using TopModel.Utils;

var fileChecker = new FileChecker();

var server = await LanguageServer.From(options =>
    options
        .WithInput(Console.OpenStandardInput())
        .WithOutput(Console.OpenStandardOutput())
        .ConfigureLogging(logging => logging.AddLanguageProtocolLogging().SetMinimumLevel(LogLevel.Information))
        .WithServices(services =>
        {
            services
                .AddSingleton<ModelStoreRegistry>()
                .AddSingleton<ModelFileCache>()
                .AddSingleton(fileChecker)
                .AddSingleton<AnnotationLoader>()
                .AddSingleton<DomainLoader>()
                .AddSingleton<ConverterLoader>()
                .AddSingleton<DataFlowLoader>()
                .AddSingleton<TranslationStore>()
                .AddSingleton<ModelWatcher>()
                .AddLocalization();
        })
        .WithHandler<TextDocumentSyncHandler>()
        .WithHandler<HoverHandler>()
        .WithHandler<SemanticTokensHandler>()
        .WithHandler<DefinitionHandler>()
        .WithHandler<CompletionHandler>()
        .WithHandler<WorkspaceSymbolHandler>()
        .WithHandler<DocumentSymbolHandler>()
        .WithHandler<CodeActionHandler>()
        .WithHandler<ReferencesHandler>()
        .WithHandler<CodeLensHandler>()
        .WithHandler<RenameHandler>()
        .WithHandler<DocumentLinkHandler>()
        .AddHandler<MermaidHandler>("mermaid")
        .OnInitialize(
            async (lspServer, request, ct) =>
            {
                var rootPath = ResolveRootPath(request.RootPath, request.RootUri?.ToString());

                var registry = lspServer.Services.GetRequiredService<ModelStoreRegistry>();

                var configFiles =
                    args.Length > 0
                        ? [new FileInfo(args[0])]
                        : ConfigUtils.FindConfigFiles(rootPath ?? Directory.GetCurrentDirectory()).ToArray();

                var sp = lspServer.Services;
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var localizer = sp.GetRequiredService<IStringLocalizer<ErrorType>>();
                var annotationLoader = sp.GetRequiredService<AnnotationLoader>();
                var domainLoader = sp.GetRequiredService<DomainLoader>();
                var converterLoader = sp.GetRequiredService<ConverterLoader>();
                var dataFlowLoader = sp.GetRequiredService<DataFlowLoader>();
                var translationStore = sp.GetRequiredService<TranslationStore>();
                var modelWatcher = sp.GetRequiredService<ModelWatcher>();
                Parallel.ForEach(
                    configFiles.Where(f => f.Exists),
                    configFile =>
                    {
                        var config = LoadConfig(fileChecker, configFile);

                        var propertyLoader = new PropertyLoader(fileChecker, config);
                        var classLoader = new ClassLoader(config, fileChecker, propertyLoader);
                        var decoratorLoader = new DecoratorLoader(fileChecker, propertyLoader);
                        var endpointLoader = new EndpointLoader(fileChecker, propertyLoader);
                        var modelFileLoader = new ModelFileLoader(
                            config,
                            loggerFactory.CreateLogger<ModelFileLoader>(),
                            annotationLoader,
                            classLoader,
                            dataFlowLoader,
                            fileChecker,
                            decoratorLoader,
                            converterLoader,
                            endpointLoader,
                            domainLoader
                        );

                        var store = new ModelStore(
                            modelFileLoader,
                            loggerFactory.CreateLogger<ModelStore>(),
                            config,
                            [modelWatcher],
                            translationStore,
                            localizer
                        )
                        {
                            KeepFileErrorsInReferenceResolution = true,
                        };

                        registry.Register(new ModelStoreEntry(store, config));
                    }
                );
            }
        )
        .OnInitialized(
            async (lspServer, _, _, ct) =>
            {
                var registry = lspServer.Services.GetRequiredService<ModelStoreRegistry>();
                await Task.WhenAll(registry.All.Select(entry => entry.Store.LoadFromConfig(watch: true, ct: ct)));
            }
        )
);

await server.WaitForExit;
server.Dispose();

// ---------------------------------------------------------------------------
// Helpers
// ---------------------------------------------------------------------------

static string? ResolveRootPath(string? rootPath, string? rootUri)
{
    if (!string.IsNullOrEmpty(rootPath))
        return rootPath;

    if (!string.IsNullOrEmpty(rootUri))
    {
        try
        {
            return new Uri(rootUri).LocalPath;
        }
        catch
        { /* uri invalide */
        }
    }

    return Directory.GetCurrentDirectory();
}

static ModelConfig LoadConfig(FileChecker fileChecker, FileInfo configFile)
{
    using var text = configFile.OpenText();
    var config = fileChecker.DeserializeConfig(text.ReadToEnd()).Init(configFile.DirectoryName!);

    foreach (var (configName, genConfigMaps) in config.Generators)
    {
        for (var j = 0; j < genConfigMaps.Count(); j++)
        {
            var genConfigMap = genConfigMaps.ElementAt(j);
            var number = j + 1;

            var genConfig = fileChecker.GetWatcherConfigBase(genConfigMap);
            genConfig.InitVariables(config.App, number);
            genConfig.Name ??= $"{configName}@{number}";
            try
            {
                config.Configs.Add(genConfig.Name, genConfig);
            }
            catch (ArgumentException)
            {
                // Nom déjà utilisé, on ignore.
            }

            foreach (var referencedTag in genConfig.ReferencedTags)
            {
                if (config.Configs.TryGetValue(referencedTag.Value, out var referencedConfig))
                {
                    genConfig.ReferencedTagConfigs.Add(referencedTag.Key, referencedConfig);
                }
            }
        }
    }

    return config;
}
