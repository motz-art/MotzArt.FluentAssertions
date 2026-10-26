namespace MotzArt.FluentAssertions.Tests.UsageExamples;

[TestFixture]
public class DictionaryAssertionsTests
{
    [Test]
    public void ShouldContainKey_AssertKeyAndCheckItsValue()
    {
        var ages = new Dictionary<string, int> { ["Alice"] = 30, ["Bob"] = 25 };
        ages.ShouldContainKey("Alice").ShouldBe(30);
    }

    [Test]
    public void ShouldNotContainKey_AssertKeyIsMissing()
    {
        var ages = new Dictionary<string, int> { ["Alice"] = 30 };
        ages.ShouldNotContainKey("Bob");
    }

    [Test]
    public void ShouldContainValue_AssertValueIsPresent()
    {
        var ages = new Dictionary<string, int> { ["Alice"] = 30, ["Bob"] = 25 };
        ages
            .ShouldContainValue(25)
            .ShouldNotContainValue(40);
    }
}
