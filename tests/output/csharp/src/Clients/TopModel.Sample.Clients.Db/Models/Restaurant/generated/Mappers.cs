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
            ClientIdClient = avisClient.ClientIdClient,
            RestaurantIdRestaurant = avisClient.RestaurantIdRestaurant
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'ClientMinimal'.
    /// </summary>
    /// <param name="client">Instance de 'Client'.</param>
    /// <returns>Une nouvelle instance de 'ClientMinimal'.</returns>
    public static ClientMinimal CreateClientMinimal(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);

        return new ClientMinimal
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
            Telephone = client.Telephone,
            Email = client.Email,
            Commandes = client.Commandes,
            AvisClientsClient = client.AvisClientsClient,
            ReservationsClient = client.ReservationsClient
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
            Id = commande.Id,
            DateCommande = commande.DateCommande,
            DateLivraison = commande.DateLivraison,
            MontantTotal = commande.MontantTotal,
            ClientId = commande.ClientId,
            TableClientId = commande.TableClientId,
            StatutCommandeCode = commande.StatutCommandeCode,
            LigneCommandes = commande.LigneCommandes
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
            Matricule = employe.Matricule,
            DateEmbauche = employe.DateEmbauche,
            Salaire = employe.Salaire,
            RestaurantIdRestaurant = employe.RestaurantIdRestaurant
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
            RestaurantIdRestaurant = menu.RestaurantIdRestaurant,
            MenuPlatsMenu = menu.MenuPlatsMenu
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'PlatAvecDetails'.
    /// </summary>
    /// <param name="plat">Instance de 'Plat'.</param>
    /// <returns>Une nouvelle instance de 'PlatAvecDetails'.</returns>
    public static PlatAvecDetails CreatePlatAvecDetails(Plat plat)
    {
        ArgumentNullException.ThrowIfNull(plat);

        return new PlatAvecDetails
        {
            Id = plat.Id,
            Nom = plat.Nom,
            Description = plat.Description,
            Prix = plat.Prix,
            Disponible = plat.Disponible,
            CategoriePlatCodeCategoriePlat = plat.CategoriePlatCodeCategoriePlat,
            RestaurantIdRestaurant = plat.RestaurantIdRestaurant,
            LigneCommandes = plat.LigneCommandes,
            MenuPlatsPlat = plat.MenuPlatsPlat,
            PromotionPlatsPlat = plat.PromotionPlatsPlat
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
            CategoriePlatCodeCategoriePlat = plat.CategoriePlatCodeCategoriePlat,
            RestaurantIdRestaurant = plat.RestaurantIdRestaurant,
            LigneCommandes = plat.LigneCommandes,
            MenuPlatsPlat = plat.MenuPlatsPlat,
            PromotionPlatsPlat = plat.PromotionPlatsPlat
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
            RestaurantIdRestaurant = promotion.RestaurantIdRestaurant,
            PromotionPlatsPromotion = promotion.PromotionPlatsPromotion
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
            ClientIdClient = reservation.ClientIdClient,
            TableClientIdTable = reservation.TableClientIdTable,
            RestaurantIdRestaurant = reservation.RestaurantIdRestaurant
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
            TableClientsRestaurant = restaurant.TableClientsRestaurant,
            PlatsRestaurant = restaurant.PlatsRestaurant,
            AvisClientsRestaurant = restaurant.AvisClientsRestaurant,
            MenusRestaurant = restaurant.MenusRestaurant,
            ReservationsRestaurant = restaurant.ReservationsRestaurant,
            PromotionsRestaurant = restaurant.PromotionsRestaurant,
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
            TableClientsRestaurant = restaurant.TableClientsRestaurant,
            PlatsRestaurant = restaurant.PlatsRestaurant,
            AvisClientsRestaurant = restaurant.AvisClientsRestaurant,
            MenusRestaurant = restaurant.MenusRestaurant,
            ReservationsRestaurant = restaurant.ReservationsRestaurant,
            PromotionsRestaurant = restaurant.PromotionsRestaurant
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
    /// Crée une nouvelle instance de 'TableClientRead'.
    /// </summary>
    /// <param name="tableClient">Instance de 'TableClient'.</param>
    /// <returns>Une nouvelle instance de 'TableClientRead'.</returns>
    public static TableClientRead CreateTableClientRead(TableClient tableClient)
    {
        ArgumentNullException.ThrowIfNull(tableClient);

        return new TableClientRead
        {
            Id = tableClient.Id,
            Numero = tableClient.Numero,
            Capacite = tableClient.Capacite,
            Disponible = tableClient.Disponible,
            RestaurantIdRestaurant = tableClient.RestaurantIdRestaurant,
            Commandes = tableClient.Commandes,
            ReservationsTable = tableClient.ReservationsTable
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
            ClientIdClient = source.ClientIdClient,
            RestaurantIdRestaurant = source.RestaurantIdRestaurant,
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
        dest.ClientIdClient = source.ClientIdClient;
        dest.RestaurantIdRestaurant = source.RestaurantIdRestaurant;
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
            Telephone = source.Telephone,
            Email = source.Email,
            Commandes = source.Commandes,
            AvisClientsClient = source.AvisClientsClient,
            ReservationsClient = source.ReservationsClient
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
        dest.Telephone = source.Telephone;
        dest.Email = source.Email;
        dest.Commandes = source.Commandes;
        dest.AvisClientsClient = source.AvisClientsClient;
        dest.ReservationsClient = source.ReservationsClient;
        return dest;
    }

    /// <summary>
    /// Mappe 'CommandeWrite' vers 'Commande'.
    /// </summary>
    /// <param name="source">Instance de 'CommandeWrite'.</param>
    /// <param name="dateCommande">Date et heure de la commande.</param>
    /// <param name="montantTotal">Montant total de la commande.</param>
    /// <returns>Une nouvelle instance de 'Commande'.</returns>
    public static Commande ToCommande(this CommandeWrite source, DateTime? dateCommande = null, decimal? montantTotal = null)
    {
        return new Commande
        {
            DateLivraison = source.DateLivraison,
            ClientId = source.ClientId,
            TableClientId = source.TableClientId,
            StatutCommandeCode = source.StatutCommandeCode,
            LigneCommandes = source.LigneCommandes,
            DateCommande = dateCommande,
            MontantTotal = montantTotal
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
        dest.DateLivraison = source.DateLivraison;
        dest.ClientId = source.ClientId;
        dest.TableClientId = source.TableClientId;
        dest.StatutCommandeCode = source.StatutCommandeCode;
        dest.LigneCommandes = source.LigneCommandes;
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
            Matricule = source.Matricule,
            DateEmbauche = source.DateEmbauche,
            Salaire = source.Salaire,
            RestaurantIdRestaurant = source.RestaurantIdRestaurant
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
        dest.Matricule = source.Matricule;
        dest.DateEmbauche = source.DateEmbauche;
        dest.Salaire = source.Salaire;
        dest.RestaurantIdRestaurant = source.RestaurantIdRestaurant;
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
            RestaurantIdRestaurant = source.RestaurantIdRestaurant,
            MenuPlatsMenu = source.MenuPlatsMenu
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
        dest.RestaurantIdRestaurant = source.RestaurantIdRestaurant;
        dest.MenuPlatsMenu = source.MenuPlatsMenu;
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
            CategoriePlatCodeCategoriePlat = source.CategoriePlatCodeCategoriePlat,
            RestaurantIdRestaurant = source.RestaurantIdRestaurant,
            LigneCommandes = source.LigneCommandes,
            MenuPlatsPlat = source.MenuPlatsPlat,
            PromotionPlatsPlat = source.PromotionPlatsPlat
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
        dest.CategoriePlatCodeCategoriePlat = source.CategoriePlatCodeCategoriePlat;
        dest.RestaurantIdRestaurant = source.RestaurantIdRestaurant;
        dest.LigneCommandes = source.LigneCommandes;
        dest.MenuPlatsPlat = source.MenuPlatsPlat;
        dest.PromotionPlatsPlat = source.PromotionPlatsPlat;
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
            RestaurantIdRestaurant = source.RestaurantIdRestaurant,
            PromotionPlatsPromotion = source.PromotionPlatsPromotion
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
        dest.RestaurantIdRestaurant = source.RestaurantIdRestaurant;
        dest.PromotionPlatsPromotion = source.PromotionPlatsPromotion;
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
            ClientIdClient = source.ClientIdClient,
            TableClientIdTable = source.TableClientIdTable,
            RestaurantIdRestaurant = source.RestaurantIdRestaurant
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
        dest.ClientIdClient = source.ClientIdClient;
        dest.TableClientIdTable = source.TableClientIdTable;
        dest.RestaurantIdRestaurant = source.RestaurantIdRestaurant;
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
            TableClientsRestaurant = source.TableClientsRestaurant,
            PlatsRestaurant = source.PlatsRestaurant,
            AvisClientsRestaurant = source.AvisClientsRestaurant,
            MenusRestaurant = source.MenusRestaurant,
            ReservationsRestaurant = source.ReservationsRestaurant,
            PromotionsRestaurant = source.PromotionsRestaurant
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
        dest.TableClientsRestaurant = source.TableClientsRestaurant;
        dest.PlatsRestaurant = source.PlatsRestaurant;
        dest.AvisClientsRestaurant = source.AvisClientsRestaurant;
        dest.MenusRestaurant = source.MenusRestaurant;
        dest.ReservationsRestaurant = source.ReservationsRestaurant;
        dest.PromotionsRestaurant = source.PromotionsRestaurant;
        return dest;
    }

    /// <summary>
    /// Mappe 'TableClientWrite' vers 'TableClient'.
    /// </summary>
    /// <param name="source">Instance de 'TableClientWrite'.</param>
    /// <returns>Une nouvelle instance de 'TableClient'.</returns>
    public static TableClient ToTableClient(this TableClientWrite source)
    {
        return new TableClient
        {
            Numero = source.Numero,
            Capacite = source.Capacite,
            Disponible = source.Disponible,
            RestaurantIdRestaurant = source.RestaurantIdRestaurant,
            Commandes = source.Commandes,
            ReservationsTable = source.ReservationsTable
        };
    }

    /// <summary>
    /// Mappe 'TableClientWrite' vers 'TableClient'.
    /// </summary>
    /// <param name="source">Instance de 'TableClientWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'TableClient'.</param>
    /// <returns>L'instance pré-existante de 'TableClient'.</returns>
    public static TableClient ToTableClient(this TableClientWrite source, TableClient dest)
    {
        dest.Numero = source.Numero;
        dest.Capacite = source.Capacite;
        dest.Disponible = source.Disponible;
        dest.RestaurantIdRestaurant = source.RestaurantIdRestaurant;
        dest.Commandes = source.Commandes;
        dest.ReservationsTable = source.ReservationsTable;
        return dest;
    }
}
