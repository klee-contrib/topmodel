# Contribuer à TopModel

## Debbugger l'extension VSCode et le Language Server

### Prérequis

- Installer VSCode
- Installer NodeJS
- Installer .NET SDK

### Etapes

- Lancer la commande `dotnet publish -c debug` dans le dossier TopModel.LanguageServer. Cela permet de builder le language server et de le mettre à disposition de l'extension.
- Ouvrir le projet TopModel.VSCode avec VsCode
- Lancer la commande `npm i` pour installer les dépendances
- Lancer la commande `npm run start` pour lancer l'extension en mode debug

Pour débugger le Language Server, ouvrir le projet TopModel.LanguageServer avec VSCode, puis lancer une session de debug en mode .NET Core Attach. Il faut ensuite sélectionner la bonne instance du language server dans la liste des processus.
