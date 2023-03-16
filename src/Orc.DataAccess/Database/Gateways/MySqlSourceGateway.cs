namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data.Common;
using DataAccess;

[ConnectToProvider("MySql.Data.MySqlClient")]
public class MySqlSourceGateway : SqlDbSourceGatewayBase
{
    public MySqlSourceGateway(DatabaseSource source) : base(source)
    {
    }

#pragma warning disable IDISP012 // Property should not return created disposable.
    protected override Dictionary<TableType, Func<DbConnection, DbCommand>> GetObjectListCommandsFactory =>
        new()
        {
            {TableType.Table, c => c.CreateCommand($"SELECT TABLE_NAME AS NAME FROM information_schema.tables where Table_schema = database() and Table_type = 'BASE TABLE';")},
            {TableType.View, c => c.CreateCommand($"SELECT TABLE_NAME AS NAME FROM information_schema.tables where Table_schema = database() and Table_type = 'VIEW';")},
            {TableType.StoredProcedure, c => c.CreateCommand($"SELECT SPECIFIC_NAME AS NAME FROM INFORMATION_SCHEMA.ROUTINES where ROUTINE_SCHEMA = database() and ROUTINE_TYPE = 'PROCEDURE';")},
        };
#pragma warning restore IDISP012 // Property should not return created disposable.

    protected override Dictionary<TableType, Func<DataSourceParameters>> DataSourceParametersFactory => new()
    {
        {TableType.StoredProcedure, () => GetArgs(GetArgsQuery)},
        {TableType.Function, () => GetArgs(GetArgsQuery)},
    };

    private string GetArgsQuery => $"SELECT PARAMETER_NAME AS NAME, DATA_TYPE AS TYPE FROM information_schema.parameters WHERE SPECIFIC_NAME = '{Source.Table}' and PARAMETER_MODE = 'IN';";

    protected override DbCommand CreateGetTableRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled)
    {
        ArgumentNullException.ThrowIfNull(connection);

        var source = Source;
        string query;
        if (isPagingEnabled)
        {
            query = offset == 0
                ? $"SELECT * FROM `{source.Table}` LIMIT {fetchCount}"
                : $"SELECT * FROM `{source.Table}` LIMIT {fetchCount} OFFSET {offset}";
        }
        else
        {
            query = $"SELECT * FROM `{source.Table}`";
        }

        return connection.CreateCommand(query);
    }

    protected override DbCommand CreateGetFunctionRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount)
    {
        ArgumentNullException.ThrowIfNull(connection);

        throw new NotSupportedException("Table valued function in MySql not supported");
    }

    protected override DbCommand CreateTableCountCommand(DbConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);

        return connection.CreateCommand($"SELECT COUNT(*) AS `count` FROM `{Source.Table}`");
    }
}
