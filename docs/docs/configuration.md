# Configuration

Pour démarrer votre projet TopModel, vous devez d'abord écrire un fichier de configuration. Celui-ci contient notamment :

- Le nom de l'application
- Le répertoire racine des fichiers de modèle
- La configuration des modules de générateurs
- Un système de filtre (tags) pour la sélection des générateurs par langage sur lesquels TopModel sera utilisé

## Fichier de configuration

Le fichier de configuration doit s'appeler `topmodel.config` ou `topmodel.[NOM DE L'APPLICATION].config`. Il s'agit d'un fichier YAML comme le reste du modèle (et qui supporte les [ancres YAML](#gestion-des-ancres-yaml)).

### Exemple minimal

```yaml
# topmodel.config
---
app: Hello World
```

### Exemple complet

```yaml
# topmodel.config
---
app: MonApplication
modelRoot: ./model

csharp:
  - name: backend
    tags: [Backend]
    outputDirectory: ./src/Backend/Generated
    nullableEnable: true

jpa:
  - name: api
    tags: [API]
    outputDirectory: ./src/main/java/com/example/generated
```

## Modules de générateurs

Chaque module de générateurs définit sa propre configuration. Il est possible d'instancier **plusieurs fois** le même module, avec des configurations différentes, afin de pouvoir générer du code dans plusieurs applications mais à partir du même modèle par exemple.

### Modules disponibles

TopModel fournit les modules de générateurs suivants :

- **`csharp`** : Module de générateurs C# (Entity Framework Core, API, etc.)
- **`jpa`** : Module de générateurs Java/JPA
- **`javascript`** : Module de générateurs JavaScript/TypeScript (Angular, React, etc.)
- **`sql`** : Module de générateurs SQL (PostgreSQL, Oracle, SQL Server, etc.)
- **`documentation`** : Module de génération de documentation
- **`translation`** : Module de générateurs de traductions manquantes

Chaque module a son propre nom (par exemple `csharp` pour le module de générateurs C#, `jpa` pour le module de générateurs Java/JPA), qui sera le nom de la propriété à utiliser dans le fichier de configuration, et qui prendra une liste de configurations.

Pour plus de détails sur chaque module, consultez la [page dédiée à la génération](./generator).

### Propriétés communes

Les schémas JSON des modules définissent un socle commun de propriétés. `tags` et `outputDirectory` sont obligatoires pour les modules de génération. Les autres propriétés disponibles peuvent varier selon le module ; les options spécifiques sont décrites dans les pages dédiées.

#### Propriétés obligatoires

- **`tags`**

  Tags pris en compte par cette configuration.

  Ce champ est **obligatoire**. L'exclusion de tags est gérée par l'option de ligne de commande `--exclude` (ou `-e`), et non par une propriété de cette configuration YAML.

- **`outputDirectory`**

  Racine du répertoire de génération.

  Ce champ est **obligatoire**.

#### Propriétés optionnelles

- **`name`**

  Nom de cette configuration.

  _Valeur par défaut_ : `{module}@{index}`

  Il peut être utilisé dans les messages d'erreur ou pour référencer cette configuration avec `referencedTags`. S'il n'est pas renseigné, il est défini automatiquement sous la forme `{module}@{index}`, par exemple `csharp@1`.

- **`language`**

  Langages de la configuration, utilisés en cascade pour choisir l'implémentation correspondante des domaines, décorateurs et convertisseurs.

  _Valeur par défaut_ : `csharp` pour `csharp`, `java` pour `jpa`, `ts` pour `javascript`, et `sql` pour `sql` et `documentation`

  _Variables par tag_ : **oui**

  Il doit correspondre à l'un des langages définis sur les domaines, décorateurs, annotations et converters du modèle. Le module `translation` n'expose pas cette option. Si plusieurs langages sont spécifiés, les implémentations sont recherchées par ordre de priorité.

  `language` supporte également les variables par tag : vous pouvez donc vous en servir pour définir des surcharges d'implémentation pour un tag en particulier, par exemple :

  ```yaml
  # Le domaine :
  domain:
    name: DO_LISTE
    csharp:
      genericType: ICollection<{T}>
      collector: ToList()
    csharp-client:
      genericType: IList<{T}>
      collector: ToList()

  # Dans la config C# :
  tagVariables:
    api-client:
      language: csharp-client
    language: ["{language}", csharp]

  # Ainsi, les fichiers taggués "api-client" utiliseront l'implémentation "csharp-client" si elle a été définie
  # Remarque : Si la variable `language` n'est pas définie, elle est remplacée par une chaîne vide qui ne correspond pas à un langage existant, et est donc ignorée.
  ```

- **`referencedTags`**

  Tags d'autres configs dont les classes peuvent être référencées dans le code généré par les générateurs de ce module.

  Chaque configuration considère par défaut que les objets disponibles sont limités aux objets générés, donc à ceux qui ont les tags de cette configuration. Si un objet référence un autre objet qui n'a **pas de tag en commun** avec la configuration (via une composition, une association, un mapper, etc.), la référence est omise de la génération.

  Cette propriété permet de **référencer des tags d'autres configurations**, afin de résoudre ces références et de les inclure au lieu de les omettre. La référence doit toutefois pouvoir être résolue dans le code généré.

  Exemple :

  ```yaml
  jpa:
    - name: back-common
      tags: [BackCommon]
      outputDirectory: ../sources/back/common
    - name: back-service
      tags: [BackService]
      referencedTags:
        BackCommon: back-common # Les classes référencées avec le tag `BackCommon` (et pas de tag `BackService`) auront leurs imports résolus avec la config `back-common`.
      outputDirectory: ../sources/back/service
  ```

- **`rootModule`**

  Définition du module racine, pour les différents regroupements à faire dessus (fichiers de traductions...).

  _Valeur par défaut_ : `{module:head}`

  _Templating_ : `{module}`

  Cette option est disponible dans les modules `csharp`, `javascript`, `jpa`, `sql` et `translation`.

- **`variables`**

  Variables globales de la configuration, qui pourront être utilisées dans toutes les propriétés de la configuration.

  Voir la section [Variables](#variables) ci-dessous.

- **`tagVariables`**

  Variables par tag de la configuration, qui pourront être utilisées dans les propriétés de la configuration qui supportent ces variables.

  Voir la section [Variables](#variables) ci-dessous.

- **`translateProperties`**

  Si les libellés des propriétés doivent être traduits dans cette configuration.

  _Valeur par défaut_ : la valeur renseignée dans le bloc `i18n` global

  Si oui, une clé de traduction est générée et les traductions sont incluses dans les fichiers générés ; sinon, la valeur renseignée dans le modèle est utilisée.

- **`translateReferences`**

  Si les libellés des listes de références doivent être traduits dans cette configuration.

  _Valeur par défaut_ : la valeur renseignée dans le bloc `i18n` global

  Si oui, une clé de traduction est générée et les traductions sont incluses dans les fichiers générés ; sinon, la valeur renseignée dans le modèle est utilisée.

Certaines options ne sont présentes que dans les schémas des modules qui les prennent en charge :

- **`disable`**

  Générateurs à désactiver pour cette configuration.

  Les valeurs autorisées sont celles énumérées dans le schéma du module.

- **`ignoreDefaultValues`**

  Désactive la génération des valeurs par défaut des propriétés dans les classes et endpoints générés avec cette configuration.

  _Valeur par défaut_ : `false` pour `csharp`, `documentation`, `javascript` et `jpa` ; `true` pour `sql`

- **`useLowerCaseSqlNames`**

  Utilise des noms de tables et de colonnes en minuscules.

  _Valeur par défaut_ : `true`

  _Variables par tag_ : **oui**

  Cette option est disponible dans les modules `csharp`, `documentation`, `jpa` et `sql`.

- **`uniqueValueGeneration`**

  Mode de génération des valeurs de propriétés avec clé d'unicité.

  _Valeurs possibles_ : `none`, `const-only`, `enum-only`, `enum-or-const`

  _Valeur par défaut_ : `enum-or-const`

- **`apiGeneration`**

  Mode de génération de l'API (`Client` ou `Server`).

  _Valeurs possibles_ : `Client`, `Server`

  _Variables par tag_ : **oui**

Ces deux options sont disponibles dans les modules `csharp` et `jpa`.

Exemple de configuration de module :

```yaml
javascript:
  - tags:
      - Interne
      - Externe
      - Common
    variables:
      root: ../sources/front
    outputDirectory: "{root}/src"
    domainPath: common/domains
