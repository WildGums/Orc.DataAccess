// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDbDataSourceProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.DataAccess.Database
{
    using System.Collections.Generic;

    public interface IDbDataSourceProvider
    {
        IList<DbDataSource> GetDataSources();
    }
}
