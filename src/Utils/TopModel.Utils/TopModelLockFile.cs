namespace TopModel.Utils;

public class TopModelLockFile
{
    public required string Version { get; set; }

    public IDictionary<string, TopModelLockModule> Modules { get; set; } = new Dictionary<string, TopModelLockModule>();

    public IDictionary<string, string> Custom { get; set; } = new Dictionary<string, string>();

    public IList<string> GeneratedFiles { get; set; } = [];
}
