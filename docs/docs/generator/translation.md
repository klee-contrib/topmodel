# Générateur de traductions manquantes

Ce générateur produit, pour chaque langue cible, des fichiers `.properties` contenant uniquement les **libellés absents** du store de traductions. Il est particulièrement utile pour alimenter un outil de traduction externe (ou un humain) avec le strict minimum à traduire, sans écraser les traductions déjà présentes.

## Principe

Le générateur s'appuie sur les `label` des propriétés du modèle et sur les valeurs des values du modèle, puis les confronte aux traductions connues de TopModel (cf. [multilinguisme](/model/i18n)) afin d'identifier ce qui reste à traduire.

Pour une langue donnée, une entrée est produite lorsque :

- la propriété possède un `label` non déjà traduit dans cette langue ;
- ou la propriété appartient à une **liste de référence** (`reference: true`) qui définit une `defaultProperty` **et** dont au moins une des `values` n'a pas de traduction connue dans cette langue.

La langue par défaut (celle déclarée comme `defaultLang` dans la configuration globale) est automatiquement ignorée : aucun fichier n'est généré pour elle puisqu'elle sert de source et n'a pas besoin d'être traduite.

_Remarque : `allowPropertyLabelFallback` est bien ignoré par le générateur, seuls les `label` explicitement définis sur les propriétés font l'objet d'une traduction. Si vous utilisez `defaultLang: null`, vous pouvez utiliser `label: ""` par exemple si vous ne voulez pas du tout écrire de libellé dans le modèle._

## Configuration

### `langs`

Langues cibles pour lesquelles les traductions manquantes doivent être générées.

La langue par défaut, définie par `defaultLang` dans la configuration globale, est exclue automatiquement.

### `rootPath`

Chemin des dossiers de traductions sortants (manquants).

_Valeur par défaut_ : `{lang}`

_Templating_ : `{lang}`

_Variables par tag_ : **oui**

Chemin relatif à `outputDirectory`. Les fichiers sont générés sous ce répertoire avec le nom du module en kebab-case et le code de langue.

Les variables définies par tag peuvent être utilisées dans ce chemin, y compris une variable nommée `{tag}` si elle est définie.

### Exemple

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
