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
    partial void AddDeResources(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Translation>().HasData(
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Boisson", Lang = "de", Value = "Boisson" },
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Dessert", Lang = "de", Value = "Dessert" },
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Entree", Lang = "de", Value = "Entrée" },
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Plat", Lang = "de", Value = "Plat principal" },
            new Translation { ResourceKey = "restaurant.departement.values.HautsDeSeine", Lang = "de", Value = "Hauts de Seine" },
            new Translation { ResourceKey = "restaurant.departement.values.Paris", Lang = "de", Value = "Paris" },
            new Translation { ResourceKey = "restaurant.departement.values.SeineEtMarne", Lang = "de", Value = "Seine et Marne" },
            new Translation { ResourceKey = "restaurant.departement.values.SeineSaintDenis", Lang = "de", Value = "Seine Saint Denis" },
            new Translation { ResourceKey = "restaurant.region.values.Idf", Lang = "de", Value = "Île de France" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.Annulee", Lang = "de", Value = "Annulée" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.EnAttente", Lang = "de", Value = "En attente" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.EnPreparation", Lang = "de", Value = "En préparation" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.Prete", Lang = "de", Value = "Prête" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.Servie", Lang = "de", Value = "Servie" }
        );
    }
}
