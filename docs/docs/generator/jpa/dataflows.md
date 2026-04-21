# Génération des flux de données (dataflows)

| Nom                 | Condition d'activation | Objets ciblés | Fichiers générés                                                                                                                             |
| ------------------- | ---------------------- | ------------- | -------------------------------------------------------------------------------------------------------------------------------------------- |
| `SpringDataFlowGen` | `dataFlowsPath` défini | Dataflows     | Définition d'un job par module, et d'un step par dataFlow. Peut également générer une interface à implémenter pour les sources en mode `partial` et les `hook` ajoutés |

Le générateur de data flow s'appuie sur `spring-batch`. Il permet de générer du code permettant de récupérer des données d'une source, appliquer éventuellement une transformation, puis les insérer dans une base de données. Les outils mis en œuvre ont été sélectionnés pour leur capacité à traiter un grand nombre de données, avec les meilleures performances possibles.

> Il est recommandé de maîtriser le fonctionnement de `spring-batch` avant de tenter de générer des flows avec `TopModel`.

## Fichiers générés

### Flow

Le générateur crée un fichier par dataFlow, comprenant :

- **Reader** : Lit les données depuis la source (base de données, API, etc.)
- **Writer** : Écrit les données dans la base de données cible
- **TruncateTasklet** (éventuellement) : Vide la table cible avant l'insertion si configuré
- **Step** : Définit une étape du job Spring Batch
- **Flow** : Définit le flux de traitement des données

La génération s'appuie sur Spring Batch, mais aussi la librairie `spring-batch-bulk`, qui permet des performances exceptionnelles grâce à l'utilisation du bulk insert PostgreSQL (avec la commande `COPY`).

```xml
<dependency>
  <groupId>io.github.kleecontrib</groupId>
  <artifactId>spring-batch-bulk</artifactId>
  <version>0.0.3</version>
</dependency>
```

### Reader

Le reader privilégié est le reader `JdbcCursorItemReaderBuilder`. Il permet d'obtenir les meilleures performances, et offre une meilleure flexibilité (choix de la source de données, requête).

Avec le mode `partial`, le reader n'est pas généré. Il faut donc fournir un `bean` dont le nom est `[Nom du flow]Reader` pour que le job fonctionne.

Il est par exemple possible de créer un `Reader` appelant une API.

#### Replace et HardReplace

Le truncate se fait avec la classe `QueryTasklet` de la librairie `spring-batch-bulk`. Cette approche est nettement plus performante qu'un `deleteAll` classique. Le mode `HardReplace` ajoute `CASCADE` à la commande `TRUNCATE`.

### Processor

Si la classe source et la classe cible sont différentes, un processor est ajouté pour appeler le mapper de l'une vers l'autre.

### Writer

Il existe deux modes de génération des writers : `jpa` ou `bulk`. Le mode est configuré via la propriété `dataFlowsWriter` dans la configuration.

#### JPA

Le writer utilise le `JpaItemWriter` de Spring Batch. Ce mode est adapté pour des volumes de données modérés et offre une meilleure compatibilité avec les fonctionnalités JPA (cascades, listeners, etc.).

#### Bulk

Les writers utilisent le `PgBulkWriter` de la librairie `spring-batch-bulk`. Ce mode offre des performances exceptionnelles grâce à l'utilisation du bulk insert PostgreSQL (avec la commande `COPY`). Il est recommandé pour traiter de très gros volumes de données.

**Configuration :**

```yaml
jpa:
  - tags:
      - entity
    dataFlowsPath: topmodel/exemple/flows
    dataFlowsWriter: bulk # ou jpa
    dataFlowsBulkSize: 100000 # Taille des chunks pour le bulk insert (par défaut: 100000)
```

Le générateur supporte plusieurs stratégies d'insertion selon le type de dataflow défini dans le modèle :

##### Insert

Le writer copie directement les données dans la table cible. TopModel génère le mapping permettant de faire cette insertion.

##### Replace

Le writer vide d'abord la table cible (`TRUNCATE`), puis copie les données. TopModel génère le mapping permettant de faire cette insertion.

##### HardReplace

Le writer vide d'abord la table cible avec un `TRUNCATE CASCADE`, puis copie les données. Cette option supprime également les données des tables liées par clé étrangère. TopModel génère le mapping permettant de faire cette insertion.

##### Merge (Upsert)

Le writer copie les données dans une table temporaire, puis recopie les données de table à table. En cas de conflit sur la clé primaire, un update est effectué. TopModel génère le mapping permettant de faire cette insertion.

