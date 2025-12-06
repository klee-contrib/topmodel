# Jpa Generator

## Présentation

Le générateur JPA peut générer les fichiers suivants :

- Un fichier de définition de classe pour chaque classe dans le modèle.
- Un fichier de définition de classe pour chaque enum.
- Un fichier d'interface DAO `JpaRepository` pour chacune des classes persistées du modèle.
- Un (ou deux) fichier(s) par module avec les mappers des classes du module.
- Un fichier de contrôleur pour chaque fichier d'endpoints dans le modèle, si les APIs sont générées en mode serveur.
- Un fichier de client d'API pour chaque fichier d'endpoints dans le modèle, si les APIs sont générées en mode client.
- Des fichiers de resources contenant les traductions (`label`) du modèle

Sur toutes les classes, interfaces générées, est ajoutée l'annotation `@Generated("TopModel : https://github.com/klee-contrib/topmodel")` pour permettre de retrouver la doc au cas où 😜. Cette annotation peut être masquée avec le paramètre `generatedHint`.

### Compatibilité avec les options TopModel

Le générateur JPA est compatible avec les options globales de TopModel :

- **`preservePropertyCasing`** : Lorsque cette option est activée dans la configuration TopModel, les noms de propriétés conservent leur casse d'origine (camelCase, PascalCase, etc.) au lieu d'être normalisés. Le générateur JPA adapte automatiquement les noms des getters et setters en conséquence.

### Générateurs

| Nom                   | Condition d'activation                                             | Objets ciblés                                                                                                                    | Fichiers générés                                                                                                                                                                                                                                                           |
| --------------------- | ------------------------------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| JavaDtoGen            | Toujours                                                           | Classes non persistées qui ne sont pas des enums                                                                                 | Pojo contenant les propriétés définies dans le modèle avec les annotations de validation                                                                                                                                                                                   |
| JdbcEntityGen         | `useJdbc: true`                                                    | Classes persistées                                                                                                               | Pojo contenant les propriétés définies dans le modèle, annotées avec les annotations de la persistance Jdbc                                                                                                                                                                |
| JpaDaoGen             | `daosPath` défini                                                  | Classes persistées qui ne sont pas des enums                                                                                     | Interface Repository permettant de requêter la classe en question                                                                                                                                                                                                          |
| JpaEntityGen          | `useJdbc: false`                                                   | Classes persistées qui ne sont pas des enums                                                                                     | Pojo contenant les propriétés définies dans le modèle, annotées avec les annotations de la persistance JPA                                                                                                                                                                 |
| JpaEnumEntityGen      | `useJdbc: false` && `enumsAsEnums: false`                          | Classes persistées qui sont des enums                                                                                            | Pojo contenant les propriétés définies dans le modèle, annotées avec les annotations de la persistance JPA. Contient également des membres statiques représentant les entitées décrites dans les values                                                                    |
| JpaEnumGen            | `useJdbc: false` && `enumsAsEnums: false`                          | Classes persistées ou non qui sont des enums                                                                                     | Enumération des valeurs possible de la clé primaire de la classe                                                                                                                                                                                                           |
| JavaEnumDtoGen        | `useJdbc: false` && `enumsAsEnums: false`                          | Classes non persistées qui sont des enums                                                                                        | Pojo contenant les propriétés définies dans le modèle, annotées avec les annotations de validation. Contient également des membres statiques représentant les instances décrites dans les values                                                                           |
| JpaEnumValuesGen      | `useJdbc: false` && `enumsAsEnums: true`                           | Enum contenant toutes les valeurs définies dans les values, dont la clé est la primaryKey ou la première propriété de la classe. |
| JpaInterfaceGen       | Toujours                                                           | Classes qui ont `abstract: true`                                                                                                 | Interface ne contenant que des `getters` des propriétés définies dans le modèle. Peut également définir une méthode `hydrate`, s'apparentant à un constructeur                                                                                                             |
| SpringDataFlowGen     | `dataFlowsPath` défini                                             | Dataflows                                                                                                                        | Définition d'un job par module, et d'un step par dataFlow. Peut également générer une interface à implémenter pour les source en mode`partial` et les `hook` ajoutés                                                                                                       |
| FeignClientApiGen     | `apiGeneration: client` && `clientApiGeneration: feignClient`      | Endpoints                                                                                                                        | Interface contenant les annotations nécessaires à la construction par Feign d'une API cliente.                                                                                                                                                                             |
| SpringApiClientGen    | `apiGeneration: client` && `clientApiGeneration: restClient`       | Endpoints                                                                                                                        |
| SpringRestTemplateGen | `apiGeneration: client` && `clientApiGeneration: restTemplate`     | Endpoints                                                                                                                        | Classe abstraite définissant les méthodes permettant d'appeler une API externe à l'aide d'un RestTemplate Spring.                                                                                                                                                          |
| SpringApiServerGen    | `apiGeneration: server`                                            | Endpoints                                                                                                                        | Interface définissant les méthodes annotées permettant de définir une API serveur. L'implémentation est à la charge du développeur                                                                                                                                         |
| JpaMapperGenerator    | Toujours                                                           | Mappers                                                                                                                          | Classe statique contenant des méthodes statiques, correspondant aux mappers définis dans le modèle                                                                                                                                                                         |
| JpaResourceGen        | `resourcesPath` défini                                             | Classes qui contiennent des labels ou des values qui ont des defaultProperty                                                     | Fichiers de resource `.properties` dans les différentes langues de l'application. Les clés sont les clés de traduction des labels des propriétés du modèle, et dont les valeurs sont les labels définis dans le modèle dans la langue de développement, ou leur traduction |
| JpaMetaModelGenerator | `metaModel: true`                                                  | Entités persistées                                                                                                               | Classes représentant le métamodèle des entités persistées. Une classe par entité.                                                                                                                                                                                          |

## Génération des classes

Le générateur de classes distingue cinq cas :

- Les classes persistées qui ne sont pas des enums
- Les classes non persistées qui ne sont pas des enums
- Les classes persistées qui sont des enums
- Les classes non persistées qui sont des enums
- Les classes abstraites

### Support des annotations Lombok

Le générateur détecte automatiquement la présence d'annotations Lombok sur les classes ou les propriétés :

- Si une classe possède l'annotation `@Data`, `@Getter` ou `@Setter`, les getters et setters ne sont pas générés pour cette classe
- Si une propriété possède l'annotation `@Getter` ou `@Setter`, le getter ou setter correspondant n'est pas généré pour cette propriété

Cette fonctionnalité permet d'utiliser Lombok pour réduire le code boilerplate tout en conservant la génération des autres éléments (constructeurs, annotations JPA, etc.).

Les propriétés générées sont `private`, du type défini dans le `domain`. Le commentaire leur étant associé correspond au commentaire défini dans le modèle.

