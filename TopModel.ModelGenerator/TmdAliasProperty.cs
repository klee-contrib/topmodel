namespace TopModel.ModelGenerator;

public class TmdAliasProperty : TmdProperty
{
    public bool Required { get; set; }

    public required TmdProperty Alias { get; set; }

    public string? As { get; set; }
}