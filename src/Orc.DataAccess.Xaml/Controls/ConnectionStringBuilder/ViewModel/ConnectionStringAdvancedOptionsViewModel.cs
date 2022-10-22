namespace Orc.DataAccess.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Database;

    public class ConnectionStringAdvancedOptionsViewModel : ViewModelBase
    {
        public ConnectionStringAdvancedOptionsViewModel(DbConnectionString connectionString)
        {
            ArgumentNullException.ThrowIfNull(connectionString);

            ConnectionString = connectionString;
            ConnectionStringProperties = new List<DbConnectionStringProperty>();
        }

        public override string Title => "Advanced options";
        public IList<DbConnectionStringProperty> ConnectionStringProperties { get; private set; }
        public bool IsAdvancedOptionsReadOnly { get; set; }
        public DbConnectionString ConnectionString { get; }

        protected override Task InitializeAsync()
        {
            ConnectionStringProperties = ConnectionString.Properties.Values.Where(x => !x.IsSensitive)
                .OrderBy(x => x.Name)
                .ToList();

            return base.InitializeAsync();
        }
    }
}
