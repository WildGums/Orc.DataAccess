// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbDataSource.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.DataAccess.Database
{
    using Catel;

    public class DbDataSource
    {
        public DbDataSource(string providerInvariantName, string instanceName)
        {
            Argument.IsNotNull(() => providerInvariantName);
            Argument.IsNotNull(() => instanceName);

            ProviderInvariantName = providerInvariantName;
            InstanceName = instanceName;
        }
        public string ProviderInvariantName { get; }
        public string InstanceName { get; }

        protected bool Equals(DbDataSource other)
        {
            return string.Equals(InstanceName, other.InstanceName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((DbDataSource)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return InstanceName != null ? InstanceName.GetHashCode() : 0;
            }
        }
    }
}
