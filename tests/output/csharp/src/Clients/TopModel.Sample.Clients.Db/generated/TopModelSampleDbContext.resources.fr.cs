////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Microsoft.EntityFrameworkCore;
using TopModel.Sample.Clients.Db.Models.Common;

namespace TopModel.Sample.Clients.Db;

/// <summary>
/// Partial pour ajouter les traductions dans EF.
/// </summary>
public partial class TopModelSampleDbContext
{
    partial void AddFrResources(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Translation>().HasData(
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Autre", Lang = "fr", Value = "Autre" },
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Boisson", Lang = "fr", Value = "Boisson" },
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Dessert", Lang = "fr", Value = "Dessert" },
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Entree", Lang = "fr", Value = "Entrée" },
            new Translation { ResourceKey = "restaurant.categoriePlat.values.Principal", Lang = "fr", Value = "Plat principal" },
            new Translation { ResourceKey = "restaurant.departement.values.HautsDeSeine", Lang = "fr", Value = "Hauts de Seine" },
            new Translation { ResourceKey = "restaurant.departement.values.Paris", Lang = "fr", Value = "Paris" },
            new Translation { ResourceKey = "restaurant.departement.values.SeineEtMarne", Lang = "fr", Value = "Seine et Marne" },
            new Translation { ResourceKey = "restaurant.departement.values.SeineSaintDenis", Lang = "fr", Value = "Seine Saint Denis" },
            new Translation { ResourceKey = "restaurant.region.values.Idf", Lang = "fr", Value = "Île de France" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.Annulee", Lang = "fr", Value = "Annulée" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.EnAttente", Lang = "fr", Value = "En attente" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.EnPreparation", Lang = "fr", Value = "En préparation" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.Prete", Lang = "fr", Value = "Prête" },
            new Translation { ResourceKey = "restaurant.statutCommande.values.Servie", Lang = "fr", Value = "Servie" }
        );
    }
}
