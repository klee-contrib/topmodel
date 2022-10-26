# Décorateurs

Afin d'enrichir de manière plus personnalisée le code généré (en Java et C#), il est possible de définir des décorateurs (`decorator`). On peut y déclarer un ensemble d'interfaces implémentées, une classe étendue (hors TopModel), des annotations à ajouter.

Ces décorateurs s'ajoutent ensuite à la définition des classes, sous forme de liste. Les décorateurs ne sont pas automatiquement disponibles dans tous les fichiers comme les domaines, donc il faudra qu'ils soient soit importés, soit définis dans le même fichier (comme une classe).

## Propriétés

Il est également possible de renseigner des propriétés sur les décorateurs. Il n'y a aucune limitation sur les propriétés que l'on peut définir dans un décorateur (on peut mettre des alias, des compositions...).

Ces propriétés sont ensuite recopiées sur les classes décorées (littéralement recopiées, il n'y a pas d'alias vers la propriété du décorateur par exemple). Ce sont par la suite des propriétés à part entière de la classe, qui peuvent être référencées par la suite sans problème (par exemple dans un alias). Leur "location" est en revanche bien dans le décorateur, donc le hover + la navigation dans l'IDE pointe bien vers la déclaration dans le décorateur.

Elles sont ajoutées en premier dans la liste des propriétés, principalement pour avoir l'erreur de nom de propriété en double (si elle existe) sur la propriété définie dans la classe et non sur le décorateur.

## Exemple

En Java, nous pouvons ajouter le décorateur EntityListener, pour ajouter l'annotation `EntityListeners` et les imports y afférent :

```yaml
decorator:
  name: EntityListeners
  description: Entity Listener pour suivre les évènements de création et de modification
  java:
    annotations:
      - EntityListeners(AuditingEntityListener.class)
    imports:
      - org.springframework.data.jpa.domain.support.AuditingEntityListener
      - javax.persistence.EntityListeners
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

## Templating

## Variables

Il est possible que certaines propriétés des décorateurs dépendent de la classe sur laquelle vous l'ajouter. Vous pourriez par exemple ajouter une interface `java` générique de la classe sur laquelle est ajouté le décorateur.

```java
/**
 * Utilisateur de l'application.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "UTILISATEUR")
@EntityListeners(AuditingEntityListener.class)
public class Utilisateur implements MonInterface<Utilisateur> {
}
```

Ici nous pourrions écrire un décorateur `MonInterfaceUtilisateur` :

```yaml
decorator:
  name: MonInterfaceUtilisateur
  description: Implémente MonInterface pour la classe utilisateur
  java:
    annotations:
      - MonInterface<Utilisateur>
```

Cela pose un problème évident : ce décorateur n'est utilisable que pour cette classe là.
Mais TopModel permet de gérer ce type de cas, et de créer des décorateurs plus génériques avec des `templates`.

TopModel peut remplacer la chaîne de caractère `{name}` par le nom de la classe sur lequel est ajouté le générateur. Ainsi :

```yaml
decorator:
  name: MonInterface
  description: Implémente MonInterface pour la classe sur laquelle ce décorateur est ajouté
  java:
    implements:
      - MonInterface<{name}>
```

Permet de généraliser le comportement du décorateur `MonInterface` à toutes les classes qui voudraient l'utiliser.

Actuellement il est possible d'utiliser ces variables

- `primaryKey.name`
- `trigram`
- `name`
- `comment`
- `label`
- `pluralName`
- `module`

Dans ces propriétés :

- `java.annotations`
- `java.implements`
- `java.extends`
- `java.imports`
- `csharp.extends`
- `csharp.implements`
- `csharp.annotations`
- `csharp.usings`

Les templates des domaines des propriétés sont également valorisés.

## Transformation

Il est possible que la variable que vous utilisez dans votre template ne corresponde pas tout à fait à votre besoin. TopModel gère l'ajout de `transformateurs` sur les templates. Vous pouvez ajouter un `transformateur` après le nom de la variable que vous référencez, précédé de `:`. Le code généré tiendra compte de cette transformation.

Exemple :

```yaml
decorator:
  name: MonInterface
  description: Implémente MonInterface pour la classe sur laquelle ce décorateur est ajouté
  java:
    annotations:
      - @Label(\"{name:lower}\")
```

Actuellement, voici les transformations gérées par `TopModel` :

| nom          | résultat      |
| ------------ | ------------- |
| `kebab`      | kebab-case    |
| `snake_case` | snake_case    |
| `constant`   | CONSTANT_CASE |
| `camel`      | camelCase     |
| `pascal`     | PascalCase    |
| `lower`      | lowercase     |
| `upper`      | UPPERCASE     |
