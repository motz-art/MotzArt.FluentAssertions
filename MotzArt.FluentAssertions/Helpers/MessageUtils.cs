namespace MotzArt.FluentAssertions.Helpers;

internal static class MessageUtils
{
    public static string DescribeItems<TItem>(TItem[] items)
    {
        return items.Length == 0 ? "<empty>" : ((ReadOnlySpan<TItem>)items).DescribeValue();
    }
}
