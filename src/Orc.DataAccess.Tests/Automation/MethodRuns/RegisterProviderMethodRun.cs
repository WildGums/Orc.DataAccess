namespace Orc.DataAccess.Tests;

using System.Data.Common;
using System.Windows;
using Catel.Reflection;
using Database;
using Orc.Automation;

public class RegisterProviderMethodRun : NamedAutomationMethodRun
{
    public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue? result)
    {
        result = AutomationValue.FromValue(true);

        var providerInvariantName = (string) method.Parameters[0].ExtractValue();

        var providerInfo = new DbProviderInfo(providerInvariantName, providerInvariantName,
            "For test sake", typeof(TestDbProviderFactory).GetSafeFullName());

        var customProvider = new DbProvider(providerInfo);

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

        testFactory.AddPropertyDescription<string>("AdvProperty_1");
        testFactory.AddPropertyDescription<string>("AdvProperty_2");
        testFactory.AddPropertyDescription<string>("AdvProperty_3");
        DbProviderFactories.RegisterFactory(customProvider.ProviderInvariantName, testFactory);

        return true;
    }
}
