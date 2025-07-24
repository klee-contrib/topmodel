using MediatR;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.Model;

namespace TopModel.LanguageServer;

public class MermaidHandler(ModelStore modelStore, ILanguageServerFacade facade) : IRequestHandler<MermaidRequest, Mermaid>, IJsonRpcHandler
{
    public static string GenerateDiagramClasses(IEnumerable<Class> classes)
    {
        string diagram = string.Empty;
        var externalClasses = new List<Class>();
        foreach (var classe in classes)
        {
            if (classe.Properties.OfType<RegularProperty>().Any())
            {
                diagram += @$"%% {classe.Comment.Replace("\n", "\n%% ")}" + '\n';

                diagram += @$"class {classe.Name}{{" + '\n';
                if (classe.EnumKey != null)
                {
                    diagram += "&lt;&lt;Enum&gt;&gt;\n";
                    foreach (var refValue in classe.Values.OrderBy(x => x.Name, StringComparer.Ordinal))
                    {
                        diagram += refValue.Value[classe.EnumKey] + '\n';
                    }

                    diagram += "}\n";
                    continue;
                }

                foreach (var property in classe.Properties.OfType<RegularProperty>())
                {
                    diagram += $" {property.Domain.Name} {property.Name}\n";
                }

                diagram += "}\n";
            }

            foreach (var property in classe.Properties.OfType<AssociationProperty>())
            {
                if (!classes.Contains(property.Association))
                {
                    externalClasses.Add(property.Association);
                }

                string cardLeft;
                string cardRight;
                switch (property.Type)
                {
                    case AssociationType.OneToOne:
                        cardLeft = property.Required ? "1" : "0..1";
                        cardRight = "1";
                        break;
                    case AssociationType.OneToMany:
                        cardLeft = property.Required ? "1..*" : "0..*";
                        cardRight = property.Required ? "1" : "0..1";
                        break;
                    case AssociationType.ManyToOne:
                        cardLeft = property.Required ? "1" : "0..1";
                        cardRight = "0..*";
                        break;
                    case AssociationType.ManyToMany:
                    default:
                        cardLeft = property.Required ? "1..*" : "1..*";
                        cardRight = "0..*";
                        break;
                }

                diagram += @$"{property.Class.Name} ""{cardLeft}"" --> ""{cardRight}"" {property.Association.Name}{(property.Role != null ? " : " + property.Role : string.Empty)}" + '\n';
            }

            foreach (var property in classe.Properties.OfType<CompositionProperty>())
            {
                diagram += $"{property.Class.Name} --* {property.Composition.Name}\n";
            }
        }

        foreach (var classe in externalClasses)
        {
            diagram += @$"%% {classe.Comment.Replace("\n", "\n%% ")}" + '\n';
            diagram += @$"class {classe.Name}:::fileReference" + '\n';
        }

        foreach (var classe in classes.Where(c => c.Extends is not null))
        {
            diagram += @$"{classe.Extends!.Name} <|--  {classe.Name}" + '\n';
        }

        diagram += "\n";
        return diagram;
    }

    public string GenerateDiagram(MermaidRequest request)
    {
        string diagram = string.Empty;
        var classes = GetClassesForScope(request.Uri, request.Scope).Where(c => c.IsPersistent);
        diagram += "classDiagram\n";
        diagram += GenerateDiagramClasses(classes);
        return diagram;
    }

    public IEnumerable<Class> GetClassesForScope(string uri, MermaidScope scope)
    {
        var file = modelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == uri);
        return scope switch
        {
            MermaidScope.File => file?.Classes ?? Enumerable.Empty<Class>(),
            MermaidScope.Module => modelStore.Files.Where(f => f.Namespace.Module == file?.Namespace.Module)?.SelectMany(f => f.Classes) ?? Enumerable.Empty<Class>(),
            MermaidScope.Model => modelStore.Files.SelectMany(f => f.Classes),
            _ => []
        };
    }

    public string GetFileName(string uri)
    {
        var file = modelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == uri);
        if (file is null)
        {
            return string.Empty;
        }

        return file!.Name.Split("/").Last();
    }

    public string GetModule(string uri)
    {
        var file = modelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == uri);
        if (file is null)
        {
            return string.Empty;
        }

        return file!.Namespace.Module;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest, TResponse}.Handle" />
    public async Task<Mermaid> Handle(MermaidRequest request, CancellationToken cancellationToken)
    {
        await modelStore.WaitForUpdates();
        var result = GenerateDiagram(request);
        return new Mermaid(result, GetModule(request.Uri), GetFileName(request.Uri), request.Scope);
    }
}