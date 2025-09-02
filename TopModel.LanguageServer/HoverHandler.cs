using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.Model;

namespace TopModel.LanguageServer;

public class HoverHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelConfig config) : HoverHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<Hover?> Handle(HoverParams request, CancellationToken cancellationToken)
    {
        await modelStore.WaitForUpdates(cancellationToken);

        var file = modelStore.Files.SingleOrDefault(f =>
            facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath()
        );
        if (file != null)
        {
            var matchedReference = file.References.Keys.SingleOrDefault(reference =>
                reference.Start.Line - 1 <= request.Position.Line
                && request.Position.Line <= reference.End.Line - 1
                && reference.Start.Column - 1 <= request.Position.Character
                && request.Position.Character <= reference.End.Column - 1
            );

            if (matchedReference != null)
            {
                var objet = file.References[matchedReference];
                return new Hover
                {
                    Range = matchedReference.ToRange(),
                    Contents = new(
                        new MarkedString(
                            objet switch
                            {
                                Class c => c.Comment,
                                Endpoint e => e.Description,
                                RegularProperty p => p.Comment,
                                AssociationProperty p => p.Comment,
                                CompositionProperty p => p.Comment,
                                AliasProperty p => p.Comment,
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
        return new HoverRegistrationOptions { DocumentSelector = config.GetDocumentSelector() };
    }
}
