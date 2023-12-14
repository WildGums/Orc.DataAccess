namespace Orc.DataAccess.Database;

public class DbObject
{
    public DbObject(TableType type)
    {
        Type = type;
        Name = string.Empty;
    }

    public string Name { get; set; }
    public TableType Type { get; }
}