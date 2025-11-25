namespace MotzArt.FluentAssertions.Tests.UsageExamples;

[TestFixture]
public class StringAssertionsTests
{
    [Test]
    public void ShouldBe_AssertExactStringMatch()
    {
        string value = "hello world"; 
        value.ShouldBe("hello world");
    }

    [Test]
    public void ShouldBeWithComparison_CaseSensitiveMatch()
    {
        string value = "Hello";
        value.ShouldBe("HELLO", StringComparison.OrdinalIgnoreCase);
    }

    [Test]
    public void ShouldNotBe_AssertStringDifference()
    {
        string value = "foo";
        value.ShouldNotBe("bar");
    }

    [Test]
    public void ShouldBeEmpty_AssertEmptyString()
    {
        string value = "";
        value.ShouldBeEmpty();
    }

    [Test]
    public void ShouldNotBeEmpty_AssertNonEmptyString()
    {
        string value = "content";
        value.ShouldNotBeEmpty();
    }

    [Test]
    public void ShouldBeNullOrEmpty_AssertNullOrEmptyString()
    {
        string? value = null;
        value.ShouldBeNullOrEmpty();
    }

    [Test]
    public void ShouldNotBeNullOrWhiteSpace_AssertHasContent()
    {
        string value = "  not empty  ";
        value.ShouldNotBeNullOrWhiteSpace();
    }

    [Test]
    public void ShouldStartWith_AssertStringPrefix()
    {
        string value = "hello world";
        value.ShouldStartWith("hello");
    }

    [Test]
    public void ShouldStartWithComparison_CaseInsensitivePrefix()
    {
        string value = "Hello World";
        value.ShouldStartWith("hello", StringComparison.OrdinalIgnoreCase);
    }

    [Test]
    public void ShouldEndWith_AssertStringSuffix()
    {
        string value = "hello world";
        value.ShouldEndWith("world");
    }

    [Test]
    public void ShouldEndWithComparison_CaseInsensitiveSuffix()
    {
        string value = "Hello World";
        value.ShouldEndWith("WORLD", StringComparison.OrdinalIgnoreCase);
    }

    [Test]
    public void ShouldContain_AssertSubstringPresence()
    {
        string value = "hello beautiful world";
        value.ShouldContain("beautiful");
    }

    [Test]
    public void ShouldNotContain_AssertSubstringAbsence()
    {
        string value = "hello world";
        value.ShouldNotContain("goodbye");
    }
}