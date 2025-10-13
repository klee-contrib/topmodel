using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Server;
using TopModel.Core;
using TopModel.Core.Loaders;
using TopModel.LanguageServer;

var server = await LanguageServer.From(options =>
    options
        .WithInput(Console.OpenStandardInput())
        .WithOutput(Console.OpenStandardOutput())
        .ConfigureLogging(logging => logging.AddLanguageProtocolLogging().SetMinimumLevel(LogLevel.Information))
        .WithServices(services =>
        {
            var fileChecker = new FileChecker();
            var file = new FileInfo(args.Length > 0 ? args[0] : "topmodel.config");
            using var text = file.OpenText();
            var config = fileChecker.DeserializeConfig(text.ReadToEnd()).Init(file.DirectoryName!);

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
                        // On ignore l'erreur, tant pis pour la configuration manquante.
                    }
                }
            }

            services
                .AddModelStore(fileChecker, config)
                .AddSingleton<IModelWatcher, ModelWatcher>()
                .AddSingleton<ModelFileCache>();
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
            async (server, _, __) =>
            {
                await server.Services.GetRequiredService<ModelStore>().LoadFromConfig(watch: true, ct: __);
            }
        )
);

await server.WaitForExit;
server.Dispose();
