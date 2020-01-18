using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core.Config;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.CSharp
{
    using static CSharpUtils;

    public class ReferenceAccessorGenerator
    {
        private readonly CSharpConfig _config;
        private readonly ILogger<CSharpGenerator> _logger;
        private readonly string _rootNamespace;

        public ReferenceAccessorGenerator(string rootNamespace, CSharpConfig config, ILogger<CSharpGenerator> logger)
        {
            _config = config;
            _logger = logger;
            _rootNamespace = rootNamespace;
        }

        /// <summary>
        /// Génère les ReferenceAccessor pour un namespace.
        /// </summary>
        /// <param name="module">Classes.</param>
        public void Generate(IGrouping<Namespace, Class> module)
        {
            var classList = module
                .Where(x => x.Stereotype == Stereotype.Reference || x.Stereotype == Stereotype.Statique)
                .OrderBy(x => Pluralize(x.Name), StringComparer.Ordinal)
                .ToList();

            if (!classList.Any())
            {
                return;
            }

            _logger.LogInformation($"Génération des accesseurs de référence pour le module {module.Key.Module}...");

            GenerateReferenceAccessorsInterface(classList, module.Key.CSharpName);
            GenerateReferenceAccessorsImplementation(classList, module.Key.CSharpName);

            _logger.LogInformation($"{classList.Count()} accesseurs ajoutés.");
        }

        /// <summary>
        /// Génère l'implémentation des ReferenceAccessors.
        /// </summary>
        /// <param name="classList">Liste de ModelClass.</param>
        /// <param name="nameSpaceName">Namespace.</param>
        private void GenerateReferenceAccessorsImplementation(List<Class> classList, string nameSpaceName)
        {
            if (_config.OutputDirectory == null)
            {
                return;
            }

            var nameSpacePrefix = nameSpaceName.Replace("DataContract", string.Empty);

            string projectDir;
            string projectName;
            string implementationName;
            if (_config.DbContextProjectPath != null)
            {
                projectDir = $"{_config.OutputDirectory}\\{_config.DbContextProjectPath}";
                projectName = _config.DbContextProjectPath.Split('/').Last();
                implementationName = $"{nameSpacePrefix}AccessorsDal";
            }
            else
            {
                projectName = $"{_rootNamespace}.{nameSpacePrefix}Implementation";
                projectDir = Path.Combine(GetImplementationDirectoryName(_config.OutputDirectory, _rootNamespace), _rootNamespace + "." + nameSpacePrefix + "Implementation\\Service.Implementation");
                implementationName = $"Service{nameSpacePrefix}Accessors";
            }

            var interfaceName = $"I{implementationName}";

            var implementationFileName = Path.Combine(projectDir, _config.DbContextProjectPath == null ? "generated" : "generated\\Reference", $"{implementationName}.cs");

            using var w = new CSharpWriter(implementationFileName);

            var usings = new[]
            {
                "System.Collections.Generic",
                "System.Linq",
                $"{_rootNamespace}.{nameSpaceName}"
            }.ToList();

            if (_config.Kinetix == KinetixVersion.Core)
            {
                usings.Add("Kinetix.Services.Annotations");
            }
            else
            {
                usings.Add("System.ServiceModel");

                if (_config.DbContextProjectPath == null)
                {
                    usings.Add($"{_rootNamespace}.{nameSpacePrefix}Contract");
                    usings.Add(_config.Kinetix == KinetixVersion.Framework
                        ? "Kinetix.Broker"
                        : "Fmk.Broker");

                    if (classList.Any(classe => classe.OrderProperty != null || classe.LabelProperty != null && classe.LabelProperty.Name != "Libelle"))
                    {
                        usings.Add(_config.Kinetix == KinetixVersion.Framework
                            ? "Kinetix.Data.SqlClient"
                            : "Fmk.Data.SqlClient");
                    }
                }
            }

            w.WriteUsings(usings.ToArray());

            w.WriteLine();
            w.WriteNamespace(projectName);

            w.WriteSummary(1, "This interface was automatically generated. It contains all the operations to load the reference lists declared in namespace " + nameSpaceName + ".");

            if (_config.Kinetix == KinetixVersion.Core)
            {
                w.WriteLine(1, "[RegisterImpl]");
            }
            else
            {
                w.WriteLine(1, "[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, IncludeExceptionDetailInFaults = true)]");
            }

            w.WriteClassDeclaration(implementationName, null, new List<string> { interfaceName });

            if (_config.DbContextProjectPath != null)
            {
                var dbContextName = $"{_rootNamespace}DbContext";
                var schema = _config.DbSchema;
                if (schema != null)
                {
                    dbContextName = $"{schema.First().ToString().ToUpper() + schema.Substring(1)}DbContext";
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

            foreach (var classe in classList)
            {
                var serviceName = "Load" + (_config.DbContextProjectPath == null ? $"{classe.Name}List" : Pluralize(classe.Name));
                w.WriteLine(2, "/// <inheritdoc cref=\"" + interfaceName + "." + serviceName + "\" />");
                w.WriteLine(2, "public ICollection<" + classe.Name + "> " + serviceName + "()\r\n{");
                w.WriteLine(3, LoadReferenceAccessorBody(_config.DbContextProjectPath == null, classe));
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
        /// <param name="nameSpaceName">Namespace.</param>
        private void GenerateReferenceAccessorsInterface(IEnumerable<Class> classList, string nameSpaceName)
        {
            if (_config.OutputDirectory == null)
            {
                return;
            }

            var nameSpacePrefix = nameSpaceName.Replace("DataContract", string.Empty);

            string projectDir;
            string projectName;
            string interfaceName;
            if (_config.DbContextProjectPath != null)
            {
                projectDir = $"{_config.OutputDirectory}\\{_config.DbContextProjectPath}";
                projectName = _config.DbContextProjectPath.Split('/').Last();
                interfaceName = $"I{nameSpacePrefix}AccessorsDal";
            }
            else
            {
                projectName = $"{_rootNamespace}.{nameSpacePrefix}Contract";
                projectDir = Path.Combine(GetDirectoryForProject(_config.LegacyProjectPaths, _config.OutputDirectory, false, _rootNamespace, $"{nameSpacePrefix}Contract"));
                interfaceName = $"IService{nameSpacePrefix}Accessors";
            }

            var interfaceFileName = Path.Combine(projectDir, _config.DbContextProjectPath == null ? "generated" : "generated\\Reference", $"{interfaceName}.cs");

            using var w = new CSharpWriter(interfaceFileName);

            if (_config.Kinetix == KinetixVersion.Core)
            {
                w.WriteUsings(
                    "System.Collections.Generic",
                    $"{_rootNamespace}.{nameSpaceName}",
                    "Kinetix.Services.Annotations");
            }
            else
            {
                w.WriteUsings(
                    "System.Collections.Generic",
                    "System.ServiceModel",
                    $"{_rootNamespace}.{nameSpaceName}",
                    _config.Kinetix == KinetixVersion.Fmk ? "Fmk.ServiceModel" : "Kinetix.ServiceModel");
            }

            w.WriteLine();
            w.WriteNamespace(projectName);
            w.WriteSummary(1, "This interface was automatically generated. It contains all the operations to load the reference lists declared in namespace " + nameSpaceName + ".");

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
                w.WriteLine(2, "ICollection<" + classe.Name + "> Load" + (_config.DbContextProjectPath == null ? $"{classe.Name}List" : Pluralize(classe.Name)) + "();");

                if (count != classList.Count())
                {
                    w.WriteLine();
                }
            }

            w.WriteLine(1, "}");
            w.WriteLine("}");
        }

        /// <summary>
        /// Retourne le code associé au cors de l'implémentation d'un service de type ReferenceAccessor.
        /// </summary>
        /// <param name="isBroker">Broker.</param>
        /// <param name="classFiles">Type chargé par le ReferenceAccessor.</param>
        /// <returns>Code généré.</returns>
        private string LoadReferenceAccessorBody(bool isBroker, Class classe)
        {
            var defaultProperty = classe.Properties.OfType<IFieldProperty>()
                .SingleOrDefault(p => p.Name == classe.OrderProperty)
            ?? classe.Properties.OfType<IFieldProperty>()
                .SingleOrDefault(p => p.Name == classe.DefaultProperty);

            var queryParameter = string.Empty;
            if (!isBroker)
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

                return "return BrokerManager.GetStandardBroker<" + classe.Name + ">().GetAll(" + queryParameter + ");";
            }
        }
    }
}
