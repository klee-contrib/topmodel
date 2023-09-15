////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using CSharp.Clients.Db.Models.Securite.Profil;
using CSharp.Clients.Db.Models.Securite.Utilisateur;
using Microsoft.EntityFrameworkCore;
using Models.CSharp.Securite.Profil.Models;
using Models.CSharp.Securite.Utilisateur.Models;

namespace CSharp.Clients.Db;

/// <summary>
/// Partial pour ajouter les commentaires EF.
/// </summary>
public partial class CSharpDbContext : DbContext
{
    partial void AddComments(ModelBuilder modelBuilder)
    {
        var droit = modelBuilder.Entity<Droit>();
        droit.ToTable(t => t.HasComment("Droits de l'application"));
        droit.Property(p => p.Code).HasComment("Code du droit");
        droit.Property(p => p.Libelle).HasComment("Libellé du droit");
        droit.Property(p => p.TypeDroitCode).HasComment("Type de profil pouvant faire l'action");

        var profil = modelBuilder.Entity<Profil>();
        profil.ToTable(t => t.HasComment("Profil des utilisateurs"));
        profil.Property(p => p.Id).HasComment("Id technique");
        profil.Property(p => p.Libelle).HasComment("Libellé du profil.");
        profil.Property(p => p.Droits).HasComment("Liste des droits du profil");
        profil.Property(p => p.DateCreation).HasComment("Date de création de l'utilisateur.");
        profil.Property(p => p.DateModification).HasComment("Date de modification de l'utilisateur.");

        var typeDroit = modelBuilder.Entity<TypeDroit>();
        typeDroit.ToTable(t => t.HasComment("Type de droit"));
        typeDroit.Property(p => p.Code).HasComment("Code du type de droit");
        typeDroit.Property(p => p.Libelle).HasComment("Libellé du type de droit");

        var typeUtilisateur = modelBuilder.Entity<TypeUtilisateur>();
        typeUtilisateur.ToTable(t => t.HasComment("Type d'utilisateur"));
        typeUtilisateur.Property(p => p.Code).HasComment("Code du type d'utilisateur");
        typeUtilisateur.Property(p => p.Libelle).HasComment("Libellé du type d'utilisateur");

        var utilisateur = modelBuilder.Entity<Utilisateur>();
        utilisateur.ToTable(t => t.HasComment("Utilisateur de l'application"));
        utilisateur.Property(p => p.Id).HasComment("Id de l'utilisateur");
        utilisateur.Property(p => p.Nom).HasComment("Nom de l'utilisateur");
        utilisateur.Property(p => p.Prenom).HasComment("Nom de l'utilisateur");
        utilisateur.Property(p => p.Email).HasComment("Email de l'utilisateur");
        utilisateur.Property(p => p.DateNaissance).HasComment("Age de l'utilisateur");
        utilisateur.Property(p => p.Actif).HasComment("Si l'utilisateur est actif");
        utilisateur.Property(p => p.ProfilId).HasComment("Profil de l'utilisateur");
        utilisateur.Property(p => p.TypeUtilisateurCode).HasComment("Type d'utilisateur");
        utilisateur.Property(p => p.DateCreation).HasComment("Date de création de l'utilisateur.");
        utilisateur.Property(p => p.DateModification).HasComment("Date de modification de l'utilisateur.");
    }
}
