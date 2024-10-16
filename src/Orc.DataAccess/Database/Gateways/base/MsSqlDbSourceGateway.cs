namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data.Common;
using Catel;
using DataAccess;

public abstract class MsSqlDbSourceGatewayBase : SqlDbSourceGatewayBase
{
    public MsSqlDbSourceGatewayBase(DatabaseSource source)
        : base(source)
    {
    }

#pragma warning disable IDISP012 // Property should not return created disposable.
    protected override Dictionary<TableType, Func<DbConnection, DbCommand>> GetObjectListCommandsFactory =>
        new()
        {
            {TableType.Table, c => CreateGetObjectsCommand(c, "U")},
            {TableType.View, c => CreateGetObjectsCommand(c, "V")},
            {TableType.StoredProcedure, c => CreateGetObjectsCommand(c, "P")},
            {TableType.Function, c => CreateGetObjectsCommand(c, "IF")},
        };
#pragma warning restore IDISP012 // Property should not return created disposable.

    protected override Dictionary<TableType, Func<DataSourceParameters>> DataSourceParametersFactory => new()
    {
        {TableType.StoredProcedure, () => GetArgs(GetArgsQuery)},
        {TableType.Function, () => GetArgs(GetArgsQuery)},
    };

    private string GetArgsQuery => $"SELECT [name], type_name(user_type_id) as type FROM [sys].[parameters] WHERE [object_id] = object_id('{GetFullTableName(Source)}')";

    protected override DbCommand CreateGetTableRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled)
    {
        ArgumentNullException.ThrowIfNull(connection);

        var fullTableName = GetFullTableName(Source);
        string query;
        if (isPagingEnabled)
        {
            query = offset == 0
                ? $"SELECT TOP ({fetchCount}) * FROM {fullTableName}"
                : $"SELECT * FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT 0)) AS [row_num] FROM {fullTableName}) AS [results_wrapper] WHERE [row_num] BETWEEN {offset + 1} AND {offset + fetchCount}";
        }
        else
        {
            query = $"SELECT * FROM {fullTableName}";
        }

        return connection.CreateCommand(query);
    }

    protected override DbCommand CreateTableCountCommand(DbConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);

        return connection.CreateCommand($"SELECT COUNT(*) AS [count] FROM {GetFullTableName(Source)}");
    }

    private static DbCommand CreateGetObjectsCommand(DbConnection connection, string commandParameter)
    {
        ArgumentNullException.ThrowIfNull(connection);
        Argument.IsNotNullOrEmpty(() => commandParameter);

        return connection.CreateCommand($"SELECT name FROM dbo.sysobjects WHERE uid = 1 AND type = '{commandParameter}' ORDER BY name;");
    }

    private static string GetFullTableName(DatabaseSource source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return string.IsNullOrWhiteSpace(source.Schema) ? $"[{source.Table}]" : $"[{source.Schema}].[{source.Table}]";
    }
}
