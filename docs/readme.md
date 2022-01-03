# Présentation <!-- {docsify-ignore-all} -->

**TopModel** est un **outil de modélisation** qui propose de représenter le modèle de données d'une application, qu'il soit persisté ou non, sous la forme d'**une série de fichiers textes éditables manuellement, au format YAML**. Son objectif est de réduire la modélisation à son expression la plus simple, en se concentrant uniquement sur la saisie d'informations pertinentes et utilisées par ses consommateurs directs (par exemple, le générateur de code), et en offrant un format texte facilement lisible, comparable et "mergeable".

## Installation

### Edition du modèle

Il est vivement conseillé d'éditer les fichiers de modèles avec VSCode, muni de l'extension **"YAML"** qui permet de fournir un environnement type "IDE" pour l'édition de fichier YAML. TopModel fournit des schémas JSON (oui, ça marche aussi pour valider du YAML) qu'il conviendra de référencer dans les settings du workspace VSCode de la manière suivante :

```json
{
  "yaml.schemas": {
    "https://raw.githubusercontent.com/klee-contrib/topmodel/develop/TopModel.Core/schema.json": "*.yml",
    "https://raw.githubusercontent.com/klee-contrib/topmodel/develop/TopModel.Generator/schema.config.json": "*.yaml"
  }
}
```

_Remarque : Un schéma JSON ne peut valider que le contenu statique un fichier JSON ou YAML. Par conséquent, il ne pourra pas être utilisé pour fournir de l'autocomplétion dynamique (en particulier sur les classes, les domaines et les fichiers référencés), et encore moins pour des fonctionnalités avancées de type auto-import. A l'heure actuelle, le service YAML de VSCode n'est pas extensible, donc il faudrait réaliser des efforts considérables pour pouvoir mettre en place ces fonctionnalités..._

### Génération

`TopModel.Generator` est une application .NET 6, packagée comme un [outil .NET](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools).

Pour l'utiliser, il faut avoir le [SDK .NET](https://dotnet.microsoft.com/download) installé sur votre machine, puis la commande

```
dotnet tool install --global TopModel.Generator
```

vous permettra d'utiliser la CLI **`modgen`** pour lancer la génération.

## Utilisation

La commande **`modgen`** permet de lancer la génération du modèle. Elle prend comme paramètre le fichier de config (par défaut `modgen.yaml` à la racine du dossier), et peut être lancée avec l'option **`--watch`**.

L'option **`--watch`** permet de "surveiller" toute modification de fichier, et TopModel essaiera de "recompiler" le modèle à chaque fois. En cas d'erreur, cette dernière sera affichée dans la console avec sa localisation dans les fichiers sources. Si TopModel est ouvert dans la console intégrée de VSCode, alors les liens seront cliquables.
