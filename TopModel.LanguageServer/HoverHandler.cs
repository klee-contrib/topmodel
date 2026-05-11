using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core.Model;

namespace TopModel.LanguageServer;

public class HoverHandler(ModelStoreRegistry registry, ILanguageServerFacade facade) : HoverHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<Hover?> Handle(HoverParams request, CancellationToken cancellationToken)
    {
        var filePath = request.TextDocument.Uri.GetFileSystemPath();
        var entry = registry.GetPrimaryForFile(filePath);
        if (entry == null)
        {
            return null;
        }

        var modelStore = entry.Store;
        await modelStore.WaitForUpdates(cancellationToken);

        var file = modelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == filePath);
        if (file != null)
        {
            var (reference, objet) = file.GetObjetAtPosition(request.Position);

            if (reference != null && objet != null)
            {
                return new Hover
                {
                    Range = reference.ToRange(),
                    Contents = new(
                        new MarkedString(
                            objet switch
                            {
                                Class c => c.Comment,
                                Endpoint e => e.Description,
                                IProperty p => p.Comment,
                                Domain d => d.Label,
                                Decorator d => d.Description,
                                DecoratorInstance { Decorator: Decorator d } => d.Description,
                                Annotation a => a.Description,
                                AnnotationInstance { Annotation: Annotation a } => a.Description,
                                DataFlow d => $"Flux de données '{d.Name}'",
                                TemplateParameter tp => tp.Description,
                                Variable v => v.Description,
                                _ => string.Empty,
                            }
                        )
                    ),
                };
            }
        }

        return null;
    }

    protected override HoverRegistrationOptions CreateRegistrationOptions(
        HoverCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new HoverRegistrationOptions { DocumentSelector = registry.GetCombinedDocumentSelector() };
    }
}
