using MotzArt.FluentAssertions.Helpers;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static MotzArt.FluentAssertions.Helpers.MessageUtils;

namespace MotzArt.FluentAssertions;

#if NET9_0_OR_GREATER
/// <summary>
/// Assertions for the keys and values of <see cref="IReadOnlyDictionary{TKey, TValue}"/>, <see cref="IDictionary{TKey, TValue}"/>
/// and <see cref="Dictionary{TKey, TValue}"/> instances.
/// </summary>
/// <remarks>
/// For types that implement both interfaces, such as <see cref="SortedDictionary{TKey, TValue}"/>, the
/// <see cref="IReadOnlyDictionary{TKey, TValue}"/> overloads are chosen. This requires C# 13 or later.
/// </remarks>
#else
/// <summary>
/// Assertions for the keys and values of <see cref="IReadOnlyDictionary{TKey, TValue}"/> and <see cref="Dictionary{TKey, TValue}"/> instances.
/// </summary>
/// <remarks>
/// Overloads for <see cref="IDictionary{TKey, TValue}"/> require .NET 10 or later. On .NET 8, cast an <see cref="IDictionary{TKey, TValue}"/>
/// to <see cref="IReadOnlyDictionary{TKey, TValue}"/>, or wrap it with <c>AsReadOnly()</c> when it doesn't implement that interface.
/// </remarks>
#endif
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

        return AssertContainsKey(actual, key, message, actualExpression, keyExpression);
    }

    /// <inheritdoc cref="ShouldContainKey{TKey, TValue}(IReadOnlyDictionary{TKey, TValue}, TKey, NUnitString, string, string)"/>
    public static TValue ShouldContainKey<TKey, TValue>(
        [NotNull] this Dictionary<TKey, TValue>? actual,
        TKey key,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(key))] string keyExpression = "<key>") where TKey : notnull
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);

        return AssertContainsKey(actual, key, message, actualExpression, keyExpression);
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

        AssertDoesNotContainKey(actual, key, message, actualExpression, keyExpression);
        return actual;
    }

    /// <inheritdoc cref="ShouldNotContainKey{TKey, TValue}(IReadOnlyDictionary{TKey, TValue}, TKey, NUnitString, string, string)"/>
    [return: NotNull]
    public static Dictionary<TKey, TValue> ShouldNotContainKey<TKey, TValue>(
        [NotNull] this Dictionary<TKey, TValue>? actual,
        TKey key,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(key))] string keyExpression = "<key>") where TKey : notnull
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);

        AssertDoesNotContainKey(actual, key, message, actualExpression, keyExpression);
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

        AssertContainsValue(actual, value, message, actualExpression, valueExpression);
        return actual;
    }

    /// <inheritdoc cref="ShouldContainValue{TKey, TValue}(IReadOnlyDictionary{TKey, TValue}, TValue, NUnitString, string, string)"/>
    [return: NotNull]
    public static Dictionary<TKey, TValue> ShouldContainValue<TKey, TValue>(
        [NotNull] this Dictionary<TKey, TValue>? actual,
        TValue value,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(value))] string valueExpression = "<value>") where TKey : notnull
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);

        AssertContainsValue(actual, value, message, actualExpression, valueExpression);
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

        AssertDoesNotContainValue(actual, value, message, actualExpression, valueExpression);
        return actual;
    }

    /// <inheritdoc cref="ShouldNotContainValue{TKey, TValue}(IReadOnlyDictionary{TKey, TValue}, TValue, NUnitString, string, string)"/>
    [return: NotNull]
    public static Dictionary<TKey, TValue> ShouldNotContainValue<TKey, TValue>(
        [NotNull] this Dictionary<TKey, TValue>? actual,
        TValue value,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(value))] string valueExpression = "<value>") where TKey : notnull
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);

        AssertDoesNotContainValue(actual, value, message, actualExpression, valueExpression);
        return actual;
    }

