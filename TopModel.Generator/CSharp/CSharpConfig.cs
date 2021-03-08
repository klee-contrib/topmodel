using System;
using System.Linq;

namespace TopModel.Generator.CSharp
{
    /// <summary>
    /// Paramètres pour la génération du C#.
    /// </summary>
    public class CSharpConfig : GeneratorConfigBase
    {
        /// <summary>
        /// Racine du répertoire de génération.
        /// </summary>
        public string? OutputDirectory { get; set; }

        /// <summary>
        /// Localisation du modèle persisté, relative au répertoire de génération. Par défaut : {module}/{app}.{module}DataContract.
        /// </summary>
        public string PersistantModelPath { get; set; } = "{module}/{app}.{module}DataContract";

        /// <summary>
        /// Localisation du modèle non-persisté, relative au répertoire de génération. Par défaut : {module}/{app}.{module}Contract/Dto.
        /// </summary>
        public string NonPersistantModelPath { get; set; } = "{module}/{app}.{module}Contract/Dto";

        /// <summary>
        /// Localisation du l'API générée (client ou serveur), relatif au répertoire de génération. Par défaut : {app}.{module}.
        /// </summary>
        public string ApiPath { get; set; } = "{app}.{module}";

        /// <summary>
        /// Mode de génération de l'API ("client" ou "server").
        /// </summary>
        public ApiGeneration ApiGeneration { get; set; }

        /// <summary>
        /// Localisation du DbContext, relatif au répertoire de génération.
        /// </summary>
        public string? DbContextPath { get; set; }

        /// <summary>
        /// Utilise les migrations EF pour créer/mettre à jour la base de données.
        /// </summary>
        public bool UseEFMigrations { get; set; }

        /// <summary>
        /// Utilise des noms de tables et de colonnes en lowercase.
        /// </summary>
        public bool UseLowerCaseSqlNames { get; set; }

        /// <summary>
        /// Le nom du schéma de base de données à cibler (si non renseigné, EF utilise 'dbo').
        /// </summary>
        public string? DbSchema { get; set; }

        /// <summary>
        /// Version de kinetix utilisée: Core, Framework ou None.
        /// </summary>
        public KinetixVersion Kinetix { get; set; }

        /// <summary>
        /// Retire les attributs de colonnes sur les alias.
        /// </summary>
        public bool NoColumnOnAlias { get; set; }

        /// <summary>
        /// Récupère le nom du DbContext.
        /// </summary>
        /// <param name="appName">Nom de l'application.</param>
        /// <returns>Nom.</returns>
        public string GetDbContextName(string appName)
        {
            return DbContextPath == null
                ? throw new Exception("Le DbContext doit être renseigné.")
                : DbSchema != null
                    ? $"{DbSchema.First().ToString().ToUpper() + DbSchema[1..]}DbContext"
                    : $"{appName.Replace(".", string.Empty)}DbContext";
        }

        /// <summary>
        /// Récupère le chemin vers un fichier de classe à générer.
        /// </summary>
        /// <param name="classe">La classe.</param>
        /// <returns>Chemin.</returns>
        public string GetModelPath(Class classe)
        {
            var baseModelPath = classe.IsPersistent ? PersistantModelPath : NonPersistantModelPath;
            return !baseModelPath.Contains("{app}") || !baseModelPath.Contains("{module}")
                ? throw new Exception("Les localisations de modèle doivent contenir '{app}' et '{module}'.")
                : baseModelPath.Replace("{app}", classe.Namespace.App).Replace("{module}", classe.Namespace.Module);
        }

        /// <summary>
        /// Récupère le namespace d'une classe.
        /// </summary>
        /// <param name="classe">La classe.</param>
        /// <returns>Namespace.</returns>
        public string GetNamespace(Class classe)
        {
            var ns = GetModelPath(classe).Replace("/", ".")
                .Replace(".Contract", string.Empty)
                .Replace(".DataContract", string.Empty)
                .Replace(".Dto", string.Empty);
            return ns[ns.IndexOf(classe.Namespace.App)..];
        }
    }
}
