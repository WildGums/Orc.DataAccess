namespace Orc.DataAccess.Tests;

using System.Data.Common;
using System.Windows;
using Catel.Reflection;
using Database;
using Orc.Automation;

public class InitProviderMethodRun : NamedAutomationMethodRun
{
    public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue? result)
    {
        result = AutomationValue.FromValue(true);

        var providerInvariantName = (string) method.Parameters[0].ExtractValue();

        var customProvider = new DbProvider(new DbProviderInfo(providerInvariantName, providerInvariantName,
            "For test sake", typeof(TestDbProviderFactory).GetSafeFullName()));

        var dataSourceProvider = new TestDbDataSourceProvider(customProvider);
        dataSourceProvider.AddDataSource("Data source 1");
        dataSourceProvider.AddDataSource("Data source 2");
        customProvider.ConnectInstance<IDbDataSourceProvider>(dataSourceProvider);

        var dataSourceSchemaProvider = new TestDataSourceSchemaProvider();
        dataSourceSchemaProvider.AddDataSource("Db_1");
        dataSourceSchemaProvider.AddDataSource("Db_2");
        dataSourceSchemaProvider.AddDataSource("Db_3");
        dataSourceSchemaProvider.AddDataSource("Db_4");
        customProvider.ConnectInstance<IDataSourceSchemaProvider>(dataSourceSchemaProvider);

        DbProvider.RegisterCustomProvider(customProvider);

        var testFactory = new TestDbProviderFactory();
        testFactory.AddPropertyDescription<string>("User ID");
        testFactory.AddPropertyDescription<bool>("Integrated Security");
        testFactory.AddPropertyDescription<string>("Initial Catalog");
        testFactory.AddPropertyDescription<string>("Server");
        testFactory.AddPropertyDescription<string>("Password");
        DbProviderFactories.RegisterFactory(customProvider.ProviderInvariantName, testFactory);

        return true;
    }
}
