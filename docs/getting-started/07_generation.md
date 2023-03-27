# Générer du code

La modélisation TopModel est le point d'entrée de son générateur `modgen`. L'outil va lire le modèle ainsi que le fichier de configuration associé, puis lancer les différents générateurs paramétrés.

## Installer `modgen`

`TopModel.Generator` est une application .NET 6, packagée comme un [outil .NET](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools).

Pour l'utiliser, il faut avoir le [SDK .NET](https://dotnet.microsoft.com/download) installé sur votre machine, puis lancer la commande

```bash
dotnet tool install --global TopModel.Generator
```

Par la suite, pour mettre à jour TopModel, utiliser la commande :

```bash
dotnet tool update --global TopModel.Generator
```

## Système de filtre

Pour affiner le paramétrage des générateurs, `modgen` utilise un système de filtre par fichier.

Dans les données d'en-tête de chaque fichier, où sont précisés les imports (`uses`) et le module, le paramètre `tags` permet d'ajouter au fichier une liste de mots clés relatifs à ce fichier.

Exemple dans le fichier `Utilisateur.yml` :

```yaml
---
uses:
  - Decorators
  - 01_References
module: Utilisateur
tags:
  - entity
```

Lorsque nous ajoutons un générateur, nous lui attribuons également une liste de tags.

Exemple dans le fichier `topmodel.config` :

```yaml
sql:
  - tags:
      - entity
```

Lors de la génération, `modgen` enverra, à chacun des générateurs paramétrés, la liste des fichiers **dont au moins un tag correspond**.

Classiquement

- `entity` est utilisé pour les fichiers contenant les classes persistées
- `dto` est utilisé pour les fichiers contenant les classes non persistées
- Les fichiers contenant les `endpoints` contiennent les deux tags pour que soient générées les API clientes et server

## Générer du 'SQL' (postgresql)

Ajoutons tout d'abord un générateur SQL à notre application.

Dans le fichier de configuration `topmodel.config`, nous pouvons ajouter le générateur `proceduralSql`, en indiquant les fichiers de destination de la génération :

```yaml
---
app: sample # Nom de l'application
sql: # Nom du générateur
  - tags: # Liste des tags des fichiers à filtrer pour ce paramétrage
      - entity # Tag des fichiers contenant des classes persistées
```

### Scripts de créations des tables

Ajoutons au générateur postgres la configuration permettant de générer les fichiers de création de table, des indexes et des clés d'unicités

```yaml
---
app: sample
sql:
  - tags:
      - entity
    outputDirectory: ./pg/model/
    targetDBMS: postgre
    procedural:
      crebasFile: 01_crebas.sql
      indexFKFile: 02_index-fk.sql
      uniqueKeysFile: 03_uniq.sql
```

Dans le répertoire du projet, contenant le fichier `topmodel.config`, lancer la commande `modgen`.

Dans les logs, vous pouvez observer :

- Le numéro de la version TopModel utilisé
- La liste des Watchers enregistrés (ici un seul, portant l'identifiant `ProceduralSqlGen@1`)
- L'ensemble des fichiers créés, modifiés ou supprimés

Les fichiers attendus, `01_crebas.sql`, `02_index-fk.sql`, `03_uniq.sql` ont été créés, dans les dossiers configurés.

### Scripts d'insertion des listes de référence

Bien qu'il soit possible de se contenter de compléter la configuration précédente, nous allons, pour l'exemple , ajouter un nouveau générateur postgresql pour générer les scripts d'initialisation des listes de référence.

Ajoutons donc un élément à la liste des générateurs posgresql :

```yaml
---
app: sample
proceduralSql:
  - tags:
      - entity
    outputDirectory: ./pg/model/
    targetDBMS: postgre
    procedural:
      crebasFile: 01_crebas.sql
      indexFKFile: 02_index-fk.sql
      uniqueKeysFile: 03_uniq.sql
  - tags:
      - entity
    outputDirectory: ./pg/model/
    targetDBMS: postgre
    procedural:
      initListFile: 04_references.sql
```

Le fichier d'initialisation des listes de référence est créé.

Dans les logs, vous pouvez observer :

- Les autres fichiers, inchangés, n'apparaissent pas
- Deux watchers ont été enregistrés, `ProceduralSqlGen@1` et `ProceduralSqlGen@2`

Nous avons donc ajouté un générateur à notre modèle, puis généré le code correspondant.

## Génération en continu

Dans le répertoire du projet, contenant le fichier `topmodel.config`, lancer la commande `modgen --watch` pour lancer la génération en continu. Ainsi, à chaque modification effectuée dans l'un des fichiers du modèle, la génération est relancée automatiquement !

## Générateurs disponibles

Voir la page [Génération](/generator.md) pour la liste des générateurs disponibles, ainsi que la documentation spécifique de chacun des générateurs pour connaître les paramétrages possibles.
