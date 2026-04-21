# Génération des classes

Plusieurs générateurs du module `TopModel.Generator.Jpa` sont dédiés à la génération des classes Java à partir du modèle :

| Nom                | Condition d'activation                    | Objets ciblés                                    | Fichiers générés                                                                                                                                                                                                     |
| ------------------ | ----------------------------------------- | ------------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `JavaDtoGen`       | Toujours                                  | Classes non persistées qui ne sont pas des enums | Pojo contenant les propriétés définies dans le modèle avec les annotations de validation                                                                                                                             |
| `JdbcEntityGen`    | `useJdbc: true`                           | Classes persistées                               | Pojo contenant les propriétés définies dans le modèle, annotées avec les annotations de la persistance Jdbc                                                                                                          |
| `JpaEntityGen`     | `useJdbc: false`                          | Classes persistées qui ne sont pas des enums     | Pojo contenant les propriétés définies dans le modèle, annotées avec les annotations de la persistance JPA                                                                                                           |
| `JpaEnumEntityGen` | `useJdbc: false`                          | Classes persistées qui sont des enums            | Pojo contenant les propriétés définies dans le modèle, annotées avec les annotations de la persistance JPA. Contient également des membres statiques représentant les entités décrites dans les values              |
| `JpaEnumGen`       | `useJdbc: false` && `enumsAsEnums: false` | Classes persistées ou non qui sont des enums     | Enumération des valeurs possibles de la clé primaire de la classe                                                                                                                                                    |
| `JavaEnumDtoGen`   | `useJdbc: false` && `enumsAsEnums: false` | Classes non persistées qui sont des enums        | Pojo contenant les propriétés définies dans le modèle, annotées avec les annotations de validation. Contient également des membres statiques représentant les instances décrites dans les values                    |
| `JpaEnumValuesGen` | `useJdbc: false` && `enumsAsEnums: true`  | Classes qui sont des enums                       | Enum contenant toutes les valeurs définies dans les values, dont la clé est la primaryKey ou la première propriété de la classe                                                                                      |
| `JpaInterfaceGen`  | Toujours                                  | Classes qui ont `abstract: true`                 | Interface ne contenant que des `getters` des propriétés définies dans le modèle. Peut également définir une méthode `hydrate`, s'apparentant à un constructeur                                                       |

Le générateur de classes distingue cinq cas :

- Les classes persistées qui ne sont pas des enums
- Les classes non persistées qui ne sont pas des enums
- Les classes persistées qui sont des enums
- Les classes non persistées qui sont des enums
- Les classes abstraites

## Support des annotations Lombok

Le générateur détecte automatiquement la présence d'annotations Lombok sur les classes ou les propriétés :

- Si une classe possède l'annotation `@Data`, `@Getter` ou `@Setter`, les getters et setters ne sont pas générés pour cette classe
- Si une propriété possède l'annotation `@Getter` ou `@Setter`, le getter ou setter correspondant n'est pas généré pour cette propriété

Cette fonctionnalité permet d'utiliser Lombok pour réduire le code boilerplate tout en conservant la génération des autres éléments (constructeurs, annotations JPA, etc.).

Les propriétés générées sont `private`, du type défini dans le `domain`. Le commentaire leur étant associé correspond au commentaire défini dans le modèle.

Des `getter` et `setter` sont ajoutés automatiquement, sauf si la classe ou la propriété possède une annotation Lombok (`@Data`, `@Getter`, ou `@Setter`). Dans ce cas, les getters et setters ne sont pas générés, car ils sont fournis par Lombok. Seul un constructeur vide est ajouté dans la classe générée.

## Classes persistées

Les classes persistées sont générées avec les annotations correspondant à ce qui est paramétré dans le modèle.
Sur la classe :

| Annotation                                            | Paramètre correspondant dans le modèle                                     |
| ----------------------------------------------------- | -------------------------------------------------------------------------- |
| `@Entity`                                             | Automatique                                                                |
| `@Table("SQL_NAME")`                                  | Automatique                                                                |
| `@UniqueConstraint`                                   | `unique` : pour chacune des contraintes d'unicité de la classe             |
| `@Immutable`                                          | si la classe a `reference: true` et que c'est une enum (clé primaire enum) |
| `@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)`  | si la classe a `reference: true` et que c'est une enum (clé primaire enum) |
| `@Cache(usage = CacheConcurrencyStrategy.READ_WRITE)` | si la classe a `reference: true` et que ce n'est pas une enum              |

Sur chacune des propriétés :

