using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

/// <summary>
/// Générateur d'enums Typescript.
/// </summary>
public class TypescriptEnumsGenerator(ILogger<TypescriptEnumsGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGroupGeneratorBase<JavascriptConfig>(logger, writerProvider)
{
    public override string Name => "JSEnumsGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.Enum != null)
        {
            yield return ("main", Config.GetEnumsFileName(classe.Namespace, tag));
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        GenerateEnumsFile(fileName, classes.OrderBy(r => r.NameCamel), tag);
    }

    /// <summary>
    /// Create the template output
    /// </summary>
    private void GenerateEnumsFile(string fileName, IEnumerable<Class> enums, string tag)
    {
        using var fw = OpenFileWriter(fileName, encoderShouldEmitUTF8Identifier: false);

        var imports = enums
            .SelectMany(r => r.ClassDependencies)
            .Select(dep =>
                (
                    Import: dep.Source switch
                    {
                        IProperty fp => Config.GetType(fp),
                        Class c => c.NamePascal,
                        _ => null!,
                    },
                    Path: Config.GetImportPathForClass(
                        dep,
                        dep.Classe.Tags.Contains(tag)
                            ? tag
                            : dep.Classe.Tags.Intersect(Config.Tags).FirstOrDefault() ?? tag,
                        tag
                    )!
                )
            )
            .Concat(
                enums.SelectMany(r => r.Properties).SelectMany(dep => Config.GetDomainImportPaths(fileName, dep, tag))
            )
            .Concat(
                enums
                    .Where(r => r.Readonly || r.Enum == EnumMode.Enum)
                    .SelectMany(r =>
                        r.Properties.SelectMany(dep =>
                            r.Values.Where(v => v.Value.ContainsKey(dep))
                                .SelectMany(v => Config.GetValueImportPaths(fileName, dep, v.Value[dep]))
                        )
                    )
            )
            .Where(i => i.Path != null && i.Path != $"./{Config.EnumsFileName}")
            .GroupAndSort();

        foreach (var import in imports)
        {
            fw.Write("import {");
            fw.Write(import.Import);
            fw.Write("} from \"");
            fw.Write(import.Path);
            fw.Write("\";\r\n");
        }

        if (imports.Count > 0)
        {
            fw.Write("\r\n");
        }

        var first = true;
        foreach (var enumClass in enums)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                fw.WriteLine();
            }

            var values = Config.GetAllValues(enumClass).ToList();

            foreach (var enumProp in enumClass.Properties.Where(e => e.EnumLikeProperty == e))
            {
                fw.Write("export type ");
                fw.Write(enumClass.NamePascal);
                if (enumClass.Enum == EnumMode.Class)
                {
                    fw.Write(enumProp.NamePascal);
                }
                fw.Write(" = ");

                var type = Config.GetImplementation(enumProp.Domain)?.Type;
                var quote = (type == "boolean" || type == "number") ? string.Empty : @"""";
                fw.Write(
                    string.Join(
                        " | ",
                        values.Select(r => $@"{quote}{r.Value[enumProp]}{quote}").Order(StringComparer.Ordinal)
                    )
                );
                fw.WriteLine(";");
            }

            if (enumClass.FlagProperty != null)
            {
                fw.Write($"export enum {enumClass.NamePascal}Flag {{\r\n");

                var flagValues = enumClass
                    .Values.Where(refValue =>
                        refValue.Value.ContainsKey(enumClass.FlagProperty)
                        && int.TryParse(refValue.Value[enumClass.FlagProperty], out var _)
                    )
                    .ToList();
                foreach (var refValue in flagValues)
                {
                    var flag = int.Parse(refValue.Value[enumClass.FlagProperty]);
                    fw.Write($"    {refValue.Name} = 0b{Convert.ToString(flag, 2)}");
                    if (flagValues.IndexOf(refValue) != flagValues.Count - 1)
                    {
                        fw.WriteLine(",");
                    }
                }

                fw.WriteLine("\r\n}");
            }

            fw.Write("export interface ");
            fw.Write(enumClass.NamePascal);

            if (enumClass.Enum == EnumMode.Enum)
            {
                fw.Write("Object");
            }

            if (enumClass.Extends != null)
            {
                fw.Write($" extends {enumClass.Extends.NamePascal}");
            }

            fw.Write(" {\r\n");

            foreach (var property in enumClass.Properties)
            {
                fw.Write("    ");
                fw.Write(property.NameCamel);
                fw.Write(property.Required || property.PrimaryKey ? string.Empty : "?");
                fw.Write(": ");
                fw.Write(Config.GetType(property));
                fw.Write(";\r\n");
            }

            fw.Write("}\r\n");

            if (enumClass.Enum != EnumMode.Class || enumClass.Readonly)
            {
                WriteEnumValues(fw, enumClass);
            }

            if (enumClass.Reference)
            {
                fw.WriteReferenceDefinition(enumClass, Config);
            }
        }
    }

    private void WriteEnumValues(IFileWriter fw, Class reference)
    {
        fw.Write("export const ");
        fw.Write(reference.NameCamel);
        fw.Write($"List: {reference.NamePascal}{(reference.Enum == EnumMode.Enum ? "Object" : string.Empty)}[] = [");
        fw.WriteLine();
        foreach (var refValue in reference.Values)
        {
            fw.WriteLine("    {");
            fw.Write("        ");
            fw.Write(
                string.Join(
                    ",\n        ",
                    refValue
                        .Value.Where(p => p.Value != "null")
                        .Select(property =>
                            $"{property.Key.NameCamel}: {(Config.TranslateReferences == true && property.Key == property.Key.Class.DefaultProperty ? $"\"{refValue.ResourceKey}\"" : Config.GetValue(property.Key, property.Value))}"
                        )
                )
            );
            fw.WriteLine();
            fw.WriteLine("    },");
        }

        fw.WriteLine("];");
    }
}
