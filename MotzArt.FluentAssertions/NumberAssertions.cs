using MotzArt.FluentAssertions.Helpers;
using NUnit.Framework;
using System.Runtime.CompilerServices;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace MotzArt.FluentAssertions;

public static class NumberAssertions
{
    public static T ShouldBeGreaterThan<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.GreaterThan(expectedValue), message, actualExpression: valueExpression, constraintExpression: $"Is.GreaterThan({expectedValue})");
        return value;
    }

    public static T ShouldBeGreaterThanOrEqual<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.GreaterThanOrEqualTo(expectedValue), message, actualExpression: valueExpression, constraintExpression: $"Is.GreaterThanOrEqualTo({expectedValue})");
        return value;
    }

    public static T ShouldBeLessThan<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.LessThan(expectedValue), message, actualExpression: valueExpression, constraintExpression: $"Is.LessThan({expectedValue})");
        return value;
    }

    public static T ShouldBeLessThanOrEqual<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.LessThanOrEqualTo(expectedValue), message, actualExpression: valueExpression, constraintExpression: $"Is.LessThanOrEqualTo({expectedValue})");
        return value;
    }

    public static T ShouldBeInRange<T>(this T value, T minValue, T maxValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.InRange(minValue, maxValue), message, actualExpression: valueExpression, constraintExpression: $"Is.InRange({minValue.DescribeValue()}, {maxValue.DescribeValue()})");
        return value;
    }

    public static T ShouldNotBeInRange<T>(this T value, T minValue, T maxValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : IComparable
    {
        Assert.That(value, Is.Not.InRange(minValue, maxValue), message, actualExpression: valueExpression, constraintExpression: $"Is.Not.InRange({minValue.DescribeValue()}, {maxValue.DescribeValue()})");
        return value;
    }

    public static T ShouldBeApproximately<T>(this T value, T expectedValue, T tolerance, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : struct, IComparable
    {
        Assert.That(value, Is.EqualTo(expectedValue).Within(tolerance), actualExpression: valueExpression, constraintExpression: $"Is.EqualTo({expectedValue.DescribeValue()}).Within({tolerance.DescribeValue()})");
        return value;
    }

    public static T ShouldNotBeApproximately<T>(this T value, T expectedValue, T tolerance, [CallerArgumentExpression(nameof(value))] string valueExpression = "") where T : struct, IComparable
    {
        Assert.That(value, Is.Not.EqualTo(expectedValue).Within(tolerance), actualExpression: valueExpression, constraintExpression: $"Is.Not.EqualTo({expectedValue.DescribeValue()}).Within({tolerance.DescribeValue()})");
        return value;
    }
}