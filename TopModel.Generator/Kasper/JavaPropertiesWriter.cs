using System.Text;
using Microsoft.Extensions.Logging;
using TopModel.Utils;

namespace TopModel.Generator.Kasper;

public class JavaPropertiesWriter : FileWriter
{
    public JavaPropertiesWriter(string fileName, ILogger logger)
        : base(fileName, logger, CodePagesEncodingProvider.Instance.GetEncoding(1252)!)
    {
    }

    public override string StartCommentToken => "####";
}