# Mappers

Dans le modèle d'une application, on utilise très souvent des classes dédiées pour les différentes APIs pour exposer des objets persistés, au lieu de leurs classes originales. Ces objets peuvent être définis simplement en utilisant des [alias de propriétés](./aliases#alias-de-propriétés) entre la classe spécialisée (le "DTO") et la classe persistée. En revanche, on va devoir régulièrement effectuer des "conversions" entres ces deux types de classes, ce qui va nécessiter d'être en mesure de pouvoir recopier les propriétés simplement entre les deux classes.

Si le besoin est simple, il reste néanmoins fastidieux à faire manuellement à chaque fois. TopModel permet donc de définir des **mappers** sur une classe, pour soit **instancier cette classe à partir d'autres classes** (`from`), soit **instancier une autre classe à partir de cette classe** (**`to`**). Les mappings entre propriétés sont déterminés automatiquement dès lors qu'une propriété est un alias d'une autre, ou à défaut si les propriétés ont le même nom et le même domaine. Il est évidemment possible de personnaliser le mapping au-delà de ce qui est déterminé automatiquement.

## Définition

Les mappers se définissent dans la section `mappers` d'une définition de classe, listés dans l'objet `from` ou `to` selon le sens :

```yaml
mappers:
  from:
    - params:
        - class: Class1
        - class: Class2
  to:
    - class: Class1
    - class: Class2
```

Les mappers `from` et `to` ne sont pas tout à fait symétriques :

- un mapper `from` est un **constructeur** de la classe, par conséquent il peut prendre **plusieurs paramètres** que l'on peut définir dans le mapper (qui doivent tous être des classes du modèle par conséquent).
- un mapper `to` est une **méthode qui recopie les propriétés de la classe vers une autre**, qui n'a donc pas de paramètres à spécifier. La méthode générée s'appelle par défaut `To{{ClasseCible}}` et pourra prendre en paramètre une instance de la classe cible (une nouvelle instance de la classe cible sera crée si non renseignée).

Chaque définition de mapping (qui correspond à un paramètre d'un `from` ou la définition complète d'un `to`) doit/peut spécifier :

- Le nom de la classe cible, `class`, obligatoire. Comme pour les autres références de classes, elle doit être disponible dans le fichier courant.
- Un nom, `name`, facultatif :
  - Pour un `from`, il s'agit du nom du paramètre, qui est par défaut renseigné par le nom de la classe en camelCase. Il devient nécessaire si la même classe est utilisée pour plusieurs paramètres.
  - Pour un `to`, il s'agit du nom du mapper, par défaut `To{{ClasseCible}}`. Il devient nécessaire si plusieurs mappers `to` sont définis vers la même classe.
- Un paramètre de mapper `from` peut également définir le caractère obligatoire du paramètre via `required`. **Tous les paramètres sont obligatoires** par défaut, il conviendra donc de spécifier `required: false` dans le cas contraire.
- Des correspondances de champs personnalisées, `mappings`, facultatifs tant qu'il n'y a pas d'ambiguïté dans les correspondances (voir plus bas pour les mappings découverts automatiquement).

## Mapping depuis une propriété unique (mapper `from` uniquement)

Dans un mapper `from`, en plus de pouvoir spécifier une classe comme paramètre, il est également possible d'avoir une **propriété comme paramètre**.

Cela permet par exemple d'ajouter les champs supplémentaires d'un DTO par rapport à sa classe persistée dans le mapper qui le crée.
Par exemple :

```yaml
class:
  name: MyDTO
  comment: DTO.
  properties:
    - alias:
        class: MyEntity

    - name: MyProperty
      domain: DO_LABEL
      required: true
      comment: Extra property.

    - composition: MyOtherDTO
      name: others
      domain: DO_LIST
      comment: Other DTOs

  mappers:
    from:
      - params:
          - class: MyEntity
          - property:
              alias:
                class: MyDTO
                property:
                  - MyProperty
                  - MyOtherDTO
```

Le mapping se fera vers la propriété de la classe qui a le même nom. On vérifie que les deux propriétés ont le même domaine, et dans le cas d'une composition, que ce sont bien des compositions des deux côtés et de la même classe. Il est possible de surcharger la propriété cible en renseignant `target` (au même niveau que `property`), si jamais les noms ne peuvent pas correspondre pour une raison ou une autre.

Le paramètre est obligatoire si la propriété est obligatoire (via `required`). La valeur par défaut de la propriété sera utilisée dans le mapper si elle est renseignée. Par conséquent, une propriété avec une valeur par défaut sera forcément non obligatoire.

## Compatibilité d'un mapping

### Considérations générales

Pour réaliser des mappings, on distingue :

- des propriétés dites "primitives", sont pas représentées par des classes, comme une association `useClass: true` ou une composition (sauf vers une classe `enum: true`, cela reste primitif)
- des propriétés dites "classes", représentées par des classes
- des propriétés de "collection", qui représentes une collection de primitives ou une collection de classes. Une propriété est dite de collection si son domaine est marqué `collection: true`.

