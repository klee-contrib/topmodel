namespace TopModel.Core.Model;

public static class NameSegmentsExtensions
{
    extension(IList<NameSegment> nameSegments)
    {
        public string Name => string.Concat(nameSegments.Select(ns => ns.Render(pascalCase: false)));

        public string NamePascal => string.Concat(nameSegments.Select(ns => ns.Render(pascalCase: true)));

        public IEnumerable<(int Offset, int Length, object Target)> Targets
        {
            get
            {
                var offset = 0;
                foreach (var segment in nameSegments)
                {
                    var length = segment.Render(pascalCase: false).Length;
                    switch (segment)
                    {
                        case NameSegment.Text te:
                            yield return (offset, length, te.Raw);
                            break;
                        case NameSegment.ClassName cn:
                            yield return (offset, length, cn.Class);
                            break;
                        case NameSegment.PropertyName { Property: { } property }:
                            yield return (offset, length, property);
                            foreach (var (subOffset, subLength, target) in property.NameSegments.Targets)
                            {
                                yield return (offset + subOffset, subLength, target);
                            }

                            break;
                    }

                    offset += length;
                }
            }
        }
    }
}
