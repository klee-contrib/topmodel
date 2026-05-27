using System.Text.RegularExpressions;
using TopModel.ModelGenerator;
using TopModel.Utils.Cli;

var command = new TopModelCommand<ModelGeneratorMessage>(ModelGeneratorMessage.RootCommandDescription, args);

if (await command.IsHelpOrVersionRequested())
{
    return 0;
}

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArgs) =>
{
    eventArgs.Cancel = true;
    cts.Cancel();
};

if (await command.FindConfigs("TopModel.ModelGenerator", new Regex(@"tmdgen[^/\\]*\.config$"), cts.Token))
{
    return 1;
}

var fileChecker = new TmdGenFileChecker();
return await command.RunConfigs<ModelGeneratorConfig, TmdGenFileChecker, TmdGenWorker>(fileChecker, cts.Token);
