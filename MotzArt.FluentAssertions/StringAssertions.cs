using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using MotzArt.FluentAssertions.Helpers;
using MotzArt.FluentStrings;
using NUnit.Framework;

namespace MotzArt.FluentAssertions;

public static class StringAssertions
{
    [return: NotNullIfNotNull(nameof(str))]
    public static string? ShouldBeNullOrEmpty(this string? str, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Is.Null.Or.Empty, message, actualExpression: strExpression);
        return str;
    }

    public static string ShouldNotBeNullOrEmpty(this string? str, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Is.Not.Null.And.Not.Empty, message, actualExpression: strExpression, constraintExpression:"Is.Not.NullOrEmpty");
        return str.ShouldNotBeNull();
    }

    public static string ShouldNotBeNullOrWhiteSpace(this string? value, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "")
    {
        Assert.That(value, Is.Not.Null.And.Not.Empty.And.Not.WhiteSpace, message, actualExpression: valueExpression, constraintExpression: "Is.Not.NullOrWhitespace");
        return value!;
    }
    
    public static void ShouldBeEmpty(this string str, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Is.Empty, message, actualExpression: strExpression);
    }

    public static string? ShouldNotBeEmpty(this string? str, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Is.Not.Empty, message, actualExpression: strExpression);
        return str;
    }

    public static string ShouldStartWith(this string? str, string expectedStart, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Does.StartWith(expectedStart), message, actualExpression: strExpression, constraintExpression: $"Does.StartWith({expectedStart})");
        return str.ShouldNotBeNull();
    }
    
    public static string ShouldStartWith(this string? str, string expectedStart, StringComparison comparison, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        if (str == null || !str.StartsWith(expectedStart, comparison))
        {
            var writer = new MessageWriter();
            writer.WriteMessageIfNotEmpty(message);
            
            writer.WriteAssertThat(strExpression, $"Does.StartWith({DescribeValuesExtensions.FormatValue(expectedStart)}).Using({nameof(StringComparison)}.{comparison})");
            
            writer.WriteExpected();
            writer.Write("String starting with ");
            writer.WriteValue(expectedStart);
            writer.WriteLine();

            writer.WriteActual();
            writer.WriteValue(str);
            writer.WriteLine();

            writer.ReportFailure();
        }

        return str;
    }

    public static string ShouldEndWith(this string? str, string expectedEnd, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Does.EndWith(expectedEnd), message, actualExpression: strExpression, constraintExpression: $"Does.EndWith({DescribeValuesExtensions.FormatValue(expectedEnd)})");
        return str.ShouldNotBeNull();
    }

    public static string ShouldEndWith(this string? str, string expectedEnd, StringComparison comparison, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        if (str == null || !str.EndsWith(expectedEnd, comparison))
        {
            var writer = new MessageWriter();
            writer.WriteMessageIfNotEmpty(message);

            writer.WriteAssertThat(strExpression, $"Does.EndWith({DescribeValuesExtensions.FormatValue(expectedEnd)}).Using({nameof(StringComparison)}.{comparison})");

            writer.WriteExpected();
            writer.Write("String ending with ");
            writer.WriteValue(expectedEnd);
            writer.WriteLine();

            writer.WriteActual();
            writer.WriteValue(str);
            writer.WriteLine();

            writer.ReportFailure();
        }

        return str;
    }

    public static string ShouldContain(this string? str, string expectedSubstring, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Contains.Substring(expectedSubstring), message, actualExpression: strExpression, constraintExpression: $"Contains.Substring({DescribeValuesExtensions.FormatValue(expectedSubstring)})");
        return str.ShouldNotBeNull();
    }

    public static string ShouldContain(this string? str, string expectedSubstring, StringComparison comparison, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Contains.Substring(expectedSubstring).Using(comparison), message, actualExpression: strExpression, constraintExpression: $"Contains.Substring({DescribeValuesExtensions.FormatValue(expectedSubstring)}).Using({nameof(StringComparison)}.{comparison})");
        return str.ShouldNotBeNull();
    }

    public static string? ShouldNotContain(this string? str, string unexpectedSubstring, StringComparison comparison, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        if (str != null && str.Contains(unexpectedSubstring, comparison))
        {
            var writer = new MessageWriter();
            writer.WriteMessageIfNotEmpty(message);

            writer.WriteAssertThat(strExpression, $"Does.Not.Contain({DescribeValuesExtensions.FormatValue(unexpectedSubstring)}).Using({nameof(StringComparison)}.{comparison})");

            writer.WriteExpected();
            writer.Write("String not containing ");
            writer.WriteValue(unexpectedSubstring);
            writer.WriteLine();

            writer.WriteActual();
            writer.WriteValue(str);
            writer.WriteLine();

            writer.ReportFailure();
        }

        return str;
    }

    public static string? ShouldNotContain(this string? str, string unexpectedSubstring, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Does.Not.Contains(unexpectedSubstring), message, actualExpression: strExpression, $"Does.Not.Contain({DescribeValuesExtensions.FormatValue(unexpectedSubstring)})");
        return str;
    }

    [return: NotNullIfNotNull(nameof(expectedValue))]
    public static string? ShouldBe(this string? str, string? expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Is.EqualTo(expectedValue), message, actualExpression: strExpression, constraintExpression: $"Is.EqualTo({DescribeValuesExtensions.FormatValue(expectedValue)})");
        return str;
    }

    [return: NotNullIfNotNull(nameof(expectedValue))]
    public static string? ShouldBe(this string? str, string? expectedValue, StringComparison comparison, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        var comparer = GetComparer(comparison);
        Assert.That(str, Is.EqualTo(expectedValue).Using(comparer), message, actualExpression: strExpression, constraintExpression: $"Is.EqualTo({DescribeValuesExtensions.FormatValue(expectedValue)}).Using({nameof(StringComparison)}.{comparison})");
        return str;
    }

    public static string? ShouldNotBe(this string? str, string? expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Is.Not.EqualTo(expectedValue), message, actualExpression: strExpression, constraintExpression: $"Is.Not.EqualTo({DescribeValuesExtensions.FormatValue(expectedValue)})");
        return str;
    }

    public static string? ShouldNotBe(this string? str, string? expectedValue, StringComparison comparison, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        var comparer = GetComparer(comparison);
        Assert.That(str, Is.Not.EqualTo(expectedValue).Using(comparer), message, actualExpression: strExpression, constraintExpression: $"Is.Not.EqualTo({DescribeValuesExtensions.FormatValue(expectedValue)}).Using({nameof(StringComparison)}.{comparison})");
        return str;
    }

    private static StringComparer GetComparer(StringComparison comparison)
    {
        return StringComparer.FromComparison(comparison);
    }
}