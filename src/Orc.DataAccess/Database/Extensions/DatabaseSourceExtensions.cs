namespace Orc.DataAccess.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using Catel;
    using Catel.Logging;

    public static class DatabaseSourceExtensions
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static IList<DbObject> GetObjectsOfType(this DatabaseSource databaseSource, TableType tableType)
        {
            var dataSourceCopy = new DatabaseSource(databaseSource.ToString())
            {
                TableType = tableType
            };

            var gateway = dataSourceCopy.CreateGateway();

            return gateway?.GetObjects() ?? new List<DbObject>();
        }

        public static DbConnection? CreateConnection(this DatabaseSource databaseSource)
        {
            var provider = databaseSource.GetProvider();
            return provider?.CreateConnection(databaseSource);
        }

        public static DbSourceGatewayBase? CreateGateway(this DatabaseSource databaseSource)
        {
            var dbProvider = databaseSource.GetProvider();
            return dbProvider?.CreateDbSourceGateway(databaseSource);
        }

        public static DbProvider GetProvider(this DatabaseSource databaseSource)
        {
            ArgumentNullException.ThrowIfNull(databaseSource);
            Argument.IsNotNullOrEmpty(databaseSource.ProviderName, "databaseSource.ProviderName");

            return DbProvider.GetRegisteredProvider(databaseSource.ProviderName);
        }
    }
}
