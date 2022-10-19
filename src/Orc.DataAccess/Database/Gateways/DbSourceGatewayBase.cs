namespace Orc.DataAccess.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using Catel.Logging;

#pragma warning disable IDISP025 // Class with no virtual dispose method should be sealed.
    public abstract class DbSourceGatewayBase : IDisposable
#pragma warning restore IDISP025 // Class with no virtual dispose method should be sealed.
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private DbConnection? _connection;
        private DbProvider? _provider;

        private readonly List<DbConnection> _openedConnections = new();

        protected DbSourceGatewayBase(DatabaseSource source)
        {
            ArgumentNullException.ThrowIfNull(source);

            Source = source;
        }

        public void Dispose()
        {
            Close();

            Connection?.Dispose();

            foreach (var openedConnection in _openedConnections)
            {
                openedConnection.Dispose();
            }
        }

        public DatabaseSource Source { get; }
        public virtual DbProvider? Provider => _provider ??= Source.GetProvider();
        public virtual DbConnection? Connection => _connection ??= Provider?.CreateConnection(Source);

        public abstract DbDataReader GetRecords(DataSourceParameters queryParameters, int offset = 0, int fetchCount = -1);
        public abstract long GetCount(DataSourceParameters queryParameters);
        public abstract DataSourceParameters GetQueryParameters();
        public abstract IList<DbObject> GetObjects();

        protected DbConnection GetOpenedConnection()
        {
            var connection = Connection;
            if (connection is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Failed to get opened connection. No connection to source is already opened or can be created in this time");
            }

            if (connection.State.HasFlag(System.Data.ConnectionState.Open))
            {
                var newConnection = Provider?.CreateConnection(Source);
                if (newConnection is null)
                {
                    throw Log.ErrorAndCreateException<InvalidOperationException>("Failed to get opened connection. No connection to source is already opened or can be created in this time");
                }
                newConnection.Open();

                _openedConnections.Add(newConnection);
                return newConnection;
            }

            if (!connection.State.HasFlag(System.Data.ConnectionState.Open))
            {
                connection.Open();
            }

            return connection;
        }

        public void Close()
        {
            Connection?.Close();

            foreach (var openedConnection in _openedConnections)
            {
                openedConnection.Close();
            }
        }
    }
}
