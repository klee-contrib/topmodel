# Génération des DAOs

| Nom         | Condition d'activation | Objets ciblés                                | Fichiers générés                                                  |
| ----------- | ---------------------- | -------------------------------------------- | ----------------------------------------------------------------- |
| `JpaDaoGen` | `daosPath` défini      | Classes persistées qui ne sont pas des enums | Interface Repository permettant de requêter la classe en question |

Un fichier d'interface DAO est généré pour chacune des classes persistées du modèle. Cette interface hérite de `JpaRepository` (ou `CrudRepository` en mode JDBC), et est paramétrée pour gérer l'entité correspondante.

**Ce fichier n'est généré qu'une seule fois !!** Vous pouvez donc le modifier pour ajouter les différentes méthodes d'accès dont vous auriez besoin. C'est tout l'intérêt.

Il est possible de modifier le comportement de ce générateur avec les configurations documentées ci-dessous.

## Configuration

### `daosPath`

Localisation des DAOs, relative au répertoire de génération.

Le chemin des fichiers cibles sera calculé en remplaçant les `.` et le `:` par des `/` dans cette valeur, tandis que le nom du package des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

_Templating_: `{app}`, `{module}`

_Variables par tag_: **oui** (plusieurs DAOs pourraient être générés si un fichier a plusieurs tags)

### `daosAbstract`

Génération des DAO sous forme 'Abstract' à hériter pour l'utiliser dans le projet avec :

- le nom `Abstract{classe.NamePascal}DAO`
- le fichier java sera mis à jour (écrasé) à chaque génération de code
- l'annotation `@NoRepositoryBean` ajoutée (`org.springframework.data.repository.NoRepositoryBean`) permettant de ne pas considérer cette interface comme un DAO
  - il faut donc créer une interface qui en hérite dans le projet
- le `daosPath` peut être dans un répertoire de type `javagen`

### `daosName`

Nom du DAO à générer. Placer la valeur `{class}` dans le nom pour remplacer par le nom de la classe. Par exemple, `daosName: "{class}Repository"` générera `UtilisateurRepository` pour la classe `Utilisateur`.

_Templating_: `{class}`

_Valeur par défaut_: `"{class}DAO"` si `daosAbstract` est à `false`, sinon `"Abstract{class}DAO"`

### `daosInterface`

Permet de surcharger les interfaces par défaut des DAOs :

- si `useJdbc`, l'interface est `org.springframework.data.repository.CrudRepository`
- si `reference: true`, l'interface est `org.springframework.data.repository.CrudRepository`
- si aucun des deux, l'interface est `org.springframework.data.jpa.repository.JpaRepository`
- si `daosInterface` est précisée, les autres cas ne sont pas utilisés.

Seul le nom de la classe est configurable, elle doit respecter le même pattern générique que `JpaRepository` et `CrudRepository` soit :

- La classe de l'entité en premier
- La classe de l'identifiant en second
- `{DaosInterface}<{classe.NamePascal}, {pk}>`
