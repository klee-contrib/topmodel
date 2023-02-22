# Alias

Dans le modèle d'une application, il est très courant d'avoir besoin de référencer des propriétés définies dans d'autres objets lorsqu'on construit un nouvel objet. L'exemple le plus courant est le cas des DTOs, qui sont des objets qui vont être utilisés dans l'API publique d'une application et qui sont bien souvent presque identiques à des objets persistés, plutôt réservés à une API "interne".

TopModel permet donc de définir des **alias** de propriétés, classes et endpoints, qui pourront être utilisés pour **recopier** des définitions déjà écrites par ailleurs.

## Alias de propriétés

Le cas le plus courant est celui où l'on veut recopier des propriétés d'une classe vers une autre (ou dans la définition d'un endpoint).

Pour se faire, il est possible de définir un `alias` à la place d'une définition de propriété, par exemple :

```yaml
alias:
  class: MyClass
  property: MyProperty1
prefix: true
```

Il est possible de préfixer ou suffixer le nom de la propriété recopiée (`true` est une valeur spéciale qui correspond au nom de la classe), et même de surcharger son libellé, commentaire, caractère obligatoire, domaine, valeur par défaut, caractère readonly...

Plusieurs propriétés peuvent être recopiées par la même définition d'alias, par exemple :

```yaml
# Recopie toutes les propriétés de MyClass avec "MyClass" comme suffixe.
alias:
  class: MyClass
suffix: true

# Recopie les deux propriétés listées de MyClass en marquant qu'elles ne sont pas obligatoires ici.
alias:
  class: MyClass
  include: # ("property" et "include" sont interchangeables)
    - MyProperty1
    - MyProperty2
required: false

# Recopie toutes les propriétés de MyClass sauf "MyProperty3" avec "Class" comme préfixe.
alias:
  class: MyClass
  exclude:
    - MyProperty3
prefix: Class
```

La classe référencée par l'alias doit être connue du fichier de modèle courant, soit parce qu'elle est définie dedans, soit parce que son fichier est référencé dans la section `uses`.

Il n'est pas possible de définir un alias sur une propriété de type composition, mais en revanche il est possible de définir un alias d'alias.

Enfin, via la propriété `asList`, il est possible de recopier une propriété en tant que liste, en utilisant le `listDomain` défini sur le domaine de la propriété aliasée au lieu de son domaine.

Exemple :

```yaml
# Génère une propriété MyClassCodeList, de domaine DO_CODE_LIST et de type string[] (ou MyClassCode[] si le language/générateur supporte les enums), en supposant que Code est du domaine DO_CODE et MyClass est une classe enum.
alias:
  class: MyClass
  include: Code
asList: true
prefix: true
suffix: List
```

## Alias de classes et endpoints

Il est possible de définir, dans un fichier de modèle, un **alias vers un autre fichier de modèle**, pour pouvoir recopier des définitions de classes ou d'endpoints dans ce nouveau fichier.

Par exemple :

```yaml
alias:
  file: Data/MyOtherFile
  classes:
    - Class1
    - Class2
  endpoints:
    - Endpoint1
```

Cette fonctionnalité est utile lorsqu'on a plusieurs générateurs et que l'on veut que l'un d'entre eux ne puisse générer qu'un sous-ensemble d'un fichier, sans avoir à réorganiser entièrement la hiérarchie des fichiers (ce qui peut poser des problèmes pour des endpoints où la génération se fait par fichier).

Le fichier référencé par l'alias doit être listé dans la section `uses`.
