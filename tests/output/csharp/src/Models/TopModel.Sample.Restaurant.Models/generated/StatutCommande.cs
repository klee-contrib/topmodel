////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Statut d'une commande.
/// </summary>
[Reference(true)]
[DefaultProperty(nameof(Libelle))]
[Table("statut_commande")]
public partial record StatutCommande
{
    /// <summary>
    /// Valeurs possibles de la liste de référence StatutCommande.
    /// </summary>
    public enum Codes
    {
        /// <summary>
        /// Annulée.
        /// </summary>
        ANNULE,

        /// <summary>
        /// En attente.
        /// </summary>
        EN_ATT,

        /// <summary>
        /// En préparation.
        /// </summary>
        EN_PREP,

        /// <summary>
        /// Prête.
        /// </summary>
        PRETE,

        /// <summary>
        /// Servie.
        /// </summary>
        SERVIE
    }

    /// <summary>
    /// Code du statut.
    /// </summary>
    [Column("stc_code")]
    [Domain(Domains.Code)]
    [Key]
    public Codes? Code { get; set; }

    /// <summary>
    /// Libellé du statut.
    /// </summary>
    [Column("stc_libelle")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Libelle { get; set; }
}
