////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopModel.Sample.Clients.Db.Models;

public record NombreVuesBase
{
    /// <summary>
    /// Nombre de vues de l'avis (calculé).
    /// </summary>
    [NotMapped]
    [Required]
    public int? NombreVues { get; init; } = 0;
}
