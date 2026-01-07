namespace Orc.DataAccess;

using System.Collections.Generic;
using Catel.Data;

public class DataSourceParameters : ModelBase
{
    public DataSourceParameters()
    {
        Parameters = new List<DataSourceParameter>();
    }

    public List<DataSourceParameter> Parameters { get; set; }
}
