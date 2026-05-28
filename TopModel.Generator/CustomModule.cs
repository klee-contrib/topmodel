using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using NuGet.Common;
using NuGet.ProjectModel;
using TopModel.Utils;
using TopModel.Utils.Cli;

namespace TopModel.Generator;

public class CustomModule(
    string customGenerator,
    string fullName,
    string modgenRoot,
    Microsoft.Extensions.Logging.ILogger logger,
    TopModelLock topModelLock,
    bool noBuild = false
)
{
    public bool HasError { get; private set; } = false;

    public static string GetHashFilePath(string modgenRoot, string customGenerator)
    {
        return Path.Combine(modgenRoot, customGenerator.Replace('/', '-').Replace('\\', '-'));
    }

    public async Task BuildAsync(CancellationToken cancellationToken)
    {
        if (noBuild)
        {
            return;
        }

        Directory.CreateDirectory(modgenRoot);

        var customDir = Path.GetFullPath(
            Path.Combine(new FileInfo(Path.GetFullPath(fullName)).DirectoryName!, customGenerator)
        );

        var customHash =
            ModuleUtils.GetHash(
                Directory
                    .EnumerateFiles(
                        Path.GetFullPath(customGenerator, new FileInfo(fullName).DirectoryName!),
                        "*.cs",
                        SearchOption.AllDirectories
                    )
                    .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")),
                customDir
            ) ?? string.Empty;

        var customHashLocalFile = GetHashFilePath(modgenRoot, customGenerator);
        var customHashLocal = File.Exists(customHashLocalFile)
            ? await File.ReadAllTextAsync(customHashLocalFile, cancellationToken)
            : string.Empty;

        if (
            !topModelLock.Custom.TryGetValue(customGenerator, out var customLockHash)
            || customHash != customLockHash
            || customHash != customHashLocal
        )
        {
            var expectedDll = $"{customGenerator.Replace('\\', '/').Split('/')[^1].ToLower()}.dll";
            if (!AssembliesUtils.LoadedAssemblies.Contains(expectedDll, StringComparer.CurrentCultureIgnoreCase))
            {
                await BuildCSharpProject(customDir, cancellationToken);
                if (HasError)
                {
                    return;
                }

                logger.LogInformation(GeneratorMessage.BuildCompleted, customGenerator);
            }

            topModelLock.Custom ??= new Dictionary<string, string>();
            topModelLock.Custom[customGenerator] = customHash;
            await File.WriteAllTextAsync(customHashLocalFile, topModelLock.Custom[customGenerator], cancellationToken);
        }
    }

    public void Check(TopModelLock topModelLock, Microsoft.Extensions.Logging.ILogger logger)
    {
        var projDir = Path.GetFullPath(customGenerator, new FileInfo(fullName).DirectoryName!);
        if (!Directory.EnumerateFiles(projDir, "*.csproj").Any())
        {
            logger.LogError(GeneratorMessage.NoCsprojFound, customGenerator);
            HasError = true;
            return;
        }

        var assetsPath = Path.Combine(projDir, "obj", "project.assets.json");
        if (!File.Exists(assetsPath))
        {
            logger.LogError(GeneratorMessage.GeneratorModuleNotBuilt, customGenerator);
            HasError = true;
            return;
        }

        var lockFile = LockFileUtilities.GetLockFile(assetsPath, NullLogger.Instance);

        foreach (
            var dep in lockFile
                .Targets.FirstOrDefault(dg => dg.TargetFramework.Version.Major <= VersionUtils.DotnetMajor)
                ?.Libraries.Where(n => n.Name?.StartsWith("TopModel.Generator") ?? false)
                ?? []
        )
        {
            if (dep.Name == "TopModel.Generator.Core")
            {
                if (dep.Version?.Major != VersionUtils.MajorVersion)
                {
                    logger.LogError(
                        GeneratorMessage.GeneratorModuleBadMajorVersion,
                        customGenerator,
                        dep.Version?.ToString() ?? string.Empty,
                        VersionUtils.Version
                    );
                    HasError = true;
                    return;
                }
                else if (dep.Version?.Minor > VersionUtils.MinorVersion)
                {
                    logger.LogError(
                        GeneratorMessage.GeneratorModuleNewerVersion,
                        customGenerator,
                        dep.Version?.ToString() ?? string.Empty,
                        VersionUtils.Version
                    );
                    HasError = true;
                    return;
                }
            }
            else
            {
                var configKey = dep.Name?.Split('.')[^1].ToLower() ?? string.Empty;
                if (!topModelLock.Modules.TryGetValue(configKey, out var ev))
                {
                    topModelLock.Modules.Add(configKey, new() { Version = dep.Version?.ToString() ?? string.Empty });
                }
                else if (ev.Version != dep.Version?.ToString())
                {
                    logger.LogError(
                        GeneratorMessage.CustomModuleWrongLockfileVersion,
                        customGenerator,
                        configKey,
                        dep.Version?.ToString() ?? string.Empty,
                        ev
                    );
                    HasError = true;
                    return;
                }
            }
        }
    }

    public void LoadAssemblies(string fullName, IList<Type> generators)
    {
        Check(topModelLock, logger);
        if (HasError)
        {
            return;
        }

        var files = new DirectoryInfo(
            Path.Combine(Path.GetFullPath(customGenerator, new FileInfo(fullName).DirectoryName!), "bin")
        ).GetFiles($"*.dll", SearchOption.AllDirectories);
        var assemblies = AssembliesUtils.LoadAssemblies(files);
        if (HasError)
        {
            return;
        }

        var assemblyModule = assemblies.Where(a =>
            a.ManifestModule.Name.Equals(
                $"{customGenerator
                .Replace('\\', '/')
                .Split('/')[^1].ToLower()}.dll",
                StringComparison.CurrentCultureIgnoreCase
            )
        );
        var moduleExportedTypes = assemblyModule.SelectMany(a => a.GetExportedTypes());
        var generatorTypes = moduleExportedTypes.Where(t => ModuleUtils.GetIGenRegInterface(t) != null);
        generators.AddRange(generatorTypes);
        return;
    }

    private async Task BuildCSharpProject(string customDir, CancellationToken cancellationToken)
    {
        logger.LogInformation(GeneratorMessage.BuildInProgress, customGenerator);
        var build = Process.Start(
            new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "dotnet",
                Arguments = "build -v q",
                WorkingDirectory = customDir,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8,
            }
        );
        string stdout;
        try
        {
            await using var registration = cancellationToken.Register(() => build?.Kill());
            var stdoutTask = build!.StandardOutput.ReadToEndAsync(cancellationToken);
            await build!.WaitForExitAsync(cancellationToken);
            stdout = await stdoutTask;
        }
        catch (OperationCanceledException)
        {
            HasError = true;
            return;
        }

        if (build.ExitCode != 0)
        {
            logger.LogError(GeneratorMessage.BuildError, customGenerator);
            var output = stdout.Trim();
            if (!string.IsNullOrEmpty(output))
            {
                logger.LogError(output);
            }
            HasError = true;
            return;
        }
    }
}
