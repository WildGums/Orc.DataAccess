namespace Orc.DataAccess.Database;

using System;

public static class DbProviderInfoExtensions
{
    public static DbConnectionString? CreateConnectionString(this DbProviderInfo dbProviderInfo, IServiceProvider serviceProvider, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(dbProviderInfo);

        return dbProviderInfo.GetProvider(serviceProvider).CreateConnectionString(connectionString);
    }

    public static DbProvider GetProvider(this DbProviderInfo dbProviderInfo, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(dbProviderInfo);

        return DbProvider.GetRegisteredProvider(dbProviderInfo.InvariantName, serviceProvider);
    }
}
