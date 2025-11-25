namespace MotzArt.FluentAssertions.Tests.UsageExamples;

[TestFixture]
public class ReadOnlySpanAssertionsTests
{
    [Test]
    public void CheckSpanIsEmpty()
    {
        ReadOnlySpan<int> span = Array.Empty<int>(); 
        span.ShouldBeEmpty();
    }

    [Test]
    public void CheckSpanIsNotEmpty()
    {
        ReadOnlySpan<int> span = [1, 2, 3];
        span.ShouldNotBeEmpty();
    }

    [Test]
    public void CompareSpansByContent()
    {
        ReadOnlySpan<int> actual = [1, 2, 3];
        ReadOnlySpan<int> expected = [1, 2, 3];
        actual.ShouldBe(expected);
    }
}