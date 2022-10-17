namespace Orc.DataAccess.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Database;

    public class ConnectionStringAdvancedOptionsViewModel : ViewModelBase
    {
        #region Constructors
        public ConnectionStringAdvancedOptionsViewModel(DbConnectionString connectionString)
        {
            Argument.IsNotNull(() => connectionString);

            ConnectionString = connectionString;
        }
        #endregion

        #region Properties
        public override string Title => "Advanced options";
        public IList<DbConnectionStringProperty> ConnectionStringProperties { get; private set; }
        public bool IsAdvancedOptionsReadOnly { get; set; }
        public DbConnectionString ConnectionString { get; }
        #endregion

        #region Methods
        protected override Task InitializeAsync()
        {
            ConnectionStringProperties = ConnectionString.Properties.Values.Where(x => !x.IsSensitive)
                .OrderBy(x => x.Name)
                .ToList();

            return base.InitializeAsync();
        }
        #endregion
    }
}
