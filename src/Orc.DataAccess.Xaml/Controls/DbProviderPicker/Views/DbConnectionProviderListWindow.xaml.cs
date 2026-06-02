namespace Orc.DataAccess.Controls;

using System.Windows.Automation.Peers;
using Catel.Windows;
using Orc.DataAccess.Automation.Controls;

public partial class DbConnectionProviderListWindow
{
    partial void OnInitializingComponent()
    {
        Mode = DataWindowMode.OkCancel;
    }
    
    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new DbConnectionProviderListWindowPeer(this);
    }
}
