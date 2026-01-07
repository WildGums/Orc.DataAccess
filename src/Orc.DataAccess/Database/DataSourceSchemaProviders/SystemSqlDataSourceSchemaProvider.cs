namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.DependencyInjection;

[ConnectToProvider("System.Data.SqlClient")]
public class SystemSqlDataSourceSchemaProvider : IDataSourceSchemaProvider
{
    private readonly IServiceProvider _serviceProvider;

    public SystemSqlDataSourceSchemaProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public DbDataSourceSchema? GetSchema(DbConnectionString connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        var provider = DbProvider.GetRegisteredProvider(connectionString.DbProvider.InvariantName, _serviceProvider);
        var databases = new List<string>();
        using var sqlConnection = provider.CreateConnection();
        if (sqlConnection is null)
        {
            return null;
        }

        sqlConnection.ConnectionString = connectionString.ToString();
        sqlConnection.Open();

        using var command = sqlConnection.CreateCommand();
        command.Connection = sqlConnection;
        command.CommandText = "SELECT name from [sys].[databases]";
        command.CommandType = CommandType.Text;

        using var dataReader = command.ExecuteReader();
        while (dataReader.Read())
        {
            var readValue = dataReader[0].ToString();
            if (string.IsNullOrEmpty(readValue))
            {
                continue;
            }
            databases.Add(readValue);
        }

        return new DbDataSourceSchema 
        {
            Databases = databases
        };
    }
}
