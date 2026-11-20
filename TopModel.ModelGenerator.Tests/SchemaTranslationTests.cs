using TopModel.Utils.Tests;

namespace TopModel.ModelGenerator.Tests;

[TestClass]
public class SchemaTranslationTests
{
    [TestMethod]
    public void SchemaTranslation_HasIdenticalStructureAndValues()
    {
        SchemaTranslationAssertions.AssertEquivalent("schema.tmdgen.config.json", "schema.tmdgen.config.fr.json");
    }
}
