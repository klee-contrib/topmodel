using System;
using System.Collections.Generic;
using System.IO;

namespace TopModel.Generator.CSharp
{
    /// <summary>
    /// Classe utilitaire destinée à la génération de C#.
    /// </summary>
    public static class CSharpUtils
    {
        private static IDictionary<string, string>? regType;

        /// <summary>
        /// Retourne le nom du module métier depuis un namespace.
        /// </summary>
        /// <param name="nameSpace">Le namespace</param>
        /// <returns>Module métier</returns>
        public static string ExtractModuleMetier(string nameSpace)
        {
            const string DataContractSuffix = "DataContract";
            const string ContractSuffix = "Contract";
            return nameSpace.EndsWith(DataContractSuffix, StringComparison.InvariantCultureIgnoreCase)
                ? nameSpace.Substring(0, nameSpace.Length - DataContractSuffix.Length)
                : nameSpace.EndsWith(ContractSuffix, StringComparison.InvariantCultureIgnoreCase)
                ? nameSpace.Substring(0, nameSpace.Length - ContractSuffix.Length)
                : nameSpace;
        }

        /// <summary>
        /// Donne la valeur par défaut d'un type de base C#.
        /// Renvoie null si le type n'est pas un type par défaut.
        /// </summary>
        /// <param name="name">Nom du type à définir.</param>
        /// <returns>Vrai si le type est un type C#.</returns>
        public static string? GetCSharpDefaultValueBaseType(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (regType == null)
            {
                InitializeRegType();
            }

            regType!.TryGetValue(name, out var res);
            return res;
        }

        /// <summary>
        /// Retourne le nom du répertoire dans lequel placer la classe générée à partir du ModelClass fourni.
        /// </summary>
        /// <param name="isLegacy">Legacy</param>
        /// <param name="outDir">Répertoire de sortie.</param>
        /// <param name="isPersistent">Trie s'il s'agit du domaine persistant.</param>
        /// <param name="projectName">Nom du projet.</param>
        /// <param name="nameSpace">Namespace de la classe.</param>
        /// <returns>Emplacement dans lequel placer la classe générée à partir du ModelClass fourni.</returns>
        public static string GetDirectoryForModelClass(bool isLegacy, string outDir, bool isPersistent, string projectName, string nameSpace)
        {
            if (isLegacy)
            {
                var basePath = Path.Combine(isPersistent ? GetDataContractDirectoryName(outDir, projectName) : GetContractDirectoryName(outDir, projectName));
                var localPath = nameSpace.Replace('.', Path.DirectorySeparatorChar);
                var path = isPersistent ? Path.Combine(basePath, localPath) : Path.Combine(basePath, localPath, "Dto");
                return Path.Combine(path, "generated");
            }
            else
            {
                var moduleMetier = ExtractModuleMetier(nameSpace);
                var localPath = Path.Combine(moduleMetier, projectName + "." + nameSpace);
                var path = isPersistent ? Path.Combine(outDir, localPath) : Path.Combine(outDir, localPath, "Dto");
                return Path.Combine(path, "generated");
            }
        }

        /// <summary>
        /// Retourne le nom du répertoire du projet d'une classe.
        /// </summary>
        /// <param name="isLegacy">Legacy</param>
        /// <param name="outDir">Répertoire de sortie.</param>
        /// <param name="isPersistent">Trie s'il s'agit du domaine persistant.</param>
        /// <param name="projectName">Nom du projet.</param>
        /// <param name="nameSpace">Namespace de la classe.</param>
        /// <returns>Nom du répertoire contenant le csproj.</returns>
        public static string GetDirectoryForProject(bool isLegacy, string outDir, bool isPersistent, string projectName, string nameSpace)
        {
            if (isLegacy)
            {
                var basePath = Path.Combine(isPersistent ? GetDataContractDirectoryName(outDir, projectName) : GetContractDirectoryName(outDir, projectName));
                var localPath = nameSpace.Replace('.', Path.DirectorySeparatorChar);
                return Path.Combine(basePath, localPath);
            }
            else
            {
                var moduleMetier = ExtractModuleMetier(nameSpace);
                var localPath = Path.Combine(moduleMetier, projectName + "." + nameSpace);
                return Path.Combine(outDir, localPath);
            }
        }

        /// <summary>
        /// Retourne le répertoire dans lequel générer les objets persistants.
        /// </summary>
        /// <param name="outDir">Répertoire sortant.</param>
        /// <param name="projectName">Nom du projet.</param>
        /// <returns>Répertoire dans lequel générer les objets persistants.</returns>
        public static string GetImplementationDirectoryName(string outDir, string projectName)
        {
            return Path.Combine(outDir, projectName + ".Implementation");
        }

        /// <summary>
        /// Détermine si le type est un type de base C#.
        /// </summary>
        /// <param name="name">Nom du type à définir.</param>
        /// <returns>Vrai si le type est un type C#.</returns>
        public static bool IsCSharpBaseType(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (regType == null)
            {
                InitializeRegType();
            }

            return regType!.ContainsKey(name);
        }

        /// <summary>
        /// Retourne le type contenu dans la collection.
        /// </summary>
        /// <param name="dataType">Type de données qualifié.</param>
        /// <returns>Nom du type de données contenu.</returns>
        public static string LoadInnerDataType(string dataType)
        {
            var beginIdx = dataType.LastIndexOf('<');
            var endIdx = dataType.LastIndexOf('>');
            if (beginIdx == -1 || endIdx == -1)
            {
                throw new NotSupportedException();
            }

            return dataType.Substring(beginIdx + 1, endIdx - 1 - beginIdx);
        }

        /// <summary>
        /// Mets au pluriel le nom.
        /// </summary>
        /// <param name="className">Le nom de la classe.</param>
        /// <returns>Au pluriel.</returns>
        public static string Pluralize(string className)
        {
            return className.EndsWith("s") ? className : className + "s";
        }

        /// <summary>
        /// Supprime les points de la chaîne.
        /// </summary>
        /// <param name="dottedString">Chaîne avec points.</param>
        /// <returns>Chaîne sans points.</returns>
        public static string RemoveDots(string dottedString)
        {
            return string.IsNullOrEmpty(dottedString)
                ? dottedString
                : dottedString.Replace(".", string.Empty);
        }

        /// <summary>
        /// Retourne le répertoire dans lequel générer les contrats.
        /// </summary>
        /// <param name="projectName">Nom du projet.</param>
        /// <returns>Répertoire dans lequel générer les contrats.</returns>
        private static string GetContractDirectoryName(string outDir, string projectName)
        {
            return Path.Combine(outDir, projectName + ".Contract");
        }

        /// <summary>
        /// Retourne le répertoire dans lequel générer les objets persistants.
        /// </summary>
        /// <param name="projectName">Nom du projet.</param>
        /// <returns>Répertoire dans lequel générer les objets persistants.</returns>
        private static string GetDataContractDirectoryName(string outDir, string projectName)
        {
            return Path.Combine(outDir, projectName + ".DataContract");
        }

        /// <summary>
        /// Initialisation des types.
        /// </summary>
        private static void InitializeRegType()
        {
            regType = new Dictionary<string, string>
            {
                { "int?", "0" },
                { "uint?", "0" },
                { "float?", "0.0f" },
                { "double?", "0.0" },
                { "bool?", "false" },
                { "short?", "0" },
                { "ushort?", "0" },
                { "long?", "0" },
                { "ulong?", "0" },
                { "decimal?", "0" },
                { "byte?", "0" },
                { "sbyte?", "0" },
                { "string", "\"\"" }
            };
        }
    }
}
