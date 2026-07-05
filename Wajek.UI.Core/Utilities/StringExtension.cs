namespace Wajek.UI.Core.Utilities;

public static class StringExtension
{
    public static string ReplaceLastOccurrence(this string value, string find, string replace)
    {
        int place = value.LastIndexOf(find);
        if (place == -1) return value;
        return value.Remove(place, find.Length).Insert(place, replace);
    }
}