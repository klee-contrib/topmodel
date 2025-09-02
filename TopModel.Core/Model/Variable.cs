namespace TopModel.Core.Model;

public class Variable
{
    private readonly string? _description = null;

    public IList<string> Configs { get; init; } = [];

    public IList<string> Tags { get; init; } = [];

    public IList<string> Languages { get; init; } = [];

    public bool ByTag { get; init; }

    public TemplateParameter? TemplateParameter { get; init; }

    public string Description
    {
        get
        {
            if (_description != null)
            {
                return _description;
            }

            if (TemplateParameter != null)
            {
                return TemplateParameter.Description;
            }

            return $"Variable de configuration ({(ByTag ? "par tag" : "globale")}) pour les modules : \n- {string.Join("\n- ", Configs)}";
        }
        init => _description = value;
    }
}
