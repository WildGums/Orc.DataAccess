// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbConnectionStringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.DataAccess.Database
{
    using Catel;

    public static class DbConnectionStringExtensions
    {
        #region Methods
        public static DbConnectionStringProperty TryGetProperty(this DbConnectionString connectionString, string propertyName)
        {
            //Argument.IsNotNull(() => connectionString);

            var properties = connectionString?.Properties;
            if (properties == null)
            {
                return null;
            }

            var upperInvariantPropertyName = propertyName.ToUpperInvariant();
            if (properties.TryGetValue(upperInvariantPropertyName, out var property))
            {
                return property;
            }

            if (properties.TryGetValue(upperInvariantPropertyName.Replace(" ", string.Empty), out property))
            {
                return property;
            }

            return null;
        }

        public static DbDataSourceSchema GetDataSourceSchema(this DbConnectionString connectionString)
        {
            Argument.IsNotNull(() => connectionString);

            var provider = connectionString.DbProvider;
            var schemaProvider = provider.GetProvider()?.GetOrCreateConnectedInstance<IDataSourceSchemaProvider>();
            return schemaProvider?.GetSchema(connectionString);
        }

        public static ConnectionState GetConnectionState(this DbConnectionString connectionString)
        {
            var connectionStringStr = connectionString?.ToString();
            if (string.IsNullOrWhiteSpace(connectionStringStr))
            {
                return ConnectionState.Invalid;
            }

            var connection = connectionString.DbProvider.GetProvider()?.CreateConnection();
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
        #endregion
    }
}
