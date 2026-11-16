using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;

namespace TopModel.LanguageServer.Handlers;

/// <summary>
/// Diagnostics "pull" d'un document (<c>textDocument/diagnostic</c>), à partir de l'état courant du <see cref="LSReporter" />.
/// </summary>
public class DocumentDiagnosticHandler(LSWorkerStore workerStore, LSReporter reporter, ILanguageServerFacade facade)
    : DocumentDiagnosticHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<RelatedDocumentDiagnosticReport> Handle(
        DocumentDiagnosticParams request,
        CancellationToken cancellationToken
    )
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var diagnostics = reporter.GetDiagnostics(facade.GetFilePath(request.TextDocument.Uri));

        // Fichier inconnu : aucun diagnostic.
        if (diagnostics is null)
        {
            return new RelatedFullDocumentDiagnosticReport { Items = new Container<Diagnostic>() };
        }

        if (request.PreviousResultId == diagnostics.ResultId)
        {
            return new RelatedUnchangedDocumentDiagnosticReport { ResultId = diagnostics.ResultId };
        }

        return new RelatedFullDocumentDiagnosticReport
        {
            Items = new Container<Diagnostic>(diagnostics.Diagnostics),
            ResultId = diagnostics.ResultId,
        };
    }

    protected override DiagnosticsRegistrationOptions CreateRegistrationOptions(
        DiagnosticClientCapabilities capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new DiagnosticsRegistrationOptions
        {
            DocumentSelector = workerStore.TmdFiles,
            Identifier = "topmodel",
            InterFileDependencies = true,
            WorkspaceDiagnostics = false,
        };
    }
}
