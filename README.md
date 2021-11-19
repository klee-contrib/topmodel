# TopModel
[![NuGet Badge](https://buildstats.info/nuget/TopModel.Generator?includePreReleases=true)](https://www.nuget.org/packages/TopModel.Generator)

**TopModel** est un **outil de modélisation** qui propose de représenter le modèle de données d'une application, qu'il soit persisté ou non, sous la forme d'**une série de fichiers textes éditables manuellement, au format YAML**. Son objectif est de réduire la modélisation à son expression la plus simple, en se concentrant uniquement sur la saisie d'informations pertinentes et utilisées par ses consommateurs directs (par exemple, le générateur de code), et en offrant un format texte facilement lisible, comparable et "mergeable".

On retrouve dans ce repository :

- **TopModel.Core**, la librairie de parsing de modèle.
- **TopModel.Generator**, un générateur de code C#, SQL (classique ou SSDT) et de modèles [focus](https://www.github.com/KleeGroup/focus4), à partir des modèles TopModel.
- **TopModel.UI**, un outil de visualisation de modèle TopModel.

## TopModel.Core

`TopModel.Core` ne dispose pas d'un exécutable indépendant : il sera utilisé et exposé par les deux outils qui sont construits dessus.

### Fichier de modèle

Un fichier de modèle est un fichier YAML qui contient une suite de documents YAML (à l'inverse du JSON, il est possible d'avoir plusieurs documents dans un même fichier en YAML. Le séparateur de document est `---`).

Le premier document YAML d'un fichier de modèle doit être le **descripteur du fichier**.
Il doit impérativement se composer de :

- Une propriété **`module`**, qui sert à identifier à quel module applicatif vont se rattacher les classes définies dans le fichier. Ce module sera utilisé par les différents générateurs pour déterminer à quel endroit chaque classe devra être générée.
- Une propriété **`tags`**, qui est une liste de noms arbitraires. Les tags seront utilisés dans la configuration des générateurs : ils permettront de définir quel générateur devra traiter quel fichier (chaque générateur aura donc aussi sa liste de tags).

De plus, si **des classes d'un fichier ont besoin de référencer des classes d'un autre fichier**, alors il faudra également **spécifier les dépendances aux autres fichiers**, via la propriété supplémentaire **`uses`**. C'est une liste de noms de fichiers. Un nom de fichier est déterminé comme étant **le chemin du fichier relatif à la racine du modèle**.

Exemple :

```yaml
---
module: Referentiel
tags:
  - Data
  - Server
uses:
  - Referentiel/Data/File_01
```

### Définition de domaines

Un domaine se définit comme un document YAML, dans un fichier de modèle.

Un domaine correspond à un type métier. Chaque champ doit avoir un domaine. Les règles de gestion liées à chaque domaine devront être implémentées dans chaque couche technique. En revanche, il faut par contre décrire ici, dans le modèle, comment chaque domaine va être représenté dans chaque langage, puisque la génération va en avoir besoin.

Un domaine se décrit donc de la façon suivante :

```yaml
---
domain:
  name: DO_ID
  label: Identifiant
  csharp:
    type: int?
    annotations: # Liste d'annotations à devoir ajouter à toute propriété de ce domaine (par exemple des annotations pour Ef)
    usings: # Liste de usings à devoir ajouter à la définition d'une classe qui utilise se domaine (par exemple un System.Collections.Generic si on veut mettre un type de liste)
  ts:
    type: number
    import: # Chemin de l'import à ajouter pour utiliser le type, si applicable.
  javaType: Integer
  sqlType: int
```

Naturellement, il n'est pas nécessaire de spécifier les langages pour lesquels le domaine n'est pas utilisé (et c'est évidemment obligatoire sinon).

Il n'y a pas besoin de préciser les dépendances aux fichiers contenant des domaines dans `uses` : tous les domaines sont automatiquement accessibles dans tous les fichiers. En revanche, cela implique que tous les fichiers ont une dépendance implicite à tous les fichiers contenant des domaines, ce qui pourrait entraîner des dépendances circulaires entre fichiers (qui ne sont **pas** supportées) involontaires. Par conséquent, et également par soucis de clarté, **il est fortement conseillé de définir tous les domaines dans un unique fichier qui ne contient que ces définitions**.

### Définition de classes

Une classe se définit comme un document YAML, dans un fichier de modèle.

Une classe se définit de la façon suivante :

```yaml
class:
  name: # Nom de la classe
  label: # Libellé de la classe
  comment: # Description de la calasse
  # Autres propriétés comme "trigram" (si persistant), "defaultProperty", "reference" pour les listes de références...

  properties:
    # 4 types de propriétés (qui doivent être définies après tout ce qui est avant)

    # Propriété "classique", identifiée si "name" est en premier
    - name: MontantInitialPret
      label: Montant initial du prêt
      required: false
      domain: DO_MONTANT
      comment: Montant initial du prêt.

    # Un alias, identifiée si "alias" est en premier
    - alias:
        property: Id
        class: Ligne
      prefix: true

    # Une association (FK), identifiée si "association" est en premier
    - association: ClasseCible
      required: true
      comment: Yolo c'est une FK obligatoire

    # Une composition, identifiée si "composition" est en premier
    - composition: ClasseCible
      name: ClasseCibleList
      kind: list
      comment: Yolo c'est une liste
```

Remarques :

- Il ne pas oublier de spécifier les dépendances aux classes d'autres fichiers dans `uses`.
- On ne gère pas de multiplicité sur les associations, elle ne se traduira directement que comme une foreign key, obligatoire ou non. Pas de n-n auto-générée non plus donc.
- Une définition d'alias peut également prendre une liste de propriétés dans `property`, et il est possible de surcharger le `label` et le `required` des propriétés aliasées. Ne pas oublier en revanche que la configuration de l'alias (`prefix`, `suffix`, et donc `label` et `required`) vont s'appliquer à toutes les propriétés de la définition de l'alias.

Une fois les propriétés définies, il est possible de compléter la définition de la classe par :

- `unique`, qui est une liste de clés unique à créer sur la classe (en base de données à priori). Une clé unique se définit via la liste des propriétés qui composent la clé unique (qui peut donc contenir une ou plusieurs propriétés)
- `values`, qui est un objet qui contient des valeurs "statiques" pour une classe que l'on veut avoir toujours disponible partout (en BDD, côté serveur, côté client). Il se définit comme une map dont les valeurs sont un objet "JSON" qui doit définir au moins toutes les propriétés obligatoires de la classe.

Exemple d'utilisation de ces deux propriétés :

```yaml
unique:
  - [Libelle]
values:
  Valeur1: { Code: 1, Libelle: Valeur 1 }
  Valeur2: { Code: 2, Libelle: Valeur 2 }
```

_Remarque: Pour initialiser une valeur d'une association dans `values`, il faut utiliser le nom de la classe et non le nom de la propriété (`AutreClasse` au lieu de `AutreClasseCode` par exemple)._

Pour conclure, un rappel sur l'ordre dans lequel il faut définir les différentes propriétés d'une classe:

```yaml
class:
  ## Tout le reste ##

  properties:
    -  ###
    -  ###
    -  ###

  unique:
    -  ###
    -  ###

  values:
    ###
    ###
    ###
```

_Remarque : il est possible d'inverser `unique` et `values`._

### Fichier de configuration

TopModel utilise un fichier de configuration, qui sera reconnu par une extension `*.yaml` (avec le "a", pour différencier des fichiers de domaines et classes). Il est partagé avec celui du générateur, qui le complétera avec de nombreuses options pour la génération. Dans son expression la plus pure, il ne nécessite qu'un seul paramètre :

- `modelRoot`, le répertoire dans lequel TopModel pourra trouver les fichiers de classes. Si non renseigné, le répertoire du fichier de config sera utilisé

### Utilisation en pratique

Il est vivement conseillé d'éditer les fichiers de modèles avec VSCode, muni de l'extension **"YAML"** qui permet de fournir un environnement type "IDE" pour l'édition de fichier YAML. TopModel fournit des schémas JSON (oui, ça marche aussi pour valider du YAML) qu'il conviendra de référencer dans les settings du workspace VSCode de la manière suivante :

```json
{
  "yaml.schemas": {
    "../../topmodel/TopModel.Core/schema.json": "*.yml",
    "../../topmodel/TopModel.Generator/schema.config.json": "*.yaml"
  }
}
```

Un schéma JSON ne peut valider que le contenu statique un fichier JSON ou YAML. Par conséquent, il ne pourra pas être utilisé pour fournir de l'autocomplétion dynamique (en particulier sur les classes, les domaines et les fichiers référencés), et encore moins pour des fonctionnalités avancées de type auto-import. A l'heure actuelle, le service YAML de VSCode n'est pas extensible, donc il faudrait réaliser des efforts considérables pour pouvoir mettre en place ces fonctionnalités...

Néanmoins, certains de ces manquements sont compensés par...

### TopModel --watch

TopModel est capable de fonctionner en mode "watch", au même titre qu'un outil comme Webpack. Il surveillera toute modification de fichier et essaiera de "recompiler" le modèle à chaque fois. En cas d'erreur, cette dernière sera affichée dans la console avec sa localisation dans les fichiers sources. Si TopModel est ouvert dans la console intégrée de VSCode, alors les liens seront cliquables.

_Remarque : il est peut être plus très à jour maintenant... Il faudra probablement y faire quelques corrections si vous voulez vous en servir..._

## TopModel.Generator

**TopModel.Generator** (TMG) est le générateur de code basé sur `TopModel.Core` et l'application principale à travers laquelle vous pourrez valider et utiliser votre modélisation. C'est une application **.NET Core 3.1** qui nécessite donc d'avoir le **SDK installé** sur votre machine.

Pour le lancer, il faudra utiliser la commande `dotnet run -p {chemin vers TMG} -- {chemin du fichier de config} (--watch)`.

TMG dispose de 5 générateurs, qui seront instanciés si la section de configuration associée est renseignée dans le fichier de config. Si vous avez déjà enregistré le schéma JSON du fichier de config depuis la présentation de `TopModel.Core`, vous devriez avoir les infos nécessaires pour configurer chacun des générateurs. Les 5 générateurs sont :

TMG dispose de 6 générateurs, qui seront instanciés si la section de configuration associée est renseignée dans le fichier de config. Chaque section est en réalité une **liste** de sections, ce qui permet de spécifier **plusieurs fois le même générateur**, avec des configurations différentes, qui peuvent varier selon les **tags listés**.

Si vous avez déjà enregistré le schéma JSON du fichier de config depuis la présentation de TopModel, vous devriez avoir les infos nécessaires pour configurer chacun des générateurs. Les 5 générateurs sont :

- **Le générateur de modèle SSDT (`ssdt`)**, qui permet de générer :
  - Un fichier par table SQL (classes dans les fichiers `"Data"`, munies d'un `"trigram"`)
  - Un fichier par type de table SQL, pour les tables qui en ont besoin
  - Un fichier par liste de référence à initialiser
  - Le fichier d'initialisation des listes de référence, qui appelle, dans l'ordre, tous les fichiers d'initialisation
- **Le générateur de SQL "procédural" (`proceduralSql`)**, qui permet de générer, pour postgre ou sqlserver :
  - Un fichier "crebas" qui contient toutes les créations de tables
  - Un fichier "index + fk" qui contient toutes les FKs et indexes
  - Un fichier qui contient toutes les initialisations de listes de références
- **Le générateur C# (`csharp`)**, qui permet de générer :
  - Un fichier par classe C#
  - Les interfaces et implémentations des accesseurs de listes de références par module
  - Le DbContext, si besoin pour Entity Framework (Core)
- **Le générateur de modèle Focus4 (`typescriptDefinition`)**, qui permet de générer :
  - Un fichier par classe non statique
  - Un fichier de classes statiques par module
- **Le générateur de ressources i18n (`javascriptResource`)**, qui permet de générer :
  - Un fichier i18n par module
- **Le générateur Kasper (`kasper`)**, qui permet de générer toutes les différentes classes abstraites, fichiers de ressources... pour ceux qui sont encore coincés avec un Kasper qui a 10 ans sur un projet dans un coin...

## TopModel.UI

**TopModel.UI** (TMUI) est un outil de visualisation d'un modèle TopModel. Il s'agit également d'une application .NET Core 3.1 qui se lance de la même manière que le générateur (sans l'option "watch", parce que TMUI est toujours en mode watch).

Une fois l'application lancée, elle est accessible par défaut sous [https://localhost:5001](https://localhost:5001) (le lien apparaît dans la console et sera cliquable depuis VSCode, si jamais).

Elle affichera dans le menu de gauche la liste des fichiers chargés, et il sera possible de cliquer dessus pour afficher un diagramme type UML généré automatiquement correspondant aux classes définies dans le fichier. Le diagramme sera mis à jour à la volée en cas de modification du fichier ouvert.

TMUI utilise [Graphviz](http://graphviz.org/) pour dessiner les diagrammes. **Il suppose que Graphviz est installé sur la machine qui l'exécute et accessible dans le PATH**. Comme l'indique la [page d'installation](https://graphviz.gitlab.io/_pages/Download/Download_windows.html), l'installer **ne renseigne pas le PATH par défaut**. Par défaut, le chemin a mettre dans le path est `C:\Program Files (x86)\Graphviz2.38\bin`.
