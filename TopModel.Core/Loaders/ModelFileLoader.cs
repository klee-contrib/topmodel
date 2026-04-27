using Meziantou.Framework.Globbing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Utils;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class ModelFileLoader(
    ModelConfig config,
    ILogger<ModelFileLoader> logger,
    AnnotationLoader annotationLoader,
    ClassLoader classLoader,
    DataFlowLoader dataFlowLoader,
    FileChecker fileChecker,
    DecoratorLoader decoratorLoader,
    ConverterLoader converterLoader,
    EndpointLoader endpointLoader,
    DomainLoader domainLoader
)
{
    private static Dictionary<
        (string App, string ModelRoot, bool PluralizeTableNames, bool UseLegacyRoleNames),
        Dictionary<string, ModelFile>
    > GlobalCache { get; } = [];

    private static Dictionary<
        string,
        (
            FileSystemWatcher FileWatcher,
            IMemoryCache Cache,
            IList<(
                GlobCollection ModelFilePaths,
                Func<IEnumerable<(string FullPath, ModelFile? ModelFile)>, CancellationToken, Task> ApplyUpdates
            )> Configs
        )
    > FileWatchers { get; } = [];

    private Dictionary<string, ModelFile> Cache
    {
        get
        {
            var key = (config.App, config.ModelRoot, config.PluralizeTableNames, config.UseLegacyRoleNames);

            if (!GlobalCache.TryGetValue(key, out var cache))
            {
                GlobalCache[key] = [];
                cache = GlobalCache[key];
            }

            return cache;
        }
    }

    public async Task<(string FullPath, ModelFile? ModelFile)> LoadModelFile(
        string fullPath,
        WatcherChangeTypes changeType,
        string? content = null,
        CancellationToken ct = default
    )
    {
        fullPath = fullPath.Replace('\\', '/');

        if (content != null)
        {
            RemoveFromCache(fullPath);
        }
        else if (Cache.TryGetValue(fullPath, out var file))
        {
            return (fullPath, file);
        }

        try
        {
            ModelFile? modelFile = null;
            if (changeType != WatcherChangeTypes.Deleted && File.Exists(fullPath))
            {
                modelFile = await ReadModelFile(fullPath, content, ct);
            }

            if (modelFile != null)
            {
                Cache.Add(fullPath, modelFile);
            }
            else
            {
                RemoveFromCache(fullPath);
            }

            return (fullPath, modelFile);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            RemoveFromCache(fullPath);
            return (fullPath, null);
        }
    }

    public Action Watch(
        Func<IEnumerable<(string FullPath, ModelFile? ModelFile)>, CancellationToken, Task> applyUpdates
    )
    {
        var watchConfig = (config.ModelFilePaths, applyUpdates);
        if (!FileWatchers.TryGetValue(config.ModelRoot, out var fw))
        {
            var fileWatcher = new FileSystemWatcher(config.ModelRoot, "*.tmd")
            {
                IncludeSubdirectories = true,
                EnableRaisingEvents = true,
            };

            FileWatchers[config.ModelRoot] = (fileWatcher, new MemoryCache(new MemoryCacheOptions()), [watchConfig]);
            fw = FileWatchers[config.ModelRoot];

            fileWatcher.Changed += (s, e) => OnFileChanged(config.ModelRoot, fw.Cache, e);
            fileWatcher.Created += (s, e) => OnFileChanged(config.ModelRoot, fw.Cache, e);
            fileWatcher.Deleted += (s, e) => OnFileChanged(config.ModelRoot, fw.Cache, e);
            fileWatcher.Renamed += (s, e) => OnFileChanged(config.ModelRoot, fw.Cache, e);
        }
        else
        {
            fw.Configs.Add(watchConfig);
        }

        return () =>
        {
            fw.Configs.Remove(watchConfig);
            if (fw.Configs.Count == 0)
            {
                fw.FileWatcher.Dispose();
            }
        };
    }

    private void OnFileChanged(string modelRoot, IMemoryCache cache, FileSystemEventArgs e)
    {
        cache.Set(
            e.FullPath.Replace('\\', '/'),
            e,
            new MemoryCacheEntryOptions()
                .AddExpirationToken(
                    new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromMilliseconds(50)).Token)
                )
                .RegisterPostEvictionCallback(
                    async (k, v, r, a) =>
                    {
                        if (r != EvictionReason.TokenExpired)
                        {
                            return;
                        }

                        e = (FileSystemEventArgs)v!;
                        var type = e.ChangeType switch
                        {
                            WatcherChangeTypes.Created => "Créé",
                            WatcherChangeTypes.Deleted => "Supprimé",
                            WatcherChangeTypes.Renamed => "Renommé",
                            _ => "Modifié",
                        };

                        logger.LogInformation($"{type}:  {e.FullPath.ToRelative()}");

                        var files = new List<(string, ModelFile?)>();

                        if (e is RenamedEventArgs re)
                        {
                            RemoveFromCache(re.OldFullPath);
                            RemoveFromCache(re.FullPath);
                            files.Add(await LoadModelFile(re.OldFullPath, WatcherChangeTypes.Deleted, ct: default));
                            files.Add(await LoadModelFile(re.FullPath, WatcherChangeTypes.Created, ct: default));
                        }
                        else
                        {
                            RemoveFromCache(e.FullPath);
                            files.Add(await LoadModelFile(e.FullPath, e.ChangeType, ct: default));
                        }

                        foreach (var (modelFilePaths, applyUpdates) in FileWatchers[modelRoot].Configs)
                        {
                            if (!modelFilePaths.IsMatch(e.FullPath.ToRelative(config.ModelRoot)[2..]))
                            {
                                continue;
                            }

                            await applyUpdates(files, default);
                        }
                    }
                )
        );
    }

    private async Task<ModelFile?> ReadModelFile(
        string filePath,
        string? content = null,
        CancellationToken ct = default
    )
    {
        var fileName = config.GetFileName(filePath);
        content ??= await File.ReadAllTextAsync(filePath, ct);

        fileChecker.CheckModelFile(filePath, content);

        var parser = new Parser(new StringReader(content));
        parser.Consume<StreamStart>();

        if (parser.Current is StreamEnd)
        {
            return null;
        }

        parser.Consume<DocumentStart>();

        var file = new ModelFile { Name = fileName, Path = filePath.ToRelative() };

        parser.ConsumeMapping(prop =>
        {
            parser.TryConsume<Scalar>(out var value);

            switch (prop.Value)
            {
                case "module":
                    file.Namespace = new Namespace { App = config.App, Module = value!.Value };
                    break;
                case "tags":
                    parser.ConsumeSequence(() => file.Tags.Add(parser.Consume<Scalar>().Value));
                    break;
                case "uses":
                    parser.ConsumeSequence(() => file.Uses.Add(new Reference(parser.Consume<Scalar>())));
                    break;
                case "options":
                    parser.Consume<MappingStart>();
                    var scalar = parser.Consume<Scalar>();
                    if (scalar.Value == "endpoints")
                    {
                        parser.ConsumeMapping(prop =>
                        {
                            parser.TryConsume<Scalar>(out var value);
                            switch (prop.Value)
                            {
                                case "fileName":
                                    file.Options.Endpoints.FileName = value!.Value;
                                    break;
                                case "prefix":
                                    file.Options.Endpoints.Prefix = new LocatedString(value!);
                                    break;
                            }
                        });
                    }

                    parser.Consume<MappingEnd>();
                    break;
            }
        });

        parser.Consume<DocumentEnd>();

        while (parser.TryConsume<DocumentStart>(out var _))
        {
            parser.Consume<MappingStart>();
            var scalar = parser.Consume<Scalar>();
            var location = new Reference(scalar);
            ILoader loader = scalar.Value switch
            {
                "annotation" => annotationLoader,
                "class" => classLoader,
                "converter" => converterLoader,
                "dataFlow" => dataFlowLoader,
                "decorator" => decoratorLoader,
                "domain" => domainLoader,
                "endpoint" => endpointLoader,
                _ => throw new ModelException(
                    file,
                    $"Type de document inconnu ('{scalar.Value}').",
                    new Reference(scalar)
                ),
            };

            loader.Load(parser, file, location);
            parser.Consume<MappingEnd>();
            parser.Consume<DocumentEnd>();
        }

        if (file.Options.Endpoints.FileName == null)
        {
            var fileSplit = file.Name.Split("/")[^1];
            file.Options.Endpoints.FileName = string.Join(
                '_',
                fileSplit.Split("_").Skip(fileSplit.Contains('_') ? 1 : 0)
            );
        }

        var lines = content.ReplaceLineEndings(Environment.NewLine).Split(Environment.NewLine);

        for (var i = 0; i < lines.Length; i++)
        {
            var commentIndex = lines[i].IndexOf('#');
            if (commentIndex >= 0)
            {
                file.Comments.Add(i + 1, lines[i][commentIndex..].TrimStart('#').Trim());
            }
        }

        return file;
    }

    private void RemoveFromCache(string fullPath)
    {
        Cache.Remove(fullPath.Replace('\\', '/'));
    }
}
