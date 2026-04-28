namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Catel.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

public abstract class MsSqlDbDataSourceProviderBase : IDbDataSourceProvider
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(MsSqlDbDataSourceProviderBase));

    private const string MicrosoftSqlServerRegPath = @"SOFTWARE\Microsoft\Microsoft SQL Server";

    private readonly IRegistryKeyService _registryKeyService;

    protected internal MsSqlDbDataSourceProviderBase(IRegistryKeyService registryKeyService)
    {
        _registryKeyService = registryKeyService;
    }

    protected abstract string ProviderName { get; }

    public virtual IReadOnlyList<DbDataSource> GetDataSources()
    {
        var localServers = GetLocalSqlServerInstances().ToList();
        var remoteServers = GetRemoteSqlServerInstances().ToList();

        return localServers.Union(remoteServers)
            .Distinct()
            .OrderBy(x => x)
            .Select(x => new DbDataSource(ProviderName, x))
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

    private IEnumerable<string> GetInstalledInstancesInRegistryView(RegistryView registryView)
    {
        using var regView = _registryKeyService.OpenBaseKey(RegistryHive.LocalMachine, registryView);
        using var sqlServerNode = regView.OpenSubKey(MicrosoftSqlServerRegPath);

        return sqlServerNode?.GetValue("InstalledInstances") as IEnumerable<string> ?? Enumerable.Empty<string>();
    }

    private IEnumerable<string> GetRemoteSqlServerInstances()
    {
        DbProviderFactory? sqlFactory;

        try
        {
            sqlFactory = DbProviderFactories.GetFactory(ProviderName);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, null);

            return Enumerable.Empty<string>();
        }

        if (!sqlFactory.CanCreateDataSourceEnumerator)
        {
            return Enumerable.Empty<string>();
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
                servers[i] += $"\\{instance}";
            }
        }

        return servers;
    }
}
