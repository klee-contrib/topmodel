using Microsoft.Extensions.Logging;
using TopModel.Core;

namespace TopModel.Generator.CSharp;
public class ReferenceAccessorGenerator
{
    private readonly CSharpConfig _config;
    private readonly ILogger<CSharpGenerator> _logger;

    public ReferenceAccessorGenerator(CSharpConfig config, ILogger<CSharpGenerator> logger)
    {
        _config = config;
        _logger = logger;
    }

    /// <summary>
    /// Génère les ReferenceAccessor pour un namespace.
    /// </summary>
    /// <param name="classes">Classes.</param>
    public void Generate(IEnumerable<Class> classes)
    {
        var classList = classes
            .OrderBy(x => x.PluralName, StringComparer.Ordinal)
            .ToList();

        if (!classList.Any() || _config.Kinetix == KinetixVersion.None)
        {
            return;
        }

        GenerateReferenceAccessorsInterface(classList);
        GenerateReferenceAccessorsImplementation(classList);
    }

    /// <summary>
    /// Génère l'implémentation des ReferenceAccessors.
    /// </summary>
    /// <param name="classList">Liste de ModelClass.</param>
    private void GenerateReferenceAccessorsImplementation(List<Class> classList)
    {
        var firstClass = classList.First();

        string projectName;
        string implementationName;

        if (_config.DbContextPath != null)
        {
            projectName = _config.DbContextPath.Split('/').Last();
            implementationName = $"{firstClass.Namespace.Module}AccessorsDal";
        }
        else
        {
            projectName = $"{firstClass.Namespace.App}.{firstClass.Namespace.Module}Implementation";
            implementationName = $"Service{firstClass.Namespace.Module}Accessors";
        }

        var interfaceName = $"I{implementationName}";

        var implementationFileName = _config.GetReferenceImplementationFilePath(classList)!;

        using var w = new CSharpWriter(implementationFileName, _logger, _config.UseLatestCSharp);

        var usings = new List<string>();

        if (!_config.UseLatestCSharp)
        {
            usings.Add("System.Collections.Generic");
        }

        usings.Add(_config.GetNamespace(firstClass));

        if (_config.Kinetix == KinetixVersion.Core)
        {
            usings.Add("Kinetix.Services.Annotations");
        }
        else
        {
            usings.Add("System.ServiceModel");
        }

        if (_config.DbContextPath == null)
        {
            usings.Add(_config.GetNamespace(firstClass).Replace("DataContract", "Contract"));
            usings.Add("Kinetix.DataAccess.Sql.Broker");

            if (classList.Any(classe => classe.OrderProperty != null || classe.DefaultProperty != null && classe.DefaultProperty.Name != "Libelle"))
            {
                usings.Add("Kinetix.DataAccess.Sql");
            }
        }
        else if (!_config.UseLatestCSharp)
        {
            usings.Add("System.Linq");
        }

        w.WriteUsings(usings.ToArray());

        w.WriteLine();
        w.WriteNamespace(projectName);

        w.WriteSummary(1, "This interface was automatically generated. It contains all the operations to load the reference lists declared in module " + firstClass.Namespace.Module + ".");

        if (_config.Kinetix == KinetixVersion.Core)
        {
            w.WriteLine(1, "[RegisterImpl]");
        }
        else
        {
            w.WriteLine(1, "[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, IncludeExceptionDetailInFaults = true)]");
        }

        w.WriteClassDeclaration(implementationName, null, interfaceName);

        if (_config.DbContextPath != null)
        {
            var dbContextName = _config.GetDbContextName(firstClass.Namespace.App);

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
        else if (_config.Kinetix == KinetixVersion.Core)
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

        foreach (var classe in classList.Where(c => c.IsPersistent || c.ReferenceValues.Any()))
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
    private void GenerateReferenceAccessorsInterface(IEnumerable<Class> classList)
    {
        var firstClass = classList.First();

        string projectName;
        string interfaceName;

        if (_config.DbContextPath != null)
        {
            projectName = _config.DbContextPath.Split('/').Last();
            interfaceName = $"I{firstClass.Namespace.Module}AccessorsDal";
        }
        else
        {
            projectName = _config.GetNamespace(firstClass).Replace("DataContract", "Contract");
            interfaceName = $"IService{firstClass.Namespace.Module}Accessors";
        }

        var interfaceFileName = _config.GetReferenceInterfaceFilePath(classList)!;

        using var w = new CSharpWriter(interfaceFileName, _logger, _config.UseLatestCSharp);

        if (_config.Kinetix == KinetixVersion.Core)
        {
            var usings = new List<string>();

            if (!_config.UseLatestCSharp)
            {
                usings.Add("System.Collections.Generic");
            }

            usings.Add(_config.GetNamespace(firstClass));
            usings.Add("Kinetix.Services.Annotations");

            w.WriteUsings(usings.ToArray());
        }
        else
        {
            w.WriteUsings(
                "System.Collections.Generic",
                "System.ServiceModel",
                _config.GetNamespace(firstClass),
                "Kinetix.ServiceModel");
        }

        w.WriteLine();
        w.WriteNamespace(projectName);
        w.WriteSummary(1, "This interface was automatically generated. It contains all the operations to load the reference lists declared in module " + firstClass.Namespace.Module + ".");

        if (_config.Kinetix == KinetixVersion.Core)
        {
            w.WriteLine(1, "[RegisterContract]");
        }
        else
        {
            w.WriteLine(1, "[ServiceContract]");
        }

        w.WriteLine(1, "public partial interface " + interfaceName + "\r\n{");

        var count = 0;
        foreach (var classe in classList)
        {
            count++;
            w.WriteSummary(2, "Reference accessor for type " + classe.Name);
            w.WriteReturns(2, "List of " + classe.Name);
            w.WriteLine(2, "[ReferenceAccessor]");
            if (_config.Kinetix != KinetixVersion.Core)
            {
                w.WriteLine(2, "[OperationContract]");
            }

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
    {string.Join(",\r\n    ", classe.ReferenceValues.Select(rv => $"new() {{ {string.Join(", ", rv.Value.Select(prop => $"{prop.Key.Name} = {(prop.Key.Domain.ShouldQuoteSqlValue ? $"\"{prop.Value}\"" : prop.Value)}"))} }}"))}
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

            return _config.Kinetix == KinetixVersion.Core
                ? "return _brokerManager.GetBroker<" + classe.Name + ">().GetAll(" + queryParameter + ");"
                : "return BrokerManager.GetStandardBroker<" + classe.Name + ">().GetAll(" + queryParameter + ");";
        }
    }
}