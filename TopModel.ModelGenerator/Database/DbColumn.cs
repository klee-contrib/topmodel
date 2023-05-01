namespace TopModel.ModelGenerator.Database;

public class DbColumn
{
    #nullable disable
    public string TableName { get; set; }

    public string ColumnName { get; set; }

    public string DataType { get; set; }

    public string Precision { get; set; }

    public string Scale { get; set; }

    public bool Nullable { get; set; }
}
