namespace MotzArt.FluentAssertions.Helpers;

internal sealed class ItemCounter<TItem>
{
    private readonly Dictionary<Key, int> counts;

    public ItemCounter(IEnumerable<TItem> items, IEqualityComparer<TItem> comparer)
    {
        counts = new Dictionary<Key, int>(new KeyComparer(comparer));

        foreach (var item in items)
        {
            Add(item);
        }
    }

    public bool TryRemove(TItem item)
    {
        var key = new Key(item);

        if (!counts.TryGetValue(key, out var count) || count == 0)
        {
            return false;
        }

        counts[key] = count - 1;
        return true;
    }

    private void Add(TItem item)
    {
        var key = new Key(item);
        counts[key] = counts.GetValueOrDefault(key) + 1;
    }

    private readonly record struct Key(TItem Item);

    private sealed class KeyComparer(IEqualityComparer<TItem> comparer) : IEqualityComparer<Key>
    {
        public bool Equals(Key x, Key y) => comparer.Equals(x.Item, y.Item);

        public int GetHashCode(Key key) => key.Item is null ? 0 : comparer.GetHashCode(key.Item);
    }
}