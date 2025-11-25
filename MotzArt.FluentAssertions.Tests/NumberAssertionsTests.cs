namespace MotzArt.FluentAssertions.Tests;
using static TestHelpers;

[TestFixture]
public class NumberAssertionsTests
{
    [Test]
    public void ShouldBeGreaterThan_ShouldPassWhenValueIsGreater()
    {
        1.ShouldBeGreaterThan(0);
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

}