using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

/// <summary>
/// Maillon du nom calculé d'une propriété (alias ou association) : le nom est le rendu de la liste de ses segments.
/// La hiérarchie est fermée : un segment est soit un texte fixe, soit le nom d'une propriété, soit le nom d'une classe.
/// </summary>
public closed record NameSegment
{
    /// <summary>
    /// Rend le segment, en appliquant des substitutions de noms.
    /// </summary>
    /// <param name="pascalCase">Rendu en PascalCase.</param>
    /// <param name="forcePropertyName">Rend aussi les segments qui n'apparaissent qu'en mode forcé (voir <see cref="PropertyName.RequiresForce"/>).</param>
    /// <returns>Le nom.</returns>
    internal abstract string Render(bool pascalCase);

    /// <summary>
    /// Nom d'une classe (au singulier ou au pluriel).
    /// </summary>
    /// <param name="Class">Classe.</param>
    /// <param name="Plural">Utilise le nom pluriel de la classe.</param>
    public sealed record ClassName(Class Class, bool Plural = false) : NameSegment
    {
        internal override string Render(bool pascalCase)
        {
            return (Plural, pascalCase) switch
            {
                (true, true) => Class.PluralNamePascal,
                (true, false) => Class.PluralName,
                (false, true) => Class.NamePascal,
                (false, false) => Class.Name.Value,
            };
        }
    }

    public sealed record PropertyName(IProperty? Property, bool ForceTarget = false, bool RequiresForce = false)
        : NameSegment
    {
        internal override string Render(bool pascalCase)
        {
            return Property is null ? string.Empty
                : pascalCase ? Property.NamePascal
                : Property.Name;
        }
    }

    /// <summary>
    /// Texte fixe, avec sa forme brute et sa forme PascalCase.
    /// </summary>
    /// <param name="Raw">Forme brute.</param>
    /// <param name="ToPascalCase">Transformation pour le PascalCase personnalisée.</param>
    public sealed record Text(LocatedString Raw, Func<string, string>? ToPascalCase = null) : NameSegment
    {
        internal override string Render(bool pascalCase)
        {
            return pascalCase ? (ToPascalCase?.Invoke(Raw) ?? Raw.ToPascalCase(strictIfUppercase: true)) : Raw;
        }
    }
}
