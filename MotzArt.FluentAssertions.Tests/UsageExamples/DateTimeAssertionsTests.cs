namespace MotzArt.FluentAssertions.Tests.UsageExamples;

[TestFixture]
public class DateTimeAssertionsTests
{
    [Test]
    public void ShouldBeBeforeAndAfter_AssertOrderOfDates()
    {
        var orderedAt = new DateTime(2025, 3, 1, 10, 0, 0);
        var shippedAt = new DateTime(2025, 3, 2, 8, 30, 0);

        shippedAt
            .ShouldBeAfter(orderedAt)
            .ShouldBeOnOrBefore(orderedAt.AddDays(1));

        orderedAt.ShouldBeBefore(shippedAt);
    }

    [Test]
    public void ShouldBeCloseTo_AssertDateWithinTolerance()
    {
        var createdAt = DateTimeOffset.UtcNow;
        createdAt.ShouldBeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }
}
