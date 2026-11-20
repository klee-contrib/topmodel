using System.Reflection;
using TopModel.Core.Loaders;
using TopModel.Utils;
using TopModel.Utils.Tests;

namespace TopModel.Core.Tests;

[TestClass]
public class SchemaTranslationTests
{
    [TestMethod]
    [DataRow(null, "schema.json")]
    [DataRow("fr", "schema.fr.json")]
    [DataRow("fr-FR", "schema.fr.json")]
    [DataRow("de", "schema.json")]
    public void GetLocalizedFilePath_UsesRequestedLanguageOrFallback(string language, string expectedFileName)
    {
        var filePath = Assembly.GetAssembly(typeof(FileChecker))!.GetLocalizedFilePath("schema.json", language);

        Assert.AreEqual(expectedFileName, Path.GetFileName(filePath));
    }

    [TestMethod]
    [DataRow("schema.json", "schema.fr.json")]
    [DataRow("schema.config.json", "schema.config.fr.json")]
    public void SchemaTranslations_HaveIdenticalStructureAndValues(string defaultFileName, string frenchFileName)
    {
        SchemaTranslationAssertions.AssertEquivalent(defaultFileName, frenchFileName);
    }
}