Des `getter` et `setter` sont ajoutés automatiquement, sauf si la classe ou la propriété possède une annotation Lombok (`@Data`, `@Getter`, ou `@Setter`). Dans ce cas, les getters et setters ne sont pas générés, car ils sont fournis par Lombok. Seul un constructeur vide est ajouté dans la classe générée.

### Classes persistées

Les classes persistées sont générées avec les annotations correspondant à ce qui est paramétré dans le modèle.
Sur la classe :

| Annotation                                          | Paramètre correspondant dans le modèle                                               |
| --------------------------------------------------- | ------------------------------------------------------------------------------------ |
| `@Entity`                                           | Automatique                                                                          |
| `@Table("SQL_NAME")`                                | Automatique                                                                          |
| `@UniqueConstraint`                                 | `unique` : pour chacune des contraintes d'unicité de la classe                       |
| `Cache(usage = CacheConcurrencyStrategy.READ_ONLY)` | si la classe a `reference: true`. La stratégie dépend du `domain` de la clé primaire |

Sur chacune des propriété :

| Annotation                     | Paramètre correspondant dans le modèle                                                                      |
| ------------------------------ | ----------------------------------------------------------------------------------------------------------- |
| `@Id`                          | `primaryKey: true` : sur la clé primaire                                                                    |
| `@Enumerated(EnumType.STRING)` | Sur la clé primaire, si TopModel a détecté qu'il s'agissait bien d'une enum                                 |
| `@SequenceGenerator`           | `primaryKey: true` : sur la clé primaire si `identity: mode: sequence` dans la configuration générale       |
| `@GeneratedValue`              | `primaryKey: true` : sur la clé primaire si `identity: mode: sequence` dans la configuration générale       |
| `@Column`                      | Sur les propriétés qui ne sont ni des compositions, ni des associations.                                    |
| `@OneToOne`                    | `type: OneToOne` sur une association                                                                        |
| `@ManyToOne`                   | `type: ManyToOne` sur une association                                                                       |
| `@OneToMany`                   | `type: OneToMany` sur une association                                                                       |
| `@ManyToMany`                  | `type: ManyToMany` sur une association                                                                      |
| `@JoinColumn`                  | Sur les associations `manyToOne` et `oneToOne`                                                              |
| `@JoinTable`                   | Sur les associations `manyToMany`                                                                           |
| `@OrderBy`                     | Sur les associations `manyToMany` et `oneToMany` pour lesquelles la classe cible définit une `orderProperty` |
| `@Convert`                     | Sur les compositions. Le converter utilisé est paramétrable.                                                |

Les paramétrages de ces annotations correspondent à ce qui est défini dans le modèle ou dans la configuration, à l'exception de :

- `fetch = FetchType.LAZY` pour tous les types d'associations, pour optimisation des performances
- `cascade = { CascadeType.ALL }` pour les associations `OneToMany` et leur association réciproque `ManyToOne`
- `cascade = { CascadeType.ALL }` pour les associations `OneToOne`

Par ailleurs, dès lors qu'une association est faite entre deux classes, si :

- Les deux classes ont même package racine
- La classe de destination n'est pas une liste de référence
- L'association n'est pas de type `oneToOne`

alors l'association réciproque sera générée dans la classe cible.

#### ManyToMany

L'association `ManyToMany` réciproque est générée dans la classe de destination. L'association "propriétaire" de la relation est celle déclarée dans le modèle TopModel.

#### OneToMany

L'association `ManyToOne` réciproque est générée dans la classe de destination. L'association "propriétaire" de la relation est **toujours** l'association `ManyToOne`

#### ManyToOne

L'association `OneToMany` réciproque est générée dans la classe de destination. L'association "propriétaire" de la relation est **toujours** l'association `ManyToOne`

#### OneToOne

Pour des raisons de performances, les associations oneToOne réciproques ne sont pas générées.

#### Enum

Sur une classe, lorsque sont remplis les critères suivants :

- La classe a au moins une propriété
- La classe a des valeurs
- La classe n'a pas `enum: false`
- La clé primaire ou, à défaut, la première propriété, remplit les critère suivants :
  - Le domain a `autoGenerated: false`
  - Le type `java` est `string`
  - Toutes les valeurs de cette propriété peuvent être des enums Java

Alors la classe est une `enum`

Il existe deux modes de gestion des enums.

##### Mode par défaut

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

##### Mode `enumsAsEnums`

En ajoutant la propriété `enumsAsEnums: true` à la configuration, le traitement des enums diffère en ces points :

- Une enum java `[NomDeLaClasse]` est créée, dont les valeurs sont les valeurs possibles définies dans le modèle, et contenant toutes les valeurs des propriétés de la classe. Cette enum est générée avec le chemin contenu dans la propriété `enumsPath` de la config, sauf si la variable `enumsValuesPath` est définie.
- Cette enum n'est donc pas une entité
- Les associations vers cette classe ne sont donc plus des associations au sens JPA, mais des colonnes classiques. Elles portent donc l'annotation `Column`, et puisqu'elles sont de type `enum`, elles portent également l'annotation `@Enumerated(EnumType.STRING)`
- Les noms de propriétés restent identiques au mode par défaut, ce qui permet de garder la compatibilité avec les autres générateurs

Exemple : L'entité TypeDroit est une enum dans le mode `enumsAsEnums`. On peut considérer cette enum comme une contraction de l'enum et de l'entité générés dans le mode par défaut.

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

#### Classes non persistées

Les classes non persistées sont générées de la même manière que les classes persistées, mais ne reçoivent pas les annotations JPA.

Par ailleurs, elles implémentent toutes l'interface `java.io.Serializable`. Est ajouté la propriété suivante :

```java
  /** Serial ID */
  private static final long serialVersionUID = 1L;
```

De plus, toutes les propriétés `required: true` reçoivent l'annotation `javax.validation.constraints.NotNull` (ou `jakarata.validation.constraints.NotNull` selon la configuration choisie).
Par ailleurs, si le domain a :

- Le type java est `String`, `CharSequence`, `Set`,`Map`,`List`, ou `Collection`
- `length` est défini
  Alors la propriété portera l'annotation `@Size(max = [length défini dans le domain])`

Egalement, si le domain a :

- Le type java est `BigDecimal`, `BigInteger`, `byte`, `short`, `int`, `long`, `Byte`, `Short`, `Integer`, `Long`, `double` ou `Double`
- `length` est défini ou `scale` est défini
  Alors la propriété portera l'annotation `@Digits(integer = [length défini dans le domain], fraction = [scale défini dans le domain])`

**Précautions d'emploi :**

- Ne pas composer avec une entité persistée

#### Classes abstraites

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

Si certaines d'entre ont `readonly: false`, qui est la valeur par défaut, alors une méthode `hydrate` sera générée, prenant en paramètre toutes les propriétés non `readonly`. Il s'agit d'un `setter` unique. Ce comportement est identique dans les autres modules standards (C#...).

Exemple :

