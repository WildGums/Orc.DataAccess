#nullable disable
namespace Orc.DataAccess.Automation.Controls;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class DbConnectionProviderListWindowMap : AutomationBase
{
    public DbConnectionProviderListWindowMap(AutomationElement element)
        : base(element)
    {
    }

    public List ProvidersList => By.Id().One<List>();

    public Button OkButton => By.Name("OK").One<Button>();
    public Button CancelButton => By.Name("Cancel").One<Button>();
}
