using MotzArt.FluentAssertions.Helpers;
using NUnit.Framework;
using System.Runtime.CompilerServices;

namespace MotzArt.FluentAssertions;

/// <summary>
/// Comparison assertions for numbers and other <see cref="IComparable"/> values.
/// </summary>
public static class NumberAssertions
{
    /// <summary>
    /// Asserts that <paramref name="value"/> is greater than <paramref name="expectedValue"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static T ShouldBeGreaterThan<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.GreaterThan(expectedValue), message, actualExpression: valueExpression, constraintExpression: $"Is.GreaterThan({expectedValue})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is greater than or equal to <paramref name="expectedValue"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static T ShouldBeGreaterThanOrEqual<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.GreaterThanOrEqualTo(expectedValue), message, actualExpression: valueExpression, constraintExpression: $"Is.GreaterThanOrEqualTo({expectedValue})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is less than <paramref name="expectedValue"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static T ShouldBeLessThan<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.LessThan(expectedValue), message, actualExpression: valueExpression, constraintExpression: $"Is.LessThan({expectedValue})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is less than or equal to <paramref name="expectedValue"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static T ShouldBeLessThanOrEqual<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.LessThanOrEqualTo(expectedValue), message, actualExpression: valueExpression, constraintExpression: $"Is.LessThanOrEqualTo({expectedValue})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is between <paramref name="minValue"/> and <paramref name="maxValue"/>, both inclusive.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static T ShouldBeInRange<T>(this T value, T minValue, T maxValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.InRange(minValue, maxValue), message, actualExpression: valueExpression, constraintExpression: $"Is.InRange({minValue.DescribeValue()}, {maxValue.DescribeValue()})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is less than <paramref name="minValue"/> or greater than <paramref name="maxValue"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static T ShouldNotBeInRange<T>(this T value, T minValue, T maxValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.Not.InRange(minValue, maxValue), message, actualExpression: valueExpression, constraintExpression: $"Is.Not.InRange({minValue.DescribeValue()}, {maxValue.DescribeValue()})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> differs from <paramref name="expectedValue"/> by no more than <paramref name="tolerance"/>, in either direction.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static T ShouldBeApproximately<T>(this T value, T expectedValue, T tolerance, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : struct, IComparable
    {
        Assert.That(value, Is.EqualTo(expectedValue).Within(tolerance), actualExpression: valueExpression, constraintExpression: $"Is.EqualTo({expectedValue.DescribeValue()}).Within({tolerance.DescribeValue()})");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> differs from <paramref name="expectedValue"/> by more than <paramref name="tolerance"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static T ShouldNotBeApproximately<T>(this T value, T expectedValue, T tolerance, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : struct, IComparable
    {
        Assert.That(value, Is.Not.EqualTo(expectedValue).Within(tolerance), actualExpression: valueExpression, constraintExpression: $"Is.Not.EqualTo({expectedValue.DescribeValue()}).Within({tolerance.DescribeValue()})");
        return value;
    }
}