```java
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
public interface IUtilisateurDto {

  /**
   * Getter for id.
   *
   * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#id id}.
   */
   long getId();

  /**
   * Getter for email.
   *
   * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#email email}.
   */
   String getEmail();

  /**
   * Getter for typeUtilisateurCode.
   *
   * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#typeUtilisateurCode typeUtilisateurCode}.
   */
   TypeUtilisateur.Values getTypeUtilisateurCode();

  /**
   * Getter for profilId.
   *
   * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#profilId profilId}.
   */
  long getProfilId();

  /**
   * Getter for profilTypeProfilCode.
   *
   * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#profilTypeProfilCode profilTypeProfilCode}.
   */
  TypeProfil.Values getProfilTypeProfilCode();

  /**
   * Getter for utilisateurParent.
   *
   * @return value of {@link topmodel.exemple.name.dao.dtos.utilisateur.UtilisateurDto#utilisateurParent utilisateurParent}.
   */
  UtilisateurDto getUtilisateurParent();
}

```

## Génération des DAO

Un fichier d'interface DAO est généré pour chacune des classes persistées du modèle. Cette interface hérite de `JpaRepository`, et est paramétrée pour gérer l'entité correspondante.

**Ce fichier n'est généré qu'une seule fois !!**. Vous pouvez donc le modifier pour ajouter les différentes méthodes d'accès dont vous auriez besoin. C'est tout l'intérêt.

Il est possible de modifier le comportement de ce générateur avec les configurations suivantes:

### `daosAbstract`

Ajoute l'annotation `NoRepositoryBean` et renomme l'interface en `Abstract[NomDeLaClasse]DAO` par défaut

### `daosName`

Permet de surcharger le nom du DAO généré. Placer la valeur `{classe}` dans le nom pour remplacer par le nom de la classe. Par défaut, il s'agit de `{classe}DAO` lorsque `daosAbstract` est à `false`, et `Abstract{classe}DAO` sinon. Par exemple, `daosName: "{classe}Repository"` générera `UtilisateurRepository` pour la classe `Utilisateur`.

### `DaosInterface`

Modifie le repository dont le repository hérite.
Par défaut, dans le mode JDBC, il s'agit de `org.springframework.data.repository.CrudRepository`, et dans le mode JPA : `org.springframework.data.jpa.repository.JpaRepository`.

## Génération des mappers

Les mappers sont générés comme des méthodes statiques dans une classe statique. Cette classe rassemble tous les mappers d'un module racine. Elle est positionnée dans le package des entités si l'une des deux classes est persistée, et dans le package des DTOs sinon.

**Remarque :** Le module utilisé pour un mapper est celui de la classe persistée qui a été trouvée, ou à défaut celui de la classe qui définit le mapper.

### Mappers `from`

Les mappers `from` sont générés sous deux formes :

- **`create[Nom de la classe à créer]`** : Crée une nouvelle instance de la classe cible en mappant les champs sources. Cette méthode appelle en interne la méthode `mapXXX` avec une nouvelle instance.

- **`map[Nom de la classe à créer]`** : Mappe les champs sources sur une instance de la classe cible passée en paramètre. Cette méthode est publique et peut être utilisée pour peupler une instance existante. Si l'instance cible est `null`, une exception `IllegalArgumentException` est lancée.

Les deux méthodes prennent en entrée la liste des paramètres d'entrée définis dans le mapper. La méthode `mapXXX` prend également une instance de la classe cible en dernier paramètre.

### Mappers `to`

Les mappers `to` s'appellent `to[Nom de la classe cible]`, ou bien du nom défini dans le `mapper`. Dans le cas des mappers `to`, le paramètre source est unique et obligatoire.

### Intégration dans les classes

Par défaut (`mappersInClass: true`), dans les classes qui définissent le mapper :

- Des constructeurs sont générés pour tous les mappers `from`
- Une méthode `toXXX` est générée pour chacun des mappers `to`

Cette option est désactivable avec la configuration `mappersInClass: false`.

### Gestion des erreurs

Si un paramètre d'entrée obligatoire n'est pas renseigné, l'exception `IllegalArgumentException` est lancée.

## Génération des endpoints

Le générateur d'endpoints crée des interfaces ou classes permettant de définir des APIs serveur ou client. Le nom du fichier généré et son emplacement sont déterminés selon les règles suivantes :

### Détermination du nom de fichier

Le nom de la classe générée est déterminé par la configuration `apisName` (si définie) ou par la valeur par défaut du mode choisi. Dans tous les cas, le template `{fileName}` est remplacé par le nom du fichier d'endpoints défini dans le modèle, converti en PascalCase.

**Valeurs par défaut selon le mode :**

| Mode | Nom de classe par défaut | Type généré |
|------|-------------------------|-------------|
| **Server** | `{fileName}Controller` | Interface |
| **RestClient** | `{fileName}Client` | Interface |
| **RestTemplate** | `Abstract{fileName}Client` | Classe abstraite |
| **FeignClient** | `{fileName}Api` | Interface |

**Exemple :** Pour un fichier d'endpoints nommé `utilisateur`, les noms générés seront :

- Mode Server : `UtilisateurController`
- Mode RestClient : `UtilisateurClient`
- Mode RestTemplate : `AbstractUtilisateurClient`
- Mode FeignClient : `UtilisateurApi`

### Personnalisation du nom

Le nom peut être personnalisé via la configuration `apisName`, qui remplace la valeur par défaut du mode. Le template `{fileName}` sera toujours remplacé par le nom du fichier d'endpoints en PascalCase.

**Exemple de personnalisation :**

```yaml
jpa:
  - tags:
      - api
    apiGeneration: Server
    apisName: "{fileName}Service"  # Génère UtilisateurService au lieu de UtilisateurController
```

### Emplacement des fichiers

Le chemin du fichier est déterminé par la configuration `apiPath`, qui peut utiliser les variables suivantes :

- `{app}` : Nom de l'application
- `{module}` : Module du fichier d'endpoints
- Variables personnalisées définies dans la configuration

La valeur par défaut de `apiPath` est `"javagen:{app:path}/api/{module:path}"`.

Le chemin complet du fichier sera : `{outputDirectory}/{apiPath}/{nomClasse}.java`

### Génération de l'Api Server (Spring)

Le générateur crée des interfaces contenant, pour chaque `endpoint` paramétré, la méthode abstraite correspondant au nom de l'endpoint, à implémenter dans votre controller. Cette méthode aura déjà l'annotation `XXXMapping` correspondant au verbe HTTP défini dans l'endpoint (par exemple `@GetMapping`, `@PostMapping`, etc.).

Pour créer votre API, il suffit donc de créer un nouveau controller qui implémente l'interface générée. L'annotation `@RestController` reste nécessaire.

**Comportements automatiques :**

- Si le domain du body de l'endpoint définit un `mediaType`, alors il sera valorisé dans l'annotation avec l'attribut `consumes`
- De la même manière pour le domain du paramètre de retour, avec l'attribut `produces`
- Si la méthode retourne `void` ou `Void`, l'annotation `@ResponseStatus(HttpStatus.NO_CONTENT)` (code HTTP 204) est automatiquement ajoutée à la méthode

