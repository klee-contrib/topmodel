using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core.FileModel;

public static class OmnisharpExtensions
{
    public static OmniSharp.Extensions.LanguageServer.Protocol.Models.Range? ToRange(this Reference? loc)
    {
        return loc == null
            ? null
            : new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                loc.Start.Line - 1,
                loc.Start.Column - 1,
                loc.End.Line - 1,
                loc.End.Column - 1);
    }

    public static string GetFilePath(this ILanguageServerFacade facade, ModelFile file)
    {
        return facade.Workspace.ClientSettings.RootPath + file.Path[1..];
    }
}
