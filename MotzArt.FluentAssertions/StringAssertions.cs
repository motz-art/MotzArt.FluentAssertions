using MotzArt.FluentAssertions.Helpers;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MotzArt.FluentAssertions;

/// <summary>
/// Assertions for <see cref="string"/> values.
/// </summary>
public static class StringAssertions
{
    /// <summary>
    /// Asserts that <paramref name="str"/> is <see langword="null"/> or empty.
    /// </summary>
    /// <returns>The asserted string, for chaining further assertions.</returns>
    [return: NotNullIfNotNull(nameof(str))]
    public static string? ShouldBeNullOrEmpty(this string? str, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Is.Null.Or.Empty, message, actualExpression: strExpression);
        return str;
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> isn't <see langword="null"/> or empty.
    /// </summary>
    /// <returns>The asserted string as non-nullable, for chaining further assertions.</returns>
    public static string ShouldNotBeNullOrEmpty(this string? str, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Is.Not.Null.And.Not.Empty, message, actualExpression: strExpression, constraintExpression: "Is.Not.NullOrEmpty");
        return str.ShouldNotBeNull();
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> isn't <see langword="null"/>, empty or white space only.
    /// </summary>
    /// <returns>The asserted string as non-nullable, for chaining further assertions.</returns>
    public static string ShouldNotBeNullOrWhiteSpace(this string? value, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "")
    {
        Assert.That(value, Is.Not.Null.And.Not.Empty.And.Not.WhiteSpace, message, actualExpression: valueExpression, constraintExpression: "Is.Not.NullOrWhitespace");
        return value!;
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> is empty.
    /// </summary>
    public static void ShouldBeEmpty(this string str, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Is.Empty, message, actualExpression: strExpression);
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> isn't empty.
    /// </summary>
    /// <returns>The asserted string, for chaining further assertions.</returns>
    public static string? ShouldNotBeEmpty(this string? str, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Is.Not.Empty, message, actualExpression: strExpression);
        return str;
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> starts with <paramref name="expectedStart"/>. Fails for <see langword="null"/>.
    /// </summary>
    /// <returns>The asserted string as non-nullable, for chaining further assertions.</returns>
    public static string ShouldStartWith(this string? str, string expectedStart, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Does.StartWith(expectedStart), message, actualExpression: strExpression, constraintExpression: $"Does.StartWith({expectedStart})");
        return str.ShouldNotBeNull();
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> starts with <paramref name="expectedStart"/> using <paramref name="comparison"/>. Fails for <see langword="null"/>.
    /// </summary>
    /// <returns>The asserted string as non-nullable, for chaining further assertions.</returns>
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

    /// <summary>
    /// Asserts that <paramref name="str"/> ends with <paramref name="expectedEnd"/>. Fails for <see langword="null"/>.
    /// </summary>
    /// <returns>The asserted string as non-nullable, for chaining further assertions.</returns>
    public static string ShouldEndWith(this string? str, string expectedEnd, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Does.EndWith(expectedEnd), message, actualExpression: strExpression, constraintExpression: $"Does.EndWith({DescribeValuesExtensions.FormatValue(expectedEnd)})");
        return str.ShouldNotBeNull();
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> ends with <paramref name="expectedEnd"/> using <paramref name="comparison"/>. Fails for <see langword="null"/>.
    /// </summary>
    /// <returns>The asserted string as non-nullable, for chaining further assertions.</returns>
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

    /// <summary>
    /// Asserts that <paramref name="str"/> contains <paramref name="expectedSubstring"/>. Fails for <see langword="null"/>.
    /// </summary>
    /// <returns>The asserted string as non-nullable, for chaining further assertions.</returns>
    public static string ShouldContain(this string? str, string expectedSubstring, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Contains.Substring(expectedSubstring), message, actualExpression: strExpression, constraintExpression: $"Contains.Substring({DescribeValuesExtensions.FormatValue(expectedSubstring)})");
        return str.ShouldNotBeNull();
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> contains <paramref name="expectedSubstring"/> using <paramref name="comparison"/>. Fails for <see langword="null"/>.
    /// </summary>
    /// <returns>The asserted string as non-nullable, for chaining further assertions.</returns>
    public static string ShouldContain(this string? str, string expectedSubstring, StringComparison comparison, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Contains.Substring(expectedSubstring).Using(comparison), message, actualExpression: strExpression, constraintExpression: $"Contains.Substring({DescribeValuesExtensions.FormatValue(expectedSubstring)}).Using({nameof(StringComparison)}.{comparison})");
        return str.ShouldNotBeNull();
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> doesn't contain <paramref name="unexpectedSubstring"/> using <paramref name="comparison"/>.
    /// Passes for <see langword="null"/>.
    /// </summary>
    /// <returns>The asserted string, for chaining further assertions.</returns>
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

    /// <summary>
    /// Asserts that <paramref name="str"/> doesn't contain <paramref name="unexpectedSubstring"/>.
    /// </summary>
    /// <returns>The asserted string, for chaining further assertions.</returns>
    public static string? ShouldNotContain(this string? str, string unexpectedSubstring, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Does.Not.Contains(unexpectedSubstring), message, actualExpression: strExpression, $"Does.Not.Contain({DescribeValuesExtensions.FormatValue(unexpectedSubstring)})");
        return str;
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> is equal to <paramref name="expectedValue"/>.
    /// </summary>
    /// <returns>The asserted string, for chaining further assertions.</returns>
    [return: NotNullIfNotNull(nameof(expectedValue))]
    public static string? ShouldBe(this string? str, string? expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Is.EqualTo(expectedValue), message, actualExpression: strExpression, constraintExpression: $"Is.EqualTo({DescribeValuesExtensions.FormatValue(expectedValue)})");
        return str;
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> is equal to <paramref name="expectedValue"/> using <paramref name="comparison"/>.
    /// </summary>
    /// <returns>The asserted string, for chaining further assertions.</returns>
    [return: NotNullIfNotNull(nameof(expectedValue))]
    public static string? ShouldBe(this string? str, string? expectedValue, StringComparison comparison, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        var comparer = GetComparer(comparison);
        Assert.That(str, Is.EqualTo(expectedValue).Using(comparer), message, actualExpression: strExpression, constraintExpression: $"Is.EqualTo({DescribeValuesExtensions.FormatValue(expectedValue)}).Using({nameof(StringComparison)}.{comparison})");
        return str;
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> isn't equal to <paramref name="expectedValue"/>.
    /// </summary>
    /// <returns>The asserted string, for chaining further assertions.</returns>
    public static string? ShouldNotBe(this string? str, string? expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        Assert.That(str, Is.Not.EqualTo(expectedValue), message, actualExpression: strExpression, constraintExpression: $"Is.Not.EqualTo({DescribeValuesExtensions.FormatValue(expectedValue)})");
        return str;
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> isn't equal to <paramref name="expectedValue"/> using <paramref name="comparison"/>.
    /// </summary>
    /// <returns>The asserted string, for chaining further assertions.</returns>
    public static string? ShouldNotBe(this string? str, string? expectedValue, StringComparison comparison, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        var comparer = GetComparer(comparison);
        Assert.That(str, Is.Not.EqualTo(expectedValue).Using(comparer), message, actualExpression: strExpression, constraintExpression: $"Is.Not.EqualTo({DescribeValuesExtensions.FormatValue(expectedValue)}).Using({nameof(StringComparison)}.{comparison})");
        return str;
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> matches the regular expression <paramref name="pattern"/>. Fails for <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// The pattern can match any part of the string; use <c>^</c> and <c>$</c> to match the whole string.
    /// </remarks>
    /// <returns>The asserted string as non-nullable, for chaining further assertions.</returns>
    public static string ShouldMatch(this string? str, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        str.ShouldNotBeNull(message, strExpression);
        Assert.That(str, Does.Match(pattern), message, actualExpression: strExpression, constraintExpression: $"Does.Match({DescribeValuesExtensions.FormatValue(pattern)})");
        return str;
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> matches <paramref name="regex"/>. Fails for <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// The regular expression can match any part of the string; use <c>^</c> and <c>$</c> to match the whole string.
    /// </remarks>
    /// <returns>The asserted string as non-nullable, for chaining further assertions.</returns>
    public static string ShouldMatch(this string? str, Regex regex, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "", [CallerArgumentExpression(nameof(regex))] string regexExpression = "")
    {
        str.ShouldNotBeNull(message, strExpression);
        Assert.That(str, Does.Match(regex), message, actualExpression: strExpression, constraintExpression: $"Does.Match({regexExpression})");
        return str;
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> doesn't match the regular expression <paramref name="pattern"/>. Passes for <see langword="null"/>.
    /// </summary>
    /// <returns>The asserted string, for chaining further assertions.</returns>
    public static string? ShouldNotMatch(this string? str, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "")
    {
        if (str != null)
        {
            Assert.That(str, Does.Not.Match(pattern), message, actualExpression: strExpression, constraintExpression: $"Does.Not.Match({DescribeValuesExtensions.FormatValue(pattern)})");
        }

        return str;
    }

    /// <summary>
    /// Asserts that <paramref name="str"/> doesn't match <paramref name="regex"/>. Passes for <see langword="null"/>.
    /// </summary>
    /// <returns>The asserted string, for chaining further assertions.</returns>
    public static string? ShouldNotMatch(this string? str, Regex regex, NUnitString message = default, [CallerArgumentExpression(nameof(str))] string strExpression = "", [CallerArgumentExpression(nameof(regex))] string regexExpression = "")
    {
        if (str != null)
        {
            Assert.That(str, Does.Not.Match(regex), message, actualExpression: strExpression, constraintExpression: $"Does.Not.Match({regexExpression})");
        }

        return str;
    }

    private static StringComparer GetComparer(StringComparison comparison)
    {
        return StringComparer.FromComparison(comparison);
    }
}