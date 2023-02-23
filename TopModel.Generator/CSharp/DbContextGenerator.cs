using Microsoft.Extensions.Logging;
using TopModel.Core;

namespace TopModel.Generator.CSharp;

public class DbContextGenerator : ClassGroupGeneratorBase
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

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.IsPersistent && !classe.Abstract)
        {
            yield return ("main", _config.GetDbContextFilePath(_appName, tag));

            if (_config.UseEFComments)
            {
                yield return ("comments", _config.GetDbContextFilePath(_appName, tag).Replace(".cs", ".comments.cs"));
            }
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        var dbContextName = _config.GetDbContextName(_appName, tag);
        var usings = new List<string> { "Microsoft.EntityFrameworkCore" };
        var contextNs = _config.GetDbContextNamespace(_appName, tag);

        foreach (var ns in classes
            .Concat(GetAssociationProperties(classes).Select(ap => ap.AssociationProperty.Association))
            .Select(c => _config.GetNamespace(c))
            .Distinct())
        {
            usings.Add(ns);
        }

        var classList = classes.OrderBy(c => c.Name).ToList();

        if (fileType == "main")
        {
            HandleMainFile(fileName, dbContextName, contextNs, usings, classList);
        }
        else
        {
            HandleCommentsFile(fileName, dbContextName, contextNs, usings, classList);
        }
    }

    private void HandleMainFile(string fileName, string dbContextName, string contextNs, IList<string> usings, IList<Class> classes)
    {
        using var w = new CSharpWriter(fileName, _logger, _config.UseLatestCSharp);

        w.WriteUsings(usings.ToArray());
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

        w.WriteLine();
        w.WriteSummary(2, "Personalisation du modèle.");
        w.WriteParam("modelBuilder", "L'objet de construction du modèle.");
        w.WriteLine(2, "protected override void OnModelCreating(ModelBuilder modelBuilder)");
        w.WriteLine(2, "{");

        var hasPropConfig = false;
        foreach (var fp in classes.Distinct().OrderBy(c => c.Name).SelectMany(c => c.Properties.OfType<IFieldProperty>()))
        {
            var prop = fp is AliasProperty alp ? alp.Property : fp;
            var ap = prop as AssociationProperty;

            var classe = ap != null ? ap.Association : prop.Class;
            var targetProp = ap != null ? ap.Property : prop;

            if (_config.CanClassUseEnums(classe, targetProp))
            {
                hasPropConfig = true;
                w.WriteLine(3, $"modelBuilder.Entity<{fp.Class}>().Property(p => p.{fp.Name}).HasConversion<{fp.Domain.CSharp!.Type}>(){(fp.Domain.Length != null ? $".HasMaxLength({fp.Domain.Length})" : string.Empty)};");
            }

            if (fp.Domain.Length != null && fp.Domain.Scale != null)
            {
                hasPropConfig = true;
                w.WriteLine(3, $"modelBuilder.Entity<{fp.Class}>().Property(x => x.{fp.Name}).HasPrecision({fp.Domain.Length}, {fp.Domain.Scale});");
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

        if (_config.UseEFMigrations)
        {
            var hasFk = false;
            foreach (var (prop, ap) in GetAssociationProperties(classes))
            {
                hasFk = true;
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
            foreach (var classe in classes.Distinct().Where(c => c.Values.Any()).OrderBy(c => c.Name))
            {
                hasData = true;
                w.WriteLine(3, $"modelBuilder.Entity<{classe.Name}>().HasData(");
                foreach (var refValue in classe.Values)
                {
                    if (!_config.UseLatestCSharp)
                    {
                        w.Write("    ");
                    }

                    w.Write($"            new {classe.Name} {{");

                    string WriteEnumValue(Class targetClass, IFieldProperty targetProp, string value)
                    {
                        return $"{(targetClass.Name == targetClass.PluralName ? $"{_config.GetNamespace(targetClass)}.{targetClass.Name}" : targetClass.Name)}.{targetProp}s.{value}";
                    }

                    foreach (var refProp in refValue.Value.ToList())
                    {
                        var prop = refProp.Key is AliasProperty alp ? alp.Property : refProp.Key;
                        var ap = prop as AssociationProperty;

                        var targetClass = ap != null ? ap.Association : prop.Class;
                        var targetProp = ap != null ? ap.Property : prop;

                        var value = _config.CanClassUseEnums(targetClass, targetProp)
                            ? WriteEnumValue(targetClass, targetProp, refProp.Value)
                            : refProp.Key.Domain.CSharp!.Type.Contains("Date")
                            ? $"{refProp.Key.Domain.CSharp.Type.ParseTemplate(refProp.Key).TrimEnd('?')}.Parse(\"{refProp.Value}\"){(refProp.Key.Domain.CSharp.Type.Contains("Time") ? ".ToUniversalTime()" : string.Empty)}"
                            : refProp.Key.Domain.ShouldQuoteSqlValue
                            ? $"\"{refProp.Value}\""
                            : refProp.Value;
                        w.Write($" {refProp.Key.Name} = {value}");
                        if (refValue.Value.ToList().IndexOf(refProp) < refValue.Value.Count - 1)
                        {
                            w.Write(",");
                        }
                    }

                    w.Write(" }");
                    if (classe.Values.IndexOf(refValue) < classe.Values.Count - 1)
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
        }

        w.WriteLine(3, "OnModelCreatingPartial(modelBuilder);");
        w.WriteLine(2, "}");

        if (_config.UseEFMigrations && _config.UseEFComments)
        {
            w.WriteLine();
            w.WriteLine(2, "partial void AddComments(ModelBuilder modelBuilder);");
        }

        w.WriteLine();
        w.WriteLine(2, "partial void OnModelCreatingPartial(ModelBuilder modelBuilder);");

        w.WriteLine(1, "}");
        w.WriteNamespaceEnd();
    }

    private void HandleCommentsFile(string fileName, string dbContextName, string contextNs, IList<string> usings, IList<Class> classes)
    {
        using var cw = new CSharpWriter(fileName, _logger, _config.UseLatestCSharp);

        var cUsings = new List<string>
            {
                "Microsoft.EntityFrameworkCore"
            };

        foreach (var ns in classes.Select(c => _config.GetNamespace(c)).Distinct())
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

    private IEnumerable<(IFieldProperty Property, AssociationProperty AssociationProperty)> GetAssociationProperties(IEnumerable<Class> classes)
    {
        return classes
            .Distinct()
            .OrderBy(c => c.Name)
            .SelectMany(c => c.Properties)
            .Where(p => p is AssociationProperty { Association.IsPersistent: true } || p is AliasProperty { Property: AssociationProperty { Association.IsPersistent: true } })
            .Select(p => p switch
            {
                AssociationProperty ap => ((IFieldProperty)p, ap),
                AliasProperty { Property: AssociationProperty ap } => ((IFieldProperty)p, ap),
                _ => (null!, null!)
            })
            .Where(p => p.ap.Type == AssociationType.ManyToOne || p.ap.Type == AssociationType.OneToOne);
    }
}