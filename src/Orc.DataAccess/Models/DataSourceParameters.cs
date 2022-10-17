namespace Orc.DataAccess
{
    using System;
    using System.Collections.Generic;
    using Catel.Data;

    [Serializable]
    public class DataSourceParameters : SavableModelBase<DataSourceParameters>
    {
        public DataSourceParameters()
        {
            Parameters = new List<DataSourceParameter>();
        }

        public List<DataSourceParameter> Parameters { get; set; }
    }
}
