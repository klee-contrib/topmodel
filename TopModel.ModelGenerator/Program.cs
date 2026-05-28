using System.Text.RegularExpressions;
using TopModel.ModelGenerator;
using TopModel.Utils.Cli;

using var command = new TopModelCommand<ModelGeneratorMessage>(ModelGeneratorMessage.RootCommandDescription, args);

if (await command.IsHelpOrVersionRequested())
{
    return 0;
}

if (await command.CheckVersionAndFindConfigs("TopModel.ModelGenerator", new Regex(@"tmdgen[^/\\]*\.config$")))
{
    return 1;
}

return await command.RunConfigs<ModelGeneratorConfig, TmdGenFileChecker, TmdGenWorker>();
