namespace Orc.DataAccess.Database
{
    using System;
    using Catel;

    public class DbDataSource
    {
        public DbDataSource(string providerInvariantName, string instanceName)
        {
            ArgumentNullException.ThrowIfNull(providerInvariantName);
            ArgumentNullException.ThrowIfNull(instanceName);

            ProviderInvariantName = providerInvariantName;
            InstanceName = instanceName;
        }
        public string ProviderInvariantName { get; }
        public string InstanceName { get; }

        protected bool Equals(DbDataSource other)
        {
            return string.Equals(InstanceName, other.InstanceName);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
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
                return InstanceName is not null ? InstanceName.GetHashCode() : 0;
            }
        }
    }
}
