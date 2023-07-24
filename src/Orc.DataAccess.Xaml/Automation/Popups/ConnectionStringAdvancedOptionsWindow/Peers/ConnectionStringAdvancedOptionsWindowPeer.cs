namespace Orc.DataAccess.Automation.Controls;

using System.Windows.Automation.Peers;
using Orc.Automation;

public class ConnectionStringAdvancedOptionsWindowPeer : AutomationWindowPeerBase<DataAccess.Controls.ConnectionStringAdvancedOptionsWindow>
{
    public ConnectionStringAdvancedOptionsWindowPeer(DataAccess.Controls.ConnectionStringAdvancedOptionsWindow owner)
        : base(owner)
    {
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Window;
    }
}
