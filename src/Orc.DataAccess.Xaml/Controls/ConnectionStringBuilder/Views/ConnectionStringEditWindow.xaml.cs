namespace Orc.DataAccess.Controls;

using System.Windows.Automation.Peers;
using Orc.DataAccess.Automation.Controls;

public sealed partial class ConnectionStringEditWindow
{
    public ConnectionStringEditWindow() => InitializeComponent();

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new ConnectionStringEditWindowPeer(this);
    }
}
