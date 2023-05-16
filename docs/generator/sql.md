# SQL Generator

## Présentation

### SQL

Le générateur SQL en mode `procedural` peut générer les fichiers suivants :

- Un fichier de définition des tables pour toutes les classes persistées du modèle.
- Un fichier de définition des contraintes de clé étrangère (et des indexes) des associations du modèle.
- Un fichier de définition des contraintes d'unicité du modèle.
- Un fichier d'insertion des valeurs pour toutes les classes persistées du modèle qui ont des `values`.
- Un fichier d'insertion des commentaires pour toutes les classes persistées du modèle, ainsi que pour leurs colonnes (`postgresql` uniquement).
- Un fichier de définition des types toutes les classes persistées du modèle, ainsi que pour leurs colonnes (`sqlserver` uniquement).

### SSDT

Le générateur SQL en mode `ssdt` peut générer les fichiers suivants :

- Un fichier par classe
- Un fichier par type de table SQL, pour les tables qui en ont besoin (si propriété `InsertKey` présente)
- Un fichier par liste de référence à initialiser
- Le fichier d'initialisation des listes de référence, qui appelle, dans l'ordre, tous les fichiers d'initialisation

## Configuration

### Fichier de configuration

- `targetDBMS`
  Système de gestion de base de données cible (`"postgre"` ou `"sqlserver"`).
- `procedural`

  Options de génération sql

  - `crebasFile`
    Nom du fichier de création des tables.
  - `indexFKFile`
    Nom du fichier de création des contraintes de clés étrangères et des indexes.
  - `uniqueKeysFile`
    Nom du fichier de création des contraintes d'unicité.
  - `initListFile`
    Nom du fichier d'insertion des listes de références.
  - `commentFile`
    Nom du fichier d'insertion des commentaires.
  - `typeFile`
    Nom du fichier de création des types.

- `ssdt`

  Options de génération ssdt
  - `tableScriptFolder`
    Dossier du projet pour les scripts de déclaration de table.
  - `tableTypeScriptFolder`
    Dossier du projet pour les scripts de déclaration de type table
  - `initListScriptFolder`
    Dossier du projet pour les scripts d'initialisation des listes de références.
  - `initListMainScriptName`
    Fichier du projet référençant les scripts d'initialisation des listes de références
  - `disableIdentity`
    Désactive les colonnes d'identité.

- `identity`

  Options de génération de la séquence

  - `mode`

    Mode de génération de la persistence (`"none"` ou `"sequence"` ou `"identity"`).

    _Valeur par défaut_: `identity`

  - `increment`

    Incrément de la séquence générée.

  - `start`

    Début de la séquence générée.

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
      identity:
        increment: 50
        start: 1000
        mode: sequence
```
