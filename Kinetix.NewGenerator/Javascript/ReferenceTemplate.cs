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
        /// <summary>
        /// Références.
        /// </summary>
        public IEnumerable<Class> References { get; set; }

        /// <summary>
        /// Create the template output
        /// </summary>
        public string TransformText()
        {
            Write("/*\r\n    Ce fichier a été généré automatiquement.\r\n    Toute modification sera perdue.\r\n*/\r\n");

            foreach (var reference in References)
            {
                Write("\r\nexport type ");
                Write(reference.Name);
                Write("Code = ");
                Write(GetConstValues(reference));
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
                Write(reference.Properties.Single(p => p.PrimaryKey).Name.ToFirstLower());
                Write("\", labelKey: \"");
                Write(reference.DefaultProperty?.ToFirstLower() ?? "libelle");
                Write("\"} as const;\r\n");
            }
            return GenerationEnvironment.ToString();
        }

        /// <summary>
        /// Transforme une liste de constantes en type Typescript.
        /// </summary>
        /// <param name="reference">La liste de constantes.</param>
        /// <returns>Le type de sorte.</returns>
        private string GetConstValues(Class reference)
        {
            //var constValues = string.Join(" | ", reference.ConstValues.Values.Select(value => value.Code));
            //return constValues == string.Empty ? "string" : constValues;
            return "string";
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
