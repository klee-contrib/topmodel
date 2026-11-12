using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;

namespace TopModel.LanguageServer.Handlers;

/// <summary>
/// Met à jour les "uses" des fichiers de modèle lorsque des fichiers ou dossiers sont renommés/déplacés.
/// </summary>
public class WillRenameFilesHandler(LSWorkerStore workerStore, ILanguageServerFacade facade)
    : WillRenameFilesHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<WorkspaceEdit?> Handle(
        WillRenameFilesParams request,
        CancellationToken cancellationToken
    )
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var edits = facade.ToWorkspaceEdit(
            from rename in request.Files
            let oldPath = DocumentUri.From(rename.OldUri).GetFileSystemPath()
            let newPath = DocumentUri.From(rename.NewUri).GetFileSystemPath()
            from store in workerStore.ModelStores
            let oldName = store.ModelFileLoadConfig.GetFileName(oldPath)
            let newName = store.ModelFileLoadConfig.GetFileName(newPath)
            from file in store.Files
            from use in file.Uses
            where use.ReferenceName == oldName || use.ReferenceName.StartsWith(oldName + "/")
            select (file, use, newName + use.ReferenceName[oldName.Length..])
        );

        return edits.Changes!.Count > 0 ? edits : null;
    }

    protected override WillRenameFilesRegistrationOptions CreateRegistrationOptions(
        FileOperationsWorkspaceClientCapabilities capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new()
        {
            Filters = new(
                new FileOperationFilter
                {
                    Scheme = "file",
                    Pattern = new() { Glob = "**/*.tmd", Matches = FileOperationPatternKind.File },
                },
                new FileOperationFilter
                {
                    Scheme = "file",
                    Pattern = new() { Glob = "**/*", Matches = FileOperationPatternKind.Folder },
                }
            ),
        };
    }
}
