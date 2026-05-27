using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Nodes;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Spectre.Console;
using TopModel.Core;
using TopModel.Core.Loaders;
using TopModel.Generator.Core;
using TopModel.Utils;
using TopModel.Utils.Cli;

namespace TopModel.Generator;

public class ModgenWorker : TopModelWorker<ModelConfig, FileChecker>
{
    private static readonly ReferencedTagInterceptor interceptor = new();
    private static readonly ProxyGenerator proxyGenerator = new();
    private readonly IList<ModgenDependency> _deps = [];
    private readonly IList<Type> _generators = [];
    private readonly IList<IDisposable> _providers = [];
    private readonly Dictionary<string, string> _resolvedConfigKeys = [];

#nullable disable

    private IEnumerable<CustomModule> _customModules;
    private Microsoft.Extensions.Logging.ILogger _logger;
    private string _modgenRoot;
    private IServiceCollection _services;
    private TopModelLock _topModelLock;

#nullable enable

    private bool hasInstalled = false;

    public string? UpdateMode { get; set; }

    public bool SchemaMode { get; set; }

    public bool WatchMode { get; set; }

    public IEnumerable<string> ExcludedTags { get; set; } = [];

    /// <inheritdoc cref="IDisposable.Dispose" />
    public override void Dispose()
    {
        foreach (var provider in _providers)
        {
            provider.Dispose();
        }
    }

    public override void Init()
    {
        _logger = LoggerProvider.CreateLogger("TopModel.Generator");

        var scope = _logger.BeginScope(StoreConfig);
        if (scope != null)
        {
            _providers.Add(scope);
        }

        _topModelLock = new TopModelLock(Config, _logger);
        _modgenRoot = Path.GetFullPath(".modgen", Config.ModelRoot);
        AddDevCustomGenerators();
        _customModules = Config
            .CustomGenerators.Select(cg => new CustomModule(cg, ConfigFullName, _modgenRoot, _logger, _topModelLock))
            .ToList();
        _services = new ServiceCollection()
            .AddTransient(typeof(ILogger<>), typeof(Logger<>))
            .AddTransient<ILoggerFactory, LoggerFactory>()
            .AddSingleton<ILoggerProvider>(LoggerProvider)
            .AddSingleton<IFileWriterProvider>(new GeneratedFileWriterProvider(Config))
            .AddModelStore(FileChecker, Config);
    }

    public async Task LoadModules(CancellationToken cancellationToken)
    {
        if (_deps.Count > 0)
        {
            Directory.CreateDirectory(_modgenRoot);
        }

        foreach (var dep in _deps)
        {
            await LoadModule(dep, cancellationToken);
        }

        _topModelLock.Write();
        if (_resolvedConfigKeys.Any())
        {
            _logger.LogInformation(
                GeneratorMessage.GeneratorsInUse,
                $"{Environment.NewLine}                          {string.Join($"{Environment.NewLine}                          ", _resolvedConfigKeys.Select(rck => $"- {rck.Key}: {rck.Value}"))}"
            );
        }

        CheckDependenciesToUpdate();
    }

    public override async Task Run(CancellationToken cancellationToken)
    {
        await InitModules(cancellationToken);
        if (HasError)
        {
            return;
        }
        await PrepareConfiguration();
        if (HasError)
        {
            return;
        }
        await RunGeneration(cancellationToken);
    }

    public async Task WriteSchema(CancellationToken cancellationToken)
    {
        _logger.LogInformation(GeneratorMessage.GeneratingConfigSchema);
        var schema = JsonNode
            .Parse(
                await File.ReadAllTextAsync(
                    Assembly.GetExecutingAssembly().GetFilePath("schema.config.json"),
                    cancellationToken
                )
            )!
            .AsObject();

        schema.Remove("additionalProperties");
        schema.Add("additionalProperties", value: false);

        foreach (var generator in _generators)
        {
            var (configType, configName) = ModuleUtils.GetIGenRegInterfaceAndName(generator);

            var configSchema = JsonNode.Parse(@"{""type"": ""array""}")!.AsObject();
            configSchema.Add(
                "items",
                JsonNode.Parse(
                    await File.ReadAllTextAsync(
                        configType.Assembly.GetFilePath($"{configName}.config.json"),
                        cancellationToken
                    )
                )
            );
            schema["properties"]!.AsObject().Add(configName, configSchema);
        }

        await File.WriteAllTextAsync(
            ConfigFullName + ".schema.json",
            schema.Root.ToJsonString(
                new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true }
            ),
            cancellationToken
        );
        var configFile = await File.ReadAllTextAsync(ConfigFullName, cancellationToken);
        if (!configFile.StartsWith("# yaml-language-server"))
        {
            var relativePath = ConfigFullName.ToRelative(Config.ConfigRoot);
            configFile = $"# yaml-language-server: $schema={relativePath}.schema.json \n" + configFile;
            await File.WriteAllTextAsync(ConfigFullName, configFile, cancellationToken);
        }

