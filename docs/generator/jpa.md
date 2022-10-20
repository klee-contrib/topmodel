# Jpa Generator

## Configuration

Voici un exemple de configuration du générateur JPA

```yaml
jpa:
  - tags:
      - dto
      - entity
    modelOutputDirectory: ./jpa/src/main/javagen # Dossier cible de la génération
    daosPackageName: topmodel.exemple.name.daos # Package des DAO
    dtosPackageName: topmodel.exemple.name.dtos # Package des objets non persistés
    entitiesPackageName: topmodel.exemple.name.entities # Package des objets non persistés
    apiOutputDirectory: ./jpa/src/main/javagen # Dossier cible des API
    apiPackageName: topmodel.exemple.name.api # Package des l'API
    apiGeneration: Server # Mode de génération de l'API (serveur ou client)
    fieldsEnum: true # Si le générateur doit ajouter une enum des champs de la classe persistés
    fieldsEnumInterface: topmodel.exemple.utils.IFieldEnum<> # Classe dont doivent hériter ces enum
```

### Dépendances obligatoires

#### Modèle

Le modèle généré par TopModel dépend d'une api de persistence. Par défaut, c'est l'API de persistence `javax` qui est utilisée, mais le mode `jakarta` est aussi disponible.

La validation elle est gérée par le package `jakarta.validation-api`, dont les imports changent entre la version 2 et la version 3.

##### Javax (spring-boot < v3)

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

##### Jakarta (spring-boot > v3)

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

#### Endpoints

Actuellement, la seule génération de endpoint cliente et serveur qui est gérée passe par les API de `Spring-web`

```xml
<!-- https://mvnrepository.com/artifact/org.springframework/spring-web -->
<dependency>
    <groupId>org.springframework</groupId>
    <artifactId>spring-web</artifactId>
</dependency>
```

### Version Java

Le cde Java généré est compatible avec toutes les versions de Java postérieures à `Java 11`.

### Utilisation combinée avec le générateur postgresql

Le mode de génération par défaut des générateur ne créé par de séquence, mais des colonnes auto-générées avec `identity`. Malheureusement, le `batch insert` de jdbc ne fonctionne pas correctement avec ce mode de génération d'ID. Il est donc recommandé d'utiliser le mode `sequence` de du générateur postgresql.

Le mode `sequence` dans la configuration jpa et dans la configuration postgresql se déclare de la même manière :

```yaml
## Configuration jpa et proceduralSql
    identity:
      increment: 50
      start: 1000
      mode: sequence
```

## Model

Le générateur JPA construit les entités persistées, les entités non persistés, et initialise les DAO.

### Entities

Les entités sont générées contenant les annotations JPA représentant le modèle de données.

#### Associations

Dès lors qu'une association est faite entre deux classes, si celles-ci ont le même package racine et que la classe de destination n'est pas une liste de référence, alors l'association réciproque sera générée.

> Les classes **`securite`**.Profil et **`utilisateur`**.Utilisateur n'ont pas le même package racine, au contraire de **`utilisateur`**.Utilisateur et **`utilisateur`**.Utilisateur

##### ManyToMany

L'association `ManyToMany` réciproque est générée dans la classe de destination. L'association "propriétaire" de la relation est celle déclarée dans le modèle TopModel.

##### OneToMany

L'association `ManyToOne` réciproque est générée dans la classe de destination. L'association "propriétaire" de la relation est **toujours** l'association `ManyToOne`

##### ManyToOne

L'association `OneToMany` réciproque est générée dans la classe de destination. L'association "propriétaire" de la relation est **toujours** l'association `ManyToOne`

##### OneToOne

Pour des raisons de performances, les associations oneToOne réciproques ne sont pas générées.

#### Compositions

