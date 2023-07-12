namespace Orc.DataAccess.Database;

using System;
using System.Data;

public static class DataRowExtensions
{
    public static DbProviderInfo ToDbProviderInfo(this DataRow row)
    {
        ArgumentNullException.ThrowIfNull(row);

        var invariantName = row["InvariantName"].ToString();
        var name = row["Name"].ToString();
        if (string.IsNullOrWhiteSpace(name))
        {
            name = invariantName?.Replace(".", " ") ?? "- nameless -";
        }
        var description = row["Description"].ToString();
        var assemblyQualifiedName = row["AssemblyQualifiedName"].ToString();

        var providerInfo = new DbProviderInfo(name,
            invariantName ?? string.Empty,
            description ?? string.Empty,
            assemblyQualifiedName ?? string.Empty);

        return providerInfo;
    }
}