```

## Variables

Chaque générateur peut définir et utiliser des **variables** dans sa configuration. Une variable n'est donc scopée qu'au générateur qui la définit.

Une variable peut être définie comme un string, un nombre, ou un booléen, mais sera toujours convertie en string lorsqu'elle est utilisée dans un template. Certains paramètres booléens de configuration autorisent les variables, mais cela passe par une implémentation dédiée qui reconvertit le string en booléen en interne.

Elles se définissent entre crochets (`{variable}`) et peuvent faire l'objet de [transformations](/model/templating#transformations).

Il existe **3 types de variables** :

### Variables globales

Les variables globales sont utilisables dans tous les paramètres (string) de toutes les générateurs, sans restriction. Elles seront remplacées à l'initialisation du générateur par la valeur qui a été définie dans la section `variables` de la configuration.

La variable globale `{app}` est définie par défaut avec la valeur de la propriété `app` de la configuration est peut donc être utilisée partout. Il en va de même pour le `name` de la configuration, si défini. Ces deux variables peuvent bien sûr être surchargées si besoin.

### Variables contextuelles

Il existe 2 variables "contextuelles", dont la valeur est automatiquement renseignée selon l'objet qui est généré, et qui sont utilisables dans certaines propriétés spécifiques des générateurs (selon leur implémentation). Ce sont :

- `{module}`, qui sera renseigné avec la valeur du module du fichier courant lors de la génération d'une classe ou d'un endpoint.

- `{lang}`, qui est utilisé lors de la génération de fichiers de ressources pour identifier la langue courante.

### Variables par tag

Un générateur peut choisir d'implémenter des variables qui ont des valeurs différentes **selon le tag** du fichier. De ce fait, un fichier ayant plusieurs tags résultant en des valeurs de paramètres différentes sera à priori généré plusieurs fois pour correspondre à chacune des valeurs possibles.

```yaml
javascript:
  - tags:
      - Interne
      - Externe
      - Common
    variables:
      root: ../sources/front
    tagVariables:
      Externe:
        tag: externe
      Common:
        tag: common
      Interne:
        tag: interne
    outputDirectory: "{root}/src"
    modelRootPath: "{tag}/model"
    resourceRootPath: "{tag}/locale"
    apiClientRootPath: "{tag}/services"
    fetchPath: "{tag}/server"
    domainPath: common/domains