**Exemple :**

```java
@RestController
public class UtilisateurControllerImpl implements UtilisateurController {
  
  @Override
  public UtilisateurDto getUtilisateur(Long id) {
    // Implémentation
  }
}
```

### Api Client (Spring)

#### RestClient (Spring Web 6+)

Il s'agit du mode par défaut, soit lorsque la variable `clientApiGeneration` vaut `RestClient`.

Le générateur crée alors des interfaces contenant des annotations `XXXExchange`, dont il faudra configurer un bean d'implémentation.

**Note importante :** Les méthodes générées retournent toujours un `ResponseEntity<T>` (où `T` est le type de retour défini dans l'endpoint), permettant de gérer les différents codes HTTP de réponse.

**Exemple de configuration :**

```java
@Bean
protected UtilisateurApiClient utilisateurApiClient() {
  var restClient = RestClient.builder()
    .baseUrl("http://localhost:8080/my-app/api/")
    .build();
  var adapter = RestClientAdapter.create(restClient);
  var factory = HttpServiceProxyFactory.builderFor(adapter).build();
  return factory.createClient(UtilisateurApiClient.class);
}
```

#### RestTemplate

Pour activer ce mode de génération, positionner la variable `clientApiGeneration` à `RestTemplate`.

Le générateur crée alors des classes abstraites contenant toutes les méthodes permettant d'accéder aux endpoints paramétrés.

**Note importante :** Les méthodes générées retournent toujours un `ResponseEntity<T>` (où `T` est le type de retour défini dans l'endpoint), permettant de gérer les différents codes HTTP de réponse.

Pour créer votre client d'API, il suffit de créer une classe qui hérite de cette classe abstraite. Pour fonctionner, elle devra appeler le constructeur de la classe abstraite, en renseignant :

- Le host de l'API
- Une instance de `RestTemplate`

**Exemple d'implémentation :**

```java
@Service
public class UtilisateurApiClient extends AbstractUtilisateurApiClient {

  private static final String HOST = "http://localhost:8080/my-app/api/";

  @Autowired
  public UtilisateurApiClient(RestTemplate restTemplate) {
    super(restTemplate, HOST);
  }
}
```

**Exemple d'utilisation :**

```java
@Service
public class UtilisateurService {

  private final UtilisateurApiClient utilisateurApiClient;

  @Autowired
  public UtilisateurService(UtilisateurApiClient utilisateurApiClient) {
    this.utilisateurApiClient = utilisateurApiClient;
  }

  public UtilisateurDto getUtilisateur(Long id) {
    var headers = new HttpHeaders();
    headers.add("token-securise", "MON_TOKEN_SECURISE");
    return utilisateurApiClient.getUtilisateur(id, headers);
  }
}
```

#### FeignClient

Pour activer ce mode de génération, positionner la variable `clientApiGeneration` à `FeignClient`.

Le générateur crée des interfaces similaires au mode `Server`, à la différence près que le suffixe est `Api` au lieu de `Controller`, et que l'annotation `@FeignClient` est ajoutée à l'interface.

**Note :** Ce mode nécessite la dépendance Spring Cloud OpenFeign.

## Dépendances

```xml
<dependency>
    <groupId>org.springframework.cloud</groupId>
    <artifactId>spring-cloud-starter-openfeign</artifactId>
</dependency>
```

### Modèle

Le modèle généré par TopModel dépend d'une api de persistence. Par défaut, c'est l'API de persistence `javax` qui est utilisée, mais le mode `jakarta` est aussi disponible.

La validation est gérée par le package `jakarta.validation-api`, dont les imports changent entre la version 2 et la version 3.

#### Javax (spring-boot < v3)

```xml
<!-- https://mvnrepository.com/artifact/javax.persistence/javax.persistence-api -->
<dependency>
    <groupId>javax.persistence</groupId>
    <artifactId>javax.persistence-api</artifactId>
</dependency>

<!-- https://mvnrepository.com/artifact/jakarta.validation/jakarta.validation-api -->
<dependency>
  <groupId>jakarta.validation</groupId>
  <artifactId>jakarta.validation-api</artifactId>
</dependency>
```

#### Jakarta (spring-boot > v3)

```xml
<!-- https://mvnrepository.com/artifact/jakarta.persistence/jakarta.persistence-api -->
<dependency>
    <groupId>jakarta.persistence</groupId>
    <artifactId>jakarta.persistence-api</artifactId>
    <version>3.1.0</version>
</dependency>

<!-- https://mvnrepository.com/artifact/jakarta.validation/jakarta.validation-api -->
<dependency>
  <groupId>jakarta.validation</groupId>
  <artifactId>jakarta.validation-api</artifactId>
</dependency>
```

### Endpoints

Actuellement, la seule génération de endpoint cliente et serveur qui est gérée passe par les API de `Spring-web`

```xml
<!-- https://mvnrepository.com/artifact/org.springframework/spring-web -->
<dependency>
    <groupId>org.springframework</groupId>
    <artifactId>spring-web</artifactId>
</dependency>
```

Si l'option `openApiAnnotations` est activée, les annotations de cette librairie sont utilisées :

```xml
<!-- https://mvnrepository.com/artifact/io.swagger.core.v3/swagger-annotations-jakarta -->
<dependency>
    <groupId>io.swagger.core.v3</groupId>
    <artifactId>swagger-annotations-jakarta</artifactId>
</dependency>
```

### Version Java

Le code Java généré est compatible avec toutes les versions de Java postérieures à `Java 11`.

## Utilisation combinée avec le générateur postgresql

Le mode de génération par défaut des générateurs ne crée pas de séquence, mais des colonnes auto-générées avec `identity`. Malheureusement, le `batch insert` de JDBC ne fonctionne pas correctement avec ce mode de génération d'ID. Il est donc recommandé d'utiliser le mode `sequence` du générateur PostgreSQL.

Le mode `sequence` dans la configuration JPA et dans la configuration PostgreSQL se déclare de la même manière :

```yaml
## Configuration jpa et proceduralSql
identity:
  increment: 50
  start: 1000
  mode: sequence
```

## FieldsEnum

Il est possible de générer dans la définition de la classe, la sous-classe (qui est une enum) `Fields`. Il s'agit d'une enumération des champs de la classe, au format const case.
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

## Générateur de resources

Le générateur de resources s'appuie sur les `Label` des propriétés, ainsi que sur les traductions récupérées dans le cadre de la configuration du [multilinguisme](/model/i18n.md).

Il suffit d'ajouter la configuration `resourcesPath` au générateur comme suit :

```yaml
jpa:
  - tags:
      - dto
    resourcesPath: resources/i18n/model # Chemin des fichiers de ressource générés.
```

Pour que, pour chaque module, soit généré les fichiers de resources dans les différentes langues configurées globalement.

Par défaut, les fichiers sont générés avec l'encodage Latin1, mais il est possible de les générer en UTF8 avec la propriété resourcesEncoding

```yaml
jpa:
  - tags:
      - dto
    resourcesPath: resources/i18n/model # Chemin des fichiers de ressource générés.
    resourcesEncoding: UTF8 # Encodage fichiers de ressource générés (Latin1 ou UTF8).
```

## Générateur de flow

Le générateur de data flow s'appuie sur `spring-batch`. Il permet de générer du code permettant de récupérer des données d'une source, appliquer éventuellement une transformation, puis les insérer dans une base de données. Les outils mis en oeuvre ont été sélectionnés pour leur capacité à traiter un grand nombre de données, avec les meilleures performances possibles.

> Il est recommandé de maîtriser le fonctionnement de `spring-batch` avant de tenter de générer des flows avec `TopModel`.

### Fichiers générés

#### Flow

Le générateur crée un fichier par dataFlow, comprenant :

- **Reader** : Lit les données depuis la source (base de données, API, etc.)
- **Writer** : Écrit les données dans la base de données cible
- **TruncateTasklet** (éventuellement) : Vide la table cible avant l'insertion si configuré
- **Step** : Définit une étape du job Spring Batch
- **Flow** : Définit le flux de traitement des données

La génération s'appuie sur Spring Batch, mais aussi la librairie `spring-batch-bulk`, qui permet des performances exceptionnelles grâce à l'utilisation du bulk insert PostgreSQL (avec la commande `COPY`).

```xml
  <dependency>
    <groupId>io.github.klee-contrib</groupId>
    <artifactId>spring-batch-bulk</artifactId>
    <version>0.0.3</version>
  </dependency>
```

#### Reader

Le reader privilégié est le reader `JdbcCursorItemReaderBuilder`. Il permet d'obtenir les meilleures performances, et offre une meilleure flexibilité (choix de la source de données, requête).

Avec le mode `partial`, le reader n'est pas généré. Il faut donc fournir un `bean` dont le nom est `[Nom du flow]Reader` pour que le job fonctionne.

Il est par exemple possible de créer un `Reader` appelant une API.

##### Replace

Le truncate se fait avec la classe `TaskletQuery` de la librairie `spring-batch-bulk`. Cette approche est nettement plus performante qu'un `deleteAll` classique.

#### Processor

Si la classe source et la classe cible sont différentes, un processor est ajouté pour appeler le mapper de l'une vers l'autre

#### Writer

Il existe deux modes de génération des writers : `jpa` ou `bulk`. Le mode est configuré via la propriété `dataFlowsWriter` dans la configuration.

##### JPA

Le writer utilise le `JpaItemWriter` de Spring Batch. Ce mode est adapté pour des volumes de données modérés et offre une meilleure compatibilité avec les fonctionnalités JPA (cascades, listeners, etc.).

##### Bulk

Les writers utilisent le `PgBulkWriter` de la librairie `spring-batch-bulk`. Ce mode offre des performances exceptionnelles grâce à l'utilisation du bulk insert PostgreSQL (avec la commande `COPY`). Il est recommandé pour traiter de très gros volumes de données.

**Configuration :**

```yaml
jpa:
  - tags:
      - entity
    dataFlowsPath: topmodel/exemple/flows
    dataFlowsWriter: bulk  # ou jpa
    dataFlowsBulkSize: 100000  # Taille des chunks pour le bulk insert (par défaut: 100000)
```

##### Insert

Le writer copie directement les données dans la table cible. TopModel génère le mapping permettant de faire cette insertion.

##### Upsert

Le writer copie les données dans une table temporaire, puis recopie les données de table à table. En cas de conflit sur la clé primaire, un update est effectué. TopModel génère le mapping permettant de faire cette insertion.

#### Listeners

Il est possible d'ajouter des listeners aux dataflows via la propriété `dataFlowsListeners`. Ces listeners seront appelés aux différents hooks du flow (beforeFlow, afterFlow, etc.).

**Configuration :**

```yaml
jpa:
  - tags:
      - entity
    dataFlowsPath: topmodel/exemple/flows
    dataFlowsListeners:
      - topmodel.exemple.listeners.CustomFlowListener
```

#### Job

Le générateur crée un fichier de configuration de job par module. Ce job ordonnance les lancements des flows selon ce qui a été paramétré avec les mots-clés `dependsOn`. Il importe les configurations nécessaires à son bon fonctionnement.

### Limitations et mises en garde

- **Source de données** : Ne fonctionne que de base à base par défaut. Pour créer un reader spécifique (par exemple appelant une API), utiliser le mode `partial`
- **Base de données cible** : La base cible ne peut être qu'une base de données PostgreSQL
- **Schéma** : Il est obligatoire de définir un `dbSchema`
- **Multi-source** : Non supporté actuellement
- **Mappers** : Un mapper doit exister de la classe source vers la classe cible (sauf s'il s'agit de la même classe)
- **Dépendances entre jobs** : Deux jobs ne peuvent pas dépendre l'un de l'autre s'ils ne sont pas dans le même module
- **Ordre d'exécution** : L'ordre d'exécution des flows suit une logique de dépendances qui peut être plus restrictive que nécessaire. Par exemple, si :
  - Le flow C dépend de A et B
  - Le flow D dépend de A
  - Alors D ne se lancera qu'après A et B (alors qu'en théorie il pourrait se lancer directement après A)

## Configuration

### Fichier de configuration

- `rootModule`

  Définition du module racine, pour les différents regroupements à faire dessus (fichiers de traductions, etc.).

  _Templating_: `{module}`

  _Valeur par défaut_: `{module:head}`

- `entitiesPath`

  Localisation des classes persistées du modèle, relative au répertoire de génération.

  Le chemin des fichiers cibles sera calculé en remplaçant les `.` et le `:` par des `/` dans cette valeur, tandis que le nom du package des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

  _Templating_: `{app}`, `{module}`

  _Valeur par défaut_: `"javagen:{app:path}/entities/{module:path}"`

  _Variables par tag_: **oui** (plusieurs définitions de classes pourraient être générées si un fichier a plusieurs tags)

- `daosPath`

  Localisation des DAOs, relative au répertoire de génération.

  Le chemin des fichiers cibles sera calculé en remplaçant les `.` et le `:` par des `/` dans cette valeur, tandis que le nom du package des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

  _Templating_: `{app}`, `{module}`

  _Variables par tag_: **oui** (plusieurs DAOs pourraient être générés si un fichier a plusieurs tags)

- `daosAbstract`

  Génération des DAO sous forme 'Abstract' à hériter pour l'utiliser dans le projet avec :

  - le nom Abstract{classe.NamePascal}DAO
  - le fichier java sera mise à jour (écrasé) à chaque génération de code
  - l'annotation @NoRepositoryBean ajoutée (org.springframework.data.repository.NoRepositoryBean) permettant de ne pas considérer cette interface comme un DAO
    - il faut donc créer une interface qui en hérite dans le projet
  - le 'daosPath' peut être dans un répertoire de type 'javagen'

- `daosName`

  Nom du DAO à générer.

  _Templating_: `{class}`

  _Valeur par défaut_: `"{class}DAO"` si `daosAbstract` est à `false`, sinon `"Abstract{class}DAO"`

- `daosInterface`

  Permet de surcharger les interfaces par défaut des DAOs :

  - si UseJdbc, l'interface est org.springframework.data.repository.CrudRepository
  - si Reference, l'interface est org.springframework.data.repository.CrudRepository
  - si aucun des deux, l'interface est org.springframework.data.jpa.repository.JpaRepository
  - si daosInterface est précisée, les autres cas ne sont pas utilisés.

  Seul le nom de la classe est configurable, elle doit respecter le même pattern générique que `JpaRepository` et `CrudRepository` soit :

  - La classe de l'entité en premier
  - La classe de l'identifiant en second
  - {DaosInterface}<{classe.NamePascal}, {pk}>

- `dtosPath`

  Localisation des classes non persistées du modèle, relative au répertoire de génération.

  Le chemin des fichiers cibles sera calculé en remplaçant les `.` et le `:` par des `/` dans cette valeur, tandis que le nom du package des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

  _Templating_: `{app}`, `{module}`

  _Valeur par défaut_: `"javagen:{app:path}/dtos/{module:path}"`

  _Variables par tag_: **oui** (plusieurs définitions de classes pourraient être générées si un fichier a plusieurs tags)

- `enumsPath`

  Localisation des classes d'enums, relative au répertoire de génération.

  Le chemin des fichiers cibles sera calculé en remplaçant les `.` et le `:` par des `/` dans cette valeur, tandis que le nom du package des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

  _Templating_: `{app}`, `{module}`

  _Valeur par défaut_: `"javagen:{app:path}/enums/{module:path}"`

  _Variables par tag_: **oui** (plusieurs définitions de classes pourraient être générées si un fichier a plusieurs tags)

- `enumsValuesPath`

  Localisation des classes d'enums dans le mode `enumsAsEnums`, relative au répertoire de génération.

  Le chemin des fichiers cibles sera calculé en remplaçant les `.` et le `:` par des `/` dans cette valeur, tandis que le nom du package des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

  _Templating_: `{app}`, `{module}`

  _Valeur par défaut_: `"javagen:{app:path}/enums/{module:path}"`

  _Variables par tag_: **oui** (plusieurs définitions de classes pourraient être générées si un fichier a plusieurs tags)

- `enumsAsEnums`

  Mode de génération des enums (voir documentation)

  _Valeur par défaut_: `false`

  _Variables par tag_: **non**

- `apiPath`

  Localisation de l'API générée (client ou serveur), relative au répertoire de génération.

  Le chemin des fichiers cibles sera calculé en remplaçant les `.` et le `:` par des `/` dans cette valeur, tandis que le nom du package des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

  _Templating_: `{app}`, `{module}`

  _Valeur par défaut_: `"javagen:{app:path}/api/{module:path}"`

  _Variables par tag_: **oui** (plusieurs clients/serveurs pourraient être générés si un fichier à plusieurs tags)

- `openApiAnnotations`

  Si les annotations `swagger-annotation-jakarta` doivent être ajoutées aux interfaces. Nécessite à minima la dépendance :

  ```xml
    <!-- https://mvnrepository.com/artifact/io.swagger.core.v3/swagger-annotations-jakarta -->
    <dependency>
        <groupId>io.swagger.core.v3</groupId>
        <artifactId>swagger-annotations-jakarta</artifactId>
    </dependency>
  ```

  _Templating_: `{module}`

  _Valeur par défaut_: `"javagen:{app}/api/{module}"`

  _Variables par tag_: **oui** (plusieurs clients/serveurs pourraient être générés si un fichier à plusieurs tags)

- `apiGeneration`

  Mode de génération de l'API (`"Client"` ou `"Server"`).

  _Variables par tag_: **oui** (la valeur de la variable doit être `"Client"` ou `"Server"`. Le client et le serveur pourraient être générés si un fichier a plusieurs tags)

- `clientApiGeneration`

  Mode de génération de l'API Client. Les valeurs possibles sont :

  - `RestClient` : Génération d'un client en mode RestClient (interface Exchange) - valeur par défaut
  - `RestTemplate` : Génération d'un client en mode RestTemplate (classe abstraite à initialiser)
  - `FeignClient` : Génération d'un client en mode Feign (interface spring controller avec l'annotation Feign)

  Cette propriété n'est utilisée que lorsque `apiGeneration` est défini à `"Client"` ou contient une variable qui peut être résolue à `"Client"`.

  _Valeur par défaut_: `RestClient`

  _Variables par tag_: **non**

