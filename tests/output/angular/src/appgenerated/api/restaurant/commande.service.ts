////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////


import { HttpClient, HttpContext, HttpHeaders, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CommandeDetailRead } from "../../model/restaurant/commande-detail-read";
import { CommandeItem } from "../../model/restaurant/commande-item";
import { CommandeRead } from "../../model/restaurant/commande-read";
import { CommandeWrite } from "../../model/restaurant/commande-write";
import { LigneCommandeItem } from "../../model/restaurant/ligne-commande-item";
import { LigneCommandeRead } from "../../model/restaurant/ligne-commande-read";
import { LigneCommandeWrite } from "../../model/restaurant/ligne-commande-write";
import { StatutCommande, StatutCommandeCode } from "../../model/restaurant/references";
import { ReservationAvecDetails } from "../../model/restaurant/reservation-avec-details";
import { ReservationWrite } from "../../model/restaurant/reservation-write";
@Injectable({
    providedIn: 'root'
})
export class CommandeService {

    private readonly http = inject(HttpClient);

    /**
     * @description Crée une nouvelle commande
     * @param commande Commande à créer
     * @returns Commande créée
     */
    addCommande(commande: CommandeWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<CommandeRead> {
        return this.http.post<CommandeRead>(`/api/restaurants/commandes`, commande, {observe: 'body', ...options});
    }

    /**
     * @description Ajoute une ligne de commande
     * @param ligneCommande Ligne de commande à créer
     * @returns Ligne de commande créée
     */
    addLigneCommande(ligneCommande: LigneCommandeWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<LigneCommandeRead> {
        return this.http.post<LigneCommandeRead>(`/api/restaurants/ligne-commandes`, ligneCommande, {observe: 'body', ...options});
    }

    /**
     * @description Crée une réservation
     * @param reservation Réservation à créer
     * @returns Réservation créée
     */
    createReservation(reservation: ReservationWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<ReservationAvecDetails> {
        return this.http.post<ReservationAvecDetails>(`/api/restaurants/reservations`, reservation, {observe: 'body', ...options});
    }

    /**
     * @description Supprime une commande
     * @param comId Identifiant de la commande
     */
    deleteCommande(comId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<void> {
        return this.http.delete<void>(`/api/restaurants/commandes/${comId}`, {observe: 'body', ...options});
    }

    /**
     * @description Supprime une ligne de commande
     * @param ligId Identifiant de la ligne
     */
    deleteLigneCommande(ligId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<void> {
        return this.http.delete<void>(`/api/restaurants/ligne-commandes/${ligId}`, {observe: 'body', ...options});
    }

    /**
     * @description Exporte les commandes au format CSV
     * @param dateDebut Date et heure de la commande
     * @param dateFin Date et heure de la commande
     * @returns Fichier CSV des commandes
     */
    exportCommandes(dateDebut?: string, dateFin?: string, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<Blob> {
        const addParam = (key: string, value: any) => {
          if (value !== null && value !== undefined) {
            if (options.params instanceof HttpParams) {
              options.params = options.params.append(key, value);
            } else {
              if (!options.params) {
                options.params = {};
              }
              options.params[key] = value;
            }
          }
        };
        addParam('dateDebut', dateDebut);
        addParam('dateFin', dateFin);

        return this.http.get(`/api/restaurants/commandes/export`, {responseType: "blob", observe: 'body', ...options});
    }

    /**
     * @description Charge le détail d'une commande
     * @param comId Identifiant de la commande
     * @returns Détail de la commande
     */
    getCommande(comId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<CommandeRead> {
        return this.http.get<CommandeRead>(`/api/restaurants/commandes/${comId}`, {observe: 'body', ...options});
    }

    /**
     * @description Récupère le détail complet d'une commande avec ses lignes
     * @param comId Identifiant de la commande
     * @returns Détail complet de la commande
     */
    getCommandeDetail(comId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<CommandeDetailRead> {
        return this.http.get<CommandeDetailRead>(`/api/restaurants/commandes/${comId}/detail`, {observe: 'body', ...options});
    }

    /**
     * @description Liste les lignes d'une commande
     * @param comId Identifiant de la commande
     * @returns Liste des lignes de la commande
     */
    getCommandeLignes(comId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<LigneCommandeItem[]> {
        return this.http.get<LigneCommandeItem[]>(`/api/restaurants/commandes/${comId}/lignes`, {observe: 'body', ...options});
    }

    /**
     * @description Liste toutes les commandes
     * @param clientId Client ayant passé la commande
     * @param statutCommandeCode Statut de la commande
     * @param tableId Table associée à la commande
     * @returns Liste des commandes
     */
    getCommandes(clientId?: number, statutCommandeCode: StatutCommandeCode = "EN_ATT", tableId?: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<CommandeItem[]> {
        const addParam = (key: string, value: any) => {
          if (value !== null && value !== undefined) {
            if (options.params instanceof HttpParams) {
              options.params = options.params.append(key, value);
            } else {
              if (!options.params) {
                options.params = {};
              }
              options.params[key] = value;
            }
          }
        };
        addParam('clientId', clientId);
        addParam('statutCommandeCode', statutCommandeCode);
        addParam('tableId', tableId);

        return this.http.get<CommandeItem[]>(`/api/restaurants/commandes`, {observe: 'body', ...options});
    }

    /**
     * @description Récupère les commandes par date
     * @param dateCommande Date et heure de la commande
     * @returns Commandes pour la date spécifiée
     */
    getCommandesByDate(dateCommande?: string, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<CommandeItem[]> {
        const addParam = (key: string, value: any) => {
          if (value !== null && value !== undefined) {
            if (options.params instanceof HttpParams) {
              options.params = options.params.append(key, value);
            } else {
              if (!options.params) {
                options.params = {};
              }
              options.params[key] = value;
            }
          }
        };
        addParam('dateCommande', dateCommande);

        return this.http.get<CommandeItem[]>(`/api/restaurants/commandes/by-date`, {observe: 'body', ...options});
    }

    /**
     * @description Charge le détail d'une ligne de commande
     * @param ligId Identifiant de la ligne
     * @returns Détail de la ligne de commande
     */
    getLigneCommande(ligId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<LigneCommandeRead> {
        return this.http.get<LigneCommandeRead>(`/api/restaurants/ligne-commandes/${ligId}`, {observe: 'body', ...options});
    }

    /**
     * @description Liste toutes les lignes de commande
     * @param commandeId Commande à laquelle appartient la ligne
     * @param platId Plat commandé
     * @returns Liste des lignes de commande
     */
    getLigneCommandes(commandeId?: number, platId?: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<LigneCommandeItem[]> {
        const addParam = (key: string, value: any) => {
          if (value !== null && value !== undefined) {
            if (options.params instanceof HttpParams) {
              options.params = options.params.append(key, value);
            } else {
              if (!options.params) {
                options.params = {};
              }
              options.params[key] = value;
            }
          }
        };
        addParam('commandeId', commandeId);
        addParam('platId', platId);

        return this.http.get<LigneCommandeItem[]>(`/api/restaurants/ligne-commandes`, {observe: 'body', ...options});
    }

    /**
     * @description Liste tous les statuts de commande
     * @returns Liste des statuts de commande
     */
    getStatutCommandes(options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<StatutCommande[]> {
        return this.http.get<StatutCommande[]>(`/api/restaurants/statuts-commande`, {observe: 'body', ...options});
    }

    /**
     * @description Met à jour partiellement une commande
     * @param comId Identifiant de la commande
     * @param commande Données partielles de la commande
     * @returns Commande mise à jour
     */
    patchCommande(comId: number, commande: CommandeWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<CommandeRead> {
        return this.http.patch<CommandeRead>(`/api/restaurants/commandes/${comId}`, commande, {observe: 'body', ...options});
    }

    /**
     * @description Met à jour une commande
     * @param comId Identifiant de la commande
     * @param commande Commande à mettre à jour
     * @returns Commande mise à jour
     */
    updateCommande(comId: number, commande: CommandeWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<CommandeRead> {
        return this.http.put<CommandeRead>(`/api/restaurants/commandes/${comId}`, commande, {observe: 'body', ...options});
    }

    /**
     * @description Met à jour uniquement le statut d'une commande
     * @param comId Identifiant de la commande
     * @param statutCommandeCode Statut de la commande
     * @returns Commande avec le statut mis à jour
     */
    updateCommandeStatut(comId: number, statutCommandeCode: StatutCommandeCode = "EN_ATT", options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<CommandeRead> {
        const addParam = (key: string, value: any) => {
          if (value !== null && value !== undefined) {
            if (options.params instanceof HttpParams) {
              options.params = options.params.append(key, value);
            } else {
              if (!options.params) {
                options.params = {};
              }
              options.params[key] = value;
            }
          }
        };
        addParam('statutCommandeCode', statutCommandeCode);

        return this.http.patch<CommandeRead>(`/api/restaurants/commandes/${comId}/statut`, {}, {observe: 'body', ...options});
    }

    /**
     * @description Met à jour une ligne de commande
     * @param ligId Identifiant de la ligne
     * @param ligneCommande Ligne de commande à mettre à jour
     * @returns Ligne de commande mise à jour
     */
    updateLigneCommande(ligId: number, ligneCommande: LigneCommandeWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<LigneCommandeRead> {
        return this.http.put<LigneCommandeRead>(`/api/restaurants/ligne-commandes/${ligId}`, ligneCommande, {observe: 'body', ...options});
    }
}
