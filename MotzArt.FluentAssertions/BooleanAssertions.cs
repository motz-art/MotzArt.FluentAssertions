using NUnit.Framework;
using System.Runtime.CompilerServices;

namespace MotzArt.FluentAssertions;

public static class BooleanAssertions
{
    public static void ShouldBeTrue(this bool value, NUnitString message = default,
        [CallerArgumentExpression(nameof(value))] string valueExpression = "")
    {
        Assert.That(value, Is.True, message, actualExpression: valueExpression);
    }

    public static void ShouldBeFalse(this bool value, NUnitString message = default,
        [CallerArgumentExpression(nameof(value))] string valueExpression = "")
    {
        Assert.That(value, Is.False, message, actualExpression: valueExpression);
    }

    public static void ShouldBeTrue(this bool? value, NUnitString message = default,
        [CallerArgumentExpression(nameof(value))] string valueExpression = "")
    {
        Assert.That(value, Is.True, message, actualExpression: valueExpression);
    }

    public static void ShouldBeFalse(this bool? value, NUnitString message = default,
        [CallerArgumentExpression(nameof(value))] string valueExpression = "")
    {
        Assert.That(value, Is.False, message, actualExpression: valueExpression);
    }
}