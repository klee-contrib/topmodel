# Décorateurs

Afin d'enrichir de manière plus personnalisée le code généré (en Java et C#), il est possible de définir des décorateurs (`decorator`). On peut y déclarer un ensemble d'interfaces implémentées, une classe étendue (hors TopModel), des annotations à ajouter.

Ces décorateurs s'ajoutent ensuite à la définition d'une classe, d'un endpoint ou d'un autre décorateur, sous forme de liste. Les décorateurs ne sont pas automatiquement disponibles dans tous les fichiers comme les domaines, donc il faudra qu'ils soient soit importés, soit définis dans le même fichier (comme une classe).

## Propriétés

Il est également possible de renseigner des propriétés sur les décorateurs. Il n'y a aucune limitation sur les propriétés que l'on peut définir dans un décorateur (on peut mettre des alias, des compositions...).

Ces propriétés sont ensuite recopiées sur les classes, les endpoints (en tant que paramètres) ou les autres décorateurs décorés (littéralement recopiées, il n'y a pas d'alias vers la propriété du décorateur par exemple). Ce sont par la suite des propriétés à part entière de la classe, de l'endpoint, ou du décorateur, qui peuvent être référencées par la suite sans problème (par exemple dans un alias). Leur "location" est en revanche bien dans le décorateur, donc le hover + la navigation dans l'IDE pointe bien vers la déclaration dans le décorateur.

Elles sont ajoutées en dernier dans la liste des propriétés de la classe ou du décorateur, et en dernier dans les paramètres de l'endpoint (attention du coup à l'erreur de doublon de nom de propriété qui s'affichera sur la propriété du décorateur...)

Lorsqu'un décorateur avec d'autres décorateurs sera appliqué sur une classe ou un endpoint, l'ensemble des propriétés, des interfaces et des imports de toute la "hiérarchie" de décorateur sera bien pris en compte.

## Exemple pour une classe

En Java, nous pouvons ajouter le décorateur EntityListener, pour ajouter l'[annotation](/model/annotations.md) `EntityListeners` et les propriétés liées :

```yaml
decorator:
  name: EntityListeners
  description: Entity Listener pour suivre les évènements de création et de modification
  annotations:
    - EntityListeners
  properties:
    - name: dateCreation
      comment: Date de création de l'utilisateur
      domain: DO_DATE_CREATION
    - name: dateModification
      comment: Date de modification de l'utilisateur
      domain: DO_DATE_MODIFICATION
```

Son utilisation dans la classe `Utilisateur`

```yaml
---
class:
  name: Utilisateur
  label: Utilisateur
  comment: Utilisateur de l'application
  trigram: UTI
  decorators:
    - EntityListeners
```

Le code généré aura les annotations et les champs issus des décorateurs :

```java
/**
 * Utilisateur de l'application.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "UTILISATEUR")
@EntityListeners(AuditingEntityListener.class)
public class Utilisateur {

  /**
   * Date de création de l'utilisateur.
   */
  @Column(name = "UTI_DATE_CREATION", nullable = true)
  @CreatedDate
  private DateTime dateCreation;

  /**
   * Date de modification de l'utilisateur.
   */
  @Column(name = "UTI_DATE_MODIFICATION", nullable = true)
  @LastModifiedDate
  private DateTime dateModification;
}
```

## Exemple pour un décorateur

En C#, on pourrait définir un décorateur et son utilisation dans un endpoint comme ceci par exemple :

```yaml
---
decorator:
  name: Authorize
  description: Authorize
  parameters:
    - name: policy
      required: true
      comment: Policy d'autorisation.
  annotations:
    - Authorize:
        policy: "{policy}"
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
        policy: interne # Les paramètres sont décrits dans la section suivante.
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
public async Task Get(bool? additionalParam = null)
```

_Remarque : les décorateurs d'endpoints ne fonctionnent que pour la génération serveur des endpoints (C# ou Java)._

_Remarque 2 : `extends`, `implements` et `generateInterface` ne sont évidemment pas utilisés lorsqu'un endpoint est décoré_

## Templating et paramètres

Comme pour les domaines et les annotations, il est possible d'utiliser du [templating](/model/templating.md) dans les annotations, et de définir des paramètres :

```yaml
decorator:
  name: MyDecorator
  parameters:
    - name: param1
      required: true
      comment: Premier paramètre.
    - name: param2
      defaultValue: Test
      comment: Deuxième paramètre.
  csharp:
    annotations:
      - text: MyAnnotation("{param1}", "{param2}"))
```

Ces paramètres pourront être passés lors de l'instanciation du décorateur :

```yaml
class:
  name: MyClass
  decorators:
    - MyDecorator:
        param1: test
        param2: hello
    - OtherDecorator
```

Tous les paramètres passés doivent être définis au préalable sur le décorateur. Les paramètres obligatoires doivent être renseignés avec le décorateur, et les paramètres non renseignés le seront avec leur `defaultValue` (qui vaut `""` si non renseignée).

Les variables et paramètres sont utilisables :

- Dans les paramètres d'annotations
- Dans les propriétés d'implémentations suivantes :
  - `implements`
  - `extends`
  - `imports`
