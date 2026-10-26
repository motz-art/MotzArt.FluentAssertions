using static MotzArt.FluentAssertions.Tests.TestHelpers;

namespace MotzArt.FluentAssertions.Tests;

[TestFixture]
public class DateTimeAssertionsTests
{
    private static readonly DateTime NewYear = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime NewYearEve = new(2024, 12, 31, 23, 0, 0, DateTimeKind.Utc);
    private static readonly DateTimeOffset NewYearOffset = new(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset NewYearEveOffset = new(2024, 12, 31, 23, 0, 0, TimeSpan.Zero);

    [Test]
    public void ShouldBeBefore_ShouldPassAndReturnValueForEarlierDate()
    {
        NewYearEve.ShouldBeBefore(NewYear).ShouldBe(NewYearEve);
    }

    [Test]
    public void ShouldBeBefore_ShouldFailForSameDate()
    {
        var date = NewYear;
        ShouldThrowAssertionException(() => date.ShouldBeBefore(NewYear))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(date)}, Is.LessThan({nameof(NewYear)}))
                                 Expected: less than 2025-01-01 00:00:00
                                 But was:  2025-01-01 00:00:00

                               """);
    }

    [Test]
    public void ShouldBeBefore_ShouldIncludeCustomMessage()
    {
        ShouldThrowAssertionException(() => NewYear.ShouldBeBefore(NewYearEve, "Custom message"))
            .Message.FirstLine(trim: true).ShouldBe("Custom message");
    }

    [Test]
    public void ShouldBeOnOrBefore_ShouldPassForSameDate()
    {
        NewYear.ShouldBeOnOrBefore(NewYear);
    }

    [Test]
    public void ShouldBeOnOrBefore_ShouldFailForLaterDate()
    {
        var date = NewYear;
        ShouldThrowAssertionException(() => date.ShouldBeOnOrBefore(NewYearEve))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(date)}, Is.LessThanOrEqualTo({nameof(NewYearEve)}))
                                 Expected: less than or equal to 2024-12-31 23:00:00
                                 But was:  2025-01-01 00:00:00

                               """);
    }

    [Test]
    public void ShouldBeAfter_ShouldPassForLaterDate()
    {
        NewYear.ShouldBeAfter(NewYearEve);
    }

    [Test]
    public void ShouldBeAfter_ShouldFailForSameDate()
    {
        var date = NewYear;
        ShouldThrowAssertionException(() => date.ShouldBeAfter(NewYear))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(date)}, Is.GreaterThan({nameof(NewYear)}))
                                 Expected: greater than 2025-01-01 00:00:00
                                 But was:  2025-01-01 00:00:00

                               """);
    }

    [Test]
    public void ShouldBeOnOrAfter_ShouldPassForSameDate()
    {
        NewYear.ShouldBeOnOrAfter(NewYear);
    }

    [Test]
    public void ShouldBeOnOrAfter_ShouldFailForEarlierDate()
    {
        var date = NewYearEve;
        ShouldThrowAssertionException(() => date.ShouldBeOnOrAfter(NewYear))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(date)}, Is.GreaterThanOrEqualTo({nameof(NewYear)}))
                                 Expected: greater than or equal to 2025-01-01 00:00:00
                                 But was:  2024-12-31 23:00:00

                               """);
    }

    [Test]
    public void ShouldBeCloseTo_ShouldPassWithinTolerance()
    {
        NewYearEve.ShouldBeCloseTo(NewYear, TimeSpan.FromHours(1)).ShouldBe(NewYearEve);
    }

    [Test]
    public void ShouldBeCloseTo_ShouldFailOutsideTolerance()
    {
        var date = NewYearEve;
        ShouldThrowAssertionException(() => date.ShouldBeCloseTo(NewYear, TimeSpan.FromMinutes(30)))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(date)}, Is.EqualTo({nameof(NewYear)}).Within(TimeSpan.FromMinutes(30)))
                                 Expected: 2025-01-01 00:00:00 +/- 00:30:00
                                 But was:  2024-12-31 23:00:00

                               """);
    }

    [Test]
    public void ShouldNotBeCloseTo_ShouldPassOutsideTolerance()
    {
        NewYearEve.ShouldNotBeCloseTo(NewYear, TimeSpan.FromMinutes(30));
    }

    [Test]
    public void ShouldNotBeCloseTo_ShouldFailWithinTolerance()
    {
        var date = NewYearEve;
        ShouldThrowAssertionException(() => date.ShouldNotBeCloseTo(NewYear, TimeSpan.FromHours(1)))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(date)}, Is.Not.EqualTo({nameof(NewYear)}).Within(TimeSpan.FromHours(1)))
                                 Expected: not 2025-01-01 00:00:00 +/- 01:00:00
                                 But was:  2024-12-31 23:00:00

                               """);
    }

    [Test]
    public void ShouldBeBefore_WithDateTimeOffset_ShouldPassForEarlierDate()
    {
        NewYearEveOffset.ShouldBeBefore(NewYearOffset).ShouldBe(NewYearEveOffset);
    }

    [Test]
    public void ShouldBeBefore_WithDateTimeOffset_ShouldCompareInstantsAcrossOffsets()
    {
        var berlinMidnight = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.FromHours(1));
        berlinMidnight.ShouldBeBefore(NewYearOffset);
    }

    [Test]
    public void ShouldBeAfter_WithDateTimeOffset_ShouldFailForEarlierDate()
    {
        var date = NewYearEveOffset;
        ShouldThrowAssertionException(() => date.ShouldBeAfter(NewYearOffset))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(date)}, Is.GreaterThan({nameof(NewYearOffset)}))
                                 Expected: greater than 2025-01-01 00:00:00+00:00
                                 But was:  2024-12-31 23:00:00+00:00

                               """);
    }

    [Test]
    public void ShouldBeOnOrBefore_WithDateTimeOffset_ShouldPassForSameDate()
    {
        NewYearOffset.ShouldBeOnOrBefore(NewYearOffset);
    }

    [Test]
    public void ShouldBeOnOrAfter_WithDateTimeOffset_ShouldPassForSameDate()
    {
        NewYearOffset.ShouldBeOnOrAfter(NewYearOffset);
    }

    [Test]
    public void ShouldBeCloseTo_WithDateTimeOffset_ShouldPassWithinTolerance()
    {
        NewYearEveOffset.ShouldBeCloseTo(NewYearOffset, TimeSpan.FromHours(1));
    }

    [Test]
    public void ShouldBeCloseTo_WithDateTimeOffset_ShouldFailOutsideTolerance()
    {
        var date = NewYearEveOffset;
        ShouldThrowAssertionException(() => date.ShouldBeCloseTo(NewYearOffset, TimeSpan.FromMinutes(30)))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(date)}, Is.EqualTo({nameof(NewYearOffset)}).Within(TimeSpan.FromMinutes(30)))
                                 Expected: 2025-01-01 00:00:00+00:00 +/- 00:30:00
                                 But was:  2024-12-31 23:00:00+00:00

                               """);
    }

    [Test]
    public void ShouldNotBeCloseTo_WithDateTimeOffset_ShouldPassOutsideTolerance()
    {
        NewYearEveOffset.ShouldNotBeCloseTo(NewYearOffset, TimeSpan.FromMinutes(30));
    }
}
