////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Ligne d'une commande.
/// </summary>
[Table("ligne_commande")]
public partial record LigneCommande
{
    /// <summary>
    /// Identifiant de la ligne.
    /// </summary>
    [Column("lig_id")]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Quantité commandée.
    /// </summary>
    [Column("lig_quantite")]
    [Required]
    public int? Quantite { get; set; }

    /// <summary>
    /// Prix unitaire au moment de la commande.
    /// </summary>
    [Column("lig_prix_unitaire")]
    [Required]
    public decimal? PrixUnitaire { get; set; }

    /// <summary>
    /// Prix total de la ligne.
    /// </summary>
    [Column("lig_prix_total")]
    [Required]
    public decimal? PrixTotal { get; set; }

    /// <summary>
    /// Commande à laquelle appartient la ligne.
    /// </summary>
    [Required]
    public Commande? Commande { get; set; }

    /// <summary>
    /// Plat commandé.
    /// </summary>
    [Required]
    public Plat? Plat { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Column("lig_date_creation")]
    [Required]
    public DateTime? DateCreation { get; init; }
}
