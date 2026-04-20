////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {CommandeItem} from "../../model/restaurant/commande-item";
import {CommandeRead} from "../../model/restaurant/commande-read";
import {CommandeWrite} from "../../model/restaurant/commande-write";
import {StatutCommande} from "../../model/restaurant/enums";
import {ReservationRead} from "../../model/restaurant/reservation-read";
import {ReservationWrite} from "../../model/restaurant/reservation-write";

/**
 * Crée une nouvelle commande
 * @param commande Commande à créer
 * @param options Options pour 'fetch'.
 * @returns Commande créée
 */
export async function addCommande(commande: CommandeWrite, options: RequestInit = {}): Promise<CommandeRead> {
    const response = await fetch(`./api/restaurants/commandes`, {
        ...options,
        method: "POST",
        body: JSON.stringify(commande),
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
export async function createReservation(reservation: ReservationWrite, options: RequestInit = {}): Promise<ReservationRead> {
    const response = await fetch(`./api/restaurants/reservations`, {
        ...options,
        method: "POST",
        body: JSON.stringify(reservation),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Supprime une commande
 * @param comId Identifiant de la commande
 * @param options Options pour 'fetch'.
 */
export async function deleteCommande(comId: number, options: RequestInit = {}): Promise<void> {
    await fetch(`./api/restaurants/commandes/${comId}`, {
        ...options,
        method: "DELETE"
    });
}

/**
 * Supprime une commande
 * @param commandeItem Commande item à supprimer dans le body
 * @param options Options pour 'fetch'.
 */
export async function deleteCommandeWithBody(commandeItem: CommandeItem, options: RequestInit = {}): Promise<void> {
    await fetch(`./api/restaurants/commandes`, {
        ...options,
        method: "DELETE",
        body: JSON.stringify(commandeItem),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
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
 * Charge le détail d'une commande
 * @param comId Identifiant de la commande
 * @param options Options pour 'fetch'.
 * @returns Détail de la commande
 */
export async function getCommande(comId: number, options: RequestInit = {}): Promise<CommandeRead> {
    const response = await fetch(`./api/restaurants/commandes/${comId}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste toutes les commandes
 * @param clientId Client ayant passé la commande
 * @param statutCommande Statut de la commande
 * @param tableId Table associée à la commande
 * @param options Options pour 'fetch'.
 * @returns Liste des commandes
 */
export async function getCommandes(clientId?: number, statutCommande: StatutCommande = "EN_ATT", tableId?: number, options: RequestInit = {}): Promise<CommandeItem[]> {
    const query = new URLSearchParams();
    if (clientId !== undefined) {
        query.append("clientId", `${clientId}`)
    }
    if (statutCommande !== undefined) {
        query.append("statutCommande", statutCommande)
    }
    if (tableId !== undefined) {
        query.append("tableId", `${tableId}`)
    }
    const response = await fetch(`./api/restaurants/commandes?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Récupère les commandes par date
 * @param dateCommande Date et heure de la commande
 * @param options Options pour 'fetch'.
 * @returns Commandes pour la date spécifiée
 */
export async function getCommandesByDate(dateCommande?: string, options: RequestInit = {}): Promise<CommandeItem[]> {
    const query = new URLSearchParams();
    if (dateCommande !== undefined) {
        query.append("dateCommande", dateCommande)
    }
    const response = await fetch(`./api/restaurants/commandes/by-date?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste tous les statuts de commande
 * @param options Options pour 'fetch'.
 * @returns Liste des statuts de commande
 */
export async function getStatutCommandes(options: RequestInit = {}): Promise<StatutCommande[]> {
    const response = await fetch(`./api/restaurants/statuts-commande`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Met à jour partiellement une commande
 * @param comId Identifiant de la commande
 * @param commande Données partielles de la commande
 * @param options Options pour 'fetch'.
 * @returns Commande mise à jour
 */
export async function patchCommande(comId: number, commande: CommandeWrite, options: RequestInit = {}): Promise<CommandeRead> {
    const response = await fetch(`./api/restaurants/commandes/${comId}`, {
        ...options,
        method: "PATCH",
        body: JSON.stringify(commande),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Met à jour une commande
 * @param comId Identifiant de la commande
 * @param commande Commande à mettre à jour
 * @param options Options pour 'fetch'.
 * @returns Commande mise à jour
 */
export async function updateCommande(comId: number, commande: CommandeWrite, options: RequestInit = {}): Promise<CommandeRead> {
    const response = await fetch(`./api/restaurants/commandes/${comId}`, {
        ...options,
        method: "PUT",
        body: JSON.stringify(commande),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Met à jour uniquement le statut d'une commande
 * @param comId Identifiant de la commande
 * @param statutCommande Statut de la commande
 * @param options Options pour 'fetch'.
 * @returns Commande avec le statut mis à jour
 */
export async function updateCommandeStatut(comId: number, statutCommande: StatutCommande = "EN_ATT", options: RequestInit = {}): Promise<CommandeRead> {
    const query = new URLSearchParams();
    if (statutCommande !== undefined) {
        query.append("statutCommande", statutCommande)
    }
    const response = await fetch(`./api/restaurants/commandes/${comId}/statut?${query}`, {
        ...options,
        method: "PATCH"
    });
    return await response.json();
}
