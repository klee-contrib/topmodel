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

    public override string Name => "CSharpDbContextGen";

    public override IEnumerable<string> GeneratedFiles => new[]
    {
        _config.GetDbContextFilePath(_appName),
        _config.UseEFComments ? _config.GetDbContextFilePath(_appName).Replace(".cs", ".comments.cs") : null!
    }
    .Where(x => x != null);

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        var classes = Classes.Where(c => c.IsPersistent).OrderBy(c => c.Name).ToList();
        var dbContextName = _config.GetDbContextName(_appName);
        var targetFileName = _config.GetDbContextFilePath(_appName);

        using var w = new CSharpWriter(targetFileName, _logger, _config.UseLatestCSharp);

        var usings = new List<string> { "Microsoft.EntityFrameworkCore" };

        foreach (var ns in classes.Select(_config.GetNamespace).Distinct())
        {
            usings.Add(ns);
        }

        w.WriteUsings(usings.ToArray());

        var contextNs = _config.GetDbContextNamespace(_appName);

        w.WriteLine();
        w.WriteNamespace(contextNs);

        w.WriteSummary(1, "DbContext généré pour Entity Framework Core.");
        w.WriteLine(1, $"public partial class {dbContextName} : DbContext");
        w.WriteLine(1, "{");

        w.WriteSummary(2, "Constructeur par défaut.");
        w.WriteParam("options", "Options du DbContext.");
        w.WriteLine(2, $"public {dbContextName}(DbContextOptions<{dbContextName}> options)");
        w.WriteLine(3, ": base(options)");
        w.WriteLine(2, "{");
        w.WriteLine(2, "}");

        foreach (var classe in classes)
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
            w.WriteLine(2, "protected override void OnModelCreating(ModelBuilder modelBuilder)");
            w.WriteLine(2, "{");

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

            var hasPk = false;
            foreach (var classe in classes.Distinct().OrderBy(c => c.Name).Where(c => c.PrimaryKey.Count() > 1))
            {
                hasPk = true;
                w.WriteLine(3, $"modelBuilder.Entity<{classe}>().HasKey(p => new {{ {string.Join(", ", classe.PrimaryKey.Select(pk => $"p.{pk.Name}"))} }});");
            }

            if (hasPk)
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

                w.WriteLine(3, $"modelBuilder.Entity<{prop.Class}>().Has{(ap.Type == AssociationType.ManyToOne || ap.Type == AssociationType.OneToOne ? "One" : "Many")}<{ap.Association}>().With{(ap.Type == AssociationType.ManyToOne || ap.Type == AssociationType.ManyToMany ? "Many" : "One")}(){(ap.Type == AssociationType.ManyToOne || ap.Type == AssociationType.OneToOne ? $".HasForeignKey{(ap.Type == AssociationType.ManyToOne ? string.Empty : $"<{prop.Class}>")}(p => p.{prop.Name})" : string.Empty)}.OnDelete(DeleteBehavior.Restrict);");
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
                        return $"{(targetClass.Name == targetClass.PluralName ? $"{_config.GetNamespace(targetClass)}.{targetClass.Name}" : targetClass.Name)}.{targetClass.PrimaryKey.Single().Name}s.{value}";
                    }

                    foreach (var prop in refValue.Value.ToList())
                    {
                        var value = _config.CanClassUseEnums(classe) && prop.Key.PrimaryKey
                            ? WriteEnumValue(classe, prop.Value)
                            : prop.Key is AssociationProperty ap && _config.CanClassUseEnums(ap.Association)
                            ? WriteEnumValue(ap.Association, prop.Value)
                            : prop.Key.Domain.CSharp!.Type.Contains("Date")
                            ? $"{prop.Key.Domain.CSharp.Type.ParseTemplate(prop.Key).TrimEnd('?')}.Parse(\"{prop.Value}\"){(prop.Key.Domain.CSharp.Type.Contains("Time") ? ".ToUniversalTime()" : string.Empty)}"
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

            if (_config.UseEFComments)
            {
                w.WriteLine(3, "AddComments(modelBuilder);");
            }

            w.WriteLine(3, "OnModelCreatingPartial(modelBuilder);");
            w.WriteLine(2, "}");

            if (_config.UseEFComments)
            {
                w.WriteLine();
                w.WriteLine(2, "partial void AddComments(ModelBuilder modelBuilder);");
            }

            w.WriteLine();
            w.WriteLine(2, "partial void OnModelCreatingPartial(ModelBuilder modelBuilder);");
        }

        w.WriteLine(1, "}");
        w.WriteNamespaceEnd();

        w.Dispose();

        if (_config.UseEFComments)
        {
            using var cw = new CSharpWriter(targetFileName.Replace(".cs", ".comments.cs"), _logger, _config.UseLatestCSharp);

            var cUsings = new List<string>
            {
                "Microsoft.EntityFrameworkCore"
            };

            foreach (var ns in classes.Select(_config.GetNamespace).Distinct())
            {
                cUsings.Add(ns);
            }

            cw.WriteUsings(usings.ToArray());

            cw.WriteLine();
            cw.WriteNamespace(contextNs);

            cw.WriteSummary(1, "Partial pour ajouter les commentaires EF.");
            cw.WriteLine(1, $"public partial class {dbContextName} : DbContext");
            cw.WriteLine(1, "{");
            cw.WriteLine(2, "partial void AddComments(ModelBuilder modelBuilder)");
            cw.WriteLine(2, "{");

            foreach (var classe in classes)
            {
                cw.WriteLine(3, $"var {classe.Name.ToFirstLower()} = modelBuilder.Entity<{classe.Name}>();");
                cw.WriteLine(3, $"{classe.Name.ToFirstLower()}.ToTable(t => t.HasComment(\"{classe.Comment.Replace("\"", "\\\"")}\"));");

                foreach (var property in classe.Properties.OfType<IFieldProperty>())
                {
                    cw.WriteLine(3, $"{classe.Name.ToFirstLower()}.Property(p => p.{property.Name}).HasComment(\"{property.Comment.Replace("\"", "\\\"")}\");");
                }

                if (classes.IndexOf(classe) < classes.Count - 1)
                {
                    cw.WriteLine();
                }
            }

            cw.WriteLine(2, "}");
            cw.WriteLine(1, "}");
        }
    }
}