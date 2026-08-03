# Endpoints

En plus de pouvoir définir des classes dans le modèle, TopModel permet aussi de définir des endpoints d'API. Cela complète l'approche "model-first" en permettant de décrire des API qui référencent explicitement des classes et des propriétés du modèle. Les générateurs peuvent ensuite utiliser ces descriptions pour produire des clients d'API, ou une "interface" pour les contrôleurs côté serveur.

Exemple d'endpoint :

```yaml
---
endpoint:
  name: GetEvenementAudit
  method: GET
  route: api/evenements-audit/{evaId}
  description: Charge le détail d'un événement d'audit.

  # Liste des paramètres de l'endpoint.
  # Ce sont des propriétés de modèle, comme pour les classes.
  # Il peut ne pas y en avoir.
  params:
    - alias:
        property: Id
        class: EvenementAudit

  # Type de retour de l'endpoint.
  # Ici aussi, il s'agit d'une propriété du modèle et il peut ne pas y en avoir.
  returns:
    composition: EvenementAuditDetail
    name: detail # Le nom est obligatoire car c'est une propriété, mais il n'est pas utilisé.
    comment: Le détail et la liste des impacts.
```

Si un paramètre d'endpoint est un **alias d'une clé primaire**, alors son **nom de paramètre est préfixé par le trigramme** de cette propriété si elle en a un, ou par le nom de sa classe à défaut. Ce préfixe est surchargeable en renseignant `trigram` sur la propriété (y compris avec `""` pour le retirer).

## Localisation des paramètres

Chaque propriété utilisée comme paramètre d'endpoint peut définir sa localisation via la propriété **`paramLocation`**, qui peut valoir :

- `route` : Le paramètre est dans la route.
- `query` : Le paramètre est dans la query.
- `json-body` : Le paramètre est l'unique paramètre du body de la requête, en JSON.
- `form-data` : Le paramètre est dans le body, de type `multipart/form-data`. Pour une composition, chaque propriété de l'objet cible est insérée individuellement dans le body.

Si `paramLocation` n'est pas renseigné, sa valeur est déterminée automatiquement selon les règles suivantes :

- `json-body` pour les compositions sans autre paramètre `form-data`.
- `form-data` pour les compositions avec un autre paramètre `form-data`.
- `query` pour le reste.

`paramLocation` peut également être renseigné au niveau du domaine, afin d'être appliqué par défaut à tous les paramètres qui utilisent ce domaine. Cette valeur peut toujours être surchargée sur la propriété.

Quelques remarques :

- Un paramètre `route` doit nécessairement être dans la route, et un paramètre utilisé dans la route doit nécessairement correspondre à un paramètre `route` existant. Par conséquent, l'information explicite `paramLocation: route` est toujours soit redondante, soit invalide.
- Un endpoint ne peut avoir qu'un seul body, par conséquent :
  - Soit il s'agit d'une seule propriété `json-body`
  - Soit il s'agit de plusieurs propriétés `form-data`.

  TopModel s'assurera que cette règle est respectée.

- Les paramètres `route` et `json-body` sont toujours obligatoires, peu importe la valeur de `required`.

## Fichier d'endpoint

Tous les générateurs produisent **un fichier client ou serveur par fichier de modèle contenant des endpoints**. Ce fichier reflète le chemin et le nom du fichier de modèle concerné. Contrairement aux générateurs de classes, le module n'est pas pris en compte ici.

Il est possible de paramétrer le nom du fichier généré, ainsi que d'ajouter un préfixe aux routes. Pour cela, dans les métadonnées du fichier (au niveau de `module`, `tags`, `uses`, etc.), vous pouvez ajouter des options :

```yaml
---
module: Securite.Utilisateur
tags:
  - dto
uses:
  - Securite/Utilisateur/02_Entities
  - Securite/Utilisateur/03_Dtos
options:
  endpoints:
    prefix: utilisateur
    fileName: UtilisateurApi
```

Ainsi, toutes les routes décrites dans ce fichier auront le préfixe `utilisateur`. Le fichier généré se nommera `UtilisateurApi` (éventuellement complété du suffixe du générateur utilisé, par exemple `Controller` ou `Client`).

**Si des fichiers de modèle d'un même module ont le même nom** (que ce soit le vrai nom de fichier dans des dossiers différents, ou une surcharge comme décrite précédemment), alors **les endpoints générés pour ces fichiers sont regroupés dans le même fichier cible**, pour tous les générateurs clients et serveurs.

Une erreur est levée si des fichiers de même nom ne définissent pas le même préfixe de route. De même, deux endpoints d'un même fichier cible ne peuvent pas avoir le même nom.

## Tags d'un endpoint

Un endpoint peut également définir ses propres tags, via la propriété `tags`. Ils s'ajoutent alors aux tags du fichier, pour plus de flexibilité dans l'organisation des endpoints en fichiers. Par exemple, s'il n'y a qu'un seul endpoint dans un fichier qui doit être pris en compte par un autre générateur, il est possible d'ajouter un tag directement sur cet endpoint au lieu de le déplacer dans un autre fichier.
