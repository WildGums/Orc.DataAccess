namespace Orc.DataAccess.Database;

[ConnectToProvider("MySql.Data.MySqlClient")]
public class MySqlSourceGateway : MsSqlDbSourceGatewayBase
{
    public MySqlSourceGateway(DatabaseSource source) 
        : base(source)
    {
    }
}
