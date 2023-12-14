namespace Orc.DataAccess;

using System;
using System.Linq;

public static class RecordTableExtensions
{
    public static bool HasHeaders(this RecordTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return table.Headers.Any();
    }
}
