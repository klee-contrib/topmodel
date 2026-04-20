# Annotations

Afin d'enrichir de manière plus personnalisée le code généré (en Java et C#), il est possible de définir des annotations (`annotation`), que l'on peut poser sur l'ensemble des objets de modèle. Ces annotations devront être implémentées séparément pour chaque langage.

Elles s'ajoutent ensuite à la définition d'une classe, d'un endpoint ou d'une propriété, mais aussi dans un domaine ou un décorateur (pour qu'elles soient affectées aux objets associés), sous forme de liste. Les annotations ne sont pas automatiquement disponibles dans tous les fichiers comme les domaines, donc il faudra qu'elles soient importées, soit définies dans le même fichier (comme une classe).

## Exemple pour une classe

En Java, nous pouvons ajouter l'annotation EntityListener, pour ajouter l'annotation `EntityListeners` et les imports y afférent :

```yaml
annotation:
  name: EntityListeners
  description: Entity Listener pour suivre les évènements de création et de modification
  java:
    - text: EntityListeners(AuditingEntityListener.class)
      imports:
        - org.springframework.data.jpa.domain.support.AuditingEntityListener
        - jakarta.persistence.EntityListeners
```

Son utilisation dans la classe `Utilisateur` :

```yaml
---
class:
  name: Utilisateur
  label: Utilisateur
  comment: Utilisateur de l'application
  trigram: UTI
  annotations:
    - EntityListeners
```

Le code généré aura donc l'annotation supplémentaire :

```java
/**
 * Utilisateur de l'application.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "UTILISATEUR")
@EntityListeners(AuditingEntityListener.class)
public class Utilisateur {
/** **/
}
```

## Exemple pour un endpoint

En C#, on pourrait définir une annotation et son utilisation dans un endpoint comme ceci par exemple :

```yaml
---
decorator:
  name: Authorize
  description: Authorize
  parameters:
    - name: policy
      required: true
      comment: Policy d'autorisation.
  csharp:
    - text: Authorize("{policy}") # Les paramètres et le templating sont décrits dans la section suivante.
  properties:
    - name: AdditionalParam
      domain: DO_BOOLEEN
      comment: Param
---
endpoint:
  name: Get
  method: GET
  route: /
  description: Get
  decorators:
    - Authorize:
        policy: interne
```

Cela générera la route suivante dans un contrôleur :

```cs
/// <summary>
/// Get
/// </summary>
/// <param name="additionalParam">Param</param>
/// <returns>Task.</returns>
[Authorize("interne")]
[HttpGet("/")]
public async Task Get()
```

## Application des annotations

### Règles générales

- Les classes, les endpoints et les propriétés peuvent définir leurs propres annotations.
- Une propriété d'alias héritera des annotations de sa propriété originale, et pourra en définir de nouvelles.
- Les annotations posées sur les domaines seront posées sur chaque propriété qui utilise ce domaine.
- Les annotations posées sur les décorateurs seront posées sur chaque classe ou endpoint qui utilise ce décorateur.

### Annotations de propriétés

En plus des annotations posées directement dessus ou héritées de leur domaine, il est également possible de poser des annotations sur des **propriétés au niveau de leur conteneur**, donc la classe, l'endpoint, ou le décorateur.

Par exemple, sur une classe :

```yaml
class:
  name: MyClass
  propertyAnnotations:
    - MyAnnotation
  properties:
    - name: Property1
      domain: DO_DOMAIN
      comment: Propriété 1.
    - name: Property2
      domain: DO_DOMAIN
      comment: Propriété 2.
```

Ainsi, l'annotation `MyAnnotation` sera prise en compte pour le listing des annotations de toutes les propriétés de la classe.

Remarques :

- Ces annotations ne seront disponibles sur chaque propriété **que dans le contexte de la classe ou du endpoint qui les défini**. Elles ne feront dont **pas** partie de la liste des annotations de la propriété, et ne seront donc pas incluses dans les annotations d'un alias de cette propriété par exemple.
- Les annotations de propriétés définies dans un décorateur ne s'appliqueront que sur les propriétés définies dans le décorateur. En revanche, ces propriétés récupèreront aussi les annotations de propriété de la classe ou du endpoint qui utilise ce décorateur. Par exemple, pour :

  ```yaml
  ---
  decorator:
    name: MyDecorator
    description: Mon décorateur.
    propertyAnnotations:
      - MyAnnotation1
    properties:
      name: MyProperty
      domain: DO_DOMAIN
      comment: Ma propriété.
  ---
  class:
    name: MyClass
    comment: Ma classe.
    decorators:
      - MyDecorator
    propertyAnnotations:
      - MyAnnotation2
    properties:
      - name: MyClassProperty
        domain: DO_DOMAIN
        comment: Ma propriété de classe.
  ```

  `MyProperty` dans `MyClass` aura bien `MyAnnotation1` et `MyAnnotation2`, tandis que `MyClassProperty` n'aura que `MyAnnotation2`.

### Priorité des annotations

Il est interdit de déclarer plusieurs fois la même annotation sur un objet. En revanche, les annotations effectives sur les classes, endpoints et propriétés peuvent se retrouver en doublon (par exemple, si une même annotation est définie sur un domaine et une propriété qui utilise ce domaine). Dans le cas où l'annotation définit des paramètres, il est possible que l'instanciation des annotations ne soit pas faite avec les mêmes valeurs. Dans ce cas, la priorité suivante est établie :

- Pour une **propriété**, les paramètres sont récupérés en priorité :
  - Sur la propriété elle-même.
  - Si c'est un alias, sur la propriété originale de l'alias.
  - Sur la classe (via `propertyAnnotations`).
  - Sur le décorateur qui définit la propriété, s'il y en a un (via `propertyAnnotations`).
  - Sur le domaine.
- Pour une **classe** ou un **endpoint**, les paramètres sont récupérés en priorité :
  - Sur la classe/l'endpoint elle/lui-même.
  - Sur le premier décorateur qui définit l'annotation, s'il y en a un.

## Ciblage des annotations

### `target`

Par défaut, une annotation peut se poser sur n'importe quel objet de modèle.

Via la propriété `target` (qui est une liste) sur l'annotation, il est possible de restreindre les objets possibles parmi la liste suivante :

- `class`
- `endpoint`
- `client-endpoint`
- `server-endpoint`
- `property`
- `association-property`
- `composition-property`
- `regular-property`

L'objet cible devra être de l'un des types listés pour que l'annotation puisse être posée (les différents types de propriétés sont en revanche posables sur toutes les propriétés, mais la génération ne sera quand même faite que pour le bon type), avec `class` et les `endpoints` possibles pour un décorateur et les `properties` pour un domaine.

### `when`

En plus de la `target`, il est possible d'ajouter des conditions de génération par annotation sur chaque implémentation via la propriété `when`. C'est aussi une liste qui peut prendre les valeurs suivantes :

- `class-property`
- `endpoint-param`
- `non-persisted`
- `persisted`
- `primary-key`

A l'inverse de la `target`, l'**ensemble des conditions doit être respecté**.

```yaml
annotation:
  name: Email
  description: Annotation pour décrire un email
  target:
    - property # Cette annotation n'existe que pour une propriété
  java:
    - text: "@EmailColumn" # Le @EmailColumn ne sera mis que sur les propriétés persistées (forcément dans une classe)
      when:
        - persisted
    - text: "@EmailValidation" # Le @EmailValidation ne sera mis que sur les propriétés de classe non persistées
      when:
        - non-persisted
        - class-property
```

### `global`

Une annotation peut également être marquée avec `global: true`, ce qui aura pour effet de la **poser automatiquement sur tous les objets ciblés par l'annotation**. Cela l'ajoute donc automatiquement dans les dépendances de tous les fichiers, à l'image des domaines. Ces annotations ne peuvent pas avoir de paramètres.

Cette fonctionnalité peut être utilisée pour compléter les annotations posées systématiquement par un générateur, par exemple pour ajouter des fonctionnalités non gérées à moindre frais.

## Exclusion d'annotations

A tous les endroits où il est possible de définir des annotations, il est également possible de définir des **exclusions**. Cela permet de gérer des exceptions, pour ne pas à avoir à définir une même annotation partout, sauf à un seul endroit.

Par exemple :

```yaml
domain:
  name: DO_LIBELLE
  label: Libellé
  annotations:
    - MyAnnotation
---
class:
  name: MyClass
  comment: Ma classe
  properties:
    - name: Libelle
      domain: DO_LIBELLE
      comment: Mon libellé sans annotation.
      excludedAnnotations:
        - MyAnnotation
```

Ici, l'annotation `MyAnnotation` sera posée sur tous les propriétés de `DO_LIBELLE`, sauf sur la propriété `Libelle` de `MyClass`.

Ces exclusions sont utilisables à tous les niveaux et pour toutes les sources d'annotations, en particulier les annotations globales (qui pourraient être exclues au niveau d'un domaine par exemple, et pas seulement sur une propriété), ou pour une annotation sur une propriété qu'on ne veut pas reprendre sur un alias.

## Templating et paramètres

Comme pour les domaines et les décorateurs, il est possible d'utiliser du [templating](/model/templating) dans les annotations, et de définir des paramètres :

```yaml
annotation:
  name: MyAnnotation
  description: Mon annotation
  parameters:
    - name: param1
      required: true
      comment: Premier paramètre.
    - name: param2
      defaultValue: Test
      comment: Deuxième paramètre.
  csharp:
    - text: MyAnnotation("{label}", "{param1}", "{param2}"))
```

Ces paramètres pourront être passés lors de l'instanciation de l'annotation :

```yaml
class:
  name: MyClass
  annotations:
    - MyAnnotation:
        param1: test
        param2: hello
    - OtherAnnotation
```

Tous les paramètres passés doivent être définis au préalable sur l'annotation. Les paramètres obligatoires doivent être renseignés avec l'annotation, et les paramètres non renseignés le seront avec leur `defaultValue` (qui vaut `""` si non renseignée).

Les variables et paramètres sont utilisables dans les propriétés d'implémentations suivantes :

- `text`
- `imports`
