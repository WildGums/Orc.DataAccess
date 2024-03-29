﻿namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Catel;
using Catel.Logging;
using DataAccess;

public abstract class SqlDbSourceGatewayBase : DbSourceGatewayBase
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    protected SqlDbSourceGatewayBase(DatabaseSource source)
        : base(source)
    {
    }

    protected virtual Dictionary<TableType, Func<DbConnection, DbCommand>> GetObjectListCommandsFactory => new();
    protected virtual Dictionary<TableType, Func<DataSourceParameters>> DataSourceParametersFactory => new();

    public override IList<DbObject> GetObjects()
    {
        var source = Source;
        if (!GetObjectListCommandsFactory.TryGetValue(source.TableType, out var commandFactory))
        {
            return new List<DbObject>();
        }

#pragma warning disable IDISP001 // Dispose created.
        var connection = GetOpenedConnection();
#pragma warning restore IDISP001 // Dispose created.

        var command = commandFactory(connection);
        return ReadAllDbObjects(command);
    }

    public override DataSourceParameters GetQueryParameters()
    {
        var source = Source;
        return DataSourceParametersFactory.TryGetValue(source.TableType, out var parametersFactory)
            ? parametersFactory()
            : new DataSourceParameters();
    }

    public override DbDataReader GetRecords(DataSourceParameters queryParameters, int offset = 0, int fetchCount = -1)
    {
#pragma warning disable IDISP001 // Dispose created.
        var connection = GetOpenedConnection();
#pragma warning restore IDISP001 // Dispose created.
        var source = Source;
        var isPagingQuery = offset >= 0 && fetchCount > 0;

        var sql = source.Table;
        if (string.IsNullOrEmpty(sql))
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Command cannot be empty");
        }

        DbCommand command;
        switch (source.TableType)
        {
            case TableType.Table:
                command = CreateGetTableRecordsCommand(connection, queryParameters, offset, fetchCount);
                break;

            case TableType.View:
                command = CreateGetViewRecordsCommand(connection, queryParameters, offset, fetchCount);
                break;

            case TableType.StoredProcedure:
                command = CreateGetStoredProcedureRecordsCommand(connection, queryParameters, offset, fetchCount);
                break;

            case TableType.Function:
                command = CreateGetFunctionRecordsCommand(connection, queryParameters, offset, fetchCount);
                break;

            case TableType.Sql:
                command = connection.CreateCommand(sql);
                break;

            default:
                throw Log.ErrorAndCreateException<NotSupportedException>($"'{source.TableType}' not supported");
        }

        command.AddParameters(queryParameters);

        var reader = command.ExecuteReader();
        if (isPagingQuery && source.TableType is TableType.Sql or TableType.StoredProcedure or TableType.Function)
        {
            return new SkipTakeDbReader(reader, offset, fetchCount);
        }

        return reader;
    }

    protected abstract DbCommand CreateGetTableRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled);

    private DbCommand CreateGetTableRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount)
    {
        ArgumentNullException.ThrowIfNull(connection);

        var isPagingQuery = offset >= 0 && fetchCount > 0;
        return CreateGetTableRecordsCommand(connection, parameters, offset, fetchCount, isPagingQuery);
    }

    protected virtual DbCommand CreateGetViewRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled)
    {
        ArgumentNullException.ThrowIfNull(connection);

        return CreateGetTableRecordsCommand(connection, parameters, offset, fetchCount, isPagingEnabled);
    }

    private DbCommand CreateGetViewRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount)
    {
        ArgumentNullException.ThrowIfNull(connection);

        return CreateGetTableRecordsCommand(connection, parameters, offset, fetchCount);
    }

    protected virtual DbCommand CreateGetStoredProcedureRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount)
    {
        ArgumentNullException.ThrowIfNull(connection);

        return connection.CreateCommand(Source.Table, CommandType.StoredProcedure);
    }

    protected virtual DbCommand CreateGetFunctionRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount)
    {
        ArgumentNullException.ThrowIfNull(connection);

        return connection.CreateCommand($"SELECT * FROM {Source.Table}({parameters.ToArgsNamesString()})");
    }

    protected abstract DbCommand CreateTableCountCommand(DbConnection connection);

    protected virtual DbCommand CreateViewCountCommand(DbConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);

        return CreateTableCountCommand(connection);
    }

    public override long GetCount(DataSourceParameters queryParameters)
    {
        using var connection = GetOpenedConnection();
        if (connection is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("No connection is already opened or can be created to source");
        }

        switch (Source.TableType)
        {
            case TableType.Table:
                {
                    return Convert.ToInt64(CreateTableCountCommand(connection).ExecuteScalar());
                }

            case TableType.View:
                {
                    return Convert.ToInt64(CreateViewCountCommand(connection).ExecuteScalar());
                }

            case TableType.Function:
                {
                    using var command = CreateGetFunctionRecordsCommand(connection, queryParameters, -1, -1);
                    command.AddParameters(queryParameters);
                    var count = command.GetRecordsCount();

                    return count;
                }

            case TableType.StoredProcedure:
                {
                    using var command = CreateGetStoredProcedureRecordsCommand(connection, queryParameters, -1, -1);
                    command.AddParameters(queryParameters);
                    var count = command.GetRecordsCount();

                    return count;
                }

            case TableType.Sql:
                {
                    using var command = connection.CreateCommand(Source.Table);
                    command.AddParameters(queryParameters);
                    var count = command.GetRecordsCount();

                    return count;
                }

            default:
                {
                    return 0;
                }
        }
    }

    protected virtual IList<DbObject> ReadAllDbObjects(DbCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var dbObjects = new List<DbObject>();
        var tableType = Source.TableType;

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var dbObject = new DbObject(tableType) { Name = reader.GetString(0) };
            dbObjects.Add(dbObject);
        }

        return dbObjects;
    }

    protected DataSourceParameters GetArgs(string query)
    {
        Argument.IsNotNullOrEmpty(() => query);

        using var connection = GetOpenedConnection();
        var queryParameters = new DataSourceParameters();
        using var reader = connection.GetReader(query);

        while (reader.Read())
        {
            var args = new DataSourceParameter
            {
                Name = reader.GetValue(0).ToString() ?? string.Empty,
                Type = reader.GetValue(1).ToString() ?? string.Empty
            };

            queryParameters.Parameters.Add(args);
        }

        return queryParameters;
    }
}
