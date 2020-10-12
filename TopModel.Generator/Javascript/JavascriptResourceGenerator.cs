using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core.FileModel;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Javascript
{
    /// <summary>
    /// Générateur des objets de traduction javascripts.
    /// </summary>
    public class JavascriptResourceGenerator : GeneratorBase
    {
        private readonly JavascriptConfig _config;
        private readonly ILogger<JavascriptResourceGenerator> _logger;
        private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

        public JavascriptResourceGenerator(ILogger<JavascriptResourceGenerator> logger, JavascriptConfig config)
            : base(logger, config)
        {
            _config = config;
            _logger = logger;
        }

        public override string Name => "JSResourceGen";

        protected override void HandleFiles(IEnumerable<ModelFile> files)
        {
            foreach (var file in files)
            {
                _files[file.Name] = file;
            }

            var modules = files.SelectMany(f => f.Classes.Select(c => c.Namespace.Module)).Distinct();

            foreach (var module in modules)
            {
                GenerateModule(module);
            }
        }

        private void GenerateModule(string module)
        {
            if (_config.ResourceOutputDirectory == null)
            {
                return;
            }

            var classes = _files.Values.SelectMany(f => f.Classes).Where(c => c.Namespace.Module == module);
            var dirInfo = Directory.CreateDirectory(_config.ResourceOutputDirectory);
            var fileName = FirstToLower(module);

            WriteNameSpaceNode(dirInfo.FullName + "/" + fileName + ".ts", module, classes);
        }

        /// <summary>
        /// Set the first character to lower.
        /// </summary>
        /// <param name="value">String to edit.</param>
        /// <returns>Parser string.</returns>
        private string FirstToLower(string value)
        {
            return value.Substring(0, 1).ToLowerInvariant() + value.Substring(1);
        }

        /// <summary>
        /// Formate le nom en javascript.
        /// </summary>
        /// <param name="name">Nom a formatter.</param>
        /// <returns>Nom formatté.</returns>
        private string FormatJsName(string name)
        {
            return FirstToLower(name);
        }

        /// <summary>
        /// Formate le nom en javascript.
        /// </summary>
        /// <param name="name">Nom a formatter.</param>
        /// <returns>Nom formatté.</returns>
        private string FormatJsPropertyName(string name)
        {
            return name.Substring(0, 1).ToLowerInvariant() + name.Substring(1);
        }

        /// <summary>
        /// Générère le noeus de classe.
        /// </summary>
        /// <param name="writer">Flux de sortie.</param>
        /// <param name="classe">Classe.</param>
        /// <param name="isLast">True s'il s'agit de al dernière classe du namespace.</param>
        private void WriteClasseNode(TextWriter writer, Class classe, bool isLast)
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
        private void WriteCloseBracket(TextWriter writer, int indentionLevel, bool isLast)
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
        private void WriteNameSpaceNode(string outputFileNameJavascript, string namespaceName, IEnumerable<Class> classes)
        {
            using var writerJs = new FileWriter(outputFileNameJavascript, _logger, encoderShouldEmitUTF8Identifier: false);

            writerJs.WriteLine($"export const {FirstToLower(namespaceName)} = {{");

            var i = 1;
            foreach (var classe in classes.OrderBy(c => c.Name))
            {
                WriteClasseNode(writerJs, classe, classes.Count() == i++);
            }

            writerJs.WriteLine("};");
        }

        /// <summary>
        /// Génère le noeud de la proprité.
        /// </summary>
        /// <param name="writer">Flux de sortie.</param>
        /// <param name="property">Propriété.</param>
        /// <param name="isLast">True s'il s'agit du dernier noeud de la classe.</param>
        private void WritePropertyNode(TextWriter writer, IProperty property, bool isLast)
        {
            writer.WriteLine("        " + FormatJsPropertyName(property.Name) + @": """ + property.Label + @"""" + (isLast ? string.Empty : ","));
        }
    }
}
