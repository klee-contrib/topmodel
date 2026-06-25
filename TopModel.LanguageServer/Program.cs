using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Server;
using TopModel.Core;
using TopModel.Core.Loaders;
using TopModel.LanguageServer;
using TopModel.LanguageServer.Handlers;
using TopModel.Utils.Cli;

IList<string> fixedArgs = [.. args];

if (!args.Contains("-w") && !args.Contains("--watch"))
{
    fixedArgs.Add("-w");
}

if (!args.Contains("-p") && !args.Contains("--parallel"))
{
    fixedArgs.Add("-p");
}

using var command = new TopModelCommand<CliMessage, ModelConfig, FileChecker, LSWorker>("TopModel LS", fixedArgs);

if (await command.IsHelpOrVersionRequested())
{
    return 0;
}

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
        .WithServices(services =>
            services
                .AddModelFileLoader(command.FileChecker)
                .AddSingleton<ModelFileCache>()
                .AddSingleton<LSWorkerStore>()
                .AddSingleton<IModelReporter, LSReporter>()
        )
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
                var lsReporter = provider.GetRequiredService<LSReporter>();
                command.OnConfigRestart = () => lsReporter.Report(refresh: true);

                var workerStore = provider.GetRequiredService<LSWorkerStore>();
                await command.RunConfigs(
                    worker =>
                    {
                        worker.Services.AddSingleton<IModelReporter>(provider.GetRequiredService<LSReporter>());
                        worker.Services.AddSingleton(provider.GetRequiredService<ModelFileLoader>());
                        workerStore.AddWorker(worker);
                    },
                    worker => workerStore.RemoveWorker(worker)
                );
            }
        )
        .OnInitialized(
            async (provider, _, _, _) =>
            {
                provider.GetRequiredService<LSReporter>().Report();
            }
        )
);

try
{
    await server.WaitForExit.WaitAsync(command.CancellationToken);
}
catch (OperationCanceledException)
{
    // Ctrl+C : le token a été annulé, on sort pour laisser les `using` disposer le serveur et les watchers.
}

return 0;
