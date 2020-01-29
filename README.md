# TopModel

**TopModel** est un **outil de modélisation** qui propose de représenter le modèle de données d'une application, qu'il soit persisté ou non, sous la forme d'**une série de fichiers textes éditables manuellement, au format YAML**. Son objectif est de réduire la modélisation à son expression la plus simple, en se concentrant uniquement sur la saisie d'informations pertinentes et utilisées par ses consommateurs directs (par exemple, le générateur de code), et en offrant un format texte facilement lisible, comparable et "mergeable".

On retrouve dans ce repository :

- **TopModel.Core**, la librairie de parsing de modèle.
- **TopModel.Generator**, un générateur de code C#, SQL (classique ou SSDT) et de modèles [focus](https://www.github.com/KleeGroup/focus4), à partir des modèles TopModel.
- **TopModel.UI**, un outil de visualisation de modèle TopModel.

## TopModel.Core

`TopModel.Core` ne dispose pas d'un exécutable indépendant : il sera utilisé et exposé par les deux outils qui sont construits dessus.

### Définition de domaines

TopModel attend un fichier de déclaration de domaines, par défaut appelé `"domains.yml"`, qui doit contenir l'ensemble des domaines utilisés par les propriétés de classes du modèle.

Dans son expression la plus simple, il se présente sous la forme :

```
---
domain:
  name: DO_ID
  label: Identifiant
  csharpType: int?
  sqlType: int
---
domain:
  name: DO_CODE_10
  label: Code à 10 caractères
  csharpType: string
  sqlType: nvarchar(10)
---
# autres déclarations de domaines...
```

Les types C# et SQL (si type persisté) doivent être renseignés explicitement pour chaque domaine, et seront utilisés tels quel par le générateur. D'autres options à la marge sont également disponibles.

### Définition de classes

Les classes doivent être définies dans des fichiers de classes, qui doivent prendre la forme suivante :

```yaml
---
# descripteur du fichier
---
# classe 1
---
# classe 2
---
# ...
```

Il est conseillé de regrouper les classes selon une thématique métier commune. TopModel considérera que tous les fichiers `*.yml` qu'il trouve, à l'exception du fichier de domaines, sont des fichiers de classes.

Le descripteur du fichier doit contenir les propriétés suivantes :

```yaml
app: # Le nom de votre application. Devra être le même pour tous les fichiers de classes.
module: # nom du module métier, par ex "Referentiel", "Structure", "Projet"...
kind: # "Data" pour un fichier contenant des classes persistées, ou "Metier" pour un fichier contenant des DTOs
file: # Nom du fichier. Ne doit pas nécessairement correspondre au nom du fichier dans le filesystem, mais c'est conseillé.
uses: # La liste des dépendances du fichier courant à d'autres fichiers de classes.
```

Un élément de la liste `uses` se présente ainsi :

```yaml
module: # Nom du module
kind: # Type de module
files: # Liste des noms de fichier
  - File1
  - File2
  # ...
```

**Toutes les dépendances doivent être listées**. De plus, il est donc **impossible d'avoir des dépendances circulaires** entre fichiers.

Une classe se définit de la façon suivante :

```yaml
class:
  name: # Nom de la classe
  label: # Libellé de la classe
  comment: # Description de la calsse
  # Autres propriétés comme "trigram" (si persistant), "defaultProperty", "stereotype" pour les listes de références...

  properties:
    # 4 types de propriétés (qui doivent être saisies en dernier)

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
      prefix: Ligne

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

_Remarque : on ne gère pas de multiplicité sur les associations, elle ne se traduira directement que comme une foreign key, obligatoire ou non. Pas de n-n auto-générée non plus donc._

### Fichier de configuration

TopModel utilise un fichier de configuration, qui sera reconnu par une extension `*.yaml` (avec le "a", pour différencier des fichiers de domaines et classes). Il est partagé avec celui du générateur, qui le complètera avec de nombreuses options pour la génération. Dans son expression la plus pure, il nécessite 2 paramètres :

- `modelRoot`, le répertoire dans lequel TopModel pourra trouver les fichiers de classes. Si non renseigné, le répertoire du fichier de config sera utilsé
- `domains`, le chemin vers le fichier de domaines. `domains.yml` sera utilisé si non renseigné.

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

## TopModel.Generator

**TopModel.Generator** (TMG) est le générateur de code basé sur `TopModel.Core` et l'application principale à travers laquelle vous pourrez valider et utiliser votre modélisation. C'est une application **.NET Core 3.1** qui nécessite donc d'avoir le **SDK installé** sur votre machine.

Pour le lancer, il faudra utiliser la commande `dotnet run -p {chemin vers TMG} -- {chemin du fichier de config} (--watch)`.

TMG dispose de 5 générateurs, qui seront instanciés si la section de configuration associée est renseignée dans le fichier de config. Si vous avez déjà enregistré le schéma JSON du fichier de config depuis la présentation de TopModel, vous devriez avoir les infos nécessaires pour configurer chacun des générateurs. Les 5 générateurs sont :

- **Le générateur de modèle SSDT (`ssdt`)**, qui permet de générer :
  - Un fichier par table SQL (classes dans les fichiers `"Data"`, munies d'un `"trigram"`)
  - Un fichier par type de table SQL, pour les tables qui en ont besoin
  - Un fichier par liste de référence à initialiser
  - Le fichier d'initialisation des listes de référence, qui appelle, dans l'ordre, tous les fichiers d'initilisation
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
- **Le générateur de ressources i18n (`javascriptResource`)**, qui permt de générer :
  - Un fichier i18n par module

## TopModel.UI

**TopModel.UI** (TMUI) est un outil de visualisation d'un modèle TopModel. Il s'agit également d'une application .NET Core 3.1 qui se lance de la même manière que le générateur (sans l'option "watch", parce que TMUI est toujours en mode watch).

Une fois l'application lancée, elle est accessible par défaut sous [https://localhost:5001](https://localhost:5001) (le lien apparaît dans la console et sera cliquable depuis VSCode, si jamais).

Elle affichera dans le menu de gauche la liste des fichiers chargés, et il sera possible de cliquer dessus pour afficher un diagramme type UML généré automatiquement correspondant aux classes définies dans le fichier. Le diagramme sera mis à jour à la volée en cas de modification du fichier ouvert.

TMUI utilise [Graphviz](http://graphviz.org/) pour dessiner les diagrammes. **Il suppose que Graphviz est installé sur la machine qui l'exécute et accessible dans le PATH**. Comme l'indique la [page d'installation](https://graphviz.gitlab.io/_pages/Download/Download_windows.html), l'installer **ne renseigne pas le PATH par défaut**. Par défaut, le chemin a mettre dans le path est `C:\Program Files (x86)\Graphviz2.38\bin`.
