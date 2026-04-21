# Génération des DAOs

| Nom         | Condition d'activation | Objets ciblés                                | Fichiers générés                                                  |
| ----------- | ---------------------- | -------------------------------------------- | ----------------------------------------------------------------- |
| `JpaDaoGen` | `daosPath` défini      | Classes persistées qui ne sont pas des enums | Interface Repository permettant de requêter la classe en question |

Un fichier d'interface DAO est généré pour chacune des classes persistées du modèle. Cette interface hérite de `JpaRepository` (ou `CrudRepository` en mode JDBC), et est paramétrée par l'entité correspondante et le type de sa clé primaire.

**Ce fichier n'est généré qu'une seule fois !** Vous pouvez donc le modifier librement pour y ajouter les méthodes d'accès dont vous avez besoin : c'est tout l'intérêt. TopModel détecte la présence du fichier et n'écrase pas son contenu.

## Classes ciblées

Un DAO est généré pour chaque classe qui est à la fois :

- **persistée** (`IsPersistent`) ;
- **non marquée comme enum**, sauf si elle est déclarée en mode `class` sans `readonly: true` (une liste de référence écrite en base, avec des valeurs pré-remplies, obtient donc un DAO).

En mode `useJdbc`, les classes à **clé primaire composite** sont en plus exclues, car `CrudRepository` ne supporte pas nativement un identifiant multi-colonnes.

## Type de la clé primaire

Le second paramètre générique du Repository (le type de l'ID) dépend de la structure de la classe :

| Cas                                                          | Type utilisé                                |
| ------------------------------------------------------------ | ------------------------------------------- |
| Clé primaire simple                                          | Type de la propriété (ex. `Integer`, `UUID`) |
| Classe sans clé primaire directe, mais qui `extends` une autre | Type de la clé primaire héritée             |
| Clé primaire composite (plusieurs propriétés `primaryKey`)  | `{Classe}.{Classe}Id` (classe interne générée dans l'entité) |

## Interface héritée

L'interface étendue est déterminée dans cet ordre :

1. Si `daosInterface` est renseigné, c'est ce type qui est utilisé (et ce type uniquement, les deux autres règles sont ignorées).
2. Sinon, si `useJdbc: true` **ou** si la classe a `reference: true` : `CrudRepository`.
3. Sinon : `JpaRepository`.

## Exemples de sortie

**DAO standard, clé primaire `Integer`, interface par défaut :**

```java
public interface CommandeDAO extends JpaRepository<Commande, Integer> {
}
```

**DAO avec clé primaire composite** (classe de liaison `MenuPlat` pour un ManyToMany) :

```java
public interface MenuPlatDAO extends JpaRepository<MenuPlat, MenuPlat.MenuPlatId> {
}
```

**DAO avec `daosInterface` surchargé :**

```yaml
jpa:
  - daosInterface: topmodel.commun.CustomCrudRepository
```

```java
public interface CommandeDAO extends CustomCrudRepository<Commande, Integer> {
}
```

## Mode `daosAbstract`

Lorsque `daosAbstract: true` est activé, le fichier est **régénéré à chaque build** (il est donc à placer dans un dossier de type `javagen`). L'interface prend le préfixe `Abstract` et reçoit l'annotation `@NoRepositoryBean` : elle ne sera pas instanciée par Spring comme un repository direct.

```java
@NoRepositoryBean
public interface AbstractCommandeDAO extends JpaRepository<Commande, Integer> {
}
```

Il revient alors au projet de créer une interface concrète qui hérite de celle-ci, et qui peut être étendue avec des méthodes métier.

## Configuration

### `daosPath`

Localisation des DAOs, relative au répertoire de génération.

Le chemin des fichiers cibles est calculé en remplaçant les `.` et le `:` par des `/` dans cette valeur, tandis que le nom du package des classes générées est calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

_Templating_: `{app}`, `{module}`

_Variables par tag_: **oui** (plusieurs DAOs pourraient être générés si un fichier a plusieurs tags)

### `daosAbstract`

Génère les DAOs sous forme d'interfaces `Abstract` à hériter dans le projet :

- le nom devient `Abstract{classe.NamePascal}DAO` ;
- le fichier Java est réécrit à chaque génération (pas de préservation) ;
- l'annotation `@NoRepositoryBean` (`org.springframework.data.repository.NoRepositoryBean`) est ajoutée pour que Spring ne traite pas cette interface comme un repository direct ;
- il faut donc créer une interface qui en hérite dans le projet ;
- le `daosPath` peut être placé dans un répertoire de type `javagen`.

### `daosName`

Nom du DAO à générer. Placer la valeur `{class}` dans le nom pour remplacer par le nom de la classe. Par exemple, `daosName: "{class}Repository"` générera `UtilisateurRepository` pour la classe `Utilisateur`.

_Templating_: `{class}`

_Valeur par défaut_: `"{class}DAO"` si `daosAbstract` est à `false`, sinon `"Abstract{class}DAO"`

### `daosInterface`

Permet de surcharger l'interface par défaut des DAOs. La valeur attendue est un nom de classe pleinement qualifié :

```yaml
jpa:
  - daosInterface: topmodel.commun.CustomCrudRepository
```

Règles :

- si `useJdbc: true`, l'interface par défaut est `org.springframework.data.repository.CrudRepository` ;
- si la classe a `reference: true`, l'interface par défaut est aussi `org.springframework.data.repository.CrudRepository` ;
- sinon, l'interface par défaut est `org.springframework.data.jpa.repository.JpaRepository` ;
- **si `daosInterface` est défini, il remplace ces valeurs par défaut dans tous les cas**.

Seul le nom de la classe est configurable : elle doit respecter le même pattern générique que `JpaRepository` et `CrudRepository`, soit :

- la classe de l'entité en premier ;
- la classe de l'identifiant en second ;
- `{DaosInterface}<{classe.NamePascal}, {pk}>`.
