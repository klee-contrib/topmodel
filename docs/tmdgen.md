# Migrer un modèle externe (`openApi`)

**TopModel.ModelGenerator** (`tmdgen`) est un outil de migration de modèle, depuis un source externe (`openApi`) de fichiers `.tmd` dans un modèle `TopModel` existant

## Installation

L'installation de l'outil `tmdgen` se fait avec la commande :

```bash
dotnet tool install --global TopModel.ModelGenerator
```

## Configuration globale

La configuration de l'outil se fait dans un fichier au format `tmdgen*.config`. La configuration globale nécessite une seule propriété `modelRoot`, qui doit contenir le chemin relatif vers la racine du modèle `TopModel` des fichiers générés (pour écrire correctement les imports).

## Lancement de la génération

L'outil `tmdgen` se lance avec la commande

```bash
tmdgen
```

Tout comme l'outil `modgen`, il est possible d'utiliser l'option `--watch`. A la différence de `modgen`, cette option ne permet de suivre les modifications que du fichier de configuration `tmdgen.config`.

Par défaut, l'outil cherchera tous les fichiers de configuration au format `tmdgen*.config`. Il est possible de préciser le fichier avec l'option `--file` (ou `-f`) manuellement.

## OpenApi

### OpenApi : Configuration

La configuration du générateur `OpenApi` permet de définir :

- `sources` : `url` ou `path` du fichier de spécification `openApi`
- `domains` : Correspondance entre les types dans la spécification `openApi` et les domaines du modèle cible. Quelques spécificités
  - Définition d'un `name` ou d'un `type`, pour matcher soit sur le nom de la propriété soit sur son type
  - Les regexp sont acceptées
  - Le premier domaine à correspondre est utilisé pour la propriété
- `outputDirectory` : répertoire de génération des fichiers
- `modelTags` : tags à ajouter au fichier de modèle
- `endpointTags` : tags à ajouter au fichier de endpoints
- `include` : tags au sens `openApi` à inclure dans la génération. Les autres sont ignorés. Si non défini, tous les tags sont générés

Le générateur créé alors deux types fichiers :

- `Model` : contenant la définition des classes et de leurs propriétés
- `Endpoints` : pour chaque tag, contient la définition des endpoints du tag. Porte son nom en PascalCase

```yaml
openApi:
  outputDirectory: ./
  domains:
    - name: /(?i)email/
      domain: DO_EMAIL
    - type: int64
      domain: DO_ID
    - type: string
      domain: DO_LIBELLE
    - type: integer
      domain: DO_ENTIER
    - type: date-time
      domain: DO_DATE
    - type: binary
      domain: DO_FILE
  sources:
    captcha:
      path: ./captcha.json
      modelFileName: Model
      modelTags:
        - Back
        - Client
      endpointTags:
        - Back
        - Client
```
