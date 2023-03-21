namespace Orc.DataAccess;

public class DataSourceParameter
{
    public DataSourceParameter()
    {
        Name = string.Empty;
        Type = string.Empty;
    }

    public string Name { get; set; }
    public string Type { get; set; }
    public object? Value { get; set; }
}