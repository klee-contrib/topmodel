# Endpoints

En plus de pouvoir définir des classes dans le modèle, TopModel permet aussi de définir des endpoints d'API. Cela permet de compléter l'approche "model-first" en pouvant décrire des APIs qui référencent explicitement des classes et des propriétés du modèle. Les générateurs pourront derrière utiliser ces descriptions pour générer des clients d'API, ou bien une "interface" pour les contrôleurs côté serveur.

Un exemple de endpoint :

```yaml
---
endpoint:
  name: GetEvenementAudit
  method: GET
  route: api/evenements-audit/{evaId}
  description: Charge le détail d'un événement d'audit.

  # Liste des paramètres du endpoint. Ce sont des propriétés de modèle comme pour les classes. Il peut ne pas y en avoir.
  params:
    - alias:
        property: Id
        class: EvenementAudit

  # Type de retour du endpoint. La aussi, c'est une propriété du modèle et il peut ne pas y en avoir.
  returns:
    composition: EvenementAuditDetail
    name: detail # Le nom est obligatoire car c'est une propriété mais il n'est pas utilisé.
    kind: object
    comment: Le détail et la liste des impacts.
```

Le type de paramètre (body, query, route) est automatiquement déterminé :

- Si le paramètre est référencé dans la route, alors il est dans la route.
  - Et si c'est un alias de clé primaire et que la classe défini un trigramme, alors le nom du paramètre sera `{trigramme}{propriété}` au lieu de `{classe}{propriété}`
  - Il sera forcément obligatoire, quelque soit la valeur de `required` pour la propriété.
- Si le paramètre est une composition, ou si le domaine de la propriété spécifie `bodyParam: true`, alors il sera dans le body.
  - Il ne peut y avoir qu'un seul paramètre dans le body.
- Sinon, il sera dans la query
  - Il sera forcément facultatif, quelque soit la valeur de `required` pour la propriété.

Tous les générateurs vont générer **un fichier client ou serveur par fichier de modèle qui contient des endpoints**, qui reflétera le chemin et le nom du fichier de modèle en question. A l'inverse des générateurs de classes qui vont utiliser le module, ici il n'est pas important.

Il est possible de paramétrer le nom du fichier généré, ainsi que d'ajouter un préfix aux routes. Pour cela, dans les méta-data du fichier (au niveau de `module`,`tags`, `uses`...), vous pouvez ajouter des options :

```yaml
---
module: Securite.Utilisateur
tags:
  - dto
uses:
  - Securite/Utilisateur/02_Entities
  - Securite/Utilisateur/03_Dtos
options:
  endpoints:
    prefix: utilisateur
    fileName: UtilisateurApi
```

Ainsi, toutes les routes décrites dans ce fichier auront le préfix `utilisateur`. Le fichier généré se nommera `UtilisateurApi` (en fonction des spécificité des générateurs).
