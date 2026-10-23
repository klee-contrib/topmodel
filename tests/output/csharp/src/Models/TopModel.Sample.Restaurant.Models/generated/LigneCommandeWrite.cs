////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une ligne de commande en écriture.
/// </summary>
public partial record LigneCommandeWrite
{
    /// <summary>
    /// Quantité commandée.
    /// </summary>
    [Required]
    public int? Quantite { get; set; }

    /// <summary>
    /// Prix unitaire au moment de la commande.
    /// </summary>
    [Required]
    public decimal? PrixUnitaire { get; set; }

    /// <summary>
    /// Prix total de la ligne.
    /// </summary>
    [Required]
    public decimal? PrixTotal { get; set; }

    /// <summary>
    /// Commande à laquelle appartient la ligne.
    /// </summary>
    [Required]
    public int? CommandeId { get; set; }

    /// <summary>
    /// Plat commandé.
    /// </summary>
    [Required]
    public int? PlatId { get; set; }
}
