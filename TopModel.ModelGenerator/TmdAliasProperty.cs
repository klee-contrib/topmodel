namespace TopModel.ModelGenerator;

public class TmdAliasProperty : TmdProperty
{
#nullable disable
    public bool Required { get; set; }

    public TmdProperty Alias { get; set; }

    public string Property { get; set; }

    public string As { get; set; }
}