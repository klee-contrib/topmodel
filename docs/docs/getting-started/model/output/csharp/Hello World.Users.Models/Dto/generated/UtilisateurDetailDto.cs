////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Hello World.Refs.Models;
using Kinetix.Modeling.Annotations;

namespace Hello World.Users.Models;

/// <summary>
/// Objet de transfert pour la classe Utilisateur, dans le cas de la consultation de la page de détail.
/// </summary>
public partial record UtilisateurDetailDto
{
    /// <summary>
    /// Adresse mail de l'utilisateur.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Email { get; set; }

    /// <summary>
    /// Nom de l'utilisateur.
    /// </summary>
    [StringLength(15)]
    public string Nom { get; set; }

    /// <summary>
    /// Date d'inscription.
    /// </summary>
    public DateTime? DateInscription { get; set; }

    /// <summary>
    /// Type de l'utilisateur.
    /// </summary>
    [ReferencedType(typeof(TypeUtilisateur))]
    public TypeUtilisateur.Codes? TypeUtilisateurCode { get; set; }

    /// <summary>
    /// Libellé du type d'utilisateur.
    /// </summary>
    [Required]
    [StringLength(15)]
    public string LibelleTypeUtilisateur { get; set; }
}
