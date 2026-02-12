using MotzArt.FluentAssertions.Helpers;

namespace MotzArt.FluentAssertions.Tests;

using static TestHelpers;

[TestFixture]
public class NumberAssertionsTests
{
    [Test]
    public void ShouldBeGreaterThan_ShouldPassWhenValueIsGreater()
    {
        1.ShouldBeGreaterThan(0).ShouldBe(1);
    }

    [Test]
    public void ShouldBeGreaterThan_ShouldPassWhenDoubleValueIsGreater()
    {
        (0.1).ShouldBeGreaterThan(0.0).ShouldBe(0.1);
    }

    [Test]
    public void ShouldBeGreaterThan_ShouldPassWhenDecimalValueIsGreater()
    {
        (0.1m).ShouldBeGreaterThan(0.0m).ShouldBe(0.1m);
    }

    [Test]
    [TestCase(0, 0, TestName = "Equal values")]
    [TestCase(0, 1, TestName = "Actual is less than expected")]
    public void ShouldBeGreaterThan_ShouldFailFor(int actual, int expected)
    {
        ShouldThrowAssertionException(() => actual.ShouldBeGreaterThan(expected))
            .Message.ShouldBe($"""
                                 Assert.That(actual, Is.GreaterThan({expected}))
                                 Expected: greater than {expected}
                                 But was:  {actual}
                               
                               """);

    }

    [Test]
    [TestCase(0, 0, TestName = "Equal values")]
    [TestCase(1, 0, TestName = "Actual is greater than expected")]
    public void ShouldBeGreaterThanOrEqual_ShouldPassFor(int actual, int expected)
    {
        actual.ShouldBeGreaterThanOrEqual(expected);
    }

    [Test]
    public void ShouldBeGreaterThanOrEqual_ShouldFailWhenActualLessThanExpected()
    {
        ShouldThrowAssertionException(() => 0.ShouldBeGreaterThanOrEqual(1))
            .Message.ShouldBe("""
                                Assert.That(0, Is.GreaterThanOrEqualTo(1))
                                Expected: greater than or equal to 1
                                But was:  0

                              """);

    }

    [Test]
    public void ShouldBeLessThan_ShouldPassWhenValueIsLess()
    {
        0.ShouldBeLessThan(1);
    }

    [Test]
    [TestCase(0, 0, TestName = "Equal values")]
    [TestCase(1, 0, TestName = "Actual is greater than expected")]
    public void ShouldBeLessThan_ShouldFailFor(int actual, int expected)
    {
        ShouldThrowAssertionException(() => actual.ShouldBeLessThan(expected))
            .Message.ShouldBe($"""
                                 Assert.That(actual, Is.LessThan({expected}))
                                 Expected: less than {expected}
                                 But was:  {actual}
                               
                               """);

    }

    [Test]
    [TestCase(0, 0, TestName = "Equal values")]
    [TestCase(0, 1, TestName = "Actual is less than expected")]
    public void ShouldBeLessThanOrEqual_ShouldPassFor(int actual, int expected)
    {
        actual.ShouldBeLessThanOrEqual(expected);
    }

    [Test]
    public void ShouldBeLessThanOrEqual_ShouldFailWhenActualLessThanExpected()
    {
        ShouldThrowAssertionException(() => 1.ShouldBeLessThanOrEqual(0))
            .Message.ShouldBe("""
                                Assert.That(1, Is.LessThanOrEqualTo(0))
                                Expected: less than or equal to 0
                                But was:  1

                              """);

    }

    [Test]
    public void ShouldBeInRange_ShouldPassWhenValueIsInRange()
    {
        5.ShouldBeInRage(0, 10).ShouldBe(5);
    }

    [Test]
    [TestCase(1.5, TestName = "Value on minimum boundary of the range")]
    [TestCase(9.9, TestName = "Value on maximum boundary of the range")]
    public void ShouldBeInRange_ShouldPassWhenDecimalValueIsOnBoundaryOfTheRange(decimal value)
    {
        value.ShouldBeInRage(1.5m, 9.9m).ShouldBe(value);
    }

    [Test]
    public void ShouldBeInRange_ShouldFailWhenValueIsOutOfRange()
    {
        ShouldThrowAssertionException(() => 15.ShouldBeInRage(0, 10))
            .Message.ShouldBe("""
                                Assert.That(15, Is.InRange(0, 10))
                                Expected: in range (0,10)
                                But was:  15

                              """);

    }

    [Test]
    [TestCase(4.99, TestName = "Value just below the minimum boundary")]
    [TestCase(10.01, TestName = "Value just above the maximum boundary")]
    public void ShouldBeInRange_ShouldFailWhenDecimalValueIsOutOfRange(decimal value)
    {
        ShouldThrowAssertionException(() => value.ShouldBeInRage(5m, 10m))
            .Message.ShouldBe($"""
                                Assert.That(value, Is.InRange(5m, 10m))
                                Expected: in range (5,10)
                                But was:  {value.DescribeValue()}

                              """);
    }
}