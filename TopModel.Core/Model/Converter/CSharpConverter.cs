namespace TopModel.Core;

public class CSharpConverter
{
#nullable disable

    /// <summary>
    /// Implémentation du convertisseur. Accepte les templates. Utiliser {value} pour la valeur du paramètre d'entrée, et {to.type} ou {from.type} pour les propriétés du domaine source ou cible
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Usings à ajouter pour utiliser ce décorateur.
    /// </summary>
    public IList<string> Usings { get; set; } = new List<string>();
}
