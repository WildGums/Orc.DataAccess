﻿namespace Orc.DataAccess.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using DataAccess;

    public abstract class SqlDbSourceGatewayBase : DbSourceGatewayBase
    {
        #region Constructors
        protected SqlDbSourceGatewayBase(DatabaseSource source)
            : base(source)
        {
        }
        #endregion

        #region Properties
        protected virtual Dictionary<TableType, Func<DbConnection, DbCommand>> GetObjectListCommandsFactory => new Dictionary<TableType, Func<DbConnection, DbCommand>>();
        protected virtual Dictionary<TableType, Func<DataSourceParameters>> DataSourceParametersFactory => new Dictionary<TableType, Func<DataSourceParameters>>();
        #endregion

        #region Methods
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

        public override DbDataReader GetRecords(DataSourceParameters queryParameters = null, int offset = 0, int fetchCount = -1)
        {
#pragma warning disable IDISP001 // Dispose created.
            var connection = GetOpenedConnection();
#pragma warning restore IDISP001 // Dispose created.
            var source = Source;
            var isPagingQuery = offset >= 0 && fetchCount > 0;

            var sql = source.Table;
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
                    throw new NotSupportedException($"'{source.TableType}' not supported");
            }

            command.AddParameters(queryParameters);

            var reader = command.ExecuteReader();
            if (isPagingQuery && (source.TableType == TableType.Sql || source.TableType == TableType.StoredProcedure || source.TableType == TableType.Function))
            {
                return new SkipTakeDbReader(reader, offset, fetchCount);
            }

            return reader;
        }

        protected abstract DbCommand CreateGetTableRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled);

        private DbCommand CreateGetTableRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount)
        {
            var isPagingQuery = offset >= 0 && fetchCount > 0;
            return CreateGetTableRecordsCommand(connection, parameters, offset, fetchCount, isPagingQuery);
        }

        protected virtual DbCommand CreateGetViewRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled)
        {
            return CreateGetTableRecordsCommand(connection, parameters, offset, fetchCount, isPagingEnabled);
        }

        private DbCommand CreateGetViewRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount)
        {
            return CreateGetTableRecordsCommand(connection, parameters, offset, fetchCount);
        }

        protected virtual DbCommand CreateGetStoredProcedureRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount)
        {
            return connection.CreateCommand(Source.Table, CommandType.StoredProcedure);
        }

        protected virtual DbCommand CreateGetFunctionRecordsCommand(DbConnection connection, DataSourceParameters parameters, int offset, int fetchCount)
        {
            return connection.CreateCommand($"SELECT * FROM {Source.Table}({parameters?.ToArgsNamesString() ?? string.Empty})");
        }

        protected abstract DbCommand CreateTableCountCommand(DbConnection connection);

        protected virtual DbCommand CreateViewCountCommand(DbConnection connection)
        {
            return CreateTableCountCommand(connection);
        }

        public override long GetCount(DataSourceParameters queryParameters = null)
        {
            var source = Source;

            using (var connection = GetOpenedConnection())
            {
                switch (source.TableType)
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
                            using (var command = CreateGetFunctionRecordsCommand(connection, queryParameters, -1, -1))
                            {
                                command.AddParameters(queryParameters);
                                var count = command.GetRecordsCount();

                                return count;
                            }
                        }

                    case TableType.StoredProcedure:
                        {
                            using (var command = CreateGetStoredProcedureRecordsCommand(connection, queryParameters, -1, -1))
                            {
                                command.AddParameters(queryParameters);
                                var count = command.GetRecordsCount();

                                return count;
                            }
                        }

                    case TableType.Sql:
                        {
                            using (var command = connection.CreateCommand(source.Table))
                            {
                                command.AddParameters(queryParameters);
                                var count = command.GetRecordsCount();

                                return count;
                            }
                        }

                    default:
                        {
                            return 0;
                        }
                }
            }
        }

        protected virtual IList<DbObject> ReadAllDbObjects(DbCommand command)
        {
            var dbObjects = new List<DbObject>();
            var tableType = Source.TableType;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var dbObject = new DbObject(tableType) { Name = reader.GetString(0) };
                    dbObjects.Add(dbObject);
                }
            }

            return dbObjects;
        }

        protected DataSourceParameters GetArgs(string query)
        {
            using (var connection = GetOpenedConnection())
            {
                var queryParameters = new DataSourceParameters();
                using var reader = connection.GetReader(query);

                while (reader.Read())
                {
                    var args = new DataSourceParameter
                    {
                        Name = reader.GetValue(0)?.ToString(),
                        Type = reader.GetValue(1)?.ToString()
                    };

                    queryParameters.Parameters.Add(args);
                }

                return queryParameters;
            }
        }
        #endregion
    }
}
