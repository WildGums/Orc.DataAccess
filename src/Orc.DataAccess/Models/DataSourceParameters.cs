// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSourceParameters.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.DataAccess
{
    using System;
    using System.Collections.Generic;
    using Catel.Data;

    [Serializable]
    public class DataSourceParameters : SavableModelBase<DataSourceParameters>
    {
        #region Constructors
        public DataSourceParameters()
        {
            Parameters = new List<DataSourceParameter>();
        }
        #endregion

        #region Properties
        public List<DataSourceParameter> Parameters { get; set; }
        #endregion
    }
}
