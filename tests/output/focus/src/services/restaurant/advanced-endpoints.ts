////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {AvisClientRead} from "../../model/restaurant/avis-client-read";
import {ClientAvecCommandes} from "../../model/restaurant/client-avec-commandes";
import {CommandeDetailRead} from "../../model/restaurant/commande-detail-read";
import {EmployeRead} from "../../model/restaurant/employe-read";
import {EmployeWrite} from "../../model/restaurant/employe-write";
import {MenuComplet} from "../../model/restaurant/menu-complet";
import {MenuWrite} from "../../model/restaurant/menu-write";
import {PromotionRead} from "../../model/restaurant/promotion-read";
import {PromotionWrite} from "../../model/restaurant/promotion-write";
import {ReservationAvecDetails} from "../../model/restaurant/reservation-avec-details";
import {ReservationWrite} from "../../model/restaurant/reservation-write";
import {RestaurantAvecStatistiques} from "../../model/restaurant/restaurant-avec-statistiques";
import {StatistiquesRestaurant} from "../../model/restaurant/statistiques-restaurant";

/**
 * Ajoute un employé (nécessite le rôle ADMIN)
 * @param employe Employé à créer
 * @param options Options pour 'fetch'.
 * @returns Employé créé
 */
export async function addEmploye(employe: EmployeWrite, options: RequestInit = {}): Promise<EmployeRead> {
    const response = await fetch(`./api/restaurants/employes`, {
        ...options,
        method: "POST",
        body: JSON.stringify(employe),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Crée un menu avec ses plats
 * @param menu Menu à créer
 * @param options Options pour 'fetch'.
 * @returns Menu créé avec ses plats
 */
export async function createMenu(menu: MenuWrite, options: RequestInit = {}): Promise<MenuComplet> {
    const response = await fetch(`./api/restaurants/menus`, {
        ...options,
        method: "POST",
        body: JSON.stringify(menu),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Crée une réservation
 * @param reservation Réservation à créer
 * @param options Options pour 'fetch'.
 * @returns Réservation créée
 */
export async function createReservation(reservation: ReservationWrite, options: RequestInit = {}): Promise<ReservationAvecDetails> {
    const response = await fetch(`./api/restaurants/reservations`, {
        ...options,
        method: "POST",
        body: JSON.stringify(reservation),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Exporte les commandes au format CSV
 * @param dateDebut Date et heure de la commande
 * @param dateFin Date et heure de la commande
 * @param options Options pour 'fetch'.
 * @returns Fichier CSV des commandes
 */
export async function exportCommandes(dateDebut?: string, dateFin?: string, options: RequestInit = {}): Promise<Blob | undefined> {
    const query = new URLSearchParams();
    if (dateDebut !== undefined) {
        query.append("dateDebut", dateDebut)
    }
    if (dateFin !== undefined) {
        query.append("dateFin", dateFin)
    }
    const response = await fetch(`./api/restaurants/commandes/export?${query}`, {
        ...options,
        method: "GET"
    });
    if (response.status === 204) {
        return undefined;
    }
    return await response.blob();
}

/**
 * Liste les avis clients avec filtres
 * @param resRestaurantId Identifiant du restaurant
 * @param noteMin Note sur 5
 * @param approuve Indique si l'avis est approuvé par le restaurant
 * @param dateDebut Date de l'avis
 * @param dateFin Date de l'avis
 * @param options Options pour 'fetch'.
 * @returns Liste des avis correspondant aux critères
 */
export async function getAvisClients(resRestaurantId?: number, noteMin?: number, approuve: boolean = false, dateDebut?: string, dateFin?: string, options: RequestInit = {}): Promise<AvisClientRead[]> {
    const query = new URLSearchParams();
    if (resRestaurantId !== undefined) {
        query.append("resRestaurantId", `${resRestaurantId}`)
    }
    if (noteMin !== undefined) {
        query.append("noteMin", `${noteMin}`)
    }
    if (approuve !== undefined) {
        query.append("approuve", `${approuve}`)
    }
    if (dateDebut !== undefined) {
        query.append("dateDebut", dateDebut)
    }
    if (dateFin !== undefined) {
        query.append("dateFin", dateFin)
    }
    const response = await fetch(`./api/restaurants/avis?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Récupère un client avec toutes ses commandes
 * @param cliId Identifiant du client
 * @param options Options pour 'fetch'.
 * @returns Client avec ses commandes
 */
export async function getClientAvecCommandes(cliId: number, options: RequestInit = {}): Promise<ClientAvecCommandes> {
    const response = await fetch(`./api/restaurants/clients/${cliId}/avec-commandes`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Récupère le détail complet d'une commande avec ses lignes
 * @param comId Identifiant de la commande
 * @param options Options pour 'fetch'.
 * @returns Détail complet de la commande
 */
export async function getCommandeDetail(comId: number, options: RequestInit = {}): Promise<CommandeDetailRead> {
    const response = await fetch(`./api/restaurants/commandes/${comId}/detail`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Récupère un menu spécifique d'un restaurant
 * @param resId Identifiant du restaurant
 * @param menId Identifiant du menu
 * @param options Options pour 'fetch'.
 * @returns Menu du restaurant
 */
export async function getRestaurantMenu(resId: number, menId: number, options: RequestInit = {}): Promise<MenuComplet> {
    const response = await fetch(`./api/restaurants/${resId}/menus/${menId}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Récupère les statistiques d'un restaurant
 * @param resId Identifiant du restaurant
 * @param dateDebut Date de début pour le calcul des statistiques
 * @param dateFin Date de fin pour le calcul des statistiques
 * @param options Options pour 'fetch'.
 * @returns Statistiques du restaurant
 */
export async function getRestaurantStatistiques(resId: number, dateDebut?: string, dateFin?: string, options: RequestInit = {}): Promise<StatistiquesRestaurant> {
    const query = new URLSearchParams();
    if (dateDebut !== undefined) {
        query.append("dateDebut", dateDebut)
    }
    if (dateFin !== undefined) {
        query.append("dateFin", dateFin)
    }
    const response = await fetch(`./api/restaurants/${resId}/statistiques?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Met à jour partiellement une promotion
 * @param proId Identifiant de la promotion
 * @param promotion Données partielles de la promotion
 * @param options Options pour 'fetch'.
 * @returns Promotion mise à jour
 */
export async function patchPromotion(proId: number, promotion: PromotionWrite, options: RequestInit = {}): Promise<PromotionRead> {
    const response = await fetch(`./api/restaurants/promotions/${proId}`, {
        ...options,
        method: "PATCH",
        body: JSON.stringify(promotion),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Recherche avancée de restaurants
 * @param nom Nom du restaurant (recherche partielle)
 * @param adresse Adresse du restaurant (recherche partielle)
 * @param noteMin Note minimum requise
 * @param options Options pour 'fetch'.
 * @returns Liste des restaurants correspondant aux critères
 */
export async function searchRestaurants(nom?: string, adresse?: string, noteMin?: number, options: RequestInit = {}): Promise<RestaurantAvecStatistiques[]> {
    const query = new URLSearchParams();
    if (nom !== undefined) {
        query.append("nom", nom)
    }
    if (adresse !== undefined) {
        query.append("adresse", adresse)
    }
    if (noteMin !== undefined) {
        query.append("noteMin", `${noteMin}`)
    }
    const response = await fetch(`./api/restaurants/search?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}
