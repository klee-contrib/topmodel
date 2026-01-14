////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Microsoft.EntityFrameworkCore;
using TopModel.Sample.Clients.Db.Models.Restaurant;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db;

/// <summary>
/// Partial pour ajouter les commentaires EF.
/// </summary>
public partial class TopModelSampleDbContext : DbContext
{
    partial void AddComments(ModelBuilder modelBuilder)
    {
        var avisClient = modelBuilder.Entity<AvisClient>();
        avisClient.ToTable(t => t.HasComment("Avis d'un client sur un restaurant"));
        avisClient.Property(p => p.Id).HasComment("Identifiant de l'avis");
        avisClient.Property(p => p.Note).HasComment("Note sur 5");
        avisClient.Property(p => p.Commentaire).HasComment("Commentaire de l'avis");
        avisClient.Property(p => p.DateAvis).HasComment("Date de l'avis");
        avisClient.Property(p => p.Approuve).HasComment("Indique si l'avis est approuvé par le restaurant");
        avisClient.Property(p => p.NombreVues).HasComment("Nombre de vues de l'avis (calculé)");
        avisClient.Property("ClientId").HasComment("Client ayant donné l'avis");
        avisClient.Property("RestaurantId").HasComment("Restaurant concerné par l'avis");

        var categoriePlat = modelBuilder.Entity<CategoriePlat>();
        categoriePlat.ToTable(t => t.HasComment("Catégorie de plat"));
        categoriePlat.Property(p => p.Code).HasComment("Code de la catégorie");
        categoriePlat.Property(p => p.Libelle).HasComment("Libellé de la catégorie");

        var client = modelBuilder.Entity<Client>();
        client.ToTable(t => t.HasComment("Client du restaurant"));
        client.Property(p => p.Email).HasComment("Adresse email du client");

        var commande = modelBuilder.Entity<Commande>();
        commande.ToTable(t => t.HasComment("Commande d'un client"));
        commande.Property(p => p.Id).HasComment("Identifiant de la commande");
        commande.Property(p => p.DateCommande).HasComment("Date et heure de la commande");
        commande.Property(p => p.DateLivraison).HasComment("Date et heure de livraison");
        commande.Property(p => p.MontantTotal).HasComment("Montant total de la commande");
        commande.Property("ClientId").HasComment("Client ayant passé la commande");
        commande.Property("TableId").HasComment("Table associée à la commande");
        commande.Property("ReservationId").HasComment("Réservation associée à la commande");
        commande.Property("StatutCommandeCode").HasComment("Statut de la commande");
        commande.Property("AvisClientId").HasComment("Avis laissé par le client sur la commande.");

        var commandeHistorique = modelBuilder.Entity<CommandeHistorique>();
        commandeHistorique.ToTable(t => t.HasComment("Commande pour historique avec préservation des clés primaires"));
        commandeHistorique.Property(p => p.Id).HasComment("Identifiant de la commande");
        commandeHistorique.Property(p => p.DateCommande).HasComment("Date et heure de la commande");
        commandeHistorique.Property(p => p.DateLivraison).HasComment("Date et heure de livraison");
        commandeHistorique.Property(p => p.MontantTotal).HasComment("Montant total de la commande");
        commandeHistorique.Property("ClientId").HasComment("Client ayant passé la commande");
        commandeHistorique.Property("TableId").HasComment("Table associée à la commande");
        commandeHistorique.Property("ReservationId").HasComment("Réservation associée à la commande");
        commandeHistorique.Property("StatutCommandeCode").HasComment("Statut de la commande");
        commandeHistorique.Property("AvisClientId").HasComment("Avis laissé par le client sur la commande.");

        var employe = modelBuilder.Entity<Employe>();
        employe.ToTable(t => t.HasComment("Employé du restaurant"));
        employe.Property(p => p.Telephone).HasComment("Numéro de téléphone de l'employé.");
        employe.Property(p => p.DateNaissance).HasComment("Date de naissance");
        employe.Property(p => p.Matricule).HasComment("Matricule de l'employé");
        employe.Property(p => p.DateEmbauche).HasComment("Date d'embauche");
        employe.Property(p => p.Salaire).HasComment("Salaire de l'employé");
        employe.Property("RestaurantId").HasComment("Restaurant où travaille l'employé");

        var ligneCommande = modelBuilder.Entity<LigneCommande>();
        ligneCommande.ToTable(t => t.HasComment("Ligne d'une commande"));
        ligneCommande.Property(p => p.Id).HasComment("Identifiant de la ligne");
        ligneCommande.Property(p => p.Quantite).HasComment("Quantité commandée");
        ligneCommande.Property(p => p.PrixUnitaire).HasComment("Prix unitaire au moment de la commande");
        ligneCommande.Property(p => p.PrixTotal).HasComment("Prix total de la ligne");
        ligneCommande.Property("CommandeId").HasComment("Commande à laquelle appartient la ligne");
        ligneCommande.Property("PlatId").HasComment("Plat commandé");

        var ligneCommandeHistorique = modelBuilder.Entity<LigneCommandeHistorique>();
        ligneCommandeHistorique.ToTable(t => t.HasComment("Ligne de commande pour historique avec préservation des clés primaires"));
        ligneCommandeHistorique.Property(p => p.Id).HasComment("Identifiant de la ligne");
        ligneCommandeHistorique.Property(p => p.Quantite).HasComment("Quantité commandée");
        ligneCommandeHistorique.Property(p => p.PrixUnitaire).HasComment("Prix unitaire au moment de la commande");
        ligneCommandeHistorique.Property(p => p.PrixTotal).HasComment("Prix total de la ligne");
        ligneCommandeHistorique.Property("PlatId").HasComment("Plat commandé");
        ligneCommandeHistorique.Property("CommandeHistoriqueId").HasComment("Commande à laquelle appartient la ligne");

        var menu = modelBuilder.Entity<Menu>();
        menu.ToTable(t => t.HasComment("Menu du restaurant"));
        menu.Property(p => p.Id).HasComment("Identifiant du menu");
        menu.Property(p => p.Nom).HasComment("Nom du menu");
        menu.Property(p => p.Description).HasComment("Description du menu");
        menu.Property(p => p.Prix).HasComment("Prix du menu");
        menu.Property(p => p.Disponible).HasComment("Indique si le menu est disponible");
        menu.Property(p => p.DateDebut).HasComment("Date de début de validité du menu");
        menu.Property(p => p.DateFin).HasComment("Date de fin de validité du menu");
        menu.Property("RestaurantId").HasComment("Restaurant proposant ce menu");

        var menuPlat = modelBuilder.Entity<MenuPlat>();
        menuPlat.ToTable(t => t.HasComment("Plat dans un menu"));
        menuPlat.Property("MenuId").HasComment("Menu contenant ce plat");
        menuPlat.Property("PlatId").HasComment("Plat du menu");
        menuPlat.Property(p => p.Ordre).HasComment("Ordre d'affichage du plat dans le menu");

        var personne = modelBuilder.Entity<Personne>();
        personne.ToTable(t => t.HasComment("Classe de base représentant une personne"));
        personne.Property(p => p.Id).HasComment("Identifiant de la personne");
        personne.Property(p => p.Nom).HasComment("Nom de la personne");
        personne.Property(p => p.Prenom).HasComment("Prénom de la personne");

        var plat = modelBuilder.Entity<Plat>();
        plat.ToTable(t => t.HasComment("Plat du menu"));
        plat.Property(p => p.Id).HasComment("Identifiant du plat");
        plat.Property(p => p.Nom).HasComment("Nom du plat");
        plat.Property(p => p.Description).HasComment("Description du plat");
        plat.Property(p => p.Prix).HasComment("Prix du plat");
        plat.Property(p => p.Disponible).HasComment("Indique si le plat est disponible");
        plat.Property("CategoriePlatCode").HasComment("Catégorie du plat");
        plat.Property("RestaurantId").HasComment("Restaurant proposant ce plat");

        var promotion = modelBuilder.Entity<Promotion>();
        promotion.ToTable(t => t.HasComment("Promotion sur un plat"));
        promotion.Property("PlatId").HasComment("Plat concerné par la promotion.");
        promotion.Property(p => p.Libelle).HasComment("Libellé de la promotion");
        promotion.Property(p => p.PourcentageReduction).HasComment("Pourcentage de réduction (0-100)");
        promotion.Property(p => p.DateDebut).HasComment("Date de début de la promotion");
        promotion.Property(p => p.DateFin).HasComment("Date de fin de la promotion");
        promotion.Property(p => p.Active).HasComment("Indique si la promotion est active");
        promotion.Property("RestaurantId").HasComment("Restaurant concerné par la promotion (null si globale)");

        var reservation = modelBuilder.Entity<Reservation>();
        reservation.ToTable(t => t.HasComment("Réservation d'une table"));
        reservation.Property(p => p.Id).HasComment("Identifiant de la réservation");
        reservation.Property(p => p.DateReservation).HasComment("Date et heure de la réservation");
        reservation.Property(p => p.NombrePersonnes).HasComment("Nombre de personnes");
        reservation.Property(p => p.Commentaire).HasComment("Commentaire sur la réservation");
        reservation.Property(p => p.Confirmee).HasComment("Indique si la réservation est confirmée");
        reservation.Property("ClientId").HasComment("Client ayant fait la réservation");
        reservation.Property("TableId").HasComment("Table réservée");
        reservation.Property("RestaurantId").HasComment("Restaurant concerné par la réservation");

        var restaurant = modelBuilder.Entity<Models.Restaurant.Restaurant>();
        restaurant.ToTable(t => t.HasComment("Restaurant"));
        restaurant.Property(p => p.Id).HasComment("Identifiant du restaurant");
        restaurant.Property(p => p.Nom).HasComment("Nom du restaurant");
        restaurant.Property(p => p.Adresse).HasComment("Adresse du restaurant");
        restaurant.Property(p => p.Telephone).HasComment("Numéro de téléphone");

        var statutCommande = modelBuilder.Entity<StatutCommande>();
        statutCommande.ToTable(t => t.HasComment("Statut d'une commande"));
        statutCommande.Property(p => p.Code).HasComment("Code du statut");
        statutCommande.Property(p => p.Libelle).HasComment("Libellé du statut");

        var tableRestaurant = modelBuilder.Entity<TableRestaurant>();
        tableRestaurant.ToTable(t => t.HasComment("Table du restaurant"));
        tableRestaurant.Property(p => p.Id).HasComment("Identifiant de la table");
        tableRestaurant.Property(p => p.Numero).HasComment("Numéro de la table");
        tableRestaurant.Property(p => p.Capacite).HasComment("Capacité de la table (nombre de places)");
        tableRestaurant.Property(p => p.Disponible).HasComment("Indique si la table est disponible");
        tableRestaurant.Property("RestaurantId").HasComment("Restaurant auquel appartient la table");
    }
}
