namespace Orc.DataAccess.Tests;

using Controls;
using Orc.Automation;
using Orc.Automation.Controls;

public abstract class ConnectionStringBuilderControlTestsBase : StyledControlTestFacts<ConnectionStringBuilder>
{
    [Target] public Automation.Controls.ConnectionStringBuilder Target { get; set; }

    protected override bool TryLoadControl(TestHostAutomationControl testHost, out string testedControlAutomationId)
    {
        var result = base.TryLoadControl(testHost, out testedControlAutomationId);

        testHost.Execute<InitProviderMethodRun>("Provider_1");
        testHost.Execute<InitProviderMethodRun>("Provider_2");
        testHost.Execute<InitProviderMethodRun>("Provider_3");

        return result;
    }
}
