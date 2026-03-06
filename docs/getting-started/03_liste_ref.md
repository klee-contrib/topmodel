# Définition d'une liste de référence

Dans notre modèle de données, nous souhaitons définir des **listes de références**. Ce sont des classes dont les instances changent peu ou pas du tout, et qui peuvent donc être mises en cache.

Ainsi, dans un nouveau fichier `"References.tmd"` on défini la classe `TypeUtilisateur` dans le module `Ref`. Pour indiquer qu'il s'agit d'une liste de référence, il suffit de préciser l'attribut `reference`. :

```yaml
# References.tmd
---
module : Refs
tags : []
---
class:
  name: TypeUtilisateur
  comment: Type d'utilisateur
  reference: true # Indique que cette classe est une classe de référentiel
  properties:
    - name: Code
      comment: Code du type d'utilisateur
      primaryKey: true
      required: true
      domain: DO_CODE

    - name: Libelle
      comment: Libellé du type d'utilisateur
      primaryKey: false
      required: true
      domain: DO_LIBELLE
```

Par ailleurs, nous connaissons tous les `TypeUtilisateur` pouvant exister dans l'application. Nous pouvons donc les renseigner dans l'attribut `values`, sous la forme d'un **dictionnaire** contenant des objets **JSON**, déclarant les différentes propriétés de la classe.

```yaml
  values:
    ADM: { Code: ADM, Libelle: Administrateur }
    GES: { Code: GES, Libelle: Gestionnaire }
    CLI: { Code: CLI, Libelle: Client }
```

On peut rajouter ces éléments à notre fichier `"References.tmd"`. La classe complète se déclare donc de la manière suivante :

```yaml
# References.tmd
---
module : Refs
tags : []
---
class:
  name: TypeUtilisateur
  comment: Type d'utilisateur
  reference: true
  properties:
    - name: Code
      comment: Code du type d'utilisateur
      primaryKey: true
      required: true
      domain: DO_CODE

    - name: Libelle
      comment: Libellé du type d'utilisateur
      primaryKey: false
      required: true
      domain: DO_LIBELLE

  values:
    ADM: { Code: ADM, Libelle: Administrateur }
    GES: { Code: GES, Libelle: Gestionnaire }
    CLI: { Code: CLI, Libelle: Client }
```

> Il existe plusieurs mode de génération pour ce type de classe. Plus d'informations dans la documentation des [classes](/model/classes.md)

## Répertoire Projet

A ce stade du tutoriel, notre répertoire "Projet" devrait contenir les fichiers suivants:

- Projet
  - topmodel.config
  - Utilisateur.tmd
  - Domains.tmd
  - References.tmd

## Exemple de code généré

<!-- tabs:start -->
#### **Java**

```java
package tuto.enums.refs;

/**
 * Enumération des valeurs possibles de la propriété Code de la classe TypeUtilisateur.
 */
public enum TypeUtilisateurCode {
 /**
  * Administrateur.
  */
 ADM,
 /**
  * Client.
  */
 CLI,
 /**
  * Gestionnaire.
  */
 GES
}

```

```java

package tuto.entities.refs;

/**
 * Type d'utilisateur.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "TYPE_UTILISATEUR")
@Immutable
@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)
public class TypeUtilisateur {

  // [...]
 /**
  * Code du type d'utilisateur.
  */
 @Id
 @Column(name = "CODE", nullable = false, length = 10, columnDefinition = "varchar")
 @Enumerated(EnumType.STRING)
 private TypeUtilisateurCode code;

 // Libellé du type d'utilisateur.
 @Column(name = "LIBELLE", nullable = false, length = 100, columnDefinition = "varchar")
 private String libelle;

  // ...
}

```

#### **C#**

```csharp
namespace Tuto.Refs.Models;

/// <summary>
/// Type d'utilisateur.
/// </summary>
[Reference(true)]
[DefaultProperty(nameof(Libelle))]
[Table("type_utilisateur")]
public partial record TypeUtilisateur
{
    /// <summary>
    /// Valeurs possibles de la liste de référence TypeUtilisateur.
    /// </summary>
    public enum Codes
    {
        /// <summary>
        /// Administrateur.
        /// </summary>
        ADM,

        /// <summary>
        /// Client.
        /// </summary>
        CLI,

        /// <summary>
        /// Gestionnaire.
        /// </summary>
        GES
    }

/// ...
}

```

#### **SQL**

```sql
create table TYPE_UTILISATEUR (
 CODE varchar(10) not null,
 LIBELLE varchar(100) not null,
 constraint PK_TYPE_UTILISATEUR primary key (CODE)
);

INSERT INTO TYPE_UTILISATEUR(CODE, LIBELLE) VALUES('ADM', 'refs.typeUtilisateur.values.ADM');
INSERT INTO TYPE_UTILISATEUR(CODE, LIBELLE) VALUES('GES', 'refs.typeUtilisateur.values.GES');
INSERT INTO TYPE_UTILISATEUR(CODE, LIBELLE) VALUES('CLI', 'refs.typeUtilisateur.values.CLI');
```

<!-- tabs:end -->