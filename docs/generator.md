# Génération

**TopModel.Generator** (TMG) est le générateur de code basé sur `TopModel.Core` et l'application principale à travers laquelle vous pourrez valider et utiliser votre modélisation. Il s'utilise via la CLI **`modgen`**.

TMG dispose de 6 générateurs, qui seront instanciés si la section de configuration associée est renseignée dans le fichier de config du générateur. Chaque générateur se configure avec une liste de **tags**, pour indiquer à chaque générateur les fichiers de modèles qu'il doit lire. Par exemple, si mon modèle est divisé en "modèle persisté" et "modèle non persisté", je peux définir deux tags correspondants ("Data" et "DTO") que je vais associer aux fichiers concernés. Dans ma configuration de générateurs, je peux dire que le tag "Data" sera passé au générateur de SQL et de C#, tandis que le tag "DTO" sera passé au C# et au JS, ce qui permet de ne pas mélanger les choses.

Chaque générateur peut être spécifié **plusieurs fois**, ce qui peut permettre par exemple de générer du modèle dans plusieurs applications, en filtrant les classes/endpoints générés avec des tags.

Les générateurs disponibles sont :

- **Le générateur C# (`csharp`)**, qui permet de générer :
  - Un fichier par classe C#
  - Les interfaces et implémentations des accesseurs de listes de références par module (si Kinetix est utilisé)
  - Le DbContext, si besoin pour Entity Framework (Core)
  - Un contrôleur ou un client, selon la configuration, par fichier de modèle qui contient des endpoints d'API
- **Le générateur JPA (`jpa`)**, qui permet de générer :
  - Un fichier par classe Java
  - Un fichier par DAO
  - Un contrôleur Spring par fichier de modèle qui contient des endpoints d'API
- **Le générateur Javascript (`javascript`)**, qui permet de générer :
  - Un fichier par classe non statique
  - Un fichier de classes statiques par module
  - Un client par fichier de modèle qui contient des endpoints d'API
  - Un fichier de ressources par module (en JS, JSON, ou en JSON Schema)
- **Le générateur de modèle SSDT (`ssdt`)**, qui permet de générer :
  - Un fichier par classe
  - Un fichier par type de table SQL, pour les tables qui en ont besoin (si propriété "InsertKey" présente)
  - Un fichier par liste de référence à initialiser
  - Le fichier d'initialisation des listes de référence, qui appelle, dans l'ordre, tous les fichiers d'initialisation
- **Le générateur de SQL "procédural" (`proceduralSql`)**, qui permet de générer, pour postgre ou sqlserver :
  - Un fichier "crebas" qui contient toutes les créations de tables
  - Un fichier "index + fk" qui contient toutes les FKs et indexes
  - Un fichier qui contient toutes les initialisations de listes de références
