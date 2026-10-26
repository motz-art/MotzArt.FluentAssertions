using NUnit.Framework;
using System.Runtime.CompilerServices;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace MotzArt.FluentAssertions;

public static class DateTimeAssertions
{
    public static DateTime ShouldBeBefore(this DateTime value, DateTime expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.LessThan(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.LessThan({expectedExpression})");
        return value;
    }

    public static DateTime ShouldBeOnOrBefore(this DateTime value, DateTime expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.LessThanOrEqualTo(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.LessThanOrEqualTo({expectedExpression})");
        return value;
    }

    public static DateTime ShouldBeAfter(this DateTime value, DateTime expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.GreaterThan(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.GreaterThan({expectedExpression})");
        return value;
    }

    public static DateTime ShouldBeOnOrAfter(this DateTime value, DateTime expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.GreaterThanOrEqualTo(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.GreaterThanOrEqualTo({expectedExpression})");
        return value;
    }

    public static DateTime ShouldBeCloseTo(this DateTime value, DateTime expected, TimeSpan tolerance, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "", [CallerArgumentExpression(nameof(tolerance))] string toleranceExpression = "")
    {
        Assert.That(value, Is.EqualTo(expected).Within(tolerance), message, actualExpression: valueExpression, constraintExpression: $"Is.EqualTo({expectedExpression}).Within({toleranceExpression})");
        return value;
    }

    public static DateTime ShouldNotBeCloseTo(this DateTime value, DateTime unexpected, TimeSpan tolerance, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(unexpected))] string unexpectedExpression = "", [CallerArgumentExpression(nameof(tolerance))] string toleranceExpression = "")
    {
        Assert.That(value, Is.Not.EqualTo(unexpected).Within(tolerance), message, actualExpression: valueExpression, constraintExpression: $"Is.Not.EqualTo({unexpectedExpression}).Within({toleranceExpression})");
        return value;
    }

    public static DateTimeOffset ShouldBeBefore(this DateTimeOffset value, DateTimeOffset expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.LessThan(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.LessThan({expectedExpression})");
        return value;
    }

    public static DateTimeOffset ShouldBeOnOrBefore(this DateTimeOffset value, DateTimeOffset expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.LessThanOrEqualTo(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.LessThanOrEqualTo({expectedExpression})");
        return value;
    }

    public static DateTimeOffset ShouldBeAfter(this DateTimeOffset value, DateTimeOffset expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.GreaterThan(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.GreaterThan({expectedExpression})");
        return value;
    }

    public static DateTimeOffset ShouldBeOnOrAfter(this DateTimeOffset value, DateTimeOffset expected, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "")
    {
        Assert.That(value, Is.GreaterThanOrEqualTo(expected), message, actualExpression: valueExpression, constraintExpression: $"Is.GreaterThanOrEqualTo({expectedExpression})");
        return value;
    }

    public static DateTimeOffset ShouldBeCloseTo(this DateTimeOffset value, DateTimeOffset expected, TimeSpan tolerance, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(expected))] string expectedExpression = "", [CallerArgumentExpression(nameof(tolerance))] string toleranceExpression = "")
    {
        Assert.That(value, Is.EqualTo(expected).Within(tolerance), message, actualExpression: valueExpression, constraintExpression: $"Is.EqualTo({expectedExpression}).Within({toleranceExpression})");
        return value;
    }

    public static DateTimeOffset ShouldNotBeCloseTo(this DateTimeOffset value, DateTimeOffset unexpected, TimeSpan tolerance, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "", [CallerArgumentExpression(nameof(unexpected))] string unexpectedExpression = "", [CallerArgumentExpression(nameof(tolerance))] string toleranceExpression = "")
    {
        Assert.That(value, Is.Not.EqualTo(unexpected).Within(tolerance), message, actualExpression: valueExpression, constraintExpression: $"Is.Not.EqualTo({unexpectedExpression}).Within({toleranceExpression})");
        return value;
    }
}
