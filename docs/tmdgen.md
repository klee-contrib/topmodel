# Migrer un modèle externe (`openApi`) <!-- {docsify-ignore-all} -->

**TopModel.ModelGenerator** (`tmdgen`) est un outil de migration de modèle, depuis un source externe (`openApi` ou `database`) de fichiers `.tmd` dans un modèle `TopModel` existant

## Installation

L'installation de l'outil `tmdgen` se fait avec la commande :

```bash
dotnet tool install --global TopModel.ModelGenerator
```

## Configuration globale

La configuration de l'outil se fait dans un fichier au format `tmdgen*.config`. La configuration globale nécessite une seule propriété `modelRoot`, qui doit contenir le chemin relatif vers la racine du modèle `TopModel` des fichiers générés (pour écrire correctement les imports).

Puis, vous pouvez définir sous la propriété `database` une liste de sources base de données, et sous la propriété `openapi`, une liste de sources `openapi`.

## Lancement de la génération

L'outil `tmdgen` se lance avec la commande

```bash
tmdgen
```

Tout comme l'outil `modgen`, il est possible d'utiliser l'option `--watch`. A la différence de `modgen`, cette option ne permet de suivre les modifications que du fichier de configuration `tmdgen.config`.

Par défaut, l'outil cherchera tous les fichiers de configuration au format `tmdgen*.config`. Il est possible de préciser le fichier avec l'option `--file` (ou `-f`) manuellement.
