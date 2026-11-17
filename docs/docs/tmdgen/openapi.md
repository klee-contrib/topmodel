# OpenApi

## OpenApi : Configuration

La configuration du générateur `OpenApi` permet de définir :

- `source` : un `url` ou un `path` vers le fichier de spécification `openApi`
- `domains` : Correspondance entre les types dans la spécification `openApi` et les domaines du modèle cible. Quelques spécificités
  - Définition d'un `name` ou d'un `type`, pour matcher soit sur le nom de la propriété soit sur son type
  - Les regexp sont acceptées, voir [Format des expressions régulières](../tmdgen#format-des-expressions-régulières)
  - Le meilleur domaine à correspondre au nom, puis au type de la propriété est utilisé, voir [Choix du domaine](../tmdgen#choix-du-domaine)
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
      # Correspondance sur le nom de la propriété (exact, insensible à la casse)
      - name: email
        domain: DO_EMAIL
      # Correspondance sur le nom par regex : propriétés finissant par "Id" (ex : userId, petId)
      - name: '/Id$/'
        domain: DO_ID
      # Correspondance sur le nom par regex, insensible à la casse : contient "date"
      - name: '/(?i)date/'
        domain: DO_DATE
      # Correspondance sur le type par regex : tous les types de tableaux (string-array, int32-array...)
      - type: '/-array$/'
        domain: DO_LIBELLE
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
