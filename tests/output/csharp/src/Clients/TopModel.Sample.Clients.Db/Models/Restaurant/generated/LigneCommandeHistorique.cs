////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Ligne de commande pour historique avec préservation des clés primaires.
/// </summary>
[Table("ligne_commande_historique")]
public partial record LigneCommandeHistorique
{
    /// <summary>
    /// Identifiant de la ligne.
    /// </summary>
    [Column("lig_id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
    /// Plat commandé.
    /// </summary>
    [Column("pla_id")]
    [Required]
    public int? PlatId { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Column("lig_date_creation")]
    [Required]
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Commande à laquelle appartient la ligne.
    /// </summary>
    [Column("com_id")]
    [Required]
    public int? CommandeHistoriqueId { get; set; }
}
