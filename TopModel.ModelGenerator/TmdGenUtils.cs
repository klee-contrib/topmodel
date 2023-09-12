using System.Text.RegularExpressions;

namespace TopModel.ModelGenerator;

public static class TmdGenUtils
{
    public static string GetDomainString(IList<DomainMapping> domains, string? type = null, string? name = null, string? scale = null, string? precision = null)
    {
        return domains.Select(d =>
        {
            var score = 0;
            if (d.Name != null && name != null)
            {
                if (d.Name.StartsWith('/'))
                {
                    if (Regex.IsMatch(name, d.Name[1..^1]))
                    {
                        score += 10000;
                    }
                }
                else
                {
                    if (name == d.Name)
                    {
                        score += 100000;
                    }
                }
            }
            else if (d.Type != null && type != null)
            {
                if (d.Type.StartsWith('/'))
                {
                    if (Regex.IsMatch(type, d.Type[1..^1]))
                    {
                        score += 100;
                    }
                }
                else if (type == d.Type)
                {
                    score += 1000;
                }
            }

            if (d.Scale == scale && scale != null)
            {
                score += 10;
            }

            if (d.Precision == precision && scale != null)
            {
                score += 1;
            }

            return (score, d.Domain);
        }).Where(t => t.score >= 10).OrderByDescending(t => t.score).FirstOrDefault().Domain ?? name ?? type ?? string.Empty;
    }
}