| Annotation                     | Paramètre correspondant dans le modèle                                                                |
| ------------------------------ | ----------------------------------------------------------------------------------------------------- |
| `@Id`                          | `primaryKey: true` : sur la clé primaire                                                              |
| `@Enumerated(EnumType.STRING)` | Sur la clé primaire, si TopModel a détecté qu'il s'agissait bien d'une enum                           |
| `@SequenceGenerator`           | `primaryKey: true` : sur la clé primaire si `identity: mode: sequence` dans la configuration générale |
| `@GeneratedValue`              | `primaryKey: true` : sur la clé primaire si `identity: mode: sequence` dans la configuration générale |
| `@Column`                      | Sur les propriétés qui ne sont ni des compositions, ni des associations.                              |
| `@OneToOne`                    | association contenant une clé d'unicité                                                               |
| `@ManyToOne`                   | Cas général de l'association                                                                          |
| `@OneToMany`                   | Propriété réciproque d'une association `ManyToOne`                                                    |
| `@JoinColumn`                  | Sur les associations `manyToOne` et `oneToOne`                                                        |
| `@OrderBy`                     | Sur les associations `oneToMany` pour lesquelles la classe cible définit une `orderProperty`          |
| `@Convert`                     | Sur les compositions. Le converter utilisé est paramétrable.                                          |

Les paramétrages de ces annotations correspondent à ce qui est défini dans le modèle ou dans la configuration, avec :

- `fetch = FetchType.LAZY` pour tous les types d'associations, pour optimisation des performances
- `cascade = { CascadeType.ALL }` pour les associations `OneToMany` par défaut
- `cascade = { CascadeType.ALL }` pour les associations `OneToOne` par défaut

### ManyToMany

Il n'est pas possible de générer d'association ManyToMany implicite. Pour créer ce type d'association, la classe de liaison doit être explicitée, et doit contenir les associations vers les deux classes de la liaison. Ces deux propriétés doivent avoir `primaryKey: true`. La classe avec clé primaire composite sera générée (avec l'annotation `IdClass`).

### OneToMany

L'association `ManyToOne` réciproque est générée dans la classe de destination. L'association "propriétaire" de la relation est **toujours** l'association `ManyToOne`.

### ManyToOne

L'association `OneToMany` réciproque est générée dans la classe de destination. L'association "propriétaire" de la relation est **toujours** l'association `ManyToOne`.

### Enum

Sur une classe, lorsque sont remplis les critères suivants :

- La classe a au moins une propriété
- La classe a des valeurs
- La classe n'a pas `enum: false`
- La clé primaire ou, à défaut, la première propriété, remplit les critères suivants :
  - Le domain a `autoGenerated: false`
  - Le type `java` est `string`
  - Toutes les valeurs de cette propriété peuvent être des enums Java

Alors la classe est une `enum`.

Il existe deux modes de gestion des enums, en fonction de ce qui est défini sur la classe.

#### Mode `enum: class`

- Une enum java `[NomDeLaClasse][NomDeLaPropriété]` est créée en utilisant la propriété `enumsPath` de la configuration, dont les valeurs sont les valeurs possibles définies dans le modèle.
- La classe générée est identique à ce qu'elle aurait été si elle n'avait pas été une enum sauf :
  - Le type de la propriété d'enum est l'enum `[NomDeLaClasse][NomDeLaPropriété]`
  - Si la classe est persistée, l'annotation `@Enumerated(EnumType.STRING)` est ajoutée
  - Des membres statiques sont ajoutés à la classe, représentant les différentes valeurs possibles ajoutées dans les `values`
  - Un constructeur prenant en entrée une instance de l'enum `[NomDeLaClasse][NomDeLaPropriété]` est ajouté

Exemple

```java
/**
 * Type de droit.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "TYPE_DROIT")
@Immutable
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
public class TypeDroit {

 @Transient
 public static final TypeDroit ADMIN = new TypeDroit(TypeDroitCode.ADMIN);
 @Transient
 public static final TypeDroit READ = new TypeDroit(TypeDroitCode.READ);
 @Transient
 public static final TypeDroit WRITE = new TypeDroit(TypeDroitCode.WRITE);

 /**
  * Code du type de droit.
  */
 @Id
 @Column(name = "TDR_CODE", nullable = false, length = 10, columnDefinition = "varchar")
 @Enumerated(EnumType.STRING)
 private TypeDroitCode code;

 /**
  * Libellé du type de droit.
  */
 @Column(name = "TDR_LIBELLE", nullable = false, length = 100, columnDefinition = "varchar")
 private String libelle;

 /**
  * No arg constructor.
  */
 public TypeDroit() {
  // No arg constructor
 }

 /**
  * Enum constructor.
  * @param code Code dont on veut obtenir l'instance.
  */
 public TypeDroit(TypeDroitCode code) {
  this.code = code;
  switch(code) {
  case ADMIN :
   this.libelle = "securite.profil.typeDroit.values.Admin";
   break;
  case READ :
   this.libelle = "securite.profil.typeDroit.values.Read";
   break;
  case WRITE :
   this.libelle = "securite.profil.typeDroit.values.Write";
   break;
  }
 }
  /// ... Le reste de la génération est inchangé par rapport aux autres classes
}
```

