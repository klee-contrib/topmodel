using System.IO;
using TopModel.Core;
using TopModel.Core.Config;

namespace TopModel.Generator.ProceduralSql
{
    /// <summary>
    /// Générateur de Transact-SQL.
    /// </summary>
    public class SqlServerSchemaGenerator : AbstractSchemaGenerator
    {
        public SqlServerSchemaGenerator(string appName, ProceduralSqlConfig config)
            : base(appName, config)
        {
        }

        protected override string BatchSeparator => "go";

        protected override bool SupportsClusteredKey => true;

        /// <summary>
        /// Gère l'auto-incrémentation des clés primaires en ajoutant identity à la colonne.
        /// </summary>
        /// <param name="writerCrebas">Flux d'écriture création bases.</param>
        protected override void WriteIdentityColumn(StreamWriter writerCrebas)
        {
            writerCrebas.Write(" identity(1, 1)");
        }

        /// <summary>
        /// Ecrit dans le writer le script de création du type.
        /// </summary>
        /// <param name="classe">Classe.</param>
        /// <param name="writerType">Writer.</param>
        protected override void WriteType(Class classe, StreamWriter? writerType)
        {
            var typeName = classe.SqlName + "_TABLE_TYPE";
            writerType?.WriteLine("/**");
            writerType?.WriteLine("  * Création du type " + classe.SqlName + "_TABLE_TYPE");
            writerType?.WriteLine(" **/");
            writerType?.WriteLine("If Exists (Select * From sys.types st Join sys.schemas ss On st.schema_id = ss.schema_id Where st.name = N'" + typeName + "')");
            writerType?.WriteLine("Drop Type " + typeName + '\n');
            writerType?.WriteLine("Create type " + typeName + " as Table (");
        }
    }
}
