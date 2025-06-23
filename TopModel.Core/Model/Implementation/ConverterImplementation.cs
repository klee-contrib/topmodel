using YamlDotNet.Serialization;

namespace TopModel.Core.Model.Implementation;

public class ConverterImplementation
{
    /// <summary>
    /// Implémentation du convertisseur. Accepte les templates. Utiliser {value} pour la valeur du paramètre d'entrée, et {to.type} ou {from.type} pour les propriétés du domaine source ou cible
    /// </summary>
    [YamlIgnore]
    public string Text => TextWithVariables;

    /// <summary>
    /// Imports à ajouter pour utiliser ce décorateur.
    /// </summary>
    public IList<string> Imports { get; set; } = [];

#nullable disable
    [YamlMember(Alias = "text")]
    public StringWithVariables TextWithVariables { get; internal set; }
}