##### MergeDisable

Le writer effectue une fusion des données comme pour `Merge`, puis désactive (bulk update) les données non matchées dans la table cible.

### Listeners

Il est possible d'ajouter des listeners aux dataflows via la propriété `dataFlowsListeners`. Ces listeners seront appelés aux différents hooks du flow (beforeFlow, afterFlow, etc.).

**Configuration :**

```yaml
jpa:
  - tags:
      - entity
    dataFlowsPath: topmodel/exemple/flows
    dataFlowsListeners:
      - topmodel.exemple.listeners.CustomFlowListener
```

### Job

Le générateur crée un fichier de configuration de job par module. Ce job ordonnance les lancements des flows selon ce qui a été paramétré avec les mots-clés `dependsOn`. Il importe les configurations nécessaires à son bon fonctionnement.

## Limitations et mises en garde

- **Source de données** : Ne fonctionne que de base à base par défaut. Pour créer un reader spécifique (par exemple appelant une API), utiliser le mode `partial`
- **Base de données cible** : La base cible ne peut être qu'une base de données PostgreSQL
- **Schéma** : Il est obligatoire de définir un `dbSchema`
- **Multi-source** : Non supporté actuellement
- **Mappers** : Un mapper doit exister de la classe source vers la classe cible (sauf s'il s'agit de la même classe)
- **Dépendances entre jobs** : Deux jobs ne peuvent pas dépendre l'un de l'autre s'ils ne sont pas dans le même module
- **Ordre d'exécution** : L'ordre d'exécution des flows suit une logique de dépendances qui peut être plus restrictive que nécessaire. Par exemple, si :
  - Le flow C dépend de A et B
  - Le flow D dépend de A
  - Alors D ne se lancera qu'après A et B (alors qu'en théorie il pourrait se lancer directement après A)

## Dépendances

Pour les DataFlows, les dépendances Spring Batch sont nécessaires :

```xml
<!-- https://mvnrepository.com/artifact/org.springframework.batch/spring-batch-core -->
<dependency>
    <groupId>org.springframework.batch</groupId>
    <artifactId>spring-batch-core</artifactId>
</dependency>
```

Si vous utilisez le mode `dataFlowsWriter: bulk`, la librairie `spring-batch-bulk` est également nécessaire :

```xml
<!-- Source: https://mvnrepository.com/artifact/io.github.klee-contrib/spring-batch-bulk -->
<dependency>
    <groupId>io.github.klee-contrib</groupId>
    <artifactId>spring-batch-bulk</artifactId>
    <version>0.0.7</version>
</dependency>
```

## Configuration

### `dataFlowsPath`

Localisation des flux de données générés. Cette variable doit être renseignée pour que les flux soient générés.

Le chemin des fichiers cibles sera calculé en remplaçant les `.` et le `:` par des `/` dans cette valeur, tandis que le nom du package des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

_Templating_: `{app}`, `{module}`

_Variables par tag_: **oui** (plusieurs flux de données pourraient être générés si un fichier a plusieurs tags)

### `dataFlowsWriter`

Writer à utiliser pour les flux de données. Les valeurs possibles sont :

- `jpa` : Utilise le `JpaItemWriter` de spring-batch (par défaut). Adapté pour des volumes de données modérés et offre une meilleure compatibilité avec les fonctionnalités JPA (cascades, listeners, etc.)
- `bulk` : Utilise le `PgBulkWriter` de la librairie `spring-batch-bulk` pour des performances optimales. Recommandé pour traiter de très gros volumes de données grâce à l'utilisation du bulk insert PostgreSQL (avec la commande `COPY`)

_Valeur par défaut_: `jpa`

### `dataFlowsBulkSize`

Taille des chunks à extraire et insérer lors de l'utilisation du mode `bulk` pour les flux de données. Cette valeur détermine le nombre d'enregistrements traités par batch lors des opérations d'insertion en masse.

_Valeur par défaut_: `100000`

### `dataFlowsListeners`

Liste des listeners à ajouter aux dataflows. Ces listeners seront appelés aux différents hooks du flow (beforeFlow, afterFlow, etc.).

_Valeur par défaut_: `[]`

**Exemple :**

```yaml
jpa:
  - tags:
      - entity
    dataFlowsPath: topmodel/exemple/flows
    dataFlowsListeners:
      - topmodel.exemple.listeners.CustomFlowListener
      - topmodel.exemple.listeners.AnotherListener
```
