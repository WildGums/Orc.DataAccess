﻿namespace Orc.DataAccess
{
    using System.Collections.Generic;
    using Catel;
    using DataAccess;

    public static class IReaderExtensions
    {
        public static List<RecordTable> ReadAll(this IReader reader)
        {
            Argument.IsNotNull(() => reader);

            var results = new List<RecordTable>();
            while (true)
            {
                var result = new RecordTable
                {
                    Headers = reader.FieldHeaders
                };

                while (reader.Read())
                {
                    var record = new Record();
                    for (var i = 0; i < result.Headers.Length; i++)
                    {
                        var name = result.Headers[i];
                        var value = reader[i];

                        record[name] = value;
                    }

                    result.Add(record);
                }

                results.Add(result);

                if (!reader.NextResultAsync().GetAwaiter().GetResult())
                {
                    break;
                }
            }

            return results;
        }
    }
}
