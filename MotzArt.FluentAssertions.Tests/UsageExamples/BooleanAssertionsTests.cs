namespace MotzArt.FluentAssertions.Tests.UsageExamples;

[TestFixture]
public class BooleanAssertionsTests
{
    [Test]
    public void ShouldBeTrue_AssertTrue() 
    { 
        bool condition = 5 > 3; 
        condition.ShouldBeTrue();
    }

    [Test]
    public void ShouldBeFalse_AssertFalse()
    {
        bool condition = 5 < 3;
        condition.ShouldBeFalse();
    }

    [Test]
    public void ShouldBeTrue_WithNullableBoolean()
    {
        bool? value = true;
        value.ShouldBeTrue();
    }

    [Test]
    public void ShouldBeFalse_WithNullableBoolean()
    {
        bool? value = false;
        value.ShouldBeFalse();
    }

    [Test]
    public void ShouldBeNull_WithNullableBoolean()
    {
        bool? value = null;
        value.ShouldBeNull();
    }
}