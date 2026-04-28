namespace Orc.DataAccess.Database;

using System;

[ConnectToProvider("System.Data.SqlClient")]
public class SystemSqlDbSourceGateway : MsSqlDbSourceGatewayBase
{
    public SystemSqlDbSourceGateway(DatabaseSource source, IServiceProvider serviceProvider)
        : base(source, serviceProvider)
    {
    }
}
