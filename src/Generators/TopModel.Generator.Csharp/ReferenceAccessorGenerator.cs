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
    /// <param name="classes">Classes à générer.</param>
    protected virtual async Task GenerateReferenceAccessorsImplementation(
        string fileName,
        string tag,
        IList<Class> classes
    )
    {
        var ns = classes[0].Namespace;

        var implementationName = $"Db{Config.GetReferenceAccessorName(ns, tag)}";
        var implementationNamespace = Config.GetReferenceImplementationNamespace(ns, tag);

        var interfaceName = $"I{implementationName}";
        var interfaceNamespace = Config.GetReferenceInterfaceNamespace(ns, tag);

        await using var w = this.OpenCSharpWriter(fileName);

        w.WriteNamespace(implementationNamespace);

        w.AddUsing("Kinetix.Services.Annotations");
        w.AddUsing(interfaceNamespace);

        w.WriteSummary(Config.GetMessage(CsharpMessage.ImplementationOf, interfaceName));

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
            classes.All(c =>
                !c.IsPersistent || c.Enum == EnumMode.Class && c.Readonly && !Config.PersistedReferencesResources
            )
        )
        {
            w.WriteClassDeclaration(implementationName, inheritedClass: null, isRecord: false, ifList: [interfaceName]);
        }
        else if (Config.DbContextPath != null)
        {
            w.AddUsing(Config.GetDbContextNamespace(tag, ns.Module));

            var dbContextName = Config.GetDbContextName(tag, ns.Module);
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
                w.WriteSummary(1, Config.GetMessage(CsharpMessage.DefaultConstructor).TrimEnd('.'));
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
            w.AddUsing("Kinetix.DataAccess.Sql.Broker");

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
                w.WriteSummary(1, Config.GetMessage(CsharpMessage.DefaultConstructor).TrimEnd('.'));
                w.WriteParam("brokerManager", "BrokerManager");
                w.WriteLine(1, $"public {implementationName}({parameters})");
                w.WriteLine(1, "{");
                w.WriteLine(2, "_brokerManager = brokerManager;");
                w.WriteLine(1, "}");
                w.WriteLine();
            }
        }

        foreach (var classe in classes.Where(c => !Config.NoPersistence(tag) && (c.IsPersistent || c.Values.Count > 0)))
        {
            w.AddUsing(Config.GetNamespace(classe, tag));

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
            WriteReferenceAccessorBody(w, classe, tag);
            w.WriteLine(1, "}");

            if (classes.IndexOf(classe) != classes.Count - 1)
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
    /// <param name="classes">Classes à générer.</param>
    protected virtual async Task GenerateReferenceAccessorsInterface(
        string fileType,
        string fileName,
        string tag,
        IEnumerable<Class> classes
    )
    {
        var ns = classes.First().Namespace;

        var interfaceNamespace = Config.GetReferenceInterfaceNamespace(ns, tag);
        var interfaceName =
            $"I{(fileType.StartsWith("db") ? "Db" : string.Empty)}{Config.GetReferenceAccessorName(ns, tag)}";

        await using var w = this.OpenCSharpWriter(fileName);

        w.WriteNamespace(interfaceNamespace);

        w.AddUsing("Kinetix.Services.Annotations");

        w.WriteSummary(
            Config.GetMessage(
                fileType.StartsWith("db")
                    ? CsharpMessage.PersistentReferenceListAccessors
                    : CsharpMessage.NonPersistentReferenceListAccessors
            )
        );
        w.WriteLine("[RegisterContract]");
        w.WriteLine($"public partial interface {interfaceName}");
        w.WriteLine("{");

        var count = 0;
        foreach (var classe in classes)
        {
            w.AddUsing(Config.GetNamespace(classe, tag));

            count++;
            w.WriteSummary(1, Config.GetMessage(CsharpMessage.ReferenceAccessor, Config.GetTypeName(classe)));
            if (Config.UseAsyncReferenceAccessors)
            {
                w.WriteParam("ct", "CancellationToken");
            }
            w.WriteReturns(1, Config.GetMessage(CsharpMessage.ListOf, Config.GetTypeName(classe)));
            w.WriteLine(1, "[ReferenceAccessor]");
            w.WriteLine(
                1,
                $"{(Config.UseAsyncReferenceAccessors ? "Task<" : string.Empty)}ICollection<"
                    + Config.GetTypeName(classe)
                    + $">{(Config.UseAsyncReferenceAccessors ? ">" : string.Empty)} Load"
                    + (Config.DbContextPath == null ? $"{classe.NamePascal}List" : classe.PluralNamePascal)
                    + $"({(Config.UseAsyncReferenceAccessors ? "CancellationToken ct = default" : string.Empty)});"
            );

            if (count != classes.Count())
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

    protected override async Task HandleFile(
        string fileType,
        string fileName,
        string tag,
        IEnumerable<Class> classes,
        CancellationToken ct = default
    )
    {
        var orderedClasses = classes
            .OrderBy(
                classe => Config.DbContextPath == null ? $"{classe.NamePascal}List" : classe.PluralNamePascal,
                StringComparer.Ordinal
            )
            .ToList();

        if (fileType == "db-implementation")
        {
            await GenerateReferenceAccessorsImplementation(fileName, tag, orderedClasses);
        }
        else
        {
            await GenerateReferenceAccessorsInterface(fileType, fileName, tag, orderedClasses);
        }
    }

    /// <summary>
    /// Retourne le code associé au corps de l'implémentation d'un service de type ReferenceAccessor.
    /// </summary>
    /// <param name="w">Writer.</param>
    /// <param name="classe">Type chargé par le ReferenceAccessor.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteReferenceAccessorBody(CSharpWriter w, Class classe, string tag)
    {
        if (classe.Enum == EnumMode.Class && classe.Readonly && !Config.PersistedReferencesResources)
        {
            w.WriteLine(2, $"return {classe.NamePascal}.Values;");
            return;
        }
        else if (!classe.IsPersistent)
        {
            foreach (var value in classe.Values.SelectMany(v => v.Value))
            {
                w.AddUsings(Config.GetValueImports(value.Key, tag, value.Value));
            }

            w.WriteLine(
                2,
                $@"return new List<{Config.GetTypeName(classe)}>
{{
    {string.Join($",{Environment.NewLine}    ", classe.Values.Select(rv => $"new() {{ {string.Join(", ", rv.Value.Select(prop => $"{prop.Key.NamePascal} = {Config.GetValue(prop.Key, tag, prop.Value)}"))} }}"))}
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
                        w.AddUsing("System.Globalization");

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

                    foreach (var prop in Config.GetProperties(classe))
                    {
                        w.Write(
                            4,
                            $"{prop.NamePascal} = {(prop == classe.DefaultProperty ? $"tra.{translationClass.DefaultProperty!.NamePascal}" : $"row.{prop.NamePascal}")}"
                        );
                        w.WriteLine(prop == Config.GetProperties(classe).Last() ? string.Empty : ",");
                    }

                    w.WriteLine(3, "}");
                    if (Config.UseAsyncReferenceAccessors)
                    {
                        w.AddUsing("Microsoft.EntityFrameworkCore");
                        w.WriteLine(2, ").ToListAsync(ct);");
                    }
                    else
                    {
                        w.WriteLine(2, ").ToList();");
                    }
                    return;
                }
            }

            w.AddUsing("Microsoft.EntityFrameworkCore");

            if (defaultProperty != null)
            {
                queryParameter = $".OrderBy(row => row.{defaultProperty.NamePascal})";
            }

            w.WriteLine(
                2,
                $"return {(Config.UseAsyncReferenceAccessors ? "await " : string.Empty)}{dbContext}.{classe.PluralNamePascal}.AsNoTracking(){queryParameter}.ToList{(Config.UseAsyncReferenceAccessors ? "Async(ct)" : "()")};"
            );
        }
        else
        {
            if (defaultProperty != null)
            {
                w.AddUsing("Kinetix.DataAccess.Sql");
                queryParameter =
                    $"new QueryParameter({Config.GetTypeName(classe)}.Cols.{defaultProperty.SqlName}, SortOrder.Asc)";
            }

            w.WriteLine(
                2,
                $"return {(Config.UseAsyncReferenceAccessors ? "await " : string.Empty)}{(PrimaryConstructor ? string.Empty : "_")}brokerManager.GetBroker<{Config.GetTypeName(classe)}>().GetAll{(Config.UseAsyncReferenceAccessors ? "Async" : string.Empty)}({queryParameter}{(Config.UseAsyncReferenceAccessors ? $"{(defaultProperty != null ? ", " : "ct: ")}ct" : string.Empty)});"
            );
        }
    }
}
