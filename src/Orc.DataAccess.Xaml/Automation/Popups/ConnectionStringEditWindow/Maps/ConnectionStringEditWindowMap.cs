#nullable disable
namespace Orc.DataAccess.Automation.Controls;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class ConnectionStringEditWindowMap : AutomationBase
{
    public ConnectionStringEditWindowMap(AutomationElement element)
        : base(element)
    {
    }

    public DbProviderPicker ProviderPicker => By.One<DbProviderPicker>();
    public Button ShowAdvancedOptionsButton => By.Id().One<Button>();
}
