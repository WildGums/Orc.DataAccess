namespace Orc.DataAccess.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using DataAccess;

    [ConnectToProvider("System.Data.SQLite")]
    public class SqLiteSourceGateway : SqlDbSourceGatewayBase
    {
        public SqLiteSourceGateway(DatabaseSource source)
            : base(source)
        {
        }

#pragma warning disable IDISP012 // Property should not return created disposable.
        protected override Dictionary<TableType, Func<DbConnection, DbCommand>> GetObjectListCommandsFactory =>
            new()
            {
                {TableType.Table, c => c.CreateCommand($"SELECT name FROM sqlite_master WHERE type ='table' AND name NOT LIKE 'sqlite_%';")},
                {TableType.View, c => c.CreateCommand($"SELECT name FROM sqlite_master WHERE type ='view' AND name NOT LIKE 'sqlite_%';")}
            };
#pragma warning restore IDISP012 // Property should not return created disposable.

        protected override DbCommand CreateGetTableRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled)
        {
            ArgumentNullException.ThrowIfNull(connection);

            var source = Source;
            string query;
            if (isPagingEnabled)
            {
                query = offset == 0
                    ? $"SELECT * FROM \"{source.Table}\" LIMIT {fetchCount}"
                    : $"SELECT * FROM \"{source.Table}\" LIMIT {fetchCount} OFFSET {offset}";
            }
            else
            {
                query = $"SELECT * FROM \"{source.Table}\"";
            }

            return connection.CreateCommand(query);
        }

        protected override DbCommand CreateTableCountCommand(DbConnection connection)
        {
            ArgumentNullException.ThrowIfNull(connection);

            return connection.CreateCommand($"SELECT COUNT(*) AS \"count\" FROM \"{Source.Table}\"");
        }

        public override DataSourceParameters GetQueryParameters()
        {
            // Note:Vladimir: Not supported by SqLite
            return new DataSourceParameters();
        }
    }
}