#if NET9_0_OR_GREATER
    /// <inheritdoc cref="ShouldContainKey{TKey, TValue}(IReadOnlyDictionary{TKey, TValue}, TKey, NUnitString, string, string)"/>
    [OverloadResolutionPriority(-1)]
    public static TValue ShouldContainKey<TKey, TValue>(
        [NotNull] this IDictionary<TKey, TValue>? actual,
        TKey key,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(key))] string keyExpression = "<key>")
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);

        return AssertContainsKey(new ReadOnlyDictionaryView<TKey, TValue>(actual), key, message, actualExpression, keyExpression);
    }

    /// <inheritdoc cref="ShouldNotContainKey{TKey, TValue}(IReadOnlyDictionary{TKey, TValue}, TKey, NUnitString, string, string)"/>
    [OverloadResolutionPriority(-1)]
    [return: NotNull]
    public static IDictionary<TKey, TValue> ShouldNotContainKey<TKey, TValue>(
        [NotNull] this IDictionary<TKey, TValue>? actual,
        TKey key,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(key))] string keyExpression = "<key>")
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);

        AssertDoesNotContainKey(new ReadOnlyDictionaryView<TKey, TValue>(actual), key, message, actualExpression, keyExpression);
        return actual;
    }

    /// <inheritdoc cref="ShouldContainValue{TKey, TValue}(IReadOnlyDictionary{TKey, TValue}, TValue, NUnitString, string, string)"/>
    [OverloadResolutionPriority(-1)]
    [return: NotNull]
    public static IDictionary<TKey, TValue> ShouldContainValue<TKey, TValue>(
        [NotNull] this IDictionary<TKey, TValue>? actual,
        TValue value,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(value))] string valueExpression = "<value>")
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);

        AssertContainsValue(new ReadOnlyDictionaryView<TKey, TValue>(actual), value, message, actualExpression, valueExpression);
        return actual;
    }

    /// <inheritdoc cref="ShouldNotContainValue{TKey, TValue}(IReadOnlyDictionary{TKey, TValue}, TValue, NUnitString, string, string)"/>
    [OverloadResolutionPriority(-1)]
    [return: NotNull]
    public static IDictionary<TKey, TValue> ShouldNotContainValue<TKey, TValue>(
        [NotNull] this IDictionary<TKey, TValue>? actual,
        TValue value,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(actual))] string actualExpression = "<some-value>",
        [CallerArgumentExpression(nameof(value))] string valueExpression = "<value>")
    {
        actual.ShouldNotBeNull(valueExpression: actualExpression);

        AssertDoesNotContainValue(new ReadOnlyDictionaryView<TKey, TValue>(actual), value, message, actualExpression, valueExpression);
        return actual;
    }
#endif

    private static TValue AssertContainsKey<TKey, TValue>(
        IReadOnlyDictionary<TKey, TValue> actual,
        TKey key,
        NUnitString message,
        string actualExpression,
        string keyExpression)
    {
        TValue? value = default;

        if (key is null || !actual.TryGetValue(key, out value))
        {
            MessageWriter.Report(message, actualExpression, $"Does.ContainKey({keyExpression})",
                $"dictionary containing key {key.DescribeValue()}", DescribeKeys(actual));
        }

        return value;
    }

    private static void AssertDoesNotContainKey<TKey, TValue>(
        IReadOnlyDictionary<TKey, TValue> actual,
        TKey key,
        NUnitString message,
        string actualExpression,
        string keyExpression)
    {
        if (key is not null && actual.ContainsKey(key))
        {
            MessageWriter.Report(message, actualExpression, $"Does.Not.ContainKey({keyExpression})",
                $"dictionary not containing key {key.DescribeValue()}", DescribeKeys(actual));
        }
    }

    private static void AssertContainsValue<TKey, TValue>(
        IReadOnlyDictionary<TKey, TValue> actual,
        TValue value,
        NUnitString message,
        string actualExpression,
        string valueExpression)
    {
        if (!actual.Values.Contains(value))
        {
            MessageWriter.Report(message, actualExpression, $"Does.ContainValue({valueExpression})",
                $"dictionary containing value {value.DescribeValue()}", DescribeValues(actual));
        }
    }

    private static void AssertDoesNotContainValue<TKey, TValue>(
        IReadOnlyDictionary<TKey, TValue> actual,
        TValue value,
        NUnitString message,
        string actualExpression,
        string valueExpression)
    {
        if (actual.Values.Contains(value))
        {
            MessageWriter.Report(message, actualExpression, $"Does.Not.ContainValue({valueExpression})",
                $"dictionary not containing value {value.DescribeValue()}", DescribeValues(actual));
        }
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