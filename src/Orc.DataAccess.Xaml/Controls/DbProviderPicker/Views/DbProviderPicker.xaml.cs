namespace Orc.DataAccess.Controls
{
    using System.Windows;
    using Catel.MVVM.Views;
    using Database;

    public sealed partial class DbProviderPicker
    {
        public static readonly DependencyProperty? DbProviderProperty = DependencyProperty.Register(
            nameof(DbProvider), typeof(DbProviderInfo), typeof(DbProviderPicker), new PropertyMetadata(default(DbProviderInfo)));

        static DbProviderPicker()
        {
            typeof(DbProviderPicker).AutoDetectViewPropertiesToSubscribe();
        }

        public DbProviderPicker()
        {
            InitializeComponent();
        }

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewModelWins)]
        public DbProviderInfo? DbProvider
        {
            get { return (DbProviderInfo?)GetValue(DbProviderProperty); }
            set { SetValue(DbProviderProperty, value); }
        }
    }
}
