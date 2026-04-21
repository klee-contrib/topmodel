# Génération du métamodèle

| Nom               | Condition d'activation                | Objets ciblés                     | Fichiers générés                                                                                      |
| ----------------- | ------------------------------------- | --------------------------------- | ----------------------------------------------------------------------------------------------------- |
| `JpaMetaModelGen` | `metaModel: true` && `useJdbc: false` | Entités persistées non abstraites | Classes représentant le métamodèle des entités persistées. Une classe par entité avec le suffixe `_`. |

Le métamodèle est une représentation typée et statique des entités, leurs attributs et relations. Il permet notamment de faciliter l'utilisation des Criteria Builder en évitant l'utilisation de chaînes de caractères pour spécifier des entités et leurs propriétés.

Lorsque cette option est activée, une classe de métamodèle est générée pour chaque entité persistée. Ces classes suivent la convention de nommage JPA : `[NomEntité]_` (avec un underscore suffixe).

**Structure des classes générées :**

- Annotation `@StaticMetamodel([NomEntité].class)`
- Annotation `@Generated` (si `generatedHint: true`)
- Attributs statiques `volatile` de type `SingularAttribute`, `ListAttribute`, `SetAttribute`, `CollectionAttribute`, ou `MapAttribute` selon le type de propriété
- Constantes String pour les noms des propriétés (en CONSTANT_CASE)
- Support de l'héritage avec `extends` si l'entité hérite d'une autre

**Exemple d'utilisation :**

```java
// Au lieu d'utiliser des chaînes de caractères
CriteriaBuilder cb = em.getCriteriaBuilder();
CriteriaQuery<Utilisateur> query = cb.createQuery(Utilisateur.class);
Root<Utilisateur> root = query.from(Utilisateur.class);
query.where(cb.equal(root.get("nom"), "Dupont")); // Risque d'erreur de typo

// Avec le métamodèle (type-safe)
query.where(cb.equal(root.get(Utilisateur_.nom), "Dupont")); // Vérifié à la compilation
```

**Documentation :**

- Spec JPA (Voir le chapitre 5) : <https://download.oracle.com/otndocs/jcp/persistence-2.0-fr-eval-oth-JSpec/>
- Exemple d'utilisation : <https://www.baeldung.com/hibernate-criteria-queries-metamodel>

## Configuration

### `metaModel`

Option pour générer le métamodèle JPA.

_Valeur par défaut_: `false`
