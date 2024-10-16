﻿namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Catel.Logging;
using DataAccess;

[ConnectToProvider("Oracle.ManagedDataAccess.Client")]
public class OracleSourceGateway : SqlDbSourceGatewayBase
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    public OracleSourceGateway(DatabaseSource source)
        : base(source)
    {
    }

#pragma warning disable IDISP012 // Property should not return created disposable.
    protected override Dictionary<TableType, Func<DbConnection, DbCommand>> GetObjectListCommandsFactory =>
        new()
        {
            {TableType.Table, c => c.CreateCommand("SELECT table_name FROM user_tables")},
            {TableType.View, c => c.CreateCommand("SELECT view_name from user_views")},
            {TableType.StoredProcedure, c => c.CreateCommand("SELECT * FROM User_Procedures WHERE OBJECT_TYPE = 'PROCEDURE'")},
            {TableType.Function, c => c.CreateCommand("SELECT * FROM User_Procedures WHERE OBJECT_TYPE = 'FUNCTION'")},
        };
#pragma warning restore IDISP012 // Property should not return created disposable.

    protected override Dictionary<TableType, Func<DataSourceParameters>> DataSourceParametersFactory => new()
    {
        {TableType.StoredProcedure, () => GetArgs(GetArgsQuery)},
        {TableType.Function, () => GetArgs(GetArgsQuery)},
    };

    private string GetArgsQuery => $"SELECT ARGUMENT_NAME AS NAME, DATA_TYPE AS TYPE FROM USER_ARGUMENTS WHERE OBJECT_NAME = UPPER('{Source.Table}') AND IN_OUT = 'IN'";

    public override DataSourceParameters GetQueryParameters()
    {
        var source = Source;
        switch (source.TableType)
        {
            case TableType.Table:
                break;
            case TableType.View:
                break;
            case TableType.Sql:
                return new DataSourceParameters();

            case TableType.StoredProcedure:
            case TableType.Function:
                {
                    var query = $"SELECT ARGUMENT_NAME AS NAME, DATA_TYPE AS TYPE FROM USER_ARGUMENTS WHERE OBJECT_NAME = UPPER('{Source.Table}') AND IN_OUT = 'IN'";
                    return GetArgs(query);
                }

            default:
                throw Log.ErrorAndCreateException<NotSupportedException>($"'{source}' not supported in GetQueryParameters");
        }

        return new DataSourceParameters();
    }

    protected override DbCommand CreateGetTableRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled)
    {
        ArgumentNullException.ThrowIfNull(connection);

        var sql = $"SELECT * FROM {Source.Table}";
        if (isPagingEnabled)
        {
            sql = $"SELECT * FROM {Source.Table} ORDER BY (select COLUMN_NAME from ALL_TAB_COLUMNS where TABLE_NAME='{Source.Table}' FETCH FIRST 1 ROWS ONLY) OFFSET {offset} ROWS FETCH NEXT {fetchCount} ROWS ONLY";
        }

        return connection.CreateCommand(sql);
    }

    protected override DbCommand CreateGetStoredProcedureRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount)
    {
        ArgumentNullException.ThrowIfNull(connection);

        if (Source.Table is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Cannot create stored procedure command on null table");
        }

        return connection.CreateCommand(Source.Table, CommandType.StoredProcedure);
    }

    protected override DbCommand CreateGetFunctionRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount)
    {
        ArgumentNullException.ThrowIfNull(connection);

        return connection.CreateCommand($"SELECT * FROM table({Source.Table}({parameters.ToArgsNamesString(":")}))");
    }

    protected override DbCommand CreateTableCountCommand(DbConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);

        return connection.CreateCommand($"SELECT COUNT(*) \"count\" FROM \"{Source.Table}\"");
    }
}
