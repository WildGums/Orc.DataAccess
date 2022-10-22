namespace Orc.DataAccess
{
    using Database;

    public static class SqlConnectionStringExtensions
    {
        public static DbConnectionStringProperty? GetProperty(this DbConnectionString connectionString, string propertyName)
        {
            var properties = connectionString.Properties;
            if (properties is null)
            {
                return null;
            }

            var upperInvariantPropertyName = propertyName.ToUpperInvariant();
            if (properties.TryGetValue(upperInvariantPropertyName, out var property))
            {
                return property;
            }

            if (properties.TryGetValue(upperInvariantPropertyName.Replace(" ", string.Empty), out property))
            {
                return property;
            }

            return null;
        }
    }
}

