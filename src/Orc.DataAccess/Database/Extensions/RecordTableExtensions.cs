namespace Orc.DataAccess
{
    using System;
    using System.Linq;
    using Catel;
    using DataAccess;

    public static class RecordTableExtensions
    {
        public static bool HasHeaders(this RecordTable table)
        {
            ArgumentNullException.ThrowIfNull(table);

            return table.Headers?.Any() ?? false;
        }
    }
}
