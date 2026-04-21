# Génération du métamodèle

| Nom               | Condition d'activation                | Objets ciblés                     | Fichiers générés                                                                                      |
| ----------------- | ------------------------------------- | --------------------------------- | ----------------------------------------------------------------------------------------------------- |
| `JpaMetaModelGen` | `metaModel: true` && `useJdbc: false` | Entités persistées non abstraites | Classes représentant le métamodèle des entités persistées. Une classe par entité avec le suffixe `_`. |

Le métamodèle JPA est une représentation typée et statique des entités, de leurs propriétés et de leurs relations. Il permet d'utiliser le `CriteriaBuilder` sans manipuler de chaînes de caractères pour désigner une entité ou l'une de ses propriétés, et offre ainsi une vérification à la compilation.

## Classes ciblées

Une classe de métamodèle est générée pour chaque classe du modèle qui est à la fois :

- **persistée** ;
- **non abstraite** ;
- **non marquée comme enum** (ni en mode `enum`, ni en mode `class`).

Les classes abstraites ou les listes de référence de type enum sont donc ignorées.

## Fichier généré

Le fichier est produit à côté de l'entité, dans le même répertoire que `entitiesPath`. Son nom reprend celui de l'entité avec un underscore final, selon la convention JPA :

- `Commande.java` → `Commande_.java`
- `Client.java` → `Client_.java`

Si l'entité hérite d'une autre entité (`extends`), la classe de métamodèle hérite elle aussi de la classe de métamodèle parente. Par exemple, si `Client` étend `Personne`, alors `Client_` étend `Personne_`.

## Contenu généré

Pour chaque propriété de l'entité, deux éléments sont générés :

- un **attribut statique typé**, de la forme `public static volatile XxxAttribute<Entité, Type>`, dont le type dépend de la nature de la propriété :

  | Type de la propriété dans le modèle | Attribut généré       |
  | ----------------------------------- | --------------------- |
  | `List<T>`                           | `ListAttribute`       |
  | `Set<T>`                            | `SetAttribute`        |
  | `Collection<T>`                     | `CollectionAttribute` |
  | `Map<K, V>`                         | `MapAttribute`        |
  | tout autre type                     | `SingularAttribute`   |

- une **constante** `public static final String` en CONSTANT_CASE, dont la valeur est le nom camelCase de la propriété. Elle est pratique pour les `@EntityGraph`, `@NamedEntityGraph`, ou pour toute API qui attend un nom de propriété sous forme de chaîne.

La classe elle-même porte l'annotation `@StaticMetamodel(Entité.class)`, ainsi que l'annotation `@Generated` si `generatedHint: true` (valeur par défaut).

## Exemple

Pour une entité `Commande` ayant des propriétés simples et une collection `lignes` de type `List<LigneCommande>`, la classe générée `Commande_` ressemble à :

```java
@StaticMetamodel(Commande.class)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Commande_ {

    public static volatile SingularAttribute<Commande, Integer> id;
    public static volatile SingularAttribute<Commande, LocalDateTime> dateCommande;
    public static volatile SingularAttribute<Commande, Client> client;
    public static volatile ListAttribute<Commande, LigneCommande> lignes;

    public static final String ID = "id";
    public static final String DATE_COMMANDE = "dateCommande";
    public static final String CLIENT = "client";
    public static final String LIGNES = "lignes";
}
```

## Exemple d'utilisation

```java
// Sans métamodèle : risque de typo, non vérifié à la compilation
query.where(cb.equal(root.get("dateCommande"), date));

// Avec métamodèle : type-safe, refactoring sûr
query.where(cb.equal(root.get(Commande_.DATE_COMMANDE), date));
```

## Documentation

- Spec JPA (chapitre 5) : <https://download.oracle.com/otndocs/jcp/persistence-2.0-fr-eval-oth-JSpec/>
- Exemple d'utilisation : <https://www.baeldung.com/hibernate-criteria-queries-metamodel>

## Configuration

### `metaModel`

Option pour générer le métamodèle JPA.

_Valeur par défaut_: `false`
