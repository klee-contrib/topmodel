using System.Reflection;
using NuGet.Frameworks;

namespace TopModel.Generator;

public static class VersionUtils
{
    public static readonly int MajorVersion = Assembly.GetEntryAssembly()!.GetName().Version!.Major;
    public static readonly int MinorVersion = Assembly.GetEntryAssembly()!.GetName().Version!.Minor;

    public static readonly string Version = Assembly
        .GetEntryAssembly()!
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
        .InformationalVersion;

    private static readonly NuGetFramework DotnetVersion = NuGetFramework.Parse(
        $"net{Environment.Version.Major}.{Environment.Version.Minor}"
    );

    public static bool Prerelease => Version.Contains('-');

    public static T? GetNearestFramework<T>(IEnumerable<T> items, Func<T, NuGetFramework> frameworkSelector)
        where T : class
    {
        var itemList = items.ToList();
        var nearestFramework = new FrameworkReducer().GetNearest(DotnetVersion, itemList.Select(frameworkSelector));

        return nearestFramework == null
            ? default
            : itemList.FirstOrDefault(item => frameworkSelector(item).Equals(nearestFramework));
    }
}
