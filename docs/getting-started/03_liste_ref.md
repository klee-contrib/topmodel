# Définition d'une liste de référence

Dans notre modèle de données, nous souhaitons définir des **listes de références**. Ce sont des classes dont les instances changent peu ou pas du tout, et qui peuvent donc être mises en cache.

Définissons donc dans notre module `Users` la classe `TypeUtilisateur`. Pour indiquer qu'il s'agit d'une liste de référence, il suffit de préciser l'attribut `reference`.
Ainsi :

```yaml
---
class:
  name: TypeUtilisateur
  comment: Type d'utilisateur
  reference: true # Indique que cette classe est une classe de référentiel
  trigram: TUT
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

La classe complète se déclare donc de la manière suivante :

```yaml
---
class:
  name: TypeUtilisateur
  comment: Type d'utilisateur
  reference: true
  trigram: TUT
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
