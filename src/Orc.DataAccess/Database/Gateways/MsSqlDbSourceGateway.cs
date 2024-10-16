namespace Orc.DataAccess.Database;

[ConnectToProvider("Microsoft.Data.SqlClient")]
public class MsSqlDbSourceGateway : MsSqlDbSourceGatewayBase
{
    public MsSqlDbSourceGateway(DatabaseSource source)
        : base(source)
    {
    }
}