De plus, pour réaliser un mapping d'une propriété vers une autre, il est toujours nécessaire que **le domaine de la propriété cible soit le même** que celui de la propriété source, ou bien qu'**il existe une conversion entre les deux** dans le bon sens. Les `converters` sont décrits dans un paragraphe ultérieur. Pour les mappings impliquant des classes, cette condition ne s'appliquera que si au moins l'un des deux domaines est générique (puisqu'une composition peut ne pas avoir de domaine, il ne sera donc forcément pas générique).

### Mappings possibles

Un mapping entre deux propriétés est **possible** dans les cas suivants :

- Les deux propriétés sont des primitives de domaines compatibles.
- Les deux propriétés sont des classes de domaines compatibles, et au choix :
  - Ce sont les mêmes classes.
  - Un mapper existe depuis la classe source vers la classe cible (soit un mapper `to` sur la classe source, soit un mapper `from` sur la classe cible avec la classe source comme unique paramètre).
- Les deux propriétés sont des collections de classes de domaines compatibles (avec les mêmes contraintes que la cas précédent).
- La propriété source est une classe qui contient une clé primaire et la propriété cible une primitive : la clé primaire sera mappée vers la propriété cible si les domaines sont compatibles.
- La propriété source est une collection de classes et la propriété cible une collection de primitives (même chose que le cas précédent).
- La propriété source est une primitive et la classe cible une classe enum readonly, et la propriété source est une référence (alias ou association) vers la clé primaire de la classe cible. Le mapping récupèrera l'instance statique de la classe cible correspondant à la valeur de la clé primaire.
- Une collection de primitives vers une collection de classes enum readonly (même chose que le cas précédent).

## Réalisation des mappings

Les mappings entre propriétés de classes peuvent être explicités dans la section `mappings` de la classe mappée en question. Un mapping ne sera accepté que s'il est possible, donc s'il correspond à un cas décrit dans le paragraphe précédent.

Il n'est **pas possible d'initialiser deux fois la même propriété dans un mapper** (quelque soit le sens). En revanche, il est bien possible d'initialiser deux propriétés à partir de la même propriété.

**Si une propriété n'est pas initialisée** dans la section `mappings`, TopModel va **essayer de trouver un mapping correspondant** selon les priorités suivantes :

1. La propriété de la classe courante est un alias de la propriété de la classe cible (ou les deux propriétés sont des alias de la même propriété).
2. La propriété de la classe courante a le même nom que la propriété de la classe cible.

Le mapping sera ensuite ajouté s'il est possible. Puisqu'il n'est pas possible d'initialiser deux fois la même propriété, une erreur sera levée plusieurs mappings automatiques ont été trouvés vers la même propriété. Pour lever l'ambiguïté, ou si un mapping n'est pas souhaité, il est possible de le désactiver dans la section `mappings` en renseignant `MyProperty: false` à l'intérieur.

Quelques exemples :

```yaml
# La classe courante possède 2 propriétés "Propriete" et "Propriete1" qui sont toutes les deux des alias de "Propriete", il y a donc ambiguïté dans un mapper "to".
- class: Classe1
  mappings:
    Propriete1: Propriete
    Propriete: false
```

La propriété à gauche est toujours celle de la classe courante (pour un `from` comme un `to`), tandis que la propriété à droite est celle de la classe cible.

```yaml
# Les deux classes définissent toutes les deux une propriété "Propriete", il y a donc ambiguïté car TopModel ne peut pas savoir laquelle des deux il faut choisir.
- params:
    - class: Classe1
    - class: Classe2
      mappings:
        Propriete: false
```

## Converters

Pour mapper deux champs de domaine différents, il est possible de définir un `converter`. Si la conversion ne nécessite pas d'opération particulière (mapper un champ `email` vers `libelle` par exemple), alors la définition est simple :

```yaml
converter:
  from:
    - DO_EMAIL
  to:
    - DO_LIBELLE
```

Si la conversion nécessite une opération en `java` ou en `csharp`, par exemple pour appeler la méthode `toString()`, alors il est possible d'écrire le template de la conversion :

```yaml
converter:
  from:
    - DO_LONG
  to:
    - DO_LIBELLE
  java:
    text: "{value}.toString()"
```

Ce template peut prendre les variables du domaine `from` ou du domain `to`, en précisant `from.type` par exemple, pour obtenir le type `java` ou `csharp` du domaine.

Il est interdit de définir plusieurs converters pour le même couple de domaines `from` - `to`. En revanche, dans une seule définition de converter, il est possible de définir plusieurs conversions identiques. Par exemple :

```yaml
converter:
  from:
    - DO_LONG
    - DO_DECIMAL
    - DO_MONTANT
  to:
    - DO_LIBELLE
    - DO_LIBELLE_COURT
  java:
    text: "{value} != null ? {value}.toString() : null"
```

## Gestion de l'héritage

Lorsqu'une classe A hérite d'une classe B, alors toutes les propriétés de la classe B sont automatiquement recopiées dans le mapping de A. Ainsi, toutes les propriétés de B pourront être mappée dans un sens ou dans l'autre (mapper `from` ou `to`).
