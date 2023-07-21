namespace Orc.DataAccess.Controls;

using System.Windows.Automation.Peers;
using Catel.Windows;
using Orc.DataAccess.Automation.Controls;

public sealed partial class ConnectionStringAdvancedOptionsWindow
{
    public ConnectionStringAdvancedOptionsWindow()
        : base(DataWindowMode.Close) => InitializeComponent();

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new ConnectionStringAdvancedOptionsWindowPeer(this);
    }
}
