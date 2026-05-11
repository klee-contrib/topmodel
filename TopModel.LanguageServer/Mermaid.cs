using TopModel.Utils.Mermaid;

namespace TopModel.LanguageServer;

public record Mermaid(string Diagram, string App, string Module, string FileName, MermaidScope Scope);
