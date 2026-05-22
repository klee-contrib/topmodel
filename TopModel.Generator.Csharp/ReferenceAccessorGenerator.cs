using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Csharp;

public class ReferenceAccessorGenerator(ILogger<ReferenceAccessorGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGroupGeneratorBase<CsharpConfig>(logger, writerProvider)
{
    public override string Name => "CSharpRefAccessGen";

    private bool PrimaryConstructor => Config.DotnetVersion >= 8;

    /// <summary>
    /// Génère l'implémentation des ReferenceAccessors.
    /// </summary>
    /// <param name="fileName">Nom du fichier cible.</param>
    /// <param name="tag">Tag du fichier cible.</param>
    /// <param name="classList">Liste de classes à générer.</param>
    protected virtual void GenerateReferenceAccessorsImplementation(string fileName, string tag, IList<Class> classList)
    {
        var ns = classList[0].Namespace;

        var implementationName = $"Db{Config.GetReferenceAccessorName(ns, tag)}";
        var implementationNamespace = Config.GetReferenceImplementationNamespace(ns, tag);

        var interfaceName = $"I{implementationName}";
        var interfaceNamespace = Config.GetReferenceInterfaceNamespace(ns, tag);

        using var w = this.OpenCSharpWriter(fileName);

        var usings = new HashSet<string>();

        if (!implementationNamespace.StartsWith(interfaceNamespace))
        {
            usings.Add(interfaceNamespace);
        }

        if (
            Config.PersistedReferencesResources
            && Config.DbContextPath != null
            && Config.AvailableClasses.Any(c => c.Translation && c.LocaleProperty != null)
            && classList.Any(c => c.DefaultProperty != null)
        )
        {
            usings.Add("System.Globalization");
        }

        foreach (var classe in classList)
        {
            var classNs = Config.GetNamespace(classe, tag);
            if (!implementationNamespace.StartsWith(classNs))
            {
                usings.Add(classNs);
            }

            if (!classe.IsPersistent)
            {
                foreach (var value in classe.Values.SelectMany(v => v.Value))
                {
                    foreach (var @using in Config.GetValueImports(value.Key, value.Value))
                    {
                        usings.Add(@using);
                    }
                }
            }
        }

        usings.Add("Kinetix.Services.Annotations");

        if (Config.DbContextPath == null)
        {
            usings.Add("Kinetix.DataAccess.Sql.Broker");

            if (
                classList.Any(classe =>
                    classe.OrderProperty != null
                    || classe.DefaultProperty != null && classe.DefaultProperty.NamePascal != "Libelle"
                )
            )
            {
                usings.Add("Kinetix.DataAccess.Sql");
            }
        }
        else
        {
            var contextNs = Config.GetDbContextNamespace(tag);
            if (!implementationNamespace.Contains(contextNs))
            {
                usings.Add(contextNs);
            }

            if (Config.UseAsyncReferenceAccessors)
            {
                usings.Add("Microsoft.EntityFrameworkCore");
            }
        }

        w.AddUsings(usings);

        w.WriteNamespace(implementationNamespace);

        w.WriteSummary($"Implémentation de {interfaceName}.");

        if (PrimaryConstructor)
        {
            if (Config.DbContextPath != null)
            {
                w.WriteParam("dbContext", "DbContext", 0);
            }
            else
            {
                w.WriteParam("brokerManager", "BrokerManager", 0);
            }
        }

        w.WriteLine("[RegisterImpl]");

        if (
            classList.All(c =>
                !c.IsPersistent || c.Enum == EnumMode.Class && c.Readonly && !Config.PersistedReferencesResources
            )
        )
        {
            w.WriteClassDeclaration(implementationName, inheritedClass: null, isRecord: false, ifList: [interfaceName]);
        }
        else if (Config.DbContextPath != null)
        {
            var dbContextName = Config.GetDbContextName(tag);
            var parameters = $"{dbContextName} dbContext";

            w.WriteClassDeclaration(
                implementationName,
                inheritedClass: null,
                isRecord: false,
                ifList: [interfaceName],
                parameters: PrimaryConstructor ? parameters : null
            );
            if (!PrimaryConstructor)
            {
                w.WriteLine(1, $"private readonly {dbContextName} _dbContext;");
                w.WriteLine();
                w.WriteSummary(1, "Constructeur");
                w.WriteParam("dbContext", "DbContext");
                w.WriteLine(1, $"public {implementationName}({parameters})");
                w.WriteLine(1, "{");
                w.WriteLine(2, "_dbContext = dbContext;");
                w.WriteLine(1, "}");
                w.WriteLine();
            }
        }
        else
        {
            var parameters = $"BrokerManager brokerManager";

            w.WriteClassDeclaration(
                implementationName,
                inheritedClass: null,
                isRecord: false,
                ifList: [interfaceName],
                parameters: PrimaryConstructor ? parameters : null
            );
            if (!PrimaryConstructor)
            {
                w.WriteLine(1, $"private readonly BrokerManager _brokerManager;");
                w.WriteLine();
                w.WriteSummary(1, "Constructeur");
                w.WriteParam("brokerManager", "BrokerManager");
                w.WriteLine(1, $"public {implementationName}({parameters})");
                w.WriteLine(1, "{");
                w.WriteLine(2, "_brokerManager = brokerManager;");
                w.WriteLine(1, "}");
                w.WriteLine();
            }
        }

        foreach (
            var classe in classList.Where(c => !Config.NoPersistence(tag) && (c.IsPersistent || c.Values.Count > 0))
        )
        {
            var serviceName =
                "Load" + (Config.DbContextPath == null ? $"{classe.NamePascal}List" : classe.PluralNamePascal);
            w.WriteLine(1, "/// <inheritdoc cref=\"" + interfaceName + "." + serviceName + "\" />");
            w.WriteLine(
                1,
                $"public {(Config.UseAsyncReferenceAccessors ? "async Task<" : string.Empty)}ICollection<"
                    + Config.GetTypeName(classe)
                    + $">{(Config.UseAsyncReferenceAccessors ? ">" : string.Empty)} "
                    + serviceName
                    + $"({(Config.UseAsyncReferenceAccessors ? "CancellationToken ct = default" : string.Empty)}){Environment.NewLine}{{"
            );
            WriteReferenceAccessorBody(w, classe);
            w.WriteLine(1, "}");

            if (classList.IndexOf(classe) != classList.Count - 1)
            {
                w.WriteLine();
            }
        }

        w.WriteLine("}");
    }

    /// <summary>
    /// Génère l'interface déclarant les ReferenceAccessors d'un namespace.
    /// </summary>
    /// <param name="fileType">Type d'interface (persistée ou pas).</param>
    /// <param name="fileName">Nom du fichier cible.</param>
    /// <param name="tag">Tag du fichier cible.</param>
    /// <param name="classList">Liste de classes à générer.</param>
    protected virtual void GenerateReferenceAccessorsInterface(
        string fileType,
        string fileName,
        string tag,
        IEnumerable<Class> classList
    )
    {
        var ns = classList.First().Namespace;

        var interfaceNamespace = Config.GetReferenceInterfaceNamespace(ns, tag);
        var interfaceName =
            $"I{(fileType.StartsWith("db") ? "Db" : string.Empty)}{Config.GetReferenceAccessorName(ns, tag)}";

        using var w = this.OpenCSharpWriter(fileName);

        var usings = new HashSet<string>();

        foreach (var classe in classList)
        {
            var classNs = Config.GetNamespace(classe, tag);
            if (!interfaceNamespace.StartsWith(classNs))
            {
                usings.Add(classNs);
            }
        }

        usings.Add("Kinetix.Services.Annotations");

        w.AddUsings(usings);

        w.WriteNamespace(interfaceNamespace);
        w.WriteSummary(
            $"Accesseurs de listes de référence {(fileType.StartsWith("db") ? "persistées" : "non persistées")}"
        );
        w.WriteLine("[RegisterContract]");
        w.WriteLine($"public partial interface {interfaceName}");
        w.WriteLine("{");

        var count = 0;
        foreach (var classe in classList)
        {
            count++;
            w.WriteSummary(1, $"Accesseur de référence pour le type {Config.GetTypeName(classe)}");
            if (Config.UseAsyncReferenceAccessors)
            {
                w.WriteParam("ct", "CancellationToken");
            }
            w.WriteReturns(1, $"Liste de {Config.GetTypeName(classe)}");
            w.WriteLine(1, "[ReferenceAccessor]");
            w.WriteLine(
                1,
                $"{(Config.UseAsyncReferenceAccessors ? "Task<" : string.Empty)}ICollection<"
                    + Config.GetTypeName(classe)
                    + $">{(Config.UseAsyncReferenceAccessors ? ">" : string.Empty)} Load"
                    + (Config.DbContextPath == null ? $"{classe.NamePascal}List" : classe.PluralNamePascal)
                    + $"({(Config.UseAsyncReferenceAccessors ? "CancellationToken ct = default" : string.Empty)});"
            );

            if (count != classList.Count())
            {
                w.WriteLine();
            }
        }

        w.WriteLine("}");
    }

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.Reference && classe.Enum != EnumMode.Enum)
        {
            if (!Config.NoPersistence(tag) && (classe.IsPersistent || classe.Values.Count > 0))
            {
                yield return ("db-interface", Config.GetReferenceInterfaceFilePath(classe.Namespace, tag, "Db"));
                yield return ("db-implementation", Config.GetReferenceImplementationFilePath(classe.Namespace, tag));
            }
            else
            {
                yield return ("interface", Config.GetReferenceInterfaceFilePath(classe.Namespace, tag));
            }
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        var classList = classes
            .OrderBy(
                x => Config.DbContextPath == null ? $"{x.NamePascal}List" : x.PluralNamePascal,
                StringComparer.Ordinal
            )
            .ToList();

        if (fileType == "db-implementation")
        {
            GenerateReferenceAccessorsImplementation(fileName, tag, classList);
        }
        else
        {
            GenerateReferenceAccessorsInterface(fileType, fileName, tag, classList);
        }
    }

    /// <summary>
    /// Retourne le code associé au corps de l'implémentation d'un service de type ReferenceAccessor.
    /// </summary>
    /// <param name="w">Writer.</param>
    /// <param name="classe">Type chargé par le ReferenceAccessor.</param>
    /// <returns>Code généré.</returns>
    protected virtual void WriteReferenceAccessorBody(CSharpWriter w, Class classe)
    {
        if (classe.Enum == EnumMode.Class && classe.Readonly && !Config.PersistedReferencesResources)
        {
            w.WriteLine(2, $"return {classe.NamePascal}.Values;");
            return;
        }
        else if (!classe.IsPersistent)
        {
            w.WriteLine(
                2,
                $@"return new List<{Config.GetTypeName(classe)}>
{{
    {string.Join($",{Environment.NewLine}    ", classe.Values.Select(rv => $"new() {{ {string.Join(", ", rv.Value.Select(prop => $"{prop.Key.NamePascal} = {Config.GetValue(prop.Key, prop.Value)}"))} }}"))}
}};"
            );
            return;
        }

        var defaultProperty = classe.OrderProperty ?? classe.DefaultProperty;

        var queryParameter = string.Empty;
        var dbContext = $"{(PrimaryConstructor ? string.Empty : "_")}dbContext";
        if (Config.DbContextPath != null)
        {
            if (Config.PersistedReferencesResources)
            {
                var translationClass = Config.AvailableClasses.FirstOrDefault(c => c.Translation);
                if (translationClass != null && classe.DefaultProperty != null)
                {
                    w.WriteLine(2, $"return {(Config.UseAsyncReferenceAccessors ? "await " : string.Empty)}(");
                    w.WriteLine(3, $"from row in {dbContext}.{classe.PluralNamePascal}");
                    w.Write(3, $"join tra in {dbContext}.{translationClass.PluralNamePascal} on ");

                    if (translationClass.LocaleProperty != null)
                    {
                        var rk = translationClass
                            .PrimaryKey.Single(p => p != translationClass.LocaleProperty)
                            .NamePascal;
                        w.WriteLine(
                            $"new {{ {rk} = row.{classe.DefaultProperty.NamePascal}, {translationClass.LocaleProperty.NamePascal} = CultureInfo.CurrentUICulture.Name }} equals new {{ tra.{rk}, tra.{translationClass.LocaleProperty.NamePascal} }}"
                        );
                    }
                    else
                    {
                        w.WriteLine(
                            $"row.{classe.DefaultProperty.NamePascal} equals tra.{translationClass.PrimaryKey.Single()}"
                        );
                    }

                    w.WriteLine(3, $"orderby row.{defaultProperty!.NamePascal}");
                    w.WriteLine(3, $"select new {Config.GetTypeName(classe)}");
                    w.WriteLine(3, "{");

                    foreach (var prop in classe.Properties)
                    {
                        w.Write(
                            4,
                            $"{prop.NamePascal} = {(prop == classe.DefaultProperty ? $"tra.{translationClass.DefaultProperty!.NamePascal}" : $"row.{prop.NamePascal}")}"
                        );
                        w.WriteLine(prop == classe.Properties[^1] ? string.Empty : ",");
                    }

                    w.WriteLine(3, "}");
                    if (Config.UseAsyncReferenceAccessors)
                    {
                        w.WriteLine(2, ").ToListAsync(ct);");
                    }
                    else
                    {
                        w.WriteLine(2, ").ToList();");
                    }
                    return;
                }
            }

            if (defaultProperty != null)
            {
                queryParameter = $".OrderBy(row => row.{defaultProperty.NamePascal})";
            }

            w.WriteLine(
                2,
                $"return {(Config.UseAsyncReferenceAccessors ? "await " : string.Empty)}{dbContext}.{classe.PluralNamePascal}{queryParameter}.ToList{(Config.UseAsyncReferenceAccessors ? "Async(ct)" : "()")};"
            );
        }
        else
        {
            if (defaultProperty != null)
            {
                queryParameter =
                    $"new QueryParameter({Config.GetTypeName(classe)}.Cols.{defaultProperty.SqlName}, SortOrder.Asc)";
            }

            w.WriteLine(
                2,
                $"return {(Config.UseAsyncReferenceAccessors ? "await " : string.Empty)}{(PrimaryConstructor ? string.Empty : "_")}brokerManager.GetBroker<{Config.GetTypeName(classe)}>().GetAll({queryParameter}{(Config.UseAsyncReferenceAccessors ? ", ct" : string.Empty)});"
            );
        }
    }
}
