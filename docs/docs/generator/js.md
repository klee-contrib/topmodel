# JS Generator

> **Note sur les imports :** Les imports spécifiés dans la configuration du générateur JS suivent cette règle :
>
> - S'ils commencent par `.` (ex : `./common` ou `../domains`), ils sont **relatifs** au répertoire de génération
> - Sinon, ils sont traités comme des **imports de modules** (ex : `@/domains` ou `luxon`)
> - Les domaines importés doivent inclure l'objet dans le chemin (ex : `luxon/DateTime` pour `import {DateTime} from "luxon"`)

## Configuration

### Modes de génération de l'API client

Il est possible de générer l'API cliente selon 5 modes (`apiMode`) : `fetch` (par défaut), `nuxt`, `angular`, `angular_promise` ou `legacy`.

### Fetch

Par défaut, TopModel génère des appels d'API en utilisant le fetch natif du navigateur. Pour générer l'authentification et la gestion des erreurs, il est fortement conseillé d'utiliser une librairie comme [`ky`](https://github.com/sindresorhus/ky) qui surcharge le fetch natif avec des hooks pour y inclure des traitements à effectuer avant l'envoi des requêtes (pour l'authentification) ou à la réception d'une réponse en erreur.

Vous pouvez surcharger le `fetch` natif en spécifiant `fetchPath` dans la configuration.

_(Remarque : Il s'agit du mode à utiliser pour focus4 12.7+)_

#### Angular

Les modes `angular` et `angular_promise` permettent de générer un service `@Injectable` au sens Angular, contenant les méthodes d'appels à l'API. Les méthodes retournent respectivement des `Observable` de RxJS ou des `Promise`.

Ce mode dispose d'une [page de documentation dédiée](./js/angular.md).

#### Nuxt

Le mode `nuxt` permet de générer des fonctions d'appels à l'API compatibles avec Nuxt 3, utilisant `useAsyncData` et `$fetch` de Nuxt. Les fonctions retournent un objet `AsyncData` qui peut être utilisé directement dans les composants Nuxt.

Exemple de code généré :

```typescript
export function getProfil(
  id: number,
  options: AsyncDataOptions<ProfilDto> = {},
): AsyncData<ProfilDto | null, Error | null> {
  return useAsyncData(
    `/api/profil/${id}`,
    () =>
      $fetch<ProfilDto>(`/api/profil/${id}`, {
        method: "GET",
      }),
    options,
  );
}
```

#### Legacy

Le mode `legacy` permet de générer un fichier TypeScript contenant les méthodes d'appels à l'API exportées sous forme de fonctions. Ce mode nécessite la définition d'une méthode `fetch`. Par défaut, cette méthode est importée de `@focus4/core`, mais il est possible de la surcharger avec le paramètre `fetchPath`.

Exemple :

```yaml
fetchPath: "@api-services"
```

Les APIs générées correspondant au format de Focus pré-12.7.

### Chemins de configuration

#### modelRootPath

Localisation du modèle, relative au répertoire de génération. Si non renseigné, aucun modèle ne sera généré. Si `{module}` n'est pas présent dans le chemin, alors il sera ajouté à la fin.

Par défaut, les fichiers de modèle sont générés dans `{module}/` avec le nom de la classe en kebab-case (ex: `profil-dto.ts`).

#### resourceRootPath

Localisation des ressources i18n, relative au répertoire de génération. Si non renseigné, aucun fichier ne sera généré. Si `{lang}` n'est pas présent dans le chemin, alors il sera ajouté à la fin.

Les fichiers de ressources sont générés par module et par langue (ex: `{module}/{lang}/securite.ts`).

#### apiClientRootPath

Localisation des clients d'API, relative au répertoire de génération. Si non renseigné, aucun fichier ne sera généré.

#### apiClientFilePath

Chemin vers lequel sont créés les fichiers d'endpoints générés, relatif à la racine de l'API (`apiClientRootPath`).

Par défaut, les fichiers d'API client sont placés dans le dossier `{module}`. Ils sont nommés d'après le fichier qui les contient, sauf dans le cas du mode `angular` ou `angular_promise`, où le nom du fichier sera de la forme `{fileName}.service.ts`.

Pour modifier ce comportement, ajuster le paramètre `apiClientFilePath`.

L'extension `.ts` est ajoutée automatiquement.

**Variables disponibles dans les chemins :**

- **`{module}`** : Module de la classe/endpoint (ex : `securite`)
- **`{lang}`** : Langue pour les fichiers de ressources (ex : `fr`, `en`)
- **`{fileName}`** : Nom du fichier pour les endpoints (défini dans la configuration des endpoints)

Exemple :

```yaml
apiClientFilePath: "api/{module}/{fileName}"
```

### Modes de génération des entités

Le paramètre `entityMode` permet de choisir le type de génération pour les entités. Quatre modes sont disponibles :

| Mode      | Description                                                                                   |
| --------- | --------------------------------------------------------------------------------------------- |
| `untyped` | Génère une interface et une définition d'entité sans dépendances externes (**par défaut**)    |
| `none`    | Génère uniquement l'interface TypeScript, sans objet de définition                            |
| `focus`   | Utilise les APIs du module `@focus4/entities` pour générer les entités                        |
| `typed`   | Utilise les types du module `@focus4/entities` sans les APIs (mode legacy - préférer `focus`) |

**Configuration des domaines :** À l'exception du mode `none`, la génération utilise la propriété `domainPath` (par défaut : `../domains`) pour importer les définitions de domaine.

_Remarque : Si vous avez besoin que vos (alias de) clés primaires soient générés comme optionnel pour compatibilité avec l'existant, vous pouvez utiliser le paramètre `optionalPrimaryKeys: true`._

#### Untyped

Le mode `untyped` permet de générer la description des entités métier en tant que **`const`** non typés.

Exemple :

```ts
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

import { DO_CODE, DO_CODE_LIST, DO_ID } from "@domains";

import {
  UtilisateurDtoEntity,
  UtilisateurDto,
} from "../utilisateur/utilisateur-dto";
import { DroitCode, TypeProfilCode } from "./references";
import { SecteurDtoEntity, SecteurDto } from "./secteur-dto";

export interface ProfilDto {
  id: number;
  typeProfilCode?: TypeProfilCode;
  droits?: DroitCode[];
  utilisateurs: UtilisateurDto[];
  secteurs: SecteurDto[];
}

export const ProfilDtoEntity = {
  id: {
    type: "field",
    name: "id",
    domain: DO_ID,
    isRequired: true,
    label: "securite.profil.id",
  },
  typeProfilCode: {
    type: "field",
    name: "typeProfilCode",
    domain: DO_CODE,
    isRequired: false,
    label: "securite.profil.typeProfilCode",
  },
  droits: {
    type: "field",
    name: "droits",
    domain: DO_CODE_LIST,
    isRequired: false,
    label: "securite.profil.droits",
  },
  utilisateurs: {
    type: "list",
    entity: UtilisateurDtoEntity,
  },
  secteurs: {
    type: "list",
    entity: SecteurDtoEntity,
  },
} as const;
```

#### None

Le mode `none` permet de générer uniquement les interfaces TypeScript sans générer les entités. Ce mode est utile lorsque vous n'avez pas besoin des métadonnées des entités.

Exemple :

```ts
export interface ProfilDto {
  id: number;
  typeProfilCode?: TypeProfilCode;
  droits?: DroitCode[];
  utilisateurs: UtilisateurDto[];
  secteurs: SecteurDto[];
}
```

#### Focus

Le mode `focus` permet de générer la description des entités métier en utilisant les APIs et les types exposés par le module NPM `@focus4/entities`, [documenté ici](https://klee-contrib.github.io/focus4/?path=/docs/mod%C3%A8le-m%C3%A9tier-entit%C3%A9s-et-champs--docs). Ce module n'est **pas lié à Focus** et peut être utilisé par **n'importe quel framework JS**.

Il génère des entités sous la forme :

```ts
import {e, entity} from "@focus4/entities";

export const ProfilDtoEntity = entity({
  id: e.field(DO_ID, f => f.optional()
    .label("securite.profil.id")
  ),
  typeProfilCode: e.field(DO_CODE, f => f.optional()
    .label("securite.profil.typeProfilCode")
  ),
  droits: e.field(DO_CODE_LIST, f => f.optional().type<DroitCode[]>()
    .label("securite.profil.droits")
  ),
  utilisateurs: e.list(UtilisateurDtoEntity, f => f
    .label("securite.profilDto.utilisateurs")
  ),
  secteurs: e.list(SecteurDtoEntity, f => f
    .label("securite.profilDto.secteurs")
  )
}
```

Vous pouvez surcharger l'import de `@focus4/entities` avec la propriété `entityTypesPath`.

#### Typed

Le mode `typed` permet de générer la description des entités métier avec les mêmes types que le mode `focus`, mais sans les APIs pour les construire. Les objets sont écrits en JavaScript et TypeScript pur. Il s'agit d'un mode **legacy** : les APIs utilisées par le mode `focus` permettent un résultat identique en étant beaucoup moins verbeux.

Vous pouvez également activer l'option `extendedCompositions` pour générer toutes les propriétés sur les compositions (`label`, `isRequired`, `comment`), qui ne sont pas générées par défaut.

Les types sont importés par défaut de `@focus4/stores`, mais ce chemin peut être surchargé avec la propriété `entityTypesPath`.

### Génération des enums

Les énumérations ne génèrent **pas** de définition d'entité. À la place, elles sont agrégées dans un fichier `enums.ts` par module (relativement au `modelRootPath`). Le nom de ce fichier peut être personnalisé avec le paramètre `enumsFileName`.

Pour chaque classe, on génèrera :

- Un type "enum" (union des valeurs possibles) par propriété enum de la classe (ou pour la classe entière si elle est `enum: true`).
- Le type Typescript "standard" correspondant à la classe (il sera suffixé par `Object` pour une `enum: true`, vu que le nom de la classe est déjà utilisé pour le type "enum")
- Pour une classe `enum: true` ou `readonly: true`, la liste des valeurs de la classe.

Par exemple :

```ts
// Region est une classe enum non readonly.
export type RegionCode = "IDF";
export interface Region {
  code: RegionCode;
  libelle: string;
  nomResponsable?: string;
}

// StatutCommande est une classe `enum: true`.
export type StatutCommande =
  | "ANNULE"
  | "EN_ATT"
  | "EN_PREP"
  | "PRETE"
  | "SERVIE";
export interface StatutCommandeObject {
  code: StatutCommande;
  libelle: string;
}
export const statutCommandeList: StatutCommandeObject[] = [
  {
    code: "EN_ATT",
    libelle: "restaurant.statutCommande.values.EnAttente", // Car `translateReferences: true`.
  },
  {
    code: "EN_PREP",
    libelle: "restaurant.statutCommande.values.EnPreparation",
  },
  {
    code: "PRETE",
    libelle: "restaurant.statutCommande.values.Prete",
  },
  {
    code: "SERVIE",
    libelle: "restaurant.statutCommande.values.Servie",
  },
  {
    code: "ANNULE",
    libelle: "restaurant.statutCommande.values.Annulee",
  },
];
```

### Génération des classes de référence

Si `reference: true` sur une classe, alors un objet décrivant la liste de référence sera généré en plus :

- Pour une classe non enum, ou une classe enum non readonly, il sera de la forme `{type, valueKey, labelKey}`
- Pour une classe `enum: true` ou enum readonly, il sera de la forme `{list, valueKey, labelKey}`

Ces propriétés incluent :

- **`type`** : Type TypeScript de la classe (généré comme `{} as Classe`)
- **`list`** : Liste des valeurs de la classe
- **`valueKey`** : Nom de la propriété de clé (clé primaire ou clé d'unicité équivalente)
- **`labelKey`** : Nom de la `DefaultProperty` de la classe de référence

Exemple :

```ts
// Liste de référence classique.
export const region = {
  type: {} as Region,
  valueKey: "code",
  labelKey: "libelle",
} as const;

// Liste de référence statique.
export const departementList: Departement[] = [
  {
    code: "75",
    libelle: "restaurant.departement.values.Paris",
    regionCode: "IDF",
  },
  {
    code: "92",
    libelle: "restaurant.departement.values.HautsDeSeine",
    regionCode: "IDF",
  },
  {
    code: "93",
    libelle: "restaurant.departement.values.SeineSaintDenis",
    regionCode: "IDF",
  },
  {
    code: "94",
    libelle: "restaurant.departement.values.SeineEtMarne",
    regionCode: "IDF",
  },
];
export const departement = {
  list: departementList,
  valueKey: "code",
  labelKey: "libelle",
} as const;
```

### Modes de génération des fichiers de ressources

Le paramètre `resourceMode` permet de choisir le format de génération des fichiers de ressources (traductions).

#### JS

La génération des ressources se met en mode **JavaScript** lorsque le paramètre `resourceMode` est défini à `js`. Les fichiers générés sont des fichiers TypeScript exportant des objets JavaScript.

Exemple :

```typescript
export const securite = {
  profil: {
    id: "Identifiant",
    typeProfilCode: "Type de profil",
    droits: "Droits",
  },
  profilDto: {
    utilisateurs: "Utilisateurs",
    secteurs: "Secteurs",
  },
};
```

#### JSON

La génération des ressources se met en mode **JSON** lorsque le paramètre `resourceMode` est défini à `json`. Les fichiers générés sont des fichiers JSON standard.

Exemple :

```json
{
  "profil": {
    "id": "Identifiant",
    "typeProfilCode": "Type de profil",
    "droits": "Droits"
  },
  "profilDto": {
    "utilisateurs": "Utilisateurs",
    "secteurs": "Secteurs"
  }
}
```

### Options de génération des ressources

#### generateMainResourceFiles

Génère un fichier `index.ts` qui importe et réexporte tous les fichiers de ressources par langue. **Compatible uniquement avec `resourceMode: js`.**

Exemple de fichier `index.ts` généré :

```typescript
import { securite } from "./securite";
import { common } from "./common";

export { securite, common };
```

#### translateProperties

Gère la traduction des libellés des propriétés :

- **`true` (par défaut)** : Les libellés sont générés dans les fichiers de ressources, et la clé de traduction correspondante est utilisée dans la définition d'entité.
- **`false`** : Les libellés sont utilisés directement.

#### translateReferences

Gère la traduction des listes de références :

- **`true` (par défaut)** : Les libellés des références sont générés dans les fichiers de ressources, et la clé de traduction correspondante est utilisée dans les valeurs (si générées).
- **`false`** : Les libellés sont utilisés directement.

### Génération de commentaires

#### generateComments

Si cette option est activée, un fichier de commentaires sera généré pour chaque module, contenant les commentaires des propriétés. Le fichier sera nommé `{module}.comments.ts` (ou `.json` selon le `resourceMode`) et placé dans le même répertoire que les fichiers de ressources.

Exemple de fichier de commentaires généré :

```typescript
export const securiteComments = {
  profil: {
    id: "Identifiant unique du profil",
    typeProfilCode: "Code du type de profil",
    droits: "Liste des droits associés au profil",
  },
};
```

### Configuration complète

Exemple de configuration complète :

```yaml
javascript:
  outputDirectory: "./src/generated"
  tags:
    - front
  modelRootPath: "./model/{module}"
  resourceRootPath: "./resources/{lang}"
  apiClientRootPath: "./api"
  apiClientFilePath: "{module}/{fileName}"
  apiMode: angular
  entityMode: focus
  resourceMode: js
  domainPath: "../domains"
  fetchPath: "@focus4/core"
  entityTypesPath: "@focus4/entities"
  generateComments: true
  generateMainResourceFiles: true
  translateProperties: true
  translateReferences: true
  extendedCompositions: false
```

### Générateurs disponibles

Le module JavaScript génère plusieurs types de fichiers :

1. **TypescriptDefinitionGenerator** (`JSDefinitionGen`) : Génère les définitions TypeScript des classes (DTOs et entités)
2. **TypescriptEnumsGenerator** (`JSEnumsGen`) : Génère les définitions des listes de références
3. **JavascriptApiClientGenerator** (`JSApiClientGen`) : Génère les clients API en mode fetch
4. **AngularApiClientGenerator** (`JSNGApiClientGen`) : Génère les services Angular pour les clients API
5. **NuxtApiClientGenerator** (`JSApiClientGen`) : Génère les fonctions API pour Nuxt
6. **LegacyApiClientGenerator** (`JSLApiClientGen`) : Génère les clients API en mode legacy (ancienne API Focus)
7. **JavascriptResourceGenerator** (`JSResourceGen`) : Génère les fichiers de ressources (traductions)

Vous pouvez désactiver certains générateurs avec la propriété `disable` :

```yaml
javascript:
  disable:
    - JSResourceGen
```
