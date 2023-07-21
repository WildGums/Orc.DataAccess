#nullable disable
namespace Orc.DataAccess.Automation.Controls;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(DataAccess.Controls.ConnectionStringEditWindow), ControlTypeName = nameof(ControlType.Window))]
public class ConnectionStringEditWindow : Window<ConnectionStringEditWindowModel, ConnectionStringEditWindowMap>
{
    public ConnectionStringEditWindow(AutomationElement element)
        : base(element)
    {
    }

    public string SelectedProvider
    {
        get => Map.ProviderPicker.SelectedProvider;
        set => Map.ProviderPicker.SelectProvider(value);
    }

    public ConnectionStringAdvancedOptionsWindow OpenAdvancedProperties()
    {
        Map.ShowAdvancedOptionsButton.Click();

        var advancedOptionsWindow = Window.WaitForWindow<ConnectionStringAdvancedOptionsWindow>(numberOfWaits: 50);

        Wait.UntilInputProcessed(500);

        return advancedOptionsWindow;
    }
}
