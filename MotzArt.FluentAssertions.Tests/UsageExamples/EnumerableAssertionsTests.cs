namespace MotzArt.FluentAssertions.Tests.UsageExamples;

[TestFixture]
public class EnumerableAssertionsTests
{
    [Test]
    public void ShouldBeEmpty_AssertNoItems()
    {
        var list = new List<int>(); 
        list.ShouldBeEmpty();
    }

    [Test]
    public void ShouldNotBeEmpty_AssertContainsItems()
    {
        var list = new List<int> { 1, 2, 3 };
        list.ShouldNotBeEmpty();
    }

    [Test]
    public void ShouldBeNullOrEmpty_AssertNullOrEmpty()
    {
        IEnumerable<string>? items = null;
        items.ShouldBeNullOrEmpty();
    }

    [Test]
    public void ShouldHaveCount_AssertItemCount()
    {
        var numbers = new[] { 1, 2, 3, 4, 5 };
        numbers.ShouldHaveCount(5);
    }

    [Test]
    public void ShouldHasSingle_AssertExactlyOneItem()
    {
        var items = new List<string> { "only one" };
        var single = items.ShouldHasSingle();
        single.ShouldBe("only one");
    }

    [Test]
    public void ShouldHasSingleWithPredicate_AssertSingleMatch()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var even = numbers.ShouldHasSingle(x => x == 2);
        even.ShouldBe(2);
    }

    [Test]
    public void ShouldContain_AssertHasMatchingItem()
    {
        var users = new List<string> { "Alice", "Bob", "Charlie" };
        users.ShouldContain(x => x.Length > 5);
    }

    [Test]
    public void ShouldNotContain_AssertNoMatchingItem()
    {
        var numbers = new List<int> { 1, 2, 3 };
        numbers.ShouldNotContain(x => x > 10);
    }
}