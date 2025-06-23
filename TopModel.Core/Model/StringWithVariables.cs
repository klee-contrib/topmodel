using System.Text.RegularExpressions;
using TopModel.Core.FileModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core;

public class StringWithVariables : LocatedString
{
    public StringWithVariables(Scalar value)
        : base(value)
    {
        var isQuoted = value.Style == ScalarStyle.SingleQuoted || value.Style == ScalarStyle.DoubleQuoted ? 1 : 0;
        var regex = new Regex(@"(\{[$a-zA-Z0-9:.\[\]]+\})");

        References = regex.Matches(Value).Cast<Match>()
            .SelectMany(match =>
            {
                var start = new Mark(Location.Start.Index, Location.Start.Line, Location.Start.Column + isQuoted + match.Index + 1);
                Mark end;

                return match.Value.Trim('{', '}').Split(':').Select((refName, i) =>
                {
                    end = new Mark(start.Index, start.Line, start.Column + refName.Length);
                    Reference reference = i == 0
                        ? new ParameterReference { Start = start, End = end, ReferenceName = refName }
                        : new TransformReference { Start = start, End = end, ReferenceName = refName };
                    start = new Mark(end.Index, end.Line, end.Column + 1);
                    return reference;
                });
            })
            .ToList();
    }

    public IEnumerable<ParameterReference> Variables => References.OfType<ParameterReference>();

    public IEnumerable<TransformReference> Transforms => References.OfType<TransformReference>();

    private IList<Reference> References { get; }
}
