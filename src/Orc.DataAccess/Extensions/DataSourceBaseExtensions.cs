// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSourceBaseExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.DataAccess
{
    using Catel;

    public static class DataSourceBaseExtensions
    {
        public static bool IsValid(this DataSourceBase dataSource)
        {
            Argument.IsNotNull(() => dataSource);

            return !dataSource.ValidationContext.HasErrors;
        }
    }
}
