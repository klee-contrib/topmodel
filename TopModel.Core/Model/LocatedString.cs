#nullable disable
using TopModel.Core.FileModel;
using TopModel.Utils;
using YamlDotNet.Core.Events;

namespace TopModel.Core;

public class LocatedString : IComparable
{
    public LocatedString(Scalar value)
    {
        Value = value.Value;
        Location = new Reference(value);
    }

    public string Value { get; init; }

    internal Reference Location { get; }

    public static implicit operator string(LocatedString ls)
    {
        return ls?.Value;
    }

    public static bool operator ==(LocatedString ls1, LocatedString ls2)
    {
        return ls1?.Value == ls2?.Value;
    }

    public static bool operator !=(LocatedString ls1, LocatedString ls2)
    {
        return ls1?.Value != ls2?.Value;
    }

    public int CompareTo(object obj)
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

    public bool EndsWith(string s)
    {
        return Value.EndsWith(s);
    }

    public string Replace(string pattern, string replacement)
    {
        return Value.Replace(pattern, replacement);
    }

    public override bool Equals(object obj)
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

    public string ToKebabCase()
    {
        return Value.ToKebabCase();
    }

    public string ToCamelCase()
    {
        return Value.ToCamelCase();
    }

    public string ToLower()
    {
        return Value.ToLower();
    }

    public bool EndsWith(string end, StringComparison c)
    {
        return Value.EndsWith(end, c);
    }

    public override string ToString()
    {
        return Value;
    }
}
