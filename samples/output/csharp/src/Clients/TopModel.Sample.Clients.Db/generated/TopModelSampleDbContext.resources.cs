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
            new Traduction { ResourceKey = "securite.profil.droit.values.Create", Label = "Création" },
            new Traduction { ResourceKey = "securite.profil.droit.values.Delete", Label = "Suppression" },
            new Traduction { ResourceKey = "securite.profil.droit.values.Read", Label = "Lecture" },
            new Traduction { ResourceKey = "securite.profil.droit.values.Update", Label = "Mise à jour" },
            new Traduction { ResourceKey = "securite.profil.typeDroit.values.Admin", Label = "Administration" },
            new Traduction { ResourceKey = "securite.profil.typeDroit.values.Read", Label = "Lecture" },
            new Traduction { ResourceKey = "securite.profil.typeDroit.values.Write", Label = "Ecriture" },
            new Traduction { ResourceKey = "securite.utilisateur.typeUtilisateur.values.Admin", Label = "Administrateur" },
            new Traduction { ResourceKey = "securite.utilisateur.typeUtilisateur.values.Client", Label = "Client" },
            new Traduction { ResourceKey = "securite.utilisateur.typeUtilisateur.values.Gestionnaire", Label = "Gestionnaire" }
        );
    }
}
