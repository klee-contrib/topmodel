using System.Resources;

namespace TopModel.Utils.Cli;

public static class LocalizeUtils
{
    public static string Localize<T>(T messageType, params object[] args)
        where T : struct, Enum
    {
        var format =
            ResourceManagerCache<T>.Instance.GetString(Enum.GetName(messageType)!) ?? Enum.GetName(messageType)!;
        return args.Length > 0 ? string.Format(format, args) : format;
    }

    private static class ResourceManagerCache<T>
        where T : struct, Enum
    {
        public static readonly ResourceManager Instance = new(typeof(T));
    }
}