```

`tagVariables` permet pour un tag de définir une liste de variables qui ne seront appliquées que lors de la génération de fichiers avec ce tag-là. Toutes les propriétés du générateur ne supportent pas les variables par tag.

**Une variable globale peut être surchargée par une variable par tag**, mais dans ce cas cette variable devient une variable par tag (et par conséquent n'est plus supportée dans les champs qui ne peuvent pas avoir de variables par tag). Une variable par tag existe dès lors qu'elle est définie par au moins un tag, et si un tag ne renseigne pas une variable par tag définie dans un autre tag, alors sa valeur pour ce tag sera automatiquement renseignée à `""` (ce qui effacera effectivement la variable lors de la résolution de valeur du paramètre).

### Résolution des variables

Les variables globales sont résolues en premier (une fois que celles qui devaient être transformées en variables par tag l'ont été), puis les variables par tag, et enfin les variables contextuelles. Cela implique en particulier que les variables contextuelles (`{module}` et `{lang}`) peuvent être "écrasées" par d'autres variables, et ainsi perdre leur contextualité. C'est rarement souhaitable, donc il vaut mieux en général éviter de provoquer des surcharges non intentionnelles.

`modgen` affichera un warning s'il trouve une variable non définie ou non supportée dans un paramètre (et précisera pourquoi, en particulier si la variable contextuelle ou par tag n'est pas supportée par le paramètre). Les variables non définies seront gardées telles quelles dans la génération, avec leurs `{}` (au cas où ça soit le comportement voulu dans ce cas précis, ce qui reste peu probable).

### Intersection de tags

Nous nous plaçons dans le cadre de la génération du tag `tag-a` d'une classe `A`, si cette classe a besoin d'importer une classe `B` qui n'est générée qu'avec le tag `tag-b` ce qui a un impact sur l'import à ajouter à A. Dans ce cas, le tag utilisé pour la résolution des variables par tag dans l'écriture de l'import de `B` dans `A` est `tag-b`.

## Options de configuration globales

- **`app`** (**obligatoire**)

  Nom de l'application.

  La variable globale `{app}` est automatiquement définie à partir de cette valeur.

- **`modelRoot`**

  Chemin du dossier racine qui contient tous les fichiers tmd du modèle, qui seront chargés par le générateur (à l'exception du fichier de domaines qui est chargé à part). Si non renseigné, le dossier contenant le fichier de config sera utilisé.

  _Valeur par défaut_ : dossier contenant le fichier de configuration

  Permet de séparer l'emplacement du fichier de configuration du reste du modèle.

  > **Attention** : Tous les chemins de fichiers dans les `uses` sont relatifs au `modelRoot`, et tous les fichiers `.tmd` doivent être dedans.

- **`modelFilePaths`**

  Si renseigné, seuls les fichiers de modèle matchant les patterns (globs) listés (relativement au `modelRoot`) seront chargés par TopModel.

  _Valeur par défaut_ : `["**/*.tmd"]`

  Si un pattern ne se termine pas par `.tmd`, TopModel lui ajoute `/*.tmd`.

- **`lockFileName`**

  Nom du fichier de lock.

  _Valeur par défaut_ : `topmodel.lock`

- **`ignoredFiles`**

  Liste des fichiers à ignorer après première génération (relatif au fichier de config).


  Chaque entrée doit renseigner un chemin (`path`) et un motif (`comment`). À utiliser avec parcimonie, pour contourner un manquement du générateur en attendant un correctif ou une meilleure solution. Un fichier ignoré déclenche un warning lors de la génération.

  _Valeur par défaut_ : `[]`

  - **`path`** (**obligatoire**)

    Chemin du fichier à ignorer, relatif au fichier de config.

  - **`comment`** (**obligatoire**)

    Motif.

  ```yaml
  ignoredFiles:
    - path: src/Generated/Custom.cs
      comment: "Fichier personnalisé à conserver"
  ```

- **`noWarn`**

  Liste des warnings à ignorer.

  _Valeur par défaut_ : `[]`

  À utiliser avec parcimonie également, en général ces warnings ne sont pas là pour rien 😉.

  Exemple :

  ```yaml
  app: Exemple
  noWarn:
    - TMD3004 # Ignore le warning sur la duplication des trigrammes
  ```

  Vous pouvez aussi **ignorer localement un warning** via un commentaire `# ignore` directement dans le fichier de modèle :

  ```yaml
  class:
    name: Utilisateur
    trigram: UTI # ignore TMD3004
  ```

  Le commentaire doit être sur la même ligne que le warning, commencer par `# ignore`, et contenir son code. À l'inverse de la solution globale via le fichier de config, le warning ne sera ignoré que pour cette instance précise.

- **`pluralizeTableNames`**

  Utilise le nom au pluriel des classes pour générer leurs noms de table SQL.

  _Valeur par défaut_ : `false`

  Le nom utilisé est le `pluralName` de la classe, plutôt que son `name`.

- **`defaultAssociationUseClass`**

  Détermine la valeur par défaut de `useClass` sur les propriétés d'association du modèle.

  _Valeur par défaut_ : `false`

  Voir les propriétés d'[association](./model/properties#association).

- **`useLegacyRoleNames`**

  Transforme les noms de rôles d'associations en majuscules au lieu du constant case pour déterminer le nom SQL des propriétés.

  _Valeur par défaut_ : `false`

- **`i18n`**

  Configuration de la localisation de l'application.

  Voir [page dédiée](./model/i18n.md) pour plus d'informations.

  - **`defaultLang`**

    Langue par défaut de l'application. Laisser vide si votre application n'a qu'une seule langue (non nommée). Une valeur nulle désactive la langue par défaut et l'utilisation des libellés définis dans le modèle dans une langue par défaut.

    _Valeur par défaut_ : `""`

  - **`langs`**

    Liste des langues de l'application (autres que la langue par défaut), indiquées par leur code.

    _Valeur par défaut_ : `[]`

  - **`rootPath`**

    Template du chemin des dossiers de traductions entrants. Doit contenir le template `{lang}`.

    _Valeur par défaut_ : `{lang}`

    _Templating_ : `{lang}`

  - **`translateReferences`**

    Si les libellés des listes de références doivent être traduits.

    _Valeur par défaut_ : `true`

  - **`translateProperties`**

    Si les libellés des propriétés doivent être traduits.

    _Valeur par défaut_ : `true`

  - **`allowPropertyLabelFallback`**

    Si le libellé d'une propriété n'est pas renseigné, autorise l'usage de son nom comme libellé.

    _Valeur par défaut_ : `true`

- **`generators`**

  Chemins vers des projets C# contenant des générateurs personnalisés.

  _Valeur par défaut_ : `[]`

  Ces projets doivent implémenter un module de générateurs personnalisé (voir la [page dédiée aux générateurs personnalisés](./generator#générateurs-personnalisés) pour plus de détails).

## Gestion des ancres YAML

Les fichiers de configuration TopModel supportent les [ancres YAML](https://smcleod.net/2022/11/yaml-anchors-and-aliases/), afin de pouvoir limiter les répétitions et de factoriser des déclarations.

Quelques exemples :

```yaml
---
app: Sample1
jpa:
  - tags: &tags
      - back-service-1
      - back-service-2
      - back-service-3
    # Le reste de la configuration
sql:
  - tags: *tags # Pour que les tags SQL soient les mêmes que les tags JPA

---
app: Sample2
jpa:
  - &jpa
    tags: #...
    outputDirectory: app1/....
    # Le reste de la configuration...

  - <<: *jpa # La deuxième config JPA est construite à partir de la première
    outputDirectory: app2/... # Et surcharge certaines propriétés
```
