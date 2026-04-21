# Génération des resources

| Nom              | Condition d'activation | Objets ciblés                                                                | Fichiers générés                                                                                                                                                                                                                                                           |
| ---------------- | ---------------------- | ---------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `JpaResourceGen` | `resourcesPath` défini | Classes qui contiennent des labels ou des values qui ont des defaultProperty | Fichiers de resource `.properties` dans les différentes langues de l'application. Les clés sont les clés de traduction des labels des propriétés du modèle, et dont les valeurs sont les labels définis dans le modèle dans la langue de développement, ou leur traduction |

Le générateur de resources s'appuie sur les `label` des propriétés du modèle, ainsi que sur les traductions récupérées via la configuration du [multilinguisme](/model/i18n).

## Classes ciblées

Un fichier `.properties` est alimenté, pour un module donné, dès qu'au moins une propriété rencontrée remplit l'une des conditions suivantes :

- la propriété possède un `label` (génère une clé de label de propriété) ;
- ou la propriété appartient à une **liste de référence** (`reference: true`) qui possède une `defaultProperty` et des `values` (génère une clé par valeur de liste).

Les classes qui ne remplissent aucune de ces deux conditions ne produisent pas d'entrée.

## Fichiers générés

Pour chaque module, un fichier est généré par langue configurée :

- `nom-du-module.properties` pour la langue de développement (celle configurée par défaut) ;
- `nom-du-module_en_US.properties`, `nom-du-module_de_DE.properties`, etc. pour les traductions.

Le nom du fichier reprend le nom du module en kebab-case. Le chemin de sortie est calculé à partir de `resourcesPath`, qui supporte les variables `{module}` et `{lang}`.

## Configuration minimale

```yaml
jpa:
  - tags:
      - dto
    resourcesPath: resources/i18n/model
```

Avec cette configuration, pour chaque module, TopModel génère un fichier de resources par langue configurée globalement.

## Encodage

Par défaut, les fichiers sont générés avec l'encodage `Latin1`, conforme à la convention historique des `.properties` Java. L'encodage peut être basculé en `UTF8` si besoin :

```yaml
jpa:
  - tags:
      - dto
    resourcesPath: resources/i18n/model
    resourcesEncoding: UTF8
```

## Exemple de sortie

Pour un module `restaurant` contenant une classe `Commande` avec des propriétés labellisées, et une liste de référence `StatutCommande`, le fichier généré ressemble à :

```properties
restaurant.commande.avisClient=Avis du client
restaurant.commande.dateCommande=Date de la commande
restaurant.commande.montantTotal=Montant total
restaurant.statutCommande.values.EnCours=En cours
restaurant.statutCommande.values.Livree=Livrée
```

Le fichier traduit `restaurant_en_US.properties` contiendra les mêmes clés, avec les valeurs traduites récupérées via le store de traduction.

## Interaction avec `translateProperties` et `translateReferences`

Ces deux options sont définies au niveau du module (voir [Configuration globale](/configuration)) :

- `translateProperties` (par défaut `true`) : si désactivé, les clés de `label` des propriétés ne sont pas générées.
- `translateReferences` (par défaut `true`) : si désactivé, les clés des valeurs des listes de référence ne sont pas générées.

Ces options permettent, par exemple, de générer uniquement les traductions des listes de référence côté back, sans dupliquer les labels déjà traduits côté front.

## Configuration

### `resourcesPath`

Localisation des ressources, relative au répertoire de génération.

_Variables par tag_: **oui**

### `resourcesEncoding`

Encodage des fichiers de ressources. Les valeurs possibles sont :

- `Latin1` : valeur par défaut
- `UTF8`

_Variables par tag_: **non**
