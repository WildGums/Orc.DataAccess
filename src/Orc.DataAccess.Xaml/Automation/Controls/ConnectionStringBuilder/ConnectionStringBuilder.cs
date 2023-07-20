namespace Orc.DataAccess.Automation.Controls;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(ConnectionStringBuilder))]
public class ConnectionStringBuilder : FrameworkElement<ConnectionStringBuilderModel, ConnectionStringBuilderMap>
{
    public ConnectionStringBuilder(AutomationElement element)
        : base(element)
    {
    }
}
