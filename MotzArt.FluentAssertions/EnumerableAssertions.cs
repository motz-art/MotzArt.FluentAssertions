using MotzArt.FluentAssertions.Helpers;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static MotzArt.FluentAssertions.Helpers.MessageUtils;

namespace MotzArt.FluentAssertions;

/// <summary>
/// Assertions for collections and other sequences.
/// </summary>
/// <remarks>
/// Unless stated otherwise, assertions fail for a <see langword="null"/> sequence.
/// Most overloads return the asserted collection with its own type (for example <see cref="List{T}"/>)
/// for chaining. When the item type can't be inferred, e.g. for an empty collection expression or a lambda with implicitly typed parameters,
/// the overload that returns <see cref="IEnumerable{T}"/> is used instead.
/// </remarks>
public static class EnumerableAssertions
{
    private const int MaxReportedItemFailures = 10;

    /// <summary>
    /// Asserts that <paramref name="actual"/> has no items.
    /// </summary>
    /// <returns>The asserted sequence, for chaining further assertions.</returns>
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

    /// <summary>
    /// Asserts that <paramref name="actual"/> is <see langword="null"/> or has no items.
    /// </summary>
    /// <returns>The asserted sequence, for chaining further assertions.</returns>
    [return: NotNullIfNotNull(nameof(actual))]
    public static T? ShouldBeNullOrEmpty<T>(
        this T? actual,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>") where T : IEnumerable
    {
        Assert.That(actual, Is.Null.Or.Empty, message, actualExpression);
        return actual;
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> has at least one item.
    /// </summary>
    /// <returns>The asserted sequence, for chaining further assertions.</returns>
    [return: NotNull]
    public static T ShouldNotBeNullOrEmpty<T>(
        [NotNull] this T? actual,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>") where T : IEnumerable
    {
        return ShouldNotBeEmpty(actual, message, actualExpression);
    }

    /// <summary>
    /// Asserts that <paramref name="source"/> has at least one item.
    /// </summary>
    /// <returns>The asserted sequence, for chaining further assertions.</returns>
    [return: NotNull]
    public static T ShouldNotBeEmpty<T>(
        [NotNull] this T? source,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(source))] string actualExpression = "<some-value>") where T : IEnumerable
    {
        Assert.That(source, Is.Not.Null.And.Not.Empty, message, actualExpression, constraintExpression: "Is.Not.Empty");
        return source ?? throw new InvalidOperationException("UPS. This should not happen because of assert above.");
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> has exactly <paramref name="count"/> items.
    /// </summary>
    /// <returns>The asserted sequence, for chaining further assertions.</returns>
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

    /// <summary>
    /// Asserts that <paramref name="source"/> has exactly one item.
    /// </summary>
    /// <returns>The single item, for chaining assertions on it.</returns>
    public static T ShouldHaveSingle<T>([NotNull] this IReadOnlyList<T>? source,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(source))]
        string actualExpression = "<some-value>")
    {
        source.ShouldNotBeNull(valueExpression: actualExpression);
        Assert.That(source, Has.Exactly(1).Items, message, actualExpression);
        return source[0];
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> has exactly one item.
    /// </summary>
    /// <returns>The single item, for chaining assertions on it.</returns>
    public static T ShouldHaveSingle<T>(
        [NotNull] this IEnumerable<T>? actual,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>")
    {
        using var enumerator = actual.ShouldNotBeNull(valueExpression: actualExpression).GetEnumerator();

        const string constraintExpression = "Has.Exactly(1).Items";

        if (!enumerator.MoveNext())
        {
            MessageWriter.Report(message, actualExpression, constraintExpression, "exactly one item", "no matching items");
        }

        var result = enumerator.Current;

        if (enumerator.MoveNext())
        {
            MessageWriter.Report(message, actualExpression, constraintExpression, "exactly one item", $"more than one matching items < {result}, {enumerator.Current} >");
        }

        return result;
    }

    /// <summary>
    /// Asserts that exactly one item of <paramref name="source"/> matches <paramref name="predicate"/>.
    /// </summary>
    /// <returns>The single matching item, for chaining assertions on it.</returns>
    public static T ShouldHaveSingle<T>(
        [NotNull] this IEnumerable<T>? source,
        Func<T, bool> predicate,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(source))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "<predicate-not-specified>")
    {
        using var enumerator = source.ShouldNotBeNull(valueExpression: actualExpression).Where(predicate).GetEnumerator();

        var constraintExpression = $"Has.Exactly(1).Item.Matching({predicateExpression})";

        if (!enumerator.MoveNext())
        {
            MessageWriter.Report(message, actualExpression, constraintExpression, "exactly one item", "no matching items");
        }

        var result = enumerator.Current;

        if (enumerator.MoveNext())
        {
            MessageWriter.Report(message, actualExpression, constraintExpression, "exactly one item", $"more than one matching items < {result}, {enumerator.Current} >");
        }

        return result;
    }

    /// <summary>
    /// Asserts that at least one item of <paramref name="actual"/> matches <paramref name="predicate"/>.
    /// </summary>
    /// <returns>The asserted sequence, for chaining further assertions.</returns>
    [return: NotNull]
    public static IEnumerable<T> ShouldContain<T>(
        [NotNull] this IEnumerable<T>? actual,
        Func<T, bool> predicate,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "<predicate-not-specified>")
    {
        if (!actual.ShouldNotBeNull(valueExpression: actualExpression).Any(predicate))
        {
            MessageWriter.Report(message, actualExpression, $"Has.Items.Matching({predicateExpression})", "has matching items", "no matching items");
        }

        return actual;
    }

    /// <summary>
    /// Asserts that no item of <paramref name="actual"/> matches <paramref name="predicate"/>.
    /// </summary>
    /// <returns>The asserted sequence, for chaining further assertions.</returns>
    [return: NotNull]
    public static IEnumerable<T> ShouldNotContain<T>(
        [NotNull] this IEnumerable<T>? actual,
        Func<T, bool> predicate,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "<predicate-not-specified>")
    {
        using var enumerator = actual.ShouldNotBeNull(valueExpression: actualExpression).Where(predicate).GetEnumerator();

        if (enumerator.MoveNext())
        {
            MessageWriter.Report(message, actualExpression, $"Has.No.Items.Matching({predicateExpression})", "has no matching items", $"matching item < {enumerator.Current} >");
        }

        return actual;
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> contains an item equal to <paramref name="expected"/>.
    /// </summary>
    /// <returns>The asserted collection, for chaining further assertions.</returns>
    [return: NotNull]
    public static T ShouldContain<T, TItem>(
        [NotNull] this T? actual,
        TItem expected,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string expectedExpression = "<expected-value>") where T : IEnumerable<TItem>
    {
        return AssertContains(actual, expected, EqualityComparer<TItem>.Default, message, actualExpression,
            constraintExpression: $"Has.Member({expectedExpression})");
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> contains an item equal to <paramref name="expected"/>.
    /// </summary>
    /// <returns>The asserted sequence, for chaining further assertions.</returns>
    [return: NotNull]
    public static IEnumerable<T> ShouldContain<T>(
        [NotNull] this IEnumerable<T>? actual,
        T expected,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string expectedExpression = "<expected-value>")
    {
        return AssertContains(actual, expected, EqualityComparer<T>.Default, message, actualExpression,
            constraintExpression: $"Has.Member({expectedExpression})");
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> contains an item equal to <paramref name="expected"/> according to <paramref name="comparer"/>.
    /// </summary>
    /// <returns>The asserted collection, for chaining further assertions.</returns>
    [return: NotNull]
    public static T ShouldContain<T, TItem>(
        [NotNull] this T? actual,
        TItem expected,
        IEqualityComparer<TItem> comparer,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string expectedExpression = "<expected-value>",
        [CallerArgumentExpression(nameof(comparer))] string comparerExpression = "<comparer>") where T : IEnumerable<TItem>
    {
        ArgumentNullException.ThrowIfNull(comparer);

        return AssertContains(actual, expected, comparer, message, actualExpression,
            constraintExpression: $"Has.Member({expectedExpression}).Using({comparerExpression})");
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> doesn't contain an item equal to <paramref name="unexpected"/>.
    /// </summary>
    /// <returns>The asserted collection, for chaining further assertions.</returns>
    [return: NotNull]
    public static T ShouldNotContain<T, TItem>(
        [NotNull] this T? actual,
        TItem unexpected,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(unexpected))] string unexpectedExpression = "<unexpected-value>") where T : IEnumerable<TItem>
    {
        return AssertDoesNotContain(actual, unexpected, EqualityComparer<TItem>.Default, message, actualExpression,
            constraintExpression: $"Has.No.Member({unexpectedExpression})");
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> doesn't contain an item equal to <paramref name="unexpected"/>.
    /// </summary>
    /// <returns>The asserted sequence, for chaining further assertions.</returns>
    [return: NotNull]
    public static IEnumerable<T> ShouldNotContain<T>(
        [NotNull] this IEnumerable<T>? actual,
        T unexpected,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(unexpected))] string unexpectedExpression = "<unexpected-value>")
    {
        return AssertDoesNotContain(actual, unexpected, EqualityComparer<T>.Default, message, actualExpression,
            constraintExpression: $"Has.No.Member({unexpectedExpression})");
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> doesn't contain an item equal to <paramref name="unexpected"/> according to <paramref name="comparer"/>.
    /// </summary>
    /// <returns>The asserted collection, for chaining further assertions.</returns>
    [return: NotNull]
    public static T ShouldNotContain<T, TItem>(
        [NotNull] this T? actual,
        TItem unexpected,
        IEqualityComparer<TItem> comparer,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(unexpected))] string unexpectedExpression = "<unexpected-value>",
        [CallerArgumentExpression(nameof(comparer))] string comparerExpression = "<comparer>") where T : IEnumerable<TItem>
    {
        ArgumentNullException.ThrowIfNull(comparer);

        return AssertDoesNotContain(actual, unexpected, comparer, message, actualExpression,
            constraintExpression: $"Has.No.Member({unexpectedExpression}).Using({comparerExpression})");
    }

    private static T AssertContains<T, TItem>(
        [NotNull] T? actual,
        TItem expected,
        IEqualityComparer<TItem> comparer,
        NUnitString message,
        string actualExpression,
        string constraintExpression) where T : IEnumerable<TItem>
    {
        var actualItems = actual.ShouldNotBeNull(valueExpression: actualExpression).ToArray();

        if (!actualItems.Contains(expected, comparer))
        {
            MessageWriter.Report(message, actualExpression, constraintExpression,
                $"collection containing {expected.DescribeValue()}", DescribeItems(actualItems));
        }

        return actual;
    }

    private static T AssertDoesNotContain<T, TItem>(
        [NotNull] T? actual,
        TItem unexpected,
        IEqualityComparer<TItem> comparer,
        NUnitString message,
        string actualExpression,
        string constraintExpression) where T : IEnumerable<TItem>
    {
        var actualItems = actual.ShouldNotBeNull(valueExpression: actualExpression).ToArray();

        if (actualItems.Contains(unexpected, comparer))
        {
            MessageWriter.Report(message, actualExpression, constraintExpression,
                $"collection not containing {unexpected.DescribeValue()}", DescribeItems(actualItems));
        }

        return actual;
    }

    /// <summary>
    /// Runs <paramref name="assertion"/> for every item of <paramref name="actual"/> and asserts that it passes for all of them.
    /// </summary>
    /// <remarks>
    /// Every item is checked, even after a failure. The failure message lists each failing item with its index and the message
    /// of its failed assertion (or the exception it threw), up to 10 items.
    /// Other NUnit result exceptions, such as from <see cref="Assert.Ignore(string)"/>, stop the check and pass through unchanged.
    /// </remarks>
    /// <returns>The asserted collection, for chaining further assertions.</returns>
    [return: NotNull]
    public static T ShouldAllSatisfy<T, TItem>(
        [NotNull] this T? actual,
        Action<TItem> assertion,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(assertion))] string assertionExpression = "<assertion>") where T : IEnumerable<TItem>
    {
        return AssertAllSatisfy<T, TItem>(actual, assertion, message, actualExpression, assertionExpression);
    }

    /// <summary>
    /// Runs <paramref name="assertion"/> for every item of <paramref name="actual"/> and asserts that it passes for all of them.
    /// </summary>
    /// <remarks>
    /// Every item is checked, even after a failure. The failure message lists each failing item with its index and the message
    /// of its failed assertion (or the exception it threw), up to 10 items.
    /// Other NUnit result exceptions, such as from <see cref="Assert.Ignore(string)"/>, stop the check and pass through unchanged.
    /// </remarks>
    /// <returns>The asserted sequence, for chaining further assertions.</returns>
    [return: NotNull]
    public static IEnumerable<T> ShouldAllSatisfy<T>(
        [NotNull] this IEnumerable<T>? actual,
        Action<T> assertion,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(assertion))] string assertionExpression = "<assertion>")
    {
        return AssertAllSatisfy<IEnumerable<T>, T>(actual, assertion, message, actualExpression, assertionExpression);
    }

    private static T AssertAllSatisfy<T, TItem>(
        [NotNull] T? actual,
        Action<TItem> assertion,
        NUnitString message,
        string actualExpression,
        string assertionExpression) where T : IEnumerable<TItem>
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);
        ArgumentNullException.ThrowIfNull(assertion);

        var actualItems = actual.ToArray();
        var failures = FindItemFailures(actualItems, assertion);

        if (failures.Length > 0)
        {
            ReportItemFailures(failures, actualItems.Length, message, actualExpression, $"Has.All.Satisfying({assertionExpression})");
        }

        return actual;
    }

    private static ItemFailure<TItem>[] FindItemFailures<TItem>(TItem[] items, Action<TItem> assertion)
    {
        var failures = new List<ItemFailure<TItem>>();

        for (var index = 0; index < items.Length; index++)
        {
            var failureMessage = GetAssertionFailureMessage(assertion, items[index]);

            if (failureMessage != null)
            {
                failures.Add(new ItemFailure<TItem>(index, items[index], failureMessage));
            }
        }

        return failures.ToArray();
    }

    private static string? GetAssertionFailureMessage<TItem>(Action<TItem> assertion, TItem item)
    {
        using var ctx = new TestExecutionContext.IsolatedContext();

        try
        {
            assertion(item);
            return null;
        }
        catch (Exception exception) when (exception is AssertionException or MultipleAssertException)
        {
            return exception.Message;
        }
        catch (Exception exception) when (exception is not ResultStateException)
        {
            return $"<{exception.GetType().FullName}: {exception.Message}>";
        }
    }

    [DoesNotReturn]
    private static void ReportItemFailures<TItem>(
        ItemFailure<TItem>[] failures,
        int itemCount,
        NUnitString message,
        string actualExpression,
        string constraintExpression)
    {
        var writer = new MessageWriter();
        writer.WriteMessageIfNotEmpty(message);
        writer.WriteAssertThat(actualExpression, constraintExpression);
        writer.WriteLine($"{failures.Length} of {itemCount} items don't satisfy the assertion.");

        foreach (var failure in failures.Take(MaxReportedItemFailures))
        {
            WriteItemFailure(writer, failure);
        }

        if (failures.Length > MaxReportedItemFailures)
        {
            writer.WriteLine($"... and {failures.Length - MaxReportedItemFailures} more.");
        }

        writer.ReportFailure();
    }

    private static void WriteItemFailure<TItem>(MessageWriter writer, ItemFailure<TItem> failure)
    {
        writer.WriteLine($"[{failure.Index}] {failure.Item.DescribeValue()}:");

        foreach (var line in failure.Message.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
        {
            writer.WriteLine($"  {line}");
        }
    }

    private readonly record struct ItemFailure<TItem>(int Index, TItem Item, string Message);

    /// <summary>
    /// Asserts that <paramref name="actual"/> has the same items as <paramref name="expected"/> in the same order,
    /// comparing items with <see cref="EqualityComparer{T}.Default"/>.
    /// </summary>
    /// <remarks>
    /// The failure message shows both lengths and the items from the first differing index.
    /// </remarks>
    /// <returns>The asserted collection, for chaining further assertions.</returns>
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

    /// <summary>
    /// Asserts that <paramref name="actual"/> has the same items as <paramref name="expected"/> in the same order,
    /// comparing items with <see cref="EqualityComparer{T}.Default"/>.
    /// </summary>
    /// <remarks>
    /// The failure message shows both lengths and the items from the first differing index.
    /// </remarks>
    /// <returns>The asserted sequence, for chaining further assertions.</returns>
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

    /// <summary>
    /// Asserts that <paramref name="actual"/> has the same items as <paramref name="expected"/> in the same order,
    /// comparing items with <paramref name="comparer"/>.
    /// </summary>
    /// <remarks>
    /// The failure message shows both lengths and the items from the first differing index.
    /// </remarks>
    /// <returns>The asserted collection, for chaining further assertions.</returns>
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

    /// <summary>
    /// Asserts that <paramref name="actual"/> has as many items as <paramref name="expected"/> and that
    /// <paramref name="itemsAreEqual"/> returns <see langword="true"/> for each pair of items at the same index.
    /// </summary>
    /// <remarks>
    /// The failure message shows both lengths and the items from the first differing index.
    /// </remarks>
    /// <returns>The asserted collection, for chaining further assertions.</returns>
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

    /// <summary>
    /// Asserts that <paramref name="actual"/> has as many items as <paramref name="expected"/> and that
    /// <paramref name="itemsAreEqual"/> returns <see langword="true"/> for each pair of items at the same index.
    /// </summary>
    /// <remarks>
    /// The failure message shows both lengths and the items from the first differing index.
    /// </remarks>
    /// <returns>The asserted sequence, for chaining further assertions.</returns>
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

    /// <summary>
    /// Asserts that every item of <paramref name="actual"/> is in <paramref name="expected"/> and vice versa, ignoring order and duplicates.
    /// </summary>
    /// <remarks>
    /// Sets are checked with their own comparer; other sequences use <see cref="EqualityComparer{T}.Default"/>.
    /// The failure message lists the missing and the extra items.
    /// </remarks>
    /// <returns>The asserted collection, for chaining further assertions.</returns>
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

    /// <summary>
    /// Asserts that every item of <paramref name="actual"/> is in <paramref name="expected"/> and vice versa, ignoring order and duplicates.
    /// </summary>
    /// <remarks>
    /// Sets are checked with their own comparer; other sequences use <see cref="EqualityComparer{T}.Default"/>.
    /// The failure message lists the missing and the extra items.
    /// </remarks>
    /// <returns>The asserted sequence, for chaining further assertions.</returns>
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

    /// <summary>
    /// Asserts that every item of <paramref name="actual"/> is in <paramref name="expected"/> and vice versa, ignoring order and duplicates,
    /// comparing items with <paramref name="comparer"/>.
    /// </summary>
    /// <remarks>
    /// <paramref name="comparer"/> is used for both sides, even when they are sets with their own comparers.
    /// The failure message lists the missing and the extra items.
    /// </remarks>
    /// <returns>The asserted collection, for chaining further assertions.</returns>
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

    /// <summary>
    /// Asserts that <paramref name="actual"/> holds the same items as <paramref name="expected"/>, each the same number of times, in any order.
    /// </summary>
    /// <returns><paramref name="actual"/>, for chaining.</returns>
    /// <exception cref="AssertionException">Thrown when <paramref name="actual"/> is null, or when an item is missing from it or occurs in it more times than expected.</exception>
    [return: NotNull]
    public static T ShouldBeMultisetEqualTo<T, TItem>(
        [NotNull] this T? actual,
        IEnumerable<TItem> expected,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string expectedExpression = "<expected-value>") where T : IEnumerable<TItem>
    {
        return AssertMultisetEqual<T, TItem>(actual, expected, EqualityComparer<TItem>.Default, message, actualExpression, expectedExpression,
            constraintExpression: $"Is.EquivalentTo({expectedExpression})");
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> holds the same items as <paramref name="expected"/>, each the same number of times, in any order.
    /// Used when the item type can't be inferred from <paramref name="expected"/>, e.g. for an empty collection expression.
    /// </summary>
    /// <returns><paramref name="actual"/>, for chaining.</returns>
    /// <exception cref="AssertionException">Thrown when <paramref name="actual"/> is null, or when an item is missing from it or occurs in it more times than expected.</exception>
    [return: NotNull]
    public static IEnumerable<T> ShouldBeMultisetEqualTo<T>(
        [NotNull] this IEnumerable<T>? actual,
        IEnumerable<T> expected,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string expectedExpression = "<expected-value>")
    {
        return AssertMultisetEqual<IEnumerable<T>, T>(actual, expected, EqualityComparer<T>.Default, message, actualExpression, expectedExpression,
            constraintExpression: $"Is.EquivalentTo({expectedExpression})");
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> holds the same items as <paramref name="expected"/>, each the same number of times, in any order,
    /// comparing items with <paramref name="comparer"/>.
    /// </summary>
    /// <returns><paramref name="actual"/>, for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="comparer"/> is null.</exception>
    /// <exception cref="AssertionException">Thrown when <paramref name="actual"/> is null, or when an item is missing from it or occurs in it more times than expected.</exception>
    [return: NotNull]
    public static T ShouldBeMultisetEqualTo<T, TItem>(
        [NotNull] this T? actual,
        IEnumerable<TItem> expected,
        IEqualityComparer<TItem> comparer,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string expectedExpression = "<expected-value>",
        [CallerArgumentExpression(nameof(comparer))] string comparerExpression = "<comparer>") where T : IEnumerable<TItem>
    {
        ArgumentNullException.ThrowIfNull(comparer);

        return AssertMultisetEqual<T, TItem>(actual, expected, comparer, message, actualExpression, expectedExpression,
            constraintExpression: $"Is.EquivalentTo({expectedExpression}).Using({comparerExpression})");
    }

    private static T AssertMultisetEqual<T, TItem>(
        [NotNull] T? actual,
        IEnumerable<TItem> expected,
        IEqualityComparer<TItem> comparer,
        NUnitString message,
        string actualExpression,
        string expectedExpression,
        string constraintExpression) where T : IEnumerable<TItem>
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);
        ArgumentNullException.ThrowIfNull(expected, expectedExpression);

        var actualItems = actual.ToArray();
        var expectedItems = expected.ToArray();

        var (missingItems, extraItems) = FindUnmatchedItems(actualItems, expectedItems, comparer);

        if (missingItems.Length > 0 || extraItems.Length > 0)
        {
            ReportUnmatchedItems(actualItems, expectedItems, missingItems, extraItems, message, actualExpression, constraintExpression);
        }

        return actual;
    }

    private static (TItem[] MissingItems, TItem[] ExtraItems) FindUnmatchedItems<TItem>(TItem[] actualItems, TItem[] expectedItems, IEqualityComparer<TItem> comparer)
    {
        var missingItems = new List<TItem>(expectedItems);
        var extraItems = new List<TItem>();

        foreach (var item in actualItems)
        {
            var missingIndex = missingItems.FindIndex(x => comparer.Equals(x, item));

            if (missingIndex < 0)
            {
                extraItems.Add(item);
            }
            else
            {
                missingItems.RemoveAt(missingIndex);
            }
        }

        return (missingItems.ToArray(), extraItems.ToArray());
    }

    [DoesNotReturn]
    private static void ReportUnmatchedItems<TItem>(
        TItem[] actualItems,
        TItem[] expectedItems,
        TItem[] missingItems,
        TItem[] extraItems,
        NUnitString message,
        string actualExpression,
        string constraintExpression)
    {
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

        writer.WriteExpected(DescribeItems(expectedItems));
        writer.WriteActual(DescribeItems(actualItems));

        writer.ReportFailure();
    }
}