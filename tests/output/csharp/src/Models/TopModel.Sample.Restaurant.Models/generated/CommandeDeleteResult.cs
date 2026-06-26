////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Résultat de suppression d'une commande.
/// </summary>
public partial record CommandeDeleteResult
{
    /// <summary>
    /// Identifiant de la commande.
    /// </summary>
    [Required]
    [Domain(Domains.Id2)]
    public int? Id { get; set; }

    /// <summary>
    /// Date et heure de la commande.
    /// </summary>
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateCommande { get; set; }
}
