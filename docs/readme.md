# Présentation <!-- {docsify-ignore-all} -->

**TopModel** est un **outil de modélisation** qui propose de représenter le modèle de données d'une application, qu'il soit persisté ou non, sous la forme d'**une série de fichiers textes éditables manuellement, au format YAML**. Son objectif est de réduire la modélisation à son expression la plus simple, en se concentrant uniquement sur la saisie d'informations pertinentes et utilisées par ses consommateurs directs (par exemple, le générateur de code), et en offrant un format texte facilement lisible, comparable et "mergeable".

## Avantages de TopModel

- **Modélisation simple** : Représentation du modèle de données en YAML, facilement lisible et éditable
- **Génération de code** : Génération automatique de code pour plusieurs langages (C#, Java/JPA, JavaScript/TypeScript, SQL)
- **Multi-plateforme** : Support de plusieurs technologies et frameworks
- **Intégration IDE** : Extension VSCode avec autocomplétion, validation et navigation
- **Versioning** : Format texte compatible avec Git et les outils de merge
- **Extensibilité** : Possibilité de créer ses propres générateurs

## Installation

### Pré-requis

`TopModel.Generator` est une application .NET 8 ou 9, packagée comme un [outil .NET](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools).

Pour l'utiliser, il faut avoir le [SDK .NET](https://dotnet.microsoft.com/download) installé sur votre machine (version 8.0 ou supérieure), puis lancer la commande :

```bash
dotnet tool install --global TopModel.Generator
```

Par la suite, pour mettre à jour TopModel, utiliser la commande :

```bash
dotnet tool update --global TopModel.Generator
```

> **💡 Astuce** : Pour vérifier que l'installation a réussi, vous pouvez exécuter `modgen --version` pour afficher la version installée.

### Extension VSCode

Il est **vivement conseillé** d'éditer les fichiers de modèles avec [VSCode](https://code.visualstudio.com/), muni de l'extension **"TopModel"** ([disponible sur le marketplace](https://marketplace.visualstudio.com/items?itemName=JabX.topmodel)) qui permet de fournir un environnement type "IDE" pour l'édition de fichier topmodel.

L'extension TopModel fournit :

- Des schémas JSON pour la validation (oui, ça marche aussi pour valider du YAML)
- L'autocomplétion intelligente
- La navigation entre fichiers et références
- La validation en temps réel
- L'intégration avec la commande `modgen`

### Configuration

Pour démarrer votre projet TopModel, vous devez d'abord écrire un fichier de configuration. Celui-ci contient notamment :

- Le nom de l'application
- Le répertoire racine des fichiers de modèle
- La configuration des générateurs (C#, JPA, JavaScript, SQL, etc.)
- Un système de filtre (tags) pour la sélection des générateurs par langage sur lesquels TopModel sera utilisé

Le fichier de configuration doit s'appeler `topmodel.config` ou `topmodel.[NOM DE L'APPLICATION].config`.

Exemple minimal :

```yaml
# topmodel.config
---
app: MonApplication
```

Pour plus de détails sur la configuration complète, consultez la [page dédiée à la configuration](./configuration.md).

#### Ignorer les warnings

Il est possible de rendre silencieux certains warnings depuis le fichier de configuration. Pour cela, ajoutez la propriété `noWarn`. L'ensemble des warnings entrés dans cette propriété seront ignorés à la génération et dans l'extension.

> **⚠️ Note** : À utiliser avec parcimonie, en général ces warnings ne sont pas là pour rien 😉

Exemple :

```yaml
app: Exemple
noWarn:
  - TMD3004 # Ignore le warning sur la duplication des trigrammes
```

Vous pouvez aussi ignorer localement un warning via un commentaire `# ignore` :

```yaml
class:
  name: Utilisateur
  trigram: UTI # ignore TMD3004
```

Le commentaire doit être sur la même ligne que le warning, commencer par `# ignore`, et contenir son code. A l'inverse de la solution globale via le fichier de config, le warning ne sera ignoré que pour cette instance précise.

## Édition du modèle

Les fichiers de modèle décrivent, comme leur nom l'indique, le modèle de données. Ils portent l'extension `.tmd` et sont au format YAML.

Dans ces fichiers, vous pouvez décrire plusieurs types d'objets :

- **Les classes** (persistées ou non) : Entités métier, DTOs, etc.
- **Les domaines** : Types métier réutilisables avec leurs représentations par langage
- **Les endpoints** : Définition des API REST
- **Les décorateurs** : Métadonnées réutilisables
- **Les mappers** : Transformations entre classes
- **Les flux de données** : Définition des flux de traitement

Pour plus de détails, voir la [page dédiée à la modélisation](./model.md) et le [tutoriel de prise en main](./getting-started/00_getting_started.md).

## Génération

Une fois votre modèle créé, vous pouvez lancer la génération du code avec la commande **`modgen`**.

La commande **`modgen`** permet de lancer la génération du modèle. Elle récupère par défaut tous les fichiers de configuration qu'elle trouve dans le répertoire courant, et génère le modèle correspondant à chaque configuration.

### Utilisation de base

```bash
# Génération simple
modgen

# Génération avec surveillance des modifications (mode watch)
modgen --watch

# Génération d'un fichier de configuration spécifique
modgen --file topmodel.config
```

### Options principales

- **`--file`**/**`-f`** : Chemin vers un fichier de config en particulier à générer (au lieu de la récupération automatique de tous les fichiers). Cette option peut être spécifiée plusieurs fois pour embarquer plusieurs configurations spécifiques.
- **`--watch`**/**`-w`** : Permet de "surveiller" toute modification de fichier, et TopModel essaiera de "recompiler" le(s) modèle(s) à chaque fois. En cas d'erreur, cette dernière sera affichée dans la console avec sa localisation dans les fichiers sources. Si TopModel est ouvert dans la console intégrée de VSCode, alors les liens seront cliquables.
- **`--exclude`**/**`-e`** : Tag à ignorer lors de la génération. Cette option peut être spécifiée plusieurs fois pour exclure plusieurs tags.
- **`--check`**/**`-c`** : Vérifie que le code généré est conforme au modèle.
- **`--schema`**/**`-s`** : Génère le fichier de schéma JSON complet pour l'autocomplétion dans VSCode.

> **💡 Astuce** : Si vous avez l'extension VSCode, `modgen` se lance à l'aide d'une action rapide via la touche `F1` ou depuis la barre de statut.

Pour plus de détails sur toutes les options disponibles, consultez la [page dédiée à la ligne de commandes](./cli.md).
