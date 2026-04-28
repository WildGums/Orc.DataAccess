namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data.Common;
using Catel;
using Catel.Logging;
using Microsoft.Extensions.Logging;

public static class DatabaseSourceExtensions
{
    public static IList<DbObject> GetObjectsOfType(this DatabaseSource databaseSource, IServiceProvider serviceProvider, TableType tableType)
    {
        ArgumentNullException.ThrowIfNull(databaseSource);

        var dataSourceCopy = new DatabaseSource(databaseSource.ToString())
        {
            TableType = tableType
        };

        var gateway = dataSourceCopy.CreateGateway(serviceProvider);

        return gateway?.GetObjects() ?? new List<DbObject>();
    }

    public static DbConnection? CreateConnection(this DatabaseSource databaseSource, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(databaseSource);

        var provider = databaseSource.GetProvider(serviceProvider);
        return provider.CreateConnection(databaseSource);
    }

    public static DbSourceGatewayBase? CreateGateway(this DatabaseSource databaseSource, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(databaseSource);

        var dbProvider = databaseSource.GetProvider(serviceProvider);
        return dbProvider.CreateDbSourceGateway(serviceProvider, databaseSource);
    }

    public static DbProvider GetProvider(this DatabaseSource databaseSource, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(databaseSource);
        Argument.IsNotNullOrEmpty(databaseSource.ProviderName, "databaseSource.ProviderName");

        return DbProvider.GetRegisteredProvider(databaseSource.ProviderName, serviceProvider);
    }
}
