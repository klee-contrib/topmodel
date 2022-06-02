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

```
