namespace TopModel.ModelGenerator;

public class TmdProperty
{
#nullable disable
    public string Name { get; set; }

    public TmdClass Class { get; set; }

    public string SqlName { get; set; }

    public string Domain { get; set; }

    public bool Required { get; set; }

    public bool PrimaryKey { get; set; }
}