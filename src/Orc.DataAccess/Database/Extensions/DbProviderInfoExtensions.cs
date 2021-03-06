﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbProviderInfoExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.DataAccess.Database
{
    using Catel;

    public static class DbProviderInfoExtensions
    {
        public static DbConnectionString CreateConnectionString(this DbProviderInfo dbProviderInfo, string connectionString = null)
        {
            Argument.IsNotNull(() => dbProviderInfo);

            return dbProviderInfo.GetProvider()?.CreateConnectionString(connectionString);
        }

        public static DbProvider GetProvider(this DbProviderInfo dbProviderInfo)
        {
            Argument.IsNotNull(() => dbProviderInfo);

            return DbProvider.GetRegisteredProvider(dbProviderInfo.InvariantName);
        }
    }
}
