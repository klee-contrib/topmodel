////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////


import { HttpClient, HttpContext, HttpHeaders, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AvisClientRead } from "../../model/restaurant/avis-client-read";
import { ClientAvecCommandes } from "../../model/restaurant/client-avec-commandes";
import { ClientItem } from "../../model/restaurant/client-item";
import { ClientRead } from "../../model/restaurant/client-read";
import { ClientWrite } from "../../model/restaurant/client-write";
import { CommandeItem } from "../../model/restaurant/commande-item";
import { EmployeRead } from "../../model/restaurant/employe-read";
import { EmployeWrite } from "../../model/restaurant/employe-write";
@Injectable({
    providedIn: 'root'
})
export class PersonneService {

    private readonly http = inject(HttpClient);

    /**
     * @description Ajoute un client
     * @param client Client à créer
     * @returns Client créé
     */
    addClient(client: ClientWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<ClientRead> {
        return this.http.post<ClientRead>(`/api/restaurants/clients`, client, {observe: 'body', ...options});
    }

    /**
     * @description Ajoute un employé (nécessite le rôle ADMIN)
     * @param employe Employé à créer
     * @returns Employé créé
     */
    addEmploye(employe: EmployeWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<EmployeRead> {
        return this.http.post<EmployeRead>(`/api/restaurants/employes`, employe, {observe: 'body', ...options});
    }

    /**
     * @description Supprime un client
     * @param perId Identifiant de la personne
     */
    deleteClient(perId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<void> {
        return this.http.delete<void>(`/api/restaurants/clients/${perId}`, {observe: 'body', ...options});
    }

    /**
     * @description Liste les avis clients avec filtres
     * @param resId Identifiant du restaurant
     * @param noteMin Note sur 5
     * @param approuve Indique si l'avis est approuvé par le restaurant
     * @param dateDebut Date de l'avis
     * @param dateFin Date de l'avis
     * @returns Liste des avis correspondant aux critères
     */
    getAvisClients(resId?: number, noteMin?: number, approuve: boolean = false, dateDebut?: string, dateFin?: string, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<AvisClientRead[]> {
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
        addParam('resId', resId);
        addParam('noteMin', noteMin);
        addParam('approuve', approuve);
        addParam('dateDebut', dateDebut);
        addParam('dateFin', dateFin);

        return this.http.get<AvisClientRead[]>(`/api/restaurants/avis`, {observe: 'body', ...options});
    }

    /**
     * @description Charge le détail d'un client
     * @param perId Identifiant de la personne
     * @returns Détail du client
     */
    getClient(perId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<ClientRead> {
        return this.http.get<ClientRead>(`/api/restaurants/clients/${perId}`, {observe: 'body', ...options});
    }

    /**
     * @description Récupère un client avec toutes ses commandes
     * @param perId Identifiant de la personne
     * @returns Client avec ses commandes
     */
    getClientAvecCommandes(perId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<ClientAvecCommandes> {
        return this.http.get<ClientAvecCommandes>(`/api/restaurants/clients/${perId}/avec-commandes`, {observe: 'body', ...options});
    }

    /**
     * @description Liste les commandes d'un client
     * @param perId Identifiant de la personne
     * @returns Liste des commandes du client
     */
    getClientCommandes(perId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<CommandeItem[]> {
        return this.http.get<CommandeItem[]>(`/api/restaurants/clients/${perId}/commandes`, {observe: 'body', ...options});
    }

    /**
     * @description Liste tous les clients
     * @param nom Nom de la personne
     * @param email Adresse email du client
     * @returns Liste des clients
     */
    getClients(nom?: string, email?: string, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<ClientItem[]> {
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
        addParam('nom', nom);
        addParam('email', email);

        return this.http.get<ClientItem[]>(`/api/restaurants/clients`, {observe: 'body', ...options});
    }

    /**
     * @description Met à jour partiellement un client
     * @param perId Identifiant de la personne
     * @param client Données partielles du client
     * @returns Client mis à jour
     */
    patchClient(perId: number, client: ClientWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<ClientRead> {
        return this.http.patch<ClientRead>(`/api/restaurants/clients/${perId}`, client, {observe: 'body', ...options});
    }

    /**
     * @description Met à jour un client
     * @param perId Identifiant de la personne
     * @param client Client à mettre à jour
     * @returns Client mis à jour
     */
    updateClient(perId: number, client: ClientWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<ClientRead> {
        return this.http.put<ClientRead>(`/api/restaurants/clients/${perId}`, client, {observe: 'body', ...options});
    }
}
