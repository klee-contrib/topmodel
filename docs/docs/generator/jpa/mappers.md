# Génération des mappers

| Nom                  | Condition d'activation | Objets ciblés | Fichiers générés                                                                                   |
| -------------------- | ---------------------- | ------------- | -------------------------------------------------------------------------------------------------- |
| `JpaMapperGenerator` | Toujours               | Mappers       | Classe statique contenant des méthodes statiques, correspondant aux mappers définis dans le modèle |

Les mappers sont générés comme des méthodes statiques dans une classe statique. Cette classe rassemble tous les mappers d'un module racine. Elle est positionnée dans le package des entités si l'une des classes impliquées est persistée, et dans le package des DTOs sinon.

**Remarque :** Le module utilisé pour un mapper est celui de la classe persistée qui a été trouvée, ou à défaut celui de la classe qui définit le mapper.

## Localisation et nommage des fichiers générés

Les mappers d'un même module racine sont regroupés dans une unique classe Java. Le nom du fichier dépend du package dans lequel le fichier est généré :

- Dans le package contenant les classes persistées (`entitites`): `{ModulePascalCase}Mappers.java` (exemple : `RestaurantMappers`)
- Dans le package contenant les classes non persistées (`dtos`) : `{ModulePascalCase}DTOMappers.java` (exemple : `RestaurantDTOMappers`)

Une classe est considérée comme **persistée** si elle a une clé primaire, **ou** si l'un de ses tags est listé dans la configuration [`mapperTagsOverrides`](#mappertagsoverrides) :

Si une des classes impliquées dans le mapper est une classe persistée, alors le mapper sera généré dans le package des classes persistées (`entities`).

Si toutes les classes impliquées dans le mappers sont non persistées, alors le mapper sera généré dans le package des classes non persistées (`dtos`).

L'option `mapperTagsOverrides` permet ainsi de forcer la génération dans `entities/` même lorsqu'aucune des classes impliquées n'est effectivement persistée.

## Structure d'un fichier de mappers

Chaque fichier contient une classe `public` portant le nom décrit ci-dessus et :

- Une annotation `@Generated("TopModel : https://github.com/klee-contrib/topmodel")` si l'option `generatedHint` est activée.
- Un constructeur `private` (sans paramètre) afin d'empêcher l'instanciation de la classe utilitaire.
- Toutes les méthodes statiques correspondant aux mappers `from` puis aux mappers `to`.

```java
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class RestaurantMappers {

    private RestaurantMappers() {
        // private constructor to hide implicite public one
    }
    // ... méthodes générées ...
}
```

## Mappers `from`

Les mappers `from` sont générés sous deux formes :

- **`create[Nom de la classe à créer]`** : crée une nouvelle instance de la classe cible en mappant les champs sources. Cette méthode délègue à `map[Nom de la classe à créer]` en lui passant une nouvelle instance comme cible.
- **`map[Nom de la classe à créer]`** : mappe les champs sources sur une instance de la classe cible passée en paramètre. Cette méthode peut être utilisée pour peupler une instance existante.

Les deux méthodes prennent en entrée la liste des paramètres d'entrée définis dans le mapper : d'abord les `classParams` (instances d'autres classes), puis les `propertyParams` (valeurs scalaires additionnelles). La méthode `mapXXX` prend également une instance de la classe cible en dernier paramètre.

**Remarque :** la méthode `createXXX` n'est pas générée pour les classes abstraites et les interfaces (`type` `abstract` ou `interface`) (seule la méthode `mapXXX` l'est).

### Contrôles de nullité

La méthode `mapXXX` lève une `IllegalArgumentException` dans les cas suivants :

