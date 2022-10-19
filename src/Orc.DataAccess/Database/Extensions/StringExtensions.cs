﻿namespace Orc.DataAccess.Database
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

        private static string AlterConnectionStringPropertyValue(this string connectionString, string providerName, Func<string, string?> alteractionFunction)
        {
            var provider = DbProvider.GetRegisteredProviders()[providerName];
            var dbConnectionString = provider.CreateConnectionString(connectionString);
            if (dbConnectionString is null)
            {
                return connectionString;
            }

            var connectionStringBuilder = dbConnectionString.ConnectionStringBuilder;

            if (dbConnectionString.Properties is not null)
            {
                var sensitiveProperties = dbConnectionString.Properties.Where(x => x.Value.IsSensitive);
                foreach (var sensitiveProperty in sensitiveProperties)
                {
                    var value = connectionStringBuilder[sensitiveProperty.Key].ToString();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        connectionStringBuilder[sensitiveProperty.Key] = alteractionFunction(value);
                    }
                }
            }

            return connectionStringBuilder.ConnectionString;
        }

        public static string? GetConnectionStringProperty(this string connectionString, string providerName, string propertyName)
        {
            var provider = DbProvider.GetRegisteredProviders()[providerName];
            var dbConnectionString = provider.CreateConnectionString(connectionString);
            if (dbConnectionString is null)
            {
                return connectionString;
            }

            if (dbConnectionString.Properties?.TryGetValue(propertyName, out var dataSourceProperty) ?? false)
            {
                return dataSourceProperty.Value?.ToString() ?? string.Empty;
            }

            return null;
        }
    }
}
