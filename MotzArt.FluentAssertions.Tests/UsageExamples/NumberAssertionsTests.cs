namespace MotzArt.FluentAssertions.Tests.UsageExamples;

[TestFixture]
public class NumberAssertionsTests
{
    [Test] public void ShouldBeGreaterThan_AssertGreaterValue() { int value = 10; value.ShouldBeGreaterThan(5); }
    [Test]
    public void ShouldBeGreaterThanOrEqual_AssertGreaterOrEqual()
    {
        int value = 10;
        value.ShouldBeGreaterThanOrEqual(10);
    }

    [Test]
    public void ShouldBeLessThan_AssertLesserValue()
    {
        decimal value = 5m;
        value.ShouldBeLessThan(10);
    }

    [Test]
    public void ShouldBeLessThanOrEqual_AssertLesserOrEqual()
    {
        double value = 10.0;
        value.ShouldBeLessThanOrEqual(10);
    }
}