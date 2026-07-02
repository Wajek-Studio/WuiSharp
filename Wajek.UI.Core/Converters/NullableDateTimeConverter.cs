using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Wajek.UI.Core.Converters;

public class NullableDateTimeConverter : IValueConverter {
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is null)
            return string.Empty;

        var format = parameter as string;

        return value switch
        {
            DateTime dt when dt == default => string.Empty,
            DateTime dt => dt.ToString(format ?? "yyyy-MM-dd HH:mm:ss", culture),

            DateTimeOffset dto => dto.ToString(format ?? "yyyy-MM-dd HH:mm:ss", culture),

            DateOnly doVal when doVal == default => string.Empty,
            DateOnly doVal => doVal.ToString(format ?? "yyyy-MM-dd", culture),

            _ => string.Empty
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return null;

        var strValue = value.ToString();

        if (targetType == typeof(DateOnly) || targetType == typeof(DateOnly?))
        {
            if (DateOnly.TryParse(strValue, culture, DateTimeStyles.None, out var doResult))
                return doResult;
        }
        else if (targetType == typeof(DateTimeOffset) || targetType == typeof(DateTimeOffset?))
        {
            if (DateTimeOffset.TryParse(strValue, culture, DateTimeStyles.None, out var dtoResult))
                return dtoResult;
        }
        else
        {
            if (DateTime.TryParse(strValue, culture, DateTimeStyles.None, out var dtResult))
                return dtResult;
        }

        return null;
    }
}