namespace Orc.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using Catel.Caching;
    using Catel.Data;
    using Catel.Reflection;
    using ValidationContext = Catel.Data.ValidationContext;

    public abstract class DataSourceBase : ModelBase
    {
        public IValidationContext ValidationContext { get; private set; }

        private static readonly CacheStorage<Type, PropertyInfo[]> PropertiesCache = new();

        protected readonly Dictionary<string, string> _properties = new();

        protected DataSourceBase()
            : this(string.Empty)
        {
        }

        protected DataSourceBase(string location)
        {
            ParseLocation(location);
            ValidationContext = new ValidationContext();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;
            if (propertyName == nameof(IsDirty) || propertyName == nameof(ValidationContext))
            {
                return;
            }

            base.OnPropertyChanged(e);

            if (e.NewValue is null)
            {
                _properties.Remove(propertyName);
            }
            else
            {
                _properties[propertyName] = e.NewValue?.ToString();
            }

            Validate();
        }

        public IReadOnlyDictionary<string, string> AsDictionary()
        {
            return _properties;
        }

        public void SetProperty(string propertyName, string propertyValueStr)
        {
            var type = GetType();
            var properties = PropertiesCache.GetFromCacheOrFetch(type, () => type.GetPropertiesEx());
            var property = properties.First(x => x.Name == propertyName);
            SetPropertyValue(property, propertyValueStr);
        }

        public virtual void Validate()
        {
            using (SuspendChangeNotifications(false))
            {
                ValidationContext = new ValidationContext();

                var type = GetType();
                var properties = PropertiesCache.GetFromCacheOrFetch(type, () => type.GetPropertiesEx());
                foreach (var property in properties)
                {
                    if (!property.IsDecoratedWithAttribute(typeof(RequiredAttribute)))
                    {
                        continue;
                    }

                    var propertyValue = property.GetValue(this);
                    if (propertyValue is null)
                    {
                        ValidationContext.AddValidationError($"Required field '{property.Name}' is empty");
                    }
                }
            }
        }

        private void ParseLocation(string location)
        {
            var type = GetType();
            var properties = PropertiesCache.GetFromCacheOrFetch(type, () => type.GetPropertiesEx());
            var dictionary = KeyValueStringParser.Parse(location);
            using (SuspendChangeNotifications(false))
            {
                ValidationContext = new ValidationContext();

                foreach (var property in properties)
                {
                    var propertyName = property.Name;

                    if (dictionary.TryGetValue(propertyName, out var propertyValueStr))
                    {
                        dictionary.Remove(propertyName);

                        SetPropertyValue(property, propertyValueStr);
                    }
                    else
                    {
                        if (property.IsDecoratedWithAttribute(typeof(RequiredAttribute)))
                        {
                            ValidationContext.AddValidationError($"Required field '{propertyName}' is empty");
                        }
                    }
                }
            }
        }

        private void SetPropertyValue(PropertyInfo property, string propertyValueStr)
        {
            var propertyName = property.Name;
            if (TryConvertFromString(propertyName, propertyValueStr, out var propertyValue))
            {
                property.SetValue(this, propertyValue);
                _properties[propertyName] = propertyValueStr;
            }
            else
            {
                ValidationContext.AddValidationError($"Can't convert {propertyValueStr} as {propertyName} value\r\n");
            }
        }

        protected virtual bool TryConvertFromString(string propertyName, string propertyValueStr, out object propertyValue)
        {
            propertyValue = propertyValueStr;

            return true;
        }

        public virtual string GetLocation()
        {
            if (ValidationContext.HasErrors)
            {
                return string.Empty;
            }

            return KeyValueStringParser.FormatToKeyValueString(_properties);
        }

        public override string ToString()
        {
            return GetLocation();
        }
    }
}
