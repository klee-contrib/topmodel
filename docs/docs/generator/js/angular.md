# Angular Generator

Le générateur Angular est un mode particulier du [générateur JavaScript](../js.md). Il est activé en configurant le paramètre `apiMode` à `angular` ou `angular_promise` dans la configuration JavaScript.

Contrairement aux autres modes (`fetch`, `nuxt`, `legacy`) qui produisent des fonctions utilitaires, le mode Angular génère une **classe de service `@Injectable`** par fichier d'endpoints, utilisant le module `HttpClient` officiel d'Angular.

## Activation

```yaml
javascript:
  outputDirectory: "./src/appgenerated"
  apiMode: angular # ou angular_promise
  apiClientRootPath: "./api"
  apiClientFilePath: "{module}/{fileName}"
  # ... autres paramètres JavaScript
```

## Modes

Deux modes sont disponibles :

### `angular`

Les méthodes du service retournent des `Observable` de RxJS (comportement natif du `HttpClient` Angular).

```typescript
getRestaurant(resId: number, options: /* ... */ = {}): Observable<RestaurantRead> {
    return this.http.get<RestaurantRead>(`/api/restaurants/${resId}`, {observe: 'body', ...options});
}
```

### `angular_promise`

Les méthodes retournent des `Promise` au lieu d'`Observable`. Le générateur utilise `lastValueFrom` de RxJS pour convertir les observables en promesses. Ce mode est utile pour les applications qui préfèrent la sémantique `async/await` sans dépendre de RxJS directement dans les composants.

```typescript
getRestaurant(resId: number, options: /* ... */ = {}): Promise<RestaurantRead> {
    return lastValueFrom(this.http.get<RestaurantRead>(`/api/restaurants/${resId}`, {observe: 'body', ...options}));
}
```

## Structure du fichier généré

### Nommage des fichiers

Contrairement aux autres modes du générateur JavaScript, le nom des fichiers d'API générés est suffixé par `.service.ts`. Par exemple, pour un fichier d'endpoints nommé `restaurant`, le fichier généré sera `restaurant.service.ts`.

Ce comportement est contrôlé par `apiClientFilePath` (valeur par défaut : `{module}`). L'extension `.ts` ainsi que le suffixe `.service` sont ajoutés automatiquement.

### Classe de service

Chaque fichier d'endpoints est converti en une classe `@Injectable` fournie au root de l'application Angular (`providedIn: 'root'`). Le `HttpClient` est injecté via la fonction `inject()` (pattern moderne Angular >= 14).

```typescript
import { HttpClient, HttpContext, HttpHeaders, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class RestaurantService {

    private readonly http = inject(HttpClient);

    // ... méthodes générées
}
```

Le nom de la classe est dérivé du nom de fichier (en PascalCase) suffixé par `Service` (ex : `restaurant` -> `RestaurantService`).

## Signature des méthodes

Chaque endpoint est transformé en méthode de la classe. La signature respecte les conventions suivantes :

- Les paramètres de route et les paramètres de query/body du modèle sont exposés dans le même ordre que la définition TopModel.
- Un paramètre `options` typé est **systématiquement** ajouté en dernier, permettant de surcharger les options standards du `HttpClient` (`headers`, `context`, `params`, `withCredentials`, `reportProgress`, `transferCache`).
- Le type de retour est typé à partir de l'objet retourné par l'endpoint.

```typescript
/**
 * @description Met à jour un restaurant
 * @param resId Identifiant du restaurant
 * @param restaurant Restaurant à mettre à jour
 * @returns Restaurant mis à jour
 */
updateRestaurant(
    resId: number,
    restaurant: RestaurantWrite,
    options: {
        headers?: HttpHeaders | {[header: string]: string | string[]};
        context?: HttpContext;
        params?: HttpParams | {[param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>};
        withCredentials?: boolean;
        reportProgress?: boolean;
        transferCache?: {includeHeaders?: string[]} | boolean;
    } = {}
): Observable<RestaurantRead> {
    return this.http.put<RestaurantRead>(`/api/restaurants/${resId}`, restaurant, {observe: 'body', ...options});
}
```

Un commentaire JSDoc est généré à partir des `comments` du modèle (endpoint, params et retour).

## Option `observe` automatique

Le générateur Angular détecte automatiquement le type de retour de l'endpoint pour configurer l'option `observe` de la requête HTTP :

- **`HttpResponse<TonObjet>`** : si le type de retour est `HttpResponse<UnObjet>`, le générateur ajoute automatiquement `observe: 'response'` à la requête. Cela permet d'accéder à l'objet `HttpResponse` complet (headers, status code, body).
- **`HttpEvent<TonObjet>`** : si le type de retour est `HttpEvent<UnObjet>`, le générateur ajoute automatiquement `observe: 'events'`. Utile pour suivre la progression d'un upload par exemple.
- **Par défaut** : pour tous les autres types de retour, l'option `observe: 'body'` est utilisée, ce qui retourne uniquement le body de la réponse (sans son enveloppe HTTP).

