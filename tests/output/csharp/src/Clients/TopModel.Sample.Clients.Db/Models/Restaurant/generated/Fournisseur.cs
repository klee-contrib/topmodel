////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Restaurant.
/// </summary>
public partial record Fournisseur : Lieu
{
    /// <summary>
    /// Numéro de téléphone.
    /// </summary>
    [Column("frn_telephone")]
    [Domain(Domains.Telephone)]
    [StringLength(20)]
    public string? Telephone { get; set; }

    /// <summary>
    /// Si le fournisseur fait du bio.
    /// </summary>
    [Column("frn_bio")]
    [Domain(Domains.Booleen)]
    public bool? Bio { get; set; }
}
