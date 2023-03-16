namespace Orc.DataAccess.Controls;

using System;
using System.Windows.Media;
using Catel.MVVM.Converters;
using Database;

public class ConnectionStateToColorBrushValueConverter : ValueConverterBase<ConnectionState, SolidColorBrush>
{
    protected override object Convert(ConnectionState value, Type targetType, object? parameter)
    {
        return value switch
        {
            ConnectionState.Undefined => new SolidColorBrush(Colors.Gray),
            ConnectionState.Valid => new SolidColorBrush(Colors.Green),
            ConnectionState.Invalid => new SolidColorBrush(Colors.Red),
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}
