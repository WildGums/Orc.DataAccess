namespace Orc.DataAccess.Automation.Controls;

using Database;
using Orc.Automation;
using Orc.DataAccess.Controls;

[ActiveAutomationModel]
public class ConnectionStringBuilderModel : FrameworkElementModel
{
    public ConnectionStringBuilderModel(AutomationElementAccessor accessor)
        : base(accessor)
    {
    }

    public string? ConnectionString { get; set; }
    public string? DatabaseProvider { get; set; }
    public bool IsInEditMode { get; set; }
    public ConnectionState ConnectionState { get; set; }
    public bool IsAdvancedOptionsReadOnly { get; set; }
    public bool IsEditable { get; set; }
    public DbConnectionPropertyDefinitionCollection? DefaultProperties { get; set; }
}
