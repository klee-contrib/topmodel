////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Hello World.Refs.Models;
using Kinetix.Modeling.Annotations;

namespace Hello World.Users.Models;

/// <summary>
/// Objet de transfert pour la classe Utilisateur dans le cas d'une création.
/// </summary>
public partial record UtilisateurCreateDto
{
    /// <summary>
    /// Adresse mail de l'utilisateur.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string UtilisateurEmail { get; set; }

    /// <summary>
    /// Nom de l'utilisateur.
    /// </summary>
    [StringLength(15)]
    public string UtilisateurNom { get; set; }

    /// <summary>
    /// Date d'inscription.
    /// </summary>
    public DateTime? UtilisateurDateInscription { get; set; }

    /// <summary>
    /// Type de l'utilisateur.
    /// </summary>
    [ReferencedType(typeof(TypeUtilisateur))]
    public TypeUtilisateur.Codes? UtilisateurTypeUtilisateurCode { get; set; }
}
