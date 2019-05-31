// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelSource.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.DataAccess.Excel
{
    public class ExcelSource : DataSourceBase
    {
        #region Constructors
        public ExcelSource()
            : this(string.Empty)
        {
        }

        public ExcelSource(string location)
            : base(location)
        {
        }
        #endregion

        #region Properties
        public string FilePath { get; set; }
        public string Worksheet { get; set; }
        public string TopLeftCell { get; set; }
        #endregion
    }
}
