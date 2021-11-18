using TopModel.Core.FileModel;

namespace TopModel.UI.Graphing;

public static class GraphingUtil
{
    public static string GetURL(this ModelFile? file)
    {
        return file == null
            ? string.Empty
            : file.Name.Replace("/", "+");
    }

    public static string ForTooltip(this string source)
    {
        return source?.Replace("\"", "\\\"").Replace(">", "&gt;").Replace("<", "&lt;") ?? string.Empty;
    }
}