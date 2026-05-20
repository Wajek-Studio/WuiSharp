using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Wajek.UI.Core.Utilities;

public class NullableDateTimeConverter : IValueConverter {
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is null)
            return string.Empty;

        var format = parameter as string ?? "yyyy-MM-dd HH:mm:ss";

        return value switch
        {
            DateTime dt when dt == default => string.Empty,
            DateTime dt => dt.ToString(format, culture),

            DateTimeOffset dto => dto.ToString(format, culture),

            _ => string.Empty
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return null;

        if (DateTime.TryParse(
                value.ToString(),
                culture,
                DateTimeStyles.None,
                out var result))
        {
            return result;
        }

        return null;
    }
}