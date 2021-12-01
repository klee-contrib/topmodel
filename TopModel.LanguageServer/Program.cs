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
        .ConfigureLogging(logging => logging
            .AddLanguageProtocolLogging()
            .SetMinimumLevel(LogLevel.Trace))
        .WithServices(services =>
        {
            var fileChecker = new FileChecker();
            var configFile = new FileInfo(args.Length > 0 ? args[0] : "modgen.yaml");
            var config = fileChecker.Deserialize<ModelConfig>(configFile.OpenText().ReadToEnd());
            var dn = configFile.DirectoryName;

            services
                .AddModelStore(fileChecker, config, dn)
                .AddSingleton<IModelWatcher, ModelErrorWatcher>();
        })
        .WithHandler<TextDocumentSyncHandler>()
        .WithHandler<HoverHandler>()
        .WithHandler<SemanticTokensHandler>()
        .WithHandler<DefinitionHandler>()
        .OnInitialize((server, _, __) =>
        {
            server.Services.GetRequiredService<ModelStore>().LoadFromConfig();
            return Task.CompletedTask;
        }));

await server.WaitForExit;