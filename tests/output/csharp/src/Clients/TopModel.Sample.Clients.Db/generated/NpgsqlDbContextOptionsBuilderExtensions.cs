////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Npgsql.NameTranslation;
using TopModel.Sample.Clients.Db.Models.Restaurant;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db;

/// <summary>
/// Extension du builder Npgsql pour enregistrer les enums Postgres.
/// </summary>
public static class NpgsqlDbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Enregistre les enums Postgres.
    /// </summary>
    /// <param name="builder">Le builder Npgsql.</param>
    public static void MapEnums(this NpgsqlDbContextOptionsBuilder builder)
    {
        var nameTranslator = new NpgsqlNullNameTranslator();
        builder.MapEnum<StatutCommande>("statut_commande", nameTranslator: nameTranslator);
        builder.MapEnum<TypeTerrasse>("type_terrasse", nameTranslator: nameTranslator);
    }
}
