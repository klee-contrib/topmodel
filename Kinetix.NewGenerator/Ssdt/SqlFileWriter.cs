using Kinetix.Tools.Common;

namespace Kinetix.NewGenerator.Ssdt
{
    /// <summary>
    /// Writer pour l'écriture de fichier.
    /// Spécifique pour les fichiers SQL (usage du token commentaire SQL).
    /// </summary>
    internal class SqlFileWriter : FileWriter
    {
        public SqlFileWriter(string fileName) 
            : base(fileName)
        {
        }

        /// <summary>
        /// Renvoie le token de début de ligne de commentaire dans le langage du fichier.
        /// </summary>
        /// <returns>Toket de début de ligne de commentaire.</returns>
        protected override string StartCommentToken => "----";
    }
}
