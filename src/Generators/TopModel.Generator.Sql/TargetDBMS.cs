namespace TopModel.Generator.Sql;

/// <summary>
/// SGBD cible pour la génération SQL.
/// </summary>
public enum TargetDBMS
{
    /// <summary>
    /// SQL Server.
    /// </summary>
    Sqlserver,

    /// <summary>
    /// PostgreSQL.
    /// </summary>
    Postgre,

    /// <summary>
    /// Oracle.
    /// </summary>
    Oracle,
}
