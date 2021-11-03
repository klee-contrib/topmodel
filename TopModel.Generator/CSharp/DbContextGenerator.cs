using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.CSharp
{
    using static CSharpUtils;

    public class DbContextGenerator
    {
        private readonly CSharpConfig _config;
        private readonly ILogger<CSharpGenerator> _logger;

        public DbContextGenerator(CSharpConfig config, ILogger<CSharpGenerator> logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Génère l'objectcontext spécialisé pour le schéma.
        /// </summary>
        /// <remarks>Support de Linq2Sql.</remarks>
        /// <param name="classes">Classes.</param>
        public void Generate(IEnumerable<Class> classes)
        {
            if (_config.OutputDirectory == null || _config.DbContextPath == null)
            {
                return;
            }

            var dbContextName = _config.GetDbContextName(classes.First().Namespace.App);
            var destDirectory = Path.Combine(_config.OutputDirectory, _config.DbContextPath);
            Directory.CreateDirectory(destDirectory);

            var targetFileName = Path.Combine(destDirectory, "generated", $"{dbContextName}.cs");
            using var w = new CSharpWriter(targetFileName, _logger);

            var usings = new List<string>();
            if (_config.Kinetix != KinetixVersion.Framework)
            {
                usings.Add("Microsoft.EntityFrameworkCore");
            }
            else
            {
                usings.Add("System.Data.Entity");
                usings.Add("System.Transactions");
                usings.Add("Kinetix.Data.SqlClient");
            }

            foreach (var ns in classes.Select(_config.GetNamespace).Distinct())
            {
                usings.Add(ns);
            }

            w.WriteUsings(usings.ToArray());

            var contextNs = _config.DbContextPath.Split("/").Last();
            w.WriteLine();
            w.WriteLine($"namespace {contextNs}");
            w.WriteLine("{");

            if (_config.Kinetix != KinetixVersion.Framework)
            {
                w.WriteSummary(1, "DbContext généré pour Entity Framework Core.");
                w.WriteLine(1, $"public partial class {dbContextName} : DbContext");
                w.WriteLine(1, "{");

                w.WriteSummary(2, "Constructeur par défaut.");
                w.WriteParam("options", "Options du DbContext.");
                w.WriteLine(2, $"public {dbContextName}(DbContextOptions<{dbContextName}> options)");
                w.WriteLine(3, ": base(options)");
                w.WriteLine(2, "{");
                w.WriteLine(2, "}");
            }
            else
            {
                w.WriteSummary(1, "DbContext généré pour Entity Framework 6.");
                w.WriteLine(1, $"public partial class {dbContextName}DbContext");
                w.WriteLine(1, "{");

                w.WriteSummary(2, "Constructeur par défaut.");
                w.WriteLine(2, $"public {dbContextName}()");
                w.WriteLine(3, ": base(SqlServerManager.Instance.ObtainConnection(\"default\"), false)");
                w.WriteLine(2, "{");
                w.WriteLine(2, "}");

                w.WriteLine();
                w.WriteSummary(2, "Constructeur par défaut.");
                w.WriteParam("scope", "Transaction scope.");
                w.WriteLine(2, $"public {dbContextName}(TransactionScope scope)");
                w.WriteLine(3, ": this()");
                w.WriteLine(2, "{");
                w.WriteLine(2, "}");
            }

            foreach (var classe in classes.OrderBy(c => c.Name))
            {
                w.WriteLine();
                w.WriteSummary(2, "Accès à l'entité " + classe.Name);
                w.WriteLine(2, "public DbSet<" + classe.Name + "> " + Pluralize(classe.Name) + " { get; set; }");
            }

            if (_config.Kinetix != KinetixVersion.Framework && _config.UseEFMigrations)
            {
                w.WriteLine();
                w.WriteSummary(2, "Personalisation du modèle.");
                w.WriteParam("modelBuilder", "L'objet de construction du modèle.");
                w.WriteLine(2, "protected override void OnModelCreating(ModelBuilder modelBuilder)");
                w.WriteLine(2, "{");

                var hasEnum = false;
                foreach (var prop in classes.SelectMany(c => c.Properties.OfType<IFieldProperty>()))
                {
                    if (prop.PrimaryKey && _config.CanClassUseEnums(prop.Class) || prop is AssociationProperty ap && _config.CanClassUseEnums(ap.Association))
                    {
                        hasEnum = true;
                        w.WriteLine(3, $"modelBuilder.Entity<{prop.Class}>().Property(p => p.{prop.Name}).HasConversion<string>();");
                    }
                }

                if (hasEnum)
                {
                    w.WriteLine();
                }

                var hasFk = false;
                foreach (var prop in classes.SelectMany(c => c.Properties.OfType<AssociationProperty>()))
                {
                    hasFk = true;
                    w.WriteLine(3, $"modelBuilder.Entity<{prop.Class.Name}>().HasOne<{prop.Association}>().WithMany().HasForeignKey(p => p.{prop.Name}).OnDelete(DeleteBehavior.Restrict);");
                }

                if (hasFk)
                {
                    w.WriteLine();
                }

                var hasUk = false;
                foreach (var uk in classes.SelectMany(c => c.UniqueKeys ?? new List<IList<IFieldProperty>>()))
                {
                    hasUk = true;
                    var expr = uk.Count == 1 ? $"p.{uk.Single().Name}" : $"new {{ {string.Join(", ", uk.Select(p => $"p.{p.Name}"))} }}";
                    w.WriteLine(3, $"modelBuilder.Entity<{uk.First().Class}>().HasIndex(p => {expr}).IsUnique();");
                }

                if (hasUk)
                {
                    w.WriteLine();
                }

                var hasData = false;
                foreach (var classe in classes.Where(c => c.ReferenceValues != null).OrderBy(c => c.Name))
                {
                    hasData = true;
                    w.WriteLine(3, $"modelBuilder.Entity<{classe.Name}>().HasData(");
                    foreach (var refValue in classe.ReferenceValues!)
                    {
                        w.Write($"                new {classe.Name} {{");
                        foreach (var prop in refValue.Value.ToList())
                        {
                            var value = _config.CanClassUseEnums(classe) && prop.Key.PrimaryKey
                                ? $"{(classe.Name.EndsWith("s") ? $"{string.Join(".", _config.GetNamespace(classe).Split(".").Except(contextNs.Split(".")))}.{classe.Name}" : classe.Name)}.{classe.PrimaryKey!.Name}s.{prop.Value}"
                                : prop.Key.Domain.ShouldQuoteSqlValue
                                ? $"\"{prop.Value}\""
                                : prop.Value is bool b
                                ? (b ? "true" : "false")
                                : prop.Value;
                            w.Write($" {prop.Key.Name} = {value}");
                            if (refValue.Value.ToList().IndexOf(prop) < refValue.Value.Count() - 1)
                            {
                                w.Write(",");
                            }
                        }

                        w.Write(" }");
                        if (classe.ReferenceValues.IndexOf(refValue) < classe.ReferenceValues.Count - 1)
                        {
                            w.Write(",\r\n");
                        }
                    }

                    w.Write(");\r\n");
                }

                if (hasData)
                {
                    w.WriteLine();
                }

                w.WriteLine(3, "OnModelCreatingPartial(modelBuilder);");

                w.WriteLine(2, "}");

                w.WriteLine();
                w.WriteLine(2, "partial void OnModelCreatingPartial(ModelBuilder modelBuilder);");
            }
            else if (_config.Kinetix == KinetixVersion.Framework)
            {
                w.WriteLine();
                w.WriteSummary(2, "Hook pour l'ajout de configuration su EF (précision des champs, etc).");
                w.WriteParam("modelBuilder", "L'objet de construction du modèle.");
                w.WriteLine(2, "protected override void OnModelCreating(DbModelBuilder modelBuilder)");
                w.WriteLine(2, "{");
                w.WriteLine(3, "base.OnModelCreating(modelBuilder);");
                w.WriteLine();

                foreach (var classe in classes.Where(c => c.IsPersistent).OrderBy(c => c.Name))
                {
                    foreach (var property in classe.Properties.OfType<IFieldProperty>())
                    {
                        if (property.Domain.SqlTypePrecision.HasValue)
                        {
                            w.WriteLine(3, string.Format(
                                "modelBuilder.Entity<{0}>().Property(x => x.{1}).HasPrecision({2}, {3});",
                                classe.Name,
                                property.Name,
                                property.Domain.SqlTypePrecision.Value.Length,
                                property.Domain.SqlTypePrecision.Value.Precision));
                        }
                    }
                }

                w.WriteLine();
                w.WriteLine(3, "OnModelCreatingCustom(modelBuilder);");
                w.WriteLine(2, "}");

                w.WriteLine();
                w.WriteSummary(2, "Hook pour l'ajout de configuration custom sur EF (view, etc).");
                w.WriteParam("modelBuilder", "L'objet de construction du modèle");
                w.WriteLine(2, "partial void OnModelCreatingCustom(DbModelBuilder modelBuilder);");
            }

            w.WriteLine(1, "}");
            w.WriteLine("}");
        }
    }
}
