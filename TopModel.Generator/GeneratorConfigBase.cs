#nullable disable
namespace TopModel.Generator;

public abstract class GeneratorConfigBase
{
    /// <summary>
    /// Racine du répertoire de génération.
    /// </summary>
    public string OutputDirectory { get; set; }

    /// <summary>
    /// Tags du générateur.
    /// </summary>
    public IList<string> Tags { get; set; }

    /// <summary>
    /// Variables globales du générateur.
    /// </summary>
    public Dictionary<string, string> Variables { get; set; } = new();

    /// <summary>
    /// Résout les variables globales.
    /// </summary>
    public void ResolveVariables()
    {
        foreach (var (varName, varValue) in Variables)
        {
            foreach (var property in GetType().GetProperties().Where(p => p.PropertyType == typeof(string)))
            {
                var value = (string)property.GetValue(this);
                if (value != null)
                {
                    property.SetValue(this, value.Replace($"{{{varName}}}", varValue));
                }
            }
        }
    }
}