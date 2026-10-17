namespace MotzArt.FluentAssertions.Tests;

[TestFixture]
public class ReadOnlySpanAssertionsTests
{
    [Test]
    public void ShouldBeEmpty_WithReadOnlySpan_ShouldPassForEmptySpan()
    {
        var span = ReadOnlySpan<int>.Empty;
        span.ShouldBeEmpty();
    }

    [Test]
    public void ShouldBeEmpty_WithReadOnlySpan_ShouldFailForNonEmptySpan()
    {
        TestHelpers.ShouldThrowAssertionException(() =>
            {
                var span = new ReadOnlySpan<int>([1, 2, 3]);
                span.ShouldBeEmpty();
            })
            .Message.ShouldBe($"""
                                 Assert.That(span, Is.Empty)
                                 Expected: empty ReadOnlySpan<Int32>
                                 But was:  [1, 2, 3]

                               """);
    }

    [Test]
    public void ShouldNotBeEmpty_WithReadOnlySpan_ShouldPassForNonEmptySpan()
    {
        var span = new ReadOnlySpan<int>([1, 2, 3]);
        var result = span.ShouldNotBeEmpty();

        result.SequenceEqual([1, 2, 3]).ShouldBeTrue();
    }

    [Test]
    public void ShouldNotBeEmpty_WithReadOnlySpan_ShouldFailForNonEmptySpan()
    {
        TestHelpers.ShouldThrowAssertionException(() =>
            {
                var span = ReadOnlySpan<int>.Empty;
                span.ShouldNotBeEmpty();
            })
            .Message.ShouldBe($"""
                                 Assert.That(span, Is.Not.Empty)
                                 Expected: not empty ReadOnlySpan<Int32>
                                 But was:  empty <ReadOnlySpan<Int32>>

                               """);
    }

    [Test]
    public void ShouldBe_WithReadOnlySpan_PassForEqualSpans()
    {
        var span1 = new ReadOnlySpan<int>([1, 2, 3]);
        var span2 = new ReadOnlySpan<int>([1, 2, 3]);

        span1.ShouldBe(span2);
    }

    [Test]
    public void ShouldBe_WithReadOnlySpan_FailWhenSpansHasDifferentLength()
    {
        TestHelpers.ShouldThrowAssertionException(() =>
        {
            var span1 = new ReadOnlySpan<int>([1, 2, 3, 4]);
            var span2 = new ReadOnlySpan<int>([1, 2, 3]);

            span1.ShouldBe(span2);
        }).Message.ShouldBe($"""
                               Assert.That(span1, Is.EqualTo(span2))
                               Expected span length 3 but was 4. Spans differ at index 3.
                               Expected: [...]
                               But was:  [..., 4]
                             
                             """);
    }

    [Test]
    public void ShouldBe_WithReadOnlySpan_FailWhenSpanHasSameLengthButDiffer()
    {
        TestHelpers.ShouldThrowAssertionException(() =>
        {
            var span1 = new ReadOnlySpan<int>([1, 2, 3]);
            var span2 = new ReadOnlySpan<int>([1, 2, 4]);

            span1.ShouldBe(span2);
        }).Message.ShouldBe($"""
                               Assert.That(span1, Is.EqualTo(span2))
                               Span lengths are both 3. Spans differ at index 2.
                               Expected: [..., 4]
                               But was:  [..., 3]

                             """);
    }

    [Test]
    public void ShouldBe_WithReadOnlySpanAndComparer_PassWhenComparerMatches()
    {
        var span1 = new ReadOnlySpan<string>(["a", "b"]);
        var span2 = new ReadOnlySpan<string>(["A", "B"]);

        span1.ShouldBe(span2, StringComparer.OrdinalIgnoreCase);
    }

    [Test]
    public void ShouldBe_WithReadOnlySpanAndComparer_FailWhenComparerDoesNotMatch()
    {
        TestHelpers.ShouldThrowAssertionException(() =>
        {
            var span1 = new ReadOnlySpan<string>(["a", "b"]);
            var span2 = new ReadOnlySpan<string>(["A", "C"]);

            span1.ShouldBe(span2, StringComparer.OrdinalIgnoreCase);
        }).Message.ShouldBe($"""
                               Assert.That(span1, Is.EqualTo(span2).Using(StringComparer.OrdinalIgnoreCase))
                               Span lengths are both 2. Spans differ at index 1.
                               Expected: [..., C]
                               But was:  [..., b]

                             """);
    }

    [Test]
    public void ShouldBe_WithReadOnlySpanAndComparer_FailWhenSpansHasDifferentLength()
    {
        TestHelpers.ShouldThrowAssertionException(() =>
        {
            var span1 = new ReadOnlySpan<string>(["a"]);
            var span2 = new ReadOnlySpan<string>(["A", "B"]);

            span1.ShouldBe(span2, StringComparer.OrdinalIgnoreCase);
        }).Message.ShouldBe($"""
                               Assert.That(span1, Is.EqualTo(span2).Using(StringComparer.OrdinalIgnoreCase))
                               Expected span length 2 but was 1. Spans differ at index 1.
                               Expected: [..., B]
                               But was:  [...]

                             """);
    }

    [Test]
    public void ShouldBe_WithReadOnlySpanAndPredicate_PassForMatchingItems()
    {
        var span1 = new ReadOnlySpan<int>([1, 2]);
        var span2 = new ReadOnlySpan<string>(["1", "2"]);

        span1.ShouldBe(span2, (a, e) => a.ToString() == e);
    }

    [Test]
    public void ShouldBe_WithReadOnlySpanAndPredicate_FailForNotMatchingItems()
    {
        TestHelpers.ShouldThrowAssertionException(() =>
        {
            var span1 = new ReadOnlySpan<int>([1, 2, 3]);
            var span2 = new ReadOnlySpan<string>(["1", "2", "4"]);

            span1.ShouldBe(span2, (a, e) => a.ToString() == e);
        }).Message.ShouldBe($"""
                               Assert.That(span1, Is.EqualTo(span2).Using((a, e) => a.ToString() == e))
                               Span lengths are both 3. Spans differ at index 2.
                               Expected: [..., 4]
                               But was:  [..., 3]

                             """);
    }
}