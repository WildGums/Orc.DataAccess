namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Win32;

[ConnectToProvider("System.Data.SqlClient")]
public class MsSqlDbDataSourceProvider : IDbDataSourceProvider
{
    private const string MicrosoftSqlServerRegPath = @"SOFTWARE\Microsoft\Microsoft SQL Server";

    public IList<DbDataSource> GetDataSources()
    {
        var localServers = GetLocalSqlServerInstances().ToList();
        var remoteServers = GetRemoteSqlServerInstances().ToList();

        return localServers.Union(remoteServers)
            .Distinct()
            .OrderBy(x => x)
            .Select(x => new DbDataSource("System.Data.SqlClient", x))
            .ToList();
    }

    private IEnumerable<string> GetLocalSqlServerInstances()
    {
        var machineName = Environment.MachineName;

        var localSqlInstances32 = GetInstalledInstancesInRegistryView(RegistryView.Registry32);
        var localSqlInstances64 = GetInstalledInstancesInRegistryView(RegistryView.Registry64);

        return localSqlInstances32.Union(localSqlInstances64)
            .Select(x => $"{machineName}" + (string.Equals(x, "MSSQLSERVER") ? string.Empty : $"\\{x}"));
    }

    private static IEnumerable<string> GetInstalledInstancesInRegistryView(RegistryView registryView)
    {
        using var regView = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
        using var sqlServerNode = regView.OpenSubKey(MicrosoftSqlServerRegPath, false);

        return sqlServerNode?.GetValue("InstalledInstances") as IList<string> ?? Array.Empty<string>();
    }

    private static IEnumerable<string> GetRemoteSqlServerInstances()
    {
        var sqlFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");

        if (!sqlFactory.CanCreateDataSourceEnumerator)
        {
            return new List<string>();
        }

        var dataSourceEnumerator = sqlFactory.CreateDataSourceEnumerator();

        using var dataTable = dataSourceEnumerator?.GetDataSources() ?? new DataTable();
        var serversCount = dataTable.Rows.Count;
        var servers = new string[serversCount];

        for (var i = 0; i < serversCount; i++)
        {
            var name = dataTable.Rows[i]["ServerName"].ToString();
            var instance = dataTable.Rows[i]["InstanceName"].ToString();

            if (string.IsNullOrEmpty(name))
            {
                continue;
            }

            servers[i] = name;
            if (!string.IsNullOrEmpty(instance))
            {
                servers[i] += "\\" + instance;
            }
        }

        return servers;
    }
}
