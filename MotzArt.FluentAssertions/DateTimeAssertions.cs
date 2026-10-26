using NUnit.Framework;
using System.Runtime.CompilerServices;

namespace MotzArt.FluentAssertions;

/// <summary>
/// Assertions for <see cref="DateTime"/> and <see cref="DateTimeOffset"/> values.
/// </summary>
/// <remarks>
/// <see cref="DateTime"/> values are compared by their ticks, ignoring <see cref="DateTime.Kind"/>.
/// <see cref="DateTimeOffset"/> values are compared as points in time, so values with different offsets can be equal.
/// </remarks>
public static class DateTimeAssertions
{
    /// <summary>
    /// Asserts that <paramref name="value"/> is earlier than <paramref name="expected"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static DateTime ShouldBeBefore(this DateTime value, DateTime expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.LessThan(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.LessThan({expectedExpression})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is earlier than or equal to <paramref name="expected"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static DateTime ShouldBeOnOrBefore(this DateTime value, DateTime expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.LessThanOrEqualTo(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.LessThanOrEqualTo({expectedExpression})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is later than <paramref name="expected"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static DateTime ShouldBeAfter(this DateTime value, DateTime expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.GreaterThan(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.GreaterThan({expectedExpression})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is later than or equal to <paramref name="expected"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static DateTime ShouldBeOnOrAfter(this DateTime value, DateTime expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.GreaterThanOrEqualTo(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.GreaterThanOrEqualTo({expectedExpression})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> differs from <paramref name="expected"/> by no more than <paramref name="tolerance"/>, in either direction.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static DateTime ShouldBeCloseTo(this DateTime value, DateTime expected, TimeSpan tolerance, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "", [CallerArgumentExpression(nameof(tolerance))] string toleranceExpression = "")
    {
        Assert.That(value, Is.EqualTo(expected).Within(tolerance), message, actualExpression: valueExpression, constraintExpression: $"Is.EqualTo({expectedExpression}).Within({toleranceExpression})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> differs from <paramref name="unexpected"/> by more than <paramref name="tolerance"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static DateTime ShouldNotBeCloseTo(this DateTime value, DateTime unexpected, TimeSpan tolerance, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(unexpected))] string unexpectedExpression = "", [CallerArgumentExpression(nameof(tolerance))] string toleranceExpression = "")
    {
        Assert.That(value, Is.Not.EqualTo(unexpected).Within(tolerance), message, actualExpression: valueExpression, constraintExpression: $"Is.Not.EqualTo({unexpectedExpression}).Within({toleranceExpression})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is earlier than <paramref name="expected"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static DateTimeOffset ShouldBeBefore(this DateTimeOffset value, DateTimeOffset expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.LessThan(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.LessThan({expectedExpression})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is earlier than or equal to <paramref name="expected"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static DateTimeOffset ShouldBeOnOrBefore(this DateTimeOffset value, DateTimeOffset expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.LessThanOrEqualTo(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.LessThanOrEqualTo({expectedExpression})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is later than <paramref name="expected"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static DateTimeOffset ShouldBeAfter(this DateTimeOffset value, DateTimeOffset expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.GreaterThan(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.GreaterThan({expectedExpression})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is later than or equal to <paramref name="expected"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static DateTimeOffset ShouldBeOnOrAfter(this DateTimeOffset value, DateTimeOffset expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.GreaterThanOrEqualTo(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.GreaterThanOrEqualTo({expectedExpression})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> differs from <paramref name="expected"/> by no more than <paramref name="tolerance"/>, in either direction.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static DateTimeOffset ShouldBeCloseTo(this DateTimeOffset value, DateTimeOffset expected, TimeSpan tolerance, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "", [CallerArgumentExpression(nameof(tolerance))] string toleranceExpression = "")
    {
        Assert.That(value, Is.EqualTo(expected).Within(tolerance), message, actualExpression: valueExpression, constraintExpression: $"Is.EqualTo({expectedExpression}).Within({toleranceExpression})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> differs from <paramref name="unexpected"/> by more than <paramref name="tolerance"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static DateTimeOffset ShouldNotBeCloseTo(this DateTimeOffset value, DateTimeOffset unexpected, TimeSpan tolerance, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(unexpected))] string unexpectedExpression = "", [CallerArgumentExpression(nameof(tolerance))] string toleranceExpression = "")
    {
        Assert.That(value, Is.Not.EqualTo(unexpected).Within(tolerance), message, actualExpression: valueExpression, constraintExpression: $"Is.Not.EqualTo({unexpectedExpression}).Within({toleranceExpression})");
        return value;
    }
}
