# Décorateurs

Afin d'enrichir de manière plus personalisée le code généré (principalement en Java et C#), il est possible de définir des décorateurs (`decorator`). On peut y déclarer un ensemble d'interfaces implémentées, une classe étendue (hors TopModel), des annotations à ajouter.

Ces décorateurs s'ajoutent ensuite à la définition des classes, sous forme de liste.
Attention aux dépendances, le fichier des décorateurs doivent être importé pour être utilisé.

Exemple :

En Java, nous pouvons ajouter le décorateur EntityListener, pour ajouter l'annotation `EntityListeners` et les imports y afférent :

```yaml
decorator:
  name: EntityListeners
  description: Entity Listeber pour suivre les évènements de création et de modification
  java:
    annotations:
      - EntityListeners(AuditingEntityListener.class)
    imports:
      - org.springframework.data.jpa.domain.support.AuditingEntityListener
      - javax.persistence.EntityListeners
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
