namespace Orc.DataAccess.Database
{
    using Catel;

    public static class DbProviderInfoExtensions
    {
        public static DbConnectionString CreateConnectionString(this DbProviderInfo dbProviderInfo, string? connectionString = null)
        {
            Argument.IsNotNull(() => dbProviderInfo);

            return dbProviderInfo.GetProvider()?.CreateConnectionString(connectionString);
        }

        public static DbProvider GetProvider(this DbProviderInfo dbProviderInfo)
        {
            Argument.IsNotNull(() => dbProviderInfo);

            return DbProvider.GetRegisteredProvider(dbProviderInfo.InvariantName);
        }
    }
}
