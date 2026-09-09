using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using AsyncKeyedLock;
using Meziantou.Framework.Globbing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using OneOf;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Utils;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class ModelFileLoader(
    ILogger<ModelFileLoader> logger,
    AnnotationLoader annotationLoader,
    ClassLoader classLoader,
    DataFlowLoader dataFlowLoader,
    FileChecker fileChecker,
    DecoratorLoader decoratorLoader,
    ConverterLoader converterLoader,
    EndpointLoader endpointLoader,
    DomainLoader domainLoader,
    IModelReporter? modelReporter = null
)
{
    private readonly ConcurrentDictionary<
        string,
        (
            FileSystemWatcher FileWatcher,
            IMemoryCache Cache,
            IList<(
                ModelFileLoadConfig Config,
                GlobCollection ModelFilePaths,
                Func<
                    IEnumerable<(string FullPath, ModelFile? ModelFile, ModelFileStatus Status)>,
                    CancellationToken,
                    Task
                > ApplyUpdates
            )> Configs
        )
    > _fileWatchers = [];
    private readonly ConcurrentDictionary<ModelFileLoadConfig, ConcurrentDictionary<string, ModelFile>> _globalCache =
    [];
    private readonly AsyncKeyedLocker<(ModelFileLoadConfig Config, string FullPath)> _lockFile = new();
    private readonly AsyncKeyedLocker<string> _lockRoot = new();

    private bool _parallelWatch;

    public void RemoveFromCache(ModelFileLoadConfig config, string fullPath)
    {
        GetCache(config).TryRemove(fullPath.Replace('\\', '/'), out _);
    }

    internal async Task<(string FullPath, ModelFile? ModelFile, ModelFileStatus Status)> LoadModelFile(
        ModelFileLoadConfig config,
        string fullPath,
        WatcherChangeTypes changeType,
        string? content = null,
        CancellationToken ct = default
    )
    {
        fullPath = fullPath.Replace('\\', '/');

        using var fileLock = await _lockFile.LockAsync((config, fullPath), ct);

        if (GetCache(config).TryGetValue(fullPath, out var file))
        {
            return (fullPath, CloneFile(file), ModelFileStatus.Ok);
        }

        try
        {
            ModelFile? modelFile = null;
            if (changeType != WatcherChangeTypes.Deleted && File.Exists(fullPath))
            {
                content ??= await File.ReadAllTextAsync(fullPath, ct);
                modelFile = ReadModelFile(fullPath, config, content);
            }

            if (modelFile != null)
            {
                GetCache(config).TryAdd(fullPath, modelFile);
                return (fullPath, CloneFile(modelFile), ModelFileStatus.Ok);
            }
            else
            {
                RemoveFromCache(config, fullPath);
                return (fullPath, modelFile, ModelFileStatus.NotFound);
            }
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            logger.LogError(e, e.Message);
            RemoveFromCache(config, fullPath);
            return (fullPath, null, ModelFileStatus.Errored);
        }
    }

    internal Action Watch(
        bool parallel,
        ModelFileLoadConfig config,
        GlobCollection modelFilePaths,
        Func<
            IEnumerable<(string FullPath, ModelFile? ModelFile, ModelFileStatus Status)>,
            CancellationToken,
            Task
        > applyUpdates
    )
    {
        _parallelWatch = parallel;

        var watchConfig = (config, modelFilePaths, applyUpdates);

        using var rootLock = _lockRoot.Lock(config.ModelRoot, default);

        if (!_fileWatchers.TryGetValue(config.ModelRoot, out var fw))
        {
            var fileWatcher = new FileSystemWatcher(config.ModelRoot, "*.tmd")
            {
                IncludeSubdirectories = true,
                EnableRaisingEvents = true,
            };

            _fileWatchers[config.ModelRoot] = (fileWatcher, new MemoryCache(new MemoryCacheOptions()), [watchConfig]);
            fw = _fileWatchers[config.ModelRoot];

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
                _fileWatchers.TryRemove(config.ModelRoot, out _);
            }
        };
    }

    [return: NotNullIfNotNull(nameof(file))]
    private static ModelFile? CloneFile(ModelFile? file)
    {
        if (file == null)
        {
            return null;
        }

        var newFile = new ModelFile
        {
            Comments = file.Comments,
            Name = file.Name,
            Namespace = file.Namespace,
            Options = file.Options,
            Path = file.Path,
            Tags = file.Tags,
            Uses = file.Uses,
        };

        newFile.Annotations.AddRange(
            file.Annotations.Select(a =>
            {
                var na = new Annotation(a.Location)
                {
                    Description = a.Description,
                    Global = a.Global,
                    Implementations = a.Implementations,
                    ModelFile = newFile,
                    Name = a.Name,
                    Namespace = a.Namespace,
                    Target = a.Target,
                };

                na.TemplateParameters.AddRange(
                    a.TemplateParameters.Select(tp => new TemplateParameter
                    {
                        Annotation = na,
                        Comment = tp.Comment,
                        DefaultValue = tp.DefaultValue,
                        Name = tp.Name,
                        Required = tp.Required,
                    })
                );

                return na;
            })
        );

        newFile.Classes.AddRange(
            file.Classes.Select(c =>
            {
                var nc = new Class(c.Location)
                {
                    AnnotationReferences = c.AnnotationReferences,
                    Comment = c.Comment,
                    CustomProperties = c.CustomProperties,
                    DecoratorReferences = c.DecoratorReferences,
                    DefaultPropertyReference = c.DefaultPropertyReference,
                    DiscriminatorPropertyReference = c.DiscriminatorPropertyReference,
                    DiscriminatorValue = c.DiscriminatorValue,
                    EnumOverride = c.EnumOverride,
                    ExcludedAnnotationReferences = c.ExcludedAnnotationReferences,
                    ExtendsReference = c.ExtendsReference,
                    FlagPropertyReference = c.FlagPropertyReference,
                    ImplementReferences = c.ImplementReferences,
                    InheritanceStrategy = c.InheritanceStrategy,
                    Label = c.Label,
                    LocalePropertyReference = c.LocalePropertyReference,
                    ModelFile = newFile,
                    Name = c.Name,
                    Namespace = c.Namespace,
                    OrderPropertyReference = c.OrderPropertyReference,
                    OwnTags = c.OwnTags,
                    PluralName = c.PluralName,
                    PreservePropertyCasing = c.PreservePropertyCasing,
                    PropertyAnnotationReferences = c.PropertyAnnotationReferences,
                    PropertySourceOrder = c.PropertySourceOrder,
                    Readonly = c.Readonly,
                    Reference = c.Reference,
                    SqlName = c.SqlName,
                    Translation = c.Translation,
                    Trigram = c.Trigram,
                    Type = c.Type,
                    ValueReferences = c.ValueReferences,
                };

                nc.FromMappers.AddRange(
                    c.FromMappers.Select(fm =>
                    {
                        var nfm = new FromMapper
                        {
                            Class = nc,
                            Comment = fm.Comment,
                            Reference = fm.Reference,
                        };

                        nfm.OwnParams.AddRange(
                            fm.OwnParams.Select(p =>
                                p.Match<OneOf<ClassMappings, PropertyMapping>>(
                                    pc => new ClassMappings
                                    {
                                        ClassReference = pc.ClassReference,
                                        Comment = pc.Comment,
                                        MappingReferences = pc.MappingReferences,
                                        Name = pc.Name,
                                        Required = pc.Required,
                                    },
                                    pp => new PropertyMapping
                                    {
                                        FromMapper = nfm,
                                        Property = pp.Property.CloneDefinition(),
                                        TargetPropertyReference = pp.TargetPropertyReference,
                                    }
                                )
                            )
                        );

                        foreach (var pp in nfm.OwnPropertyParams)
                        {
                            pp.Property.PropertyMapping = pp;
                        }

                        return nfm;
                    })
                );

                nc.Indexes.AddRange(
                    c.Indexes.Select(i => new IndexDefinition
                    {
                        Class = nc,
                        PropertyReferences = i.PropertyReferences,
                        Unique = i.Unique,
                    })
                );

                nc.OwnProperties.AddRange(c.OwnProperties.Select(p => p.CloneDefinition()));
                foreach (var prop in nc.OwnProperties)
                {
                    prop.Class = nc;
                }

                nc.ToMappers.AddRange(
                    c.ToMappers.Select(tm => new ClassMappings
                    {
                        ClassReference = tm.ClassReference,
                        Comment = tm.Comment,
                        MappingReferences = tm.MappingReferences,
                        Name = tm.Name,
                        Required = tm.Required,
                        To = true,
                    })
                );

                return nc;
            })
        );

        newFile.Converters.AddRange(
            file.Converters.Select(c => new Converter(c.Location)
            {
                DomainsFromReferences = c.DomainsFromReferences,
                DomainsToReferences = c.DomainsToReferences,
                Implementations = c.Implementations,
                ModelFile = newFile,
            })
        );

        newFile.DataFlows.AddRange(
            file.DataFlows.Select(df =>
            {
                var ndf = new DataFlow(df.Location)
                {
                    ActivePropertyReference = df.ActivePropertyReference,
                    ClassReference = df.ClassReference,
                    DependsOnReference = df.DependsOnReference,
                    Hooks = df.Hooks,
                    ModelFile = newFile,
                    Name = df.Name,
                    Target = df.Target,
                    Type = df.Type,
                };

                ndf.Sources.AddRange(
                    df.Sources.Select(s => new DataFlowSource
                    {
                        ClassReference = s.ClassReference,
                        DataFlow = ndf,
                        InnerJoin = s.InnerJoin,
                        JoinPropertyReferences = s.JoinPropertyReferences,
                        Mode = s.Mode,
                        Source = s.Source,
                    })
                );

                return ndf;
            })
        );

        newFile.Decorators.AddRange(
            file.Decorators.Select(d =>
            {
                var nd = new Decorator(d.Location)
                {
                    AnnotationReferences = d.AnnotationReferences,
                    DecoratorReferences = d.DecoratorReferences,
                    Description = d.Description,
                    ExcludedAnnotationReferences = d.ExcludedAnnotationReferences,
                    Implementations = d.Implementations,
                    ModelFile = newFile,
                    Name = d.Name,
                    Namespace = d.Namespace,
                    PreservePropertyCasing = d.PreservePropertyCasing,
                    PropertyAnnotationReferences = d.PropertyAnnotationReferences,
                    PropertySourceOrder = d.PropertySourceOrder,
                    Target = d.Target,
                };

                nd.TemplateParameters.AddRange(
                    d.TemplateParameters.Select(tp => new TemplateParameter
                    {
                        Comment = tp.Comment,
                        Decorator = nd,
                        DefaultValue = tp.DefaultValue,
                        Name = tp.Name,
                        Required = tp.Required,
                    })
                );

                nd.OwnProperties.AddRange(d.OwnProperties.Select(p => p.CloneDefinition()));
                foreach (var prop in nd.OwnProperties)
                {
                    prop.Decorator = nd;
                }

                return nd;
            })
        );

        newFile.Domains.AddRange(
            file.Domains.Select(d =>
            {
                var nd = new Domain(d.Location)
                {
                    AnnotationReferences = d.AnnotationReferences,
                    AsDomainReferences = d.AsDomainReferences,
                    GeneratedValue = d.GeneratedValue,
                    Collection = d.Collection,
                    DefaultValue = d.DefaultValue,
                    ExcludedAnnotationReferences = d.ExcludedAnnotationReferences,
                    Implementations = d.Implementations,
                    Label = d.Label,
                    Length = d.Length,
                    MediaType = d.MediaType,
                    ModelFile = newFile,
                    Name = d.Name,
                    ParamLocation = d.ParamLocation,
                    Scale = d.Scale,
                };

                nd.TemplateParameters.AddRange(
                    d.TemplateParameters.Select(tp => new TemplateParameter
                    {
                        Comment = tp.Comment,
                        DefaultValue = tp.DefaultValue,
                        Domain = nd,
                        Name = tp.Name,
                        Required = tp.Required,
                    })
                );

                return nd;
            })
        );

        newFile.Endpoints.AddRange(
            file.Endpoints.Select(e =>
            {
                var ne = new Endpoint(e.Location)
                {
                    AnnotationReferences = e.AnnotationReferences,
                    CustomProperties = e.CustomProperties,
                    DecoratorReferences = e.DecoratorReferences,
                    Description = e.Description,
                    ExcludedAnnotationReferences = e.ExcludedAnnotationReferences,
                    Method = e.Method,
                    ModelFile = newFile,
                    Name = e.Name,
                    Namespace = e.Namespace,
                    OwnTags = e.OwnTags,
                    PreservePropertyCasing = e.PreservePropertyCasing,
                    PropertyAnnotationReferences = e.PropertyAnnotationReferences,
                    PropertySourceOrder = e.PropertySourceOrder,
                    OwnReturns = e.OwnReturns?.CloneDefinition(),
                    Route = e.Route,
                };

                ne.OwnParams.AddRange(e.OwnParams.Select(p => p.CloneDefinition()));
                foreach (var prop in ne.OwnParams)
                {
                    prop.Endpoint = ne;
                }

                ne.OwnReturns?.Endpoint = ne;

                return ne;
            })
        );

        return newFile;
    }

    private ConcurrentDictionary<string, ModelFile> GetCache(ModelFileLoadConfig config)
    {
        if (!_globalCache.TryGetValue(config, out var cache))
        {
            _globalCache[config] = [];
            cache = _globalCache[config];
        }

        return cache;
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

                        foreach (var (config, _, _) in _fileWatchers[modelRoot].Configs)
                        {
                            if (e is RenamedEventArgs re)
                            {
                                RemoveFromCache(config, re.OldFullPath);
                            }

                            RemoveFromCache(config, e.FullPath);
                        }

                        async Task HandleChange(
                            (
                                ModelFileLoadConfig Config,
                                GlobCollection ModelFilePaths,
                                Func<
                                    IEnumerable<(string FullPath, ModelFile? ModelFile, ModelFileStatus Status)>,
                                    CancellationToken,
                                    Task
                                > ApplyUpdates
                            ) c
                        )
                        {
                            var files = new List<(string FullPath, ModelFile? ModelFile, ModelFileStatus Status)>();

                            if (e is RenamedEventArgs re)
                            {
                                files.Add(
                                    await LoadModelFile(
                                        c.Config,
                                        re.OldFullPath,
                                        WatcherChangeTypes.Deleted,
                                        ct: default
                                    )
                                );
                                files.Add(
                                    await LoadModelFile(c.Config, re.FullPath, WatcherChangeTypes.Created, ct: default)
                                );
                            }
                            else
                            {
                                files.Add(await LoadModelFile(c.Config, e.FullPath, e.ChangeType, ct: default));
                            }

                            if (!c.ModelFilePaths.IsMatch(e.FullPath.ToRelative(modelRoot)[2..]))
                            {
                                return;
                            }

                            await c.ApplyUpdates(files.Select(f => (f.FullPath, f.ModelFile, f.Status)), default);
                        }

                        if (_parallelWatch)
                        {
                            await Parallel.ForEachAsync(
                                _fileWatchers[modelRoot].Configs,
                                async (c, ct) => await HandleChange(c)
                            );
                        }
                        else
                        {
                            foreach (var c in _fileWatchers[modelRoot].Configs)
                            {
                                await HandleChange(c);
                            }
                        }

                        modelReporter?.Report();
                    }
                )
        );
    }

    private ModelFile? ReadModelFile(string filePath, ModelFileLoadConfig config, string content)
    {
        var fileName = config.GetFileName(filePath);

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

            loader.Load(parser, file, config, location);
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
}
