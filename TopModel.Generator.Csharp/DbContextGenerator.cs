using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;

namespace TopModel.Generator.Csharp;

public class DbContextGenerator : ClassGroupGeneratorBase<CsharpConfig>
{
    private readonly ILogger<DbContextGenerator> _logger;

    public DbContextGenerator(ILogger<DbContextGenerator> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override string Name => "CSharpDbContextGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.IsPersistent && !classe.Abstract)
        {
            yield return ("main", Config.GetDbContextFilePath(tag));

            if (Config.UseEFComments)
            {
                yield return ("comments", Config.GetDbContextFilePath(tag).Replace(".cs", ".comments.cs"));
            }
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        var dbContextName = Config.GetDbContextName(tag);
        var usings = new List<string> { "Microsoft.EntityFrameworkCore" };
        var contextNs = Config.GetDbContextNamespace(tag);

        foreach (var ns in classes
            .Concat(GetAssociationProperties(classes).Select(ap => ap.AssociationProperty.Association))
            .Select(c => Config.GetNamespace(c, tag))
            .Distinct())
        {
            usings.Add(ns);
        }

        var classList = classes.OrderBy(c => c.NamePascal).ToList();

        if (fileType == "main")
        {
            HandleMainFile(fileName, tag, dbContextName, contextNs, usings, classList);
        }
        else
        {
            HandleCommentsFile(fileName, tag, dbContextName, contextNs, usings, classList);
        }
    }

    private IEnumerable<(IFieldProperty Property, AssociationProperty AssociationProperty)> GetAssociationProperties(IEnumerable<Class> classes)
    {
        return classes
            .Distinct()
            .OrderBy(c => c.NamePascal)
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

    private void HandleCommentsFile(string fileName, string tag, string dbContextName, string contextNs, IList<string> usings, IList<Class> classes)
    {
        using var cw = new CSharpWriter(fileName, _logger, Config.UseLatestCSharp);

        var cUsings = new List<string>
        {
            "Microsoft.EntityFrameworkCore"
        };

        foreach (var ns in classes.Select(c => Config.GetNamespace(c, tag)).Distinct())
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
            cw.WriteLine(3, $"var {classe.NameCamel} = modelBuilder.Entity<{classe.NamePascal}>();");
            cw.WriteLine(3, $"{classe.NameCamel}.ToTable(t => t.HasComment(\"{classe.Comment.Replace("\"", "\\\"")}\"));");

            foreach (var property in classe.Properties.OfType<IFieldProperty>())
            {
                cw.WriteLine(3, $"{classe.NameCamel}.Property(p => p.{property.NamePascal}).HasComment(\"{property.Comment.Replace("\"", "\\\"")}\");");
            }

            if (classes.IndexOf(classe) < classes.Count - 1)
            {
                cw.WriteLine();
            }
        }

        cw.WriteLine(2, "}");
        cw.WriteLine(1, "}");
    }

    private void HandleMainFile(string fileName, string tag, string dbContextName, string contextNs, IList<string> usings, IList<Class> classes)
    {
        using var w = new CSharpWriter(fileName, _logger, Config.UseLatestCSharp);

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
            w.WriteSummary(2, "Accès à l'entité " + classe.NamePascal);
            w.WriteLine(2, "public DbSet<" + classe.NamePascal + "> " + classe.PluralNamePascal + " { get; set; }");
        }

        w.WriteLine();
        w.WriteSummary(2, "Personalisation du modèle.");
        w.WriteParam("modelBuilder", "L'objet de construction du modèle.");
        w.WriteLine(2, "protected override void OnModelCreating(ModelBuilder modelBuilder)");
        w.WriteLine(2, "{");

        var hasPropConfig = false;
        foreach (var fp in classes.Distinct().OrderBy(c => c.NamePascal).SelectMany(c => c.Properties.OfType<IFieldProperty>()))
        {
            var prop = fp is AliasProperty alp ? alp.Property : fp;
            var ap = prop as AssociationProperty;

            var classe = ap != null ? ap.Association : prop.Class;
            var targetProp = ap != null ? ap.Property : prop;

            if (Config.CanClassUseEnums(classe, Classes, targetProp))
            {
                hasPropConfig = true;
                w.WriteLine(3, $"modelBuilder.Entity<{fp.Class}>().Property(p => p.{fp.NamePascal}).HasConversion<{Config.GetImplementation(fp.Domain)!.Type}>(){(fp.Domain.Length != null ? $".HasMaxLength({fp.Domain.Length})" : string.Empty)};");
            }

            if (fp.Domain.Length != null && fp.Domain.Scale != null)
            {
                hasPropConfig = true;
                w.WriteLine(3, $"modelBuilder.Entity<{fp.Class}>().Property(x => x.{fp.NamePascal}).HasPrecision({fp.Domain.Length}, {fp.Domain.Scale});");
            }
        }

        if (hasPropConfig)
        {
            w.WriteLine();
        }

        var hasPk = false;
        foreach (var classe in classes.Distinct().OrderBy(c => c.NamePascal).Where(c => c.PrimaryKey.Count() > 1))
        {
            hasPk = true;
            w.WriteLine(3, $"modelBuilder.Entity<{classe}>().HasKey(p => new {{ {string.Join(", ", classe.PrimaryKey.Select(pk => $"p.{pk.NamePascal}"))} }});");
        }

        if (hasPk)
        {
            w.WriteLine();
        }

        if (Config.UseEFMigrations)
        {
            var hasFk = false;
            foreach (var (prop, ap) in GetAssociationProperties(classes))
            {
                hasFk = true;
                w.WriteLine(3, $"modelBuilder.Entity<{prop.Class}>().HasOne<{ap.Association}>().With{(ap.Type == AssociationType.ManyToOne ? "Many" : "One")}().HasForeignKey{(ap.Type == AssociationType.ManyToOne ? string.Empty : $"<{prop.Class}>")}(p => p.{prop.NamePascal}).OnDelete(DeleteBehavior.Restrict);");
            }

            if (hasFk)
            {
                w.WriteLine();
            }

            var hasUk = false;
            foreach (var uk in classes.Distinct().OrderBy(c => c.NamePascal).SelectMany(c => c.UniqueKeys))
            {
                hasUk = true;
                var expr = uk.Count == 1 ? $"p.{uk.Single().NamePascal}" : $"new {{ {string.Join(", ", uk.Select(p => $"p.{p.NamePascal}"))} }}";
                w.WriteLine(3, $"modelBuilder.Entity<{uk.First().Class}>().HasIndex(p => {expr}).IsUnique();");
            }

            if (hasUk)
            {
                w.WriteLine();
            }

            var hasData = false;
            foreach (var classe in classes.Distinct().Where(c => c.Values.Any()).OrderBy(c => c.NamePascal))
            {
                hasData = true;
                w.WriteLine(3, $"modelBuilder.Entity<{classe.NamePascal}>().HasData(");
                foreach (var refValue in classe.Values)
                {
                    if (!Config.UseLatestCSharp)
                    {
                        w.Write("    ");
                    }

                    w.Write($"            new {classe.NamePascal} {{");

                    string WriteEnumValue(Class targetClass, IFieldProperty targetProp, string value)
                    {
                        return $"{(targetClass.NamePascal == targetClass.PluralNamePascal ? $"{Config.GetNamespace(targetClass, tag)}.{targetClass.NamePascal}" : targetClass.NamePascal)}.{targetProp}s.{value}";
                    }

                    foreach (var refProp in refValue.Value.ToList())
                    {
                        var prop = refProp.Key is AliasProperty alp ? alp.Property : refProp.Key;
                        var ap = prop as AssociationProperty;

                        var targetClass = ap != null ? ap.Association : prop.Class;
                        var targetProp = ap != null ? ap.Property : prop;

                        var value = Config.CanClassUseEnums(targetClass, Classes, targetProp)
                            ? WriteEnumValue(targetClass, targetProp, refProp.Value)
                            : Config.GetType(refProp.Key).Contains("Date")
                            ? $"{Config.GetType(refProp.Key).TrimEnd('?')}.Parse(\"{refProp.Value}\"){(Config.GetType(refProp.Key).Contains("Time") ? ".ToUniversalTime()" : string.Empty)}"
                            : Config.ShouldQuoteValue(refProp.Key)
                            ? $"\"{refProp.Value}\""
                            : refProp.Value;
                        w.Write($" {refProp.Key.NamePascal} = {value}");
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

            if (Config.UseEFComments)
            {
                w.WriteLine(3, "AddComments(modelBuilder);");
            }
        }

        w.WriteLine(3, "OnModelCreatingPartial(modelBuilder);");
        w.WriteLine(2, "}");

        if (Config.UseEFMigrations && Config.UseEFComments)
        {
            w.WriteLine();
            w.WriteLine(2, "partial void AddComments(ModelBuilder modelBuilder);");
        }

        w.WriteLine();
        w.WriteLine(2, "partial void OnModelCreatingPartial(ModelBuilder modelBuilder);");

        w.WriteLine(1, "}");
        w.WriteNamespaceEnd();
    }
}