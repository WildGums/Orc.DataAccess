namespace Orc.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;

    public static class DbDataReaderExtensions
    {
        public static string[] GetHeaders(this DbDataReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader);

            return Enumerable.Range(0, reader.FieldCount)
                .Select(reader.GetName)
                .ToArray();
        }

        public static List<RecordTable> ReadAll(this DbDataReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader);

            var results = new List<RecordTable>();
            while (true)
            {
                if (!reader.HasRows)
                {
                    break;
                }

                var result = new RecordTable
                {
                    Headers = reader.GetHeaders()
                };

                while (reader.Read())
                {
                    var record = new Record();
                    for (var i = 0; i < result.Headers.Length; i++)
                    {
                        var name = result.Headers[i];
                        var value = reader.GetValue(i);

                        record[name] = value;
                    }

                    result.Add(record);
                }

                results.Add(result);

                if (!reader.NextResult())
                {
                    break;
                }
            }

            return results;
        }
    }
}
