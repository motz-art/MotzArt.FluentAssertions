#if NET9_0_OR_GREATER
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace MotzArt.FluentAssertions.Helpers;

internal sealed class ReadOnlyDictionaryView<TKey, TValue>(IDictionary<TKey, TValue> dictionary) : IReadOnlyDictionary<TKey, TValue>
{
    public TValue this[TKey key] => dictionary[key];

    public IEnumerable<TKey> Keys => dictionary.Keys;

    public IEnumerable<TValue> Values => dictionary.Values;

    public int Count => dictionary.Count;

    public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => dictionary.TryGetValue(key, out value);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
#endif
