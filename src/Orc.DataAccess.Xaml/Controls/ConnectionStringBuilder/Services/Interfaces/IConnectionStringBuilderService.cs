// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConnectionStringBuilderService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.DataAccess.Controls
{
    using System.Collections.Generic;
    using Database;

    public interface IConnectionStringBuilderService
    {
        #region Methods
        void AddDataSourceProvider(string invariantName, IDataSourceProvider provider);
        DbConnectionString CreateConnectionString(DbProviderInfo dbProvider, string connectionString = "");
        ConnectionState GetConnectionState(DbConnectionString connectionString);
        IList<string> GetDataSources(DbConnectionString connectionString);
        IList<string> GetDatabases(DbConnectionString connectionString);
        #endregion
    }
}
