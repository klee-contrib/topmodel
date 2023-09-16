////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {coreFetch} from "@focus4/core";

import {TypeUtilisateurCode} from "../../../model/securite/utilisateur/references";
import {UtilisateurItem} from "../../../model/securite/utilisateur/utilisateur-item";
import {UtilisateurRead} from "../../../model/securite/utilisateur/utilisateur-read";
import {UtilisateurWrite} from "../../../model/securite/utilisateur/utilisateur-write";

/**
 * Ajoute un utilisateur
 * @param utilisateur Utilisateur à sauvegarder
 * @param options Options pour 'fetch'.
 * @returns Utilisateur sauvegardé
 */
export function addUtilisateur(utilisateur: UtilisateurWrite, options: RequestInit = {}): Promise<UtilisateurRead> {
    return coreFetch("POST", `./utilisateur`, {body: utilisateur}, options);
}

/**
 * Supprime un utilisateur
 * @param utiId Id de l'utilisateur
 * @param options Options pour 'fetch'.
 */
export function deleteUtilisateur(utiId: number, options: RequestInit = {}): Promise<void> {
    return coreFetch("DELETE", `./utilisateur/${utiId}`, {}, options);
}

/**
 * Charge le détail d'un utilisateur
 * @param utiId Id de l'utilisateur
 * @param options Options pour 'fetch'.
 * @returns Le détail de l'utilisateur
 */
export function getUtilisateur(utiId: number, options: RequestInit = {}): Promise<UtilisateurRead> {
    return coreFetch("GET", `./utilisateur/${utiId}`, {}, options);
}

/**
 * Recherche des utilisateurs
 * @param nom Nom de l'utilisateur
 * @param prenom Nom de l'utilisateur
 * @param email Email de l'utilisateur
 * @param dateNaissance Age de l'utilisateur
 * @param actif Si l'utilisateur est actif
 * @param profilId Profil de l'utilisateur
 * @param typeUtilisateurCode Type d'utilisateur
 * @param options Options pour 'fetch'.
 * @returns Utilisateurs matchant les critères
 */
export function searchUtilisateur(nom?: string, prenom?: string, email?: string, dateNaissance?: string, actif?: boolean, profilId?: number, typeUtilisateurCode?: TypeUtilisateurCode, options: RequestInit = {}): Promise<UtilisateurItem[]> {
    return coreFetch("GET", `./utilisateur`, {query: {nom, prenom, email, dateNaissance, actif, profilId, typeUtilisateurCode}}, options);
}

/**
 * Sauvegarde un utilisateur
 * @param utiId Id de l'utilisateur
 * @param utilisateur Utilisateur à sauvegarder
 * @param options Options pour 'fetch'.
 * @returns Utilisateur sauvegardé
 */
export function updateUtilisateur(utiId: number, utilisateur: UtilisateurWrite, options: RequestInit = {}): Promise<UtilisateurRead> {
    return coreFetch("PUT", `./utilisateur/${utiId}`, {body: utilisateur}, options);
}
