using MotzArt.FluentAssertions.Helpers;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static MotzArt.FluentAssertions.Helpers.MessageUtils;

namespace MotzArt.FluentAssertions;

/// <summary>
/// Assertions for the keys and values of <see cref="IReadOnlyDictionary{TKey, TValue}"/> instances.
/// </summary>
public static class DictionaryAssertions
{
    /// <summary>
    /// Asserts that <paramref name="actual"/> isn't <see langword="null"/> and contains <paramref name="key"/>.
    /// </summary>
    /// <remarks>
    /// A <see langword="null"/> <paramref name="key"/> counts as missing.
    /// </remarks>
    /// <returns>The value of <paramref name="key"/>, for chaining assertions on it.</returns>
    public static TValue ShouldContainKey<TKey, TValue>(
        [NotNull] this IReadOnlyDictionary<TKey, TValue>? actual,
        TKey key,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(key))] string keyExpression = "<key>")
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);

        TValue? value = default;

        if (key is null || !actual.TryGetValue(key, out value))
        {
            MessageWriter.Report(message, actualExpression, $"Does.ContainKey({keyExpression})",
                $"dictionary containing key {key.DescribeValue()}", DescribeKeys(actual));
        }

        return value;
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> isn't <see langword="null"/> and doesn't contain <paramref name="key"/>.
    /// </summary>
    /// <remarks>
    /// A <see langword="null"/> <paramref name="key"/> counts as missing.
    /// </remarks>
    /// <returns>The asserted dictionary, for chaining further assertions.</returns>
    [return: NotNull]
    public static IReadOnlyDictionary<TKey, TValue> ShouldNotContainKey<TKey, TValue>(
        [NotNull] this IReadOnlyDictionary<TKey, TValue>? actual,
        TKey key,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(key))] string keyExpression = "<key>")
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);

        if (key is not null && actual.ContainsKey(key))
        {
            MessageWriter.Report(message, actualExpression, $"Does.Not.ContainKey({keyExpression})",
                $"dictionary not containing key {key.DescribeValue()}", DescribeKeys(actual));
        }

        return actual;
    }

    /// <summary>
    /// Asserts that <paramref name="actual"/> isn't <see langword="null"/> and contains <paramref name="value"/> under any key.
    /// </summary>
    /// <returns>The asserted dictionary, for chaining further assertions.</returns>
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

    /// <summary>
    /// Asserts that <paramref name="actual"/> isn't <see langword="null"/> and doesn't contain <paramref name="value"/> under any key.
    /// </summary>
    /// <returns>The asserted dictionary, for chaining further assertions.</returns>
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
