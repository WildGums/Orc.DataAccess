#nullable disable
namespace Orc.DataAccess.Automation.Converters;

using Database;
using Orc.Automation;

public class DbProviderInfoAutomationConverter : SerializationValueConverterBase<DbProviderInfo, SerializableDbProviderInfo>
{
    public override object ConvertFrom(DbProviderInfo info)
    {
        return new SerializableDbProviderInfo
        {
            Name = info.Name,
            AssemblyQualifiedName = info.AssemblyQualifiedName,
            Description = info.Description,
            InvariantName = info.InvariantName
        };
    }

    public override object ConvertTo(SerializableDbProviderInfo serializable)
    {
        return new DbProviderInfo(serializable.Name, serializable.InvariantName,
            serializable.Description, serializable.AssemblyQualifiedName);
    }
}
