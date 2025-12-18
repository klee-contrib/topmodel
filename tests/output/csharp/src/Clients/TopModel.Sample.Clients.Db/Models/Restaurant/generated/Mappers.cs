////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Mappers pour le module 'Restaurant'.
/// </summary>
public static class Mappers
{
    /// <summary>
    /// Crée une nouvelle instance de 'AvisClientRead'.
    /// </summary>
    /// <param name="avisClient">Instance de 'AvisClient'.</param>
    /// <returns>Une nouvelle instance de 'AvisClientRead'.</returns>
    public static AvisClientRead CreateAvisClientRead(AvisClient avisClient)
    {
        ArgumentNullException.ThrowIfNull(avisClient);

        return new AvisClientRead
        {
            Id = avisClient.Id,
            Note = avisClient.Note,
            Commentaire = avisClient.Commentaire,
            DateAvis = avisClient.DateAvis,
            Approuve = avisClient.Approuve,
            ClientId = avisClient.ClientId,
            RestaurantId = avisClient.RestaurantId
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'ClientItem'.
    /// </summary>
    /// <param name="client">Instance de 'Client'.</param>
    /// <returns>Une nouvelle instance de 'ClientItem'.</returns>
    public static ClientItem CreateClientItem(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);

        return new ClientItem
        {
            Id = client.Id,
            Nom = client.Nom,
            Prenom = client.Prenom
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'ClientRead'.
    /// </summary>
    /// <param name="client">Instance de 'Client'.</param>
    /// <returns>Une nouvelle instance de 'ClientRead'.</returns>
    public static ClientRead CreateClientRead(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);

        return new ClientRead
        {
            Id = client.Id,
            Nom = client.Nom,
            Prenom = client.Prenom,
            Email = client.Email,
            AvisClients = client.AvisClients
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'CommandeRead'.
    /// </summary>
    /// <param name="commande">Instance de 'Commande'.</param>
    /// <returns>Une nouvelle instance de 'CommandeRead'.</returns>
    public static CommandeRead CreateCommandeRead(Commande commande)
    {
        ArgumentNullException.ThrowIfNull(commande);

        return new CommandeRead
        {
            Reservation = new() { Id = commande.ReservationId },
            Id = commande.Id,
            DateCommande = commande.DateCommande,
            DateLivraison = commande.DateLivraison,
            MontantTotal = commande.MontantTotal,
            TableId = commande.TableId,
            StatutCommandeCode = commande.StatutCommandeCode
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'EmployeRead'.
    /// </summary>
    /// <param name="employe">Instance de 'Employe'.</param>
    /// <returns>Une nouvelle instance de 'EmployeRead'.</returns>
    public static EmployeRead CreateEmployeRead(Employe employe)
    {
        ArgumentNullException.ThrowIfNull(employe);

        return new EmployeRead
        {
            Id = employe.Id,
            Nom = employe.Nom,
            Prenom = employe.Prenom,
            Telephone = employe.Telephone,
            DateNaissance = employe.DateNaissance,
            Matricule = employe.Matricule,
            DateEmbauche = employe.DateEmbauche,
            Salaire = employe.Salaire,
            RestaurantId = employe.RestaurantId
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'LigneCommandeRead'.
    /// </summary>
    /// <param name="ligneCommande">Instance de 'LigneCommande'.</param>
    /// <returns>Une nouvelle instance de 'LigneCommandeRead'.</returns>
    public static LigneCommandeRead CreateLigneCommandeRead(LigneCommande ligneCommande)
    {
        ArgumentNullException.ThrowIfNull(ligneCommande);

        return new LigneCommandeRead
        {
            Id = ligneCommande.Id,
            Quantite = ligneCommande.Quantite,
            PrixUnitaire = ligneCommande.PrixUnitaire,
            PrixTotal = ligneCommande.PrixTotal,
            CommandeId = ligneCommande.CommandeId,
            PlatId = ligneCommande.PlatId
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'MenuRead'.
    /// </summary>
    /// <param name="menu">Instance de 'Menu'.</param>
    /// <returns>Une nouvelle instance de 'MenuRead'.</returns>
    public static MenuRead CreateMenuRead(Menu menu)
    {
        ArgumentNullException.ThrowIfNull(menu);

        return new MenuRead
        {
            Id = menu.Id,
            Nom = menu.Nom,
            Description = menu.Description,
            Prix = menu.Prix,
            Disponible = menu.Disponible,
            DateDebut = menu.DateDebut,
            DateFin = menu.DateFin,
            RestaurantId = menu.RestaurantId
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'PlatRead'.
    /// </summary>
    /// <param name="plat">Instance de 'Plat'.</param>
    /// <returns>Une nouvelle instance de 'PlatRead'.</returns>
    public static PlatRead CreatePlatRead(Plat plat)
    {
        ArgumentNullException.ThrowIfNull(plat);

        return new PlatRead
        {
            Id = plat.Id,
            Nom = plat.Nom,
            Description = plat.Description,
            Prix = plat.Prix,
            Disponible = plat.Disponible,
            CategoriePlatCode = plat.CategoriePlatCode,
            RestaurantId = plat.RestaurantId
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'PromotionRead'.
    /// </summary>
    /// <param name="promotion">Instance de 'Promotion'.</param>
    /// <returns>Une nouvelle instance de 'PromotionRead'.</returns>
    public static PromotionRead CreatePromotionRead(Promotion promotion)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        return new PromotionRead
        {
            Id = promotion.Id,
            Libelle = promotion.Libelle,
            PourcentageReduction = promotion.PourcentageReduction,
            DateDebut = promotion.DateDebut,
            DateFin = promotion.DateFin,
            Active = promotion.Active,
            RestaurantId = promotion.RestaurantId
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'ReservationRead'.
    /// </summary>
    /// <param name="reservation">Instance de 'Reservation'.</param>
    /// <returns>Une nouvelle instance de 'ReservationRead'.</returns>
    public static ReservationRead CreateReservationRead(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        return new ReservationRead
        {
            Id = reservation.Id,
            DateReservation = reservation.DateReservation,
            NombrePersonnes = reservation.NombrePersonnes,
            Commentaire = reservation.Commentaire,
            Confirmee = reservation.Confirmee,
            ClientId = reservation.ClientId,
            TableId = reservation.TableId,
            RestaurantId = reservation.RestaurantId
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'RestaurantAvecStatistiques'.
    /// </summary>
    /// <param name="restaurant">Instance de 'Restaurant'.</param>
    /// <param name="nombrePlats">Nombre de plats.</param>
    /// <param name="nombreTables">Nombre de tables.</param>
    /// <param name="noteMoyenne">Note moyenne.</param>
    /// <returns>Une nouvelle instance de 'RestaurantAvecStatistiques'.</returns>
    public static RestaurantAvecStatistiques CreateRestaurantAvecStatistiques(Restaurant restaurant, int? nombrePlats = null, int? nombreTables = null, decimal? noteMoyenne = null)
    {
        ArgumentNullException.ThrowIfNull(restaurant);

        return new RestaurantAvecStatistiques
        {
            Id = restaurant.Id,
            Nom = restaurant.Nom,
            Adresse = restaurant.Adresse,
            Telephone = restaurant.Telephone,
            Menus = restaurant.Menus,
            Plats = restaurant.Plats,
            Promotions = restaurant.Promotions,
            AvisClients = restaurant.AvisClients,
            Tables = restaurant.Tables,
            NombrePlats = nombrePlats,
            NombreTables = nombreTables,
            NoteMoyenne = noteMoyenne
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'RestaurantRead'.
    /// </summary>
    /// <param name="restaurant">Instance de 'Restaurant'.</param>
    /// <returns>Une nouvelle instance de 'RestaurantRead'.</returns>
    public static RestaurantRead CreateRestaurantRead(Restaurant restaurant)
    {
        ArgumentNullException.ThrowIfNull(restaurant);

        return new RestaurantRead
        {
            Id = restaurant.Id,
            Nom = restaurant.Nom,
            Adresse = restaurant.Adresse,
            Telephone = restaurant.Telephone,
            Menus = restaurant.Menus,
            Plats = restaurant.Plats,
            Promotions = restaurant.Promotions,
            AvisClients = restaurant.AvisClients,
            Tables = restaurant.Tables
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'StatistiquesRestaurant'.
    /// </summary>
    /// <param name="restaurant">Instance de 'Restaurant'.</param>
    /// <param name="nombreCommandes">Nombre de commandes.</param>
    /// <param name="chiffreAffaires">Chiffre d'affaires.</param>
    /// <param name="nombreClients">Nombre de clients.</param>
    /// <param name="noteMoyenne">Note moyenne.</param>
    /// <returns>Une nouvelle instance de 'StatistiquesRestaurant'.</returns>
    public static StatistiquesRestaurant CreateStatistiquesRestaurant(Restaurant restaurant, int? nombreCommandes = null, decimal? chiffreAffaires = null, int? nombreClients = null, decimal? noteMoyenne = null)
    {
        ArgumentNullException.ThrowIfNull(restaurant);

        return new StatistiquesRestaurant
        {
            NombreCommandes = nombreCommandes,
            ChiffreAffaires = chiffreAffaires,
            NombreClients = nombreClients,
            NoteMoyenne = noteMoyenne
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'TableRead'.
    /// </summary>
    /// <param name="table">Instance de 'TableRestaurant'.</param>
    /// <returns>Une nouvelle instance de 'TableRead'.</returns>
    public static TableRead CreateTableRead(TableRestaurant table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return new TableRead
        {
            Id = table.Id,
            Numero = table.Numero,
            Capacite = table.Capacite,
            Disponible = table.Disponible,
            RestaurantId = table.RestaurantId
        };
    }

    /// <summary>
    /// Mappe 'AvisClientWrite' vers 'AvisClient'.
    /// </summary>
    /// <param name="source">Instance de 'AvisClientWrite'.</param>
    /// <param name="dateAvis">Date de l'avis.</param>
    /// <returns>Une nouvelle instance de 'AvisClient'.</returns>
    public static AvisClient ToAvisClient(this AvisClientWrite source, DateTime? dateAvis = null)
    {
        return new AvisClient
        {
            Note = source.Note,
            Commentaire = source.Commentaire,
            Approuve = source.Approuve,
            ClientId = source.ClientId,
            RestaurantId = source.RestaurantId,
            DateAvis = dateAvis
        };
    }

    /// <summary>
    /// Mappe 'AvisClientWrite' vers 'AvisClient'.
    /// </summary>
    /// <param name="source">Instance de 'AvisClientWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'AvisClient'.</param>
    /// <returns>L'instance pré-existante de 'AvisClient'.</returns>
    public static AvisClient ToAvisClient(this AvisClientWrite source, AvisClient dest)
    {
        dest.Note = source.Note;
        dest.Commentaire = source.Commentaire;
        dest.Approuve = source.Approuve;
        dest.ClientId = source.ClientId;
        dest.RestaurantId = source.RestaurantId;
        return dest;
    }

    /// <summary>
    /// Mappe 'ClientWrite' vers 'Client'.
    /// </summary>
    /// <param name="source">Instance de 'ClientWrite'.</param>
    /// <returns>Une nouvelle instance de 'Client'.</returns>
    public static Client ToClient(this ClientWrite source)
    {
        return new Client
        {
            Nom = source.Nom,
            Prenom = source.Prenom,
            Email = source.Email,
            AvisClients = source.AvisClients
        };
    }

    /// <summary>
    /// Mappe 'ClientWrite' vers 'Client'.
    /// </summary>
    /// <param name="source">Instance de 'ClientWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Client'.</param>
    /// <returns>L'instance pré-existante de 'Client'.</returns>
    public static Client ToClient(this ClientWrite source, Client dest)
    {
        dest.Nom = source.Nom;
        dest.Prenom = source.Prenom;
        dest.Email = source.Email;
        dest.AvisClients = source.AvisClients;
        return dest;
    }

    /// <summary>
    /// Mappe 'CommandeWrite' vers 'Commande'.
    /// </summary>
    /// <param name="source">Instance de 'CommandeWrite'.</param>
    /// <returns>Une nouvelle instance de 'Commande'.</returns>
    public static Commande ToCommande(this CommandeWrite source)
    {
        return new Commande
        {
            DateCommande = source.DateCommande,
            DateLivraison = source.DateLivraison,
            MontantTotal = source.MontantTotal,
            ClientId = source.ClientId,
            TableId = source.TableId,
            ReservationId = source.ReservationId,
            StatutCommandeCode = source.StatutCommandeCode
        };
    }

    /// <summary>
    /// Mappe 'CommandeWrite' vers 'Commande'.
    /// </summary>
    /// <param name="source">Instance de 'CommandeWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Commande'.</param>
    /// <returns>L'instance pré-existante de 'Commande'.</returns>
    public static Commande ToCommande(this CommandeWrite source, Commande dest)
    {
        dest.DateCommande = source.DateCommande;
        dest.DateLivraison = source.DateLivraison;
        dest.MontantTotal = source.MontantTotal;
        dest.ClientId = source.ClientId;
        dest.TableId = source.TableId;
        dest.ReservationId = source.ReservationId;
        dest.StatutCommandeCode = source.StatutCommandeCode;
        return dest;
    }

    /// <summary>
    /// Mappe 'EmployeWrite' vers 'Employe'.
    /// </summary>
    /// <param name="source">Instance de 'EmployeWrite'.</param>
    /// <returns>Une nouvelle instance de 'Employe'.</returns>
    public static Employe ToEmploye(this EmployeWrite source)
    {
        return new Employe
        {
            Nom = source.Nom,
            Prenom = source.Prenom,
            Telephone = source.Telephone,
            DateNaissance = source.DateNaissance,
            Matricule = source.Matricule,
            DateEmbauche = source.DateEmbauche,
            Salaire = source.Salaire,
            RestaurantId = source.RestaurantId
        };
    }

    /// <summary>
    /// Mappe 'EmployeWrite' vers 'Employe'.
    /// </summary>
    /// <param name="source">Instance de 'EmployeWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Employe'.</param>
    /// <returns>L'instance pré-existante de 'Employe'.</returns>
    public static Employe ToEmploye(this EmployeWrite source, Employe dest)
    {
        dest.Nom = source.Nom;
        dest.Prenom = source.Prenom;
        dest.Telephone = source.Telephone;
        dest.DateNaissance = source.DateNaissance;
        dest.Matricule = source.Matricule;
        dest.DateEmbauche = source.DateEmbauche;
        dest.Salaire = source.Salaire;
        dest.RestaurantId = source.RestaurantId;
        return dest;
    }

    /// <summary>
    /// Mappe 'LigneCommandeWrite' vers 'LigneCommande'.
    /// </summary>
    /// <param name="source">Instance de 'LigneCommandeWrite'.</param>
    /// <returns>Une nouvelle instance de 'LigneCommande'.</returns>
    public static LigneCommande ToLigneCommande(this LigneCommandeWrite source)
    {
        return new LigneCommande
        {
            Quantite = source.Quantite,
            PrixUnitaire = source.PrixUnitaire,
            PrixTotal = source.PrixTotal,
            CommandeId = source.CommandeId,
            PlatId = source.PlatId
        };
    }

    /// <summary>
    /// Mappe 'LigneCommandeWrite' vers 'LigneCommande'.
    /// </summary>
    /// <param name="source">Instance de 'LigneCommandeWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'LigneCommande'.</param>
    /// <returns>L'instance pré-existante de 'LigneCommande'.</returns>
    public static LigneCommande ToLigneCommande(this LigneCommandeWrite source, LigneCommande dest)
    {
        dest.Quantite = source.Quantite;
        dest.PrixUnitaire = source.PrixUnitaire;
        dest.PrixTotal = source.PrixTotal;
        dest.CommandeId = source.CommandeId;
        dest.PlatId = source.PlatId;
        return dest;
    }

    /// <summary>
    /// Mappe 'MenuWrite' vers 'Menu'.
    /// </summary>
    /// <param name="source">Instance de 'MenuWrite'.</param>
    /// <returns>Une nouvelle instance de 'Menu'.</returns>
    public static Menu ToMenu(this MenuWrite source)
    {
        return new Menu
        {
            Nom = source.Nom,
            Description = source.Description,
            Prix = source.Prix,
            Disponible = source.Disponible,
            DateDebut = source.DateDebut,
            DateFin = source.DateFin,
            RestaurantId = source.RestaurantId
        };
    }

    /// <summary>
    /// Mappe 'MenuWrite' vers 'Menu'.
    /// </summary>
    /// <param name="source">Instance de 'MenuWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Menu'.</param>
    /// <returns>L'instance pré-existante de 'Menu'.</returns>
    public static Menu ToMenu(this MenuWrite source, Menu dest)
    {
        dest.Nom = source.Nom;
        dest.Description = source.Description;
        dest.Prix = source.Prix;
        dest.Disponible = source.Disponible;
        dest.DateDebut = source.DateDebut;
        dest.DateFin = source.DateFin;
        dest.RestaurantId = source.RestaurantId;
        return dest;
    }

    /// <summary>
    /// Mappe 'PlatWrite' vers 'Plat'.
    /// </summary>
    /// <param name="source">Instance de 'PlatWrite'.</param>
    /// <returns>Une nouvelle instance de 'Plat'.</returns>
    public static Plat ToPlat(this PlatWrite source)
    {
        return new Plat
        {
            Nom = source.Nom,
            Description = source.Description,
            Prix = source.Prix,
            Disponible = source.Disponible,
            CategoriePlatCode = source.CategoriePlatCode,
            RestaurantId = source.RestaurantId
        };
    }

    /// <summary>
    /// Mappe 'PlatWrite' vers 'Plat'.
    /// </summary>
    /// <param name="source">Instance de 'PlatWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Plat'.</param>
    /// <returns>L'instance pré-existante de 'Plat'.</returns>
    public static Plat ToPlat(this PlatWrite source, Plat dest)
    {
        dest.Nom = source.Nom;
        dest.Description = source.Description;
        dest.Prix = source.Prix;
        dest.Disponible = source.Disponible;
        dest.CategoriePlatCode = source.CategoriePlatCode;
        dest.RestaurantId = source.RestaurantId;
        return dest;
    }

    /// <summary>
    /// Mappe 'PromotionWrite' vers 'Promotion'.
    /// </summary>
    /// <param name="source">Instance de 'PromotionWrite'.</param>
    /// <returns>Une nouvelle instance de 'Promotion'.</returns>
    public static Promotion ToPromotion(this PromotionWrite source)
    {
        return new Promotion
        {
            Libelle = source.Libelle,
            PourcentageReduction = source.PourcentageReduction,
            DateDebut = source.DateDebut,
            DateFin = source.DateFin,
            Active = source.Active,
            RestaurantId = source.RestaurantId
        };
    }

    /// <summary>
    /// Mappe 'PromotionWrite' vers 'Promotion'.
    /// </summary>
    /// <param name="source">Instance de 'PromotionWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Promotion'.</param>
    /// <returns>L'instance pré-existante de 'Promotion'.</returns>
    public static Promotion ToPromotion(this PromotionWrite source, Promotion dest)
    {
        dest.Libelle = source.Libelle;
        dest.PourcentageReduction = source.PourcentageReduction;
        dest.DateDebut = source.DateDebut;
        dest.DateFin = source.DateFin;
        dest.Active = source.Active;
        dest.RestaurantId = source.RestaurantId;
        return dest;
    }

    /// <summary>
    /// Mappe 'ReservationWrite' vers 'Reservation'.
    /// </summary>
    /// <param name="source">Instance de 'ReservationWrite'.</param>
    /// <returns>Une nouvelle instance de 'Reservation'.</returns>
    public static Reservation ToReservation(this ReservationWrite source)
    {
        return new Reservation
        {
            DateReservation = source.DateReservation,
            NombrePersonnes = source.NombrePersonnes,
            Commentaire = source.Commentaire,
            Confirmee = source.Confirmee,
            ClientId = source.ClientId,
            TableId = source.TableId,
            RestaurantId = source.RestaurantId
        };
    }

    /// <summary>
    /// Mappe 'ReservationWrite' vers 'Reservation'.
    /// </summary>
    /// <param name="source">Instance de 'ReservationWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Reservation'.</param>
    /// <returns>L'instance pré-existante de 'Reservation'.</returns>
    public static Reservation ToReservation(this ReservationWrite source, Reservation dest)
    {
        dest.DateReservation = source.DateReservation;
        dest.NombrePersonnes = source.NombrePersonnes;
        dest.Commentaire = source.Commentaire;
        dest.Confirmee = source.Confirmee;
        dest.ClientId = source.ClientId;
        dest.TableId = source.TableId;
        dest.RestaurantId = source.RestaurantId;
        return dest;
    }

    /// <summary>
    /// Mappe 'RestaurantWrite' vers 'Restaurant'.
    /// </summary>
    /// <param name="source">Instance de 'RestaurantWrite'.</param>
    /// <returns>Une nouvelle instance de 'Restaurant'.</returns>
    public static Restaurant ToRestaurant(this RestaurantWrite source)
    {
        return new Restaurant
        {
            Nom = source.Nom,
            Adresse = source.Adresse,
            Telephone = source.Telephone,
            Menus = source.Menus,
            Plats = source.Plats,
            Promotions = source.Promotions,
            AvisClients = source.AvisClients,
            Tables = source.Tables
        };
    }

    /// <summary>
    /// Mappe 'RestaurantWrite' vers 'Restaurant'.
    /// </summary>
    /// <param name="source">Instance de 'RestaurantWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Restaurant'.</param>
    /// <returns>L'instance pré-existante de 'Restaurant'.</returns>
    public static Restaurant ToRestaurant(this RestaurantWrite source, Restaurant dest)
    {
        dest.Nom = source.Nom;
        dest.Adresse = source.Adresse;
        dest.Telephone = source.Telephone;
        dest.Menus = source.Menus;
        dest.Plats = source.Plats;
        dest.Promotions = source.Promotions;
        dest.AvisClients = source.AvisClients;
        dest.Tables = source.Tables;
        return dest;
    }

    /// <summary>
    /// Mappe 'TableWrite' vers 'TableRestaurant'.
    /// </summary>
    /// <param name="source">Instance de 'TableWrite'.</param>
    /// <returns>Une nouvelle instance de 'TableRestaurant'.</returns>
    public static TableRestaurant ToTableRestaurant(this TableWrite source)
    {
        return new TableRestaurant
        {
            Numero = source.Numero,
            Capacite = source.Capacite,
            Disponible = source.Disponible,
            RestaurantId = source.RestaurantId
        };
    }

    /// <summary>
    /// Mappe 'TableWrite' vers 'TableRestaurant'.
    /// </summary>
    /// <param name="source">Instance de 'TableWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'TableRestaurant'.</param>
    /// <returns>L'instance pré-existante de 'TableRestaurant'.</returns>
    public static TableRestaurant ToTableRestaurant(this TableWrite source, TableRestaurant dest)
    {
        dest.Numero = source.Numero;
        dest.Capacite = source.Capacite;
        dest.Disponible = source.Disponible;
        dest.RestaurantId = source.RestaurantId;
        return dest;
    }
}
