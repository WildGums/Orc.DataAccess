﻿namespace Orc.DataAccess.Database;

[ConnectToProvider("System.Data.SqlClient")]
public class SystemSqlDbDataSourceProvider : MsSqlDbDataSourceProviderBase
{
    public SystemSqlDbDataSourceProvider()
    {
    }

    internal SystemSqlDbDataSourceProvider(IRegistryKeyService registryKeyService)
        : base(registryKeyService)
    {
    }

    protected override string ProviderName => "System.Data.SqlClient";
}
