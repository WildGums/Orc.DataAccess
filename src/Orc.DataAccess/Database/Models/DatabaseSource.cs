namespace Orc.DataAccess.Database
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using DataAccess;

    public class DatabaseSource : DataSourceBase
    {
        public DatabaseSource()
            : this(string.Empty)
        {
        }

        public DatabaseSource(string location)
            : base(location)
        {
        }

        public string Schema { get; set; }
        public string Table { get; set; }
        public TableType TableType { get; set; }

        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public string ProviderName { get; set; }

        protected override bool TryConvertFromString(string propertyName, string propertyValueStr, out object propertyValue)
        {
            if (propertyName != nameof(TableType))
            {
                return base.TryConvertFromString(propertyName, propertyValueStr, out propertyValue);
            }

            if (Enum.TryParse(propertyValueStr, true, out TableType tableType))
            {
                propertyValue = tableType;

                return true;
            }

            propertyValue = null;
            return false;
        }
    }
}
