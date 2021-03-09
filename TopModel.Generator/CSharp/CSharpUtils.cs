using System;
using System.Collections.Generic;

namespace TopModel.Generator.CSharp
{
    /// <summary>
    /// Classe utilitaire destinée à la génération de C#.
    /// </summary>
    public static class CSharpUtils
    {
        private static IDictionary<string, string>? regType;

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

        public static string GetPropertyTypeName(IProperty prop, bool nonNullable = false)
        {
            var type = prop switch
            {
                IFieldProperty fp => fp.Domain.CSharp?.Type ?? string.Empty,
                CompositionProperty cp => cp.Kind switch
                {
                    "object" => cp.Composition.Name,
                    "list" => $"IEnumerable<{cp.Composition.Name}>",
                    string _ => $"{cp.DomainKind!.CSharp!.Type}<{cp.Composition.Name}>"
                },
                _ => string.Empty
            };

            return nonNullable && type.EndsWith("?") ? type[0..^1] : type;
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
        /// Mets au pluriel le nom.
        /// </summary>
        /// <param name="className">Le nom de la classe.</param>
        /// <returns>Au pluriel.</returns>
        public static string Pluralize(string className)
        {
            return className.EndsWith("s") ? className : className + "s";
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
