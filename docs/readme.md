# Présentation <!-- {docsify-ignore-all} -->

**TopModel** est un **outil de modélisation** qui propose de représenter le modèle de données d'une application, qu'il soit persisté ou non, sous la forme d'**une série de fichiers textes éditables manuellement, au format YAML**. Son objectif est de réduire la modélisation à son expression la plus simple, en se concentrant uniquement sur la saisie d'informations pertinentes et utilisées par ses consommateurs directs (par exemple, le générateur de code), et en offrant un format texte facilement lisible, comparable et "mergeable".

## Installation

### Pré-requis

`TopModel.Generator` est une application .NET 8 ou 9, packagée comme un [outil .NET](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools).

Pour l'utiliser, il faut avoir le [SDK .NET](https://dotnet.microsoft.com/download) installé sur votre machine, puis lancer la commande

```bash
dotnet tool install --global TopModel.Generator
```

Par la suite, pour mettre à jour TopModel, utiliser la commande :

```bash
dotnet tool update --global TopModel.Generator
```

Il est vivement conseillé d'éditer les fichiers de modèles avec [VSCode](https://code.visualstudio.com/), muni de l'extension **"TopModel"** qui permet de fournir un environnement type "IDE" pour l'édition de fichier topmodel. TopModel fournit des schémas JSON (oui, ça marche aussi pour valider du YAML), ainsi que des fonctionnalités d'autocomplétion et de navigation.

### Configuration

Pour démarrer votre projet TopModel, vous devez d'abord écrire un fichier de configuration. Celui-ci contient notamment :

- Le nom de l'application
- Le répertoire racine des fichiers de modèle
- La configuration des générateurs
- Un système de filtre (tags) pour la sélection des générateurs par langage sur lesquels TopModel sera utilisé.

Le fichier de configuration doit s'appeler `topmodel.config` ou `topmodel.[NOM DE L'APPLICATION].config`

#### Ignorer les warnings

Il est possible de rendre silencieux certains warnings depuis le fichier de configuration. Pour cela, ajoutez la propriété `noWarn`. L'ensemble des warnings entrés dans cette propriété seront ignorés à la génération et dans l'extension.

> **Note** : À utiliser avec parcimonie, en général ces warnings ne sont pas là pour rien 😉

Exemple :

```yaml
app: Exemple
noWarn:
  - TMD3004 # Ignore le warning sur la duplication des trigrammes
```

## Edition du modèle

Les fichiers de modèle décrivent, comme leur nom l'indique, le modèle de données. Ils portent l'extension `.tmd`.

Dans ces fichiers, vous pouvez décrire trois types d'objets :

- Les classes (persistées ou non)
- Les domaines
- Les endpoints

Pour plus de détails, voir la [page dédiée](./model)

## Génération

Une fois votre modèle créé, vous pouvez lancer la génération du code avec la commande **`modgen`**.

La commande **`modgen`** permet de lancer la génération du modèle. Elle récupère par défaut tous les fichiers de configuration qu'elle trouve dans le répertoire courant, et génère le modèle correspondant à chaque configuration.

### Options principales

- **`--file`**/**`-f`** : Chemin vers un fichier de config en particulier à générer (au lieu de la récupération automatique de tous les fichiers). Cette option peut être spécifiée plusieurs fois pour embarquer plusieurs configurations spécifiques.
- **`--watch`**/**`-w`** : Permet de "surveiller" toute modification de fichier, et TopModel essaiera de "recompiler" le(s) modèle(s) à chaque fois. En cas d'erreur, cette dernière sera affichée dans la console avec sa localisation dans les fichiers sources. Si TopModel est ouvert dans la console intégrée de VSCode, alors les liens seront cliquables.

> **Astuce** : Si vous avez l'extension VSCode, `modgen` se lance à l'aide d'une action rapide via la touche `F1` ou depuis la barre de statut.

Pour plus de détails sur toutes les options disponibles, consultez la [page dédiée à la ligne de commandes](./cli.md).
