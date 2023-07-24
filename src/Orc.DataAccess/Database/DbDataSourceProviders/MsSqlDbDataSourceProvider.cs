namespace Orc.DataAccess.Database;

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
