namespace TopModel.ModelGenerator.Tests;

[TestClass]
public class TmdGenUtilsTests
{
    [TestMethod]
    public void DomainNamedLikePropertyIsKept()
    {
        var domains = Domains(new DomainMapping { Name = "/status/", Domain = "status" });

        Assert.AreEqual("status", TmdGenUtils.FindDomain(domains, name: "status"));
    }

    [TestMethod]
    public void ExactNameBeatsRegexName()
    {
        var domains = Domains(
            new DomainMapping { Name = "/Id$/", Domain = "DO_ID" },
            new DomainMapping { Name = "userId", Domain = "DO_USER_ID" });

        Assert.AreEqual("DO_USER_ID", TmdGenUtils.FindDomain(domains, name: "userId"));
    }

    [TestMethod]
    public void ExactNameIsCaseInsensitive()
    {
        var domains = Domains(new DomainMapping { Name = "email", Domain = "DO_EMAIL" });

        Assert.AreEqual("DO_EMAIL", TmdGenUtils.FindDomain(domains, name: "EMAIL"));
    }

    [TestMethod]
    public void ExactTypeBeatsRegexType()
    {
        var domains = Domains(
            new DomainMapping { Type = "/char/", Domain = "DO_TEXTE" },
            new DomainMapping { Type = "varchar", Domain = "DO_LIBELLE" });

        Assert.AreEqual("DO_LIBELLE", TmdGenUtils.FindDomain(domains, type: "varchar"));
        Assert.AreEqual("DO_TEXTE", TmdGenUtils.FindDomain(domains, type: "character varying"));
    }

    [TestMethod]
    public void FirstDeclaredWinsOnTie()
    {
        var domains = Domains(
            new DomainMapping { Type = "int", Domain = "DO_FIRST" },
            new DomainMapping { Type = "int", Domain = "DO_SECOND" });

        Assert.AreEqual("DO_FIRST", TmdGenUtils.FindDomain(domains, type: "int"));
    }

    [TestMethod]
    public void GetDomainStringFallsBackToNameThenType()
    {
        var domains = Domains();

        Assert.AreEqual("libelle", TmdGenUtils.GetDomainString(domains, name: "libelle"));
        Assert.AreEqual("varchar", TmdGenUtils.GetDomainString(domains, type: "varchar"));
    }

    [TestMethod]
    public void PrecisionBonusDoesNotDependOnScale()
    {
        var domains = Domains(
            new DomainMapping { Type = "numeric", Domain = "DO_NUM" },
            new DomainMapping { Type = "numeric", Precision = "2", Domain = "DO_NUM_2" });

        Assert.AreEqual("DO_NUM_2", TmdGenUtils.FindDomain(domains, type: "numeric", precision: "2"));
        Assert.AreEqual("DO_NUM", TmdGenUtils.FindDomain(domains, type: "numeric", scale: "10"));
    }

    [TestMethod]
    public void RegexKeepsInnerTrailingSlash()
    {
        var domains = Domains(new DomainMapping { Type = "/a\\//", Domain = "DO_A" });

        Assert.AreEqual("DO_A", TmdGenUtils.FindDomain(domains, type: "a/"));
        Assert.IsNull(TmdGenUtils.FindDomain(domains, type: "a"));
    }

    [TestMethod]
    public void RegexNameIsUnanchoredAndCaseSensitive()
    {
        var domains = Domains(new DomainMapping { Name = "/Id$/", Domain = "DO_ID" });

        Assert.AreEqual("DO_ID", TmdGenUtils.FindDomain(domains, name: "userId"));
        Assert.IsNull(TmdGenUtils.FindDomain(domains, name: "user_id"));
    }

    [TestMethod]
    public void RegexWithoutClosingSlashIsAccepted()
    {
        var domains = Domains(new DomainMapping { Name = "/^code", Domain = "DO_CODE" });

        Assert.AreEqual("DO_CODE", TmdGenUtils.FindDomain(domains, name: "codePays"));
    }

    [TestMethod]
    public void ScaleAloneDoesNotMatch()
    {
        var domains = Domains(
            new DomainMapping { Type = "numeric", Scale = "10", Domain = "DO_NUM" },
            new DomainMapping { Type = "varchar", Domain = "DO_LIBELLE" });

        Assert.IsNull(TmdGenUtils.FindDomain(domains, name: "libelle", scale: "10"));
        Assert.AreEqual("DO_LIBELLE", TmdGenUtils.FindDomain(domains, type: "varchar", scale: "10"));
    }

    [TestMethod]
    public void ScaleAndPrecisionBreakTies()
    {
        var domains = Domains(
            new DomainMapping { Type = "numeric", Domain = "DO_NUM" },
            new DomainMapping { Type = "numeric", Scale = "10", Domain = "DO_NUM_10" },
            new DomainMapping { Type = "numeric", Scale = "10", Precision = "2", Domain = "DO_NUM_10_2" });

        Assert.AreEqual("DO_NUM", TmdGenUtils.FindDomain(domains, type: "numeric", scale: "5"));
        Assert.AreEqual("DO_NUM_10", TmdGenUtils.FindDomain(domains, type: "numeric", scale: "10"));
        Assert.AreEqual("DO_NUM_10_2", TmdGenUtils.FindDomain(domains, type: "numeric", scale: "10", precision: "2"));
    }

    private static List<DomainMapping> Domains(params DomainMapping[] domains) => [.. domains];
}
