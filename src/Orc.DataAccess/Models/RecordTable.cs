namespace Orc.DataAccess
{
    using System.Collections.Generic;

    public class RecordTable : List<Record>
    {
        public string[] Headers { get; set; }
    }
}
