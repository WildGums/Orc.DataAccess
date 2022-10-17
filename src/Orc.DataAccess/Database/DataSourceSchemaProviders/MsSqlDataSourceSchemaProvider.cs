namespace Orc.DataAccess.Database
{
    using System.Collections.Generic;
    using System.Data;

    [ConnectToProvider("System.Data.SqlClient")]
    public class MsSqlDataSourceSchemaProvider : IDataSourceSchemaProvider
    {
        public DbDataSourceSchema GetSchema(DbConnectionString connectionString)
        {
            var provider = DbProvider.GetRegisteredProvider(connectionString.DbProvider.InvariantName);
            if (provider is null)
            {
                return null;
            }

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
            command.CommandText = "SELECT name from sys.databases";
            command.CommandType = CommandType.Text;

            using var dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                databases.Add(dataReader[0].ToString());
            }

            return new DbDataSourceSchema {Databases = databases};
        }
    }
}
