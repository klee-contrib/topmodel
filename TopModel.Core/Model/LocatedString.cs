using System.Diagnostics.CodeAnalysis;
using TopModel.Core.FileModel;
using TopModel.Utils;
using YamlDotNet.Core.Events;

namespace TopModel.Core;

public class LocatedString(Scalar value) : IComparable
{
    public string Value { get; init; } = value.Value;

    public int Length => Value.Length;

    internal Reference Location { get; } = new Reference(value);

    [return: NotNullIfNotNull(nameof(ls))]
    public static implicit operator string?(LocatedString? ls)
    {
        return ls?.Value;
    }

    public static bool operator ==(LocatedString? ls1, LocatedString? ls2)
    {
        return ls1?.Value == ls2?.Value;
    }

    public static bool operator !=(LocatedString? ls1, LocatedString? ls2)
    {
        return ls1?.Value != ls2?.Value;
    }

    /// <inheritdoc cref="IComparable.CompareTo" />
    public int CompareTo(object? obj)
    {
        if (obj is LocatedString ls)
        {
            return Value.CompareTo(ls.Value);
        }

        if (obj is string s)
        {
            return Value.CompareTo(s);
        }

        return 0;
    }

    public bool Contains(char value)
    {
        return Value.Contains(value);
    }

    public bool Contains(string value)
    {
        return Value.Contains(value);
    }

    public bool Contains(string value, StringComparison stringComparison)
    {
        return Value.Contains(value, stringComparison);
    }

    public bool EndsWith(string end, StringComparison c)
    {
        return Value.EndsWith(end, c);
    }

    public bool EndsWith(string s)
    {
        return Value.EndsWith(s);
    }

    public override bool Equals(object? obj)
    {
        if (obj is LocatedString ls)
        {
            return ls.Value == Value;
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public string Replace(string pattern, string replacement)
    {
        return Value.Replace(pattern, replacement);
    }

    public string ToCamelCase()
    {
        return Value.ToCamelCase();
    }

    public string ToKebabCase()
    {
        return Value.ToKebabCase();
    }

    public string ToLower()
    {
        return Value.ToLower();
    }

    public string ToPascalCase()
    {
        return Value.ToPascalCase();
    }

    public override string ToString()
    {
        return Value;
    }

    public string ToUpper()
    {
        return Value.ToUpper();
    }

    public string Trim()
    {
        return Value.Trim();
    }

    public string Trim(char value)
    {
        return Value.Trim(value);
    }
}
