using NUnit.Framework;
using System.Runtime.CompilerServices;

namespace MotzArt.FluentAssertions;

/// <summary>
/// Assertions for <see cref="bool"/> values.
/// </summary>
public static class BooleanAssertions
{
    /// <summary>
    /// Asserts that <paramref name="value"/> is <see langword="true"/>.
    /// </summary>
    public static void ShouldBeTrue(this bool value, NUnitString message = default,
        [CallerArgumentExpression(nameof(value))] string valueExpression = "")
    {
        Assert.That(value, Is.True, message, actualExpression: valueExpression);
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is <see langword="false"/>.
    /// </summary>
    public static void ShouldBeFalse(this bool value, NUnitString message = default,
        [CallerArgumentExpression(nameof(value))] string valueExpression = "")
    {
        Assert.That(value, Is.False, message, actualExpression: valueExpression);
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is <see langword="true"/>. Fails for <see langword="null"/>.
    /// </summary>
    public static void ShouldBeTrue(this bool? value, NUnitString message = default,
        [CallerArgumentExpression(nameof(value))] string valueExpression = "")
    {
        Assert.That(value, Is.True, message, actualExpression: valueExpression);
    }

    /// <summary>
    /// Asserts that <paramref name="value"/> is <see langword="false"/>. Fails for <see langword="null"/>.
    /// </summary>
    public static void ShouldBeFalse(this bool? value, NUnitString message = default,
        [CallerArgumentExpression(nameof(value))] string valueExpression = "")
    {
        Assert.That(value, Is.False, message, actualExpression: valueExpression);
    }
}