- `compositionConverterCanonicalName`

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

- `resourcesPath`

  Localisation des ressources, relative au répertoire de génération.

  _Variables par tag_: **oui**

- `resourcesEncoding`

  Encodage des fichiers de ressources. Les valeurs possibles sont :

  - `Latin1` : valeur par défaut
  - `UTF8`

  _Variables par tag_: **non**

- `fieldsEnum`

  Option pour générer une enum des champs de certaines classes. Il s'agit d'une liste dont les 2 valeurs possibles sont :

  - `persisted` : ajoute l'enum des champs sur les classes persistées
  - `non-persisted` : ajoute l'enum des champs sur les classes non persistées

- `fieldsEnumInterface`

  Précise l'interface des fields enum générés.

  _Templating_: `<>` (remplace par `<NomDeLaClasse>`)

- `associationAdders`

  Option pour générer des méthodes d'ajout pour les associations `oneToMany` et `manyToMany`. Ces méthodes permettent de synchroniser les objets ajoutés en mettant à jour la relation réciproques.

  _Valeur par défaut_: `false`

  **Exemple :**

  Pour une association `OneToMany` entre `Utilisateur` et `Commande`, si `associationAdders: true`, une méthode `addCommande(Commande commande)` sera générée dans la classe `Utilisateur`. Cette méthode ajoutera la commande à la liste et mettra à jour la référence réciproque (`commande.setUtilisateur(this)`).

