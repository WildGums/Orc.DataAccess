namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Catel;
using Catel.Logging;
using Microsoft.Extensions.Logging;

internal static class SqlConnectionExtensions
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(SqlConnectionExtensions));

    private static readonly Dictionary<Type, DbProvider> ConnectionTypeToProvider = new();

    public static DbDataReader GetReaderSql(this DbConnection connection, string sql, int? commandTimeout = null)
    {
        ArgumentNullException.ThrowIfNull(connection);
        Argument.IsNotNullOrEmpty(() => sql);

        return connection.GetReader(sql, CommandType.Text, commandTimeout);
    }

    public static DbDataReader GetReader(this DbConnection connection, string sql, CommandType commandType = CommandType.Text,
        int? commandTimeout = null)
    {
        ArgumentNullException.ThrowIfNull(connection);
        Argument.IsNotNullOrEmpty(() => sql);

        var command = connection.CreateCommand(sql, commandType, commandTimeout);
        return command.ExecuteReader();
    }

    public static DbCommand CreateCommand(this DbConnection connection, string sql, CommandType commandType = CommandType.Text, int? commandTimeout = null)
    {
        ArgumentNullException.ThrowIfNull(connection);
        Argument.IsNotNullOrEmpty(() => sql);

        var command = connection.CreateCommand();
        command.CommandType = commandType;
        command.CommandText = sql;
        if (commandTimeout.HasValue)
        {
            command.CommandTimeout = commandTimeout.Value;
        }

        return command;
    }

    public static DbProvider GetDbProvider(this DbConnection connection, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(connection);

        var connectionType = connection.GetType();
        return GetProviderByConnectionType(connectionType, serviceProvider);
    }

    private static DbProvider GetProviderByConnectionType(Type connectionType, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(connectionType);

        if (ConnectionTypeToProvider.TryGetValue(connectionType, out var dbProvider))
        {
            return dbProvider;
        }

        var dbProviders = DbProvider.GetRegisteredProviders(serviceProvider);
        foreach (var currentProvider in dbProviders.Values)
        {
            if (currentProvider.ConnectionType != connectionType)
            {
                continue;
            }

            ConnectionTypeToProvider[connectionType] = currentProvider;
            return currentProvider;
        }

        throw Logger.LogErrorAndCreateException<InvalidOperationException>("Failed to obtain 'DbProviderInfo'");
    }
}
