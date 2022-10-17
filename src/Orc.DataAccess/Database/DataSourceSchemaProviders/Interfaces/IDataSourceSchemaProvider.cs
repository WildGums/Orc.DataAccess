namespace Orc.DataAccess.Database
{
    public interface IDataSourceSchemaProvider
    {
        DbDataSourceSchema GetSchema(DbConnectionString connectionString);
    }
}