- Si `target` est `null`.
- Pour chaque `classParam` dont `required` vaut `true` : si le paramètre correspondant est `null`.
- Pour chaque `propertyParam` dont la propriété est `required` : si la valeur correspondante est `null`. Les paramètres correspondant à une association persistante sur une classe persistante sont ignorés pour ce contrôle (la clé étrangère étant gérée via l'association).

### Exemple

```java
public static AvisClientRead createAvisClientRead(AvisClient avisClient) {
    return mapAvisClientRead(avisClient, new AvisClientRead());
}

public static AvisClientRead mapAvisClientRead(AvisClient avisClient, AvisClientRead target) {
    if (target == null) {
        throw new IllegalArgumentException("target cannot be null");
    }

    if (avisClient == null) {
        throw new IllegalArgumentException("avisClient cannot be null");
    }

    target.setId(avisClient.getId());
    target.setNote(avisClient.getNote());
    // ... propriétés simples ...
    if (avisClient.getClient() != null) {
        target.setClientId(avisClient.getClient().getId());
    } else {
        target.setClientId(null);
    }

    return target;
}
```

### Mappers `from` avec `propertyParams`

Lorsque le mapper déclare des `propertyParams` (propriétés scalaires additionnelles), ceux-ci sont ajoutés à la signature après les `classParams`. Les valeurs sont simplement affectées via les setters correspondants.

```java
public static RestaurantAvecStatistiques createRestaurantAvecStatistiques(
        Restaurant restaurant, Integer nombrePlats, Integer nombreTables, BigDecimal noteMoyenne) {
    return mapRestaurantAvecStatistiques(restaurant, nombrePlats, nombreTables, noteMoyenne, new RestaurantAvecStatistiques());
}
```

## Mappers `to`

Les mappers `to` sont nommés d'après le `name` du mapper défini dans le modèle, converti en `camelCase` (valeur par défaut : `to{NomClasseSource}`). Le paramètre source est unique et obligatoire.

Deux formes sont générées :

- **`to[Nom](source)`** : crée une nouvelle instance de la classe cible et délègue à la méthode `to[Nom](source, target)`. Cette variante n'est **pas** générée lorsque la classe cible est `abstract`.
- **`to[Nom](source, target)`** : mappe les champs sources sur l'instance `target` passée en paramètre.

La méthode avec `target` lève une `IllegalArgumentException` si `source` ou `target` est `null`.

### Exemple (avec mapping d'enum)

```java
public static Plat toPlat(PlatWrite source) {
    return toPlat(source, new Plat());
}

public static Plat toPlat(PlatWrite source, Plat target) {
    if (source == null) {
        throw new IllegalArgumentException("source cannot be null");
    }

    if (target == null) {
        throw new IllegalArgumentException("target cannot be null");
    }

    target.setNom(source.getNom());
    // ... propriétés simples ...
    if (source.getCategoriePlatCode() != null) {
        target.setCategoriePlat(new CategoriePlat(source.getCategoriePlatCode()));
    } else {
        target.setCategoriePlat(null);
    }

    return target;
}
```

## Règles de nullité et de conversion

Le code généré pour chaque mapping dépend du type des propriétés source et cible. Les cas suivants sont gérés (par `GetSourceGetter` dans `JpaMapperGenerator.cs`) :

### Propriétés simples

Lorsque les deux propriétés sont simples et compatibles, une affectation directe est générée :

```java
target.setNote(avisClient.getNote());
```

Si les domaines diffèrent, la conversion déclarée par le domaine est appliquée automatiquement.

### Association vers clé étrangère (propriété)

Lorsqu'une association simple est projetée vers une clé étrangère (typiquement `id`), le getter est protégé par un `null`-check avec fallback à `null` :

```java
if (avisClient.getClient() != null) {
    target.setClientId(avisClient.getClient().getId());
} else {
    target.setClientId(null);
}
```

### Collection d'associations vers collection de clés étrangères

Une collection d'associations est projetée via un `stream()` filtrant les valeurs nulles :

```java
if (restaurant.getMenus() != null) {
    target.setMenus(restaurant.getMenus().stream().filter(Objects::nonNull).map(Menu::getId).collect(Collectors.toList()));
} else {
    target.setMenus(null);
}
```

Si l'association source est elle-même multi-niveaux (collection d'associations dont une propriété est également une association), un `.map(...).filter(Objects::nonNull)` intermédiaire est inséré :

```java
if (restaurant.getPromotions() != null) {
    target.setPromotions(restaurant.getPromotions().stream().filter(Objects::nonNull).map(Promotion::getPlat).filter(Objects::nonNull).map(Plat::getId).collect(Collectors.toList()));
} else {
    target.setPromotions(null);
}
```

### Composition objet vers objet (classes différentes)

Lorsqu'une composition est projetée vers une composition d'une autre classe, le générateur délègue au mapper approprié (`createXXX`/`mapXXX` pour un `from`, `toXXX` pour un `to`). Si la cible possède déjà une instance, la variante `mapXXX(source, target.getX())`/`toXXX(source, target.getX())` est utilisée pour préserver l'instance ; sinon une nouvelle instance est créée :

```java
if (commande.getClient() != null) {
    target.setClient(target.getClient() != null
        ? RestaurantMappers.mapClientRead(commande.getClient(), target.getClient())
        : RestaurantMappers.createClientRead(commande.getClient()));
} else {
    target.setClient(null);
}
```

