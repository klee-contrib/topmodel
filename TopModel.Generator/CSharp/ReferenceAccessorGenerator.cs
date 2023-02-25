using Microsoft.Extensions.Logging;
using TopModel.Core;

namespace TopModel.Generator.CSharp;

public class ReferenceAccessorGenerator : ClassGroupGeneratorBase
{
    private readonly CSharpConfig _config;
    private readonly ILogger<ReferenceAccessorGenerator> _logger;

    public ReferenceAccessorGenerator(ILogger<ReferenceAccessorGenerator> logger, CSharpConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "CSharpRefAccessGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.Reference)
        {
            yield return ("interface", _config.GetReferenceInterfaceFilePath(classe.Namespace, tag));
            yield return ("implementation", _config.GetReferenceImplementationFilePath(classe.Namespace, tag));
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        var classList = classes
            .OrderBy(x => (_config.DbContextPath == null ? $"{x.Name}List" : x.PluralName), StringComparer.Ordinal)
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
        var firstPersistedClass = classList.FirstOrDefault(c => c.IsPersistent);

        var implementationName = _config.GetReferenceAccessorName(ns, tag);
        var implementationNamespace = _config.GetReferenceImplementationNamespace(ns, tag);

        var interfaceName = $"I{implementationName}";
        var interfaceNamespace = _config.GetReferenceInterfaceNamespace(ns, tag);

        using var w = new CSharpWriter(fileName, _logger, _config.UseLatestCSharp);

        var usings = new List<string>();

        if (!_config.UseLatestCSharp)
        {
            usings.Add("System.Collections.Generic");
        }

        if (!implementationNamespace.Contains(interfaceNamespace))
        {
            usings.Add(interfaceNamespace);
        }

        if (firstPersistedClass != null)
        {
            usings.Add(_config.GetNamespace(firstPersistedClass, tag));
        }

        usings.Add("Kinetix.Services.Annotations");

        if (_config.DbContextPath == null)
        {
            usings.Add("Kinetix.DataAccess.Sql.Broker");

            if (classList.Any(classe => classe.OrderProperty != null || classe.DefaultProperty != null && classe.DefaultProperty.Name != "Libelle"))
            {
                usings.Add("Kinetix.DataAccess.Sql");
            }
        }
        else
        {
            if (!_config.UseLatestCSharp)
            {
                usings.Add("System.Linq");
            }

            var contextNs = _config.GetDbContextNamespace(ns, tag);
            if (!implementationNamespace.Contains(contextNs))
            {
                usings.Add(contextNs);
            }
        }

        w.WriteUsings(usings.ToArray());

        w.WriteLine();
        w.WriteNamespace(implementationNamespace);

        w.WriteSummary(1, "This interface was automatically generated. It contains all the operations to load the reference lists declared in module " + ns.Module + ".");
        w.WriteLine(1, "[RegisterImpl]");

        w.WriteClassDeclaration(implementationName, null, interfaceName);

        if (_config.DbContextPath != null)
        {
            var dbContextName = _config.GetDbContextName(ns, tag);

            w.WriteLine(2, $"private readonly {dbContextName} _dbContext;");
            w.WriteLine();
            w.WriteSummary(2, "Constructeur");
            w.WriteParam("dbContext", "DbContext");
            w.WriteLine(2, $"public {implementationName}({dbContextName} dbContext)");
            w.WriteLine(2, "{");
            w.WriteLine(3, "_dbContext = dbContext;");
            w.WriteLine(2, "}");
            w.WriteLine();
        }
        else
        {
            w.WriteLine(2, $"private readonly BrokerManager _brokerManager;");
            w.WriteLine();
            w.WriteSummary(2, "Constructeur");
            w.WriteParam("brokerManager", "BrokerManager");
            w.WriteLine(2, $"public {implementationName}(BrokerManager brokerManager)");
            w.WriteLine(2, "{");
            w.WriteLine(3, "_brokerManager = brokerManager;");
            w.WriteLine(2, "}");
            w.WriteLine();
        }

        foreach (var classe in classList.Where(c => c.IsPersistent || c.Values.Any()))
        {
            var serviceName = "Load" + (_config.DbContextPath == null ? $"{classe.Name}List" : classe.PluralName);
            w.WriteLine(2, "/// <inheritdoc cref=\"" + interfaceName + "." + serviceName + "\" />");
            w.WriteLine(2, "public ICollection<" + classe.Name + "> " + serviceName + "()\r\n{");
            w.WriteLine(3, LoadReferenceAccessorBody(classe));
            w.WriteLine(2, "}");

            if (classList.IndexOf(classe) != classList.Count - 1)
            {
                w.WriteLine();
            }
        }

        w.WriteLine(1, "}");
        w.WriteNamespaceEnd();
    }

    /// <summary>
    /// Génère l'interface déclarant les ReferenceAccessors d'un namespace.
    /// </summary>
    /// <param name="classList">Liste de ModelClass.</param>
    private void GenerateReferenceAccessorsInterface(string fileName, string tag, IEnumerable<Class> classList)
    {
        var ns = classList.First().Namespace;
        var firstPersistedClass = classList.FirstOrDefault(c => c.IsPersistent);

        var interfaceNamespace = _config.GetReferenceInterfaceNamespace(ns, tag);
        var interfaceName = $"I{_config.GetReferenceAccessorName(ns, tag)}";

        using var w = new CSharpWriter(fileName, _logger, _config.UseLatestCSharp);

        var usings = new List<string>();

        if (!_config.UseLatestCSharp)
        {
            usings.Add("System.Collections.Generic");
        }

        if (firstPersistedClass != null)
        {
            usings.Add(_config.GetNamespace(firstPersistedClass, tag));
        }

        usings.Add("Kinetix.Services.Annotations");

        w.WriteUsings(usings.ToArray());

        w.WriteLine();
        w.WriteNamespace(interfaceNamespace);
        w.WriteSummary(1, "This interface was automatically generated. It contains all the operations to load the reference lists declared in module " + ns.Module + ".");
        w.WriteLine(1, "[RegisterContract]");
        w.WriteLine(1, "public partial interface " + interfaceName + "\r\n{");

        var count = 0;
        foreach (var classe in classList)
        {
            count++;
            w.WriteSummary(2, "Reference accessor for type " + classe.Name);
            w.WriteReturns(2, "List of " + classe.Name);
            w.WriteLine(2, "[ReferenceAccessor]");
            w.WriteLine(2, "ICollection<" + classe.Name + "> Load" + (_config.DbContextPath == null ? $"{classe.Name}List" : classe.PluralName) + "();");

            if (count != classList.Count())
            {
                w.WriteLine();
            }
        }

        w.WriteLine(1, "}");
        w.WriteNamespaceEnd();
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
            return $@"return new List<{classe.Name}>
{{
    {string.Join(",\r\n    ", classe.Values.Select(rv => $"new() {{ {string.Join(", ", rv.Value.Select(prop => $"{prop.Key.Name} = {(prop.Key.Domain.ShouldQuoteSqlValue ? $"\"{prop.Value}\"" : prop.Value)}"))} }}"))}
}};";
        }

        var defaultProperty = classe.OrderProperty ?? classe.DefaultProperty;

        var queryParameter = string.Empty;
        if (_config.DbContextPath != null)
        {
            if (defaultProperty != null)
            {
                queryParameter = $".OrderBy(row => row.{defaultProperty.Name})";
            }

            return $"return _dbContext.{classe.PluralName}{queryParameter}.ToList();";
        }
        else
        {
            if (defaultProperty != null)
            {
                queryParameter = "new QueryParameter(" + classe.Name + ".Cols." + defaultProperty.SqlName + ", SortOrder.Asc)";
            }

            return "return _brokerManager.GetBroker<" + classe.Name + ">().GetAll(" + queryParameter + ");";
        }
    }
}