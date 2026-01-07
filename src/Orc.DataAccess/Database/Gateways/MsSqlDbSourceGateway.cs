namespace Orc.DataAccess.Database;

using System;

[ConnectToProvider("Microsoft.Data.SqlClient")]
public class MsSqlDbSourceGateway : MsSqlDbSourceGatewayBase
{
    public MsSqlDbSourceGateway(DatabaseSource source, IServiceProvider serviceProvider)
        : base(source, serviceProvider)
    {
    }
}
