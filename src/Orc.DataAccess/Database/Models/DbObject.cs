namespace Orc.DataAccess.Database
{
    public class DbObject
    {
        public DbObject(TableType type)
        {
            Type = type;
        }

        public string Name { get; set; }
        public TableType Type { get; }
    }
}
