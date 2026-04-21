# Générateur de traductions manquantes

Ce générateur produit, pour chaque langue cible, des fichiers `.properties` contenant uniquement les **libellés absents** du store de traductions. Il est particulièrement utile pour alimenter un outil de traduction externe (ou un humain) avec le strict minimum à traduire, sans écraser les traductions déjà présentes.

## Principe

Le générateur s'appuie sur les `label` des propriétés du modèle et sur les valeurs des values du modèle, puis les confronte aux traductions connues de TopModel (cf. [multilinguisme](/model/i18n)) afin d'identifier ce qui reste à traduire.

Pour une langue donnée, une entrée est produite lorsque :

- la propriété possède un `label` non déjà traduit dans cette langue ;
- ou la propriété appartient à une **liste de référence** (`reference: true`) qui définit une `defaultProperty` **et** dont au moins une des `values` n'a pas de traduction connue dans cette langue.

La langue de développement (celle déclarée comme `defaultLang` dans la configuration globale) est automatiquement ignorée : aucun fichier n'est généré pour elle puisqu'elle sert de source et n'a pas besoin d'être traduite.

## Configuration

```yaml
translation:
  - tags: # Tags pour lesquels on souhaite générer les traductions manquantes
      - dto
    outputDirectory: ./ # Racine du répertoire de génération
    rootPath: i18n/{lang}/out # Dossier dans lequel les fichiers seront générés
    langs: # Langues pour lesquelles on souhaite produire un fichier de traductions manquantes
      - en_US
      - de_DE
```

### `outputDirectory`

Racine du répertoire de génération, comme pour tous les autres générateurs.

### `rootPath`

Chemin relatif (à `outputDirectory`) dans lequel les fichiers sont déposés. Il supporte les variables `{lang}` et `{tag}`. Valeur par défaut : `{lang}`.

_Variables par tag_ : **oui**  
_Variables par langue_ : **oui**

### `langs`

Liste des langues de l'application. La langue par défaut (définie par `defaultLang` dans la configuration du modèle) est exclue automatiquement, inutile de la lister.

### `translateProperties` / `translateReferences`

Ces options héritées permettent de filtrer la génération :

- `translateProperties` : si `true`, les `label` de propriétés sont pris en compte ;
- `translateReferences` : si `true`, les valeurs des listes de référence sont prises en compte.

Si ces deux options sont à `false` (ou non définies), aucun fichier n'est généré.

## Clés de traduction

Les clés générées suivent les conventions standard de TopModel :

- pour un `label` de propriété : la `resourceKey` de la propriété (typiquement `{module}.{classe}.{propriété}`) ;
- pour une valeur de liste de référence : `{module}.{classe}.values.{clé d'unicité de la valeur}`.

## Fichiers générés

Pour chaque module contenant au moins une traduction manquante, un fichier est produit par langue cible :

```
{outputDirectory}/{rootPath}/{nom-du-module-kebab}_{lang}.properties
```

Le nom du module est transformé en _kebab-case_, avec un underscore comme séparateur de sous-modules (par exemple `ventes_commande_en_US.properties` pour le module `Ventes.Commande` en anglais).

À l'intérieur du fichier :

- les propriétés sont regroupées par classe, les classes étant triées par nom ;
- au sein d'une classe, les propriétés manquantes sont triées par nom, puis viennent les valeurs manquantes de liste de référence triées par clé ;
- chaque ligne est de la forme `cle=valeur`, où `valeur` correspond au `label` (pour une propriété) ou à la valeur de la `defaultProperty` (pour une valeur de liste de référence) — donc exprimée dans la **langue de développement**, à titre de référence pour le traducteur.

Seules les entrées **manquantes** sont écrites : les traductions déjà présentes dans le store ne sont jamais régénérées, ce qui rend le fichier idéal pour être confié à un traducteur.

## Exemple de sortie

Pour un module `restaurant` contenant une classe `Commande` (dont le label de `avisClient` n'est pas encore traduit en anglais) et une liste de référence `StatutCommande` (dont la valeur `Livree` n'est pas encore traduite), le fichier `restaurant_en_US.properties` ressemblera à :

```properties
restaurant.commande.avisClient=Avis du client
restaurant.statutCommande.values.Livree=Livrée
```

Une fois ces lignes traduites et intégrées au store de traductions, le générateur ne les reproduira plus lors des exécutions suivantes.