- `associationRemovers`

  Option pour générer des méthodes de suppression pour les associations `oneToMany` et `manyToMany`. Ces méthodes permettent de synchroniser les objets supprimés en mettant à jour la relation réciproque.

  _Valeur par défaut_: `false`

  **Exemple :**

  Pour une association `OneToMany` entre `Utilisateur` et `Commande`, si `associationRemovers: true`, une méthode `removeCommande(Commande commande)` sera générée dans la classe `Utilisateur`. Cette méthode retirera la commande de la liste et mettra à jour la référence réciproque (`commande.setUtilisateur(null)`).

- `generatedHint`

  Option pour générer l'annotation @Generated("TopModel : <https://github.com/klee-contrib/topmodel>")

  _Valeur par défaut_: `true`

- `persistenceMode`

  Mode de génération de la persistence (`"javax"` ou `"jakarta"`). Par défaut, `javax` est utilisé pour la compatibilité avec Spring Boot 2.x, et `jakarta` pour Spring Boot 3.x.

  _Valeur par défaut_: `javax`

  _Variables par tag_: **non**

- `mappersInClass`

  Indique s'il faut ajouter les mappers en tant que méthode (`to...`) ou constructeur dans les classes qui les déclarent. Si `true`, les mappers `from` sont générés comme constructeurs et les mappers `to` comme méthodes dans les classes concernées.

  _Valeur par défaut_: `true`

