// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConnectionStringBuilderService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.DataAccess.Controls
{
    using Database;

    public interface IConnectionStringBuilderService
    {
        #region Methods
        ConnectionState GetConnectionState(DbConnectionString connectionString);
        #endregion
    }
}
