using MotzArt.FluentAssertions.Helpers;
using NUnit.Framework;
using System.Runtime.CompilerServices;

namespace MotzArt.FluentAssertions;

/// <summary>
/// Assertions for <see cref="ReadOnlySpan{T}"/> values.
/// </summary>
/// <remarks>
/// A failed comparison reports the lengths of both spans and the index of the first differing item.
/// </remarks>
public static class ReadOnlySpanAssertions
{

    /// <summary>
    /// Asserts that <paramref name="source"/> has no items.
    /// </summary>
    public static void ShouldBeEmpty<T>(
        this ReadOnlySpan<T> source,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(source))] string sourceExpression = "<some-value>")
    {
        if (!source.IsEmpty)
        {
            MessageWriter.Report(message, sourceExpression, "Is.Empty",
                $"empty ReadOnlySpan<{typeof(T).DescribeType()}>", source.DescribeValue());
        }
    }

    /// <summary>
    /// Asserts that <paramref name="source"/> has at least one item.
    /// </summary>
    /// <returns>The asserted span, for chaining further assertions.</returns>
    public static ReadOnlySpan<T> ShouldNotBeEmpty<T>(
        this ReadOnlySpan<T> source,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(source))] string sourceExpression = "<some-value>")
    {
        if (source.IsEmpty)
        {
            MessageWriter.Report(message, sourceExpression, "Is.Not.Empty",
                $"not empty ReadOnlySpan<{typeof(T).DescribeType()}>", source.DescribeValue());
        }

        return source;
    }


    /// <summary>
    /// Asserts that <paramref name="actual"/> has the same items as <paramref name="expected"/> in the same order,
    /// comparing items with <see cref="EqualityComparer{T}.Default"/>.
    /// </summary>
    /// <returns>The asserted span, for chaining further assertions.</returns>
    public static ReadOnlySpan<T> ShouldBe<T>(this ReadOnlySpan<T> actual, ReadOnlySpan<T> expected,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>", [CallerArgumentExpression(nameof(expected))] string otherExpression = "<ReadOnlySpan>")
    {
        return AssertSequenceEqual(actual, expected, EqualityComparer<T>.Default.Equals, message, actualExpression,
            constraintExpression: $"Is.EqualTo({otherExpression})");
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> has the same items as <paramref name="expected"/> in the same order,
    /// comparing items with <paramref name="comparer"/>.
    /// </summary>
    /// <returns>The asserted span, for chaining further assertions.</returns>
    public static ReadOnlySpan<T> ShouldBe<T>(this ReadOnlySpan<T> actual, ReadOnlySpan<T> expected,
        IEqualityComparer<T> comparer,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string otherExpression = "<ReadOnlySpan>",
        [CallerArgumentExpression(nameof(comparer))] string comparerExpression = "<comparer>")
    {
        ArgumentNullException.ThrowIfNull(comparer);

        return AssertSequenceEqual(actual, expected, comparer.Equals, message, actualExpression,
            constraintExpression: $"Is.EqualTo({otherExpression}).Using({comparerExpression})");
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> has as many items as <paramref name="expected"/> and that
    /// <paramref name="itemsAreEqual"/> returns <see langword="true"/> for each pair of items at the same index.
    /// </summary>
    /// <returns>The asserted span, for chaining further assertions.</returns>
    public static ReadOnlySpan<TActual> ShouldBe<TActual, TExpected>(this ReadOnlySpan<TActual> actual, ReadOnlySpan<TExpected> expected,
        Func<TActual, TExpected, bool> itemsAreEqual,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(expected))] string otherExpression = "<ReadOnlySpan>",
        [CallerArgumentExpression(nameof(itemsAreEqual))] string predicateExpression = "<predicate>")
    {
        return AssertSequenceEqual(actual, expected, itemsAreEqual, message, actualExpression,
            constraintExpression: $"Is.EqualTo({otherExpression}).Using({predicateExpression})");
    }

    private static ReadOnlySpan<TActual> AssertSequenceEqual<TActual, TExpected>(ReadOnlySpan<TActual> actual, ReadOnlySpan<TExpected> expected,
        Func<TActual, TExpected, bool> itemsAreEqual,
        NUnitString message,
        string actualExpression,
        string constraintExpression)
    {
        ArgumentNullException.ThrowIfNull(itemsAreEqual);

        var diffIndex = FindDiffIndex(actual, expected, itemsAreEqual);

        if (diffIndex == actual.Length && actual.Length == expected.Length)
        {
            return actual;
        }

        var writer = new MessageWriter();
        writer.WriteMessageIfNotEmpty(message);

        writer.WriteAssertThat(actualExpression, constraintExpression);

        if (actual.Length != expected.Length)
        {
            writer.Write($"Expected span length {expected.Length} but was {actual.Length}.");
        }
        else
        {
            writer.Write($"Span lengths are both {expected.Length}.");
        }

        writer.WriteLine($" Spans differ at index {diffIndex}.");
        writer.WriteExpected(expected.DescribeValue(skip: diffIndex));
        writer.WriteActual(actual.DescribeValue(skip: diffIndex));

        writer.ReportFailure();
        return actual;
    }

    private static int FindDiffIndex<TActual, TExpected>(ReadOnlySpan<TActual> a, ReadOnlySpan<TExpected> b, Func<TActual, TExpected, bool> itemsAreEqual)
    {
        var length = Math.Min(a.Length, b.Length);

        for (int i = 0; i < length; i++)
        {
            if (!itemsAreEqual(a[i], b[i]))
            {
                return i;
            }
        }

        return length;
    }
}