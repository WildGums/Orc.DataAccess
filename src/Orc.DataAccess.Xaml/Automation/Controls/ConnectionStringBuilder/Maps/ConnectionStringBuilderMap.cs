namespace Orc.DataAccess.Automation.Controls;
#nullable disable
using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class ConnectionStringBuilderMap : AutomationBase
{
    public ConnectionStringBuilderMap(AutomationElement element)
        : base(element)
    {
    }

    public Edit ConnectionStringTextBox => By.Id("PART_ConnectionStringTextBox").One<Edit>();
    public Button EditButton => By.Id("PART_EditButton").One<Button>();
    public Button ClearButton => By.Id("PART_ClearButton").One<Button>();
}
