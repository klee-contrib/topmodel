using System.Net;
using TopModel.Core.FileModel;

namespace TopModel.UI.Graphing
{
    public static class GraphingUtil
    {
        public static string GetURL(this ModelFile? file)
        {
            return file == null
                ? string.Empty
                : $"{file.Descriptor.Module}/{file.Descriptor.Kind}/{WebUtility.UrlEncode(file.Descriptor.File)}";
        }

        public static string ForTooltip(this string source)
        {
            return source.Replace("\"", "\\\"") ?? string.Empty;
        }
    }
}