```java
/**
 * Enumération des valeurs possibles de la propriété Code de la classe TypeDroit.
 */
public enum TypeDroitCode {
 /**
  * Administration.
  */
 ADMIN,
 /**
  * Lecture.
  */
 READ,
 /**
  * Ecriture.
  */
 WRITE
}
```

#### Mode `enum: true`

Si la classe a l'attribut `enum: true`, le traitement des enums diffère en ces points :

- Une enum java `[NomDeLaClasse]` est créée, dont les valeurs sont les valeurs possibles définies dans le modèle, et contenant toutes les valeurs des propriétés de la classe. Cette enum est générée avec le chemin contenu dans la propriété `enumsPath` de la config
- Cette enum n'est donc pas une entité
- Les associations vers cette classe ne sont donc plus des associations au sens JPA, mais des colonnes classiques. Elles portent donc l'annotation `Column`, et puisqu'elles sont de type `enum`, elles portent également l'annotation `@Enumerated(EnumType.STRING)`
- Les noms de propriétés restent identiques au mode par défaut, ce qui permet de garder la compatibilité avec les autres générateurs

Exemple : L'entité TypeDroit est une enum dans le mode `enumsAsEnums`. On peut considérer cette enum comme une contraction de l'enum et de l'entité générées dans le mode par défaut.

```java
////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

package topmodel.jpa.sample.demo.enums.securite.profil;

/**
 * Enumération des valeurs possibles de la classe TypeDroit.
 */
public enum TypeDroit {
 /**
  * Lecture.
  */
 READ("securite.profil.typeDroit.values.Read"),

 /**
  * Ecriture.
  */
 WRITE("securite.profil.typeDroit.values.Write"),

 /**
  * Administration.
  */
 ADMIN("securite.profil.typeDroit.values.Admin");

 /**
  * Libelle.
  */
 private final String libelle;

 /**
  * Enum values constructor.
  */
 private TypeDroit(final String libelle) {
  this.libelle = libelle;
 }

 /**
  * Getter for libelle.
  */
 public String getLibelle() {
  return this.libelle;
 }

}
```

## Classes non persistées

Les classes non persistées sont générées de la même manière que les classes persistées, mais ne reçoivent pas les annotations JPA.

Par ailleurs, elles implémentent toutes l'interface `java.io.Serializable`. Est ajoutée la propriété suivante :

```java
  /** Serial ID */
  private static final long serialVersionUID = 1L;
```

De plus, toutes les propriétés `required: true` reçoivent l'annotation `javax.validation.constraints.NotNull` (ou `jakarta.validation.constraints.NotNull` selon la configuration choisie).
Par ailleurs, si le domain a :

- Le type java est `String`, `CharSequence`, `Set`, `Map`, `List`, ou `Collection`
- `length` est défini

Alors la propriété portera l'annotation `@Size(max = [length défini dans le domain])`.

Également, si le domain a :

- Le type java est `BigDecimal`, `BigInteger`, `byte`, `short`, `int`, `long`, `Byte`, `Short`, `Integer`, `Long`, `double` ou `Double`
- `length` est défini ou `scale` est défini

Alors la propriété portera l'annotation `@Digits(integer = [length défini dans le domain], fraction = [scale défini dans le domain])`.

**Précautions d'emploi :**

- Ne pas composer avec une entité persistée

## Classes abstraites

Pour générer des interfaces à partir d'une classe du modèle, vous pouvez passer la propriété `abstract` d'une classe à `true`.
Ainsi, le fichier généré sera non plus une classe mais une interface ne contenant que des getters pour chacune des propriétés.

Le cas d'usage typique est celui des [projections de Spring JPA](https://docs.spring.io/spring-data/jpa/docs/current/reference/html/#projections).