        _logger.LogInformation(GeneratorMessage.ConfigSchemaGenerated);
    }

    static async Task<List<PackageDependency>> DownloadMissingDependenciesCascade(
        string moduleFolder,
        IEnumerable<PackageDependency> dependencies,
        string framework,
        CancellationToken cancellationToken
    )
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return [];
        }

        var installedDependencies = new List<PackageDependency>();
        var newDeps = new List<PackageDependency>();
        foreach (var otherDep in dependencies)
        {
            using var packageReaderDep = await NugetUtils.DownloadPackageAsync(
                otherDep.Id,
                otherDep.VersionRange.MinVersion!.ToString(),
                cancellationToken
            );
            var file = (await packageReaderDep.GetFilesAsync(cancellationToken)).SingleOrDefault(f =>
                f.StartsWith($"lib/{framework}") && f.EndsWith(".dll") && !f.EndsWith(".resources.dll")
            );
            if (file != null)
            {
                packageReaderDep.ExtractFile(
                    file,
                    Path.Combine(moduleFolder, file.Split('/')[^1]),
                    NullLogger.Instance
                );

                installedDependencies.Add(otherDep);

                var nuspecReaderDep = await packageReaderDep.GetNuspecReaderAsync(cancellationToken);
                if (nuspecReaderDep.GetDependencyGroups().Any())
                {
                    newDeps.AddRange(
                        nuspecReaderDep
                            .GetDependencyGroups()
                            .Single(dg => dg.TargetFramework.ToString() == framework)
                            .Packages.Where(dep => !installedDependencies.Select(d => d.Id).Contains(dep.Id))
                    );
                }
            }
        }

        if (newDeps.Count > 0)
        {
            installedDependencies.AddRange(
                await DownloadMissingDependenciesCascade(moduleFolder, newDeps, framework, cancellationToken)
            );
        }

        return installedDependencies;
    }

    static string? GetFolderHash(string path)
    {
        if (!Directory.Exists(path))
        {
            return null;
        }

        return GetHash(Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories), path);
    }

    static string? GetHash(IEnumerable<string> f, string path)
    {
        var md5 = MD5.Create();
        var files = f.Order().ToList();
        foreach (var file in files)
        {
            var relativePath = Path.GetRelativePath(path, file).Replace('\\', '/');
            var pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
            md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

            var contentBytes = File.ReadAllBytes(file);
            if (files.IndexOf(file) == files.Count - 1)
            {
                md5.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
            }
            else
            {
                md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
            }
        }

        return md5.Hash != null ? BitConverter.ToString(md5.Hash).Replace("-", string.Empty).ToLower() : null;
    }

    private void AddDevCustomGenerators()
    {
        if (Environment.GetEnvironmentVariable("LOCAL_DEV") != null)
        {
            var generatorsPath = Path.GetFullPath(
                Path.Combine(
                    new FileInfo(Assembly.GetEntryAssembly()!.Location).DirectoryName!,
                    Path.Combine("..", "..", "..", "..")
                )
            );
            var modules = Directory
                .GetFileSystemEntries(generatorsPath)
                .Where(e => e.Contains("TopModel.Generator.") && !e.Contains("TopModel.Generator.Core"));
            var customGeneratorsToAdd = modules.Select(m =>
                Path.GetRelativePath(new FileInfo(ConfigFullName).DirectoryName!, m)
            );

            Config.CustomGenerators.AddRange(
                customGeneratorsToAdd.Where(cg =>
                    !Config.CustomGenerators.Select(c => c.Replace('\\', '/')).Contains(cg.Replace('\\', '/'))
                )
            );
        }
    }

    private async Task AddRemoteModule(string configKey, CancellationToken cancellationToken)
    {
        if (_generators.ToList().Exists(g => ModuleUtils.GetIGenRegInterfaceAndName(g).Name == configKey))
        {
            _resolvedConfigKeys.Add(configKey, "custom");
            return;
        }

        var fullModuleName = $"TopModel.Generator.{configKey.ToFirstUpper()}";

        if (!_topModelLock.Modules.TryGetValue(configKey, out var moduleVersion))
        {
            moduleVersion = await NugetUtils.GetLatestVersionAsync(
                fullModuleName,
                cancellationToken,
                forceCheck: true,
                VersionUtils.Prerelease
            );

            if (moduleVersion == null)
            {
                _logger.LogError(GeneratorMessage.NoGeneratorModuleFound, configKey);
                HasError = true;
                return;
            }
            _topModelLock.Modules.Add(configKey, moduleVersion);
        }
        else if (!await NugetUtils.DoesPackageExistsAsync(fullModuleName, moduleVersion.Version, cancellationToken))
        {
            _logger.LogError(GeneratorMessage.PackageNotFound, fullModuleName, moduleVersion.Version);
            HasError = true;
            return;
        }
        _deps.Add(new(configKey, moduleVersion));
    }

    private async Task AddRemoteModules(CancellationToken cancellationToken)
    {
        foreach (var configKey in Config.Generators.Keys)
        {
            await AddRemoteModule(configKey, cancellationToken);
        }
    }

    private async Task BuildCustomModulesAsync(CancellationToken cancellationToken)
    {
        foreach (var customModule in _customModules)
        {
            await customModule.BuildAsync(cancellationToken);
            if (customModule.HasError)
            {
                HasError = true;
                return;
            }
        }
    }

    private void CheckDependenciesToUpdate()
    {
        var depsToUpdate = GetDependenciesToUpdate();
        if (depsToUpdate.Any())
        {
            _logger.LogWarning(
                GeneratorMessage.GeneratorUpdatesAvailable,
                $"{Environment.NewLine}                          {string.Join($"{Environment.NewLine}                          ", depsToUpdate.Select(dep => $"- {dep.ConfigKey}: {dep.Version.Version} -> {dep.LatestVersion}"))}"
            );
            _logger.LogWarning(
                GeneratorMessage.ModgenUpdateCommand,
                depsToUpdate.Count() == 1 ? depsToUpdate.Single().ConfigKey : "all"
            );
        }
    }

    private async Task CheckMinVersionAsync(
        string moduleFolder,
        string depVersion,
        ModgenDependency dep,
        CancellationToken cancellationToken
    )
    {
        var minVersionText = await File.ReadAllTextAsync(Path.Combine(moduleFolder, "min-version"), cancellationToken);
        if (NuGetVersion.TryParse(minVersionText, out var minNuGetVersion))
        {
            if (minNuGetVersion.Major != VersionUtils.MajorVersion)
            {
                _logger.LogError(
                    GeneratorMessage.ModuleBadMajorVersion,
                    dep.ConfigKey,
                    depVersion,
                    VersionUtils.Version
                );
                HasError = true;
            }
            else if (minNuGetVersion.Minor > VersionUtils.MinorVersion)
            {
                _logger.LogError(
                    GeneratorMessage.ModuleNewerVersion,
                    dep.ConfigKey,
                    minVersionText,
                    VersionUtils.Version
                );
                HasError = true;
            }
        }
    }

    private async Task DownloadDependencies(
        ModgenDependency dep,
        string moduleFolder,
        CancellationToken cancellationToken
    )
    {
        var depVersion = dep.Version.Version;
        if (Directory.Exists(moduleFolder))
        {
            _logger.LogInformation(GeneratorMessage.ModuleCorrupted, dep.FullName);
            Directory.Delete(moduleFolder, recursive: true);
        }

        _logger.LogInformation(GeneratorMessage.ModuleInstallInProgress, dep.FullName, depVersion);

        Directory.CreateDirectory(moduleFolder);

        using var packageReader = await NugetUtils.DownloadPackageAsync(dep.FullName, depVersion, cancellationToken);
        var nuspecReader = await packageReader.GetNuspecReaderAsync(cancellationToken);

        var dependencyGroup = nuspecReader
            .GetDependencyGroups()
            .OrderByDescending(dg => dg.TargetFramework.Version.Major)
            .First(dg => dg.TargetFramework.Version.Major <= VersionUtils.DotnetMajor);

        var dependencies = dependencyGroup.Packages;
        var framework = dependencyGroup.TargetFramework.ToString();
        var coreDep = dependencies.Single(d => d.Id == "TopModel.Generator.Core");
        await File.WriteAllTextAsync(
            Path.Combine(moduleFolder, "min-version"),
            coreDep.VersionRange.MinVersion!.ToString(),
            cancellationToken
        );

        foreach (
            var file in (await packageReader.GetFilesAsync(cancellationToken)).Where(f =>
                f == $"lib/{framework}/{dep.FullName}.dll" || f.EndsWith("config.json")
            )
        )
        {
            packageReader.ExtractFile(file, Path.Combine(moduleFolder, file.Split('/')[^1]), NullLogger.Instance);
        }

        var missingDependencies = dependencies.Where(d => d.Id != "TopModel.Generator.Core").ToList();
        await DownloadMissingDependenciesCascade(moduleFolder, missingDependencies, framework, cancellationToken);

        _logger.LogInformation(GeneratorMessage.ModuleInstallCompleted, dep.FullName, depVersion);
        dep.Version.Hash = GetFolderHash(moduleFolder);
    }

    private IEnumerable<ModgenDependency> GetDependenciesToUpdate()
    {
        return _deps.Where(dep => dep.LatestVersion != null && dep.LatestVersion != dep.Version.Version);
    }

    private void HandleUpdate()
    {
        if (UpdateMode == null)
        {
            return;
        }
        var modgenRoot = Path.GetFullPath(".modgen", Config.ModelRoot);
        if (UpdateMode == "all")
        {
            _topModelLock.Modules = new Dictionary<string, TopModelLockModule>();

            if (Directory.Exists(modgenRoot))
            {
                Directory.Delete(modgenRoot, recursive: true);
            }
        }
        else if (UpdateMode != null)
        {
            _topModelLock.Modules.Remove(UpdateMode);

            foreach (
                var module in Directory
                    .GetFileSystemEntries(modgenRoot)
                    .Where(p => p.Split('/')[^1].Contains(UpdateMode))
            )
            {
                Directory.Delete(module, recursive: true);
            }
        }
    }

    private async Task InitModules(CancellationToken cancellationToken)
    {
        AnsiConsole.WriteLine();

        HandleUpdate();
        if (HasError)
        {
            return;
        }

        await BuildCustomModulesAsync(cancellationToken);
        if (HasError)
        {
            return;
        }

        LoadCustomModulesAssemblies();
        if (HasError)
        {
            return;
        }

        await AddRemoteModules(cancellationToken);
        if (HasError)
        {
            return;
        }

        if (Directory.Exists(_modgenRoot))
        {
            var usedPrefixes = _deps.Select(d => $"{d.ConfigKey}.").ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var dir in Directory.GetDirectories(_modgenRoot))
            {
                if (!usedPrefixes.Any(p => Path.GetFileName(dir).StartsWith(p)))
                {
                    Directory.Delete(dir, recursive: true);
                    _logger.LogInformation($"Supprimé: {dir.ToRelative()}");
                }
            }
        }

        await LoadModules(cancellationToken);
        if (HasError)
        {
            return;
        }

        if (SchemaMode || hasInstalled)
        {
            await WriteSchema(cancellationToken);
        }

        RemoveUnusedCustomModules();
    }

    private void LoadCustomModulesAssemblies()
    {
        foreach (var customModule in _customModules)
        {
            customModule.LoadAssemblies(ConfigFullName, _generators);
            if (HasError)
            {
                break;
            }
        }
    }

    private async Task LoadModule(ModgenDependency dep, CancellationToken cancellationToken)
    {
        var depVersion = dep.Version.Version;
        var moduleFolder = Path.Combine(_modgenRoot, $"{dep.ConfigKey}.{depVersion}");

        var depHash = GetFolderHash(moduleFolder);

        if (depHash == null || depHash != dep.Version.Hash)
        {
            await DownloadDependencies(dep, moduleFolder, cancellationToken);
            hasInstalled = true;
        }
        await CheckMinVersionAsync(moduleFolder, depVersion, dep, cancellationToken);
        if (HasError)
        {
            return;
        }

        _generators.AddRange(
            Directory
                .GetFiles(moduleFolder, "*.dll")
                .SelectMany(a =>
                    Assembly.LoadFrom(a).GetExportedTypes().Where(t => ModuleUtils.GetIGenRegInterface(t) != null)
                )
        );
        _resolvedConfigKeys.Add(dep.ConfigKey, depVersion);
        dep.LatestVersion = (
            await NugetUtils.GetLatestVersionAsync(dep.FullName, cancellationToken, prerelease: VersionUtils.Prerelease)
        )?.Version;
    }

    private async Task PrepareConfiguration()
    {
        foreach (var generator in _generators)
        {
            var (configType, configName) = ModuleUtils.GetIGenRegInterfaceAndName(generator);

            if (Config.Generators.TryGetValue(configName, out var genConfigMaps))
            {
                for (var j = 0; j < genConfigMaps.Count(); j++)
                {
                    var genConfigMap = genConfigMaps.ElementAt(j);
                    var number = j + 1;

                    try
                    {
                        var genConfig = (GeneratorConfigBase)
                            FileChecker.GetGenConfig(configName, configType, genConfigMap);
                        genConfig.InitVariables(Config.App, number, _logger);

                        genConfig.ExcludedTags = ExcludedTags.ToList();

                        genConfig.TranslateReferences ??= Config.I18n.TranslateReferences;
                        genConfig.TranslateProperties ??= Config.I18n.TranslateProperties;

                        ModelUtils.TrimSlashes(genConfig, c => c.OutputDirectory);
                        ModelUtils.CombinePath(Config.ConfigRoot, genConfig, c => c.OutputDirectory);

                        genConfig.Name ??= $"{configName}@{number}";
                        try
                        {
                            Config.Configs.Add(genConfig.Name, genConfig);
                        }
                        catch (ArgumentException)
                        {
                            _logger.LogError(GeneratorMessage.ConfigNameAlreadyInUse, genConfig.Name);
                            HasError = true;
                            return;
                        }

                        foreach (var referencedTag in genConfig.ReferencedTags)
                        {
                            if (Config.Configs.TryGetValue(referencedTag.Value, out var referencedConfig))
                            {
                                genConfig.ReferencedTagConfigs.Add(referencedTag.Key, referencedConfig);
                            }
                            else
                            {
                                _logger.LogWarning(
                                    GeneratorMessage.ReferencedConfigNotFound,
                                    referencedTag.Value,
                                    referencedTag.Key,
                                    genConfig.Name
                                );
                            }
                        }

                        if (genConfig.ReferencedTagConfigs.Any())
                        {
                            genConfig = (GeneratorConfigBase)
                                proxyGenerator.CreateClassProxyWithTarget(genConfig.GetType(), genConfig, interceptor);
                        }

                        var instance = Activator.CreateInstance(generator);
                        instance!.GetType().GetMethod("Register")!.Invoke(instance, [_services, genConfig, number]);
                    }
                    catch (ModelException me)
                    {
                        HasError = true;
                        AnsiConsole.MarkupLine($"[red]{me.Message.EscapeMarkup()}[/]");
                        AnsiConsole.WriteLine();
                        return;
                    }
                }
            }
        }
    }

    private void RemoveUnusedCustomModules()
    {
        var unused = _topModelLock.Custom.Keys.Except(Config.CustomGenerators).ToList();
        if (unused.Count > 0)
        {
            foreach (var key in unused)
            {
                _topModelLock.Custom.Remove(key);
                var hashFile = CustomModule.GetHashFilePath(_modgenRoot, key);
                if (File.Exists(hashFile))
                {
                    File.Delete(hashFile);
                }
            }

            _topModelLock.Write();
        }
    }

    private async Task RunGeneration(CancellationToken cancellationToken)
    {
        var provider = _services.BuildServiceProvider();
        _providers.Add(provider);

        var modelStore = provider.GetRequiredService<ModelStore>();

        modelStore.DisableLockfile = ExcludedTags.Any();

        modelStore.OnResolve += he =>
        {
            HasError = he;
        };

        await modelStore.LoadFromConfig(WatchMode, _topModelLock, StoreConfig, cancellationToken);
    }
}
