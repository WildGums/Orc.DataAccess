#nullable disable
namespace Orc.DataAccess.Automation.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(DataAccess.Controls.ConnectionStringAdvancedOptionsWindow), ControlTypeName = nameof(ControlType.Window))]
public class ConnectionStringAdvancedOptionsWindow : Window<ConnectionStringAdvancedOptionsWindowModel, ConnectionStringAdvancedOptionsWindowMap>
{
    public ConnectionStringAdvancedOptionsWindow(AutomationElement element)
        : base(element)
    {
    }

    public IReadOnlyCollection<string> Properties => Map.PropertiesDataGrid.Rows.Select(x => x.Cells[0].Text).ToArray();

    public void SetFilter(string filter)
    {
        Map.AdvancedPropertiesFilterBox.Text = filter;

        Wait.UntilInputProcessed(500);
    }

    public string GetValue(string propertyName)
    {
        var propertyRow = GetPropertyRow(propertyName);

        return propertyRow.Cells[1].Text;
    }

    public void SetValue(string propertyName, string value)
    {
        var propertyRow = GetPropertyRow(propertyName);

        //TODO:Vladimir:Move to DataGridCell object in Orc.Automation
        propertyRow.Cells[1].Element.SetValue(value);
    }

    private DataItem GetPropertyRow(string propertyName)
    {
        var rows = Map.PropertiesDataGrid.Rows;
        var propertyRow = rows.FirstOrDefault(x => Equals(x.Cells[0].Text, propertyName));
        if (propertyRow is null)
        {
            //Throw to stop test
            throw new Exception($"Can't find row for property {propertyName}");
        }

        return propertyRow;
    }
}
