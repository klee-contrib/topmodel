# Les classes non persistées

Pour communiquer entre les différentes couches d'une application, il généralement conseillé d'utiliser des objets de transfert, appelés aussi Dto. Ces objets sont **non persistés**, et sont généralement construits à partir des objets persistés. Ils en sont souvent des **fractions**, et peuvent en **aggréger** plusieurs.

## Définition d'un objet non persisté

Dans TopModel, il est possible de définir un objet non persisté exactement de la même manière qu'un objet persisté. La seule différence étant l'absence de **clé primaire**, dans un objet de transfert.

Exemple avec la classe `ProfilDto` (à ne pas reproduire, lire la suite) :

```yaml
---
class:
  name: ProfilDto
  comment: Objet de transfert pour la classe Profil
  properties:
    - name: Nom
      comment: Nom du profil
      label: Profil
      domain: DO_LIBELLE
      required: true
```

La classe `ProfilDto` ne contient qu'un libelle, pas de clé primaire, et sera donc considérée comme une classe non persistée.

## Alias

Evidemment, il est extrêmement laborieux de définir `ProfilDto` de cette manière. Il serait préférable de matérialiser le lien fort entre la propriété `Nom` de `Profil` et la propriété `Nom` de `ProfilDto`.

Pour cela, TopModel permet de définir des propriétés d'`Alias`. L'objectif est de référencer une propriété ou un ensemble de propriétés définis dans une autre classe.

### Alias vers une propriété

Pour définir un alias vers une unique propriété, nous devons renseigner deux informations : la classe visée, et la propriété concernée.
Dans la liste des propriétés d'une classe non persistée, cela se traduit de cette manière :

```yaml
# Dto.yml
---
class:
  name: ProfilDto
  comment: Objet de transfert pour la classe Profil
  properties:
    - alias:
        class: Profil # Classe cible de l'alias
        property: Nom # Propriété cible de l'Alias
```

> La classe en question doit être accessible depuis le fichier sur lequel nous travaillons. Si elle n'est pas définie dans le fichier courant, alors le fichier qui la contient doit être importé dans les `uses`.

Ainsi, la classe `ProfilDto` contiendra une propriété `Nom`, qui aura exactement les mêmes caractéristiques que la propriété `Nom` dans la classe `Profil`, à savoir :

- Son domaine
- Son libelle
- Son commentaire
- Le fait qu'elle soit requise
- etc

**Néanmoins** si la propriété en question est une clé primaire, elle ne fera pas de la classe `ProfilDto` une classe persistée.

#### Préfix/Suffix

Dans l'éventualité ou l'on créerait une classe contenant des alias vers plusieurs classes différentes, nous pourrions nous retrouver avec des conflits sur les noms des propriétés. Pour les distinguer, nous pouvons alors ajouter aux noms des propriétés d'alias un `prefix` ou un `suffix`, qui viendront respectivement se placer  avant ou après le nom de la propriété d'alias. Si le `suffix`/`prefix` est `true`, alors la chaîne de caractère ajoutée sera le nom de la classe

```yaml
# Dto.yml
---
class:
  name: ProfilDto
  comment: Objet de transfert pour la classe Profil
  properties:
    - alias:
        class: Profil
        property: Nom
      suffix: true
```

Ici, `ProfilDto` contiendra une propriété `NomProfil`, ayant les mêmes attributs que la propriété `Nom` de la classe `Profil`.

#### Surcharger les attributs

En dehors du **nom** et du domaine, il est possible de surcharger tous les attributs de la propriété cible de l'alias. Il suffit pour cela de repréciser la nouvelle valeur dans la propriété d'alias

```yaml
# Dto.yml
---
class:
  name: ProfilDto
  comment: Objet de transfert pour la classe Profil
  properties:
    - alias:
        class: Profil
        property: Nom
      required: false # Surcharge de la valeur du champ required. 
```

**Attention** : ces surcharges se placent au même niveau que le mot clé `alias`.

### Alias vers un ensemble de propriétés

Pour créer des Dtos **fragments** de classes persistées, il existe des raccourcis permettant d'ajouter des ensembles de propriétés. Dans l'exemple ci-dessus, retirons simplement la précision de la propriété cible, et TopModel comprendra qu'il faut faire un alias sur **l'ensemble des propriétés** de la classe cible.

Exemple :

```yaml
# Dto.yml
---
class:
  name: ProfilDto
  comment: Objet de transfert pour la classe Profil
  properties:
    - alias:
        class: Profil
```

> Les surchages évoquées au paragraphes précédent s'appliqueront à toutes les propriétés aliasées

#### Include/Exclude

Pour ajuster l'ensemble des propriétés aliasées, nous pouvons soit exclure une liste de propriétés de la classe cible, soit préciser quelles propriétés nous souhaitons reprendre.

Exemple avec `include`

```yaml
# Dto.yml
---
class:
  name: ProfilDto
  comment: Objet de transfert pour la classe Profil
  properties:
    - alias:
        class: Profil
        include:
          - Nom # La liste des propriétés à inclure. Toutes les autres seront ignorées
```

Exemple avec `exclude`

