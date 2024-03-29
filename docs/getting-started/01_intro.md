# Intro

Commençons par créer un environnement de travail propice à une expérience de développement extraordinaire. Pour cela, nous allons utiliser [VSCode](https://code.visualstudio.com/), qui est l'outil le plus adapté pour utiliser TopModel. En effet une [extension](https://marketplace.visualstudio.com/items?itemName=JabX.topmodel) a été développée spécialement pour cet IDE afin d'enrichir considérablement l'expérience de développement.

- Installer [VSCode](https://code.visualstudio.com/)
- Installer l'[extension](https://marketplace.visualstudio.com/items?itemName=JabX.topmodel)
- Installer le [SDK .NET](https://dotnet.microsoft.com/download)
- Installer le générateur avec la commande `dotnet tool install --global TopModel.Generator`

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
