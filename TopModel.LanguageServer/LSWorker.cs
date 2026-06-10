using Microsoft.Extensions.DependencyInjection;
using TopModel.Core;
using TopModel.Core.Loaders;
using TopModel.Utils.Cli;

namespace TopModel.LanguageServer;

public class LSWorker : TopModelWorker<ModelConfig, FileChecker>
{
    private ModelStore? _modelStore;

    public ModelConfig ModelConfig => ServiceProvider.GetRequiredService<ModelConfig>();

    public ModelFileLoader ModelFileLoader => ServiceProvider.GetRequiredService<ModelFileLoader>();

    public ModelStore ModelStore => ServiceProvider.GetRequiredService<ModelStore>();

    public override void Init()
    {
        Services.AddLogging().AddModelStore(Config);
    }

    public override async Task Run(CancellationToken cancellationToken)
    {
        foreach (var (configName, genConfigMaps) in Config.Generators)
        {
            for (var j = 0; j < genConfigMaps.Count(); j++)
            {
                var genConfigMap = genConfigMaps.ElementAt(j);
                var number = j + 1;

                var genConfig = FileChecker.GetWatcherConfigBase(genConfigMap);
                genConfig.InitVariables(Config.App, number);
                genConfig.Name ??= $"{configName}@{number}";
                try
                {
                    Config.Configs.Add(genConfig.Name, genConfig);
                }
                catch (ArgumentException)
                {
                    // On ignore l'erreur, tant pis si le nom est déjà utilisé.
                }

                foreach (var referencedTag in genConfig.ReferencedTags)
                {
                    if (Config.Configs.TryGetValue(referencedTag.Value, out var referencedConfig))
                    {
                        genConfig.ReferencedTagConfigs.Add(referencedTag.Key, referencedConfig);
                    }
                    else
                    {
                        // On ignore l'erreur, tant pis pour la configuration manquante.
                    }
                }
            }
        }

        _modelStore = ServiceProvider.GetRequiredService<ModelStore>();
        _modelStore.KeepFileErrorsInReferenceResolution = true;
        await _modelStore.LoadFromConfig(WatchMode, ParallelMode, ct: cancellationToken);
    }

    public override async Task WaitForFinished(CancellationToken cancellationToken)
    {
        if (_modelStore != null)
        {
            await _modelStore.WaitForUpdates(cancellationToken);
        }
    }
}
