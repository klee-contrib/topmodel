using System.CommandLine;
using System.Text.RegularExpressions;
using TopModel.ModelGenerator;
using TopModel.Utils.Cli;

var schemaOption = new Option<bool>("--schema", "-s") { Description = CliMessage.SchemaOptionDescription.GetMessage() };

using var command = new TopModelCommand<ModelGeneratorMessage, ModelGeneratorConfig, TmdGenFileChecker, TmdGenWorker>(
    ModelGeneratorMessage.RootCommandDescription,
    args,
    schemaOption
);

if (await command.IsHelpOrVersionRequested())
{
    return 0;
}

var schemaMode = command.Args.GetValue(schemaOption);

if (await command.CheckVersionAndFindConfigs("TopModel.ModelGenerator", new Regex(@"tmdgen[^/\\]*\.config$")))
{
    return 1;
}

return await command.RunConfigs(worker =>
{
    worker.SchemaMode = schemaMode;
});
