﻿namespace Orc.DataAccess;

using System;
using System.Collections.Generic;
using System.Linq;

public static class DataSourceParametersExtensions
{
    public static string ToArgsValueString(this DataSourceParameters queryParameters)
    {
        ArgumentNullException.ThrowIfNull(queryParameters);

        return string.Join(",", queryParameters.Parameters.Select(x => $"'{x.Value}'"));
    }

    public static string ToArgsNamesString(this DataSourceParameters queryParameters, string argsPrefix = "")
    {
        ArgumentNullException.ThrowIfNull(queryParameters);

        return string.Join(",", queryParameters.Parameters.Select(x => $"{argsPrefix}{x.Name}"));
    }

    public static bool IsEmpty(this DataSourceParameters databaseQueryParameters)
    {
        ArgumentNullException.ThrowIfNull(databaseQueryParameters);

        return !databaseQueryParameters.Parameters.Any();
    }

    public static bool IsSameAs(this DataSourceParameters databaseQueryParameters, DataSourceParameters other)
    {
        if (ReferenceEquals(databaseQueryParameters, other))
        {
            return true;
        }

        if (databaseQueryParameters.IsEmpty() && other.IsEmpty())
        {
            return true;
        }

        var parameters = databaseQueryParameters.Parameters;
        var otherParameters = other.Parameters;

        return parameters.SequenceEqual(otherParameters, new NameAndTypeEqualsEquallyComparer());
    }

    #region Nested type: NameAndTypeEqualsEqualyComparer
    private class NameAndTypeEqualsEquallyComparer : IEqualityComparer<DataSourceParameter>
    {
        public bool Equals(DataSourceParameter? x, DataSourceParameter? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return x.Name == y.Name && x.Type == y.Type;
        }

        public int GetHashCode(DataSourceParameter obj)
        {
            unchecked
            {
                var hashCode = 1168257605;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(obj.Name);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(obj.Type);
                return hashCode;
            }
        }
    }
    #endregion
}
