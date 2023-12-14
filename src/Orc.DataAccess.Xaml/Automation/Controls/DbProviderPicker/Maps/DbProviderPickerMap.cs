#nullable disable
namespace Orc.DataAccess.Automation.Controls;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class DbProviderPickerMap : AutomationBase
{
    public DbProviderPickerMap(AutomationElement element)
        : base(element)
    {
    }

    public Edit DbProviderTextBox => By.Id().One<Edit>();
    public Button ChangeDbProviderButton => By.Id().One<Button>();
}
