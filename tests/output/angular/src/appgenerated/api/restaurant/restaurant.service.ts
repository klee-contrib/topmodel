////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import { HttpClient, HttpContext, HttpHeaders, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { CategoriePlatCode } from "../../model/restaurant/enums";
import { MenuRead } from "../../model/restaurant/menu-read";
import { PlatItem } from "../../model/restaurant/plat-item";
import { RestaurantAvecStatistiques } from "../../model/restaurant/restaurant-avec-statistiques";
import { RestaurantItem } from "../../model/restaurant/restaurant-item";
import { RestaurantRead } from "../../model/restaurant/restaurant-read";
import { RestaurantWrite } from "../../model/restaurant/restaurant-write";
import { StatistiquesRestaurant } from "../../model/restaurant/statistiques-restaurant";
import { TableItem } from "../../model/restaurant/table-item";
import { TableRead } from "../../model/restaurant/table-read";
import { TableWrite } from "../../model/restaurant/table-write";

@Injectable({
    providedIn: 'root'
})
export class RestaurantService {

    private readonly http = inject(HttpClient);

    /**
     * Ajoute un restaurant
     * @param restaurant Restaurant à créer
     * @returns Restaurant créé
     */
    addRestaurant(restaurant: RestaurantWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<RestaurantRead> {
        return this.http.post<RestaurantRead>(`/api/restaurants`, restaurant, {observe: 'body', ...options});
    }

    /**
     * Ajoute une table
     * @param table Table à créer
     * @returns Table créée
     */
    addTable(table: TableWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<TableRead> {
        return this.http.post<TableRead>(`/api/restaurants/tables`, table, {observe: 'body', ...options});
    }

    /**
     * Supprime un restaurant
     * @param resId Identifiant du restaurant
     */
    deleteRestaurant(resId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<void> {
        return this.http.delete<void>(`/api/restaurants/${resId}`, {observe: 'body', ...options});
    }

    /**
     * Supprime une table
     * @param tabId Identifiant de la table
     */
    deleteTable(tabId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<void> {
        return this.http.delete<void>(`/api/restaurants/tables/${tabId}`, {observe: 'body', ...options});
    }

    /**
     * Charge le détail d'un restaurant
     * @param resId Identifiant du restaurant
     * @returns Détail du restaurant
     */
    getRestaurant(resId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<RestaurantRead> {
        return this.http.get<RestaurantRead>(`/api/restaurants/${resId}`, {observe: 'body', ...options});
    }

    /**
     * Récupère un menu spécifique d'un restaurant
     * @param resId Identifiant du restaurant
     * @param menId Identifiant du menu
     * @returns Menu du restaurant
     */
    getRestaurantMenu(resId: number, menId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<MenuRead> {
        return this.http.get<MenuRead>(`/api/restaurants/${resId}/menus/${menId}`, {observe: 'body', ...options});
    }

    /**
     * Liste les plats d'un restaurant
     * @param resId Identifiant du restaurant
     * @param categoriePlatCode Catégorie du plat
     * @param disponible Indique si le plat est disponible
     * @returns Liste des plats du restaurant
     */
    getRestaurantPlats(resId: number, categoriePlatCode: CategoriePlatCode, disponible: boolean = true, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<PlatItem[]> {
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
        addParam('categoriePlatCode', categoriePlatCode);
        addParam('disponible', disponible);

        return this.http.get<PlatItem[]>(`/api/restaurants/${resId}/plats`, {observe: 'body', ...options});
    }

    /**
     * Récupère les statistiques d'un restaurant
     * @param resId Identifiant du restaurant
     * @param dateDebut Date de début pour le calcul des statistiques
     * @param dateFin Date de fin pour le calcul des statistiques
     * @returns Statistiques du restaurant
     */
    getRestaurantStatistiques(resId: number, dateDebut: string, dateFin: string, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<StatistiquesRestaurant> {
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
     * Liste les tables d'un restaurant
     * @param resId Identifiant du restaurant
     * @param disponible Indique si la table est disponible
     * @returns Liste des tables du restaurant
     */
    getRestaurantTables(resId: number, disponible: boolean = true, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<TableItem[]> {
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
        addParam('disponible', disponible);

        return this.http.get<TableItem[]>(`/api/restaurants/${resId}/tables`, {observe: 'body', ...options});
    }

    /**
     * Liste tous les restaurants
     * @returns Liste des restaurants
     */
    getRestaurants(options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<RestaurantItem[]> {
        return this.http.get<RestaurantItem[]>(`/api/restaurants`, {observe: 'body', ...options});
    }

    /**
     * Charge le détail d'une table
     * @param tabId Identifiant de la table
     * @returns Détail de la table
     */
    getTable(tabId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<TableRead> {
        return this.http.get<TableRead>(`/api/restaurants/tables/${tabId}`, {observe: 'body', ...options});
    }

    /**
     * Liste toutes les tables
     * @param restaurantId Restaurant auquel appartient la table
     * @param disponible Indique si la table est disponible
     * @returns Liste des tables
     */
    getTables(restaurantId: number, disponible: boolean = true, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<TableItem[]> {
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
        addParam('restaurantId', restaurantId);
        addParam('disponible', disponible);

        return this.http.get<TableItem[]>(`/api/restaurants/tables`, {observe: 'body', ...options});
    }

    /**
     * Recherche avancée de restaurants
     * @param nom Nom du restaurant (recherche partielle)
     * @param adresse Adresse du restaurant (recherche partielle)
     * @param noteMin Note minimum requise
     * @returns Liste des restaurants correspondant aux critères
     */
    searchRestaurants(nom: string, adresse?: string, noteMin?: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<RestaurantAvecStatistiques[]> {
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

    /**
     * Met à jour un restaurant
     * @param resId Identifiant du restaurant
     * @param restaurant Restaurant à mettre à jour
     * @returns Restaurant mis à jour
     */
    updateRestaurant(resId: number, restaurant: RestaurantWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<RestaurantRead> {
        return this.http.put<RestaurantRead>(`/api/restaurants/${resId}`, restaurant, {observe: 'body', ...options});
    }

    /**
     * Met à jour une table
     * @param tabId Identifiant de la table
     * @param table Table à mettre à jour
     * @returns Table mise à jour
     */
    updateTable(tabId: number, table: TableWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<TableRead> {
        return this.http.put<TableRead>(`/api/restaurants/tables/${tabId}`, table, {observe: 'body', ...options});
    }
}
