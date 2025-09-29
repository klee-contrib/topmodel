using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using NeoSmart.AsyncLock;
using TopModel.Core.FileModel;
using TopModel.Core.Loaders;
using TopModel.Core.Model;
using TopModel.Core.Resolvers;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core;

public class ModelStore(
    IMemoryCache fsCache,
    ModelFileLoader modelFileLoader,
    ILogger<ModelStore> logger,
    ModelConfig config,
    IEnumerable<IModelWatcher> modelWatchers,
    TranslationStore translationStore
) : IDisposable
{
    private readonly AsyncLock _lockInit = new();
    private readonly AsyncLock _lockUpdate = new();
    private readonly Dictionary<string, ModelFile> _modelFiles = [];
    private readonly IEnumerable<IModelWatcher> _modelWatchers = modelWatchers.Where(mw => !mw.Disabled);
    private readonly ConcurrentQueue<(string FullPath, string? FileName, ModelFile? ModelFile)> _pendingUpdates = new();
    private LoggingScope? _storeConfig;
    private TopModelLock? _topModelLock;

#pragma warning disable MA0046
    public event Action<bool>? OnResolve;
#pragma warning restore MA0046

    public FileSystemWatcher? FileSystemWatcher { get; private set; }

    public bool DisableLockfile { get; set; }

    public IEnumerable<ModelFile> Files => _modelFiles.Values;

    public IEnumerable<Annotation> Annotations => Files.SelectMany(mf => mf.Annotations).Distinct();

    public IEnumerable<Class> Classes => Files.SelectMany(mf => mf.Classes).Distinct();

    public IEnumerable<Endpoint> Endpoints => Files.SelectMany(mf => mf.Endpoints).Distinct();

    public IEnumerable<DataFlow> DataFlows => Files.SelectMany(mf => mf.DataFlows).Distinct();

    public IDictionary<string, Domain> Domains =>
        Files.SelectMany(mf => mf.Domains).DistinctBy(d => (string)d.Name).ToDictionary(d => (string)d.Name, d => d);

    public IList<Converter> Converters => Files.SelectMany(mf => mf.Converters).ToList();

    public IEnumerable<Decorator> Decorators => Files.SelectMany(mf => mf.Decorators).Distinct();

    public IEnumerable<IProperty> Properties => Files.SelectMany(mf => mf.Properties);

    public IEnumerable<IAnnotationContainer> AnnotationContainers =>
        Files.SelectMany(mf => mf.AnnotationContainers).Distinct();

    public IEnumerable<IPropertyContainer> PropertyContainers =>
        Files.SelectMany(mf => mf.PropertyContainers).Distinct();

    public IEnumerable<IVariableContainer> VariableContainers =>
        Files.SelectMany(mf => mf.VariableContainers).Distinct();

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        FileSystemWatcher?.Dispose();
    }

    public IEnumerable<Annotation> GetAvailableAnnotations(ModelFile file)
    {
        return GetDependencies(file).SelectMany(m => m.Annotations).Concat(file.Annotations);
    }

    public IEnumerable<Class> GetAvailableClasses(ModelFile file)
    {
        return GetDependencies(file).SelectMany(m => m.Classes).Concat(file.Classes);
    }

    public IEnumerable<DataFlow> GetAvailableDataFlows(ModelFile file)
    {
        return GetDependencies(file).SelectMany(m => m.DataFlows).Concat(file.DataFlows);
    }

    public IEnumerable<Decorator> GetAvailableDecorators(ModelFile file)
    {
        return GetDependencies(file).SelectMany(m => m.Decorators).Concat(file.Decorators);
    }

    public IEnumerable<Endpoint> GetAvailableEndpoints(ModelFile file)
    {
        return GetDependencies(file).SelectMany(m => m.Endpoints).Concat(file.Endpoints);
    }

    public IDictionary<string, Annotation> GetReferencedAnnotations(ModelFile modelFile)
    {
        return GetDependencies(modelFile)
            .SelectMany(m => m.Annotations)
            .Concat(modelFile.Annotations)
            .Distinct()
            .GroupBy(c => c.Name.Value)
            .ToDictionary(c => c.Key, c => c.First());
    }

    public IDictionary<string, Class> GetReferencedClasses(ModelFile modelFile)
    {
        return GetDependencies(modelFile)
            .SelectMany(m => m.Classes)
            .Concat(modelFile.Classes)
            .Distinct()
            .GroupBy(c => c.Name.Value)
            .ToDictionary(c => c.Key, c => c.First());
    }

    public IDictionary<string, Decorator> GetReferencedDecorators(ModelFile modelFile)
    {
        return GetDependencies(modelFile)
            .SelectMany(m => m.Decorators)
            .Concat(modelFile.Decorators)
            .Distinct()
            .GroupBy(c => c.Name.Value)
            .ToDictionary(c => c.Key, c => c.First());
    }

    public IDictionary<string, Endpoint> GetReferencedEndpoints(ModelFile modelFile)
    {
        return GetDependencies(modelFile)
            .SelectMany(m => m.Endpoints)
            .Concat(modelFile.Endpoints)
            .Distinct()
            .GroupBy(c => c.Name.Value)
            .ToDictionary(c => c.Key, c => c.First());
    }

    public async Task LoadFromConfig(
        bool watch = false,
        TopModelLock? topModelLock = null,
        LoggingScope? storeConfig = null,
        CancellationToken ct = default
    )
    {
        _storeConfig = storeConfig;
        _topModelLock = topModelLock;

        using var scope = logger.BeginScope(_storeConfig!);

        var watchers = _modelWatchers
            .Select(mw => mw.FullName.Split("@"))
            .GroupBy(split => split[0])
            .Select(grp => $"{grp.Key}@{{{string.Join(',', grp.Select(split => split[1]))}}}");
        if (watchers.Any())
        {
            logger.LogInformation(
                $"Watchers enregistrés : \n                          - {string.Join("\n                          - ", watchers.Order())}"
            );
        }
        else
        {
            logger.LogWarning($"Aucun watcher enregistré pour cette configuration");
        }

        if (watch)
        {
            FileSystemWatcher = new FileSystemWatcher(config.ModelRoot, "*.tmd");
            FileSystemWatcher.Changed += OnFileChanged;
            FileSystemWatcher.Created += OnFileChanged;
            FileSystemWatcher.Deleted += OnFileChanged;
            FileSystemWatcher.Renamed += OnFileChanged;
            FileSystemWatcher.IncludeSubdirectories = true;
            FileSystemWatcher.EnableRaisingEvents = true;
        }

        _modelFiles.Clear();

        logger.LogInformation("Chargement du modèle...");

        using (await _lockInit.LockAsync(ct))
        {
            var files = await Directory
                .EnumerateFiles(config.ModelRoot, "*.tmd", SearchOption.AllDirectories)
                .ToAsyncEnumerable()
                .SelectAwait(async fullPath => await LoadFile(fullPath, WatcherChangeTypes.Created))
                .ToListAsync(cancellationToken: ct);

            await LoadTranslations(ct);
            await ApplyUpdates(files, ct);
        }
    }

    public async Task OnModelFileChange(string fullPath, string content, CancellationToken ct = default)
    {
        await ApplyUpdates([await LoadFile(fullPath, WatcherChangeTypes.Changed, content, ct)], ct);
    }

    public async Task WaitForUpdates(CancellationToken ct = default)
    {
        using (await _lockUpdate.LockAsync(ct))
        {
            // Juste pour attendre la dispo du lock.
        }
    }

    private async Task ApplyUpdates(
        IEnumerable<(string FullPath, string? FileName, ModelFile? ModelFile)> updates,
        CancellationToken ct = default
    )
    {
        using (await _lockInit.LockAsync(ct))
        {
            foreach (var update in updates)
            {
                _pendingUpdates.Enqueue(update);
            }
        }

        using (await _lockUpdate.LockAsync(ct))
        {
            var files = new List<(string FullPath, string? FileName, ModelFile? ModelFile)>();
            while (_pendingUpdates.TryDequeue(out var file))
            {
                files.Add(file);
            }

            var pendingFileChanges = new Dictionary<string, string>();
            var pendingFileDeletes = new HashSet<string>();

            foreach (var (fullPath, fileName, modelFile) in files)
            {
                if (fileName != null)
                {
                    pendingFileChanges[fullPath] = fileName;

                    if (modelFile != null)
                    {
                        _modelFiles[fileName] = modelFile;
                    }
                    else
                    {
                        _modelFiles.Remove(fileName);
                        pendingFileDeletes.Add(fileName);
                    }
                }
            }

            foreach (var modelWatcher in _modelWatchers)
            {
                modelWatcher.OnFilesDeleted(pendingFileDeletes);
            }

            if (pendingFileChanges.Count == 0)
            {
                return;
            }

            try
            {
                var referenceErrors = new List<ModelError>();

                var affectedFiles = pendingFileChanges
                    .Values.Select(pu => _modelFiles.TryGetValue(pu, out var mf) ? mf : null)
                    .Any(mf => mf?.Domains.Count > 0 || mf?.Converters.Count > 0)
                    ? _modelFiles
                    : GetAffectedFiles(pendingFileChanges.Values).Distinct().ToDictionary(f => f.Name, f => f);

                var sortedFiles = CoreUtils.SortWithCycles(
                    affectedFiles.Values,
                    f => GetDependencies(f).Where(d => affectedFiles.ContainsKey(d.Name))
                );

                foreach (var affectedFile in sortedFiles)
                {
                    referenceErrors.AddRange(ResolveReferences(affectedFile));
                }

                referenceErrors.AddRange(GetGlobalErrors());

                Parallel.ForEach(
                    _modelWatchers,
                    modelWatcher =>
                    {
                        modelWatcher.OnErrors(
                            affectedFiles
                                .Values.Select(file =>
                                    (
                                        file,
                                        errors: referenceErrors.Where(e =>
                                            e.File == file && !config.NoWarn.Contains(e.ErrorType)
                                        )
                                    )
                                )
                                .ToDictionary(i => i.file, i => i.errors)
                        );
                    }
                );

                foreach (var error in referenceErrors.Where(e => e.IsError))
                {
                    logger.LogError(error.ToString());
                }

                foreach (var error in referenceErrors.Where(e => !e.IsError && !config.NoWarn.Contains(e.ErrorType)))
                {
                    logger.LogWarning(error.ToString());
                }

                var hasError = referenceErrors.Exists(r => r.IsError);
                OnResolve?.Invoke(hasError);

                if (hasError)
                {
                    throw new ModelException("Erreur lors de la lecture du modèle.");
                }

                logger.LogInformation("Modèle chargé avec succès.");

                Parallel.ForEach(
                    _modelWatchers,
                    modelWatcher =>
                    {
                        modelWatcher.OnFilesChanged(sortedFiles.SelectMany(x => x), _storeConfig);
                    }
                );

                var generatedFiles = _modelWatchers
                    .Where(m => m.GeneratedFiles != null)
                    .SelectMany(m => m.GeneratedFiles!);
                if (generatedFiles.Any() && !DisableLockfile && _topModelLock != null)
                {
                    _topModelLock.UpdateFiles(generatedFiles);
                }

                logger.LogInformation($"Mise à jour terminée avec succès.");
                logger.LogInformation(string.Empty);

                pendingFileChanges.Clear();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }
    }

    private IEnumerable<ModelFile> GetAffectedFiles(IEnumerable<string> fileNames, HashSet<string>? foundFiles = null)
    {
        foundFiles ??= [];

        foreach (
            var file in _modelFiles.Values.Where(f =>
                fileNames.Contains(f.Name) || f.Uses.Any(d => fileNames.Contains(d.ReferenceName))
            )
        )
        {
            if (!foundFiles.Contains(file.Name))
            {
                foundFiles.Add(file.Name);
                yield return file;

                foreach (var use in GetAffectedFiles([file.Name], foundFiles))
                {
                    yield return use;
                }
            }
        }
    }

    private IEnumerable<ModelFile> GetDependencies(ModelFile modelFile)
    {
        return
        [
            .. modelFile
                .Uses.Select(dep => _modelFiles.TryGetValue(dep.ReferenceName, out var depFile) ? depFile : null!)
                .Where(dep => dep != null),
            .. Files.Where(f =>
                f != modelFile
                && f.Converters.Count > 0
                && modelFile.Classes.Any(c => c.FromMappers.Count > 0 || c.ToMappers.Count > 0)
            ),
            .. Files.Where(f =>
                f != modelFile
                && f.Domains.Count > 0
                && (modelFile.Domains.Count == 0 || modelFile.Domains.Any(d => d.AsDomainReferences.Count > 0))
            ),
            .. Files.Where(f => f != modelFile && f.Annotations.Any(a => a.Global)),
        ];
    }

    private IEnumerable<ModelError> GetGlobalErrors()
    {
        foreach (var g in Files.SelectMany(f => f.Domains).GroupBy(f => (string)f.Name).Where(g => g.Count() > 1))
        {
            foreach (var domain in g.Skip(1))
            {
                yield return new ModelError(ErrorType.TMD6001, domain, $"Le domaine '{domain}' est déjà défini");
            }
        }

        foreach (var converter in Converters)
        {
            var dup = Converters.FirstOrDefault(c =>
                Converters.IndexOf(c) < Converters.IndexOf(converter)
                && c.Conversions.Intersect(converter.Conversions).Any()
            );
            if (dup != null)
            {
                foreach (var (from, to) in dup.Conversions.Intersect(converter.Conversions))
                {
                    yield return new ModelError(
                        ErrorType.TMD6002,
                        converter,
                        $"La définition de la conversion entre {from.Name} et {to.Name} est déjà définie dans un autre converter"
                    );
                }
            }
        }

        foreach (
            var classe in Classes.Where(c => c.Trigram != null && Classes.Any(u => u.Trigram == c.Trigram && u != c))
        )
        {
            var otherClasses = Classes.Where(u => u.Trigram == classe.Trigram && u != classe);
            yield return new ModelError(
                ErrorType.TMD3004,
                classe.ModelFile,
                $"Le trigram '{classe.Trigram}' est déjà utilisé dans {(otherClasses.Count() > 1 ? "les classes suivantes : " : "la classe : ")}{string.Join(", ", otherClasses.Select(c => c.Name))}",
                classe.Trigram.GetLocation(),
                isError: false
            );
        }

        foreach (var domain in Domains.Values.Where(domain => !this.GetDomainReferences(domain).Any()))
        {
            yield return new ModelError(
                ErrorType.TMD0009,
                domain,
                $"Le domaine '{domain.Name}' n'est pas utilisé.",
                isError: false
            );
        }

        foreach (var decorator in Decorators.Where(decorator => !this.GetDecoratorReferences(decorator).Any()))
        {
            yield return new ModelError(
                ErrorType.TMD0010,
                decorator,
                $"Le décorateur '{decorator.Name}' n'est pas utilisé.",
                isError: false
            );
        }

        foreach (var files in Files.GroupBy(file => new { file.Options.Endpoints.FileName, file.Namespace.Module }))
        {
            var endpoints = files.SelectMany(f => f.Endpoints);

            foreach (
                var endpoint in endpoints.Where(
                    (e, i) => files.SelectMany(f => f.Endpoints).Where((p, j) => p.Name == e.Name && j < i).Any()
                )
            )
            {
                yield return new ModelError(
                    ErrorType.TMD0001,
                    endpoint,
                    $"Le nom '{endpoint.Name}' est déjà utilisé.",
                    endpoint.Name.GetLocation()
                );
            }

            if (files.Select(file => file.Options.Endpoints.Prefix).Distinct().Count() > 1)
            {
                foreach (var file in files)
                {
                    if (file.Options.Endpoints.Prefix != null)
                    {
                        yield return new ModelError(
                            ErrorType.TMD7001,
                            file,
                            $"Le préfixe d'endpoint '{file.Options.Endpoints.Prefix}' doit être identique à celui de tous les fichiers de même nom et de même module.",
                            file.Options.Endpoints.Prefix?.GetLocation()
                        );
                    }
                    else
                    {
                        yield return new ModelError(
                            ErrorType.TMD7001,
                            file,
                            $"Le fichier ne définit pas de préfixe d'endpoint alors que d'autres fichiers de même nom et de même module le font."
                        );
                    }
                }
            }
        }
    }

    private async Task<(string FullPath, string? FileName, ModelFile? ModelFile)> LoadFile(
        string fullPath,
        WatcherChangeTypes changeType,
        string? content = null,
        CancellationToken ct = default
    )
    {
        var fileName = config.GetFileName(fullPath);

        try
        {
            if (changeType != WatcherChangeTypes.Deleted)
            {
                ModelFile? modelFile = null;
                if (File.Exists(fullPath))
                {
                    modelFile = await modelFileLoader.LoadModelFile(fullPath, content, ct);
                }

                if (modelFile != null)
                {
                    return (fullPath, modelFile.Name, modelFile);
                }
            }

            return (fullPath, fileName, null);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return (fullPath, null, null);
        }
    }

    private async Task LoadTranslations(CancellationToken ct = default)
    {
        translationStore.Translations[config.I18n.DefaultLang] = [];

        foreach (var lang in config.I18n.Langs)
        {
            var langMap = new Dictionary<string, string>();
            var directoryPath = config.I18n.RootPath.Replace("{lang}", lang);
            var exists = Directory.Exists(directoryPath);
            if (exists)
            {
                var files = Directory.GetFiles(directoryPath, "*.properties", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var lines = await File.ReadAllLinesAsync(file, ct);
                    foreach (var line in lines)
                    {
                        if (line != null && line != string.Empty)
                        {
                            langMap[line.Split("=")[0]] = line.Split("=")[1];
                        }
                    }
                }
            }

            translationStore.Translations[lang] = langMap;
        }
    }

    private void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        fsCache.Set(
            $"{e.FullPath}:{e.ChangeType}",
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

                        var files = new List<(string, string?, ModelFile?)>();

                        if (e is RenamedEventArgs re)
                        {
                            files.Add(await LoadFile(re.OldFullPath, WatcherChangeTypes.Deleted, ct: default));
                            files.Add(await LoadFile(re.FullPath, WatcherChangeTypes.Created, ct: default));
                        }
                        else
                        {
                            files.Add(await LoadFile(e.FullPath, e.ChangeType, ct: default));
                        }

                        await ApplyUpdates(files, default);
                    }
                )
        );
    }

    private IEnumerable<ModelError> ResolveReferences(IList<ModelFile> modelFiles)
    {
        foreach (var modelFile in modelFiles)
        {
            var nonExistingFiles = modelFile.Uses.Where(use => !_modelFiles.ContainsKey(use.ReferenceName));
            foreach (var use in nonExistingFiles)
            {
                yield return new ModelError(
                    ErrorType.TMD1001,
                    modelFile,
                    $"Le fichier référencé '{use.ReferenceName}' est introuvable.",
                    use
                );
            }

            var duplicatedUses = modelFile
                .Uses.GroupBy(u => u.ReferenceName)
                .Select(u => new { ReferenceName = u.Key, Count = u.Count() })
                .Where(r => r.Count > 1)
                .Select(u => u.ReferenceName);

            foreach (var use in modelFile.Uses.Where(u => duplicatedUses.Contains(u.ReferenceName)).Skip(1))
            {
                yield return new ModelError(
                    ErrorType.TMD1002,
                    modelFile,
                    $"L'import '{use.ReferenceName}' ne doit être spécifié qu'une seule fois",
                    use
                );
            }
        }

        var dependencies = modelFiles.SelectMany(GetDependencies).Distinct().Except(modelFiles).ToList();

        var referencedClassesRaw = dependencies
            .SelectMany(m => m.Classes)
            .Concat(modelFiles.SelectMany(mf => mf.Classes))
            .Distinct();

        var duplicateClasses = referencedClassesRaw
            .GroupBy(c => c.Name.Value)
            .Where(g => g.Count() > 1)
            .Select(g =>
                g.OrderByDescending(c =>
                        (modelFiles.Contains(c.ModelFile) ? 1_000_000 : 0) + c.Name.Location.Start.Line
                    )
                    .First()
            );

        foreach (var classe in duplicateClasses.Where(c => modelFiles.Contains(c.ModelFile)))
        {
            yield return new ModelError(
                ErrorType.TMD3001,
                classe,
                $"La classe '{classe}' est définie plusieurs fois dans le fichier ou une de ses dépendences.",
                classe.Name.Location
            );
        }

        var referencedClasses = referencedClassesRaw
            .Where(c => !duplicateClasses.Select(c => c.Name.Value).Contains(c.Name.Value))
            .ToDictionary(c => c.Name.Value, c => c);

        var referencedEndpointsRaw = dependencies
            .SelectMany(m => m.Endpoints)
            .Concat(modelFiles.SelectMany(mf => mf.Endpoints))
            .Distinct();

        var duplicateEndpoints = referencedEndpointsRaw
            .GroupBy(c => c.Name.Value)
            .Where(g => g.Count() > 1)
            .Select(g =>
                g.OrderByDescending(c =>
                        (modelFiles.Contains(c.ModelFile) ? 1_000_000 : 0) + c.Name.Location.Start.Line
                    )
                    .First()
            );

        var referencedEndpoints = referencedEndpointsRaw
            .Where(c => !duplicateEndpoints.Select(c => c.Name.Value).Contains(c.Name.Value))
            .ToDictionary(c => (string)c.Name, c => c);

        var referencedAnnotations = dependencies
            .SelectMany(m => m.Annotations)
            .Concat(modelFiles.SelectMany(mf => mf.Annotations))
            .Distinct()
            .ToDictionary(d => (string)d.Name, c => c);

        var referencedDecorators = dependencies
            .SelectMany(m => m.Decorators)
            .Concat(modelFiles.SelectMany(mf => mf.Decorators))
            .Distinct()
            .ToDictionary(d => (string)d.Name, c => c);

        var referencedDataFlowsRaw = dependencies
            .SelectMany(m => m.DataFlows)
            .Concat(modelFiles.SelectMany(mf => mf.DataFlows))
            .Distinct();

        var duplicateDataFlows = referencedDataFlowsRaw
            .GroupBy(c => c.Name.Value)
            .Where(g => g.Count() > 1)
            .Select(g =>
                g.OrderByDescending(c =>
                        (modelFiles.Contains(c.ModelFile) ? 1_000_000 : 0) + c.Name.Location.Start.Line
                    )
                    .First()
            );

        foreach (var dataFlow in duplicateDataFlows.Where(c => modelFiles.Contains(c.ModelFile)))
        {
            yield return new ModelError(
                ErrorType.TMD4001,
                dataFlow,
                $"Le flux de données '{dataFlow}' est défini plusieurs fois dans le fichier ou une de ses dépendences.",
                dataFlow.Name.Location
            );
        }

        var referencedDataFlows = referencedDataFlowsRaw
            .Where(c => !duplicateDataFlows.Select(c => c.Name.Value).Contains(c.Name.Value))
            .ToDictionary(c => c.Name.Value, c => c);

        var annotationResolver = new AnnotationResolver(modelFiles, config, referencedAnnotations);
        var classResolver = new ClassResolver(modelFiles, referencedClasses);
        var dataFlowResolver = new DataFlowResolver(modelFiles, referencedDataFlows, referencedClasses);
        var decoratorResolver = new DecoratorResolver(modelFiles, config, referencedDecorators);
        var domainResolver = new DomainResolver(modelFiles, config, Domains, Converters);
        var endpointResolver = new EndpointResolver(modelFiles);
        var mapperResolver = new MapperResolver(
            modelFiles,
            referencedClasses,
            Converters,
            config.UseLegacyAssociationCompositionMappers
        );
        var propertyResolver = new PropertyResolver(
            modelFiles,
            Domains,
            referencedClasses,
            referencedEndpoints,
            referencedDecorators
        );

        foreach (var error in domainResolver.ResolveDomainVariables())
        {
            yield return error;
        }

        foreach (var error in domainResolver.ResolveAsDomains())
        {
            yield return error;
        }

        foreach (var error in classResolver.ResolveExtends())
        {
            yield return error;
        }

        foreach (var error in decoratorResolver.ResolveDecorators())
        {
            yield return error;
        }

        foreach (var error in propertyResolver.ResolveNonAliasProperties())
        {
            yield return error;
        }

        foreach (var error in annotationResolver.ResolveAnnotations())
        {
            yield return error;
        }

        propertyResolver.ResetAliases();

        // Résolution des alias des décorateurs
        foreach (
            var error in propertyResolver.ResolveAliases(alp => alp.Decorator is not null && alp.Reference is not null)
        )
        {
            yield return error;
        }

        decoratorResolver.CopyDecoratorProperties();

        // Résolution des alias des classes et endpoints.
        foreach (
            var error in propertyResolver.ResolveAliases(alp => alp.Decorator is null && alp.Reference is not null)
        )
        {
            yield return error;
        }

        foreach (var error in annotationResolver.CheckAliasAnnotations())
        {
            yield return error;
        }

        foreach (var error in propertyResolver.ResolveAssociationProperties())
        {
            yield return error;
        }

        foreach (var error in classResolver.ResolveUniqueKeys())
        {
            yield return error;
        }

        foreach (var error in classResolver.ResolveValues())
        {
            yield return error;
        }

        foreach (var error in classResolver.ResolveSpecialProperties())
        {
            yield return error;
        }

        foreach (var error in classResolver.ResolveEnums())
        {
            yield return error;
        }

        foreach (var error in domainResolver.ResolveConverters())
        {
            yield return error;
        }

        foreach (var error in mapperResolver.ResolveMappers())
        {
            yield return error;
        }

        foreach (var error in dataFlowResolver.ResolveDataFlows())
        {
            yield return error;
        }

        foreach (var error in classResolver.CheckResult())
        {
            yield return error;
        }

        foreach (var error in endpointResolver.CheckResult())
        {
            yield return error;
        }

        classResolver.ResolveTranslations(translationStore, config.I18n.DefaultLang);

        foreach (var modelFile in modelFiles)
        {
            foreach (var use in this.GetUselessImports(modelFile))
            {
                yield return new ModelError(
                    ErrorType.TMD1003,
                    modelFile,
                    $"L'import '{use.ReferenceName}' n'est pas utilisé.",
                    use,
                    isError: false
                );
            }
        }
    }
}
