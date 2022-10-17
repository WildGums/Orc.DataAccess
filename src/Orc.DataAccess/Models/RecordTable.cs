namespace Orc.DataAccess
{
    using System.Collections.Generic;

    public class RecordTable : List<Record>
    {
        #region Properties
        public string[] Headers { get; set; }
        #endregion
    }
}
