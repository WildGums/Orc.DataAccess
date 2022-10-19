namespace Orc.DataAccess.Database
{
    using System.Data;

    internal static class DataRowExtensions
    {
        public static DbProviderInfo ToDbProviderInfo(this DataRow row)
        {
            var name = row["Name"]?.ToString() ?? row["InvariantName"]?.ToString() ?? "- nameless -";
            var description = row["Description"]?.ToString();
            var invariantName = row["InvariantName"]?.ToString();
            var assemblyQualifiedName = row["AssemblyQualifiedName"]?.ToString();

            var providerInfo = new DbProviderInfo(name ?? string.Empty, description ?? string.Empty, invariantName ?? string.Empty, assemblyQualifiedName ?? string.Empty);

            return providerInfo;
        }
    }
}
