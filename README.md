# TopModel

[![NuGet Badge](https://badgen.net/nuget/v/TopModel.Generator)](https://www.nuget.org/packages/TopModel.Generator) [![VS Badge](https://vsmarketplacebadges.dev/version-short/jabx.topmodel.svg)](https://marketplace.visualstudio.com/items?itemName=JabX.topmodel)

**TopModel** est un **outil de modélisation** qui propose de représenter le modèle de données d'une application, qu'il soit persisté ou non, sous la forme d'**une série de fichiers textes éditables manuellement, au format YAML**. Son objectif est de réduire la modélisation à son expression la plus simple, en se concentrant uniquement sur la saisie d'informations pertinentes et utilisées par ses consommateurs directs (par exemple, le générateur de code), et en offrant un format texte facilement lisible, comparable et "mergeable".

On retrouve dans ce repository :

- **TopModel.Core**, la librairie de parsing de modèle.
- **TopModel.Generator**, un générateur de code C#, SQL (classique ou SSDT) et de modèles [focus](https://www.github.com/KleeGroup/focus4), à partir des modèles TopModel.
- **TopModel.UI**, un outil de visualisation de modèle TopModel.

[La documentation est par ici](https://klee-contrib.github.io/topmodel)
