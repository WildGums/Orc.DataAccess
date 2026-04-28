namespace Orc.DataAccess.Controls;

using System.Windows.Automation.Peers;
using Orc.DataAccess.Automation.Controls;

public sealed partial class DbConnectionProviderListWindow
{
    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new DbConnectionProviderListWindowPeer(this);
    }
}