- `identity`

  Options de génération de la séquence

  - `mode`

    Mode de génération de la séquence. Les valeurs possibles sont :

    - `"none"` : Aucune génération automatique
    - `"sequence"` : Utilise une séquence de base de données (nécessite `increment` et optionnellement `start`)
    - `"identity"` : Utilise l'auto-incrémentation de la base de données (par défaut)
    - `"uuid"` : Génère un UUID pour la clé primaire

    _Valeur par défaut_: `identity`

  - `increment`

    Incrément de la séquence générée.

  - `start`

    Début de la séquence générée.

- `metaModel`

  Option pour générer le métamodèle JPA.

  _Valeur par défaut_: `false`

  Le métamodèle est une représentation typée et statique des entités, leurs attributs et relations. Il permet notamment de faciliter l'utilisation des Criteria Builder en évitant l'utilisation de chaînes de caractères pour spécifier des entités et leurs propriétés.

  Lorsque cette option est activée, une classe de métamodèle est générée pour chaque entité persistée. Ces classes suivent la convention de nommage JPA : `[NomEntité]_` (avec un underscore suffixe).

  **Exemple d'utilisation :**

  ```java
  // Au lieu d'utiliser des chaînes de caractères
  CriteriaBuilder cb = em.getCriteriaBuilder();
  CriteriaQuery<Utilisateur> query = cb.createQuery(Utilisateur.class);
  Root<Utilisateur> root = query.from(Utilisateur.class);
  query.where(cb.equal(root.get("nom"), "Dupont")); // ❌ Risque d'erreur de typo

  // Avec le métamodèle (type-safe)
  query.where(cb.equal(root.get(Utilisateur_.nom), "Dupont")); // ✅ Vérifié à la compilation
  ```

  **Documentation :**

  - Spec JPA (Voir le chapitre 5): <https://download.oracle.com/otndocs/jcp/persistence-2.0-fr-eval-oth-JSpec/>
  - Exemple d'utilisation: <https://www.baeldung.com/hibernate-criteria-queries-metamodel>

  > **Note :** Le métamodèle est généré uniquement pour les entités persistées (pas pour les DTOs).

- `useJdbc`

  Génération en mode JDBC au lieu de JPA. Dans ce mode, les entités sont générées sans annotations JPA, mais avec des annotations JDBC simples. Les DAOs héritent de `CrudRepository` au lieu de `JpaRepository`.

  _Valeur par défaut_: `false`

  **Note :** En mode JDBC, les enums ne sont pas supportés de la même manière qu'en mode JPA. Les classes avec des valeurs ne peuvent pas utiliser le mode enum. Les DAOs héritent de `CrudRepository` au lieu de `JpaRepository`.

- `dbSchema`

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

- `dataFlowsPath`

  Localisation des flux de données générés. Cette variable doit être renseignée pour que les flux soient générés.

  Le chemin des fichiers cibles sera calculé en remplaçant les `.` et le `:` par des `/` dans cette valeur, tandis que le nom du package des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

  _Templating_: `{app}`, `{module}`

  _Variables par tag_: **oui** (plusieurs flux de données pourraient être générés si un fichier a plusieurs tags)

- `dataFlowsWriter`

  Writer à utiliser pour les flux de données. Les valeurs possibles sont :

  - `jpa` : Utilise le `JpaItemWriter` de spring-batch (par défaut). Adapté pour des volumes de données modérés et offre une meilleure compatibilité avec les fonctionnalités JPA (cascades, listeners, etc.)
  - `bulk` : Utilise le `PgBulkWriter` de la librairie `spring-batch-bulk` pour des performances optimales. Recommandé pour traiter de très gros volumes de données grâce à l'utilisation du bulk insert PostgreSQL (avec la commande `COPY`)

  _Valeur par défaut_: `jpa`

- `dataFlowsBulkSize`

  Taille des chunks à extraire et insérer lors de l'utilisation du mode `bulk` pour les flux de données. Cette valeur détermine le nombre d'enregistrements traités par batch lors des opérations d'insertion en masse.

  _Valeur par défaut_: `100000`

- `dataFlowsListeners`

  Liste des listeners à ajouter aux dataflows. Ces listeners seront appelés aux différents hooks du flow (beforeFlow, afterFlow, etc.).

  _Valeur par défaut_: `[]`

  **Exemple :**

  ```yaml
  jpa:
    - tags:
        - entity
    dataFlowsPath: topmodel/exemple/flows
    dataFlowsListeners:
      - topmodel.exemple.listeners.CustomFlowListener
      - topmodel.exemple.listeners.AnotherListener
  ```

- `apisName`

  Nom des classes d'API générées. Permet de personnaliser le nom des interfaces/classes d'API.

  _Templating_: `{fileName}` (remplacé par le nom du fichier en PascalCase)

  _Valeur par défaut_: Dépend du type d'API générée (par exemple, `{fileName}Api` pour les clients, `{fileName}Controller` pour les serveurs)

  **Exemple :**

  ```yaml
  jpa:
    - tags:
        - api
    apiGeneration: Server
    apisName: "{fileName}Service"  # Génère UtilisateurService au lieu de UtilisateurController
  ```

### Exemple

Voici un exemple de configuration du générateur JPA

```yaml
jpa:
  - tags:
      - dto
      - entity
    outputDirectory: ./jpa/src/main/javagen  # Dossier cible de la génération
    entitiesPath: topmodel/exemple/name/entities  # Dossier cible des entités persistées
    daosPath: topmodel/exemple/name/daos  # Dossier cible des DAO
    dtosPath: topmodel/exemple/name/dtos  # Dossier cible des objets non persistés
    enumsPath: topmodel/exemple/name/enums  # Dossier cible des enums
    apiPath: topmodel/exemple/name/api  # Dossier cible des API
    apiGeneration: Server  # Mode de génération de l'API (Client ou Server)
    fieldsEnum: ["persisted"]  # Classes dans lesquelles le générateur doit ajouter une enum des champs
    fieldsEnumInterface: topmodel.exemple.utils.IFieldEnum<>  # Interface dont doivent hériter ces enums
    persistenceMode: jakarta  # Mode de persistence (javax ou jakarta)
    identity:
      mode: sequence
      increment: 50
      start: 1000
```

## Snippets

### Domains

