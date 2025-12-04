////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////


import { HttpClient, HttpContext, HttpHeaders, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AvisClientRead } from "../../model/restaurant/avis-client-read";
import { ClientAvecCommandes } from "../../model/restaurant/client-avec-commandes";
import { CommandeDetailRead } from "../../model/restaurant/commande-detail-read";
import { EmployeRead } from "../../model/restaurant/employe-read";
import { EmployeWrite } from "../../model/restaurant/employe-write";
import { MenuComplet } from "../../model/restaurant/menu-complet";
import { MenuWrite } from "../../model/restaurant/menu-write";
import { PromotionRead } from "../../model/restaurant/promotion-read";
import { PromotionWrite } from "../../model/restaurant/promotion-write";
import { ReservationAvecDetails } from "../../model/restaurant/reservation-avec-details";
import { ReservationWrite } from "../../model/restaurant/reservation-write";
import { RestaurantAvecStatistiques } from "../../model/restaurant/restaurant-avec-statistiques";
import { StatistiquesRestaurant } from "../../model/restaurant/statistiques-restaurant";
@Injectable({
    providedIn: 'root'
})
export class AdvancedEndpointsService {

    private readonly http = inject(HttpClient);

    /**
     * @description Ajoute un employé (nécessite le rôle ADMIN)
     * @param employe Employé à créer
     * @returns Employé créé
     */
    addEmploye(employe: EmployeWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<EmployeRead> {
        return this.http.post<EmployeRead>(`/api/restaurants/employes`, employe, {observe: 'body', ...options});
    }

    /**
     * @description Crée un menu avec ses plats
     * @param menu Menu à créer
     * @returns Menu créé avec ses plats
     */
    createMenu(menu: MenuWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<MenuComplet> {
        return this.http.post<MenuComplet>(`/api/restaurants/menus`, menu, {observe: 'body', ...options});
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
     * @description Liste les avis clients avec filtres
     * @param resRestaurantId Identifiant du restaurant
     * @param noteMin Note sur 5
     * @param approuve Indique si l'avis est approuvé par le restaurant
     * @param dateDebut Date de l'avis
     * @param dateFin Date de l'avis
     * @returns Liste des avis correspondant aux critères
     */
    getAvisClients(resRestaurantId?: number, noteMin?: number, approuve: boolean = false, dateDebut?: string, dateFin?: string, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<AvisClientRead[]> {
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
        addParam('resRestaurantId', resRestaurantId);
        addParam('noteMin', noteMin);
        addParam('approuve', approuve);
        addParam('dateDebut', dateDebut);
        addParam('dateFin', dateFin);

        return this.http.get<AvisClientRead[]>(`/api/restaurants/avis`, {observe: 'body', ...options});
    }

    /**
     * @description Récupère un client avec toutes ses commandes
     * @param cliId Identifiant du client
     * @returns Client avec ses commandes
     */
    getClientAvecCommandes(cliId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<ClientAvecCommandes> {
        return this.http.get<ClientAvecCommandes>(`/api/restaurants/clients/${cliId}/avec-commandes`, {observe: 'body', ...options});
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
     * @description Récupère un menu spécifique d'un restaurant
     * @param resId Identifiant du restaurant
     * @param menId Identifiant du menu
     * @returns Menu du restaurant
     */
    getRestaurantMenu(resId: number, menId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<MenuComplet> {
        return this.http.get<MenuComplet>(`/api/restaurants/${resId}/menus/${menId}`, {observe: 'body', ...options});
    }

    /**
     * @description Récupère les statistiques d'un restaurant
     * @param resId Identifiant du restaurant
     * @param dateDebut Date de début pour le calcul des statistiques
     * @param dateFin Date de fin pour le calcul des statistiques
     * @returns Statistiques du restaurant
     */
    getRestaurantStatistiques(resId: number, dateDebut?: string, dateFin?: string, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<StatistiquesRestaurant> {
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

        return this.http.get<StatistiquesRestaurant>(`/api/restaurants/${resId}/statistiques`, {observe: 'body', ...options});
    }

    /**
     * @description Met à jour partiellement une promotion
     * @param proId Identifiant de la promotion
     * @param promotion Données partielles de la promotion
     * @returns Promotion mise à jour
     */
    patchPromotion(proId: number, promotion: PromotionWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<PromotionRead> {
        return this.http.patch<PromotionRead>(`/api/restaurants/promotions/${proId}`, promotion, {observe: 'body', ...options});
    }

    /**
     * @description Recherche avancée de restaurants
     * @param nom Nom du restaurant (recherche partielle)
     * @param adresse Adresse du restaurant (recherche partielle)
     * @param noteMin Note minimum requise
     * @returns Liste des restaurants correspondant aux critères
     */
    searchRestaurants(nom?: string, adresse?: string, noteMin?: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<RestaurantAvecStatistiques[]> {
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
        addParam('adresse', adresse);
        addParam('noteMin', noteMin);

        return this.http.get<RestaurantAvecStatistiques[]>(`/api/restaurants/search`, {observe: 'body', ...options});
    }
}