- Les compositions ne sont pas gérées dans le modèle persisté (n'utiliser que les associations)

#### FieldsEnum

Depuis la version 1.1.0, il est possible de générer dans la définition de la classe, la sous-classe (qui est une enum) `Fields`. Il s'agit d'une enumération des champs de la classe, au format const case.
Il faut pour cela ajouter la propriété `fieldsEnum: true` A la configuration JPA.

Il est également possible d'ajouter la référence d'une interface à cette configuration. Cette interface sera implémentée par la classe `Fields`. Vous pourrez ainsi la manipuler plus facilement. Si l'interface en question est suffixée par `<>`, alors elle sera considérée comme générique de la classe persistée.

Exemple :

La configuration suivante

```yaml
fieldsEnum: true
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

### References

Lorsque sont ajoutées des listes de références, le générateur créé les `enum` correspondantes. Le domaine de clé primaire de la classe de référence est ignoré, et le champs prend le type de l'enum. L'enum est générée à l'intérieur de la classe de référence, et s'appelle  `[Nom de la classe].Values`. Les différents champs renseignés dans les valeurs sont également ajoutés en tant que propriétés de l'enum.

Par ailleurs, si la classe possède une association avec une classe qui contient une liste de référence, alors il le type du champ dans l'enum sera le type de l'enum de la clé primaire de la classe associée.

#### EnumShortcutMode

Il peut être laborieux de toujours passer par la classe de référence lorsqu'on ne manipule le plus souvent que leurs clés primaires. CTopModel - JPA permet de créer des raccoucis pour rendre cette approche possible. Si la configuration `enumShortcutMode` est activée :

```yaml
enumShortcutMode: true
```

Alors les getters et setters des références statiques ne considèreront plus le type de la table de référence, mais uniquement celui de sa clé primaire.

Exemple :

```java
  private TypeUtilisateur.Values getTypeUtilisateurCode() {
    return this.typeUtilisateur.getCode();
  }

  private void setTypeUtilisateurCode(TypeUtilisateur.Values typeUtilisateurCode) {
    this.typeUtilisateur = new TypeUtilisateur(typeUtilisateurCode, typeUtilisateurCode.getLibelle());
  }
```

En effet, pour les références dont toutes les valeurs sont connues à l'avance et identifiées par un code, celui-ci est beaucoup plus utilisé dans le code java. L'existence de la table correspondante en base de donnée n'est utile que pour la création d'une contrainte de valeur sur les tables qui la référencent.

### DAO

Les DAO sont générés une seule fois pour chaque entité persistée (afin de ne pas perdre de code écrit)

### Objets non persistés (Dtos)

Les dtos sont le reflet du modèle persité, sans les annotations JPA. Pour les utiliser :

- Ne pas ajouter d'association (non sens dans un modèle non persisté)
- Ne pas composer avec une entité persitée

Ceci afin d'éviter de mélanger les objets persistés et non persistés. En effet, si votre objet est sérializé, Hibernate risque de charger tout l'arbre de l'objet correspondant

### Constructeurs

Le générateur JPA constuit 3 par classe :

- Constructeur vide
- Construteur tout argument
- Constructeur par recopie

### Interfaces et projections

Afin d'utiliser les [projections de Spring JPA](https://docs.spring.io/spring-data/jpa/docs/current/reference/html/#projections), il peut être nécessaire d'obtenir des interfaces représentant les Dtos (contenant uniquement les getters).

Pour générer de telles interface, vous pouvez passer la propriété `generateInterface` d'un décorateur `Java` à `true`.

```yaml
---
decorator:
  name: Interface
  description: Ajoute la génération de l'interface
  java:
    generateInterface: true
```

Ajouté à une classe, il permet de générer une interface, implémentée par la classe.

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

## Api Server

Le générateur créé des `interface` contenant, pour chaque endPoint paramétré, la méthode abstraite `Nom du endpoint`, à implémenter dans votre controller.

Pour créer votre API, il suffit donc de créer un nouveau controller qui implémente la classe générée. L'annotation `@RestController` reste nécessaire.

## Api Client

Le générateur créé des classes abstraites contenant, toutes les méthodes permettant d'accéder aux endpoints paramétrés.

Pour créer votre client d'API, il suffit de créer une classe qui hérite de cette classe abstraite. Pour fonctionner, elle devra appeler le constructeur de la classe abrstaite, en renseignant :

- Le host de l'API
- Une instance de `RestTemplate`

Exemple :

```java
@Service
public class UtilisateurApiClient extends AbstractUtilisateurApiCLient {

  private static final HOST = "http://localhost:8080/my-app/api/";

  @Autowired
  public UtilisateurApiClient(RestTemplate restTemplate) {
    super(restTemplate, HOST);
  }
}
```

Pour appeler l'API utilisateur, injecter le service UtilisateurApiClient. Puis appeler la méthode de votre choix en entrant les différents paramètres, en y ajoutant l'objet HttpHeaders désiré.

```java
@Service
public class UtilisateurService {

  private static final HOST = "http://localhost:8080/my-app/api/";

  private final UtilisateurApiClient utilisateurApiClient;

  @Autowired
  public UtilisateurService(UtilisateurApiClient utilisateurApiClient) {
    this.utilisateurApiClient = utilisateurApiClient;
  }

  public UtilisateurDto getUtilisateur(Long id){
    var headers = new HttpHeaders();
    headers.add("token-sécurisé", "MON_TOKEN_SECURISE");
    return utilisateurApiClient.getUtilisateur(id, headers);
  }
}
```
