# JS Generator

_Remarque : tous les imports spécifiés pour le générateur JS dans les domaines et la configuration seront relatifs au répertoire de génération (`outputDirectory`) s'ils commencent par un `.` (exemple : `./common` ou `../domains`). Ils seront considérés comme des imports de modules sinon (exemple : `@/domains` ou `luxon`). Les imports dans les implémentations de domaines doivent aussi inclure l'objet importé dans leur chemin (exemple, pour avoir `import {DateTime} from "luxon"`, il faut écrire `luxon/DateTime`)._

## Configuration

### Modes de génération de l'API client

Il est possible de générer l'API cliente selon deux modes (`apiMode`) : `vanilla`, `nuxt` ou `angular`.

#### Angular

Le mode `angular` permet de générer un service injectable au sens `Angular`, contenant les méthodes d'appels à l'API.

##### Observe

Le générateur Angular détecte automatiquement le type de retour de l'endpoint pour configurer l'option `observe` de la requête HTTP :

- **`HttpResponse<TonObjet>`** : Si le type de retour est `HttpResponse<UnObjet>`, le générateur ajoutera automatiquement `observe: 'response'` à la requête. Cela permet d'accéder à l'objet `HttpResponse` complet, incluant les headers et le status code, en plus du body.

- **`HttpEvent<>`** : Si le type de retour est `HttpEvent<>`, le générateur ajoutera automatiquement `observe: 'events'` à la requête.

- **Par défaut** : Pour tous les autres types de retour, l'option `observe: 'body'` sera ajoutée, ce qui retourne uniquement le body de la réponse.

#### Vanilla

Le mode `vanilla` permet de générer un fichier ts, contenant les méthodes d'appels à l'API exportées sous forme de fonctions. Ce mode nécessite la définission d'une méthode `fetch`. Par défaut, cette méthode est importée de `focus4/core`, mais il est possible de la surcharger avec le paramètre `fetchPath`.

exemple :

```yaml
fetchPath: "@api-services"
```

### ApiFilePath

Par défaut, les fichiers d'api client sont placés dans le dossier `{module}`. Ils sont nommés d'après le fichier qui les contient, sauf dans le cas du mode `angular`, où le nom du fichier sera de la forme `{fileName}.service.ts`.

Pour modifier ce comportement, ajuster le paramètre `apiFilePath`.

L'extension `.ts` est ajoutée automatiquement

### Modes de génération des entités

Il est possible de générer les entités selon trois modes (`entityMode`) :

- `focus` : génération avec les APIs du module `@focus4/entities`.
- `typed` : génération du DTO et de l'entité, avec typage du DTO via l'entité.
- `untyped` : génération du DTO et de l'entité, sans typage du DTO via l'entité.
- `none` : génération du DTO uniquement.

Dans les deux premiers cas, la génération utilise le chemin défini dans la propriété `domainPath`, pour importer les objets de définition de domaine.

Par défaut `domainPath` vaut `../domains`

#### Focus

Le mode `focus` permet de générer la description des entités métier en utilisant les APIs et les types exposés par le module NPM `@focus4/entities`, [documenté ici](https://klee-contrib.github.io/focus4/?path=/docs/mod%C3%A8le-m%C3%A9tier-entit%C3%A9s-et-champs--docs). Ce module n'est **pas lié à Focus** et peut être utilisé par **n'importe quel framework JS**.

Il génère des entitiés sous la forme :

```ts
import {e, entity} from "@focus4/entities";

export const ProfilDtoEntity = entity({
  id: e.field(DO_ID, f => f.optional()
    .label("securite.profil.id")
  ),
  typeProfilCode: e.field(DO_CODE, f => f.optional()
    .label(securite.profil.typeProfilCode")
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

Le mode `typed` permet de générer la description des entités métier avec les mêmes types que le mode `Focus`, mais sans les APIs pour les construire. Les objets sont écrits en JS et en TS pur. Il s'agit d'un mode "legacy", car il faut mieux utiliser le mode `Focus` si on veut des objets typés.

Vous pouvez également activer l'option `extendedCompositions` pour générer toutes les propriétés sur les compositions (`label`, `isRequired`, `comment`), qui ne sont pas générées par défaut.

Les types sont importés par défaut de `@focus4/stores`, mais ce chemin peut être surchargé avec la propriété `entityTypesPath`.

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
  id?: number;
  typeProfilCode?: TypeProfilCode;
  droits?: DroitCode[];
  utilisateurs?: UtilisateurDto[];
  secteurs?: SecteurDto[];
}

export const ProfilDtoEntity = {
  id: {
    type: "field",
    name: "id",
    domain: DO_ID,
    isRequired: false,
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

### Modes de génération des fichiers de ressource

#### JS

La génération des listes de références se met en mode **JavaScript** lorsque le paramètre `resourceMode` est défini à `js`.

#### JSON

La génération des listes de références se met en mode **JSON** lorsque le paramètre `resourceMode` est défini à `json`.

### Modes de génération des listes de références

#### Définition (défaut)

La génération des listes de références se met en mode **Définition** lorsque le paramètre `referenceMode` est défini à `definition`.

Ce mode permet de générer, pour chaque liste de référence, une définition de ses propriétés (laquelle représente la clé primaire, laquelle représente le texte à afficher).

Exemple :

```typescript
export type TypeProfilCode = "ADM" | "GES";
export interface TypeProfil {
  code: TypeProfilCode;
  libelle: string;
}
export const typeProfil = {
  type: {} as TypeProfil,
  valueKey: "code",
  labelKey: "libelle",
} as const;
```

#### Valeurs

La génération des listes de références se met en mode **Valeurs** lorsque le paramètre `referenceMode` est défini à `values`.

Il permet de générer l'ensemble des valeurs de chaque liste de référence telles que définies dans la propriété `values`. Ce mode est préconisé pour les listes de références statiques uniquement.

Exemple :

```typescript
export type TypeProfilCode = "ADM" | "GES";

export interface TypeProfil {
  code: TypeProfilCode;
  libelle: string;
}

export const typeProfilList: TypeProfil[] = [
  {
    code: "ADM",
    libelle: "typeProfil.values.ADM",
  },
  {
    code: "GES",
    libelle: "typeProfil.values.GES",
  },
];
```

Si le paramètre `translateReferences` est passé à `false`, les clés de traductions ci-dessus seront remplacées par les libellés correspondant.
