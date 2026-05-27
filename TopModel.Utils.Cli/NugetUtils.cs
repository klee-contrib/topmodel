using System.Text.Json;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using TopModel.Utils;

namespace TopModel.Utils.Cli;

public static class NugetUtils
{
    private static readonly string CacheFile = Path.Combine(
        NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp),
        "topmodel-cache.json"
    );
    private static readonly SourceCacheContext NugetCache = new();
    private static readonly Dictionary<string, ModuleLatestVersion> Versions = [];

    private static bool _cantCheckVersion;
    private static FindPackageByIdResource? _nugetResource;

    static NugetUtils()
    {
        if (File.Exists(CacheFile))
        {
            Versions = JsonSerializer.Deserialize<Dictionary<string, ModuleLatestVersion>>(
                File.ReadAllText(CacheFile)
            )!;
        }
    }

    public static async Task ClearAsync(CancellationToken Ct)
    {
        Versions.Clear();
        await WriteAsync(Ct);
    }

    public static async Task<bool> DoesPackageExistsAsync(string id, string version, CancellationToken Ct)
    {
        var nugetResource = await GetNugetResourceAsync(Ct);
        return await nugetResource.DoesPackageExistAsync(
            id,
            new NuGetVersion(version),
            NugetCache,
            NullLogger.Instance,
            Ct
        );
    }

    public static async Task<PackageArchiveReader> DownloadPackageAsync(string id, string version, CancellationToken Ct)
    {
        var nugetResource = await GetNugetResourceAsync(Ct);
        var packageStream = new MemoryStream();
        await nugetResource.CopyNupkgToStreamAsync(
            id,
            new NuGetVersion(version),
            packageStream,
            NugetCache,
            NullLogger.Instance,
            Ct
        );
        return new PackageArchiveReader(packageStream);
    }

    public static async Task<TopModelLockModule?> GetLatestVersionAsync(
        string id,
        CancellationToken Ct,
        bool forceCheck = false,
        bool prerelease = false
    )
    {
        if (Versions.TryGetValue(prerelease ? $"{id}-prerelease" : id, out var cachedVersion))
        {
            if (cachedVersion.CheckDate.AddHours(6) < DateTime.UtcNow)
            {
                Versions.Remove(prerelease ? $"{id}-prerelease" : id);
            }
            else
            {
                if (cachedVersion.Version == null)
                {
                    _cantCheckVersion = true;
                }
                else
                {
                    return new TopModelLockModule { Version = cachedVersion.Version };
                }
            }
        }

        if (_cantCheckVersion && !forceCheck)
        {
            return null;
        }

        try
        {
            var nugetResource = await GetNugetResourceAsync(Ct);
            IEnumerable<NuGetVersion> moduleVersions;
            try
            {
                moduleVersions = await nugetResource.GetAllVersionsAsync(id, NugetCache, NullLogger.Instance, Ct);
            }
            catch (InvalidPackageIdException)
            {
                return null;
            }

            if (!moduleVersions.Any())
            {
                return null;
            }

            var version = new TopModelLockModule
            {
                Version = moduleVersions.Last(m => prerelease || !m.IsPrerelease).ToFullString(),
            };

            Versions[prerelease ? $"{id}-prerelease" : id] = new(version.Version, DateTime.UtcNow);
            await WriteAsync(Ct);
            return version;
        }
        catch (FatalProtocolException)
        {
            // Si on a pas internet par exemple.
            _cantCheckVersion = true;
            Versions[prerelease ? $"{id}-prerelease" : id] = new(Version: null, DateTime.UtcNow);
            await WriteAsync(Ct);
            return null;
        }
    }

    private static async Task<FindPackageByIdResource> GetNugetResourceAsync(CancellationToken Ct)
    {
        if (_nugetResource == null)
        {
            var nugetRepository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            _nugetResource = await nugetRepository.GetResourceAsync<FindPackageByIdResource>(Ct);
        }

        return _nugetResource;
    }

    private static async Task WriteAsync(CancellationToken Ct)
    {
        await File.WriteAllTextAsync(CacheFile, JsonSerializer.Serialize(Versions), Ct);
    }
}
