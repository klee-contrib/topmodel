using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;

namespace TopModel.Generator.Csharp;

public class ReferenceAccessorGenerator : ClassGroupGeneratorBase<CsharpConfig>
{
    private readonly ILogger<ReferenceAccessorGenerator> _logger;

    public ReferenceAccessorGenerator(ILogger<ReferenceAccessorGenerator> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override string Name => "CSharpRefAccessGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.Reference)
        {
            yield return ("interface", Config.GetReferenceInterfaceFilePath(classe.Namespace, tag));
            if (!Config.NoPersistence(tag) && (classe.IsPersistent || classe.Values.Any()))
            {
                yield return ("implementation", Config.GetReferenceImplementationFilePath(classe.Namespace, tag));
            }
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        var classList = classes
            .OrderBy(x => Config.DbContextPath == null ? $"{x.NamePascal}List" : x.PluralNamePascal, StringComparer.Ordinal)
            .ToList();

        if (fileType == "interface")
        {
            GenerateReferenceAccessorsInterface(fileName, tag, classList);
        }
        else
        {
            GenerateReferenceAccessorsImplementation(fileName, tag, classList);
        }
    }

    /// <summary>
    /// Génère l'implémentation des ReferenceAccessors.
    /// </summary>
    /// <param name="classList">Liste de ModelClass.</param>
    private void GenerateReferenceAccessorsImplementation(string fileName, string tag, List<Class> classList)
    {
        var ns = classList.First().Namespace;

        var implementationName = Config.GetReferenceAccessorName(ns, tag);
        var implementationNamespace = Config.GetReferenceImplementationNamespace(ns, tag);

        var interfaceName = $"I{implementationName}";
        var interfaceNamespace = Config.GetReferenceInterfaceNamespace(ns, tag);

        using var w = new CSharpWriter(fileName, _logger);

        var usings = new HashSet<string>();

        if (!implementationNamespace.StartsWith(interfaceNamespace))
        {
            usings.Add(interfaceNamespace);
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

            if (classList.Any(classe => classe.OrderProperty != null || classe.DefaultProperty != null && classe.DefaultProperty.NamePascal != "Libelle"))
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
        }

        w.WriteUsings(usings.ToArray());

        w.WriteLine();
        w.WriteNamespace(implementationNamespace);

        w.WriteSummary("This interface was automatically generated. It contains all the operations to load the reference lists declared in module " + ns.Module + ".");
        w.WriteLine("[RegisterImpl]");

        w.WriteClassDeclaration(implementationName, null, false, interfaceName);

        if (Config.DbContextPath != null)
        {
            var dbContextName = Config.GetDbContextName(tag);

            w.WriteLine(1, $"private readonly {dbContextName} _dbContext;");
            w.WriteLine();
            w.WriteSummary(1, "Constructeur");
            w.WriteParam("dbContext", "DbContext");
            w.WriteLine(1, $"public {implementationName}({dbContextName} dbContext)");
            w.WriteLine(1, "{");
            w.WriteLine(2, "_dbContext = dbContext;");
            w.WriteLine(1, "}");
            w.WriteLine();
        }
        else
        {
            w.WriteLine(1, $"private readonly BrokerManager _brokerManager;");
            w.WriteLine();
            w.WriteSummary(1, "Constructeur");
            w.WriteParam("brokerManager", "BrokerManager");
            w.WriteLine(1, $"public {implementationName}(BrokerManager brokerManager)");
            w.WriteLine(1, "{");
            w.WriteLine(2, "_brokerManager = brokerManager;");
            w.WriteLine(1, "}");
            w.WriteLine();
        }

        foreach (var classe in classList.Where(c => !Config.NoPersistence(tag) && (c.IsPersistent || c.Values.Any())))
        {
            var serviceName = "Load" + (Config.DbContextPath == null ? $"{classe.NamePascal}List" : classe.PluralNamePascal);
            w.WriteLine(1, "/// <inheritdoc cref=\"" + interfaceName + "." + serviceName + "\" />");
            w.WriteLine(1, "public ICollection<" + classe.NamePascal + "> " + serviceName + "()\r\n{");
            w.WriteLine(2, LoadReferenceAccessorBody(classe));
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
    /// <param name="classList">Liste de ModelClass.</param>
    private void GenerateReferenceAccessorsInterface(string fileName, string tag, IEnumerable<Class> classList)
    {
        var ns = classList.First().Namespace;

        var interfaceNamespace = Config.GetReferenceInterfaceNamespace(ns, tag);
        var interfaceName = $"I{Config.GetReferenceAccessorName(ns, tag)}";

        using var w = new CSharpWriter(fileName, _logger);

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

        w.WriteUsings(usings.ToArray());

        w.WriteLine();
        w.WriteNamespace(interfaceNamespace);
        w.WriteSummary("This interface was automatically generated. It contains all the operations to load the reference lists declared in module " + ns.Module + ".");
        w.WriteLine("[RegisterContract]");
        w.WriteLine("public partial interface " + interfaceName + "\r\n{");

        var count = 0;
        foreach (var classe in classList)
        {
            count++;
            w.WriteSummary(1, "Reference accessor for type " + classe.NamePascal);
            w.WriteReturns(1, "List of " + classe.NamePascal);
            w.WriteLine(1, "[ReferenceAccessor]");
            w.WriteLine(1, "ICollection<" + classe.NamePascal + "> Load" + (Config.DbContextPath == null ? $"{classe.NamePascal}List" : classe.PluralNamePascal) + "();");

            if (count != classList.Count())
            {
                w.WriteLine();
            }
        }

        w.WriteLine("}");
    }

    /// <summary>
    /// Retourne le code associé au corps de l'implémentation d'un service de type ReferenceAccessor.
    /// </summary>
    /// <param name="classe">Type chargé par le ReferenceAccessor.</param>
    /// <returns>Code généré.</returns>
    private string LoadReferenceAccessorBody(Class classe)
    {
        if (!classe.IsPersistent)
        {
            return $@"return new List<{classe.NamePascal}>
{{
    {string.Join(",\r\n    ", classe.Values.Select(rv => $"new() {{ {string.Join(", ", rv.Value.Select(prop => $"{prop.Key.NamePascal} = {Config.GetValue(prop.Key, Classes, prop.Value)}"))} }}"))}
}};";
        }

        var defaultProperty = classe.OrderProperty ?? classe.DefaultProperty;

        var queryParameter = string.Empty;
        if (Config.DbContextPath != null)
        {
            if (defaultProperty != null)
            {
                queryParameter = $".OrderBy(row => row.{defaultProperty.NamePascal})";
            }

            return $"return _dbContext.{classe.PluralNamePascal}{queryParameter}.ToList();";
        }
        else
        {
            if (defaultProperty != null)
            {
                queryParameter = "new QueryParameter(" + classe.NamePascal + ".Cols." + defaultProperty.SqlName + ", SortOrder.Asc)";
            }

            return "return _brokerManager.GetBroker<" + classe.NamePascal + ">().GetAll(" + queryParameter + ");";
        }
    }
}