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
        SqlConnectionString CreateConnectionString(DbProviderInfo dbProvider, string connectionString = "");
        ConnectionState GetConnectionState(SqlConnectionString connectionString);
        IList<string> GetDataSources(SqlConnectionString connectionString);
        IList<string> GetDatabases(SqlConnectionString connectionString);
        #endregion
    }
}
