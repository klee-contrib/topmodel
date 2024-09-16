using System.Text.Json;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace TopModel.Utils;

public static class NugetUtils
{
    private static readonly string CacheFile = Path.Combine(NuGetEnvironment.GetFolderPath(NuGetFolderPath.Temp), "topmodel-cache.json");
    private static readonly CancellationToken Ct = CancellationToken.None;
    private static readonly SourceCacheContext NugetCache = new();
    private static readonly Dictionary<string, ModuleLatestVersion> Versions = [];

    private static bool cantCheckVersion;
    private static FindPackageByIdResource? nugetResource;

    static NugetUtils()
    {
        if (File.Exists(CacheFile))
        {
            Versions = JsonSerializer.Deserialize<Dictionary<string, ModuleLatestVersion>>(File.ReadAllText(CacheFile))!;
        }
    }

    public static async Task ClearAsync()
    {
        Versions.Clear();
        await WriteAsync();
    }

    public static async Task<bool> DoesPackageExistsAsync(string id, string version)
    {
        var nugetResource = await GetNugetResourceAsync();
        return await nugetResource.DoesPackageExistAsync(id, new NuGetVersion(version), NugetCache, NullLogger.Instance, Ct);
    }

    public static async Task<PackageArchiveReader> DownloadPackageAsync(string id, string version)
    {
        var nugetResource = await GetNugetResourceAsync();
        var packageStream = new MemoryStream();
        await nugetResource.CopyNupkgToStreamAsync(id, new NuGetVersion(version), packageStream, NugetCache, NullLogger.Instance, Ct);
        return new PackageArchiveReader(packageStream);
    }

    public static async Task<TopModelLockModule?> GetLatestVersionAsync(string id)
    {
        if (cantCheckVersion)
        {
            return null;
        }

        if (Versions.TryGetValue(id, out var cachedVersion))
        {
            if (cachedVersion.CheckDate.AddHours(6) < DateTime.UtcNow)
            {
                Versions.Remove(id);
            }
            else
            {
                return new TopModelLockModule { Version = cachedVersion.Version };
            }
        }

        try
        {
            var nugetResource = await GetNugetResourceAsync();
            var moduleVersions = await nugetResource.GetAllVersionsAsync(id, NugetCache, NullLogger.Instance, Ct);

            if (!moduleVersions.Any())
            {
                return null;
            }

            var nugetVersion = moduleVersions.Last().Version;
            var version = new TopModelLockModule { Version = $"{nugetVersion.Major}.{nugetVersion.Minor}.{nugetVersion.Build}" };

            Versions[id] = new(version.Version, DateTime.UtcNow);
            await WriteAsync();
            return version;
        }
        catch (FatalProtocolException)
        {
            // Si on a pas internet par exemple.
            cantCheckVersion = true;
            return null;
        }
    }

    private static async Task<FindPackageByIdResource> GetNugetResourceAsync()
    {
        if (nugetResource == null)
        {
            var nugetRepository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            nugetResource = await nugetRepository.GetResourceAsync<FindPackageByIdResource>();
        }

        return nugetResource;
    }

    private static async Task WriteAsync()
    {
        await File.WriteAllTextAsync(CacheFile, JsonSerializer.Serialize(Versions));
    }
}
