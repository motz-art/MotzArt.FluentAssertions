using System.Runtime.CompilerServices;
using System.Text;
using MotzArt.FluentAssertions.Helpers;
using MotzArt.FluentStrings;
using NUnit.Framework;

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
        if (!actual.SequenceEqual(expected))
        {
            var writer = new MessageWriter();
            writer.WriteMessageIfNotEmpty(message);

            writer.WriteAssertThat(actualExpression, $"Is.EqualTo({otherExpression})");

            if (actual.Length != expected.Length)
            {
                writer.Write($"Expected span length {expected.Length} but was {actual.Length}.");
            }
            else
            {
                writer.Write($"Span lengths are both {expected.Length}.");
            }

            var diffIndex = FindDiffIndex(actual, expected);

            writer.WriteLine($" Spans differ at index {diffIndex}.");
            writer.WriteExpected(expected.DescribeValue(skip: diffIndex));
            writer.WriteActual(actual.DescribeValue(skip: diffIndex));
            
            writer.ReportFailure();
        }

        return actual;
    }

    private static int FindDiffIndex<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
    {
        var comparer = EqualityComparer<T>.Default;

        var length = Math.Min(a.Length, b.Length);

        for (int i = 0; i < length; i++)
        {
            if (!comparer.Equals(a[i], b[i]))
            {
                return i;
            }
        }

        return length;
    }
}