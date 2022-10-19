namespace Orc.DataAccess.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using Catel;
    using Catel.Logging;

    internal static class SqlConnectionExtensions
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

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

        public static DbProvider GetDbProvider(this DbConnection connection)
        {
            ArgumentNullException.ThrowIfNull(connection);

            var connectionType = connection.GetType();
            return GetProviderByConnectionType(connectionType);
        }

        private static DbProvider GetProviderByConnectionType(Type connectionType)
        {
            ArgumentNullException.ThrowIfNull(connectionType);

            if (ConnectionTypeToProvider.TryGetValue(connectionType, out var dbProvider))
            {
                return dbProvider;
            }

            var dbProviders = DbProvider.GetRegisteredProviders();
            foreach (var currentProvider in dbProviders.Values)
            {
                if (currentProvider.ConnectionType == connectionType)
                {
                    ConnectionTypeToProvider[connectionType] = currentProvider;
                    return currentProvider;
                }
            }

            throw Log.ErrorAndCreateException<InvalidOperationException>($"Failed to obtain '{nameof(DbProviderInfo)}'");
        }
    }
}
