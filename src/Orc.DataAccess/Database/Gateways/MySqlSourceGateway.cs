namespace Orc.DataAccess.Database;

using System;

[ConnectToProvider("MySql.Data.MySqlClient")]
public class MySqlSourceGateway : MsSqlDbSourceGatewayBase
{
    public MySqlSourceGateway(DatabaseSource source, IServiceProvider serviceProvider) 
        : base(source, serviceProvider)
    {
    }
}
