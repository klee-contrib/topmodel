using TopModel.Core;
using TopModel.Generator.Csharp;
using TopModel.Generator.Javascript;
using TopModel.Generator.Jpa;
using TopModel.Generator.Sql;
using TopModel.Generator.Translation;

namespace TopModel.Generator;

public class FullConfig : ModelConfig
{
    public IList<SqlConfig>? Sql { get; set; }

    public IList<JavascriptConfig>? Javascript { get; set; }

    public IList<CsharpConfig>? Csharp { get; set; }

    public IList<JpaConfig>? Jpa { get; set; }

    public IList<TranslationConfig>? Translation { get; set; }
}