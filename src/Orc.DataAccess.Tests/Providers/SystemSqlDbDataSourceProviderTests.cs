namespace Orc.DataAccess.Tests.Providers;

using System;
using System.Collections.Generic;
using System.Reflection;
using Database;
using Microsoft.Win32;
using Moq;
using NUnit.Framework;

[TestFixture]
public class SystemSqlDbDataSourceProviderTests
{
    [Orc.Automation.Tests.TestCase<SystemSqlDbDataSourceProvider>("System.Data.SqlClient")]
    [Orc.Automation.Tests.TestCase<MsSqlDbDataSourceProvider>("Microsoft.Data.SqlClient")]
    public void GetDataSources_Returns_Correct_Local_Server_List<T>(string providerName)
        where T : MsSqlDbDataSourceProviderBase
    {
        const string serverName = "Test server name";

        var registerKey = new Mock<IRegistryKey>();
        registerKey.Setup(x => x.OpenSubKey(It.IsAny<string>()))
            .Returns<string>(n =>
            {
                if (!Equals(n, @"SOFTWARE\Microsoft\Microsoft SQL Server"))
                {
                    return null;
                }

                var subKey = new Mock<IRegistryKey>();
                subKey.Setup(x => x.GetValue(It.IsAny<string>()))
                    .Returns<string>(x =>
                    {
                        if (Equals(x, "InstalledInstances"))
                        {
                            return new List<string> { serverName };
                        }

                        return null;
                    });

                return subKey.Object;
            });

        var registerKeyService = new Mock<IRegistryKeyService>();
        registerKeyService.Setup(x => x.OpenBaseKey(It.IsAny<RegistryHive>(), It.IsAny<RegistryView>()))
            .Returns<RegistryHive, RegistryView>((h, v) =>
            {
                if (h != RegistryHive.LocalMachine)
                {
                    return null;
                }

                if (v != RegistryView.Registry32 && v != RegistryView.Registry64)
                {
                    return null;
                }

                return registerKey.Object;
            });

        var dbProvider = (T)Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Instance,
            null, new object?[] { registerKeyService.Object }, null);

        var expectedServerName = $"{Environment.MachineName}\\{serverName}";

        var sources = dbProvider.GetDataSources();

        Assert.That(sources, Is.EquivalentTo(new[] { new DbDataSource(providerName, expectedServerName) }));
    }
}
