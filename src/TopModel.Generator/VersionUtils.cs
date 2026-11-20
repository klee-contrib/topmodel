using System.Reflection;

namespace TopModel.Generator;

public static class VersionUtils
{
    public static readonly int DotnetMajor = Environment.Version.Major;
    public static readonly int MajorVersion = Assembly.GetEntryAssembly()!.GetName().Version!.Major;
    public static readonly int MinorVersion = Assembly.GetEntryAssembly()!.GetName().Version!.Minor;

    public static readonly string Version = Assembly
        .GetEntryAssembly()!
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
        .InformationalVersion;
    public static bool Prerelease => Version.Contains('-');
}
