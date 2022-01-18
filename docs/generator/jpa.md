# Jpa Generator

## Configuration

Préconisations :

Il n'est pas recommandé d'utiliser le générateur Postgresql en combinaison avec le générateur JPA. En effet, celui-ci ne gère pas les relations `manyToMany` de la même manière. La seule génération des listes de références suffit, en y ajoutant la configuration du `generateddl` de spring :

Dans le fichier `application.yml` de votre application

```yml
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

Le générateur suppose que vous utilisez la librairie [lombok](https://projectlombok.org/), et la validation `ajax`

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

Lorsque sont ajoutées des listes de références, le générateur créé les `enum` correspondantes. Le domaine de clé primaire de la classe de référence est ignoré, et le champs prend le type de l'enum. Par défaut, l'enum s'appelle `[Nom de la table de ref]Code`

### DAO

Les DAO sont générés une seule fois pour chaque entité persistée (afin de ne pas perdre de code écrit)

### Objets non persistés (Dtos)

Les dtos sont le reflet du modèle persité, sans les annotations JPA. Pour les utiliser :

- Ne pas ajouter d'association (non sens dans un modèle non persisté)
- Ne pas composer avec une entité persitée

Ceci afin d'éviter de mélanger les objets persistés et non persistés. En effet, si votre objet est sérializé, Hibernate risque de charger tout l'arbre de l'objet correspondant

## Api

Le générateur créé des `interface` contenant, pour chaque endPoint paramétré, deux méthode :

- Une méthode par défaut `[NomDuEndPoint]Mapping`, portant l'annotation de mappping de la route, et qui appelle la méthode suivante
- Une méthode abstraite `Nom du endpoint`, méthode à implémenter dans votre controller.
