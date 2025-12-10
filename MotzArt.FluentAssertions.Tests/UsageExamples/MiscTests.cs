namespace MotzArt.FluentAssertions.Tests.UsageExamples;

[TestFixture]
public class MiscTests
{
    [Test]
    public void ChainAssertions()
    {
        var user = new { Name = "Alice", Age = 30 };
        
        user
            .ShouldNotBeNull()
            .ShouldBeOfType<dynamic>();

        var list = new[] { 1, 2, 3 };
        list
            .ShouldNotBeEmpty()
            .ShouldHaveCount(3);

        "hello world"
            .ShouldNotBeNullOrEmpty()
            .ShouldStartWith("hello")
            .ShouldEndWith("world")
            .ShouldContain("lo wo");
    }

    [Test]
    [Ignore("Test is designed to fail to demonstrate custom message functionality.")]
    public void WithCustomMessage()
    {
        var value = 42; 
        value.ShouldBe(43, message: "Expected the answer to be 43");

        //  Expected the answer to be 43
        //  Assert.That(value, Is.EqualTo(expectedValue))
        //  Expected: 43
        //  But was:  42
    }
}