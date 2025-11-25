using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace MotzArt.FluentAssertions;

public static class NumberAssertions
{
    public static void ShouldBeGreaterThan<T>(this T value, int expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.GreaterThan(expectedValue), message, actualExpression: valueExpression, constraintExpression: $"Is.GreaterThan({expectedValue})");
    }

    public static void ShouldBeGreaterThanOrEqual<T>(this T value, int expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.GreaterThanOrEqualTo(expectedValue), message, actualExpression: valueExpression, constraintExpression: $"Is.GreaterThanOrEqualTo({expectedValue})");
    }

    public static void ShouldBeLessThan<T>(this T value, int expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.LessThan(expectedValue), message, actualExpression: valueExpression, constraintExpression: $"Is.LessThan({expectedValue})");
    }

    public static void ShouldBeLessThanOrEqual<T>(this T value, int expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.LessThanOrEqualTo(expectedValue), message, actualExpression: valueExpression, constraintExpression: $"Is.LessThanOrEqualTo({expectedValue})");
    }
}