// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringBuilderService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.DataAccess.Controls
{
    using Database;

    public class ConnectionStringBuilderService : IConnectionStringBuilderService
    {
        #region IConnectionStringBuilderService Members
        public ConnectionState GetConnectionState(DbConnectionString connectionString)
        {
            var connectionStringStr = connectionString?.ToString();
            if (string.IsNullOrWhiteSpace(connectionStringStr))
            {
                return ConnectionState.Invalid;
            }

            var connection = connectionString.DbProvider.GetProvider()?.CreateConnection();
            if (connection == null)
            {
                return ConnectionState.Invalid;
            }

            // Try to open
            try
            {
                connection.ConnectionString = connectionStringStr;
                connection.Open();
            }
            catch
            {
                return ConnectionState.Invalid;
            }
            finally
            {
                connection.Dispose();
            }

            return ConnectionState.Valid;
        }
        #endregion
    }
}
