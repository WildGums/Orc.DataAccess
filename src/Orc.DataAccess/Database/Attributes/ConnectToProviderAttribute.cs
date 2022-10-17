namespace Orc.DataAccess.Database
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ConnectToProviderAttribute : Attribute
    {
        #region Constructors
        public ConnectToProviderAttribute(string providerInvariantName)
        {
            ProviderInvariantName = providerInvariantName;
        }
        #endregion

        #region Properties
        public string ProviderInvariantName { get; }
        #endregion
    }
}
