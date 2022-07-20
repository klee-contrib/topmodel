using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;

class ReferencesHandler : ReferencesHandlerBase
{
    private readonly ModelStore _modelStore;
    private readonly ILanguageServerFacade _facade;

    public ReferencesHandler(ModelStore modelStore, ILanguageServerFacade facade)
    {
        _modelStore = modelStore;
        _facade = facade;
    }

    public override Task<LocationContainer> Handle(ReferenceParams request, CancellationToken cancellationToken)
    {
        var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
        if (file != null)
        {
            object? objet = null;

            var matchedReference = file.References.Keys.SingleOrDefault(reference =>
                reference.Start.Line - 1 <= request.Position.Line && request.Position.Line <= reference.End.Line - 1
                && reference.Start.Column - 1 <= request.Position.Character && request.Position.Character <= reference.End.Column - 1);

            if (matchedReference != null)
            {
                objet = file.References[matchedReference];
            }
            else
            {
                objet =
                    (object?)file.Classes.SingleOrDefault(c => c.Name.GetLocation()!.Start.Line - 1 == request.Position.Line || c.GetLocation()!.Start.Line - 1 == request.Position.Line)
                    ?? (object?)file.Domains.SingleOrDefault(d => d.Name.GetLocation()!.Start.Line - 1 == request.Position.Line || d.GetLocation()!.Start.Line - 1 == request.Position.Line)
                    ?? file.Decorators.SingleOrDefault(d => d.Name.GetLocation()!.Start.Line - 1 == request.Position.Line || d.GetLocation()!.Start.Line - 1 == request.Position.Line);
            }

            var references = objet switch
            {
                Class classe => new[] { (Reference: classe.Name.GetLocation()!, File: classe.GetFile()!) }
                    .Concat(_modelStore.GetClassReferences(classe).Select(c => (Reference: (Reference)c.Reference, c.File))),
                Domain domain => new[] { (Reference: domain.Name.GetLocation()!, File: domain.GetFile()!) }
                    .Concat(_modelStore.GetDomainReferences(domain).Select(d => (Reference: (Reference)d.Reference, d.File))),
                Decorator decorator => new[] { (Reference: decorator.Name.GetLocation()!, File: decorator.GetFile()!) }
                    .Concat(_modelStore.GetDecoratorReferences(decorator).Select(d => (Reference: (Reference)d.Reference, d.File))),
                _ => null
            };


            if (references != null)
            {
                return Task.FromResult(new LocationContainer(
                    references.Select(r => new Location
                    {
                        Uri = new Uri(_facade.GetFilePath(r.File)),
                        Range = r.Reference.ToRange()!
                    })));
            }
        }

        return Task.FromResult<LocationContainer>(new());
    }

    protected override ReferenceRegistrationOptions CreateRegistrationOptions(ReferenceCapability capability, ClientCapabilities clientCapabilities)
    {
        return new ReferenceRegistrationOptions()
        {
            DocumentSelector = DocumentSelector.ForLanguage("yaml")
        };
    }
}