using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using TopModel.Core;

class CodeLensHandler : CodeLensHandlerBase
{
    private readonly ModelStore _modelStore;
    private readonly ILanguageServerFacade _facade;

    private readonly ReferencesHandler _referencesHandler;

    public CodeLensHandler(ModelStore modelStore, ILanguageServerFacade facade, ReferencesHandler referencesHandler)
    {
        _modelStore = modelStore;
        _facade = facade;
        _referencesHandler = referencesHandler;
    }

    public override Task<CodeLens> Handle(CodeLens request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    public override Task<CodeLensContainer> Handle(CodeLensParams request, CancellationToken cancellationToken)
    {
        var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
        if (file != null)
        {
            return Task.FromResult<CodeLensContainer>(new CodeLensContainer(file.Classes.Select(clazz =>
            {
                return new CodeLens()
                {
                    Range = clazz.GetLocation().ToRange()!,
                    Command = new Command()
                    {
                        Title = $"{_referencesHandler.findClassReferences(clazz).Count()} references",
                        Name = "topmodel.findRef",
                        Arguments = new Newtonsoft.Json.Linq.JArray(){
                            clazz.GetLocation()!.Start.Line
                        }

                    }
                };
            })));
        }
        return Task.FromResult<CodeLensContainer>(new());
    }

    protected override CodeLensRegistrationOptions CreateRegistrationOptions(CodeLensCapability capability, ClientCapabilities clientCapabilities)
    {
        return new CodeLensRegistrationOptions()
        {
            DocumentSelector = DocumentSelector.ForLanguage("yaml")
        };
    }
}