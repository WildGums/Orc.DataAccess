namespace Orc.DataAccess.Database;

using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DataAccess;

public class DatabaseSource : DataSourceBase
{
    public DatabaseSource()
        : this(string.Empty)
    {
       
    }

    public DatabaseSource(string location)
        : base(location)
    {
        // Note: the base parses the location and dynamically sets the values
        // using reflection. To make sure these values are not null, they are
        // set to string.Empty in case they might be set to null by reflection (or not set at all)
        Schema ??= string.Empty;
        Table ??= string.Empty;
        ConnectionString ??= string.Empty;
        ProviderName ??= string.Empty;
    }

    public string Schema { get; set; }
    public string Table { get; set; }
    public TableType TableType { get; set; }

    [Required]
    public string ConnectionString { get; set; }

    [Required]
    public string ProviderName { get; set; }

    protected override bool TryConvertFromString(string propertyName, string propertyValueStr, [NotNullWhen(true)] out object? propertyValue)
    {
        if (propertyName != nameof(TableType))
        {
            return base.TryConvertFromString(propertyName, propertyValueStr, out propertyValue);
        }

        if (Enum.TryParse(propertyValueStr, true, out TableType tableType))
        {
            propertyValue = tableType;

            return true;
        }

        propertyValue = null;
        return false;
    }
}