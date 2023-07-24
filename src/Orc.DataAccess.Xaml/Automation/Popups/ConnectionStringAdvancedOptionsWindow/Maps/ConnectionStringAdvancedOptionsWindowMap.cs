#nullable disable
namespace Orc.DataAccess.Automation.Controls;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Controls.Automation;

public class ConnectionStringAdvancedOptionsWindowMap : AutomationBase
{
    public ConnectionStringAdvancedOptionsWindowMap(AutomationElement element)
        : base(element)
    {
    }

    public FilterBox AdvancedPropertiesFilterBox => By.Id().One<FilterBox>();
    public DataGrid PropertiesDataGrid => By.Id().One<DataGrid>();
}
