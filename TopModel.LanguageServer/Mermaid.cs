namespace TopModel.LanguageServer;

public class Mermaid : IEquatable<Mermaid>
{
    public readonly string Module;
    public readonly string Diagram;
    public Mermaid(string diagram, string module)
    { 
        this.Diagram = diagram;
        this.Module = module;
    }
    public bool Equals(Mermaid? other)
    {
        return true;
    }
}