////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Statut d'une commande.
/// </summary>
public enum StatutCommande
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
