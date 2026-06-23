////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

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
    [Domain(Domains.Id2)]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int? Id { get; set; }

    /// <summary>
    /// Quantité commandée.
    /// </summary>
    [Column("lig_quantite")]
    [Required]
    [Domain(Domains.Quantite)]
    public int? Quantite { get; set; }

    /// <summary>
    /// Prix unitaire au moment de la commande.
    /// </summary>
    [Column("lig_prix_unitaire")]
    [Required]
    [Domain(Domains.Prix)]
    public decimal? PrixUnitaire { get; set; }

    /// <summary>
    /// Prix total de la ligne.
    /// </summary>
    [Column("lig_prix_total")]
    [Required]
    [Domain(Domains.Prix)]
    public decimal? PrixTotal { get; set; }

    /// <summary>
    /// Plat commandé.
    /// </summary>
    [Column("pla_id")]
    [Required]
    [Domain(Domains.SeqId)]
    public int? PlatId { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Column("lig_date_creation")]
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Commande à laquelle appartient la ligne.
    /// </summary>
    [Column("com_id")]
    [Required]
    [Domain(Domains.Id2)]
    public int? CommandeHistoriqueId { get; set; }
}
