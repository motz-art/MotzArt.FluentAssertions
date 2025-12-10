using static MotzArt.FluentAssertions.Tests.TestHelpers;

namespace MotzArt.FluentAssertions.Tests;

[TestFixture]
public class BooleanAssertionsTests
{
    [Test]
    public void ShouldBeTrue_Success()
    {
        true.ShouldBeTrue();
    }

    [Test]
    public void ShouldBeTrue_ShouldFailWhenNotTrue()
    {
        var variable = !true;
        ShouldThrowAssertionException(() => variable.ShouldBeTrue())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(variable)}, Is.True)
                                 Expected: True
                                 But was:  False

                               """);
    }

    [Test]
    public void ShouldBeFalse_Success()
    {
        false.ShouldBeFalse();
    }

    [Test]
    public void ShouldBeFalse_ShouldFailWhenNotFalse()
    {
        var variable = !false;
        ShouldThrowAssertionException(() => variable.ShouldBeFalse(), $"  Assert.That({nameof(variable)}, Is.False)\r\n  Expected: False\r\n  But was:  True\r\n");
    }

    [Test]
    public void ShouldBeTrue_WithNullableBoolean_Success()
    {
        bool? variable = true;
        variable.ShouldBeTrue();
    }

    [Test]
    [TestCase(false)]
    [TestCase(null)]
    public void ShouldBeTrue_WithNullableBoolean_ShouldFailWhenNotTrue(bool? variable)
    {
        ShouldThrowAssertionException(() => variable.ShouldBeTrue(), $"  Assert.That({nameof(variable)}, Is.True)\r\n  Expected: True\r\n  But was:  {(variable == null ? "null" : variable.ToString())}\r\n");
    }

    [Test]
    public void ShouldBeFalse_WithNullableBoolean_Success()
    {
        bool? variable = false;
        variable.ShouldBeFalse();
    }

    [Test]
    [TestCase(true)]
    [TestCase(null)]
    public void ShouldBeFalse_WithNullableBoolean_ShouldFailWhenNotFalse(bool? variable)
    {
        ShouldThrowAssertionException(() => variable.ShouldBeFalse(), $"  Assert.That({nameof(variable)}, Is.False)\r\n  Expected: False\r\n  But was:  {(variable == null ? "null" : variable.ToString())}\r\n");
    }    
    
    [Test]
    public void ShouldBeTrue_WithMessage_Success()
    {
        true.ShouldBeTrue("Some message.");
    }

    [Test]
    public void ShouldBeTrue_WithMessage_ShouldThrowWhenNotTrue()
    {
        var variable = !true;
        ShouldThrowAssertionException(() => variable.ShouldBeTrue("Some message.")).Message.FirstLine(trim: true).ShouldBe("Some message.");
    }

    [Test]
    public void ShouldBeFalse_WithMessage()
    {
        false.ShouldBeFalse("Some message.");
    }

    [Test]
    public void ShouldBeFalse_WithMessage_ShouldThrowWhenNotFalse()
    {
        var variable = !false;
        ShouldThrowAssertionException(() => variable.ShouldBeFalse("Some message.")).Message.ShouldContain("Some message.");
    }

    [Test]
    public void ShouldBeTrue_WithNullableBooleanAndMessage()
    {
        bool? variable = true;
        variable.ShouldBeTrue("Some message.");
    }

    [Test]
    [TestCase(false)]
    [TestCase(null)]
    public void ShouldBeTrue_WithNullableBooleanAndMessage_ShouldFail(bool? variable)
    {
        ShouldThrowAssertionException(() => variable.ShouldBeTrue("Some message.")).Message.FirstLine(trim: true).ShouldBe("Some message.");
    }

    [Test]
    public void ShouldBeFalse_WithNullableBooleanAndMessage()
    {
        bool? variable = false;
        variable.ShouldBeFalse("Some message.");
    }

    [Test]
    [TestCase(true)]
    [TestCase(null)]
    public void ShouldBeFalse_WithNullableBooleanAndMessage_ShouldFail(bool? variable)
    {
        ShouldThrowAssertionException(() => variable.ShouldBeFalse("Some message.")).Message.FirstLine(trim: true).ShouldBe("Some message.");
    }
}