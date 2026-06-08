using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
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
    private static ConcurrentDictionary<
        (
            string App,
            string ModelRoot,
            bool DefaultAssociationUseClass,
            bool PluralizeTableNames,
            bool UseLegacyRoleNames
        ),
        ConcurrentDictionary<string, ModelFile>
    > GlobalCache { get; } = [];

    private static ConcurrentDictionary<
        string,
        (
            FileSystemWatcher FileWatcher,
            IMemoryCache Cache,
            IList<(
                GlobCollection ModelFilePaths,
                Func<
                    IEnumerable<(string FullPath, ModelFile? ModelFile, ModelFileStatus Status)>,
                    CancellationToken,
                    Task
                > ApplyUpdates
            )> Configs
        )
    > FileWatchers { get; } = [];

    private ConcurrentDictionary<string, ModelFile> Cache
    {
        get
        {
            var key = (
                config.App,
                config.ModelRoot,
                config.DefaultAssociationUseClass,
                config.PluralizeTableNames,
                config.UseLegacyRoleNames
            );

            if (!GlobalCache.TryGetValue(key, out var cache))
            {
                GlobalCache[key] = [];
                cache = GlobalCache[key];
            }

            return cache;
        }
    }

    internal async Task<(string FullPath, ModelFile? ModelFile, ModelFileStatus Status)> LoadModelFile(
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
            return (fullPath, CloneFile(file), ModelFileStatus.Ok);
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
                Cache.TryAdd(fullPath, modelFile);
                return (fullPath, CloneFile(modelFile), ModelFileStatus.Ok);
            }
            else
            {
                RemoveFromCache(fullPath);
                return (fullPath, modelFile, ModelFileStatus.NotFound);
            }
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            logger.LogError(e, e.Message);
            RemoveFromCache(fullPath);
            return (fullPath, null, ModelFileStatus.Errored);
        }
    }

    internal Action Watch(
        Func<
            IEnumerable<(string FullPath, ModelFile? ModelFile, ModelFileStatus Status)>,
            CancellationToken,
            Task
        > applyUpdates
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
                FileWatchers.TryRemove(config.ModelRoot, out _);
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
                var na = new Annotation
                {
                    Description = a.Description,
                    Global = a.Global,
                    Implementations = a.Implementations,
                    Location = a.Location,
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
                var nc = new Class
                {
                    Abstract = c.Abstract,
                    AnnotationReferences = c.AnnotationReferences,
                    Comment = c.Comment,
                    CustomProperties = c.CustomProperties,
                    DecoratorReferences = c.DecoratorReferences,
                    DefaultPropertyReference = c.DefaultPropertyReference,
                    EnumOverride = c.EnumOverride,
                    ExcludedAnnotationReferences = c.ExcludedAnnotationReferences,
                    ExtendsReference = c.ExtendsReference,
                    FlagPropertyReference = c.FlagPropertyReference,
                    Label = c.Label,
                    LocalePropertyReference = c.LocalePropertyReference,
                    Location = c.Location,
                    ModelFile = newFile,
                    Name = c.Name,
                    Namespace = c.Namespace,
                    OrderPropertyReference = c.OrderPropertyReference,
                    OwnTags = c.OwnTags,
                    PluralName = c.PluralName,
                    PreservePropertyCasing = c.PreservePropertyCasing,
                    PropertyAnnotationReferences = c.PropertyAnnotationReferences,
                    Readonly = c.Readonly,
                    Reference = c.Reference,
                    SqlName = c.SqlName,
                    Translation = c.Translation,
                    Trigram = c.Trigram,
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

                        nfm.Params.AddRange(
                            fm.Params.Select(p =>
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

                        foreach (var pp in nfm.PropertyParams)
                        {
                            foreach (var prop in pp.Properties)
                            {
                                prop.PropertyMapping = pp;
                            }
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

                nc.Properties.AddRange(c.Properties.Select(p => p.CloneDefinition()));
                foreach (var prop in nc.Properties)
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
            file.Converters.Select(c => new Converter
            {
                DomainsFromReferences = c.DomainsFromReferences,
                DomainsToReferences = c.DomainsToReferences,
                Implementations = c.Implementations,
                Location = c.Location,
                ModelFile = newFile,
            })
        );

        newFile.DataFlows.AddRange(
            file.DataFlows.Select(df =>
            {
                var ndf = new DataFlow
                {
                    ActivePropertyReference = df.ActivePropertyReference,
                    ClassReference = df.ClassReference,
                    DependsOnReference = df.DependsOnReference,
                    Hooks = df.Hooks,
                    Location = df.Location,
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
                var nd = new Decorator
                {
                    AnnotationReferences = d.AnnotationReferences,
                    DecoratorReferences = d.DecoratorReferences,
                    Description = d.Description,
                    ExcludedAnnotationReferences = d.ExcludedAnnotationReferences,
                    Implementations = d.Implementations,
                    Location = d.Location,
                    ModelFile = newFile,
                    Name = d.Name,
                    Namespace = d.Namespace,
                    PreservePropertyCasing = d.PreservePropertyCasing,
                    PropertyAnnotationReferences = d.PropertyAnnotationReferences,
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

                nd.Properties.AddRange(d.Properties.Select(p => p.CloneDefinition()));
                foreach (var prop in nd.Properties)
                {
                    prop.Decorator = nd;
                }

                return nd;
            })
        );

        newFile.Domains.AddRange(
            file.Domains.Select(d =>
            {
                var nd = new Domain
                {
                    AnnotationReferences = d.AnnotationReferences,
                    AsDomainReferences = d.AsDomainReferences,
                    AutoGeneratedValue = d.AutoGeneratedValue,
                    BodyParam = d.BodyParam,
                    Collection = d.Collection,
                    ExcludedAnnotationReferences = d.ExcludedAnnotationReferences,
                    Implementations = d.Implementations,
                    Label = d.Label,
                    Length = d.Length,
                    Location = d.Location,
                    MediaType = d.MediaType,
                    ModelFile = newFile,
                    Name = d.Name,
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
                var ne = new Endpoint
                {
                    AnnotationReferences = e.AnnotationReferences,
                    CustomProperties = e.CustomProperties,
                    DecoratorReferences = e.DecoratorReferences,
                    Description = e.Description,
                    ExcludedAnnotationReferences = e.ExcludedAnnotationReferences,
                    Location = e.Location,
                    Method = e.Method,
                    ModelFile = newFile,
                    Name = e.Name,
                    Namespace = e.Namespace,
                    OwnTags = e.OwnTags,
                    PreservePropertyCasing = e.PreservePropertyCasing,
                    PropertyAnnotationReferences = e.PropertyAnnotationReferences,
                    Returns = e.Returns?.CloneDefinition(),
                    Route = e.Route,
                };

                ne.Params.AddRange(e.Params.Select(p => p.CloneDefinition()));
                foreach (var prop in ne.Params)
                {
                    prop.Endpoint = ne;
                }

                ne.Returns?.Endpoint = ne;

                return ne;
            })
        );

        return newFile;
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

                        var files = new List<(string FullPath, ModelFile? ModelFile, ModelFileStatus Status)>();

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

                            await applyUpdates(
                                files.Select(f => (f.FullPath, CloneFile(f.ModelFile), f.Status)),
                                default
                            );
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
        Cache.TryRemove(fullPath.Replace('\\', '/'), out _);
    }
}
