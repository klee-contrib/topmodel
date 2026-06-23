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
                && !p.IsReverseProperty
                && Config.AvailableClasses.Contains(p.Association)
                && p.Association?.Enum != EnumMode.Enum
                && Config.IsPersistent(p.Association!, Config.GetBestClassTag(p.Association!, tag))
            );
    }

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (
            classe.IsPersistent
            && classe.Type != ClassType.Interface
            && !Config.NoPersistence(tag)
            && classe.Enum != EnumMode.Enum
        )
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

            if (
                classe.Extends?.InheritanceStrategy != InheritanceStrategy.SingleTable
                && (
                    classe.Type != ClassType.Abstract
                    || classe.InheritanceStrategy != InheritanceStrategy.DistinctTables
                )
            )
            {
                cw.WriteLine(
                    2,
                    $"{classe.NameCamel}.ToTable(t => t.HasComment(\"{classe.Comment.Replace("\"", "\\\"")}\"));"
                );
            }

            foreach (var property in classe.Properties.Where(p => !p.AssociationMultiple && !p.IsReverseProperty))
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

        var hasSequence = false;
        foreach (var property in classes.SelectMany(c => c.Properties).Where(p => p.GeneratedValue != null))
        {
            var sequenceName = Config.UseLowerCaseSqlNames
                ? Config.GetSequenceName(property, tag)?.ToLower()
                : Config.GetSequenceName(property, tag);
            if (sequenceName != null)
            {
                hasSequence = true;
                w.WriteLine(
                    2,
                    $"modelBuilder.HasSequence(\"{sequenceName}\").StartsAt({property.GeneratedValue!.Start}).IncrementsBy({property.GeneratedValue.Increment});"
                );

                var classesWithSequence = new List<Class>();
                if (
                    property.Class.Type != ClassType.Abstract
                    || property.Class.InheritanceStrategy != InheritanceStrategy.DistinctTables
                )
                {
                    classesWithSequence.Add(property.Class);
                }

                if (property.Class.InheritanceStrategy == InheritanceStrategy.DistinctTables)
                {
                    classesWithSequence.AddRange(classes.Where(c => c.Extends == property.Class));
                }

                foreach (var classeWithSequence in classesWithSequence)
                {
                    w.Write(2, $"modelBuilder.Entity<{GetClassName(classeWithSequence, tag)}>().Property(");

                    if (property.UseClassForAssociation)
                    {
                        w.Write($"\"{property.PropertyNamePascal}\"");
                    }
                    else
                    {
                        w.Write($"p => p.{property.NamePascal}");
                    }

                    w.WriteLine($").UseHiLo(\"{sequenceName}\");");
                }
            }
            else if (property.GeneratedValue!.Start != 1 || property.GeneratedValue.Increment != 1)
            {
                w.Write(2, $"modelBuilder.Entity<{GetClassName(property.Class, tag)}>().Property(");

                if (property.UseClassForAssociation)
                {
                    w.Write($"\"{property.PropertyNamePascal}\"");
                }
                else
                {
                    w.Write($"p => p.{property.NamePascal}");
                }

                w.WriteLine(
                    $").UseIdentityColumn({property.GeneratedValue.Start}, {property.GeneratedValue.Increment});"
                );
            }
        }

        if (hasSequence)
        {
            w.WriteLine();
        }

        var hasTphOrTpc = false;
        foreach (var classe in classes.Where(c => c.InheritanceStrategy == InheritanceStrategy.SingleTable))
        {
            if (classe.DiscriminatorProperty == null)
            {
                continue;
            }

            hasTphOrTpc = true;
            w.WriteLine(2, $"modelBuilder.Entity<{GetClassName(classe, tag)}>()");
            if (classe.Properties.Contains(classe.DiscriminatorProperty))
            {
                if (classe.DiscriminatorProperty.UseClassForAssociation)
                {
                    w.WriteLine(
                        3,
                        $".HasDiscriminator<{Config.GetType(classe.DiscriminatorProperty, forceAssociationPropertyType: true)}>(\"{classe.DiscriminatorProperty.PropertyNamePascal}\")"
                    );
                }
                else
                {
                    w.WriteLine(3, $".HasDiscriminator(p => p.{classe.DiscriminatorProperty.NamePascal})");
                }
            }
            else
            {
                w.WriteLine(
                    3,
                    $".HasDiscriminator<string>(\"{(Config.UseLowerCaseSqlNames
                    ? classe.DiscriminatorProperty.SqlName.ToLower()
                    : classe.DiscriminatorProperty.SqlName)}\")"
                );
            }

            var subClasses = (classe.Type == ClassType.Regular ? new[] { classe } : [])
                .Concat(classes.Where(c => c.Extends == classe))
                .ToList();

            foreach (var subClasse in subClasses)
            {
                var rawValue = subClasse.DiscriminatorValue ?? subClasse.SqlName;
                w.Write(
                    3,
                    $".HasValue<{GetClassName(subClasse, tag)}>({(classe.DiscriminatorProperty == null
                        ? $"\"{rawValue}\""
                        : GetValue(classe.DiscriminatorProperty, rawValue, tag, contextNs))})"
                );
                w.WriteLine(subClasses[^1] == subClasse ? ";" : string.Empty);
            }
        }

        foreach (var classe in classes.Where(c => c.InheritanceStrategy == InheritanceStrategy.DistinctTables))
        {
            hasTphOrTpc = true;
            w.WriteLine(2, $"modelBuilder.Entity<{GetClassName(classe, tag)}>().UseTpcMappingStrategy();");
        }

        if (hasTphOrTpc)
        {
            w.WriteLine();
        }

        if (Config.UseEFMigrations)
        {
            var hasIndex = false;
            foreach (
                var idx in classes
                    .Distinct()
                    .OrderBy(c => c.NamePascal)
                    .SelectMany(c =>
                        c.Indexes.Where(idx =>
                            idx.Properties.Count > 1
                            || !(
                                idx.Unique
                                && c.Properties.Any(p =>
                                    p.Association != null && p.Unique && p == idx.Properties.Single()
                                )
                            )
                        )
                    )
            )
            {
                hasIndex = true;
                var expr =
                    idx.Properties.Any(p => p.UseClassForAssociation)
                        ? string.Join(", ", idx.Properties.Select(p => $"\"{p.PropertyNamePascal}\""))
                    : idx.Properties.Count == 1 ? $"p => p.{idx.Properties.Single().NamePascal}"
                    : $"p => new {{ {string.Join(", ", idx.Properties.Select(p => $"p.{p.NamePascal}"))} }}";
                w.WriteLine(
                    2,
                    $"modelBuilder.Entity<{GetClassName(idx.Properties[0].Class, tag)}>().HasIndex({expr}){(idx.Unique ? ".IsUnique()" : string.Empty)};"
                );
            }

            if (hasIndex)
            {
                w.WriteLine();
            }

            var hasSp = false;
            foreach (
                var sp in classes
                    .Distinct()
                    .OrderBy(c => c.NamePascal)
                    .SelectMany(c =>
                        c.Properties.Where(p =>
                            !p.AssociationMultiple && !p.IsReverseProperty && p.UseClassForAssociation
                        )
                    )
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

                var hasClassAssociations = classe.Properties.Any(p => p.UseClassForAssociation);

                w.Write(2, $"modelBuilder.Entity<{GetClassName(classe, tag)}>().HasData(");
                if (classe.Enum == EnumMode.Class && classe.Readonly && !hasClassAssociations)
                {
                    w.Write($"{GetClassName(classe, tag)}.Values");
                }
                else
                {
                    foreach (var refValue in classe.Values)
                    {
                        w.WriteLine();
                        w.Write(
                            $"            new {(hasClassAssociations ? string.Empty : $"{GetClassName(classe, tag)} ")}{{"
                        );

                        foreach (var refProp in refValue.Value.ToList())
                        {
                            var value = GetValue(refProp.Key, refProp.Value, tag, contextNs);

                            if (
                                classe.Reference
                                && refProp.Key == classe.DefaultProperty
                                && Config.TranslateReferences == true
                            )
                            {
                                value = $"\"{refValue.ResourceKey}\"";
                            }

                            w.Write($" {refProp.Key.PropertyNamePascal} = {value}");
                            if (refValue.Value.ToList().IndexOf(refProp) < refValue.Value.Count - 1)
                            {
                                w.Write(",");
                            }
                        }

                        w.Write(" }");

                        if (classe.Values.IndexOf(refValue) < classe.Values.Count - 1)
                        {
                            w.Write(",");
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

    private string? GetValue(IProperty property, string rawValue, string tag, string contextNs)
    {
        var targetClass = property.Association ?? property.Class;
        var value = Config.GetValue(property, rawValue);
        if (targetClass != null && value.StartsWith(targetClass.PluralNamePascal))
        {
            value = $"{Config.GetNamespace(targetClass, tag, contextNs)}.{value}";
        }

        if (property.ReadonlyEnumClassAssociation != null)
        {
            value += $".{property.AssociationProperty!.NamePascal}";
        }

        return value;
    }
}
