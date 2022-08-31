////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {fetch} from "@api-services";

import {Page} from "@/types";
import {TypeUtilisateurCode} from "../../../model/utilisateur/references";
import {UtilisateurDto} from "../../../model/utilisateur/utilisateur-dto";

/**
 * Charge le détail d'un utilisateur
 * @param utiId Id technique
 * @param options Options pour 'fetch'.
 * @returns Le détail de l'utilisateur
 */
export function getUtilisateur(utiId: number, options: RequestInit = {}): Promise<UtilisateurDto> {
    return fetch("GET", `./utilisateur/${utiId}`, {}, options);
}

/**
 * Charge une liste d'utilisateurs par leur type
 * @param typeUtilisateurCode Type d'utilisateur en Many to one
 * @param options Options pour 'fetch'.
 * @returns Liste des utilisateurs
 */
export function getUtilisateurList(typeUtilisateurCode?: TypeUtilisateurCode, options: RequestInit = {}): Promise<UtilisateurDto[]> {
    return fetch("GET", `./utilisateur/list`, {query: {typeUtilisateurCode}}, options);
}

/**
 * Sauvegarde un utilisateur
 * @param utilisateur Utilisateur à sauvegarder
 * @param options Options pour 'fetch'.
 * @returns Utilisateur sauvegardé
 */
export function saveUtilisateur(utilisateur: UtilisateurDto, options: RequestInit = {}): Promise<UtilisateurDto> {
    return fetch("POST", `./utilisateur/save`, {body: utilisateur}, options);
}

/**
 * Sauvegarde une liste d'utilisateurs
 * @param utilisateur Utilisateur à sauvegarder
 * @param options Options pour 'fetch'.
 * @returns Utilisateur sauvegardé
 */
export function saveAllUtilisateur(utilisateur: UtilisateurDto[], options: RequestInit = {}): Promise<UtilisateurDto[]> {
    return fetch("POST", `./utilisateur/saveAll`, {body: utilisateur}, options);
}

/**
 * Recherche des utilisateurs
 * @param utiUtilisateurId Id technique
 * @param utilisateuremail Email de l'utilisateur
 * @param utilisateurTypeUtilisateurCode Type d'utilisateur en Many to one
 * @param options Options pour 'fetch'.
 * @returns Utilisateurs matchant les critères
 */
export function search(utiUtilisateurId?: number, utilisateuremail?: string, utilisateurTypeUtilisateurCode?: TypeUtilisateurCode, options: RequestInit = {}): Promise<Page<UtilisateurDto>> {
    return fetch("POST", `./utilisateur/search`, {query: {utiUtilisateurId, utilisateuremail, utilisateurTypeUtilisateurCode}}, options);
}