```yaml
---
domain:
  name: ID
  label: ID technique
  autoGeneratedValue: true
  asDomains:
    list: LIST
  java:
    type: Long
---
domain:
  name: MAIL
  asDomains:
    list: LIST
  label: Mail
  length: 100
  java:
    type: String
    annotations:
      - text: '@Email(message = "Le mail ''${validatedValue}'' n''est pas valide")'
        imports:
          - "jakarta.validation.constraints.Email"
---
domain:
  name: DATE_TIME
  label: Date
  asDomains:
    list: LIST
  java:
    type: LocalDateTime
    imports:
      - java.time.LocalDateTime
---
domain:
  name: TIME
  label: Heure
  asDomains:
    list: LIST
  java:
    type: LocalTime
    imports:
      - java.time.LocalTime
---
domain:
  name: DATE
  label: Date
  asDomains:
    list: LIST
  java:
    type: LocalDate
    imports:
      - java.time.LocalDate
---
domain:
  name: DATE_PAST
  label: Date
  asDomains:
    list: LIST
  java:
    type: LocalDate
    imports:
      - java.time.LocalDate
    annotations:
      - text: "@Past"
        imports:
          - "jakarta.validation.constraints.Past"
---
domain:
  name: DATE_CREATION
  label: Date
  asDomains:
    list: LIST
  java:
    type: LocalDate
    imports:
      - java.time.LocalDate
    annotations:
      - text: "@CreatedDate"
        imports:
          - org.springframework.data.annotation.CreatedDate
        target: Persisted
      - text: "@PastOrPresent"
        imports:
          - "jakarta.validation.constraints.PastOrPresent"
---
domain:
  name: DATE_MODIFICATION
  label: Date
  asDomains:
    list: LIST
  java:
    type: LocalDate
    imports:
      - java.time.LocalDate
    annotations:
      - text: "@LastModifiedDate"
        imports:
          - org.springframework.data.annotation.LastModifiedDate
        target: Persisted
      - text: "@PastOrPresent"
        imports:
          - "jakarta.validation.constraints.PastOrPresent"
---
domain:
  name: CREE_PAR
  label: Créé par
  scale: 50
  asDomains:
    list: LIST
  java:
    type: String
    annotations:
      - text: "@CreatedBy"
        imports:
          - org.springframework.data.annotation.CreatedBy
        target: Persisted
---
domain:
  name: MODIFIE_PAR
  label: Modifié par
  scale: 50
  asDomains:
    list: LIST
  java:
    type: String
    annotations:
      - text: "@LastModifiedBy"
        imports:
          - org.springframework.data.annotation.LastModifiedBy
        target: Persisted
---
domain:
  name: FILE_FORM
  mediaType: "multipart/form-data"
  label: Fichier
  bodyParam: true
  java:
    type: MultipartFile
    imports:
      - "org.springframework.web.multipart.MultipartFile"
---
domain:
  name: FILE
  mediaType: "multipart/form-data"
  label: Fichier
  bodyParam: true
  java:
    type: File
    imports:
      - "java.io.File"
---
domain:
  name: RESPONSE_ENTITY
  label: Response Entity
  parameters:
    - name: type
      required: true
      comment: Type de réponse
    - name: import
      required: true
      comment: Import pour le type de la réponse
  java:
    type: ResponseEntity<{type}>
    imports:
      - org.springframework.http.ResponseEntity
      - "{import}"
---
domain:
  name: LIST
  label: Liste
  java:
    type: List<String>
    genericType: List<{T}>
    imports:
      - java.util.List
---
domain:
  name: POINT
  label: Point
  java:
    type: Point
    imports:
      - org.locationtech.jts.geom.Point
---
domain:
  name: POLYGONE
  label: Polygone
  java:
    type: Polygon
    imports:
      - org.locationtech.jts.geom.Polygon
---
domain:
  name: PAGE
  label: Date
  java:
    type: Page
    genericType: Page<{T}>
    imports:
      - "org.springframework.data.domain.Page"
---
domain:
  name: HTTP_RESPONSE
  label: Réponse Http
  java:
    type: ResponseEntity<Void>
    imports:
      - org.springframework.http.ResponseEntity
```

### Décorateurs

```yaml
---
decorator:
  name: DateCreation
  description: Entity Listener pour suivre les évènements de création
  java:
    annotations:
      - EntityListeners(AuditingEntityListener.class)
    imports:
      - org.springframework.data.jpa.domain.support.AuditingEntityListener
      - jakarta.persistence.EntityListeners
  properties:
    - name: DateCreation
      comment: Date de création de l'objet
      required: true
      domain: DATE_CREATION
      label: Date de création
---
decorator:
  name: DateModification
  description: Entity Listener pour suivre les évènements de modification
  java:
    annotations:
      - EntityListeners(AuditingEntityListener.class)
    imports:
      - org.springframework.data.jpa.domain.support.AuditingEntityListener
      - jakarta.persistence.EntityListeners
  properties:
    - name: DateModification
      comment: Date de création de l'objet
      required: true
      domain: DATE_MODIFICATION
      label: Date de modification
---
decorator:
  name: CreePar
  description: Entity Listener pour suivre les évènements de création
  java:
    annotations:
      - EntityListeners(AuditingEntityListener.class)
    imports:
      - org.springframework.data.jpa.domain.support.AuditingEntityListener
      - jakarta.persistence.EntityListeners
  properties:
    - name: CreePar
      comment: Auteur de la création de l'objet
      required: true
      domain: CREE_PAR
      label: Créateur
---
decorator:
  name: ModifiePar
  description: Entity Listener pour suivre les évènements de création
  java:
    annotations:
      - EntityListeners(AuditingEntityListener.class)
    imports:
      - org.springframework.data.jpa.domain.support.AuditingEntityListener
      - jakarta.persistence.EntityListeners
  properties:
    - name: ModifiePar
      comment: Auteur de la création de l'objet
      required: true
      domain: MODIFIE_PAR
      label: Créateur
---
decorator:
  name: HasAuthority
  description: Droit nécessaire pour pouvoir accéder au endpoint
  parameters:
    - name: authority
      required: true
      comment: Autorité a passer à `PreAuthorize`.
  java:
    annotations:
      - '@PreAuthorize("hasAuthority(''{authority}'')")'
    imports:
      - org.springframework.security.access.prepost.PreAuthorize
```

### Configuration maven

Pour ajouter les sources du dossier `javagen` au build, vous pouvez utiliser la configuration suivante :

```xml
            <!-- Ajout des sources générées -->
            <plugin>
                <groupId>org.codehaus.mojo</groupId>
                <artifactId>build-helper-maven-plugin</artifactId>
                <executions>
                    <execution>
                        <id>add-source</id>
                        <phase>generate-sources</phase>
                        <goals>
                            <goal>add-source</goal>
                        </goals>
                        <configuration>
                            <sources>
                                <source>src/main/javagen</source>
                            </sources>
                        </configuration>
                    </execution>
                </executions>
            </plugin>
```
