# Domaines

Un domaine se définit comme un document YAML, dans un fichier de modèle.

Un domaine correspond à un type métier. Chaque champ doit avoir un domaine. Les règles de gestion liées à chaque domaine devront être implémentées dans chaque couche technique. En revanche, il faut par contre décrire ici, dans le modèle, comment chaque domaine va être représenté dans chaque langage, puisque la génération va en avoir besoin.

Un domaine se décrit donc de la façon suivante :

```yaml
---
domain:
  name: DO_ID
  label: Identifiant
  length: # Longueur du champ, si applicable.
  scale: # Nombre de décimales du champ, si applicable.
  csharp:
    type: int?
    annotations: # Liste d'annotations à devoir ajouter à toute propriété de ce domaine, si applicable.
    usings: # Liste de usings à devoir ajouter à la définition d'une classe qui utilise ce domaine, si applicable.
  ts:
    type: number
    import: # Chemin de l'import à ajouter pour utiliser le type, si applicable.
  java:
    type: Integer
    import: # Chemin de l'import à ajouter pour utiliser le type, si applicable.
  sqlType: int
```

Naturellement, il n'est pas nécessaire de spécifier les langages pour lesquels le domaine n'est pas utilisé (et c'est évidemment obligatoire sinon).

Il n'y a pas besoin de préciser les dépendances aux fichiers contenant des domaines dans `uses` : tous les domaines sont automatiquement accessibles dans tous les fichiers. En revanche, cela implique que tous les fichiers ont une dépendance implicite à tous les fichiers contenant des domaines, ce qui pourrait entraîner des dépendances circulaires entre fichiers (qui ne sont **pas** supportées) involontaires. Par conséquent, et également par soucis de clarté, **il est fortement conseillé de définir tous les domaines dans un unique fichier qui ne contient que ces définitions**.

Il est possible de définir le `mediaType` du domaine. Cette information pourra être prise en compte par certains générateurs (notamment les générateurs d'API).

Exemple :

```yaml
---
domain:
  name: DO_PDF
  mediaType: application/pdf
  label: File Response Entity
  ts:
    type: File
  java:
    type: ResponseEntity<Resource>
    imports:
      - org.springframework.http.ResponseEntity
      - org.springframework.core.io.Resource
```
