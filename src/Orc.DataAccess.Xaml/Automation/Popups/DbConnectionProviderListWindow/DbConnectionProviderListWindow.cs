#nullable disable
namespace Orc.DataAccess.Automation.Controls;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(DataAccess.Controls.DbConnectionProviderListWindow), ControlTypeName = nameof(ControlType.Window))]
public class DbConnectionProviderListWindow : Window<DbConnectionProviderListWindowModel, DbConnectionProviderListWindowMap>
{
    public DbConnectionProviderListWindow(AutomationElement element)
        : base(element)
    {
    }

    public IReadOnlyCollection<string> Providers
    {
        get
        {
            return Map.ProvidersList.Items.Select(x => x.DisplayText).ToArray();
        }
    }

    public string SelectedProvider
    {
        get
        {
            return Map.ProvidersList.SelectedItem.TryGetDisplayText();
        }
        set
        {
            var item = Map.ProvidersList.Items.FirstOrDefault(x => Equals(x.DisplayText, value));
            if (item is not null)
            {
                item.IsSelected = true;
            }
        }
    }

    public void AcceptAndClose()
    {
        Map.OkButton.Click();
    }

    public void CancelAndClose()
    {
        Map.CancelButton.Click();
    }
}
