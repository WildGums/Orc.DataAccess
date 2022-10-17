namespace Orc.DataAccess.Database
{
    using System.Data;
    using Catel;

    internal static class DataRowExtensions
    {
        #region Methods
        public static DbProviderInfo ToDbProviderInfo(this DataRow row)
        {
            Argument.IsNotNull(() => row);

            var providerInfo = new DbProviderInfo
            {
                Name = row["Name"]?.ToString(),
                Description = row["Description"]?.ToString(),
                InvariantName = row["InvariantName"]?.ToString(),
                AssemblyQualifiedName = row["AssemblyQualifiedName"]?.ToString()
            };

            if (string.IsNullOrWhiteSpace(providerInfo.Name))
            {
                providerInfo.Name = row["InvariantName"]?.ToString();

                if (string.IsNullOrWhiteSpace(providerInfo.Name))
                {
                    providerInfo.Name = "- nameless -";
                }
            }

            return providerInfo;
        }
        #endregion
    }
}