```yaml
---
class:
  name: IUtilisateur
  comment: Interface de projection
  abstract: true
```

## FieldsEnum

Il est possible de générer dans la définition de la classe, la sous-classe (qui est une enum) `Fields`. Il s'agit d'une énumération des champs de la classe, au format const case.
Il faut pour cela compléter la propriété `fieldsEnum` à la configuration JPA, qui est une liste des types de classes pour laquelle on veut générer cette enum : les classes persistées (avec `"persisted"`) et/ou non persistées (avec `"non-persisted"`).

Il est également possible d'ajouter la référence d'une interface à cette configuration. Cette interface sera implémentée par la classe `Fields`. Vous pourrez ainsi la manipuler plus facilement. Si l'interface en question est suffixée par `<>`, alors elle sera considérée comme générique de la classe persistée.

Exemple :

La configuration suivante

```yaml
fieldsEnum: ["persisted"]
fieldsEnumInterface: topmodel.exemple.utils.IFieldEnum<>
```

Génèrera, dans la classe `Departement`, l'enum suivante :

```java
    public enum Fields implements IFieldEnum<Departement> {
         ID, //
         CODE_POSTAL, //
         LIBELLE
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

Localisation des classes d'enums, relative au répertoire de génération.

_Templating_: `{app}`, `{module}`

_Valeur par défaut_: `"javagen:{app:path}/enums/{module:path}"`

_Variables par tag_: **oui** (plusieurs définitions de classes pourraient être générées si un fichier a plusieurs tags)

### `enumsValuesPath`

Localisation des classes d'enums dans le mode `enumsAsEnums`, relative au répertoire de génération.

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

Option pour générer des méthodes d'ajout pour les associations `oneToMany`. Ces méthodes permettent de synchroniser les objets ajoutés en mettant à jour la relation réciproque.

_Valeur par défaut_: `false`

**Exemple :**

Pour une association `OneToMany` entre `Utilisateur` et `Commande`, si `associationAdders: true`, une méthode `addCommande(Commande commande)` sera générée dans la classe `Utilisateur`. Cette méthode ajoutera la commande à la liste et mettra à jour la référence réciproque (`commande.setUtilisateur(this)`).

### `associationRemovers`

Option pour générer des méthodes de suppression pour les associations `oneToMany`. Ces méthodes permettent de synchroniser les objets supprimés en mettant à jour la relation réciproque.

_Valeur par défaut_: `false`

**Exemple :**

Pour une association `OneToMany` entre `Utilisateur` et `Commande`, si `associationRemovers: true`, une méthode `removeCommande(Commande commande)` sera générée dans la classe `Utilisateur`. Cette méthode retirera la commande de la liste et mettra à jour la référence réciproque (`commande.setUtilisateur(null)`).

### `cascadeTypes`

Types de cascade à ajouter sur les associations JPA générées, par type d'association.

Les clés configurables sont :

- `oneToOne` : cascade(s) à ajouter sur les associations `@OneToOne`
- `oneToMany` : cascade(s) à ajouter sur les associations `@OneToMany`
- `manyToOne` : cascade(s) à ajouter sur les associations `@ManyToOne`

Les valeurs possibles pour chaque clé sont : `all`, `persist`, `merge`, `remove`, `refresh`, `detach`, `lock`.

_Valeur par défaut_: `
{
  oneToOne: [all],
  oneToMany: [all]
}`

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
```

**Code Java généré :**

Avec la configuration ci-dessus, une association `@OneToMany` produira :

```java
@OneToMany(cascade = { CascadeType.ALL }, fetch = FetchType.LAZY, mappedBy = "commande")
private List<LigneCommande> lignes;
```

Et une association `@OneToOne` avec `oneToOne: [all, detach]` produira :

```java
@JoinColumn(name = "AVI_ID", referencedColumnName = "AVI_ID", unique = true)
@OneToOne(cascade = { CascadeType.ALL, CascadeType.DETACH }, fetch = FetchType.LAZY, optional = true)
private AvisClient avisClient;
```

### `dbSchema`

Nom du schéma de base de données sur lequel les entités sont sauvegardées. Cette propriété est utilisée pour générer les annotations `@Table` avec le schéma approprié.

_Templating_: `{module}`

_Variables par tag_: **oui** (plusieurs schémas pourraient être utilisés si un fichier a plusieurs tags)

**Exemple :**

```yaml
jpa:
  - tags:
      - entity
  dbSchema: public
```

Pour un schéma par module :

```yaml
jpa:
  - tags:
      - entity
  dbSchema: "{module}"
```
