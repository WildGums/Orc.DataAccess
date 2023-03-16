namespace Orc.DataAccess;

using System;
using System.Data.Common;
using Catel.Logging;

public static class DbCommandExtensions
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    public static DbCommand AddParameters(this DbCommand dbCommand, DataSourceParameters parameters)
    {
        ArgumentNullException.ThrowIfNull(dbCommand);

        parameters.Parameters.ForEach(x => dbCommand.AddParameter(x));

        return dbCommand;
    }

    public static DbCommand AddParameter(this DbCommand dbCommand, DataSourceParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(dbCommand);
        ArgumentNullException.ThrowIfNull(parameter);

        if (parameter.Value is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Cannot add parameter with null value");
        }
        return dbCommand.AddParameter(parameter.Name, parameter.Value);
    }

    public static DbCommand AddParameter(this DbCommand dbCommand, string name, object value)
    {
        ArgumentNullException.ThrowIfNull(dbCommand);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(value);

        var parameter = dbCommand.CreateParameter();
        parameter.Value = value;
        parameter.ParameterName = name;
        dbCommand.Parameters.Add(parameter);

        return dbCommand;
    }
    public static long GetRecordsCount(this DbCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        long count = 0;
        using var reader = command.ExecuteReader();

        while (true)
        {
            while (reader.Read())
            {
                count++;
            }

            if (!reader.NextResult())
            {
                break;
            }

            if (!reader.HasRows)
            {
                break;
            }
        }

        return count;
    }
}
