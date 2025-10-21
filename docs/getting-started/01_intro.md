# Intro

Commençons par créer un environnement de travail propice à une expérience de développement extraordinaire. Pour cela, nous allons utiliser [VSCode](https://code.visualstudio.com/), qui est l'outil le plus adapté pour utiliser TopModel. En effet une [extension](https://marketplace.visualstudio.com/items?itemName=JabX.topmodel) a été développée spécialement pour cet IDE afin d'enrichir considérablement l'expérience de développement.

- Installer [VSCode](https://code.visualstudio.com/)
- Installer l'[extension](https://marketplace.visualstudio.com/items?itemName=JabX.topmodel)
- Installer le [SDK .NET](https://dotnet.microsoft.com/download)
- Installer le générateur avec la commande `dotnet tool install --global TopModel.Generator`

## Installation sous Linux / WSL

1. Installer le [SDK .NET sous Linux](https://learn.microsoft.com/fr-fr/dotnet/core/install/linux)

2. Installer TopModel via dotnet :

    ```bash
    dotnet tool install --global TopModel.Generator
    ```

3. Ajouter le chemin des outils .NET au PATH en modifiant le fichier de profil :

    ```bash
    export PATH="$HOME/.dotnet/tools:$PATH"
    ```

4. Recharger le profil :

    ```bash
    source ~/.bashrc  # ou source ~/.zshrc pour zsh
    ```

5. Une fois l'installation terminée, on peut utiliser la commande ```modgen``` pour générer du code.

    Pour vérifier l'installation :

    ```bash
    modgen --version
    ```

## Initialisation du fichier de configuration

Dans un nouveau dossier nommé "Projet", nous allons créer un fichier de configuration. Celui-ci permettra à l'extension TopModel de démarrer et de vous offrir les fonctionnalités d'auto-complétion, coloration syntaxique etc.

> Le fichier doit respecter le format `topmodel*.config`

Dans ce fichier de configuration (que l'on nommera `"topmodel.config"` dans le cadre de ce tutoriel) nous allons pour le moment nous contenter d'indiquer le nom de notre application.

```yaml
# topmodel.config
---
app: Hello World
```

Redémarrez VSCode, l'extension `TopModel` démarre, c'est parti...
