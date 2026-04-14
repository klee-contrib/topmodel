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
        modelBuilder.Entity<Translation>().HasData(
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Boisson", Value = "Boisson" },
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Dessert", Value = "Dessert" },
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Entree", Value = "Entrée" },
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Plat", Value = "Plat principal" },
            new Translation { ResourceKey = "restaurant.departement.values.HautsDeSeine", Value = "Hauts de Seine" },
            new Translation { ResourceKey = "restaurant.departement.values.Paris", Value = "Paris" },
            new Translation { ResourceKey = "restaurant.departement.values.SeineEtMarne", Value = "Seine et Marne" },
            new Translation { ResourceKey = "restaurant.departement.values.SeineSaintDenis", Value = "Seine Saint Denis" },
            new Translation { ResourceKey = "restaurant.region.values.Idf", Value = "Île de France" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.Annulee", Value = "Annulée" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.EnAttente", Value = "En attente" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.EnPreparation", Value = "En préparation" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.Prete", Value = "Prête" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.Servie", Value = "Servie" }
        );
    }
}
