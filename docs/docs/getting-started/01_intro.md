# Introduction

Commençons par créer un environnement de travail propice à une expérience de développement optimale. Pour cela, nous allons utiliser [VSCode](https://code.visualstudio.com/), qui est l'outil le plus adapté pour utiliser TopModel. En effet, une [extension](https://marketplace.visualstudio.com/items?itemName=JabX.topmodel) a été développée spécialement pour cet IDE afin d'enrichir considérablement l'expérience de développement.

## Prérequis

Avant de commencer, vous devez installer les éléments suivants :

1. **[VSCode](https://code.visualstudio.com/)** : L'éditeur de code recommandé
2. **L'[extension TopModel](https://marketplace.visualstudio.com/items?itemName=JabX.topmodel)** : Pour l'autocomplétion, la validation et la navigation
3. **Le [SDK .NET](https://dotnet.microsoft.com/download)** : Version 8.0 ou supérieure
4. **TopModel.Generator** : Le générateur de code
5. **TopModel.LanguageServer** : Le language server, utilisé par l'extension VSCode

### Installation de TopModel.Generator

Installez le générateur avec la commande suivante :

```bash
dotnet tool install --global TopModel.Generator
```

Pour vérifier que l'installation a réussi :

```bash
modgen --version
```

Par la suite, pour mettre à jour TopModel :

```bash
dotnet tool update --global TopModel.Generator
```

> **💡 Astuce** : L'extension VSCode propose également la commande `Mettre à jour TopModel.Generator` via `F1`.

### Installation du language server (`modls`)

Depuis sa version 4.7.0, l'extension VSCode s'appuie sur le language server `TopModel.LanguageServer`, distribué séparément comme outil .NET global (commande `modls`). L'extension l'installe automatiquement à son démarrage. Vous pouvez aussi l'installer manuellement :

```bash
dotnet tool install --global TopModel.LanguageServer
```

> **⚠️ Important** : Si `modls` n'est pas installé (par exemple en l'absence de connexion réseau au démarrage), l'extension affiche une erreur et les fonctionnalités d'autocomplétion, de validation et de navigation ne sont pas disponibles. Installez `modls` avec la commande ci-dessus, puis redémarrez VSCode.

> **💡 Note** : Les versions de `modls`, `modgen` et `tmdgen` sont publiées ensemble. L'extension vérifie qu'elles sont alignées et propose une mise à jour si ce n'est pas le cas.

## Installation sous Linux / WSL

Si vous utilisez Linux ou WSL, suivez ces étapes supplémentaires :

1. **Installer le [SDK .NET sous Linux](https://learn.microsoft.com/fr-fr/dotnet/core/install/linux)**

2. **Installer TopModel via dotnet** :

   ```bash
   dotnet tool install --global TopModel.Generator
   ```

3. **Ajouter le chemin des outils .NET au PATH** en modifiant le fichier de profil :

   ```bash
   export PATH="$HOME/.dotnet/tools:$PATH"
   ```

4. **Recharger le profil** :

   ```bash
   source ~/.bashrc  # ou source ~/.zshrc pour zsh
   ```

5. **Vérifier l'installation** :

   ```bash
   modgen --version
   ```

## Initialisation du fichier de configuration

Dans un nouveau dossier nommé "Projet", nous allons créer un fichier de configuration. Celui-ci permettra à l'extension TopModel de démarrer et de vous offrir les fonctionnalités d'auto-complétion, coloration syntaxique, etc.

> **💡 Note** : Le fichier doit respecter le format `topmodel*.config` (par exemple `topmodel.config` ou `topmodel.monapp.config`)

Dans ce fichier de configuration (que l'on nommera `"topmodel.config"` dans le cadre de ce tutoriel), nous allons pour le moment nous contenter d'indiquer le nom de notre application.

```yaml title="topmodel.config"
---
app: Hello World
```

### Activer l'extension

Une fois le fichier de configuration créé :

1. **Redémarrez VSCode** pour que l'extension TopModel démarre
2. L'extension devrait maintenant être active et vous offrir :
   - L'autocomplétion dans les fichiers `.tmd`
   - La validation en temps réel
   - La navigation entre fichiers
   - La coloration syntaxique

> **✅ Vérification** : Si l'extension fonctionne correctement, vous devriez voir l'icône TopModel dans la barre de statut de VSCode.

> **⚠️ En cas d'erreur** : Si la barre de statut affiche une erreur indiquant que le language server (`modls`) n'est pas installé, installez-le avec `dotnet tool install --global TopModel.LanguageServer`, puis redémarrez VSCode.

C'est parti ! Vous êtes maintenant prêt à créer votre premier modèle TopModel.
