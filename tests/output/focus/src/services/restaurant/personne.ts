////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {AvisClientRead} from "../../model/restaurant/avis-client-read";
import {ClientAvecCommandes} from "../../model/restaurant/client-avec-commandes";
import {ClientItem} from "../../model/restaurant/client-item";
import {ClientRead} from "../../model/restaurant/client-read";
import {ClientWrite} from "../../model/restaurant/client-write";
import {CommandeItem} from "../../model/restaurant/commande-item";
import {EmployeRead} from "../../model/restaurant/employe-read";
import {EmployeWrite} from "../../model/restaurant/employe-write";

/**
 * Ajoute un client
 * @param client Client à créer
 * @param options Options pour 'fetch'.
 * @returns Client créé
 */
export async function addClient(client: ClientWrite, options: RequestInit = {}): Promise<ClientRead> {
    const response = await fetch(`./api/restaurants/clients`, {
        ...options,
        method: "POST",
        body: JSON.stringify(client),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

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
 * Supprime un client
 * @param perId Identifiant de la personne
 * @param options Options pour 'fetch'.
 */
export async function deleteClient(perId: number, options: RequestInit = {}): Promise<void> {
    await fetch(`./api/restaurants/clients/${perId}`, {
        ...options,
        method: "DELETE"
    });
}

/**
 * Liste les avis clients avec filtres
 * @param resId Identifiant du restaurant
 * @param noteMin Note sur 5
 * @param approuve Indique si l'avis est approuvé par le restaurant
 * @param dateDebut Date de l'avis
 * @param dateFin Date de l'avis
 * @param options Options pour 'fetch'.
 * @returns Liste des avis correspondant aux critères
 */
export async function getAvisClients(resId: number, noteMin?: number, approuve: boolean = false, dateDebut?: string, dateFin?: string, options: RequestInit = {}): Promise<AvisClientRead[]> {
    const query = new URLSearchParams();
    if (resId !== undefined) {
        query.append("resId", `${resId}`)
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
 * Charge le détail d'un client
 * @param perId Identifiant de la personne
 * @param options Options pour 'fetch'.
 * @returns Détail du client
 */
export async function getClient(perId: number, options: RequestInit = {}): Promise<ClientRead> {
    const response = await fetch(`./api/restaurants/clients/${perId}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Récupère un client avec toutes ses commandes
 * @param perId Identifiant de la personne
 * @param options Options pour 'fetch'.
 * @returns Client avec ses commandes
 */
export async function getClientAvecCommandes(perId: number, options: RequestInit = {}): Promise<ClientAvecCommandes> {
    const response = await fetch(`./api/restaurants/clients/${perId}/avec-commandes`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste les commandes d'un client
 * @param perId Identifiant de la personne
 * @param options Options pour 'fetch'.
 * @returns Liste des commandes du client
 */
export async function getClientCommandes(perId: number, options: RequestInit = {}): Promise<CommandeItem[]> {
    const response = await fetch(`./api/restaurants/clients/${perId}/commandes`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Liste tous les clients
 * @param nom Nom de la personne
 * @param email Adresse email du client
 * @param options Options pour 'fetch'.
 * @returns Liste des clients
 */
export async function getClients(nom: string, email?: string, options: RequestInit = {}): Promise<ClientItem[]> {
    const query = new URLSearchParams();
    if (nom !== undefined) {
        query.append("nom", nom)
    }
    if (email !== undefined) {
        query.append("email", email)
    }
    const response = await fetch(`./api/restaurants/clients?${query}`, {
        ...options,
        method: "GET"
    });
    return await response.json();
}

/**
 * Met à jour partiellement un client
 * @param perId Identifiant de la personne
 * @param client Données partielles du client
 * @param options Options pour 'fetch'.
 * @returns Client mis à jour
 */
export async function patchClient(perId: number, client: ClientWrite, options: RequestInit = {}): Promise<ClientRead> {
    const response = await fetch(`./api/restaurants/clients/${perId}`, {
        ...options,
        method: "PATCH",
        body: JSON.stringify(client),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}

/**
 * Met à jour un client
 * @param perId Identifiant de la personne
 * @param client Client à mettre à jour
 * @param options Options pour 'fetch'.
 * @returns Client mis à jour
 */
export async function updateClient(perId: number, client: ClientWrite, options: RequestInit = {}): Promise<ClientRead> {
    const response = await fetch(`./api/restaurants/clients/${perId}`, {
        ...options,
        method: "PUT",
        body: JSON.stringify(client),
        headers: {...options.headers, "Content-Type": "application/json"}
    });
    return await response.json();
}
