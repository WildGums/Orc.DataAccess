namespace Orc.DataAccess.Tests;

using System;
using Catel;
using Controls;
using Orc.Automation;

public abstract class ConnectionStringBuilderControlTestsBase : StyledControlTestFacts<ConnectionStringBuilder>
{
    [Target] 
    public Automation.Controls.ConnectionStringBuilder Target { get; set; }

    protected IDisposable StartProvider(string provider)
    {
        return new DisposableToken<ConnectionStringBuilderControlTestsBase>(this,
            x => Target.Execute<RegisterProviderMethodRun>(provider),
            x => Target.Execute<UnregisterProviderMethodRun>(provider));
    }
}
