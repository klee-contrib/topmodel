# Génération des classes

Plusieurs générateurs du module `TopModel.Generator.Jpa` sont dédiés à la génération des classes Java à partir du modèle :

| Nom                    | Condition d'activation                         | Objets ciblés                                                          | Fichiers générés                                                                                                                          |
| ---------------------- | ---------------------------------------------- | ---------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------- |
| `JavaDtoGen`           | Toujours                                       | Classes non abstraites, non persistées, qui ne sont pas des enums      | POJO contenant les propriétés définies dans le modèle avec les annotations de validation. La classe implémente `Serializable`.            |
| `JdbcEntityGen`        | `useJdbc: true`                                | Classes non abstraites, persistées, qui ne sont pas en `enum: true`    | POJO annoté avec les annotations `org.springframework.data.*` (persistance Spring Data JDBC, sans associations JPA).                      |
| `JpaEntityGen`         | `useJdbc: false`                               | Classes non abstraites, persistées, non `readonly`                     | POJO annoté avec les annotations JPA (Jakarta Persistence).                                                                               |
| `JpaEnumEntityGen`     | `useJdbc: false`                               | Classes non abstraites, persistées, avec `enum: class` et `readonly`   | POJO persisté (JPA) contenant, en plus, des membres statiques `@Transient` représentant les entités décrites dans les `values`.           |
| `JavaEnumClassPropGen` | `uniqueValueGeneration` autorise les enums     | Classes non abstraites avec `enum: class`                              | Enum Java nommée `[NomDeLaClasse][NomDeLaPropriété]`, listant les valeurs possibles de la clé primaire de la classe.                      |
| `JavaEnumDtoGen`       | Toujours                                       | Classes non abstraites, non persistées, avec `enum: class`             | POJO non persisté avec des membres statiques représentant les instances décrites dans les `values`.                                       |
| `JavaEnumEnumGen`      | Toujours                                       | Classes non abstraites avec `enum: true`                               | Enum Java contenant toutes les valeurs définies dans les `values`, dont la clé est la `primaryKey` ou la première propriété de la classe. |
| `JpaInterfaceGen`      | Toujours                                       | Classes avec `abstract: true`                                          | Interface ne contenant que des `getters` (et `setters` si la propriété n'est pas `readonly`) des propriétés définies dans le modèle.      |

Le choix entre les deux modes d'enum (`enum: class` ou `enum: true`) se fait **classe par classe**, directement sur la classe dans le modèle.

## Structure d'une classe générée

Quel que soit le générateur utilisé, une classe Java générée contient :

- les **propriétés** en `private`, typées par leur `domain`, commentées avec le commentaire du modèle ;
- un **getter** et un **setter** publics par propriété (sauf cas particuliers : classes `readonly`, annotations Lombok, clés primaires sur certains enums persistés) ;
- éventuellement une sous-classe `Fields` (voir [FieldsEnum](#fieldsenum)) ou une classe interne pour les clés primaires composites.

L'annotation `@Generated("TopModel : https://github.com/klee-contrib/topmodel")` est ajoutée sur chaque classe si `generatedHint: true` (valeur par défaut).

## Support des annotations Lombok

Le générateur détecte automatiquement la présence d'annotations Lombok sur les classes ou les propriétés :

- Si une classe possède l'annotation `@Data`, `@Getter` ou `@Setter`, les getters et setters ne sont pas générés pour cette classe.
- Si une propriété possède l'annotation `@Getter` ou `@Setter`, le getter ou setter correspondant n'est pas généré pour cette propriété.

Cela permet d'utiliser Lombok pour réduire le boilerplate tout en conservant la génération des autres éléments (constructeurs, annotations JPA, etc.).

## Classes persistées

Les classes persistées sont générées avec les annotations correspondant à ce qui est paramétré dans le modèle.

### Sur la classe

| Annotation                                            | Paramètre correspondant dans le modèle                                                                           |
| ----------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------- |
| `@Entity`                                             | Automatique                                                                                                      |
| `@Table(name = "SQL_NAME")`                           | Automatique                                                                                                      |
| `@Table(uniqueConstraints = { ... })`                 | Pour chaque index marqué `unique` dans la classe                                                                 |
| `@Table(indexes = { @Index(...) })`                   | Pour chaque index non unique de la classe                                                                        |
| `@Inheritance(strategy = InheritanceType.JOINED)`     | Si au moins une autre classe du modèle étend cette classe                                                        |
| `@IdClass(XxxId.class)`                               | Si la classe a une clé primaire composite (plus d'une propriété `primaryKey: true`)                              |
| `@Immutable`                                          | Si `reference: true` **et** la classe est `readonly` (typiquement une enum persistée de type `class`)            |
| `@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)`  | Si `reference: true` **et** la classe est `readonly`                                                             |
| `@Cache(usage = CacheConcurrencyStrategy.READ_WRITE)` | Si `reference: true` **et** la classe n'est pas `readonly`                                                       |

### Sur chacune des propriétés

| Annotation                               | Paramètre correspondant dans le modèle                                                                                                                     |
| ---------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `@Id`                                    | Sur une propriété `primaryKey: true`                                                                                                                       |
| `@MapsId`                                | Sur l'association correspondant à la clé primaire quand celle-ci est une association à PK simple                                                           |
| `@GeneratedValue` + `@SequenceGenerator` | Sur la PK auto-générée si `identity.mode: sequence`                                                                                                        |
| `@GeneratedValue(strategy = IDENTITY)`   | Sur la PK auto-générée si `identity.mode: identity` (valeur par défaut)                                                                                    |
| `@GeneratedValue(strategy = UUID)`       | Sur la PK auto-générée si `identity.mode: uuid`                                                                                                            |
| `@Enumerated(EnumType.STRING)`           | Sur toute propriété persistée dont le type est une enum Java générée par TopModel                                                                          |
| `@Column`                                | Sur les propriétés qui ne sont ni des compositions, ni des associations (nom, nullable, length/precision/scale, columnDefinition sont déduits du `domain`) |
| `@OneToOne`                              | Sur une association portant une contrainte d'unicité                                                                                                       |
| `@ManyToOne`                             | Cas général de l'association                                                                                                                               |
| `@OneToMany`                             | Propriété réciproque d'une association `ManyToOne`                                                                                                         |
| `@JoinColumn`                            | Sur les associations `@ManyToOne` et `@OneToOne`                                                                                                           |
| `@OrderBy`                               | Sur les `@OneToMany` dont la classe cible définit une `orderProperty`                                                                                      |
| `@Convert`                               | Sur les compositions (le converter utilisé est paramétrable via `compositionConverterCanonicalName`)                                                       |

Les associations sont toutes générées avec `fetch = FetchType.LAZY` pour optimiser les performances. Les `cascade` par défaut sont configurés via l'option [`cascadeTypes`](#cascadetypes).

### ManyToMany

Il n'est pas possible de générer d'association `ManyToMany` implicite. Pour créer ce type d'association, la classe de liaison doit être explicitée et contenir les associations vers les deux classes liées. Ces deux propriétés doivent avoir `primaryKey: true`. La classe avec clé primaire composite sera générée avec l'annotation `@IdClass(XxxId.class)`, ainsi qu'une classe interne statique `XxxId` contenant les propriétés de la clé, ses `equals`/`hashCode` et les associations JPA correspondantes.

### OneToMany / ManyToOne

L'association `OneToMany` réciproque est générée dans la classe de destination. **L'association "propriétaire" de la relation est toujours l'association `ManyToOne`** (elle porte le `@JoinColumn`, la `@OneToMany` utilise `mappedBy`).

### Exemple : entité simple

```java
@Entity
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(
    name = "RESERVATION",
    uniqueConstraints = {
        @UniqueConstraint(columnNames = {"TAB_ID", "REV_DATE_RESERVATION"})
    }
)
public class Reservation {

    /** Identifiant de la réservation. */
    @Id
    @Column(name = "REV_ID", nullable = false, columnDefinition = "int")
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "SEQ_RESERVATION")
    @SequenceGenerator(sequenceName = "SEQ_RESERVATION", name = "SEQ_RESERVATION", initialValue = 1000, allocationSize = 50)
    private Integer id;

    /** Client ayant fait la réservation. */
    @JoinColumn(name = "PER_ID", referencedColumnName = "PER_ID")
    @ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = Client.class)
    private Client client;

    // ... getters / setters
}
```

### Enum

Sur une classe, lorsque sont remplis les critères suivants :

- La classe a au moins une propriété
- La classe a des `values`
- La classe n'a pas `enum: false`
- La clé primaire remplit les critères suivants :
  - Le `domain` a `autoGenerated: false`
  - Le type `java` est `string`
  - Toutes les valeurs de cette propriété peuvent être des enums Java

Alors la classe est peut être générée en `enum`.

Il existe deux modes de gestion des enums, en fonction de ce qui est défini sur la classe.

#### Mode `enum: class`

- Une enum Java `[NomDeLaClasse][NomDeLaPropriété]` est créée en utilisant `enumsPath`, dont les valeurs sont les valeurs possibles définies dans le modèle.
- La classe générée est identique à ce qu'elle aurait été si elle n'était pas une enum, sauf :
  - Le type de la propriété d'enum est l'enum `[NomDeLaClasse][NomDeLaPropriété]`
  - Si la classe est persistée, l'annotation `@Enumerated(EnumType.STRING)` est ajoutée
  - Des membres statiques `@Transient` sont ajoutés à la classe, représentant les différentes `values`
  - Un constructeur prenant en entrée une instance de l'enum `[NomDeLaClasse][NomDeLaPropriété]` est ajouté ; son corps initialise le reste des propriétés à partir d'un `switch` sur la valeur

Exemple :

```java
/** Catégorie de plat. */
@Entity
@Immutable
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Table(
    name = "CATEGORIE_PLAT",
    uniqueConstraints = {
        @UniqueConstraint(columnNames = {"CAT_ORDRE"})
    }
)
public class CategoriePlat {

    @Transient
    public static final CategoriePlat BOISSON = new CategoriePlat(CategoriePlatCode.BOISSON);
    @Transient
    public static final CategoriePlat DESSERT = new CategoriePlat(CategoriePlatCode.DESSERT);
    // ...

    /** Code de la catégorie. */
    @Id
    @Enumerated(EnumType.STRING)
    @Column(name = "CAT_CODE", nullable = false, length = 10, columnDefinition = "varchar")
    private CategoriePlatCode code;

    /** Libellé de la catégorie. */
    @Column(name = "CAT_LIBELLE", nullable = false, length = 100, columnDefinition = "varchar")
    private String libelle;

    /** No arg constructor. */
    public CategoriePlat() {
        // No arg constructor
    }

    /**
     * Enum constructor.
     * @param code Code dont on veut obtenir l'instance.
     */
    public CategoriePlat(CategoriePlatCode code) {
        this.code = code;
        switch (code) {
            case BOISSON:
                this.libelle = "restaurant.categoriePlat.values.Boisson";
                break;
            case DESSERT:
                this.libelle = "restaurant.categoriePlat.values.Dessert";
                break;
            // ...
        }
    }

    // ... getters (pas de setters pour les classes readonly)
}
```

```java
/** Enumération des valeurs possibles de la propriété Code de la classe CategoriePlat. */
public enum CategoriePlatCode {
    /** Boisson. */
    BOISSON,
    /** Dessert. */
    DESSERT,
    /** Entrée. */
    ENTREE,
    /** Plat principal. */
    PLAT
}
```

#### Mode `enum: true`

Si la classe a l'attribut `enum: true`, le traitement des enums diffère en ces points :

- Une enum Java `[NomDeLaClasse]` est créée, dont les valeurs sont celles définies dans le modèle, et contenant toutes les valeurs des autres propriétés en paramètres. Cette enum est générée dans `enumsPath`.
- Cette enum n'est **pas** une entité persistée.
- Les associations vers cette classe ne sont plus des associations JPA, mais des colonnes classiques : elles portent `@Column` et (puisqu'elles sont de type enum) `@Enumerated(EnumType.STRING)`.
- Les noms de propriétés restent identiques au mode par défaut, ce qui permet de garder la compatibilité avec les autres générateurs.

Exemple :

```java
/** Enumération des valeurs possibles de la classe StatutCommande. */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public enum StatutCommande {
    /** En attente. */
    EN_ATT("restaurant.statutCommande.values.EnAttente"),
    /** En préparation. */
    EN_PREP("restaurant.statutCommande.values.EnPreparation"),
    // ...
    ;

    /** Libelle. */
    private final String libelle;

    StatutCommande(final String libelle) {
        this.libelle = libelle;
    }

    public String getLibelle() {
        return this.libelle;
    }
}
```

## Mode JDBC (`useJdbc: true`)

Lorsque `useJdbc: true`, le générateur `JdbcEntityGen` remplace `JpaEntityGen` et `JpaEnumEntityGen`. Les classes utilisent alors les annotations **Spring Data Relational** (`org.springframework.data.annotation.Id`, `org.springframework.data.relational.core.mapping.Table`, `org.springframework.data.relational.core.mapping.Column`) plutôt que les annotations JPA. Les associations JPA (`@ManyToOne`, `@OneToMany`, `@JoinColumn`...) ne sont pas générées : seules les colonnes de clés étrangères sont présentes.

```java
@Table(name = "restaurant")
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public class Restaurant {

    /** Identifiant du restaurant. */
    @Id
    @Column("res_id")
    private Integer id;

    /** Nom du restaurant. */
    @NotNull
    @Column("res_nom")
    private String nom;

    // ... getters / setters
}
```

## Classes non persistées

Les classes non persistées sont générées de la même manière que les classes persistées, mais ne reçoivent pas les annotations JPA.

Elles implémentent toutes `java.io.Serializable` et se voient ajouter le champ suivant (annoté `@Serial`) :

```java
/** Serial ID. */
@Serial
private static final long serialVersionUID = 1L;
```

Les propriétés `required: true` reçoivent l'annotation `jakarta.validation.constraints.NotNull`. Par ailleurs, si le `domain` a :

- Un type Java parmi `String`, `CharSequence`, `Set`, `Map`, `List`, `Collection` (ou un tableau `[]`)
- Un `length` défini

Alors la propriété portera l'annotation `@Size(max = [length])`.

Si le `domain` a :

- Un type Java numérique (`BigDecimal`, `BigInteger`, `byte`, `short`, `int`, `long`, `Byte`, `Short`, `Integer`, `Long`, `double`, `Double`)
- `length` **et** `scale` définis

Alors la propriété portera l'annotation `@Digits(integer = [length], fraction = [scale])`.

Les compositions vers d'autres classes non persistées reçoivent en outre l'annotation `@Valid` pour permettre la validation en cascade.

Lorsque la propriété est issue d'un mapper (`from`/`to`) ou d'un alias, le commentaire Javadoc inclut automatiquement un lien de type `Alias of {@link ...}` vers la propriété d'origine :

```java
/**
 * Nom du restaurant.
 * Alias of {@link restaurant.jpa_sequence_server.entities.restaurant.Restaurant#getNom() Restaurant#getNom()}
 */
@NotNull
@Size(max = 100)
private String nom;
```

**Précautions d'emploi :**

- Ne pas composer avec une entité persistée.

## Classes abstraites

Pour générer des interfaces à partir d'une classe du modèle, vous pouvez passer la propriété `abstract` d'une classe à `true`.
Ainsi, le fichier généré sera non plus une classe mais une interface contenant un `getter` (et un `setter` si la propriété n'est pas `readonly`) pour chacune des propriétés.

Le cas d'usage typique est celui des [projections de Spring JPA](https://docs.spring.io/spring-data/jpa/docs/current/reference/html/#projections).

```yaml
---
class:
  name: IUtilisateur
  comment: Interface de projection
  abstract: true
```

## FieldsEnum

Il est possible de générer dans la définition de la classe, la sous-classe (qui est une enum) `Fields`. Il s'agit d'une énumération des champs de la classe, au format `CONSTANT_CASE`, à laquelle est associé le type Java du champ.
Il faut pour cela compléter la propriété `fieldsEnum` à la configuration JPA, qui est une liste des types de classes pour lesquels on veut générer cette enum : les classes persistées (avec `"persisted"`) et/ou non persistées (avec `"non-persisted"`).

Il est également possible d'ajouter la référence d'une interface à cette configuration. Cette interface sera implémentée par la classe `Fields`. Vous pourrez ainsi la manipuler plus facilement. Si l'interface en question est suffixée par `<>`, alors elle sera considérée comme générique de la classe persistée.

Exemple :

La configuration suivante :

```yaml
fieldsEnum: ["persisted"]
fieldsEnumInterface: topmodel.exemple.utils.IFieldEnum<>
```

Génèrera, dans la classe `Departement`, l'enum suivante :

```java
public enum Fields implements IFieldEnum<Departement> {
    CODE(String.class),
    LIBELLE(String.class),
    REGION_CODE(RegionCode.class);

    private final Class<?> type;

    Fields(Class<?> type) {
        this.type = type;
    }

    public Class<?> getType() {
        return this.type;
    }
}
```

## Configuration

### `entitiesPath`

Localisation des classes persistées du modèle, relative au répertoire de génération.

Le chemin des fichiers cibles sera calculé en remplaçant les `.` et le `:` par des `/` dans cette valeur, tandis que le nom du package des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

_Templating_: `{app}`, `{module}`

_Valeur par défaut_: `"javagen:{app:path}/entities/{module:path}"`

_Variables par tag_: **oui** (plusieurs définitions de classes pourraient être générées si un fichier a plusieurs tags)

### `dtosPath`

Localisation des classes non persistées du modèle, relative au répertoire de génération.

_Templating_: `{app}`, `{module}`

_Valeur par défaut_: `"javagen:{app:path}/dtos/{module:path}"`

_Variables par tag_: **oui** (plusieurs définitions de classes pourraient être générées si un fichier a plusieurs tags)

### `enumsPath`

Localisation des classes d'enums, relative au répertoire de génération. Utilisée aussi bien en mode `enum: class` qu'en mode `enum: true`.

_Templating_: `{app}`, `{module}`

_Valeur par défaut_: `"javagen:{app:path}/enums/{module:path}"`

_Variables par tag_: **oui** (plusieurs définitions de classes pourraient être générées si un fichier a plusieurs tags)

### `compositionConverterCanonicalName`

Nom complet de la classe permettant de convertir les compositions stockées en JSON dans la base de données. Les compositions sont des propriétés de type classe non persistée qui sont sérialisées en JSON dans une colonne de la base de données.

_Templating_:

- `{package}` : remplacé par le package de la classe composée
- `{class}` : remplacé par le nom de la classe composée

_Valeur par défaut_: `"{package}.{class}Converter"`

_Variables par tag_: **non**

**Exemple :**

Pour une classe `Adresse` dans le package `topmodel.exemple.entities.common`, le converter généré sera `topmodel.exemple.entities.common.AdresseConverter` par défaut. Vous pouvez personnaliser ce nom :

```yaml
jpa:
  - tags:
      - entity
    compositionConverterCanonicalName: "{package}.converters.{class}JsonConverter"
```

### `fieldsEnum`

Option pour générer une enum des champs de certaines classes. Il s'agit d'une liste dont les 2 valeurs possibles sont :

- `persisted` : ajoute l'enum des champs sur les classes persistées
- `non-persisted` : ajoute l'enum des champs sur les classes non persistées

### `fieldsEnumInterface`

Précise l'interface des fields enum générés.

_Templating_: `<>` (remplace par `<NomDeLaClasse>`)

### `associationAdders`

Option pour générer des méthodes d'ajout pour les associations `@OneToMany`. Ces méthodes permettent de synchroniser les objets ajoutés en mettant à jour la relation réciproque.

_Valeur par défaut_: `false`

**Exemple :**

Pour une association `@OneToMany` entre `Utilisateur` et `Commande`, si `associationAdders: true`, une méthode `addCommande(Commande commande)` sera générée dans la classe `Utilisateur`. Cette méthode ajoutera la commande à la liste et mettra à jour la référence réciproque (`commande.setUtilisateur(this)`).

### `associationRemovers`

Option pour générer des méthodes de suppression pour les associations `@OneToMany`. Ces méthodes permettent de synchroniser les objets supprimés en mettant à jour la relation réciproque.

_Valeur par défaut_: `false`

**Exemple :**

Pour une association `@OneToMany` entre `Utilisateur` et `Commande`, si `associationRemovers: true`, une méthode `removeCommande(Commande commande)` sera générée dans la classe `Utilisateur`. Cette méthode retirera la commande de la liste et mettra à jour la référence réciproque (`commande.setUtilisateur(null)`).

### `cascadeTypes`

Types de cascade à ajouter sur les associations JPA générées, par type d'association.

Les clés configurables sont :

- `oneToOne` : cascade(s) à ajouter sur les associations `@OneToOne`
- `oneToMany` : cascade(s) à ajouter sur les associations `@OneToMany`
- `manyToOne` : cascade(s) à ajouter sur les associations `@ManyToOne`

Les valeurs possibles pour chaque clé sont : `all`, `persist`, `merge`, `remove`, `refresh`, `detach`, `lock`.

_Valeur par défaut_ :

```yaml
cascadeTypes:
  oneToOne: [all]
  oneToMany: [all]
```

Aucune cascade n'est ajoutée par défaut sur les associations `@ManyToOne`.

**Format généré :**

- Avec une seule valeur, l'attribut `cascade` est généré sans accolades : `cascade = CascadeType.ALL`.
- Avec plusieurs valeurs, il est généré avec des accolades : `cascade = { CascadeType.ALL, CascadeType.DETACH }`.

**Exemple de configuration :**

```yaml
jpa:
  - tags:
      - entity
    cascadeTypes:
      oneToMany:
        - all
      manyToOne:
        - persist
        - merge
      oneToOne:
        - all
        - detach
```

**Code Java généré :**

Avec la configuration ci-dessus, une association `@OneToMany` produira :

```java
@OneToMany(cascade = CascadeType.ALL, fetch = FetchType.LAZY, mappedBy = "commande")
private List<LigneCommande> lignes;
```

Et une association `@OneToOne` produira :

```java
@JoinColumn(name = "AVI_ID", referencedColumnName = "AVI_ID", unique = true)
@OneToOne(cascade = { CascadeType.ALL, CascadeType.DETACH }, fetch = FetchType.LAZY, optional = true)
private AvisClient avisClient;
```
