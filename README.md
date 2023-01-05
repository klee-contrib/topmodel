# TopModel

![Logo TopModel](./docs/media/logo-Dark.svg#gh-dark-mode-only)[Logo TopModel](./docs/media/logo-light.svg#gh-light-mode-only)

| Outil                                | Version                                                                                                                                           |
| ------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------- |
| `modgen` (`TopModel.Generator`)      | [![NuGet Badge](https://badgen.net/nuget/v/TopModel.Generator)](https://www.nuget.org/packages/TopModel.Generator)                                |
| Extension VSCode (`jabx.topmodel`)   | [![VS Badge](https://vsmarketplacebadges.dev/version-short/jabx.topmodel.svg)](https://marketplace.visualstudio.com/items?itemName=JabX.topmodel) |
| `tmdgen` (`TopModel.ModelGenerator`) | [![NuGet Badge](https://badgen.net/nuget/v/TopModel.ModelGenerator)](https://www.nuget.org/packages/TopModel.ModelGenerator)                      |

**TopModel** est un **outil de modélisation** qui propose de représenter le modèle de données d'une application, qu'il soit persisté ou non, sous la forme d'**une série de fichiers textes éditables manuellement, au format YAML**. Son objectif est de réduire la modélisation à son expression la plus simple, en se concentrant uniquement sur la saisie d'informations pertinentes et utilisées par ses consommateurs directs (par exemple, le générateur de code), et en offrant un format texte facilement lisible, comparable et "mergeable".

On retrouve dans ce repository :

- **TopModel.Core**, la librairie de parsing de modèle.
- **TopModel.Generator**, un générateur de code C#, SQL (classique ou SSDT) et de modèles [focus](https://www.github.com/KleeGroup/focus4), à partir des modèles TopModel.
- **TopModel.LanguageServer**, le language serveur utilisé dans l'extension VSCode.
- **TopModel.VSCode**, l'extension VSCode qui fournit des fonctionnalités d'auto-complétion, validation, auto-import etc.
- **TopModel.ModelGenerator** générateur de fichiers `tmd` à partir d'une source (OpenApi...)

> Désactiver AdBlock pour voir la documentation (mais promis ya pas de pub)

[La documentation est par ici](https://klee-contrib.github.io/topmodel)
