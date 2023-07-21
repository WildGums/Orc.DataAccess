#nullable disable
namespace Orc.DataAccess.Automation.Controls;

using System.Collections.Generic;
using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(DataAccess.Controls.DbProviderPicker))]
public class DbProviderPicker : FrameworkElement<DbProviderPickerModel, DbProviderPickerMap>
{
    public DbProviderPicker(AutomationElement element)
        : base(element)
    {
    }

    public string SelectedProvider => Map.DbProviderTextBox.Text;

    public IReadOnlyCollection<string> GetAvailableProviders()
    {
        var dbConnectionProviderListWindow = ShowProviderListWindow();

        var providers = dbConnectionProviderListWindow.Providers;

        dbConnectionProviderListWindow.CancelAndClose();

        return providers;
    }

    public void SelectProvider(string invariantName)
    {
        var dbConnectionProviderListWindow = ShowProviderListWindow();

        dbConnectionProviderListWindow.SelectedProvider = invariantName;

        dbConnectionProviderListWindow.AcceptAndClose();
    }
    
    public DbConnectionProviderListWindow ShowProviderListWindow()
    {
        Map.ChangeDbProviderButton.Click();

        return Window.WaitForWindow<DbConnectionProviderListWindow>();
    }
}
