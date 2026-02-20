using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Csharp;

public class DbContextGenerator(
    ILogger<DbContextGenerator> logger,
    IFileWriterProvider writerProvider,
    TranslationStore translationStore
) : ClassGroupGeneratorBase<CsharpConfig>(logger, writerProvider)
{
    public override string Name => "CSharpDbContextGen";

    protected virtual IEnumerable<IProperty> GetAssociationProperties(IEnumerable<Class> classes, string tag)
    {
        return classes
            .Distinct()
            .OrderBy(c => c.NamePascal)
            .SelectMany(c => c.Properties)
            .Where(p =>
                p is { Association.IsPersistent: true }
                && !p.AssociationMultiple
                && Config.AvailableClasses.Contains(p.Association)
                && p.Association?.Enum != EnumMode.Enum
                && Config.IsPersistent(p.Association!, Config.GetBestClassTag(p.Association!, tag))
            );
    }

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.IsPersistent && !classe.Abstract && !Config.NoPersistence(tag) && classe.Enum != EnumMode.Enum)
        {
            yield return ("main", Config.GetDbContextFilePath(tag));

            if (Config.UseEFComments)
            {
                yield return ("comments", Config.GetDbContextFilePath(tag).Replace(".cs", ".comments.cs"));
            }
        }
    }

    protected virtual void HandleCommentsFile(
        string fileName,
        string tag,
        string dbContextName,
        string contextNs,
        IList<string> usings,
        IList<Class> classes
    )
    {
        using var cw = this.OpenCSharpWriter(fileName);

        cw.AddUsings(usings);

        cw.WriteNamespace(contextNs);

        cw.WriteSummary("Partial pour ajouter les commentaires EF.");
        cw.WriteLine($"public partial class {dbContextName} : DbContext");
        cw.WriteLine("{");
        cw.WriteLine(1, "partial void AddComments(ModelBuilder modelBuilder)");
        cw.WriteLine(1, "{");

        foreach (var classe in classes)
        {
            cw.WriteLine(2, $"var {classe.NameCamel} = modelBuilder.Entity<{GetClassName(classe, tag)}>();");
            cw.WriteLine(
                2,
                $"{classe.NameCamel}.ToTable(t => t.HasComment(\"{classe.Comment.Replace("\"", "\\\"")}\"));"
            );

            foreach (var property in classe.Properties.Where(p => !p.AssociationMultiple))
            {
                cw.WriteLine(
                    2,
                    $"{classe.NameCamel}.Property({(property.NamePascal == property.PropertyNamePascal ? $"p => p.{property.NamePascal}" : $"\"{property.PropertyNamePascal}\"")}).HasComment(\"{property.Comment.Replace("\"", "\\\"")}\");"
                );
            }

            if (classes.IndexOf(classe) < classes.Count - 1)
            {
                cw.WriteLine();
            }
        }

        cw.WriteLine(1, "}");
        cw.WriteLine("}");
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        var dbContextName = Config.GetDbContextName(tag);
        var usings = new List<string> { "Microsoft.EntityFrameworkCore" };
        var contextNs = Config.GetDbContextNamespace(tag);

        foreach (
            var ns in classes
                .Concat(GetAssociationProperties(classes, tag).Select(ap => ap.Association!))
                .Select(c => Config.GetNamespace(c, Config.GetBestClassTag(c, tag)))
                .Distinct()
        )
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

    protected virtual void HandleMainFile(
        string fileName,
        string tag,
        string dbContextName,
        string contextNs,
        IList<string> usings,
        IList<Class> classes
    )
    {
        using var w = this.OpenCSharpWriter(fileName);

        foreach (var value in classes.SelectMany(c => c.Values.SelectMany(v => v.Value)))
        {
            foreach (var import in Config.GetValueImports(value.Key, value.Value))
            {
                usings.Add(import);
            }
        }

        w.AddUsings(usings);

        w.WriteNamespace(contextNs);

        w.WriteSummary("DbContext généré pour Entity Framework Core.");

        var primaryConstructor = Config.DotnetVersion >= 8;

        if (primaryConstructor)
        {
            w.WriteLine(
                $"public partial class {dbContextName}(DbContextOptions<{dbContextName}> options) : DbContext(options)"
            );
            w.WriteLine("{");
        }
        else
        {
            w.WriteLine($"public partial class {dbContextName} : DbContext");
            w.WriteLine("{");

            w.WriteSummary(1, "Constructeur par défaut.");
            w.WriteParam("options", "Options du DbContext.");
            w.WriteLine(1, $"public {dbContextName}(DbContextOptions<{dbContextName}> options)");
            w.WriteLine(2, ": base(options)");
            w.WriteLine(1, "{");
            w.WriteLine(1, "}");
        }

        foreach (var classe in classes)
        {
            if (classes.IndexOf(classe) > 0 || !primaryConstructor)
            {
                w.WriteLine();
            }
            w.WriteSummary(1, "Accès à l'entité " + Config.GetTypeName(classe));
            w.WriteLine(
                1,
                "public DbSet<" + GetClassName(classe, tag) + "> " + classe.PluralNamePascal + " { get; set; }"
            );
        }

        w.WriteLine();
        w.WriteSummary(1, "Personalisation du modèle.");
        w.WriteParam("modelBuilder", "L'objet de construction du modèle.");
        w.WriteLine(1, "protected override void OnModelCreating(ModelBuilder modelBuilder)");
        w.WriteLine(1, "{");

        var hasPropConfig = false;
        foreach (var fp in classes.Distinct().OrderBy(c => c.NamePascal).SelectMany(c => c.Properties))
        {
            if (fp.EnumProperty != null && Config.UniqueValueGeneration.CanEnum)
            {
                hasPropConfig = true;
                if (!fp.UseClassForAssociation || fp.EnumProperty?.Class.Enum == EnumMode.Enum)
                {
                    w.WriteLine(
                        2,
                        $"modelBuilder.Entity<{GetClassName(fp.Class, tag)}>().Property(p => p.{fp.NamePascal}).HasConversion<{Config.GetImplementation(fp.Domain)?.Type ?? string.Empty}>(){(fp.Domain?.Length != null ? $".HasMaxLength({fp.Domain.Length})" : string.Empty)};"
                    );
                }
                else if (fp.Domain?.Length != null)
                {
                    w.WriteLine(
                        2,
                        $"modelBuilder.Entity<{GetClassName(fp.Class, tag)}>().Property(\"{fp.PropertyNamePascal}\").HasMaxLength({fp.Domain.Length});"
                    );
                }
            }

            if (fp.Domain?.Length != null && fp.Domain?.Scale != null)
            {
                hasPropConfig = true;
                w.WriteLine(
                    2,
                    $"modelBuilder.Entity<{GetClassName(fp.Class, tag)}>().Property(x => x.{fp.NamePascal}).HasPrecision({fp.Domain.Length}, {fp.Domain.Scale});"
                );
            }
        }

        if (hasPropConfig)
        {
            w.WriteLine();
        }

        var hasPk = false;
        foreach (
            var classe in classes
                .Distinct()
                .Where(c => c.PrimaryKey.Count() > 1 || c.PrimaryKey.Any(p => p.UseClassForAssociation))
                .OrderBy(c => c.NamePascal)
        )
        {
            hasPk = true;
            var expr = classe.PrimaryKey.Any(p => p.UseClassForAssociation)
                ? string.Join(", ", classe.PrimaryKey.Select(p => $"\"{p.PropertyNamePascal}\""))
                : $"p => new {{ {string.Join(", ", classe.PrimaryKey.Select(p => $"p.{p.NamePascal}"))} }}";
            w.WriteLine(2, $"modelBuilder.Entity<{GetClassName(classe, tag)}>().HasKey({expr});");
        }

        if (hasPk)
        {
            w.WriteLine();
        }

        var hasJson = false;
        foreach (var cp in classes.Distinct().SelectMany(c => c.Properties.Where(p => p is { Composition: not null })))
        {
            hasJson = true;
            var sqlName = Config.UseLowerCaseSqlNames ? cp.SqlName.ToLower() : cp.SqlName;
            w.WriteLine(
                2,
                $@"modelBuilder.Entity<{GetClassName(cp.Class, tag)}>().Owns{(cp.Domain == null ? "One" : "Many")}(p => p.{cp.NamePascal}, p => p.ToJson(""{sqlName}""));"
            );
        }

        if (hasJson)
        {
            w.WriteLine();
        }

        if (Config.UseEFMigrations)
        {
            var hasFk = false;
            foreach (
                var g in GetAssociationProperties(classes, tag)
                    .GroupBy(c => new
                    {
                        c.Class,
                        c.Association,
                        c.Unique,
                        c.AssociationRole,
                        c.UseClassForAssociation,
                    })
            )
            {
                hasFk = true;
                w.Write(2, $"modelBuilder.Entity<{g.Key.Class}>().HasOne");

                if (g.Key.UseClassForAssociation)
                {
                    w.Write($"(p => p.{g.First().NamePascal})");
                }
                else
                {
                    w.Write($"<{GetClassName(g.Key.Association!, tag)}>()");
                }

                w.Write($".With{(!g.Key.Unique ? "Many" : "One")}");

                if (g.Key.UseClassForAssociation && g.Single().ReverseProperty != null)
                {
                    w.Write($"(p => p.{g.Single().ReverseProperty?.NamePascal})");
                }
                else
                {
                    w.Write($"()");
                }

                if (!g.Key.UseClassForAssociation)
                {
                    w.Write(
                        $".HasForeignKey{(!g.Key.Unique ? string.Empty : $"<{GetClassName(g.Key.Class, tag)}>")}(p => {(g.Count() == 1 ? $"p.{g.Single().NamePascal}" : $"new {{ {string.Join(", ", g.Select(p => $"p.{p.NamePascal}"))} }}")})"
                    );
                }
                else if (g.Key.Unique)
                {
                    w.Write($".HasForeignKey<{GetClassName(g.Key.Class, tag)}>(\"{g.Single().PropertyNamePascal}\")");
                }

                w.WriteLine(".OnDelete(DeleteBehavior.Restrict);");
            }

            if (hasFk)
            {
                w.WriteLine();
            }

            var hasUk = false;
            foreach (
                var uk in classes
                    .Distinct()
                    .OrderBy(c => c.NamePascal)
                    .SelectMany(c =>
                        c.UniqueKeys.Where(uk =>
                            uk.Count > 1
                            || !c.Properties.Any(p => p.Association != null && p.Unique && p == uk.Single())
                        )
                    )
            )
            {
                hasUk = true;
                var expr =
                    uk.Any(p => p.UseClassForAssociation)
                        ? string.Join(", ", uk.Select(p => $"\"{p.PropertyNamePascal}\""))
                    : uk.Count == 1 ? $"p => p.{uk.Single().NamePascal}"
                    : $"p => new {{ {string.Join(", ", uk.Select(p => $"p.{p.NamePascal}"))} }}";
                w.WriteLine(2, $"modelBuilder.Entity<{GetClassName(uk[0].Class, tag)}>().HasIndex({expr}).IsUnique();");
            }

            if (hasUk)
            {
                w.WriteLine();
            }

            var hasSp = false;
            foreach (
                var sp in classes
                    .Distinct()
                    .OrderBy(c => c.NamePascal)
                    .SelectMany(c => c.Properties.Where(p => !p.AssociationMultiple && p.UseClassForAssociation))
            )
            {
                hasSp = true;
                w.WriteLine(
                    2,
                    $"modelBuilder.Entity<{GetClassName(sp.Class, tag)}>().Property(\"{sp.PropertyNamePascal}\").HasColumnName(\"{(Config.UseLowerCaseSqlNames ? sp.SqlName.ToLower() : sp.SqlName)}\");"
                );
            }

            if (hasSp)
            {
                w.WriteLine();
            }

            var hasResourceIndex = false;
            if (Config.PersistedReferencesResources && Config.AvailableClasses.Any(c => c.Translation))
            {
                foreach (
                    var translationClass in Config.AvailableClasses.Where(c =>
                        c.Translation && c.LocaleProperty != null
                    )
                )
                {
                    hasResourceIndex = true;
                    w.WriteLine(
                        2,
                        $"modelBuilder.Entity<{GetClassName(translationClass, tag)}>().HasIndex(p => p.{translationClass.PrimaryKey.Single(p => p != translationClass.LocaleProperty).NamePascal});"
                    );
                }

                var resourceProperties = classes
                    .Where(c => c.DefaultProperty != null && c.Values.Count > 0 && c.Enum != null)
                    .OrderBy(c => c.SqlName)
                    .Select(c => c.DefaultProperty!);

                foreach (var fkProperty in resourceProperties)
                {
                    hasResourceIndex = true;
                    w.WriteLine(
                        2,
                        $"modelBuilder.Entity<{GetClassName(fkProperty.Class, tag)}>().HasIndex(p => p.{fkProperty.NamePascal});"
                    );
                }
            }

            if (hasResourceIndex)
            {
                w.WriteLine();
            }

            var hasData = false;
            foreach (var classe in classes.Distinct().Where(c => c.Values.Count > 0).OrderBy(c => c.NamePascal))
            {
                hasData = true;
                w.Write(2, $"modelBuilder.Entity<{GetClassName(classe, tag)}>().HasData(");
                foreach (var refValue in classe.Values)
                {
                    if (classe.Enum == EnumMode.Class && classe.Readonly)
                    {
                        w.Write($"{GetClassName(classe, tag)}.{refValue.Name.ToPascalCase(strictIfUppercase: true)}");

                        if (classe.Values.IndexOf(refValue) < classe.Values.Count - 1)
                        {
                            w.Write(", ");
                        }
                    }
                    else
                    {
                        w.WriteLine();
                        w.Write($"            new {GetClassName(classe, tag)} {{");

                        foreach (var refProp in refValue.Value.ToList())
                        {
                            var targetClass = refProp.Key.Association ?? refProp.Key.Class;

                            var value = Config.GetValue(refProp.Key, refProp.Value);
                            if (targetClass != null && value.StartsWith(targetClass.PluralNamePascal))
                            {
                                value = $"{Config.GetNamespace(targetClass, tag, contextNs)}.{value}";
                            }

                            if (
                                classe.Reference
                                && refProp.Key == classe.DefaultProperty
                                && Config.TranslateReferences == true
                            )
                            {
                                value = $"\"{refValue.ResourceKey}\"";
                            }

                            w.Write($" {refProp.Key.NamePascal} = {value}");
                            if (refValue.Value.ToList().IndexOf(refProp) < refValue.Value.Count - 1)
                            {
                                w.Write(",");
                            }
                        }

                        w.Write(" }");

                        if (classe.Values.IndexOf(refValue) < classe.Values.Count - 1)
                        {
                            w.WriteLine(",");
                        }
                    }
                }

                w.WriteLine(");");
            }

            if (hasData)
            {
                w.WriteLine();
            }

            if (Config.UseEFComments)
            {
                w.WriteLine(2, "AddComments(modelBuilder);");
            }

            if (
                (Config.PersistedPropertiesResources || Config.PersistedReferencesResources)
                && Config.AvailableClasses.Any(c => c.Translation)
            )
            {
                foreach (var lang in translationStore.Translations.Keys)
                {
                    w.WriteLine(2, $"Add{lang.ToPascalCase()}Resources(modelBuilder);");
                }
            }
        }

        w.WriteLine(2, "OnModelCreatingPartial(modelBuilder);");
        w.WriteLine(1, "}");

        if (Config.UseEFMigrations && Config.UseEFComments)
        {
            w.WriteLine();
            w.WriteLine(1, "partial void AddComments(ModelBuilder modelBuilder);");
        }

        if (
            Config.UseEFMigrations
            && (Config.PersistedPropertiesResources || Config.PersistedReferencesResources)
            && Config.AvailableClasses.Any(c => c.Translation)
        )
        {
            foreach (var lang in translationStore.Translations.Keys.Order(StringComparer.Ordinal))
            {
                w.WriteLine();
                w.WriteLine(1, $"partial void Add{lang.ToPascalCase()}Resources(ModelBuilder modelBuilder);");
            }
        }

        w.WriteLine();
        w.WriteLine(1, "partial void OnModelCreatingPartial(ModelBuilder modelBuilder);");

        w.WriteLine("}");
    }

    private string GetClassName(Class classe, string tag)
    {
        var classNs = Config.GetNamespace(classe, tag);
        if (classNs.Split(".").Contains(Config.GetTypeName(classe)))
        {
            return $"{Config.GetNamespace(classe, tag, Config.GetDbContextNamespace(tag))}.{Config.GetTypeName(classe)}";
        }
        else
        {
            return Config.GetTypeName(classe);
        }
    }
}
