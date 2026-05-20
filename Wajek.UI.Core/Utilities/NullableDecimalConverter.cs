using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Wajek.UI.Core.Utilities;

public class NullableDecimalConverter : IValueConverter {
    public object Convert(
        object? value, Type targetType,
        object? parameter, CultureInfo culture
    ) {
        if(value is null) return "";
        return value is decimal d
            ? d.ToString("N0", culture)
            : "";
    }

    public object? ConvertBack(object? value, Type targetType,
        object? parameter, CultureInfo culture) {
        var text = value?.ToString();

        if (string.IsNullOrWhiteSpace(text)) return null;

        return decimal.TryParse(
            text, 
            NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands,
            culture,
            out var result
        ) ? result : AvaloniaProperty.UnsetValue;
    }
}
