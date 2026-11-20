namespace TopModel.ModelGenerator.Database;

public class ConstraintKey
{
    public required string Name { get; set; }

    public required string TableName { get; set; }

    public required string ColumnName { get; set; }

    public required string ForeignTableName { get; set; }

    public required string ForeignColumnName { get; set; }
}
