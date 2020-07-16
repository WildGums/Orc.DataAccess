// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.DataAccess.Database
{
    using System;
    using System.Linq;
    using Catel;

    public static class StringExtensions
    {
        public static string EncryptConnectionString(this string connectionString, string providerName)
        {
            return AlterConnectionStringPropertyValue(connectionString, providerName, x => x.Encrypt());
        }

        public static string DecryptConnectionString(this string connectionString, string providerName)
        {
            return AlterConnectionStringPropertyValue(connectionString, providerName, x => x.Decrypt());
        }

        private static string AlterConnectionStringPropertyValue(this string connectionString, string providerName, Func<string, string> alteractionFunction)
        {
            Argument.IsNotNullOrEmpty(() => connectionString);
            Argument.IsNotNullOrEmpty(() => providerName);
            Argument.IsNotNull(() => alteractionFunction);

            var provider = DbProvider.GetRegisteredProviders()[providerName];
            var dbConnectionString = provider.CreateConnectionString(connectionString);
            if (dbConnectionString == null)
            {
                return connectionString;
            }

            var connectionStringBuilder = dbConnectionString.ConnectionStringBuilder;
            var sensitiveProperties = dbConnectionString.Properties.Where(x => x.Value.IsSensitive);
            foreach (var sensitiveProperty in sensitiveProperties)
            {
                var value = connectionStringBuilder[sensitiveProperty.Key].ToString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    connectionStringBuilder[sensitiveProperty.Key] = alteractionFunction(value);
                }
            }

            return connectionStringBuilder.ConnectionString;
        }

        public static string GetConnectionStringProperty(this string connectionString, string providerName, string propertyName)
        {
            Argument.IsNotNullOrEmpty(() => connectionString);
            Argument.IsNotNullOrEmpty(() => providerName);

            var provider = DbProvider.GetRegisteredProviders()[providerName];
            var dbConnectionString = provider.CreateConnectionString(connectionString);
            if (dbConnectionString == null)
            {
                return connectionString;
            }

            if (dbConnectionString.Properties.TryGetValue(propertyName, out var dataSourceProperty))
            {
                return dataSourceProperty.Value?.ToString() ?? string.Empty;
            }

            return null;
        }
    }
}
