namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data.Common;
using DataAccess;

[ConnectToProvider("FirebirdSql.Data.FirebirdClient")]
public class FirebirdSourceGateway : SqlDbSourceGatewayBase
{
    public FirebirdSourceGateway(DatabaseSource source)
        : base(source)
    {
    }

#pragma warning disable IDISP012 // Property should not return created disposable.
    protected override Dictionary<TableType, Func<DbConnection, DbCommand>> GetObjectListCommandsFactory =>
        new()
        {
            {TableType.Table, c => c.CreateCommand($"SELECT rdb$relation_name FROM rdb$relations WHERE rdb$view_blr is null AND (rdb$system_flag is null OR rdb$system_flag = 0);")},
            {TableType.View, c => c.CreateCommand($"SELECT rdb$relation_name FROM rdb$relations WHERE rdb$view_blr is not null AND (rdb$system_flag is null OR rdb$system_flag = 0);")},
            {TableType.StoredProcedure, c => c.CreateCommand("SELECT rdb$procedure_name FROM rdb$procedures;")},
            {TableType.Function, c => c.CreateCommand($"SELECT rdb$function_name FROM rdb$functions;")},
        };
#pragma warning restore IDISP012 // Property should not return created disposable.

    protected override Dictionary<TableType, Func<DataSourceParameters>> DataSourceParametersFactory => new()
    {
        {TableType.StoredProcedure, () => GetArgs($"SELECT rdb$parameter_name, rdb$parameter_type FROM rdb$procedure_parameters WHERE rdb$procedure_name = '{Source.Table}'")},
        {TableType.Function, () => GetArgs($"SELECT rdb$argument_name, rdb$field_type FROM rdb$procedure_parameters WHERE rdb$procedure_name = '{Source.Table}'")},
    };

    protected override DbCommand CreateGetTableRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled)
    {
        ArgumentNullException.ThrowIfNull(connection);

        string query;
        if (isPagingEnabled)
        {
            query = offset == 0
                ? $"SELECT FIRST {fetchCount} * FROM \"{Source.Table}\""
                : $"SELECT * FROM \"{Source.Table}\" ROWS {offset + 1} TO {offset + fetchCount}";
        }
        else
        {
            query = $"SELECT * FROM \"{Source.Table}\"";
        }

        return connection.CreateCommand(query);
    }

    protected override DbCommand CreateTableCountCommand(DbConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);

        return connection.CreateCommand($"SELECT COUNT(*) AS \"COUNT\" FROM \"{Source.Table}\"");
    }
}
