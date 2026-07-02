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

            DateOnly d when d == default => string.Empty,
            DateOnly d => d.ToString(format, culture),

            _ => string.Empty
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return null;

        var text = value.ToString()!;
        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (underlyingType == typeof(DateOnly))
        {
            return DateOnly.TryParse(text, culture, DateTimeStyles.None, out var d)
                ? d
                : null;
        }

        if (underlyingType == typeof(DateTime))
        {
            return DateTime.TryParse(text, culture, DateTimeStyles.None, out var dt)
                ? dt
                : null;
        }

        if (underlyingType == typeof(DateTimeOffset))
        {
            return DateTimeOffset.TryParse(text, culture, DateTimeStyles.None, out var dto)
                ? dto
                : null;
        }

        return null;
    }
}