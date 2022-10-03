using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.CSharp;

public class DbContextGenerator : GeneratorBase
{
    private readonly string _appName;
    private readonly CSharpConfig _config;
    private readonly ILogger<DbContextGenerator> _logger;

    public DbContextGenerator(ILogger<DbContextGenerator> logger, CSharpConfig config, string appName)
        : base(logger, config)
    {
        _appName = appName;
        _config = config;
        _logger = logger;
    }

    public override string Name => "DbContextGen";

    public override IEnumerable<string> GeneratedFiles => new[] { _config.GetDbContextFilePath(_appName) };

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        var classes = Files.Values.SelectMany(f => f.Classes).Where(c => c.IsPersistent);
        var dbContextName = _config.GetDbContextName(_appName);
        var targetFileName = _config.GetDbContextFilePath(_appName);

        using var w = new CSharpWriter(targetFileName, _logger, _config.UseLatestCSharp);

        var usings = new List<string>();
        if (_config.Kinetix != KinetixVersion.Framework)
        {
            usings.Add("Microsoft.EntityFrameworkCore");
        }
        else
        {
            usings.Add("System.Data.Entity");
            usings.Add("System.Transactions");
            usings.Add("Kinetix.DataAccess.Sql");
        }

        foreach (var ns in classes.Select(_config.GetNamespace).Distinct())
        {
            usings.Add(ns);
        }

        w.WriteUsings(usings.ToArray());

        var contextNs = _config.DbContextPath!.Split("/").Last();
        w.WriteLine();
        w.WriteNamespace(contextNs);

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

        foreach (var classe in classes.Distinct().OrderBy(c => c.Name))
        {
            w.WriteLine();
            w.WriteSummary(2, "Accès à l'entité " + classe.Name);
            w.WriteLine(2, "public DbSet<" + classe.Name + "> " + classe.PluralName + " { get; set; }");
        }

        if (_config.UseEFMigrations)
        {
            w.WriteLine();
            w.WriteSummary(2, "Personalisation du modèle.");
            w.WriteParam("modelBuilder", "L'objet de construction du modèle.");

            if (_config.Kinetix == KinetixVersion.Framework)
            {
                w.WriteLine(2, "protected override void OnModelCreating(DbModelBuilder modelBuilder)");
                w.WriteLine(2, "{");
                w.WriteLine(3, "base.OnModelCreating(modelBuilder);");
            }
            else
            {
                w.WriteLine(2, "protected override void OnModelCreating(ModelBuilder modelBuilder)");
                w.WriteLine(2, "{");
            }

            var hasPropConfig = false;
            foreach (var prop in classes.Distinct().OrderBy(c => c.Name).SelectMany(c => c.Properties.OfType<IFieldProperty>()))
            {
                if (prop.PrimaryKey && _config.CanClassUseEnums(prop.Class) || prop is AssociationProperty ap && _config.CanClassUseEnums(ap.Association))
                {
                    hasPropConfig = true;
                    w.WriteLine(3, $"modelBuilder.Entity<{prop.Class}>().Property(p => p.{prop.Name}).HasConversion<string>(){(prop.Domain.Length != null ? $".HasMaxLength({prop.Domain.Length})" : string.Empty)};");
                }

                if (prop.Domain.Length != null && prop.Domain.Scale != null)
                {
                    hasPropConfig = true;
                    w.WriteLine(3, $"modelBuilder.Entity<{prop.Class}>().Property(x => x.{prop.Name}).HasPrecision({prop.Domain.Length}, {prop.Domain.Scale});");
                }
            }

            if (hasPropConfig)
            {
                w.WriteLine();
            }

            var hasFk = false;
            foreach (var prop in classes.Distinct().OrderBy(c => c.Name).SelectMany(c => c.Properties).Where(p => p is AssociationProperty { Association.IsPersistent: true } || p is AliasProperty { Property: AssociationProperty { Association.IsPersistent: true } }))
            {
                hasFk = true;
                var ap = prop switch
                {
                    AssociationProperty a => a,
                    AliasProperty { Property: AssociationProperty a } => a,
                    _ => null!
                };

                if (ap.Type != AssociationType.ManyToOne && ap.Type != AssociationType.OneToOne)
                {
                    throw new ModelException(ap, $"Le type d'association {ap.Type} n'est pas supporté par le générateur C#");
                }

                w.WriteLine(3, $"modelBuilder.Entity<{prop.Class}>().HasOne<{ap.Association}>().With{(ap.Type == AssociationType.ManyToOne ? "Many" : "One")}().HasForeignKey{(ap.Type == AssociationType.ManyToOne ? string.Empty : $"<{prop.Class}>")}(p => p.{prop.Name}).OnDelete(DeleteBehavior.Restrict);");
            }

            if (hasFk)
            {
                w.WriteLine();
            }

            var hasUk = false;
            foreach (var uk in classes.Distinct().OrderBy(c => c.Name).SelectMany(c => c.UniqueKeys))
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
            foreach (var classe in classes.Distinct().Where(c => c.ReferenceValues.Any()).OrderBy(c => c.Name))
            {
                hasData = true;
                w.WriteLine(3, $"modelBuilder.Entity<{classe.Name}>().HasData(");
                foreach (var refValue in classe.ReferenceValues)
                {
                    if (!_config.UseLatestCSharp)
                    {
                        w.Write("    ");
                    }

                    w.Write($"            new {classe.Name} {{");

                    string WriteEnumValue(Class targetClass, string value)
                    {
                        return $"{(targetClass.Name == targetClass.PluralName ? $"{string.Join(".", _config.GetNamespace(targetClass).Split(".").Except(contextNs.Split(".")))}.{targetClass.Name}" : targetClass.Name)}.{targetClass.PrimaryKey!.Name}s.{value}";
                    }

                    foreach (var prop in refValue.Value.ToList())
                    {
                        var value = _config.CanClassUseEnums(classe) && prop.Key.PrimaryKey
                            ? WriteEnumValue(classe, prop.Value)
                            : prop.Key is AssociationProperty ap && _config.CanClassUseEnums(ap.Association)
                            ? WriteEnumValue(ap.Association, prop.Value)
                            : prop.Key.Domain.CSharp!.Type.Contains("Date")
                            ? $"{prop.Key.Domain.CSharp.Type.TrimEnd('?')}.Parse(\"{prop.Value}\"){(prop.Key.Domain.CSharp.Type.Contains("Time") ? ".ToUniversalTime()" : string.Empty)}"
                            : prop.Key.Domain.ShouldQuoteSqlValue
                            ? $"\"{prop.Value}\""
                            : prop.Value;
                        w.Write($" {prop.Key.Name} = {value}");
                        if (refValue.Value.ToList().IndexOf(prop) < refValue.Value.Count - 1)
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

        w.WriteLine(1, "}");
        w.WriteNamespaceEnd();
    }
}