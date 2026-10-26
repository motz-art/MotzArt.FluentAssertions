using JetBrains.Annotations;
using MotzArt.FluentAssertions.Helpers;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace MotzArt.FluentAssertions;

/// <summary>
/// Assertions for values of any type: equality, identity, nullness and type.
/// </summary>
/// <remarks>
/// Every assertion accepts an optional custom message that is written before the failure details.
/// Parameters marked with <see cref="System.Runtime.CompilerServices.CallerArgumentExpressionAttribute"/> are filled in
/// by the compiler with the asserted expression and shouldn't be passed explicitly.
/// </remarks>
public static class CommonAssertions
{
    /// <summary>
    /// The expression shown in failure messages when the compiler didn't provide the asserted expression.
    /// </summary>
    public const string ValueExpressionPlaceholder = "<Expression Not Provided>";

    /// <summary>
    /// Asserts that <paramref name="value"/> is equal to <paramref name="expectedValue"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    [return: NotNullIfNotNull(nameof(expectedValue))]
    public static T ShouldBe<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder)
    {
        Assert.That(value, Is.EqualTo(expectedValue), message, actualExpression: valueExpression);
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is equal to <paramref name="expectedValue"/> according to <paramref name="comparer"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    [return: NotNullIfNotNull(nameof(expectedValue))]
    public static T ShouldBe<T>(this T value, T expectedValue, IEqualityComparer<T> comparer, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder)
    {
        Assert.That(value, Is.EqualTo(expectedValue).Using<T>(comparer.Equals), message, actualExpression: valueExpression, "Is.EqualTo(expectedValue).Using(comparer)");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is the same instance as <paramref name="expectedValue"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    [return: NotNullIfNotNull(nameof(expectedValue))]
    public static T? ShouldBeSameAs<T>([NoEnumeration] this T? value, [NoEnumeration] T? expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder) where T : class
    {
        Assert.That(value, Is.SameAs(expectedValue), message, actualExpression: valueExpression);
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> isn't the same instance as <paramref name="expectedValue"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static T ShouldNotBeSameAs<T>([NoEnumeration] this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder) where T : class
    {
        Assert.That(value, Is.Not.SameAs(expectedValue), message, actualExpression: valueExpression);
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> isn't equal to <paramref name="expectedValue"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    public static T ShouldNotBe<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder)
    {
        Assert.That(value, Is.Not.EqualTo(expectedValue), message, actualExpression: valueExpression);
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> isn't equal to <paramref name="expectedValue"/> according to <paramref name="comparer"/>.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    [return: NotNullIfNotNull(nameof(expectedValue))]
    public static T ShouldNotBe<T>(this T value, T expectedValue, IEqualityComparer<T> comparer, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder)
    {
        Assert.That(value, Is.Not.EqualTo(expectedValue).Using<T>(comparer.Equals), message, actualExpression: valueExpression, "Is.Not.EqualTo(expectedValue).Using(comparer)");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="obj"/> isn't <see langword="null"/>.
    /// </summary>
    /// <returns>The asserted value as non-nullable, for chaining further assertions.</returns>
    [return: NotNull]
    public static T ShouldNotBeNull<T>([NotNull] this T? obj, NUnitString message = default, [CallerArgumentExpression(nameof(obj))] string valueExpression = ValueExpressionPlaceholder)
    {
        Assert.That(obj, Is.Not.Null, message, actualExpression: valueExpression);
        return obj ?? throw new InvalidOperationException("UPS! This should never happened because of assert above!");
    }

    /// <summary>
    /// Asserts that <paramref name="obj"/> is <see langword="null"/>.
    /// </summary>
    public static void ShouldBeNull<T>(this T? obj, NUnitString message = default, [CallerArgumentExpression(nameof(obj))] string objExpression = ValueExpressionPlaceholder)
    {
        Assert.That(obj, Is.Null, message, actualExpression: objExpression);
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> has the same property values as <paramref name="expectedValue"/>,
    /// even when they are different instances that don't override <see cref="object.Equals(object)"/>.
    /// </summary>
    public static void ShouldBeEquivalentTo<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder)
    {
        Assert.That(value, Is.EqualTo(expectedValue).UsingPropertiesComparer(), message, actualExpression: valueExpression);
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> doesn't have the same property values as <paramref name="expectedValue"/>.
    /// </summary>
    public static void ShouldNotBeEquivalentTo<T>(this T value, T expectedValue, NUnitString message = default, [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder)
    {
        Assert.That(value, Is.Not.EqualTo(expectedValue).UsingPropertiesComparer(), message, actualExpression: valueExpression);
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is an instance of <typeparamref name="T"/> or of a type derived from it.
    /// </summary>
    /// <returns>The asserted value cast to <typeparamref name="T"/>, for chaining further assertions.</returns>
    [return: NotNull]
    public static T ShouldBeOfType<T>([NotNull] this object? value, NUnitString message = default,
        [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder) where T : class
    {
        Assert.That(value, Is.InstanceOf<T>(), message, actualExpression: valueExpression, constraintExpression: $"Is.InstanceOf<{typeof(T).DescribeType()}>()");
        return (T)value.ShouldNotBeNull();
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is an instance of <paramref name="type"/> or of a type derived from it.
    /// </summary>
    /// <returns>The asserted value as non-nullable, for chaining further assertions.</returns>
    [return: NotNull]
    public static T ShouldBeOfType<T>(this T? value, Type type, NUnitString message = default,
        [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder) where T : class
    {
        Assert.That(value, Is.InstanceOf(type), message, actualExpression: valueExpression, constraintExpression: $"Is.InstanceOf({type.DescribeType()})");
        return value.ShouldNotBeNull();
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> isn't an instance of <typeparamref name="T"/> or of a type derived from it.
    /// </summary>
    /// <returns>The asserted value.</returns>
    [Obsolete("This method will be removed. Use ShouldNotBeOfType<T>(this object? value, Type type, NUnitString message = default) instead because it preserves type of original parameter.")]
    [return: NotNullIfNotNull(nameof(value))]
    public static object? ShouldNotBeOfType<T>(this object? value, NUnitString message = default,
        [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder) where T : class
    {
        Assert.That(value, Is.Not.InstanceOf<T>(), message, actualExpression: valueExpression, constraintExpression: $"Is.Not.InstanceOf<{typeof(T).DescribeType()}>()");
        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> isn't an instance of <paramref name="type"/> or of a type derived from it.
    /// </summary>
    /// <returns>The asserted value, for chaining further assertions.</returns>
    [return: NotNullIfNotNull(nameof(value))]
    public static T? ShouldNotBeOfType<T>(this T? value, Type type, NUnitString message = default,
        [CallerArgumentExpression(nameof(value))] string valueExpression = ValueExpressionPlaceholder) where T : class
    {
        Assert.That(value, Is.Not.InstanceOf(type), message, actualExpression: valueExpression, constraintExpression: $"Is.Not.InstanceOf({type.DescribeType()})");
        return value;
    }
}