## Gestion des types binaires

Lorsque le type de retour est `string`, `Blob` ou `ArrayBuffer`, le générateur ajoute automatiquement l'option `responseType` adaptée :

- `string` -> `responseType: 'text'`
- `Blob` -> `responseType: 'blob'`
- `ArrayBuffer` -> `responseType: 'arraybuffer'`

Le générateur omet par ailleurs le type générique de la méthode `http.get/post/...` dans ces cas, pour rester compatible avec la signature du `HttpClient`.

```typescript
/**
 * @description Exporte les commandes au format CSV
 * @param dateDebut Date et heure de la commande
 * @param dateFin Date et heure de la commande
 * @returns Fichier CSV des commandes
 */
exportCommandes(dateDebut?: string, dateFin?: string, options: /* ... */ = {}): Observable<Blob> {
    // ... construction des query params ...
    return this.http.get(`/api/restaurants/commandes/export`, {observe: 'body', responseType: 'blob', ...options});
}
```

## Paramètres de query string

Pour les endpoints qui déclarent des query params, le générateur insère une fonction utilitaire `addParam` dans chaque méthode, qui injecte le paramètre dans `options.params` uniquement s'il est défini (différent de `null`/`undefined`). Cette fonction gère à la fois les `HttpParams` (API immuable Angular) et les objets `{[param: string]: ...}`.

```typescript
searchRestaurants(nom?: string, adresse?: string, noteMin?: number, options: /* ... */ = {}): Observable<RestaurantAvecStatistiques[]> {
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
```

Les query params sans valeur par défaut sont générés comme optionnels (`?:`), ceux avec valeur par défaut utilisent la syntaxe `= value`.

## Body d'une requête DELETE

Contrairement aux autres méthodes, si un endpoint `DELETE` déclare un paramètre de body JSON, celui-ci est transmis via l'option `body` (et non en deuxième argument de `http.delete`), conformément à la signature Angular :

```typescript
deleteCommandeWithBody(commandeItem: CommandeItem, options: /* ... */ = {}): Observable<void> {
    return this.http.delete<void>(`/api/restaurants/commandes`, {body: commandeItem, observe: 'body', ...options});
}
```

## Endpoints multipart

Lorsqu'un endpoint est marqué comme multipart (upload de fichier par exemple), le générateur produit un `FormData` à partir des paramètres de body, et ajoute une méthode utilitaire privée `fillFormData` à la classe pour construire récursivement le `FormData` (support des objets imbriqués, des tableaux et des `File`).

```typescript
private fillFormData(data: any, formData: FormData, prefix = "") {
    if (Array.isArray(data)) {
        for (const [i, item] of data.entries()) {
            this.fillFormData(item, formData, prefix + (typeof item === "object" && !(item instanceof File) ? `[${i}]` : ""));
        }
    } else if (typeof data === "object" && !(data instanceof File)) {
        for (const key in data) {
            this.fillFormData(data[key], formData, (prefix ? `${prefix}.` : "") + key);
        }
    } else {
        formData.append(prefix, data);
    }
}
```

## Utilisation dans un composant Angular

Le service étant fourni au root, il peut être injecté n'importe où via la fonction `inject` :

```typescript
import { Component, inject, signal } from "@angular/core";
import { RestaurantService } from "./appgenerated/api/restaurant/restaurant.service";
import { RestaurantItem } from "./appgenerated/model/restaurant/restaurant-item";

@Component({
  selector: "app-root",
  template: `...`,
})
export class App {
  private readonly restaurantService = inject(RestaurantService);

  restaurants = signal<RestaurantItem[]>([]);

  ngOnInit() {
    this.restaurantService.getRestaurants().subscribe((data) => {
      this.restaurants.set(data);
    });
  }
}
```

En mode `angular_promise`, la consommation se fait naturellement avec `async/await` :

```typescript
async ngOnInit() {
  const data = await this.restaurantService.getRestaurants();
  this.restaurants.set(data);
}
```

## Configuration complète

```yaml
javascript:
  outputDirectory: "./src/appgenerated"
  tags:
    - front
  modelRootPath: "./model/{module}"
  resourceRootPath: "./resources/{lang}"
  apiClientRootPath: "./api"
  apiClientFilePath: "{module}/{fileName}"
  apiMode: angular # ou angular_promise
  entityMode: none
  resourceMode: json
  domainPath: "../domains"
```

Pour désactiver uniquement la génération du service Angular (tout en conservant la génération des modèles et des ressources), utilisez :

```yaml
javascript:
  disable:
    - JSNGApiClientGen
```
