using TopModel.Utils.Mermaid;

namespace TopModel.LanguageServer.Handlers.Mermaid;

public record MermaidResponse(string Diagram, string Module, string FileName, MermaidScope Scope);
