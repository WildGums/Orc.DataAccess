namespace Orc.DataAccess.Automation.Controls;

using System.Windows.Automation.Peers;
using Orc.Automation;

public class ConnectionStringEditWindowPeer : AutomationWindowPeerBase<DataAccess.Controls.ConnectionStringEditWindow>
{
    public ConnectionStringEditWindowPeer(DataAccess.Controls.ConnectionStringEditWindow owner)
        : base(owner)
    {
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Window;
    }
}
