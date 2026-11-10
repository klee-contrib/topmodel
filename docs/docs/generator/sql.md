# SQL Generator

## Présentation

### SQL

Le générateur SQL en mode `procedural` peut générer les fichiers suivants :

- Un fichier de définition des tables pour toutes les classes persistées du modèle.
- Un fichier de définition des contraintes de clés étrangères, d'unicité, et les indexes du modèles.
- Un fichier d'insertion des valeurs pour toutes les classes persistées du modèle qui ont des `values`.
- Un fichier d'insertion des traductions dans les tables de [traduction](/model/classes#classe-de-traductions) (si `translateProperties` ou `translateReferences` sont à `true`).
- Un fichier de définition des types toutes les classes persistées du modèle, ainsi que pour leurs colonnes (`sqlserver` uniquement).
- Un fichier d'insertion des commentaires pour toutes les classes persistées du modèle, ainsi que pour leurs colonnes (`postgresql` uniquement).

Les 5 premiers fichiers seront générés automatiquement selon les besoins du modèle et de la configuration (les fichiers ont des noms par défaut), tandis que le fichier de commentaire ne sera généré que si son nom est renseigné dans la configuration.

### SSDT

Le générateur SQL en mode `ssdt` peut générer les fichiers suivants :

- Un fichier par classe
- Un fichier par type de table SQL, pour les tables qui en ont besoin (si propriété `InsertKey` présente)
- Un fichier par liste de référence à initialiser
- Le fichier d'initialisation des listes de référence, qui appelle, dans l'ordre, tous les fichiers d'initialisation

## Configuration

### `procedural`

Config pour la génération de SQL procédural.

#### `tablesFileName`

Nom du fichier contenant le script de création de tables.

_Valeur par défaut_ : `01_tables.sql`

#### `indexesAndKeysFileName`

Nom du fichier contenant le script de création des indexes et des clés étrangères et uniques.

_Valeur par défaut_ : `02_indexes_and_keys.sql`

#### `valuesFileName`

Nom du fichier contenant le script d'insertion des valeurs initiales.

_Valeur par défaut_ : `03_values.sql`

#### `resourcesFileName`

Nom du fichier contenant le script d'insertion des ressources (libellés traduits).

_Valeur par défaut_ : `04_resources.sql`

#### `typesFileName`

Nom du fichier contenant le script de création de types (pour SQL Server).

_Valeur par défaut_ : `05_types.sql`

#### `commentsFileName`

Nom du fichier contenant le script de création de commentaires sur les tables et les colonnes.

### `ssdt`

Config pour la génération SSDT.

#### `tableScriptFolder`

Dossier du projet pour les scripts de déclaration de table.

#### `tableTypeScriptFolder`

Dossier du projet pour les scripts de déclaration de type table.

#### `initListScriptFolder`

Dossier du projet pour les scripts d'initialisation des listes de références.

#### `initListMainScriptName`

Fichier du projet référençant les scripts d'initialisation des listes de références.

#### `disableIdentity`

Désactive les colonnes d'identité.

_Valeur par défaut_ : `false`

#### `generateComments`

Génère les commentaires SQL sur les tables et les colonnes.

_Valeur par défaut_ : `false`

### `tableTablespace`

Nom du tablespace pour les tables (Postgres ou Oracle).

### `indexTablespace`

Nom du tablespace pour les index (Postgres ou Oracle).

### `targetDBMS`

SGBD cible de la génération.

_Valeurs possibles_ : `sqlserver`, `postgre`, `oracle`

_Valeur par défaut_ : `postgre`

### Exemple

```yaml
sql:
  - tags:
      - back
    outputDirectory: ./src
    targetDBMS: postgre
    procedural:
      tablesFileName: 01_tables.sql
      indexesAndKeysFileName: 02_indexes_and_keys.sql
      valuesFileName: 03_values.sql
      resourcesFileName: 04_resources.sql
      typesFileName: 05_types.sql
      commentsFileName: 06_comments.sql
    ssdt:
      tableScriptFolder: Tables
      tableTypeScriptFolder: TableTypes
      initListScriptFolder: Values
      initListMainScriptName: Values.sql
      disableIdentity: false
      generateComments: true
```
