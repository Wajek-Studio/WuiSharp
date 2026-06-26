using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Wajek.UI.Core.Converters;

public class NullableIntegerConverter : IValueConverter {
    public object Convert(
        object? value, Type targetType,
        object? parameter, CultureInfo culture
    ) {
        if (value is null) return "";
        return value is int i
            ? i.ToString("N0", culture)
            : "";
    }

    public object? ConvertBack(object? value, Type targetType,
        object? parameter, CultureInfo culture) {
        var text = value?.ToString();
        var isNullable = !targetType.IsValueType || Nullable.GetUnderlyingType(targetType) != null;

        if (string.IsNullOrWhiteSpace(text)) return isNullable ? null : 0;

        return int.TryParse(
            text, 
            NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands,
            culture,
            out var result
        ) ? result : (isNullable ? null : 0);
    }
}
