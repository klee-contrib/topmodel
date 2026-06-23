# Configuration

Cette page documente les options de configuration **transverses** du module JPA, c'est-à-dire celles qui ne sont pas rattachées à un générateur en particulier. Pour les options propres à chaque générateur, consultez la page correspondante :

- [Classes](/generator/jpa/classes)
- [DAOs](/generator/jpa/daos)
- [Mappers](/generator/jpa/mappers)
- [Endpoints](/generator/jpa/endpoints)
- [Dataflows](/generator/jpa/dataflows)
- [Resources](/generator/jpa/resources)
- [MetaModel](/generator/jpa/metamodel)

## Compatibilité avec les options TopModel

Le générateur JPA est compatible avec les options globales de TopModel :

- **`preservePropertyCasing`** : Lorsque cette option est activée dans la configuration TopModel, les noms de propriétés conservent leur casse d'origine (camelCase, PascalCase, etc.) au lieu d'être normalisés. Le générateur JPA adapte automatiquement les noms des getters et setters en conséquence.

## Templating commun

La plupart des propriétés de chemin acceptent un templating via les variables suivantes :

- `{app}` : nom de l'application
- `{module}` : module du fichier courant
- Variables personnalisées définies dans la configuration

Le chemin des fichiers cibles sera calculé en remplaçant les `.` et le `:` par des `/` dans ces valeurs, tandis que le nom du package des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

De nombreuses propriétés supportent également des **variables par tag**, ce qui permet de générer plusieurs fois la même chose si un fichier est associé à plusieurs tags.

## Options transverses

### `rootModule`

Définition du module racine, pour les différents regroupements à faire dessus (fichiers de traductions, noms de clients Feign, etc.). Cette propriété est utilisée notamment pour :

- Déterminer le nom des fichiers de ressources générés (fichiers `.properties` de traduction)
- Définir l'attribut `name` de l'annotation `@FeignClient` lors de la génération de clients Feign

_Templating_: `{module}`

_Valeur par défaut_: `{module:head}`

### `generatedHint`

Option pour générer l'annotation `@Generated("TopModel : https://github.com/klee-contrib/topmodel")` sur l'ensemble des classes et interfaces générées. Cette annotation permet de retrouver rapidement la documentation de TopModel pour les développeurs qui tomberaient sur du code généré sans contexte.

_Valeur par défaut_: `true`

### `useJdbc`

Génération en mode JDBC au lieu de JPA. Dans ce mode, les entités sont générées sans annotations JPA, mais avec des annotations JDBC simples. Les DAOs héritent de `CrudRepository` au lieu de `JpaRepository`.

_Valeur par défaut_: `false`

**Note :** En mode JDBC, les enums ne sont pas supportés de la même manière qu'en mode JPA. Les classes avec des valeurs ne peuvent pas utiliser le mode enum.

## Exemple de configuration

Voici un exemple de configuration complet du générateur JPA, mixant plusieurs générateurs :

```yaml
jpa:
  - tags:
      - dto
      - entity
    outputDirectory: ./jpa/src/main/javagen # Dossier cible de la génération
    entitiesPath: topmodel/exemple/name/entities # Dossier cible des entités persistées
    daosPath: topmodel/exemple/name/daos # Dossier cible des DAO
    dtosPath: topmodel/exemple/name/dtos # Dossier cible des objets non persistés
    enumsPath: topmodel/exemple/name/enums # Dossier cible des enums
    apiPath: topmodel/exemple/name/api # Dossier cible des API
    apiGeneration: Server # Mode de génération de l'API (Client ou Server)
    fieldsEnum: ["persisted"] # Classes dans lesquelles le générateur doit ajouter une enum des champs
    fieldsEnumInterface: topmodel.exemple.utils.IFieldEnum<> # Interface dont doivent hériter ces enums
```
