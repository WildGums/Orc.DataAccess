namespace Orc.DataAccess.Controls;

using System;
using System.Windows;
using System.Windows.Media;
using Catel.MVVM.Converters;
using Database;

public class ConnectionStateToColorBrushValueConverter : ValueConverterBase<ConnectionState, SolidColorBrush>
{
    protected override object Convert(ConnectionState value, Type targetType, object? parameter)
    {
        return value switch
        {
            ConnectionState.Undefined => Application.Current.FindResource("Orc.Brushes.Control.Default.Border") as SolidColorBrush ?? Brushes.Gray,
            ConnectionState.Valid => Brushes.Green,
            ConnectionState.Invalid => Brushes.Red,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}
