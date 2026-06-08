using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Server;
using TopModel.Core;
using TopModel.Core.Loaders;
using TopModel.LanguageServer;
using TopModel.LanguageServer.Handlers;
using TopModel.Utils.Cli;

var fixedArgs = !args.Contains("-w") && !args.Contains("--watch") ? args.Concat(["-w"]) : args;
fixedArgs = !args.Contains("-p") && !args.Contains("--parallel") ? args.Concat(["-p"]) : args;

using var command = new TopModelCommand<CliMessage>("TopModel LS", fixedArgs);

if (
    await command.CheckVersionAndFindConfigs(
        "TopModel.LanguageServer",
        new Regex("topmodel\\.?([a-zA-Z-_.]*)\\.config$")
    )
)
{
    return 1;
}

using var server = await LanguageServer.From(options =>
    options
        .WithInput(Console.OpenStandardInput())
        .WithOutput(Console.OpenStandardOutput())
        .ConfigureLogging(logging => logging.AddLanguageProtocolLogging().SetMinimumLevel(LogLevel.Information))
        .WithServices(services => services.AddSingleton<ModelFileCache>().AddSingleton<LSWorkerStore>())
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
            async (provider, _, _) =>
            {
                var workerStore = provider.GetRequiredService<LSWorkerStore>();
                await command.RunConfigs<ModelConfig, FileChecker, LSWorker>(
                    worker =>
                    {
                        worker.Services.AddSingleton(provider.GetRequiredService<ILanguageServerFacade>());
                        workerStore.AddWorker(worker);
                    },
                    worker => workerStore.RemoveWorker(worker)
                );
            }
        )
);

await server.WaitForExit;
return 0;
