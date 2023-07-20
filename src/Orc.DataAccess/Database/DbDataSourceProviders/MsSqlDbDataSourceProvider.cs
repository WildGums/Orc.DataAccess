namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Win32;
using Orc.DataAccess.Registry;
using RegistryKey = Registry.RegistryKey;

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
