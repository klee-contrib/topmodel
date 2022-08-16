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

## Api

Le générateur créé des `interface` contenant, pour chaque endPoint paramétré, la méthode abstraite `Nom du endpoint`, à implémenter dans votre controller.

Pour créer votre API, il suffit donc de créer un nouveau controller qui implémente la classe générée. L'annotation `@RestController` reste nécessaire.
