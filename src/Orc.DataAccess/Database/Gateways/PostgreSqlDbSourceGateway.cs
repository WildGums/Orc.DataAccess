namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DataAccess;

[ConnectToProvider("Npgsql")]
public class PostgreSqlDbSourceGateway : SqlDbSourceGatewayBase
{
    public PostgreSqlDbSourceGateway(DatabaseSource source)
        : base(source)
    {
    }

#pragma warning disable IDISP012 // Property should not return created disposable.
    protected override Dictionary<TableType, Func<DbConnection, DbCommand>> GetObjectListCommandsFactory =>
        new()
        {
            { TableType.Table, c => c.CreateCommand($"SELECT table_name FROM information_schema.tables WHERE table_schema = 'public';")},
            { TableType.View, c => c.CreateCommand($"SELECT table_name FROM information_schema.views WHERE table_schema = 'public';")},
            {
                TableType.StoredProcedure, c => c.CreateCommand(@"SELECT  p.proname
                                FROM    pg_catalog.pg_namespace n
                                JOIN    pg_catalog.pg_proc p
                                ON      p.pronamespace = n.oid
                                WHERE   n.nspname = 'public' AND prokind = 'p'")
            },
            {
                TableType.Function, c => c.CreateCommand(@"SELECT   distinct  r.routine_name
                                FROM     information_schema.routines r
                                JOIN     information_schema.parameters p
                                USING   (specific_catalog, specific_schema, specific_name)
                                JOIN     pg_namespace pg_n ON r.specific_schema = pg_n.nspname
                                JOIN     pg_proc pg_p ON pg_p.pronamespace = pg_n.oid
                                AND      pg_p.proname = r.routine_name
                                Where 	 r.data_type = 'record' AND pg_n.nspname = 'public'")
            },
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

    public override DataSourceParameters GetQueryParameters()
    {
        switch (Source.TableType)
        {
            case TableType.Table:
                break;

            case TableType.View:
                break;

            case TableType.Sql:
                //TODO: parse sql string
                break;

            case TableType.StoredProcedure:
            case TableType.Function:
                {
                    var query = $"SELECT pg_get_function_identity_arguments('{Source.Table}'::regproc);";

                    using var connection = GetOpenedConnection();
                    using var reader = connection.GetReaderSql(query);
                    if (!reader.Read())
                    {
                        return new DataSourceParameters();
                    }

                    var result = reader.GetString(0);
                    if (string.IsNullOrEmpty(result))
                    {
                        return new DataSourceParameters();
                    }

                    var parameters = result.Split(',').Select(x => x.Trim().Split(' ')).Select(x => new DataSourceParameter
                    {
                        Name = $"@{x[0]}",
                        Type = x[1]
                    }).ToList();

                    return new DataSourceParameters
                    {
                        Parameters = parameters
                    };
                }

            default:
                return new DataSourceParameters();
        }

        return new DataSourceParameters();
    }

    protected override DbCommand CreateGetStoredProcedureRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount)
    {
        ArgumentNullException.ThrowIfNull(connection);

        return connection.CreateCommand($"call {Source.Table}({parameters.ToArgsNamesString()})");
    }

    protected override DbCommand CreateTableCountCommand(DbConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);

        return connection.CreateCommand($"SELECT COUNT(*) AS \"count\" FROM \"{Source.Table}\"");
    }
}
