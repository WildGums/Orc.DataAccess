// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataSourceSchemaProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.DataAccess.Database
{
    public interface IDataSourceSchemaProvider
    {
        DbDataSourceSchema GetSchema(DbConnectionString connectionString);
    }
}
