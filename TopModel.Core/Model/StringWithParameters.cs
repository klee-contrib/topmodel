using System.Text.RegularExpressions;
using TopModel.Core.FileModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core;

public class StringWithParameters : LocatedString
{
    public StringWithParameters(Scalar value)
        : base(value)
    {
        var isQuoted = value.Style == ScalarStyle.SingleQuoted || value.Style == ScalarStyle.DoubleQuoted ? 1 : 0;
        var regex = new Regex(@"(\{[$a-zA-Z0-9:.\[\]]+\})");
        Parameters = regex.Matches(Value).Cast<Match>()
            .Select(match =>
            {
                var refName = match.Value.Trim('{', '}').Split(':').First();
                return new ParameterReference
                {
                    Start = new Mark(Location.Start.Index, Location.Start.Line, Location.Start.Column + isQuoted + match.Index + 1),
                    End = new Mark(Location.Start.Index, Location.Start.Line, Location.Start.Column + isQuoted + match.Index + 1 + refName.Length),
                    ReferenceName = refName
                };
            })
            .ToList();
    }

    public IList<ParameterReference> Parameters { get; }
}
