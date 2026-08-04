# Contribuer à TopModel

## TopModel

### [TopModel] Prérequis

- Installer .NET SDK

### Architecture du projet

TopModel est décomposé en différents modules :

#### Modules Core

- **TopModel.Core** : Contient les classes de base de TopModel, le parsing des modèles et la gestion du modèle de données
- **TopModel.Generator.Core** : Contient les classes de base pour les générateurs (générateurs de classes, endpoints, mappers, etc.)

#### Modgen

- **TopModel.Generator** : Générateur de code principal (`modgen`) qui orchestre l'exécution des différents générateurs spécialisés

#### Modules de génération

- **TopModel.Generator.Csharp** : Générateur pour le langage C# (classes, API client/serveur, DbContext, mappers, ressources)
- **TopModel.Generator.Javascript** : Générateur pour JavaScript/TypeScript (API clients fetch/Angular, ressources, définitions TypeScript)
- **TopModel.Generator.Jpa** : Générateur pour Java/JPA (classes, endpoints, mappers, ressources)
- **TopModel.Generator.Sql** : Générateur pour SQL (scripts procéduraux, SSDT)
- **TopModel.Generator.Translation** : Générateur pour les fichiers de traduction

#### Outils et extensions

- **TopModel.ModelGenerator** : Générateur de fichiers `.tmd` à partir de sources externes (OpenAPI, PostgreSQL, Oracle, etc.) - outil `tmdgen`
- **TopModel.Utils** : Utilitaires partagés entre les différents modules
- **TopModel.LanguageServer** : Language Server Protocol (LSP) utilisé par l'extension VSCode pour fournir l'auto-complétion, la validation, l'auto-import... Publié comme tool .NET global (`modls`, package NuGet `TopModel.LanguageServer`)
- **TopModel.VSCode** : Extension VSCode qui intègre TopModel dans l'éditeur

## Debbugger l'extension VSCode et le Language Server

### [VSCode] Prérequis

- Installer VSCode
- Installer NodeJS
- Installer .NET SDK

### Etapes

- Le language server est consommé par l'extension comme tool .NET global (`modls`). Pour tester une version locale, le packager puis utiliser le dll généré dans l'extension VSCode :

```bash
# Depuis la racine du dépôt
dotnet publish -c debug
```

- Ouvrir le projet TopModel.VSCode avec VsCode
- Lancer la commande `npm ci` pour installer les dépendances
- Lancer la commande `npm run start` pour lancer le build de l'extension en mode watch
- Le fichier `.vscode/launch.json` contient la configuration pour lancer le debug de l'extension. Il suffit de démarrer une session de debug avec la touche `F5`

Paramétrer l'extension pour qu'elle utilise le dll généré.

Pour débugger le Language Server, ouvrir le projet `TopModel.LanguageServer` avec VSCode, puis lancer une session de debug en mode .NET Core Attach. Il faut ensuite sélectionner la bonne instance du language server dans la liste des processus.

## Documentation

La documentation est construite avec [Docusaurus](https://docusaurus.io/) et vit intégralement dans le dossier `docs`.

### [Docs] Prérequis

- Installer NodeJS (≥ 24)

### Structure

- `docs/docs/` : contenu de la documentation (Markdown/MDX)
  - `getting-started/` : tutoriel pas-à-pas (fichiers préfixés pour l'ordre du sommaire)
  - `model/` : référence du langage de modélisation (classes, domaines, endpoints, mappers, etc.)
  - `generator/` : documentation des générateurs (`csharp`, `jpa`, `js`, `sql`, `translation`)
  - `tmdgen/` : documentation de `tmdgen` (sources OpenAPI et base de données)
  - `cli.md`, `configuration.md`, `generator.md`, `model.md`, `tmdgen.md` : pages d'index
- `docs/sidebars.ts` : configuration du sommaire
- `docs/docusaurus.config.ts` : configuration Docusaurus (thème, plugins, URL)
- `docs/src/` et `docs/static/` : ressources (CSS, images, page d'accueil)

### Commandes

Depuis le dossier `docs` :

- `npm ci` : installation des dépendances
- `npm run start` : serveur de développement avec rechargement à chaud (<http://localhost:3000>)
- `npm run build` : build de production dans `docs/build/`
- `npm run serve` : sert le build de production localement
- `npm run typecheck` : vérification TypeScript de la configuration

### Publication

La documentation est publiée automatiquement sur GitHub Pages via le workflow `.github/workflows/doc.yml` à chaque push de tag. Aucun commit manuel du build n'est nécessaire.