Le même schéma s'applique pour les mappers `to` :

```java
if (source.getClient() != null) {
    target.setClient(target.getClient() != null
        ? RestaurantMappers.toClient(source.getClient(), target.getClient())
        : RestaurantMappers.toClient(source.getClient()));
} else {
    target.setClient(null);
}
```

### Collection de compositions (classes différentes)

Les collections de compositions sont projetées en streamant et en déléguant à la méthode `createXXX`/`toXXX` via une méthode de référence :

```java
if (commande.getLignes() != null) {
    target.setLignes(commande.getLignes().stream().filter(Objects::nonNull).map(RestaurantMappers::createLigneCommandeRead).collect(Collectors.toList()));
} else {
    target.setLignes(null);
}
```

### Clé étrangère vers association (enum)

Lorsqu'une clé étrangère scalaire est projetée vers une association (typiquement une enum à code), le générateur instancie la classe cible via son constructeur dédié :

```java
if (source.getCategoriePlatCode() != null) {
    target.setCategoriePlat(new CategoriePlat(source.getCategoriePlatCode()));
} else {
    target.setCategoriePlat(null);
}
```

### Collection de clés étrangères vers collection d'associations (enums)

Le principe est identique, appliqué sur un `stream()` :

```java
if (source.getCategoriesPlat() != null) {
    target.setCategoriesPlat(source.getCategoriesPlat().stream().filter(Objects::nonNull).map(CategoriePlat::new).collect(Collectors.toList()));
} else {
    target.setCategoriesPlat(null);
}
```

## Intégration dans les classes (`mappersInClass`)

Lorsque `mappersInClass: true`, les mappers sont également exposés directement sur les classes qui les déclarent :

- **Pour chaque mapper `from`**, un **constructeur** est généré. Ce constructeur prend en paramètre les `classParams` et `propertyParams` du mapper, et délègue à la méthode `Mappers.map[Nom de la classe](..., this)` de la classe utilitaire.
- **Si au moins un `FromMapper`** a tous ses `classParams` disponibles, un **constructeur sans argument** est également généré (afin de préserver la possibilité d'instancier la classe sans paramètre).
- **Pour chaque mapper `to`**, une méthode `toXXX(target)` est générée sur la classe source. Cette méthode délègue à `Mappers.toXXX(this, target)`. La version sans `target` n'est pas ajoutée dans la classe (elle n'est disponible que sur la classe utilitaire).

Cette option est désactivable (et l'est par défaut) via `mappersInClass: false`.

## Spécificités JDBC (`useJdbc: true`)

Lorsque le mode JDBC est activé (`useJdbc: true`), certains mappings sont filtrés ou simplifiés :

- Les mappings impliquant une **composition** (côté source ou cible) sont ignorés.
- Les mappings impliquant une **association multiple** sur une classe persistée (côté source ou cible) sont ignorés.
- Les `null`-checks sur les getters des associations ne sont pas générés : la conversion de domaine est appliquée directement sur le getter source.

Ces restrictions reflètent le fait que, en mode JDBC, les relations objet ne sont pas matérialisées par des graphes d'entités.

## Gestion des erreurs

Si un paramètre d'entrée obligatoire n'est pas renseigné, l'exception `IllegalArgumentException` est lancée. Cela concerne :

- `target` dans les méthodes `mapXXX` et `toXXX(source, target)` ;
- `source` dans les méthodes `toXXX(source, target)` ;
- chaque `classParam` marqué `required: true` ;
- chaque `propertyParam` dont la propriété est `required` (sauf association persistante sur classe persistante).

## Configuration

### `mappersInClass`

Indique s'il faut ajouter les mappers en tant que méthode (`to...`) ou constructeur dans les classes qui les déclarent. Si `true`, les mappers `from` sont générés comme constructeurs et les mappers `to` comme méthodes dans les classes concernées.

_Valeur par défaut_: `false`

### `mapperTagsOverrides`

Si un mapper contient au moins une classe dont le tag est listé ici, alors ce mapper sera généré avec les tags de cette classe, au lieu du comportement par défaut qui priorise les tags de la classe persistée puis de celle qui définit le mapper.

Ceci permet de forcer la localisation de la génération des mappers pour certains tags particuliers, indépendamment du caractère persisté des classes impliquées.

_Valeur par défaut_: `[]`

_Variables par tag_: **non**

**Exemple :**

```yaml
jpa:
  - tags:
      - entity
    mapperTagsOverrides:
      - feign
      - dto-special
```
