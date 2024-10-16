namespace Orc.DataAccess.Database;

using System;
using Catel;

public static class DbConnectionStringExtensions
{
    public static DbConnectionStringProperty? GetProperty(this DbConnectionString connectionString, string propertyName)
    {
        ArgumentNullException.ThrowIfNull(connectionString);
        Argument.IsNotNullOrEmpty(() => propertyName);

        var properties = connectionString.Properties;
        var upperInvariantPropertyName = propertyName.ToUpperInvariant();
        if (properties.TryGetValue(upperInvariantPropertyName, out var property))
        {
            return property;
        }

        return properties.TryGetValue(upperInvariantPropertyName.Replace(" ", string.Empty), out property) 
            ? property 
            : null;
    }

    //TODO: Make it async
    public static DbDataSourceSchema? GetDataSourceSchema(this DbConnectionString connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        var provider = connectionString.DbProvider;
        var schemaProvider = provider.GetProvider().GetOrCreateConnectedInstance<IDataSourceSchemaProvider>();
        return schemaProvider?.GetSchema(connectionString);
    }

    public static ConnectionState GetConnectionState(this DbConnectionString connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        var connectionStringStr = connectionString.ToString();
        if (string.IsNullOrWhiteSpace(connectionStringStr))
        {
            return ConnectionState.Invalid;
        }

        var connection = connectionString.DbProvider.GetProvider().CreateConnection();
        if (connection is null)
        {
            return ConnectionState.Invalid;
        }

        // Try to open
        try
        {
            connection.ConnectionString = connectionStringStr;
            connection.Open();
        }
        catch
        {
            return ConnectionState.Invalid;
        }
        finally
        {
            connection.Dispose();
        }

        return ConnectionState.Valid;
    }
}
