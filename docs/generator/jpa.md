# Jpa Generator

## Configuration

Préconisations :

Il n'est pas recommandé d'utiliser le générateur Postgresql en combinaison avec le générateur JPA. En effet, celui-ci ne gère pas les relations `manyToMany` de la même manière. La seule génération des listes de références suffit, en y ajoutant la configuration du `generateddl` de spring :

Dans le fichier `application.yml` de votre application

```yaml
spring:
  jpa:
    properties:
      javax:
        persistence:
          schema-generation:
            create-source: metadata
            scripts:
              action: create
              create-target: create.sql
```

## Model

Le générateur JPA construit les entités persistées, les entités non persistés, et initialise les DAO.

### Entities

Les entités sont générées contenant les annotations JPA représentant le modèle de données.

A savoir :

- Si l'annotation `@createdDate` ou `@updatedDate` est ajoutée à un champs, alors l'annotation `@EntityListeners(AuditingEntityListener.class)` sera automatiquement ajoutée à la classe
- Il n'est actuellement pas possible de créer des liaisons bi-directionnelles
- Les compositions ne sont pas gérées dans le modèle persisté (n'utiliser que les associations)

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

Lorsque sont ajoutées des listes de références, le générateur créé les `enum` correspondantes. Le domaine de clé primaire de la classe de référence est ignoré, et le champs prend le type de l'enum. L'enum s'appelle `Values`. Les différents champs renseignés dans les valeurs sont également ajoutés en tant que propriété de l'enum.

Si la configuration `enumShortcutMode` est activée :

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

### Constructeurs par alias

Dans les classes contenant des alias, un constructeur par alias est généré. Celui-ci prend en entrée les classes référencées par des alias, et affecte les propriétés correspondantes.

A savoir :

- Si plusieurs alias ont la même classe, le même suffix et le même préfix (y compris dans la classe parente si elle existe), alors le constructeur par alias prendra le même argument pour ces propriétés

Exemple d'utilisation pour initialiser des Dto dans des repository Hibernate :

Si le constructeur par alias n'a qu'un seul argument, alors il est possible d'écrire :

```java
public interface ProfilDAO extends PagingAndSortingRepository<Profil, Long> {
 
 /**
  * Récupération du profil par son Id, mapping automatique avec le constructeur par alias
  *
  * @param profilId identifiant du profil à récupérer
  * @return profilDto le profil mappé en profilDto
  */
  ProfilDto findById(Long profilId)
}
```

En effet, Hibernate est capable de détecter automatiquement que l'objet de type `Profil` peut être mappé en `ProfilDto` grâce au constructeur par alias.

En revanche, lorsque le constructeur par alias prend plusieurs arguments, le mapping ne peut pas être fait automatiquement. Dans ce cas, il est recommandé d'utiliser le constructeur par alias dans un second temps, évenutellement en utilisant l'annotation `@EntityGraph` pour optimiser les performances.

```java

// UtilisateurDAO.java

public interface UtilisateurDAO extends PagingAndSortingRepository<Utilisateur, Long> {

  /**
  * Récupération de la liste des utilisateurs par filtrés par leur type
  *
  * @param typeUtilisateur type d'utilisateur
  * @return la liste des utilisateurs
  */
  @EntityGraph(attributePaths = { "profil" })
  List<Utilisateur> findByTypeUtilisateur(TypeUtilisateur typeUtilisateur);
}
```

```java

// UtilisateurService.java
@Service
public class UtilisateurService {

  @Autowired
  private final UtilisateurDAO utilisateurDAO;

  /**
  * Récupération de la liste des utilisateurs par filtrés par leur type
  *
  * @param typeUtilisateur type d'utilisateur
  * @return la liste des utilisateurs mappés en UtilisateurDto
  */
  List<UtilisateurDto> findByTypeUtilisateur(TypeUtilisateur typeUtilisateur){
    return utilisateurDAO.findByTypeUtilisateur(typeUtilisateur).stream().map(uti -> new UtilisateurDto(uti, uti.getProfil()));
  }
}
```

> D'un point de vue performance, il est important de souligner qu'il vaut mieux éviter de créer un dto à l'aide de plusieurs requêtes puis du constructeur par alias.

## Api

Le générateur créé des `interface` contenant, pour chaque endPoint paramétré, deux méthode :

- Une méthode par défaut `[NomDuEndPoint]Mapping`, portant l'annotation de mappping de la route, et qui appelle la méthode suivante
- Une méthode abstraite `Nom du endpoint`, méthode à implémenter dans votre controller.
