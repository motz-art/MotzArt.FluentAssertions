using MotzArt.FluentAssertions.Helpers;
using NUnit.Framework;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

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

    public static T ShouldHaveSingle<T>([NotNull] this IReadOnlyList<T>? source,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(source))]
        string actualExpression = "<some-value>")
    {
        source.ShouldNotBeNull(valueExpression: actualExpression);
        Assert.That(source, Has.Exactly(1).Items, message, actualExpression);
        return source[0];
    }

    public static T ShouldHaveSingle<T>(
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

    public static T ShouldHaveSingle<T>(
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

    [return: NotNull]
    public static T ShouldBeSequenceEqualTo<T, TItem>(
        [NotNull] this T? actual,
        IEnumerable<TItem> expected,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string expectedExpression = "<expected-value>") where T : IEnumerable<TItem>
    {
        return AssertSequenceEqual<T, TItem, TItem>(actual, expected, EqualityComparer<TItem>.Default.Equals, message, actualExpression, expectedExpression,
            constraintExpression: $"Is.EqualTo({expectedExpression})");
    }

    // Fallback for expected values without an item type (e.g. empty collection expression): the item type can't be inferred from the constraint of the overload above.
    [return: NotNull]
    public static IEnumerable<T> ShouldBeSequenceEqualTo<T>(
        [NotNull] this IEnumerable<T>? actual,
        IEnumerable<T> expected,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string expectedExpression = "<expected-value>")
    {
        return AssertSequenceEqual<IEnumerable<T>, T, T>(actual, expected, EqualityComparer<T>.Default.Equals, message, actualExpression, expectedExpression,
            constraintExpression: $"Is.EqualTo({expectedExpression})");
    }

    [return: NotNull]
    public static T ShouldBeSequenceEqualTo<T, TItem>(
        [NotNull] this T? actual,
        IEnumerable<TItem> expected,
        IEqualityComparer<TItem> comparer,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string expectedExpression = "<expected-value>",
        [CallerArgumentExpression(nameof(comparer))] string comparerExpression = "<comparer>") where T : IEnumerable<TItem>
    {
        ArgumentNullException.ThrowIfNull(comparer);

        return AssertSequenceEqual<T, TItem, TItem>(actual, expected, comparer.Equals, message, actualExpression, expectedExpression,
            constraintExpression: $"Is.EqualTo({expectedExpression}).Using({comparerExpression})");
    }

    [return: NotNull]
    public static T ShouldBeSequenceEqualTo<T, TActual, TExpected>(
        [NotNull] this T? actual,
        IEnumerable<TExpected> expected,
        Func<TActual, TExpected, bool> itemsAreEqual,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string expectedExpression = "<expected-value>",
        [CallerArgumentExpression(nameof(itemsAreEqual))] string predicateExpression = "<predicate>") where T : IEnumerable<TActual>
    {
        return AssertSequenceEqual(actual, expected, itemsAreEqual, message, actualExpression, expectedExpression,
            constraintExpression: $"Is.EqualTo({expectedExpression}).Using({predicateExpression})");
    }

    // Fallback for lambdas with implicitly typed parameters: the item type can't be inferred from the constraint of the overload above.
    [return: NotNull]
    public static IEnumerable<TActual> ShouldBeSequenceEqualTo<TActual, TExpected>(
        [NotNull] this IEnumerable<TActual>? actual,
        IEnumerable<TExpected> expected,
        Func<TActual, TExpected, bool> itemsAreEqual,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string expectedExpression = "<expected-value>",
        [CallerArgumentExpression(nameof(itemsAreEqual))] string predicateExpression = "<predicate>")
    {
        return AssertSequenceEqual(actual, expected, itemsAreEqual, message, actualExpression, expectedExpression,
            constraintExpression: $"Is.EqualTo({expectedExpression}).Using({predicateExpression})");
    }

    private static T AssertSequenceEqual<T, TActual, TExpected>(
        [NotNull] T? actual,
        IEnumerable<TExpected> expected,
        Func<TActual, TExpected, bool> itemsAreEqual,
        NUnitString message,
        string actualExpression,
        string expectedExpression,
        string constraintExpression) where T : IEnumerable<TActual>
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);
        ArgumentNullException.ThrowIfNull(expected, expectedExpression);
        ArgumentNullException.ThrowIfNull(itemsAreEqual);

        var actualItems = actual.ToArray();
        var expectedItems = expected.ToArray();

        var length = Math.Min(actualItems.Length, expectedItems.Length);
        var diffIndex = 0;

        while (diffIndex < length && itemsAreEqual(actualItems[diffIndex], expectedItems[diffIndex]))
        {
            diffIndex++;
        }

        if (diffIndex == length && actualItems.Length == expectedItems.Length)
        {
            return actual;
        }

        var writer = new MessageWriter();
        writer.WriteMessageIfNotEmpty(message);
        writer.WriteAssertThat(actualExpression, constraintExpression);

        if (actualItems.Length != expectedItems.Length)
        {
            writer.Write($"Expected sequence length {expectedItems.Length} but was {actualItems.Length}.");
        }
        else
        {
            writer.Write($"Sequence lengths are both {expectedItems.Length}.");
        }

        writer.WriteLine($" Sequences differ at index {diffIndex}.");
        writer.WriteExpected(((ReadOnlySpan<TExpected>)expectedItems).DescribeValue(skip: diffIndex));
        writer.WriteActual(((ReadOnlySpan<TActual>)actualItems).DescribeValue(skip: diffIndex));

        writer.ReportFailure();
        return actual;
    }

    // Each item of one set must be present in the other one and vice versa. Order and duplicates are ignored.
    // Sets are checked using their own Contains (and so their own comparer), other sequences are converted to HashSet<T> with the default comparer.
    [return: NotNull]
    public static T ShouldBeSetEqualTo<T, TItem>(
        [NotNull] this T? actual,
        IEnumerable<TItem> expected,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string expectedExpression = "<expected-value>") where T : IEnumerable<TItem>
    {
        return AssertSetEqual<T, TItem>(actual, expected, comparer: null, message, actualExpression, expectedExpression,
            constraintExpression: $"Is.SetEqualTo({expectedExpression})");
    }

    // Fallback for expected values without an item type (e.g. empty collection expression): the item type can't be inferred from the constraint of the overload above.
    [return: NotNull]
    public static IEnumerable<T> ShouldBeSetEqualTo<T>(
        [NotNull] this IEnumerable<T>? actual,
        IEnumerable<T> expected,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string expectedExpression = "<expected-value>")
    {
        return AssertSetEqual<IEnumerable<T>, T>(actual, expected, comparer: null, message, actualExpression, expectedExpression,
            constraintExpression: $"Is.SetEqualTo({expectedExpression})");
    }

    // The comparer is used for both actual and expected items, overriding comparers of the sets.
    [return: NotNull]
    public static T ShouldBeSetEqualTo<T, TItem>(
        [NotNull] this T? actual,
        IEnumerable<TItem> expected,
        IEqualityComparer<TItem> comparer,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string expectedExpression = "<expected-value>",
        [CallerArgumentExpression(nameof(comparer))] string comparerExpression = "<comparer>") where T : IEnumerable<TItem>
    {
        ArgumentNullException.ThrowIfNull(comparer);

        return AssertSetEqual<T, TItem>(actual, expected, comparer, message, actualExpression, expectedExpression,
            constraintExpression: $"Is.SetEqualTo({expectedExpression}).Using({comparerExpression})");
    }

    private static T AssertSetEqual<T, TItem>(
        [NotNull] T? actual,
        IEnumerable<TItem> expected,
        IEqualityComparer<TItem>? comparer,
        NUnitString message,
        string actualExpression,
        string expectedExpression,
        string constraintExpression) where T : IEnumerable<TItem>
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);
        ArgumentNullException.ThrowIfNull(expected, expectedExpression);

        var actualSet = AsSet(actual, comparer);
        var expectedSet = AsSet(expected, comparer);

        var missingItems = expectedSet.Where(x => !actualSet.Contains(x)).ToArray();
        var extraItems = actualSet.Where(x => !expectedSet.Contains(x)).ToArray();

        if (missingItems.Length == 0 && extraItems.Length == 0)
        {
            return actual;
        }

        var writer = new MessageWriter();
        writer.WriteMessageIfNotEmpty(message);
        writer.WriteAssertThat(actualExpression, constraintExpression);

        if (missingItems.Length > 0)
        {
            writer.WriteLine($"Missing items: {DescribeItems(missingItems)}");
        }

        if (extraItems.Length > 0)
        {
            writer.WriteLine($"Extra items: {DescribeItems(extraItems)}");
        }

        writer.WriteExpected(DescribeItems(expectedSet.ToArray()));
        writer.WriteActual(DescribeItems(actualSet.ToArray()));

        writer.ReportFailure();
        return actual;
    }

    private static IReadOnlySet<TItem> AsSet<TItem>(IEnumerable<TItem> items, IEqualityComparer<TItem>? comparer)
    {
        if (comparer == null && items is IReadOnlySet<TItem> set)
        {
            return set;
        }

        return new HashSet<TItem>(items, comparer);
    }

    private static string DescribeItems<TItem>(TItem[] items)
    {
        return items.Length == 0 ? "<empty>" : ((ReadOnlySpan<TItem>)items).DescribeValue();
    }
}