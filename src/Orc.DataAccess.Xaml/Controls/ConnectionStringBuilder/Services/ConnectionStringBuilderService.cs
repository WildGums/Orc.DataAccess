// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringBuilderService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.DataAccess.Controls
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using Catel;
    using Database;

    public class ConnectionStringBuilderService : IConnectionStringBuilderService
    {
        #region Fields
        private readonly Dictionary<string, IDataSourceProvider> _providers = new Dictionary<string, IDataSourceProvider>();
        #endregion

        #region Constructors
        public ConnectionStringBuilderService(IConnectionStringBuilderServiceInitializer connectionStringBuilderServiceInitializer)
        {
            Argument.IsNotNull(() => connectionStringBuilderServiceInitializer);

            connectionStringBuilderServiceInitializer.Initialize(this);
        }
        #endregion

        #region IConnectionStringBuilderService Members
        public ConnectionState GetConnectionState(DbConnectionString connectionString)
        {
            var connectionStringStr = connectionString?.ToString();

            if (string.IsNullOrWhiteSpace(connectionStringStr))
            {
                return ConnectionState.Invalid;
            }

            var factory = DbProviderFactories.GetFactory(connectionString.DbProvider.InvariantName);
            var connection = factory.CreateConnection();
            if (connection == null)
            {
                return ConnectionState.Invalid;
            }

            // Try to open
            try
            {
                connection.ConnectionString = connectionStringStr;
                connection.Open();
            }
            catch
            {
                return ConnectionState.Invalid;
            }
            finally
            {
                connection.Dispose();
            }

            return ConnectionState.Valid;
        }

        public IList<string> GetDatabases(DbConnectionString connectionString)
        {
            var dbProviderInvariantName = connectionString.DbProvider?.InvariantName;
            if (string.IsNullOrWhiteSpace(dbProviderInvariantName))
            {
                return new List<string>();
            }

            _providers.TryGetValue(dbProviderInvariantName, out var provider);
            if (provider == null)
            {
                return new List<string>();
            }

            var dbProvider = DbProvider.GetRegisteredProvider(dbProviderInvariantName);

            var databases = new List<string>();
            using (var sqlConnection = dbProvider.CreateConnection())
            {
                if (sqlConnection == null)
                {
                    return new List<string>();
                }

                sqlConnection.ConnectionString = connectionString.ToString();
                sqlConnection.Open();

                using (var command = sqlConnection.CreateCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandText = provider.DataBasesQuery;
                    command.CommandType = CommandType.Text;
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            databases.Add(dataReader[0].ToString());
                        }
                    }
                }
            }

            return databases;
        }

        public void AddDataSourceProvider(string invariantName, IDataSourceProvider provider)
        {
            if (_providers.ContainsKey(invariantName))
            {
                return;
            }

            _providers.Add(invariantName, provider);
        }
        #endregion
    }
}
