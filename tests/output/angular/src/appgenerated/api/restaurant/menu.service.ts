////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////


import { HttpClient, HttpContext, HttpHeaders, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { MenuRead } from "../../model/restaurant/menu-read";
import { MenuWrite } from "../../model/restaurant/menu-write";
import { PlatItem } from "../../model/restaurant/plat-item";
import { PlatRead } from "../../model/restaurant/plat-read";
import { PlatWrite } from "../../model/restaurant/plat-write";
import { PromotionRead } from "../../model/restaurant/promotion-read";
import { PromotionWrite } from "../../model/restaurant/promotion-write";
import { CategoriePlat, CategoriePlatCode } from "../../model/restaurant/references";
@Injectable({
    providedIn: 'root'
})
export class MenuService {

    private readonly http = inject(HttpClient);

    /**
     * @description Ajoute un plat
     * @param plat Plat à créer
     * @returns Plat créé
     */
    addPlat(plat: PlatWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<PlatRead> {
        return this.http.post<PlatRead>(`/api/restaurants/plats`, plat, {observe: 'body', ...options});
    }

    /**
     * @description Crée un menu avec ses plats
     * @param menu Menu à créer
     * @returns Menu créé avec ses plats
     */
    createMenu(menu: MenuWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<MenuRead> {
        return this.http.post<MenuRead>(`/api/restaurants/menus`, menu, {observe: 'body', ...options});
    }

    /**
     * @description Supprime un plat
     * @param plaId Identifiant du plat
     */
    deletePlat(plaId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<void> {
        return this.http.delete<void>(`/api/restaurants/plats/${plaId}`, {observe: 'body', ...options});
    }

    /**
     * @description Liste toutes les catégories de plats
     * @returns Liste des catégories de plats
     */
    getCategoriePlats(options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<CategoriePlat[]> {
        return this.http.get<CategoriePlat[]>(`/api/restaurants/categorie-plats`, {observe: 'body', ...options});
    }

    /**
     * @description Charge le détail d'un plat
     * @param plaId Identifiant du plat
     * @returns Détail du plat
     */
    getPlat(plaId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<PlatRead> {
        return this.http.get<PlatRead>(`/api/restaurants/plats/${plaId}`, {observe: 'body', ...options});
    }

    /**
     * @description Liste tous les plats
     * @param disponible Indique si le plat est disponible
     * @param restaurantId Restaurant proposant ce plat
     * @param categoriePlatCode Catégorie du plat
     * @returns Liste des plats
     */
    getPlats(disponible: boolean = true, restaurantId?: number, categoriePlatCode?: CategoriePlatCode, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<PlatItem[]> {
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
        addParam('restaurantId', restaurantId);
        addParam('categoriePlatCode', categoriePlatCode);

        return this.http.get<PlatItem[]>(`/api/restaurants/plats`, {observe: 'body', ...options});
    }

    /**
     * @description Met à jour partiellement un plat
     * @param plaId Identifiant du plat
     * @param plat Données partielles du plat
     * @returns Plat mis à jour
     */
    patchPlat(plaId: number, plat: PlatWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<PlatRead> {
        return this.http.patch<PlatRead>(`/api/restaurants/plats/${plaId}`, plat, {observe: 'body', ...options});
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
     * @description Recherche de plats avec critères multiples
     * @param nom Nom du plat
     * @param restaurantId Restaurant proposant ce plat
     * @param categoriePlatCode Catégorie du plat
     * @param disponible Indique si le plat est disponible
     * @returns Plats correspondant aux critères de recherche
     */
    searchPlats(nom?: string, restaurantId?: number, categoriePlatCode?: CategoriePlatCode, disponible: boolean = true, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<PlatItem[]> {
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
        addParam('restaurantId', restaurantId);
        addParam('categoriePlatCode', categoriePlatCode);
        addParam('disponible', disponible);

        return this.http.get<PlatItem[]>(`/api/restaurants/plats/search`, {observe: 'body', ...options});
    }

    /**
     * @description Met à jour un plat
     * @param plaId Identifiant du plat
     * @param plat Plat à mettre à jour
     * @returns Plat mis à jour
     */
    updatePlat(plaId: number, plat: PlatWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<PlatRead> {
        return this.http.put<PlatRead>(`/api/restaurants/plats/${plaId}`, plat, {observe: 'body', ...options});
    }
}
