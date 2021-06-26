﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSourceParametersExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;

    public static class DataSourceParametersExtensions
    {
        #region Methods
        public static string ToArgsValueString(this DataSourceParameters queryParameters)
        {
            return queryParameters is not null ? string.Join(",", queryParameters.Parameters?.Select(x => $"'{x.Value}'") ?? new List<string>()) : string.Empty;
        }

        public static string ToArgsNamesString(this DataSourceParameters queryParameters, string argsPrefix = "")
        {
            return queryParameters is not null ? string.Join(",", queryParameters.Parameters?.Select(x => $"{argsPrefix}{x.Name}") ?? new List<string>()) : string.Empty;
        }

        public static bool IsEmpty(this DataSourceParameters databaseQueryParameters)
        {
            return databaseQueryParameters?.Parameters is null || !databaseQueryParameters.Parameters.Any();
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

            var parameters = databaseQueryParameters?.Parameters;
            var otherParameters = other?.Parameters;

            if (parameters is null || otherParameters is null)
            {
                return false;
            }

            return parameters.SequenceEqual(otherParameters, new NameAndTypeEqualsEqualyComparer());
        }
        #endregion

        #region Nested type: NameAndTypeEqualsEqualyComparer
        private class NameAndTypeEqualsEqualyComparer : IEqualityComparer<DataSourceParameter>
        {
            #region IEqualityComparer<DataSourceParameter> Members
            public bool Equals(DataSourceParameter x, DataSourceParameter y)
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
            #endregion
        }
        #endregion
    }
}
