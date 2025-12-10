using System.Diagnostics.CodeAnalysis;

namespace MotzArt.FluentAssertions.Tests;

public static class StringExtensions
{
    [return: NotNullIfNotNull(nameof(str))]
    public static string? FirstLine(this string? str, bool trim = false)
    {
        if (str == null)
        {
            return null;
        }

        var span = str.AsSpan();

        var index = str.IndexOf('\n');
        if (index >= 0)
        {
            span = span.Slice(0, index);
        }

        if (trim)
        {
            span = span.Trim();
        }

        return span.ToString();
    }
}