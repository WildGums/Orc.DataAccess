namespace Orc.DataAccess.Database;

using System.Collections.Generic;

public interface IDbDataSourceProvider
{
    IReadOnlyList<DbDataSource> GetDataSources();
}
