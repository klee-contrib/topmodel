# TopModel Language Service for VSCode

![Features demo](https://raw.githubusercontent.com/klee-contrib/topmodel/develop/TopModel.VSCode/demo.gif "Features demonstration")

## Fonctionnalités disponibles

Cette extension enrichit considérablement l'expérience de développement de projets TopModel (fichiers avec extensions `*.tmd`).

Fonctionnalités principales :

- Coloration sémantique (références de classes, propriétés et domaines, imports)
- Auto-complétion des domaines, classes et propriétés d'alias
- Validation et affichages des erreurs et warnings
- Commandes `TopModel : Start model generation` et `TopModel : Start model generation (watch mode)` avec détection automatique du ou des fichiers de configuration
- Imports automatiques
- Recherche des symboles (classes, domaines et endpoints) avec `Ctrl + T`
- Listing des références de classes et domaines (via `Maj + F12` et CodeLens)
- Mise en forme :
  - Tri des imports
- Aides à la saisie :
  - Ajout de l'import manquant
  - Ajout de la classe manquante au fichier courant
  - Ajout du domaine manquant au fichier des domaines
  - Renommage des classes et des domaines (`F2`)
- Prévisualisation diagram UML
- Panneau `outline`
- Warnings
  - Imports inutiles
  - Trigrams en doublon
  - Propriétés d'alias en doublon
