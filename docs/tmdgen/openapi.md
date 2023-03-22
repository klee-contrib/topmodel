# OpenApi

## OpenApi : Configuration

La configuration du générateur `OpenApi` permet de définir :

- `source` : un `url` ou un `path` vers le fichier de spécification `openApi`
- `domains` : Correspondance entre les types dans la spécification `openApi` et les domaines du modèle cible. Quelques spécificités
  - Définition d'un `name` ou d'un `type`, pour matcher soit sur le nom de la propriété soit sur son type
  - Les regexp sont acceptées
  - Le premier domaine à correspondre est utilisé pour la propriété
- `outputDirectory` : répertoire de génération des fichiers, relatif au `modelRoot`
- `modelTags` : tags à ajouter au fichier de modèle
- `endpointTags` : tags à ajouter au fichier de endpoints
- `include` : tags au sens `openApi` à inclure dans la génération. Les autres sont ignorés. Si non défini, tous les tags sont générés

Le générateur créé alors deux types fichiers :

- `Model` : contenant la définition des classes et de leurs propriétés
- `Endpoints` : pour chaque tag, contient la définition des endpoints du tag. Porte son nom en `PascalCase`

```yaml
modelRoot: ./petstore
openApi:
  - module: Petstore
    outputDirectory: ./
    domains:
      - type: int64
        domain: DO_ID
      - type: int32
        domain: DO_ENTIER
      - type: int32-map
        domain: DO_ENTIER_MAP
      - type: string
        domain: DO_LIBELLE
      - type: date-time
        domain: DO_DATE_TIME
      - type: boolean
        domain: DO_BOOLEAN
      - type: string-array
        domain: DO_LIBELLE
      - type: binary
        domain: DO_FILE
    source: ./petstore.json
    modelTags:
      - petstore
    endpointTags:
      - petstore
```
