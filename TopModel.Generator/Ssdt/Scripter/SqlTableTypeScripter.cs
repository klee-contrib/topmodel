using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TopModel.Core;
using TopModel.Generator.Ssdt.Contract;

namespace TopModel.Generator.Ssdt.Scripter
{
    /// <summary>
    /// Scripter permettant d'écrire les scripts de création d'un type de table SQL.
    /// </summary>
    public class SqlTableTypeScripter : ISqlScripter<Class>
    {
        private const string InsertKeyName = "InsertKey";

        /// <summary>
        /// Calcule le nom du script pour l'item.
        /// </summary>
        /// <param name="item">Item à scripter.</param>
        /// <returns>Nom du fichier de script.</returns>
        public string GetScriptName(Class item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item.GetTableTypeName() + ".sql";
        }

        /// <summary>
        /// Indique si l'item doit générer un script.
        /// </summary>
        /// <param name="item">Item candidat.</param>
        /// <returns><code>True</code> si un script doit être généré.</returns>
        public bool IsScriptGenerated(Class item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item.Properties.Where(p => p.Name == InsertKeyName).Count() > 0;
        }

        /// <summary>
        /// Ecrit dans un flux le script pour l'item.
        /// </summary>
        /// <param name="writer">Flux d'écriture.</param>
        /// <param name="item">Table à scripter.</param>
        public void WriteItemScript(TextWriter writer, Class item)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            // Entête du fichier.
            WriteHeader(writer, item.GetTableTypeName());

            // Ouverture du create table.
            WriteCreateTableOpening(writer, item);

            // Intérieur du create table.
            WriteInsideInstructions(writer, item);

            // Fin du create table.
            WriteCreateTableClosing(writer);
        }

        /// <summary>
        /// Ecrit le SQL pour une colonne.
        /// </summary>
        /// <param name="sb">Flux.</param>
        /// <param name="property">Propriété.</param>
        private static void WriteColumn(StringBuilder sb, IFieldProperty property)
        {
            var persistentType = property.Domain.SqlType;
            sb.Append("[").Append(property.SqlName).Append("] ").Append(persistentType).Append(" null");
        }

        /// <summary>
        /// Ecrit le pied du script.
        /// </summary>
        /// <param name="writer">Flux.</param>
        private static void WriteCreateTableClosing(TextWriter writer)
        {
            writer.WriteLine(")");
            writer.WriteLine("go");
            writer.WriteLine();
        }

        /// <summary>
        /// Ecrit l'ouverture du create table.
        /// </summary>
        /// <param name="writer">Flux.</param>
        /// <param name="table">Table.</param>
        private static void WriteCreateTableOpening(TextWriter writer, Class table)
        {
            writer.WriteLine("Create type [" + table.GetTableTypeName() + "] as Table (");
        }

        /// <summary>
        /// Ecrit l'entête du fichier.
        /// </summary>
        /// <param name="writer">Flux.</param>
        /// <param name="tableName">Nom de la table.</param>
        private static void WriteHeader(TextWriter writer, string tableName)
        {
            writer.WriteLine("-- ===========================================================================================");
            writer.WriteLine("--   Description		:	Création du type de table " + tableName + ".");
            writer.WriteLine("-- ===========================================================================================");
            writer.WriteLine();
        }

        /// <summary>
        /// Ecrit la colonne InsertKey.
        /// </summary>
        /// <param name="sb">Flux.</param>
        /// <param name="classe">Classe.</param>
        private static void WriteInsertKeyLine(StringBuilder sb, Class classe)
        {
            sb.Append("[").Append(classe.TrigramPrefix + "INSERT_KEY] int null");
        }

        /// <summary>
        /// Ecrit les instructions à l'intérieur du create table.
        /// </summary>
        /// <param name="writer">Flux.</param>
        /// <param name="table">Table.</param>
        private static void WriteInsideInstructions(TextWriter writer, Class table)
        {
            // Construction d'une liste de toutes les instructions.
            var definitions = new List<string>();
            var sb = new StringBuilder();

            // Colonnes
            foreach (var property in table.Properties.OfType<IFieldProperty>())
            {
                if ((!property.PrimaryKey || property.Domain.CsharpType == "string") && property.Name != InsertKeyName)
                {
                    sb.Clear();
                    WriteColumn(sb, property);
                    definitions.Add(sb.ToString());
                }
            }

            // InsertKey.
            sb.Clear();
            WriteInsertKeyLine(sb, table);
            definitions.Add(sb.ToString());

            // Ecriture de la liste concaténée.
            var separator = "," + Environment.NewLine;
            writer.Write(string.Join(separator, definitions.Select(x => "\t" + x)));
        }
    }
}
