using System.Collections.Generic;
using Kinetix.NewGenerator.Model;

#nullable disable
namespace Kinetix.NewGenerator.Ssdt.Dto
{
    /// <summary>
    /// Table de référence.
    /// Contient une définition de classe et une liste de valeurs.
    /// </summary>
    public class ReferenceClass
    {
        /// <summary>
        /// Définition de la classe.
        /// </summary>
        public Class Class
        {
            get;
            set;
        }

        /// <summary>
        /// Liste des valeurs de la table de référence.
        /// </summary>
        public IEnumerable<ReferenceValue> Values
        {
            get;
            set;
        }

        /// <summary>
        /// Indique si la liste de référence est statique.
        /// </summary>
        public bool IsStatic
        {
            get;
            set;
        }
    }
}
