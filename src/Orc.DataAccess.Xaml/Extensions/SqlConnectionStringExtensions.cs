// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlConnectionStringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.DataAccess
{
    using Database;

    public static class SqlConnectionStringExtensions
    {
        #region Methods
        public static DbConnectionStringProperty TryGetProperty(this DbConnectionString connectionString, string propertyName)
        {
            var properties = connectionString?.Properties;
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
        #endregion
    }
}

