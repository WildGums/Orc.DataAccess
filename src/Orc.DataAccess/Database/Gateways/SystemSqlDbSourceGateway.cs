namespace Orc.DataAccess.Database;

[ConnectToProvider("System.Data.SqlClient")]
public class SystemSqlDbSourceGateway : MsSqlDbSourceGatewayBase
{
    public SystemSqlDbSourceGateway(DatabaseSource source)
        : base(source)
    {
    }
}
