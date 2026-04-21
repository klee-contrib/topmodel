# Génération des resources

| Nom              | Condition d'activation | Objets ciblés                                                                | Fichiers générés                                                                                                                                                                                                                                                            |
| ---------------- | ---------------------- | ---------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `JpaResourceGen` | `resourcesPath` défini | Classes qui contiennent des labels ou des values qui ont des defaultProperty | Fichiers de resource `.properties` dans les différentes langues de l'application. Les clés sont les clés de traduction des labels des propriétés du modèle, et dont les valeurs sont les labels définis dans le modèle dans la langue de développement, ou leur traduction |

Le générateur de resources s'appuie sur les `Label` des propriétés, ainsi que sur les traductions récupérées dans le cadre de la configuration du [multilinguisme](/model/i18n).

Il suffit d'ajouter la configuration `resourcesPath` au générateur comme suit :

```yaml
jpa:
  - tags:
      - dto
    resourcesPath: resources/i18n/model # Chemin des fichiers de ressource générés.
```

Pour que, pour chaque module, soient générés les fichiers de resources dans les différentes langues configurées globalement.

Par défaut, les fichiers sont générés avec l'encodage Latin1, mais il est possible de les générer en UTF8 avec la propriété `resourcesEncoding` :

```yaml
jpa:
  - tags:
      - dto
    resourcesPath: resources/i18n/model # Chemin des fichiers de ressource générés.
    resourcesEncoding: UTF8 # Encodage fichiers de ressource générés (Latin1 ou UTF8).
```

## Configuration

### `resourcesPath`

Localisation des ressources, relative au répertoire de génération.

_Variables par tag_: **oui**

### `resourcesEncoding`

Encodage des fichiers de ressources. Les valeurs possibles sont :

- `Latin1` : valeur par défaut
- `UTF8`

_Variables par tag_: **non**
