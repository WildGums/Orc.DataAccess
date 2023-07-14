namespace Orc.DataAccess.Database;

using Orc.DataAccess.Registry;

[ConnectToProvider("Microsoft.Data.SqlClient")]
public class MsSqlDbDataSourceProvider : MsSqlDbDataSourceProviderBase
{
    public MsSqlDbDataSourceProvider()
    {
    }

    internal MsSqlDbDataSourceProvider(IRegistryKeyService registryKeyService)
        : base(registryKeyService)
    {
    }

    protected override string ProviderName => "Microsoft.Data.SqlClient";
}
