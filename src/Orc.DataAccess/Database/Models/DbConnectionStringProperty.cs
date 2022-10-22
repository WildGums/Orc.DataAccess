namespace Orc.DataAccess.Database
{
    using System;
    using System.ComponentModel;
    using System.Data.Common;
    using Catel.Data;

    public class DbConnectionStringProperty : ObservableObject
    {
        private readonly DbConnectionStringBuilder _dbConnectionStringBuilder;
        private readonly PropertyDescriptor _propertyDescriptor;

        public DbConnectionStringProperty(bool isSensitive, DbConnectionStringBuilder dbConnectionStringBuilder, PropertyDescriptor propertyDescriptor)
        {
            ArgumentNullException.ThrowIfNull(dbConnectionStringBuilder);
            ArgumentNullException.ThrowIfNull(propertyDescriptor);

            _dbConnectionStringBuilder = dbConnectionStringBuilder;
            _propertyDescriptor = propertyDescriptor;
            Name = propertyDescriptor.DisplayName.ToUpperInvariant();
            IsSensitive = isSensitive;
        }

        public string Name { get; }
        public bool IsSensitive { get; }

        public object? Value
        {
            get => _propertyDescriptor.GetValue(_dbConnectionStringBuilder);

            set
            {
                var currentValue = _propertyDescriptor.GetValue(_dbConnectionStringBuilder);
                if (Equals(currentValue, value))
                {
                    return;
                }

                _propertyDescriptor.SetValue(_dbConnectionStringBuilder, value);
                RaisePropertyChanged(nameof(Value));
            }
        }
    }
}
