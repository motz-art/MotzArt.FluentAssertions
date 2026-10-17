using MotzArt.FluentAssertions.Helpers;
using NUnit.Framework;
using System.Runtime.CompilerServices;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace MotzArt.FluentAssertions;

public static class ReadOnlySpanAssertions
{

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


    public static ReadOnlySpan<T> ShouldBe<T>(this ReadOnlySpan<T> actual, ReadOnlySpan<T> expected,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>", [CallerArgumentExpression(nameof(expected))] string otherExpression = "<ReadOnlySpan>")
    {
        return AssertSequenceEqual(actual, expected, EqualityComparer<T>.Default.Equals, message, actualExpression,
            constraintExpression: $"Is.EqualTo({otherExpression})");
    }

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