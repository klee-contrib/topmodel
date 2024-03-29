﻿using MediatR;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.LanguageServer;

public class MermaidHandler : IRequestHandler<MermaidRequest, Mermaid>, IJsonRpcHandler
{
    private readonly ILanguageServerFacade _facade;
    private readonly ModelStore _modelStore;

    public MermaidHandler(ModelStore modelStore, ILanguageServerFacade facade)
    {
        _facade = facade;
        _modelStore = modelStore;
    }

    public static string GenerateDiagramFile(ModelFile file)
    {
        string diagram = string.Empty;
        var classes = file.Classes
            .Where(c => c.IsPersistent);

        diagram += "classDiagram\n";
        var notClasses = new List<Class>();
        foreach (var classe in classes)
        {
            diagram += @$"%% {classe.Comment}" + '\n';
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

            foreach (var property in classe.Properties.OfType<AssociationProperty>())
            {
                if (property.Association.ModelFile != file)
                {
                    notClasses.Add(property.Association);
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

        foreach (var classe in notClasses)
        {
            diagram += @$"%% {classe.Comment}" + '\n';
            diagram += @$"class {classe.Name}:::fileReference" + '\n';
        }

        foreach (var classe in classes.Where(c => c.Extends is not null))
        {
            diagram += @$"{classe.Extends!.Name} <|--  {classe.Name}" + '\n';
        }

        diagram += "\n";
        return diagram;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest, TResponse}.Handle" />
    public Task<Mermaid> Handle(MermaidRequest request, CancellationToken cancellationToken)
    {
        var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.Uri);
        var result = GenerateDiagramFile(file!);
        return Task.FromResult(new Mermaid(result, file!.Name));
    }
}