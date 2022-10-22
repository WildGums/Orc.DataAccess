namespace Orc.DataAccess
{
    using System;

    public static class DataSourceBaseExtensions
    {
        public static bool IsValid(this DataSourceBase dataSource)
        {
            ArgumentNullException.ThrowIfNull(dataSource);

            return !dataSource.ValidationContext.HasErrors;
        }
    }
}
