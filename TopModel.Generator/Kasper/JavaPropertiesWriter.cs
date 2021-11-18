using System.Text;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Kasper;

public class JavaPropertiesWriter : FileWriter
{
    public JavaPropertiesWriter(string fileName, ILogger logger)
        : base(fileName, logger, CodePagesEncodingProvider.Instance.GetEncoding(1252)!)
    {
    }

    protected override string StartCommentToken => "####";
}