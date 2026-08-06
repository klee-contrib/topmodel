using System.Text;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

/// <summary>
/// FileWriter avec des méthodes spécialisées pour écrire du CJava
/// </summary>
public class JavaWriter(IFileWriter writer, string packageName) : IDisposable
{
    private readonly List<string> _imports = [];
    private readonly HashSet<string> _inlinedImports = [];
    private readonly List<WriterLine> _toWrite = [];

    public void AddImport(string import)
    {
        _imports.Add(import);
    }

    public void AddImports(IEnumerable<string> imports)
    {
        _imports.AddRange(imports);
    }

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        writer.IndentValue = "\t";
        writer.WriteLine($"package {packageName};");
        WriteImports();
        _toWrite.ForEach(l => writer.WriteLine(l.Indent, l.Line));
        writer.Dispose();
    }

    /// <summary>
    /// Ecrit la classe Java.
    /// </summary>
    /// <param name="javaClass">Classe à écrire dans le flux.</param>
    public void Write(JavaClass javaClass)
    {
        AddImports(javaClass.Imports);
        WriteClass(0, javaClass);
    }

    /// <summary>
    /// Ecrit la signature de méthode Java pour une classe hors JavaClass.
    /// </summary>
    /// <param name="javaMethod">Valeur à écrire dans le flux.</param>
    public void Write(JavaMethod javaMethod)
    {
        AddImports(javaMethod.Imports);
        WriteMethod(1, javaMethod, classe: null);
    }

    /// <summary>
    /// Ecrit l'annotation pour une classe hors JavaClass.
    /// </summary>
    /// <param name="javaAnnotation">Valeur à écrire dans le flux.</param>
    public void Write(JavaAnnotation javaAnnotation)
    {
        AddImports(javaAnnotation.Imports);
        WriteAnnotation(0, javaAnnotation, classe: null);
    }

    public void WriteDocEnd(int indentationLevel)
    {
        WriteLine(indentationLevel, " */");
    }

    /// <summary>
    /// Ecrit la valeur du résumé du commentaire.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indentation.</param>
    /// <param name="value">Valeur à écrire.</param>
    public void WriteDocStart(int indentationLevel, string value)
    {
        value = value.Trim();

        var sb = new StringBuilder();
        sb.Append($"/**{Environment.NewLine}");
        sb.Append(" * ").Append(value.Replace(Environment.NewLine, $"{Environment.NewLine} * "));
        if (!value.EndsWith(".", StringComparison.OrdinalIgnoreCase))
        {
            sb.Append('.');
        }

        if (!string.IsNullOrEmpty(value))
        {
            WriteLine(indentationLevel, sb.ToString());
        }
    }

    /// <summary>
    /// Ecrit la chaine de caractère dans le flux.
    /// </summary>
    /// <param name="value">Valeur à écrire dans le flux.</param>
    public void WriteLine(string? value = null)
    {
        WriteLine(0, value ?? string.Empty);
    }

    /// <summary>
    /// Ecrit la chaine avec le niveau indenté.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indentation.</param>
    /// <param name="value">Valeur à écrire dans le flux.</param>
    public void WriteLine(int indentationLevel, string value)
    {
        _toWrite.Add(new WriterLine() { Line = value, Indent = indentationLevel });
    }

    /// <summary>
    /// Ecrit le commentaire de returns.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indention.</param>
    /// <param name="value">Description du returns.</param>
    public void WriteReturns(int indentationLevel, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            var sb = new StringBuilder();
            sb.Append(" * @return ");
            sb.Append(value);
            if (!value.EndsWith(".", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append('.');
            }

            WriteLine(indentationLevel, " *");
            WriteLine(indentationLevel, sb.ToString());
        }
    }

    private string GetAnnotation(JavaAnnotation annotation, JavaClass? classe)
    {
        var name = annotation.Name;

        if (
            _imports.Except(annotation.Imports).Any(i => i.Split('.')[^1] == annotation.Name)
            || annotation.Name == classe?.Name
        )
        {
            var fullName = annotation.Imports.FirstOrDefault(i => i.EndsWith(annotation.Name));
            if (fullName != null)
            {
                name = fullName;
                _inlinedImports.Add(fullName);
            }
        }

        name = name.StartsWith('@') ? name : $"@{name}";

        if (!annotation.Attributes.Any())
        {
            return name;
        }
        else if (
            annotation.Attributes.Count == 1
            && annotation.Attributes.Any(a => a.Key == "value")
            && !annotation.Attributes.First().Value.TryPickT2(out var _, out var value)
        )
        {
            return $"{name}({value.Match(s => s, v => GetAnnotation(v, classe))})";
        }
        else if (annotation.Attributes.Values.Any(v => v.IsT2))
        {
            var sb = new StringBuilder();
            sb.Append($"{name}(");
            var attrList = annotation.Attributes.ToList();
            for (var i = 0; i < attrList.Count; i++)
            {
                var attr = attrList[i];
                var isLast = i == attrList.Count - 1;
                sb.Append($"{Environment.NewLine}\t");
                if (attr.Value.TryPickT2(out var annotations, out var v))
                {
                    sb.Append($"{attr.Key} = {{");
                    for (var j = 0; j < annotations.Count; j++)
                    {
                        sb.Append($"{Environment.NewLine}\t\t{GetAnnotation(annotations[j], classe)}");
                        if (j < annotations.Count - 1)
                        {
                            sb.Append(',');
                        }
                    }
                    sb.Append($"{Environment.NewLine}\t}}");
                }
                else
                {
                    sb.Append($"{attr.Key} = {v.Match(s => s, value => GetAnnotation(value, classe))}");
                }

                if (!isLast)
                {
                    sb.Append(',');
                }
            }

            sb.Append($"{Environment.NewLine})");
            return sb.ToString();
        }
        else
        {
            var attributes = string.Join(
                ", ",
                annotation.Attributes.Select(a =>
                    $"{a.Key} = {a.Value.Match(s => s, value => GetAnnotation(value, classe), s => throw new NotSupportedException())}"
                )
            );
            return $"{name}({attributes})";
        }
    }

    private string GetConstructorSignature(JavaConstructor constructor, JavaClass? classe)
    {
        return $@"{(!string.IsNullOrEmpty(constructor.Visibility) ? $"{constructor.Visibility} " : string.Empty)}{constructor.ReturnType}({string.Join(", ", constructor.Parameters.Select(p => GetMethodParameterDeclaration(p, classe)))})";
    }

    private string GetMethodParameterDeclaration(JavaMethodParameter parameter, JavaClass? classe)
    {
        return $@"{(parameter.Final ? "final " : string.Empty)}{string.Join(' ', parameter.Annotations.DistinctBy(e => e.Name.Split('(')[0]).OrderBy(a => a.Name).Select(a => GetAnnotation(a, classe)))}{(parameter.Annotations.Count > 0 ? ' ' : string.Empty)}{parameter.Type} {parameter.Name}";
    }

    private string GetMethodSignature(JavaMethod method, JavaClass? classe)
    {
        return $@"{(!string.IsNullOrEmpty(method.Visibility) ? $"{method.Visibility} " : string.Empty)}{(method.Static ? "static " : string.Empty)}{(method.GenericTypes.Count > 0 ? $"<{string.Join(", ", method.GenericTypes)}> " : string.Empty)}{method.ReturnType} {method.Name}({string.Join(", ", method.Parameters.Select(p => GetMethodParameterDeclaration(p, classe)))})";
    }

    /// <summary>
    /// Ecrit l'annotation avec le niveau indenté.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indentation.</param>
    /// <param name="javaAnnotation">Valeur à écrire dans le flux.</param>
    /// <param name="classe">Classe Java associée.</param>
    private void WriteAnnotation(int indentationLevel, JavaAnnotation javaAnnotation, JavaClass? classe)
    {
        _toWrite.Add(new WriterLine() { Line = GetAnnotation(javaAnnotation, classe), Indent = indentationLevel });
    }

    /// <summary>
    /// Ecrit l'annotation avec le niveau indenté.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indentation.</param>
    /// <param name="javaAnnotations">Valeurs à écrire dans le flux.</param>
    /// <param name="classe">Classe Java</param>
    private void WriteAnnotations(int indentationLevel, IEnumerable<JavaAnnotation> javaAnnotations, JavaClass? classe)
    {
        foreach (
            var annotation in javaAnnotations
                .DistinctBy(e => e.Name.Split('(')[0])
                .OrderBy(a => GetAnnotation(a, classe).Length)
        )
        {
            WriteAnnotation(indentationLevel, annotation, classe);
        }
    }

    /// <summary>
    /// Ecrit la classe Java avec le niveau indenté.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indentation.</param>
    /// <param name="javaClass">Classe à écrire dans le flux.</param>
    private void WriteClass(int indentationLevel, JavaClass javaClass)
    {
        AddImports(javaClass.Imports);
        WriteLine();
        if (!string.IsNullOrEmpty(javaClass.Comment))
        {
            WriteDocStart(indentationLevel, javaClass.Comment);
            WriteDocEnd(indentationLevel);
        }

        WriteAnnotations(indentationLevel, javaClass.Annotations, javaClass);
        WriteLine(indentationLevel, $@"{javaClass.GetDeclaration()} {{");
        if (javaClass is JavaEnum javaEnum)
        {
            var i = -1;
            foreach (var value in javaEnum.Values)
            {
                i++;
                if (value.Comment != string.Empty)
                {
                    if (i > 0)
                    {
                        WriteLine();
                    }

                    WriteDocStart(indentationLevel + 1, value.Comment);
                    WriteDocEnd(indentationLevel + 1);
                }
                _toWrite.Add(
                    new WriterLine()
                    {
                        Line =
                            $"{value.Name}{(value.Parameters.Count > 0 ? $"({string.Join(", ", value.Parameters)})" : string.Empty)}"
                            + (
                                i < javaEnum.Values.Count - 1 ? ","
                                : (
                                    javaClass.Fields.Count == 0
                                    && javaClass.Constructors.Count == 0
                                    && javaClass.Methods.Count == 0
                                )
                                    ? string.Empty
                                : ";"
                            ),
                        Indent = indentationLevel + 1,
                    }
                );
            }
        }
        foreach (var field in javaClass.Fields)
        {
            WriteField(indentationLevel + 1, field, javaClass);
        }

        foreach (var constructor in javaClass.Constructors)
        {
            WriteConstructor(indentationLevel + 1, constructor, javaClass);
        }

        foreach (var method in javaClass.Methods)
        {
            WriteMethod(indentationLevel + 1, method, javaClass);
        }

        foreach (var innerClass in javaClass.InnerClasses)
        {
            WriteClass(indentationLevel + 1, innerClass);
        }

        WriteLine(indentationLevel, "}");
    }

    /// <summary>
    /// Ecrit la déclaration d'un constructeur.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indentation.</param>
    /// <param name="constructor">Constructeur à écrire.</param>
    /// <param name="classe">Classe.</param>
    private void WriteConstructor(int indentationLevel, JavaConstructor constructor, JavaClass classe)
    {
        WriteLine();
        WriteAnnotations(indentationLevel, constructor.Annotations, classe);
        if (!string.IsNullOrEmpty(constructor.Comment))
        {
            WriteDocStart(indentationLevel, constructor.Comment);
            foreach (var param in constructor.Parameters)
            {
                WriteParam(1, param.Name, param.Comment);
            }

            if (!string.IsNullOrEmpty(constructor.ReturnComment))
            {
                WriteReturns(indentationLevel, constructor.ReturnComment);
            }

            WriteDocEnd(indentationLevel);
        }

        var hasBody = constructor.Body.Count > 0;
        _toWrite.Add(
            new WriterLine()
            {
                Line = @$"{GetConstructorSignature(constructor, classe)}{(hasBody ? " {" : ";")}",
                Indent = indentationLevel,
            }
        );
        foreach (var bodyLine in constructor.Body)
        {
            _toWrite.Add(new WriterLine() { Line = bodyLine.Line, Indent = bodyLine.Indent + indentationLevel + 1 });
        }

        if (hasBody)
        {
            _toWrite.Add(new WriterLine() { Line = "}", Indent = indentationLevel });
        }
    }

    /// <summary>
    /// Ecrit la déclaration d'un champ.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indentation.</param>
    /// <param name="field">Champ à écrire.</param>
    /// <param name="classe">Classe.</param>
    private void WriteField(int indentationLevel, JavaField field, JavaClass? classe)
    {
        WriteLine();
        if (field.Comments.Any())
        {
            WriteDocStart(indentationLevel, field.Comments.First());
            for (var i = 1; i < field.Comments.Count; i++)
            {
                WriteLine(indentationLevel, $" * {field.Comments[i]}");
            }
            WriteDocEnd(indentationLevel);
        }

        WriteAnnotations(indentationLevel, field.Annotations, classe);

        _toWrite.Add(
            new WriterLine()
            {
                Line =
                    $"{field.Visibility}{(field.Static ? " static" : string.Empty)}{(field.Final ? " final" : string.Empty)}{(field.Volatile ? " volatile" : string.Empty)} {field.Type} {field.Name}{(field.DefaultValue != string.Empty ? $" = {field.DefaultValue}" : string.Empty)};",
                Indent = indentationLevel,
            }
        );
    }

    /// <summary>
    /// Ajoute les imports
    /// </summary>
    /// <param name="fw">FileWriter.</param>
    private void WriteImports()
    {
        var imports = _imports
            .Where(i => string.Join('.', i.Split('.').SkipLast(1).ToList()) != packageName)
            .Except(_inlinedImports)
            .Distinct()
            .ToList();
        var currentPackage = string.Empty;
        foreach (var import in imports.Where(i => i.StartsWith("java") || i.StartsWith("org")).Order())
        {
            var package = import.Split('.')[0];
            if (package != currentPackage)
            {
                writer.WriteLine();
                currentPackage = package;
            }

            writer.WriteLine($"import {import};");
        }

        foreach (var import in imports.Where(i => !(i.StartsWith("java") || i.StartsWith("org"))).Order())
        {
            var package = import.Split('.')[0];
            if (package != currentPackage)
            {
                writer.WriteLine();
                currentPackage = package;
            }

            writer.WriteLine($"import {import};");
        }
    }

    /// <summary>
    /// Ecrit la signature de méthode avec le niveau indenté.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indentation.</param>
    /// <param name="javaMethod">Valeur à écrire dans le flux.</param>
    /// <param name="classe">Classe.</param>
    private void WriteMethod(int indentationLevel, JavaMethod javaMethod, JavaClass? classe)
    {
        WriteLine();
        if (!string.IsNullOrEmpty(javaMethod.Comment))
        {
            WriteDocStart(indentationLevel, javaMethod.Comment);
            foreach (var param in javaMethod.Parameters)
            {
                WriteParam(indentationLevel, param.Name, param.Comment);
            }

            if (!string.IsNullOrEmpty(javaMethod.ReturnComment))
            {
                WriteReturns(indentationLevel, javaMethod.ReturnComment);
            }

            WriteDocEnd(indentationLevel);
        }

        WriteAnnotations(indentationLevel, javaMethod.Annotations, classe);
        var hasBody = javaMethod.Body.Count > 0;
        _toWrite.Add(
            new WriterLine()
            {
                Line = @$"{GetMethodSignature(javaMethod, classe)}{(hasBody ? " {" : ";")}",
                Indent = indentationLevel,
            }
        );
        foreach (var bodyLine in javaMethod.Body)
        {
            _toWrite.Add(
                new WriterLine()
                {
                    Line = bodyLine.Line,
                    Indent = bodyLine.Line == string.Empty ? 0 : bodyLine.Indent + indentationLevel + 1,
                }
            );
        }

        if (hasBody)
        {
            _toWrite.Add(new WriterLine() { Line = "}", Indent = indentationLevel });
        }
    }

    /// <summary>
    /// Ecrit le commentaire de parametre.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indentation.</param>
    /// <param name="paramName">Nom du paramètre.</param>
    /// <param name="value">Valeur du paramètre.</param>
    private void WriteParam(int indentationLevel, string paramName, string value)
    {
        if (!string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(value))
        {
            var sb = new StringBuilder();
            sb.Append(" * @param ");
            sb.Append(paramName);
            sb.Append(' ');
            sb.Append(value);
            if (!value.EndsWith(".", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append('.');
            }

            WriteLine(indentationLevel, sb.ToString());
        }
    }
}
