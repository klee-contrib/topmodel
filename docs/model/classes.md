# Classes

Une classe se définit comme un document YAML, dans un fichier de modèle.

Une classe se définit de la façon suivante :

```yaml
class:
  name: # Nom de la classe
  pluralName: # Nom de la classe au pluriel, si différent de "name" avec un "s"
  sqlName: # Nom de la table SQL associée, si différent de "name" ou "pluralName" (selon configuration) en SNAKE_CASE
  label: # Libellé de la classe
  comment: # Description de la calasse
  # Autres propriétés comme "trigram" (si persistant), "defaultProperty", "reference" pour les listes de références...

  properties: # Les propriétés de la classes, voir section "Propriétés"
```

Une fois les [propriétés](/model/properties.md) définies, il est possible de compléter la définition de la classe par :

- `unique`, qui est une liste de clés unique à créer sur la classe (en base de données à priori). Une clé unique se définit via la liste des propriétés qui composent la clé unique (qui peut donc contenir une ou plusieurs propriétés)
- `values`, qui est un objet qui contient des valeurs "statiques" pour une classe que l'on veut avoir toujours disponible partout (en BDD, côté serveur, côté client). Il se définit comme une map dont les valeurs sont un objet "JSON" qui doit définir au moins toutes les propriétés obligatoires de la classe.

Exemple d'utilisation de ces deux propriétés :

```yaml
unique:
  - [Libelle]
values:
  Valeur1: { Code: 1, Libelle: Valeur 1 }
  Valeur2: { Code: 2, Libelle: Valeur 2 }
```
