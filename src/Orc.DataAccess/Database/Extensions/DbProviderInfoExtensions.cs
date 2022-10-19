namespace Orc.DataAccess.Database
{
    using System;

    public static class DbProviderInfoExtensions
    {
        public static DbConnectionString? CreateConnectionString(this DbProviderInfo dbProviderInfo, string? connectionString = null)
        {
            ArgumentNullException.ThrowIfNull(dbProviderInfo);

            return dbProviderInfo.GetProvider()?.CreateConnectionString(connectionString ?? string.Empty);
        }

        public static DbProvider GetProvider(this DbProviderInfo dbProviderInfo)
        {
            ArgumentNullException.ThrowIfNull(dbProviderInfo);

            return DbProvider.GetRegisteredProvider(dbProviderInfo.InvariantName);
        }
    }
}
