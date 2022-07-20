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

Il est possible de préfixer ou suffixer le nom de la propriété recopiée (`true` est une valeur spéciale qui correspond au nom de la classe), et même de surcharger son libellé, commentaire et caractère obligatoire.

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

Enfin, via la propriété `asListWithDomain`, il est possible de recopier une propriété en tant que liste, en spécifiant un domaine qui devra correspondre au type "généré" par cet alias. Le type défini dans le domaine sera ignoré.

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

Cette fonctionnalité est utile lorsqu'on a plusieurs générateurs et que l'on veut qu'un d'entre eux n'ait accès qu'à une partie d'un fichier, sans avoir à revoir intégralement la structure des fichiers pour isoler la partie commune (ce qui peut être particulièrement embêtant lorsqu'il s'agit d'endpoints où un fichier correspond à un contrôleur ou un client).

Le fichier référencé par l'alias doit être listé dans la section `uses`.
