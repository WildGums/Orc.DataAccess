namespace Orc.DataAccess.Tests;

using System.Data.Common;
using System.Windows;
using Database;
using Orc.Automation;

public class UnregisterProviderMethodRun : NamedAutomationMethodRun
{
    public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue? result)
    {
        result = AutomationValue.FromValue(true);

        var providerInvariantName = (string)method.Parameters[0].ExtractValue();

        DbProvider.UnregisterProvider(new DbProviderInfo(string.Empty, providerInvariantName, string.Empty, string.Empty));
        DbProviderFactories.UnregisterFactory(providerInvariantName);

        return true;
    }
}
