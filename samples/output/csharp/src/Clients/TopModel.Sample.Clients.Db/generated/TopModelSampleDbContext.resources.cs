////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Microsoft.EntityFrameworkCore;
using TopModel.Sample.Clients.Db.Models.Common;

namespace TopModel.Sample.Clients.Db;

/// <summary>
/// Partial pour ajouter les traductions dans EF.
/// </summary>
public partial class TopModelSampleDbContext : DbContext
{
    partial void AddResources(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Traduction>().HasData(
            new Traduction { ResourceKey = "securite.profil.droit.code", Label = "Droit" },
            new Traduction { ResourceKey = "securite.profil.droit.libelle", Label = "Droit" },
            new Traduction { ResourceKey = "securite.profil.droit.values.Create", Label = "Création" },
            new Traduction { ResourceKey = "securite.profil.droit.values.Delete", Label = "Suppression" },
            new Traduction { ResourceKey = "securite.profil.droit.values.Read", Label = "Lecture" },
            new Traduction { ResourceKey = "securite.profil.droit.values.Update", Label = "Mise à jour" },
            new Traduction { ResourceKey = "common.entityListeners.dateCreation", Label = "Date de création" },
            new Traduction { ResourceKey = "common.entityListeners.dateModification", Label = "Date de modification" },
            new Traduction { ResourceKey = "securite.profil.profil.id", Label = "Id technique du profil" },
            new Traduction { ResourceKey = "securite.profil.profil.libelle", Label = "Libellé du profil" },
            new Traduction { ResourceKey = "securite.profil.profilItem.nombreUtilisateurs", Label = "Nombre d'utilisateurs affectés" },
            new Traduction { ResourceKey = "securite.profil.profilRead.droits", Label = "Droits" },
            new Traduction { ResourceKey = "securite.profil.typeDroit.code", Label = "Type de droit" },
            new Traduction { ResourceKey = "securite.profil.typeDroit.values.Admin", Label = "Administration" },
            new Traduction { ResourceKey = "securite.profil.typeDroit.values.Read", Label = "Lecture" },
            new Traduction { ResourceKey = "securite.profil.typeDroit.values.Write", Label = "Ecriture" },
            new Traduction { ResourceKey = "securite.utilisateur.typeUtilisateur.code", Label = "Type d'utilisateur" },
            new Traduction { ResourceKey = "securite.utilisateur.typeUtilisateur.values.Admin", Label = "Administrateur" },
            new Traduction { ResourceKey = "securite.utilisateur.typeUtilisateur.values.Client", Label = "Client" },
            new Traduction { ResourceKey = "securite.utilisateur.typeUtilisateur.values.Gestionnaire", Label = "Gestionnaire" },
            new Traduction { ResourceKey = "securite.utilisateur.utilisateur.adresse", Label = "Adresse" },
            new Traduction { ResourceKey = "securite.utilisateur.utilisateur.dateNaissance", Label = "Date de naissance" },
            new Traduction { ResourceKey = "securite.utilisateur.utilisateur.email", Label = "Adresse email" },
            new Traduction { ResourceKey = "securite.utilisateur.utilisateur.id", Label = "Id technique" },
            new Traduction { ResourceKey = "securite.utilisateur.utilisateur.prenom", Label = "Prénom" },
            new Traduction { ResourceKey = "securite.utilisateur.utilisateur.profilId", Label = "Profil" },
            new Traduction { ResourceKey = "securite.utilisateur.utilisateur.typeUtilisateurCode", Label = "Type d'utilisateur" }
        );
    }
}
