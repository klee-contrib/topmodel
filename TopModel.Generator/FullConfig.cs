using TopModel.Generator.CSharp;
using TopModel.Generator.Javascript;
using TopModel.Generator.ProceduralSql;
using TopModel.Generator.Ssdt;

namespace TopModel.Generator
{
    public class FullConfig : ModelConfig
    {
        public ProceduralSqlConfig? ProceduralSql { get; set; }

        public SsdtConfig? Ssdt { get; set; }

        public JavascriptConfig? Javascript { get; set; }

        public CSharpConfig? Csharp { get; set; }
    }
}
