namespace TopModel.Core.Model.Implementation;

public class ConverterImplementation
{
    /// <summary>
    /// Implémentation du convertisseur. Accepte les templates. Utiliser {value} pour la valeur du paramètre d'entrée, et {to.type} ou {from.type} pour les propriétés du domaine source ou cible
    /// </summary>
    public required string Text { get; set; }

    /// <summary>
    /// Imports à ajouter pour utiliser ce décorateur.
    /// </summary>
    public IList<string> Imports { get; set; } = [];
}