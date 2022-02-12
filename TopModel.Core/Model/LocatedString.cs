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

    public string Value { get; }

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

    public string ToDashCase()
    {
        return Value.ToDashCase();
    }

    public string ToFirstLower()
    {
        return Value.ToFirstLower();
    }

    public string ToFirstUpper()
    {
        return Value.ToFirstUpper();
    }

    public string ToLower()
    {
        return Value.ToLower();
    }

    public override string ToString()
    {
        return Value;
    }
}