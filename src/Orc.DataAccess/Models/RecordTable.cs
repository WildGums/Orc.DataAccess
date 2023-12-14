namespace Orc.DataAccess;

using System;
using System.Collections.Generic;

public class RecordTable : List<Record>
{
    public RecordTable()
    {
        Headers = Array.Empty<string>();
    }

    public string[] Headers { get; set; }
}