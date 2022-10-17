namespace Orc.DataAccess.Database
{
    public class DbProviderInfo
    {
        #region Properties
        public string Name { get; set; }
        public string InvariantName { get; set; }
        public string Description { get; set; }
        public string AssemblyQualifiedName { get; set; }
        #endregion

        #region Methods
        protected bool Equals(DbProviderInfo other)
        {
            return string.Equals(InvariantName, other.InvariantName);
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

            return Equals((DbProviderInfo)obj);
        }

        public override int GetHashCode()
        {
            return (InvariantName is not null ? InvariantName.GetHashCode() : 0);
        }
        #endregion
    }
}
