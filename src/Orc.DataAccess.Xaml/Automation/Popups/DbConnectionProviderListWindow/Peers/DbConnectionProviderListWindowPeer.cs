namespace Orc.DataAccess.Automation.Controls;

using System.Windows.Automation.Peers;
using Orc.Automation;

public class DbConnectionProviderListWindowPeer : AutomationWindowPeerBase<DataAccess.Controls.DbConnectionProviderListWindow>
{
    public DbConnectionProviderListWindowPeer(DataAccess.Controls.DbConnectionProviderListWindow owner)
        : base(owner)
    {
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Window;
    }
}
