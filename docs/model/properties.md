# Propriétés

Les propriétés sont définies aussi bien dans des classes que pour des endpoints.

Il y a au total 3 types de propriétés :

## Propriété "standard"

C'est une propriété tout ce qu'il y a de plus standard (comme son nom l'indique). Elle est identifiée par la présence de la propriété `name` en premier.

Elle correspond à un champ "primitif" (<=> ne référence pas une autre classe), et doit donc être obligatoirement associée à un [domaine](/model/domains.md). Elle peut être marquée comme étant la clé primaire de la classe.

Exemple :

```yaml
name: MontantInitialPret
label: Montant initial du prêt
required: false
domain: DO_MONTANT
comment: Montant initial du prêt.
```

## Association

Une association est une propriété spéciale qui permet de **référencer la clé primaire d'une autre classe**. L'usage principal est de pouvoir définir des clés étrangères dans un modèle persisté. Elle est identifiée par la présence de la propriété `association` en premier.

Une association peut être obligatoire (ou non) et définir un rôle optionnel. Le nom de la propriété sera déterminé automatiquement comme étant `{ClasseCible}{CléPrimaire}{Rôle}`. Une association peut aussi définir une multiplicité ("one to one", "one to many"...), mais cette information n'est en général pas utilisée par les divers générateurs, qui supposent qu'il s'agit toujours d'une "one to many".

La classe référencée par l'association doit être connue du fichier de modèle courant, soit parce qu'elle est définie dedans, soit parce que son fichier est référencé dans la section `uses`.

Exemple :

```yaml
association: ClasseCible
required: true
comment: C'est une FK obligatoire
```

## Composition

Une composition est une propriété spéciale qui permet de **référencer une autre classe**, à l'inverse de l'association qui ne concerne que la clé primaire. Par conséquent, la composition est le seul type de **propriété non primitif**, ce qui à priori proscrit son usage dans un objet persisté (dans lequel on utilisera plutôt une association). Elle est identifiée par la présence de la propriété `composition` en premier.

Exemple :

```yaml
composition: ClasseCible
name: ClasseCibleList
kind: list
comment: C'est une liste
```

Une composition doit avoir un type, qui indique de quelle façon la classe sera référencée. Les deux types par défaut sont `"object"` et `"list"`, pour indiquer si on veut juste une instance de la classe ou bien une liste. Il est possible d'utiliser un **type personnalisé** en renseignant un nom de [domaine](/model/domains.md) dans la propriété **`kind`**. Le type du domaine sera utilisé comme "conteneur" de la classe.

Par exemple, avec

```yaml
---
domain:
  name: DO_SET
  label: Set
  csharp:
    type: HashSet
---
class:
  name: MyClass

  properties:
    - composition: ClasseCible
      name: ClasseCibleSet
      kind: DO_SET
      comment: C'est un set
```

La classe C# générée aura une propriété `ClasseCibleSet` de type `HashSet<ClasseCible>`.

La classe référencée par la composition doit être connue du fichier de modèle courant, soit parce qu'elle est définie dedans, soit parce que son fichier est référencé dans la section `uses`.

Il est également possible de définir un pattern pour la classe générique. Pour cela, dans la définition du domaine, il est possible d'écrire

```yaml
domain:
  name: DO_DICTIONNAIRE
  label: Dictionnaire
  csharp:
    type: Dictionary<string, {class}>
  ts:
    type: Map<String, {class}>
```

Ainsi, les propriétés de composition générées avec le `kind: DO_DICTIONNAIRE` auront la forme du pattern défini, `{class}` étant remplacé par la classe cible.

## Alias

[Voir section correspondante](/model/aliases.md?id=alias-de-propriétés)
