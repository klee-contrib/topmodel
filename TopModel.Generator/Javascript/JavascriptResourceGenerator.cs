using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core;
using TopModel.Core.Config;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Javascript
{
    /// <summary>
    /// Générateur des objets de traduction javascripts.
    /// </summary>
    public class JavascriptResourceGenerator : IGenerator
    {
        private readonly JavascriptConfig? _config;
        private readonly ILogger<JavascriptResourceGenerator> _logger;
        private readonly ModelStore _modelStore;

        public JavascriptResourceGenerator(ModelStore modelStore, ILogger<JavascriptResourceGenerator> logger, JavascriptConfig? config = null)
        {
            _config = config;
            _logger = logger;
            _modelStore = modelStore;
        }

        public bool CanGenerate => _config?.ResourceOutputDirectory != null;

        public string Name => "des ressources Typescript";

        /// <summary>
        /// Génère le code des classes.
        /// </summary>
        public void Generate()
        {
            if (_config?.ResourceOutputDirectory == null)
            {
                return;
            }

            var nameSpaceMap = _modelStore.Classes.GroupBy(c => c.Namespace.Module).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var entry in nameSpaceMap)
            {
                var dirInfo = Directory.CreateDirectory(_config.ResourceOutputDirectory);
                var fileName = FirstToLower(entry.Key);

                _logger.LogInformation($"Génération du fichier de ressources pour le module {entry.Key}...");
                WriteNameSpaceNode(dirInfo.FullName + "/" + fileName + ".ts", entry.Key, entry.Value);
                _logger.LogInformation($"{entry.Value.Count} classes ajoutées dans le fichier de ressources.");
            }
        }

        /// <summary>
        /// Set the first character to lower.
        /// </summary>
        /// <param name="value">String to edit.</param>
        /// <returns>Parser string.</returns>
        private static string FirstToLower(string value)
        {
            return value.Substring(0, 1).ToLowerInvariant() + value.Substring(1);
        }

        /// <summary>
        /// Formate le nom en javascript.
        /// </summary>
        /// <param name="name">Nom a formatter.</param>
        /// <returns>Nom formatté.</returns>
        private static string FormatJsName(string name)
        {
            return FirstToLower(name);
        }

        /// <summary>
        /// Formate le nom en javascript.
        /// </summary>
        /// <param name="name">Nom a formatter.</param>
        /// <returns>Nom formatté.</returns>
        private static string FormatJsPropertyName(string name)
        {
            return name.Substring(0, 1).ToLowerInvariant() + name.Substring(1);
        }

        /// <summary>
        /// Générère le noeus de classe.
        /// </summary>
        /// <param name="writer">Flux de sortie.</param>
        /// <param name="classe">Classe.</param>
        /// <param name="isLast">True s'il s'agit de al dernière classe du namespace.</param>
        private static void WriteClasseNode(TextWriter writer, Class classe, bool isLast)
        {
            writer.WriteLine("    " + FormatJsName(classe.Name) + ": {");
            var i = 1;

            foreach (var property in classe.Properties)
            {
                WritePropertyNode(writer, property, classe.Properties.Count == i++);
            }

            WriteCloseBracket(writer, 1, isLast);
        }

        /// <summary>
        /// Ecrit dans le flux de sortie la fermeture du noeud courant.
        /// </summary>
        /// <param name="writer">Flux de sortie.</param>
        /// <param name="indentionLevel">Idention courante.</param>
        /// <param name="isLast">Si true, on n'ajoute pas de virgule à la fin.</param>
        private static void WriteCloseBracket(TextWriter writer, int indentionLevel, bool isLast)
        {
            for (var i = 0; i < indentionLevel; i++)
            {
                writer.Write("    ");
            }

            writer.Write("}");
            writer.WriteLine(!isLast ? "," : string.Empty);
        }

        /// <summary>
        /// Générère le noeud de namespace.
        /// </summary>
        /// <param name="outputFileNameJavascript">Nom du fichier de sortie..</param>
        /// <param name="namespaceName">Nom du namespace.</param>
        /// <param name="classes">Liste des classe du namespace.</param>
        private static void WriteNameSpaceNode(string outputFileNameJavascript, string namespaceName, IList<Class> classes)
        {
            using var writerJs = new FileWriter(outputFileNameJavascript, encoderShouldEmitUTF8Identifier: false);
           
            writerJs.WriteLine($"export const {FirstToLower(namespaceName)} = {{");
            
            var i = 1;
            foreach (var classe in classes.OrderBy(c => c.Name))
            {
                WriteClasseNode(writerJs, classe, classes.Count == i++);
            }

            writerJs.WriteLine("};");
        }

        /// <summary>
        /// Génère le noeud de la proprité.
        /// </summary>
        /// <param name="writer">Flux de sortie.</param>
        /// <param name="property">Propriété.</param>
        /// <param name="isLast">True s'il s'agit du dernier noeud de la classe.</param>
        private static void WritePropertyNode(TextWriter writer, IProperty property, bool isLast)
        {
            writer.WriteLine("        " + FormatJsPropertyName(property.Name) + @": """ + property.Label + @"""" + (isLast ? string.Empty : ","));
        }
    }
}
