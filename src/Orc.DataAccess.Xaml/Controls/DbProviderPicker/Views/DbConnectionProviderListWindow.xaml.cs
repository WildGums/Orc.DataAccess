namespace Orc.DataAccess.Controls;

using System.Windows.Automation.Peers;
using Orc.DataAccess.Automation.Controls;

public sealed partial class DbConnectionProviderListWindow
{
    public DbConnectionProviderListWindow() => InitializeComponent();

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new DbConnectionProviderListWindowPeer(this);
    }
}
