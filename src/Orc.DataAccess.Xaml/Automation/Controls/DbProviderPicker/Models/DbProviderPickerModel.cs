#nullable disable
namespace Orc.DataAccess.Automation.Controls;

using Orc.Automation;
using Database;

[ActiveAutomationModel]
public class DbProviderPickerModel : FrameworkElementModel
{
    public DbProviderPickerModel(AutomationElementAccessor accessor)
        : base(accessor)
    {
    }

    public DbProviderInfo DbProvider { get; set; }
}
