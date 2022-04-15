using Newtonsoft.Json.Linq;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;

class CodeLensHandler : CodeLensHandlerBase
{
    private readonly ModelStore _modelStore;
    private readonly ILanguageServerFacade _facade;

    public CodeLensHandler(ModelStore modelStore, ILanguageServerFacade facade)
    {
        _modelStore = modelStore;
        _facade = facade;
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
            return Task.FromResult(new CodeLensContainer(file.Classes.Where(c => !file.ResolvedAliases.Contains(c)).Select(clazz =>
                new CodeLens
                {
                    Range = clazz.GetLocation().ToRange()!,
                    Command = new Command()
                    {
                        Title = $"{_modelStore.GetClassReferences(clazz).Count()} references",
                        Name = "topmodel.findRef",
                        Arguments = new JArray
                        {
                            clazz.GetLocation()!.Start.Line - 1
                        }

                    }
                })
                .Concat(file.Domains.Select(domain => new CodeLens
                {
                    Range = domain.GetLocation().ToRange()!,
                    Command = new Command()
                    {
                        Title = $"{_modelStore.GetDomainReferences(domain).Count()} references",
                        Name = "topmodel.findRef",
                        Arguments = new JArray
                        {
                            domain.GetLocation()!.Start.Line - 1
                        }

                    }
                }))
                .Concat(file.Decorators.Select(decorator => new CodeLens
                {
                    Range = decorator.GetLocation().ToRange()!,
                    Command = new Command()
                    {
                        Title = $"{_modelStore.GetDecoratorReferences(decorator).Count()} references",
                        Name = "topmodel.findRef",
                        Arguments = new JArray
                        {
                            decorator.GetLocation()!.Start.Line - 1
                        }
                    }
                }))));
        }
        return Task.FromResult<CodeLensContainer>(new());
    }

    protected override CodeLensRegistrationOptions CreateRegistrationOptions(CodeLensCapability capability, ClientCapabilities clientCapabilities)
    {
        return new CodeLensRegistrationOptions
        {
            DocumentSelector = DocumentSelector.ForLanguage("yaml")
        };
    }
}