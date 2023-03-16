namespace Orc.DataAccess.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using Catel.Logging;
using Microsoft.Win32;

public class MsSqlDataSourceProvider : IDataSourceProvider
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    public string DataBasesQuery => "SELECT name from sys.databases";

    public IList<string> GetDataSources()
    {
        var localServers = GetLocalSqlServerInstances();
        var remoteServers = GetRemoteSqlServerInstances();

        return localServers.Union(remoteServers)
            .Distinct()
            .OrderBy(x => x)
            .ToList();
    }

    private static IEnumerable<string> GetLocalSqlServerInstances()
    {
        var machineName = Environment.MachineName;

        var localSqlInstances32 = GetInstalledInstancesInRegistryView(RegistryView.Registry32);
        var localSqlInstances64 = GetInstalledInstancesInRegistryView(RegistryView.Registry64);

        return localSqlInstances32.Union(localSqlInstances64)
            .Select(x => $"{machineName}\\{x}");
    }

    private static IEnumerable<string> GetInstalledInstancesInRegistryView(RegistryView registryView)
    {
        using var regView = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
        using var sqlServerNode = regView.OpenSubKey(DataSourcePath.MicrosoftSqlServerRegPath, false);
        return sqlServerNode?.GetValue("InstalledInstances") as IList<string> ?? new List<string>();
    }

    private static IEnumerable<string> GetRemoteSqlServerInstances()
    {
        throw Log.ErrorAndCreateException<NotSupportedException>($"Not supported on .NET Core, SqlDataSourceEnumerator is not (yet) available. See https://github.com/dotnet/corefx/issues/32874");
    }
}
