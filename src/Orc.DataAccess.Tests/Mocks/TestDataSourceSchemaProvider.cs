namespace Orc.DataAccess.Tests;

using Database;

public class TestDataSourceSchemaProvider : IDataSourceSchemaProvider
{
    private readonly DbDataSourceSchema _dataSourceSchema = new();

    public void AddDataSource(string name)
    {
        _dataSourceSchema.Databases.Add(name);
    }

    public DbDataSourceSchema? GetSchema(DbConnectionString connectionString) => _dataSourceSchema;
}