using System.Text.RegularExpressions;

namespace TopModel.ModelGenerator;

static class TmdGenUtils
{
    public static string GetDomainString(IList<DomainMapping> domains, string nameOrType, string? scale = null, string? precision = null)
    {
        return domains.Select(d =>
        {
            var score = 0;
            if (d.Name != null)
            {
                if (d.Name.StartsWith('/'))
                {
                    if (Regex.IsMatch(nameOrType, d.Name[1..^1]))
                    {
                        score += 10000;
                    }
                }
                else
                {
                    if (nameOrType == d.Name)
                    {
                        score += 100000;
                    }
                }
            }
            else if (d.Type != null)
            {
                if (d.Type.StartsWith('/'))
                {
                    if (Regex.IsMatch(nameOrType, d.Type[1..^1]))
                    {
                        score += 100;
                    }
                }
                else if (nameOrType == d.Type)
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

            return (score, Domain: d.Domain);
        }).Where(t => t.score >= 10).OrderByDescending(t => t.score).FirstOrDefault().Domain ?? nameOrType;
    }
}
