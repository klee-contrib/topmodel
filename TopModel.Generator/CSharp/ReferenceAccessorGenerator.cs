using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.CSharp
{
    using static CSharpUtils;

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
                .OrderBy(x => Pluralize(x.Name), StringComparer.Ordinal)
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
            if (_config.OutputDirectory == null)
            {
                return;
            }

            var firstClass = classList.First();

            string projectDir;
            string projectName;
            string implementationName;
            if (_config.DbContextPath != null)
            {
                projectDir = $"{_config.OutputDirectory}\\{_config.DbContextPath}";
                projectName = _config.DbContextPath.Split('/').Last();
                implementationName = $"{firstClass.Namespace.Module}AccessorsDal";
            }
            else
            {
                projectName = $"{firstClass.Namespace.App}.{firstClass.Namespace.Module}Implementation";
                projectDir = $"{_config.OutputDirectory}\\{firstClass.Namespace.App}.Implementation\\{projectName}\\Service.Implementation";
                implementationName = $"Service{firstClass.Namespace.Module}Accessors";
            }

            var interfaceName = $"I{implementationName}";

            var implementationFileName = Path.Combine(projectDir, _config.DbContextPath == null ? "generated" : "generated\\Reference", $"{implementationName}.cs");

            using var w = new CSharpWriter(implementationFileName, _logger);

            var usings = new[]
            {
                "System.Collections.Generic",
                _config.GetNamespace(firstClass)
            }.ToList();

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
                usings.Add("Kinetix.Broker");

                if (classList.Any(classe => classe.OrderProperty != null || classe.LabelProperty != null && classe.LabelProperty.Name != "Libelle"))
                {
                    usings.Add("Kinetix.Data.SqlClient");
                }
            }
            else
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
                var schema = _config.DbSchema;
                if (schema != null)
                {
                    dbContextName = $"{schema.First().ToString().ToUpper() + schema[1..]}DbContext";
                }

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

            foreach (var classe in classList.Where(c => c.IsPersistent || c.ReferenceValues != null))
            {
                var serviceName = "Load" + (_config.DbContextPath == null ? $"{classe.Name}List" : Pluralize(classe.Name));
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
            w.WriteLine("}");
        }

        /// <summary>
        /// Génère l'interface déclarant les ReferenceAccessors d'un namespace.
        /// </summary>
        /// <param name="classList">Liste de ModelClass.</param>
        private void GenerateReferenceAccessorsInterface(IEnumerable<Class> classList)
        {
            if (_config.OutputDirectory == null)
            {
                return;
            }

            var firstClass = classList.First();

            string projectDir;
            string projectName;
            string interfaceName;
            if (_config.DbContextPath != null)
            {
                projectDir = $"{_config.OutputDirectory}\\{_config.DbContextPath}";
                projectName = _config.DbContextPath.Split('/').Last();
                interfaceName = $"I{firstClass.Namespace.Module}AccessorsDal";
            }
            else
            {
                projectName = _config.GetNamespace(firstClass).Replace("DataContract", "Contract");
                projectDir = $"{_config.OutputDirectory}\\{_config.GetModelPath(firstClass).Replace("DataContract", "Contract")}";
                interfaceName = $"IService{firstClass.Namespace.Module}Accessors";
            }

            var interfaceFileName = Path.Combine(projectDir, _config.DbContextPath == null ? "generated" : "generated\\Reference", $"{interfaceName}.cs");

            using var w = new CSharpWriter(interfaceFileName, _logger);

            if (_config.Kinetix == KinetixVersion.Core)
            {
                w.WriteUsings(
                    "System.Collections.Generic",
                    _config.GetNamespace(firstClass),
                    "Kinetix.Services.Annotations");
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

                w.WriteLine(2, "ICollection<" + classe.Name + "> Load" + (_config.DbContextPath == null ? $"{classe.Name}List" : Pluralize(classe.Name)) + "();");

                if (count != classList.Count())
                {
                    w.WriteLine();
                }
            }

            w.WriteLine(1, "}");
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
                return $@"return new List<{classe.Name}>
{{
    {string.Join(",\r\n    ", classe.ReferenceValues.Select(rv => $"new() {{ {string.Join(", ", rv.Value.Select(prop => $"{prop.Key.Name} = {(prop.Key.Domain.ShouldQuoteSqlValue ? $"\"{prop.Value}\"" : prop.Value is bool b ? (b ? "true" : "false") : prop.Value)}"))} }}"))}
}};";
            }

            var defaultProperty = classe.Properties.OfType<IFieldProperty>()
                .SingleOrDefault(p => p.Name == classe.OrderProperty)
            ?? classe.Properties.OfType<IFieldProperty>()
                .SingleOrDefault(p => p.Name == classe.DefaultProperty);

            var queryParameter = string.Empty;
            if (_config.DbContextPath != null)
            {
                if (defaultProperty != null)
                {
                    queryParameter = $".OrderBy(row => row.{defaultProperty.Name})";
                }

                return $"return _dbContext.{Pluralize(classe.Name)}{queryParameter}.ToList();";
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
}
