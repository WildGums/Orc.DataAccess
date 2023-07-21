namespace Orc.DataAccess.Automation.Controls;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(DataAccess.Controls.ConnectionStringBuilder))]
public class ConnectionStringBuilder : FrameworkElement<ConnectionStringBuilderModel, ConnectionStringBuilderMap>
{
    public ConnectionStringBuilder(AutomationElement element)
        : base(element)
    {
    }

    public ConnectionStringEditWindow OpenEditWindow()
    {
        Map.EditButton.Click();

        var editWindow = Window.WaitForWindow<ConnectionStringEditWindow>(numberOfWaits:50);

        Wait.UntilInputProcessed(500);

        return editWindow;
    }
}
