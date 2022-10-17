namespace Orc.DataAccess
{
    using System.Linq;
    using Catel;
    using DataAccess;

    public static class RecordTableExtensions
    {
        public static bool HasHeaders(this RecordTable table)
        {
            Argument.IsNotNull(() => table);

            return table.Headers?.Any() ?? false;
        }
    }
}
