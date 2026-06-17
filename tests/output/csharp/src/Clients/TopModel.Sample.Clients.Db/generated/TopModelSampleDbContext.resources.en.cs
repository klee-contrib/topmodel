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
    partial void AddEnResources(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Translation>().HasData(
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Autre", Lang = "en", Value = "Autre" },
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Boisson", Lang = "en", Value = "Drink" },
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Dessert", Lang = "en", Value = "Dessert" },
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Entree", Lang = "en", Value = "Starter" },
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Principal", Lang = "en", Value = "Plat principal" },
            new Translation { ResourceKey = "restaurant.departement.values.HautsDeSeine", Lang = "en", Value = "Hauts de Seine" },
            new Translation { ResourceKey = "restaurant.departement.values.Paris", Lang = "en", Value = "Paris" },
            new Translation { ResourceKey = "restaurant.departement.values.SeineEtMarne", Lang = "en", Value = "Seine et Marne" },
            new Translation { ResourceKey = "restaurant.departement.values.SeineSaintDenis", Lang = "en", Value = "Seine Saint Denis" },
            new Translation { ResourceKey = "restaurant.region.values.Idf", Lang = "en", Value = "Île de France" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.Annulee", Lang = "en", Value = "Cancelled" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.EnAttente", Lang = "en", Value = "Pending" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.EnPreparation", Lang = "en", Value = "In preparation" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.Prete", Lang = "en", Value = "Ready" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.Servie", Lang = "en", Value = "Served" }
        );
    }
}