```yaml
# Dto.yml
---
class:
  name: ProfilDto
  comment: Objet de transfert pour la classe Profil
  properties:
    - alias:
        class: Profil
        exclude:
          - Id # La liste des propriétés à exclure. Toutes les autres seront ajoutées à la classe ProfilDto
```

Les deux exemples ci-dessus produisent exactement le même résultat que le premier exemple proposé : `ProfilDto` contiendra une propriété `Nom`, ayant les mêmes attributs que la propriété `Nom` de la classe `Profil`.

> **Astuce** : il est tout à fait possible d'ajouter deux alias vers la même classe. Cette pratique permet notamment de surcharger différemment des ensembles de propriétés.

## Mappers

Avec la possibililité de créer aisément des Dtos vient la nécessité de transformer nos entités persistées en dtos et vice et versa. L'usage de mappers par convention de nommage peut être hasardeuse, et TopModel peut mieux faire. En effet, le modèle contient, dans sa description, le lien fort qu'entretiennent les propriétés des deux côtés des alias. C'est pourquoi TopModel donne la possibilité de créer des `mappers`. La correspondance entre les champs se fera d'abord par **correspondance entre alias**. Puis, les propriétés restantes seront mappées avec la règle **même nom et même domaine**.

S'il y a ambiguité sur les mappings, où si certains champs doivent être ignorés, il est possible de donner des précisions sous l'attribut `mappings`.

### Mapper To

Prenons d'abord la classe `UtilisateurCreateDto` définie telle que :

```yaml
---
class:
  name: UtilisateurCreateDto
  comment: Objet de transfert pour la classe Utilisateur
  properties:
    - alias:
        class: Utilisateur
        exclude:
          - Id
      prefix: true
```

Dans l'application qui utilisera ce modèle, on souhaite donner la possibilité de renseigner un nouvel utilisateur en renseignant toutes ses propriétés, sauf l'identifiant technique.
Lorsque sont saisies ces informations, nous obtenons une `UtilisateurDto` que nous souhaiterons convertir en `Utilisateur` afin de le sauvegarder en base de données. Créons donc un mapper `UtilisateurCreateDto -> Utilisateur`.

Il serait en théorie possible de créer soit :

- Un mapper `from` sur la classe `Utilisateur` qui prend comme paramètre un `UtilisateurCreateDto`
- Un mapper `to` sur la classe `UtilisateurDto` qui prend comme classe destination un `Utilisateur`

Mais, le plus souvent, les classes persistées et non persistées sont définis dans des fichiers différents. Ainsi le fichier dans lequel est déclaré `UtilisateurCreateDto` a une dépendance au fichier dans lequel est défini `Utilisateur`. Pour éviter d'introduire une dépendance circulaire, nous sommes obligés de choisir la deuxième option, **créer un mapper `to` sur la classe `UtilisateurCreateDto`**.

Voici comment l'ajouter à notre définition de classe :

```yaml
---
class:
  name: UtilisateurCreateDto
  comment: Objet de transfert pour la classe Utilisateur dans le cas d'une création
  properties:
    - alias:
        class: Utilisateur
        exclude:
          - Id
      prefix: true
  mappers: # Définition des mappers
    to: # Mappers to ou from
      - class: Utilisateur # Définition de notre premier mapper, vers la classe Utilisateur
```

Nous avons donc défini un mapper de la classe `UtilisateurCreateDto` vers la classe Utilisateur

### Mapper From

Considérons maintenant la classe `UtilisateurSearchResultDto`, qui représente les résultats d'une recherche dans la table `Utilisateur`. elle se définit comme suit :

```yaml
---
class:
  name: UtilisateurSearchResultDto
  comment: Objet de transfert pour la classe Utilisateur, dans le cas d'une recherche
  properties:
    - alias:
        class: Utilisateur
        exclude: 
          - ProfilId
    - alias:
        class: Profil
        include:
          - Nom
      suffix: true
```

Ici, notre résultat de recherche devra renvoyer tous les champs de la classe `Utilisateur`, ainsi que le nom de son `Profil`.

Nous aurions donc besoin d'un `mapper` pour construire ces objets `UtilisateurSearchResultDto`. Idéalement, ce `mapper` doit pouvoir prendre en paramètres une instance de la classe `Utilisateur`, et une instance de la classe `Profil` correspondante.

Un tel mapper s'écrit de la façon suivante :

```yaml
---
class:
  name: UtilisateurSearchResultDto
  comment: Objet de transfert pour la classe Utilisateur, dans le cas d'une recherche
  properties:
    - alias:
        class: Utilisateur
    - alias:
        class: Profil
        include:
          - Nom
      suffix: true
  mappers:
    from: # Liste des mappers From
      - params: # Liste des paramètres du mapper
        - class: Utilisateur # Premier paramètre, la classe Utilisateur
        - class: Profil # Deuxième paramètre, la classe Profil
```

Nous avons donc défini un mapper `from`, prenant deux paramètres, `Utilisateur` et `Profil`, permettant de créer une instance de la classe `UtilisateurSearchResultDto`.

Pour plus de détails sur les cas d'usage avancés, se rapporter à la section [Mappers](/model/properties?id=mappers)
