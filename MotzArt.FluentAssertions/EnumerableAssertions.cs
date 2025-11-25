using NUnit.Framework;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MotzArt.FluentAssertions;

public static class EnumerableAssertions
{
    [return: NotNull]
    public static T ShouldBeEmpty<T>(
        [NotNull] this T? actual,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>") where T : IEnumerable
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);
        Assert.That(actual, Is.Empty, message, actualExpression);
        return actual;
    }

    [return: NotNullIfNotNull(nameof(actual))]
    public static T? ShouldBeNullOrEmpty<T>(
        this T? actual,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>") where T : IEnumerable
    {
        Assert.That(actual, Is.Null.Or.Empty, message, actualExpression);
        return actual;
    }

    [return: NotNull]
    public static T ShouldNotBeNullOrEmpty<T>(
        [NotNull] this T? actual,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>") where T : IEnumerable
    {
        return ShouldNotBeEmpty(actual, message, actualExpression);
    }

    [return: NotNull]
    public static T ShouldNotBeEmpty<T>(
        [NotNull] this T? source,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(source))] string actualExpression = "<some-value>") where T : IEnumerable
    {
        Assert.That(source, Is.Not.Null.And.Not.Empty, message, actualExpression, constraintExpression: "Is.Not.Empty");
        return source ?? throw new InvalidOperationException("UPS. This should not happen because of assert above.");
    }
    
    [return: NotNull]
    public static T ShouldHaveCount<T>(
        [NotNull] this T? actual, 
        int count,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>") where T : IEnumerable
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);
        Assert.That(actual, Has.Exactly(count).Items, message, actualExpression, constraintExpression: $"Has.Exactly({count}).Items");
        return actual;
    }
    
    public static T ShouldHasSingle<T>([NotNull] this IReadOnlyList<T>? source,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(source))]
        string actualExpression = "<some-value>")
    {
        source.ShouldNotBeNull(valueExpression: actualExpression);
        Assert.That(source, Has.Exactly(1).Items, message, actualExpression);
        return source[0];
    }

    public static T ShouldHasSingle<T>(
        [NotNull] this IEnumerable<T>? actual,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>")
    {
        using var enumerator = actual.ShouldNotBeNull(valueExpression: actualExpression).GetEnumerator();

        if (!enumerator.MoveNext())
        {
            Assert.Fail($"""
                            Assert.That({actualExpression}, Has.Exactly(1).Items)
                            Expected: exactly one item
                            But was:  no matching items
                          
                          """);
        }

        var result = enumerator.Current;

        if (enumerator.MoveNext())
        {
            Assert.Fail($"""
                           Assert.That({actualExpression}, Has.Exactly(1).Items)
                           Expected: exactly one item
                           But was:  more then one matching items < {result}, {enumerator.Current} >
                         
                         """);
        }

        return result;
    }

    public static T ShouldHasSingle<T>(
        [NotNull] this IEnumerable<T>? source,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(source))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "<predicate-not-specified>")
    {
        using var enumerator = source.ShouldNotBeNull(valueExpression: actualExpression).Where(predicate).GetEnumerator();

        if (!enumerator.MoveNext())
        {
            Assert.Fail($"""
                           Assert.That({actualExpression}, Has.Exactly(1).Item.Matching({predicateExpression}))
                           Expected: exactly one item
                           But was:  no matching items

                         """);
        }

        var result = enumerator.Current;

        if (enumerator.MoveNext())
        {
            Assert.Fail($"""
                           Assert.That({actualExpression}, Has.Exactly(1).Item.Matching({predicateExpression}))
                           Expected: exactly one item
                           But was:  more then one matching items < {result}, {enumerator.Current} >

                         """);
        }

        return result;
    }

    public static void ShouldContain<T>([NotNull] this IEnumerable<T>? actual,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(actual))]
        string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = "<predicate-not-specified>")
    {
        using var enumerator = actual.ShouldNotBeNull(valueExpression: actualExpression).Where(predicate).GetEnumerator();

        if (!enumerator.MoveNext())
        {
            Assert.Fail($"""
                           Assert.That({actualExpression}, Has.Items.Matching({predicateExpression}))
                           Expected: has matching items
                           But was:  no matching items

                         """);
        }
    }

    public static void ShouldNotContain<T>([NotNull] this IEnumerable<T>? actual,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(actual))]
        string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = "<predicate-not-specified>")
    {
        using var enumerator = actual.ShouldNotBeNull(valueExpression: actualExpression).Where(predicate).GetEnumerator();

        if (enumerator.MoveNext())
        {
            Assert.Fail($"""
                           Assert.That({actualExpression}, Has.No.Items.Matching({predicateExpression}))
                           Expected: has no matching items
                           But was:  matching item < {enumerator.Current} >

                         """);
        }
    }
}