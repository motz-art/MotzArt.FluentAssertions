using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using MotzArt.FluentAssertions.Helpers;
using NUnit.Framework;

namespace MotzArt.FluentAssertions;

public static class CommonAssertions
{
    public const string ValueExpressionPlaceholder = "<Expression Not Provided>";

    [return: NotNullIfNotNull(nameof(expectedValue))]
    public static T ShouldBe<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder)
    {
        Assert.That(value, Is.EqualTo(expectedValue), message, actualExpression: valueExpression);
        return value;
    }

    [return: NotNullIfNotNull(nameof(expectedValue))]
    public static T ShouldBe<T>(this T value, T expectedValue, IEqualityComparer<T> comparer, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder)
    {
        Assert.That(value, Is.EqualTo(expectedValue).Using<T>(comparer.Equals), message, actualExpression: valueExpression, "Is.EqualTo(expectedValue).Using(comparer)");
        return value;
    }

    [return: NotNullIfNotNull(nameof(expectedValue))]
    public static T? ShouldBeSameAs<T>([NoEnumeration] this T? value, [NoEnumeration] T? expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder) where T : class
    {
        Assert.That(value, Is.SameAs(expectedValue), message, actualExpression: valueExpression);
        return value;
    }

    public static T ShouldNotBeSameAs<T>([NoEnumeration] this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder) where T : class
    {
        Assert.That(value, Is.Not.SameAs(expectedValue), message, actualExpression: valueExpression);
        return value;
    }

    public static T ShouldNotBe<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder)
    {
        Assert.That(value, Is.Not.EqualTo(expectedValue), message, actualExpression: valueExpression);
        return value;
    }

    [return: NotNullIfNotNull(nameof(expectedValue))]
    public static T ShouldNotBe<T>(this T value, T expectedValue, IEqualityComparer<T> comparer, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder)
    {
        Assert.That(value, Is.Not.EqualTo(expectedValue).Using<T>(comparer.Equals), message, actualExpression: valueExpression, "Is.Not.EqualTo(expectedValue).Using(comparer)");
        return value;
    }

    [return: System.Diagnostics.CodeAnalysis.NotNull]
    public static T ShouldNotBeNull<T>([System.Diagnostics.CodeAnalysis.NotNull] this T? obj, NUnitString message = default, [CallerArgumentExpression(nameof(obj))] string valueExpression = ValueExpressionPlaceholder)
    {
        Assert.That(obj, Is.Not.Null, message, actualExpression: valueExpression);
        return obj ?? throw new InvalidOperationException("UPS! This should never happened because of assert above!");
    }

    public static void ShouldBeNull<T>(this T? obj, NUnitString message = default, [CallerArgumentExpression(nameof(obj))] string objExpression = ValueExpressionPlaceholder)
    {
        Assert.That(obj, Is.Null, message, actualExpression: objExpression);
    }
    
    public static void ShouldBeEquivalentTo<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder)
    {
        Assert.That(value, Is.EqualTo(expectedValue).UsingPropertiesComparer(), message, actualExpression: valueExpression);
    }

    public static void ShouldNotBeEquivalentTo<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder)
    {
        Assert.That(value, Is.Not.EqualTo(expectedValue).UsingPropertiesComparer(), message, actualExpression: valueExpression);
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static T ShouldBeOfType<T>(this object? value, NUnitString message = default,
        [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder) where T: class
    {
        Assert.That(value, Is.InstanceOf<T>(), message, actualExpression: valueExpression, constraintExpression: $"Is.InstanceOf<{typeof(T).DescribeType()}>()");
        return (T)value.ShouldNotBeNull();
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static object? ShouldNotBeOfType<T>(this object? value, NUnitString message = default,
        [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder) where T: class
    {
        Assert.That(value, Is.Not.InstanceOf<T>(), message, actualExpression: valueExpression, constraintExpression: $"Is.Not.InstanceOf<{typeof(T).DescribeType()}>()");
        return value;
    }
}
