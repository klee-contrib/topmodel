using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core.Model;

namespace TopModel.LanguageServer.Handlers;

public class HoverHandler(LSWorkerStore workerStore) : HoverHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<Hover?> Handle(HoverParams request, CancellationToken cancellationToken)
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var files = workerStore.GetFiles(request.TextDocument);

        var references = files
            .Select(f => f.File.GetObjetAtPosition(request.Position))
            .Where(r => r.Reference != null && r.Objet != null)
            .Select(r => new
            {
                r.Reference,
                Contents = r.Objet switch
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
                },
            })
            .ToList();

        var reference = references.GroupBy(r => r.Reference).FirstOrDefault();

        if (reference != null)
        {
            return new Hover
            {
                Range = reference.Key.ToRange(),
                Contents = new(
                    new MarkedString(
                        string.Join(
                            $"{Environment.NewLine}{Environment.NewLine}",
                            reference.Distinct().Select(r => r.Contents)
                        )
                    )
                ),
            };
        }

        return null;
    }

    protected override HoverRegistrationOptions CreateRegistrationOptions(
        HoverCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new HoverRegistrationOptions { DocumentSelector = TextDocumentSelector.TmdFiles };
    }
}
