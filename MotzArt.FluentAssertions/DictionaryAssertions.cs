using MotzArt.FluentAssertions.Helpers;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static MotzArt.FluentAssertions.Helpers.MessageUtils;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace MotzArt.FluentAssertions;

public static class DictionaryAssertions
{
    public static TValue ShouldContainKey<TKey, TValue>(
        [NotNull] this IReadOnlyDictionary<TKey, TValue>? actual,
        TKey key,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(key))] string keyExpression = "<key>")
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);

        if (!actual.TryGetValue(key, out var value))
        {
            MessageWriter.Report(message, actualExpression, $"Does.ContainKey({keyExpression})",
                $"dictionary containing key {key.DescribeValue()}", DescribeKeys(actual));
        }

        return value;
    }

    [return: NotNull]
    public static IReadOnlyDictionary<TKey, TValue> ShouldNotContainKey<TKey, TValue>(
        [NotNull] this IReadOnlyDictionary<TKey, TValue>? actual,
        TKey key,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(key))] string keyExpression = "<key>")
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);

        if (actual.ContainsKey(key))
        {
            MessageWriter.Report(message, actualExpression, $"Does.Not.ContainKey({keyExpression})",
                $"dictionary not containing key {key.DescribeValue()}", DescribeKeys(actual));
        }

        return actual;
    }

    [return: NotNull]
    public static IReadOnlyDictionary<TKey, TValue> ShouldContainValue<TKey, TValue>(
        [NotNull] this IReadOnlyDictionary<TKey, TValue>? actual,
        TValue value,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(value))] string valueExpression = "<value>")
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);

        if (!actual.Values.Contains(value))
        {
            MessageWriter.Report(message, actualExpression, $"Does.ContainValue({valueExpression})",
                $"dictionary containing value {value.DescribeValue()}", DescribeValues(actual));
        }

        return actual;
    }

    [return: NotNull]
    public static IReadOnlyDictionary<TKey, TValue> ShouldNotContainValue<TKey, TValue>(
        [NotNull] this IReadOnlyDictionary<TKey, TValue>? actual,
        TValue value,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(value))] string valueExpression = "<value>")
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);

        if (actual.Values.Contains(value))
        {
            MessageWriter.Report(message, actualExpression, $"Does.Not.ContainValue({valueExpression})",
                $"dictionary not containing value {value.DescribeValue()}", DescribeValues(actual));
        }

        return actual;
    }

    private static string DescribeKeys<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> dictionary)
    {
        return $"keys {DescribeItems(dictionary.Keys.ToArray())}";
    }

    private static string DescribeValues<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> dictionary)
    {
        return $"values {DescribeItems(dictionary.Values.ToArray())}";
    }
}
