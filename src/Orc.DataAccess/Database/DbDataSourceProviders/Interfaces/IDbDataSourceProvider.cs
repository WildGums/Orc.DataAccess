namespace Orc.DataAccess.Database;

using System.Collections.Generic;

public interface IDbDataSourceProvider
{
    IList<DbDataSource> GetDataSources();
}