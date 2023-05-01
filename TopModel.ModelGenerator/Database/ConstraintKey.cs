namespace TopModel.ModelGenerator.Database;

public class ConstraintKey
{
    #nullable disable
    public string Name { get; set; }

    public string TableName { get; set; }

    public string ColumnName { get; set; }

    public string ForeignTableName { get; set; }

    public string ForeignColumnName { get; set; }
}
