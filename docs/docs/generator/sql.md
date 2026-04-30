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

_Remarque : A l'inverse de tous les autres générateurs, le générateur SQL est configuré avec `ignoreDefaultValues: true` par défaut_

### Fichier de configuration

- `targetDBMS` : Système de gestion de base de données cible (`"postgre"` ou `"sqlserver"` ou `"oracle"`).
- `procedural`

  Options de génération sql
  - `tablesFileName` : Nom du fichier contenant le script de création de tables. Par défaut : `01_tables.sql`.
  - `indexesAndKeysFileName` : Nom du fichier contenant le script de création des indexes et des clés étrangères et uniques. Par défaut : `02_indexes_and_keys.sql`.
  - `valuesFileName` : Nom du fichier contenant le script d'insertion des valeurs initiales. Par défaut : `03_values.sql`.
  - `resourcesFileName` : Nom du fichier contenant le script d'insertion des ressources (libellés traduits). Par défaut : `04_resources.sql`.
  - `typesFileName` : Nom du fichier contenant le script de création de types (pour SQL Server). Par défaut : `05_types.sql`.
  - `commentsFileName` : Nom du fichier contenant le script de création de commentaires sur les tables et les colonnes. Pas de valeur par défaut.

- `ssdt`

  Options de génération ssdt
  - `tableScriptFolder` : Dossier du projet pour les scripts de déclaration de table.
  - `tableTypeScriptFolder` : Dossier du projet pour les scripts de déclaration de type table
  - `initListScriptFolder` : Dossier du projet pour les scripts d'initialisation des listes de références.
  - `initListMainScriptName` : Fichier du projet référençant les scripts d'initialisation des listes de références
  - `disableIdentity` : Désactive les colonnes d'identité.
  - `generateComments` : Génère les commentaires pour les tables et les colonnes dans les fichiers de table.

- `identity`

  Options de génération de la séquence
  - `mode`

    Mode de génération de la persistence (`"none"` ou `"sequence"` ou `"identity"`).

    _Valeur par défaut_: `identity`

    En mode `sequence`, si une classe possède des `values` mais dont la PK n'est pas précisée, le générateur inclut la colonne PK dans l'`INSERT` et l'initialise via la séquence de la table.

  - `increment`

    Incrément de la séquence générée.

  - `start`

    Début de la séquence générée.

- `resourcesTableName`

  Nom de la table contenant les traductions

- `tableTablespace`

  Nom du tablespace pour les tables (Postgres ou Oracle).

- `indexTablespace`

  Nom du tablespace pour les index (Postgres ou Oracle).

### Exemple

```yaml
sql:
  - tags:
      - back
    outputDirectory: ./src
    targetDBMS: postgre
    procedural:
      crebasFile: 01_tables.sql
      indexFKFile: 02_fk_indexes.sql
      uniqueKeysFile: 03_unique_keys.sql
      initListFile: 04_references.sql
      commentFile: 05_comments.sql
      resourceFile: 06_resources.sql
      identity:
        increment: 50
        start: 1000
        mode: sequence
```
