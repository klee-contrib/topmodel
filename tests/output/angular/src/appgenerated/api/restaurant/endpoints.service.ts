////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////


import { HttpClient, HttpContext, HttpHeaders, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ClientItem } from "../../model/restaurant/client-item";
import { ClientRead } from "../../model/restaurant/client-read";
import { ClientWrite } from "../../model/restaurant/client-write";
import { CommandeItem } from "../../model/restaurant/commande-item";
import { CommandeRead } from "../../model/restaurant/commande-read";
import { CommandeWrite } from "../../model/restaurant/commande-write";
import { LigneCommandeItem } from "../../model/restaurant/ligne-commande-item";
import { LigneCommandeRead } from "../../model/restaurant/ligne-commande-read";
import { LigneCommandeWrite } from "../../model/restaurant/ligne-commande-write";
import { PlatItem } from "../../model/restaurant/plat-item";
import { PlatRead } from "../../model/restaurant/plat-read";
import { PlatWrite } from "../../model/restaurant/plat-write";
import { CategoriePlat, CategoriePlatCode, StatutCommande, StatutCommandeCode } from "../../model/restaurant/references";
import { RestaurantItem } from "../../model/restaurant/restaurant-item";
import { RestaurantRead } from "../../model/restaurant/restaurant-read";
import { RestaurantWrite } from "../../model/restaurant/restaurant-write";
import { TableClientItem } from "../../model/restaurant/table-client-item";
import { TableClientRead } from "../../model/restaurant/table-client-read";
import { TableClientWrite } from "../../model/restaurant/table-client-write";
@Injectable({
    providedIn: 'root'
})
export class EndpointsService {

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
     * @description Ajoute un plat
     * @param plat Plat à créer
     * @returns Plat créé
     */
    addPlat(plat: PlatWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<PlatRead> {
        return this.http.post<PlatRead>(`/api/restaurants/plats`, plat, {observe: 'body', ...options});
    }

    /**
     * @description Ajoute un restaurant
     * @param restaurant Restaurant à créer
     * @returns Restaurant créé
     */
    addRestaurant(restaurant: RestaurantWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<RestaurantRead> {
        return this.http.post<RestaurantRead>(`/api/restaurants`, restaurant, {observe: 'body', ...options});
    }

    /**
     * @description Ajoute une table
     * @param table Table à créer
     * @returns Table créée
     */
    addTable(table: TableClientWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<TableClientRead> {
        return this.http.post<TableClientRead>(`/api/restaurants/tables`, table, {observe: 'body', ...options});
    }

    /**
     * @description Supprime un client
     * @param cliId Identifiant du client
     */
    deleteClient(cliId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<void> {
        return this.http.delete<void>(`/api/restaurants/clients/${cliId}`, {observe: 'body', ...options});
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
     * @description Supprime un plat
     * @param plaId Identifiant du plat
     */
    deletePlat(plaId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<void> {
        return this.http.delete<void>(`/api/restaurants/plats/${plaId}`, {observe: 'body', ...options});
    }

    /**
     * @description Supprime un restaurant
     * @param resId Identifiant du restaurant
     */
    deleteRestaurant(resId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<void> {
        return this.http.delete<void>(`/api/restaurants/${resId}`, {observe: 'body', ...options});
    }

    /**
     * @description Supprime une table
     * @param tabId Identifiant de la table
     */
    deleteTable(tabId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<void> {
        return this.http.delete<void>(`/api/restaurants/tables/${tabId}`, {observe: 'body', ...options});
    }

    /**
     * @description Liste toutes les catégories de plats
     * @returns Liste des catégories de plats
     */
    getCategoriePlats(options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<CategoriePlat[]> {
        return this.http.get<CategoriePlat[]>(`/api/restaurants/categorie-plats`, {observe: 'body', ...options});
    }

    /**
     * @description Charge le détail d'un client
     * @param cliId Identifiant du client
     * @returns Détail du client
     */
    getClient(cliId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<ClientRead> {
        return this.http.get<ClientRead>(`/api/restaurants/clients/${cliId}`, {observe: 'body', ...options});
    }

    /**
     * @description Liste les commandes d'un client
     * @param cliId Identifiant du client
     * @returns Liste des commandes du client
     */
    getClientCommandes(cliId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<CommandeItem[]> {
        return this.http.get<CommandeItem[]>(`/api/restaurants/clients/${cliId}/commandes`, {observe: 'body', ...options});
    }

    /**
     * @description Liste tous les clients
     * @param nom Nom du client
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
     * @description Charge le détail d'une commande
     * @param comId Identifiant de la commande
     * @returns Détail de la commande
     */
    getCommande(comId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<CommandeRead> {
        return this.http.get<CommandeRead>(`/api/restaurants/commandes/${comId}`, {observe: 'body', ...options});
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
     * @param tableClientId Table associée à la commande
     * @returns Liste des commandes
     */
    getCommandes(clientId?: number, statutCommandeCode: StatutCommandeCode = "EN_ATT", tableClientId?: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<CommandeItem[]> {
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
        addParam('tableClientId', tableClientId);

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
     * @param restaurantIdRestaurant Restaurant proposant ce plat
     * @param categoriePlatCodeCategoriePlat Catégorie du plat
     * @returns Liste des plats
     */
    getPlats(disponible: boolean = true, restaurantIdRestaurant?: number, categoriePlatCodeCategoriePlat?: CategoriePlatCode, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<PlatItem[]> {
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
        addParam('restaurantIdRestaurant', restaurantIdRestaurant);
        addParam('categoriePlatCodeCategoriePlat', categoriePlatCodeCategoriePlat);

        return this.http.get<PlatItem[]>(`/api/restaurants/plats`, {observe: 'body', ...options});
    }

    /**
     * @description Charge le détail d'un restaurant
     * @param resId Identifiant du restaurant
     * @returns Détail du restaurant
     */
    getRestaurant(resId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<RestaurantRead> {
        return this.http.get<RestaurantRead>(`/api/restaurants/${resId}`, {observe: 'body', ...options});
    }

    /**
     * @description Liste les plats d'un restaurant
     * @param resId Identifiant du restaurant
     * @param disponible Indique si le plat est disponible
     * @param categoriePlatCodeCategoriePlat Catégorie du plat
     * @returns Liste des plats du restaurant
     */
    getRestaurantPlats(resId: number, disponible: boolean = true, categoriePlatCodeCategoriePlat?: CategoriePlatCode, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<PlatItem[]> {
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
        addParam('categoriePlatCodeCategoriePlat', categoriePlatCodeCategoriePlat);

        return this.http.get<PlatItem[]>(`/api/restaurants/${resId}/plats`, {observe: 'body', ...options});
    }

    /**
     * @description Liste les tables d'un restaurant
     * @param resId Identifiant du restaurant
     * @param disponible Indique si la table est disponible
     * @returns Liste des tables du restaurant
     */
    getRestaurantTables(resId: number, disponible: boolean = true, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<TableClientItem[]> {
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

        return this.http.get<TableClientItem[]>(`/api/restaurants/${resId}/tables`, {observe: 'body', ...options});
    }

    /**
     * @description Liste tous les restaurants
     * @returns Liste des restaurants
     */
    getRestaurants(options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<RestaurantItem[]> {
        return this.http.get<RestaurantItem[]>(`/api/restaurants`, {observe: 'body', ...options});
    }

    /**
     * @description Liste tous les statuts de commande
     * @returns Liste des statuts de commande
     */
    getStatutCommandes(options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<StatutCommande[]> {
        return this.http.get<StatutCommande[]>(`/api/restaurants/statuts-commande`, {observe: 'body', ...options});
    }

    /**
     * @description Charge le détail d'une table
     * @param tabId Identifiant de la table
     * @returns Détail de la table
     */
    getTable(tabId: number, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<TableClientRead> {
        return this.http.get<TableClientRead>(`/api/restaurants/tables/${tabId}`, {observe: 'body', ...options});
    }

    /**
     * @description Liste toutes les tables
     * @param restaurantIdRestaurant Restaurant auquel appartient la table
     * @param disponible Indique si la table est disponible
     * @returns Liste des tables
     */
    getTables(restaurantIdRestaurant?: number, disponible: boolean = true, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<TableClientItem[]> {
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
        addParam('restaurantIdRestaurant', restaurantIdRestaurant);
        addParam('disponible', disponible);

        return this.http.get<TableClientItem[]>(`/api/restaurants/tables`, {observe: 'body', ...options});
    }

    /**
     * @description Met à jour partiellement un client
     * @param cliId Identifiant du client
     * @param client Données partielles du client
     * @returns Client mis à jour
     */
    patchClient(cliId: number, client: ClientWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<ClientRead> {
        return this.http.patch<ClientRead>(`/api/restaurants/clients/${cliId}`, client, {observe: 'body', ...options});
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
     * @description Met à jour partiellement un plat
     * @param plaId Identifiant du plat
     * @param plat Données partielles du plat
     * @returns Plat mis à jour
     */
    patchPlat(plaId: number, plat: PlatWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<PlatRead> {
        return this.http.patch<PlatRead>(`/api/restaurants/plats/${plaId}`, plat, {observe: 'body', ...options});
    }

    /**
     * @description Recherche de plats avec critères multiples
     * @param nom Nom du plat
     * @param restaurantIdRestaurant Restaurant proposant ce plat
     * @param categoriePlatCodeCategoriePlat Catégorie du plat
     * @param disponible Indique si le plat est disponible
     * @returns Plats correspondant aux critères de recherche
     */
    searchPlats(nom?: string, restaurantIdRestaurant?: number, categoriePlatCodeCategoriePlat?: CategoriePlatCode, disponible: boolean = true, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<PlatItem[]> {
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
        addParam('restaurantIdRestaurant', restaurantIdRestaurant);
        addParam('categoriePlatCodeCategoriePlat', categoriePlatCodeCategoriePlat);
        addParam('disponible', disponible);

        return this.http.get<PlatItem[]>(`/api/restaurants/plats/search`, {observe: 'body', ...options});
    }

    /**
     * @description Met à jour un client
     * @param cliId Identifiant du client
     * @param client Client à mettre à jour
     * @returns Client mis à jour
     */
    updateClient(cliId: number, client: ClientWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<ClientRead> {
        return this.http.put<ClientRead>(`/api/restaurants/clients/${cliId}`, client, {observe: 'body', ...options});
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

    /**
     * @description Met à jour un plat
     * @param plaId Identifiant du plat
     * @param plat Plat à mettre à jour
     * @returns Plat mis à jour
     */
    updatePlat(plaId: number, plat: PlatWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<PlatRead> {
        return this.http.put<PlatRead>(`/api/restaurants/plats/${plaId}`, plat, {observe: 'body', ...options});
    }

    /**
     * @description Met à jour un restaurant
     * @param resId Identifiant du restaurant
     * @param restaurant Restaurant à mettre à jour
     * @returns Restaurant mis à jour
     */
    updateRestaurant(resId: number, restaurant: RestaurantWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<RestaurantRead> {
        return this.http.put<RestaurantRead>(`/api/restaurants/${resId}`, restaurant, {observe: 'body', ...options});
    }

    /**
     * @description Met à jour une table
     * @param tabId Identifiant de la table
     * @param table Table à mettre à jour
     * @returns Table mise à jour
     */
    updateTable(tabId: number, table: TableClientWrite, options: {headers?: HttpHeaders | {[header: string]: string | string[]}; context?: HttpContext; params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>}; withCredentials?: boolean; reportProgress?: boolean; transferCache?: {includeHeaders?: string[]} | boolean} = {}): Observable<TableClientRead> {
        return this.http.put<TableClientRead>(`/api/restaurants/tables/${tabId}`, table, {observe: 'body', ...options});
    }
}
