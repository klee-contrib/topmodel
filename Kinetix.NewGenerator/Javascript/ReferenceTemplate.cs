using System;
using System.Collections.Generic;
using System.Linq;
using Kinetix.NewGenerator.Model;
using Kinetix.Tools.Common;

namespace Kinetix.NewGenerator.Javascript
{
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    public partial class ReferenceTemplate : TemplateBase
    {
        public readonly IEnumerable<Class> _references;

        public ReferenceTemplate(IEnumerable<Class> references)
        {
            _references = references;
        }

        /// <summary>
        /// Create the template output
        /// </summary>
        public string TransformText()
        {
            Write("/*\r\n    Ce fichier a été généré automatiquement.\r\n    Toute modification sera perdue.\r\n*/\r\n");

            foreach (var reference in _references)
            {
                Write("\r\nexport type ");
                Write(reference.Name);
                Write("Code = ");
                Write(reference.ReferenceValues != null
                    ? string.Join(" | ", reference.ReferenceValues.Select(r => r.Value.code).OrderBy(x => x))
                    : "string");
                Write(";\r\nexport interface ");
                Write(reference.Name);
                Write(" {\r\n");

                foreach (var property in reference.Properties.OfType<IFieldProperty>())
                {
                    Write("    ");
                    Write(property.Name.ToFirstLower());
                    Write(property.Required || property.PrimaryKey ? string.Empty : "?");
                    Write(": ");
                    Write(GetTSType(property, reference));
                    Write(";\r\n");
                }

                Write("}\r\n");

                Write("export const ");
                Write(reference.Name.ToFirstLower());
                Write(" = {type: {} as ");
                Write(reference.Name);
                Write(", valueKey: \"");
                Write(reference.PrimaryKey!.Name.ToFirstLower());
                Write("\", labelKey: \"");
                Write(reference.DefaultProperty?.ToFirstLower() ?? "libelle");
                Write("\"} as const;\r\n");
            }
            return GenerationEnvironment.ToString();
        }

        /// <summary>
        /// Transforme le type en type Typescript.
        /// </summary>
        /// <param name="property">La propriété dont on cherche le type.</param>
        /// <param name="reference">Classe de la propriété.</param>
        /// <returns>Le type en sortie.</returns>
        private string GetTSType(IFieldProperty property, Class reference)
        {
            if (property.Name == "Code")
            {
                return $"{reference.Name}Code";
            }
            else if (property.Name.EndsWith("Code", StringComparison.Ordinal))
            {
                return property.Name.ToFirstUpper();
            }

            return TSUtils.CSharpToTSType(property.Domain.CsharpType);
        }
    }
}
