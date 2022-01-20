////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import {fetch} from "@api-services";

import {Page} from "@/types";
import {TypeProfilCode} from "../../../model/securite/references";
import {TypeUtilisateurCode} from "../../../model/utilisateur/references";
import {UtilisateurDto} from "../../../model/utilisateur/utilisateur-dto";

/**
 * Charge le détail d'un utilisateur
 * @param utilisateurId Id technique
 * @param options Options pour 'fetch'.
 * @returns Le détail de l'utilisateur
 */
export function getUtilisateur(utilisateurId: number, options: RequestInit = {}): Promise<UtilisateurDto> {
    return fetch("GET", `./utilisateur/${utilisateurId}`, {}, options);
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
 * @param utilisateurId Id technique
 * @param email Email de l'utilisateur
 * @param typeUtilisateurCode Type d'utilisateur en Many to one
 * @param typeUtilisateurCodeOneToOneType Type d'utilisateur en one to one
 * @param profilProfilId Id technique
 * @param profilTypeProfilCode Type de profil
 * @param options Options pour 'fetch'.
 * @returns Utilisateurs matchant les critères
 */
export function search(utilisateurId?: number, email?: string, typeUtilisateurCode?: TypeUtilisateurCode, typeUtilisateurCodeOneToOneType?: TypeUtilisateurCode, profilProfilId?: number, profilTypeProfilCode?: TypeProfilCode, options: RequestInit = {}): Promise<Page<UtilisateurDto>> {
    return fetch("POST", `./utilisateur/search`, {query: {utilisateurId, email, typeUtilisateurCode, typeUtilisateurCodeOneToOneType, profilProfilId, profilTypeProfilCode}}, options);
}
