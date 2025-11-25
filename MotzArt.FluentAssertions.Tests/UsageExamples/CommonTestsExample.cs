namespace MotzArt.FluentAssertions.Tests.UsageExamples;

[TestFixture]
public class CommonTestsExample
{
    [Test]
    public void ShouldBe_AssertEquality()
    {
        int value = 42;
        value.ShouldBe(42);
    }

    [Test]
    public void ShouldNotBe_AssertInequality()
    {
        string actual = "hello";
        actual.ShouldNotBe("world");
    }

    [Test]
    public void ShouldBeSameAs_AssertReferenceEquality()
    {
        var obj = new object();
        var sameRef = obj;
        obj.ShouldBeSameAs(sameRef);
    }

    [Test]
    public void ShouldNotBeNull_AssertNonNull()
    {
        string value = "test";
        value.ShouldNotBeNull().ShouldBe("test");
    }

    [Test]
    public void ShouldBeNull_AssertNull()
    {
        string? value = null;
        value.ShouldBeNull();
    }

    [Test]
    public void ShouldBeOfType_AssertType()
    {
        object value = "test string";
        var str = value.ShouldBeOfType<string>();
        str.ShouldBe("test string");
    }

    [Test]
    public void ShouldBeEquivalentTo_AssertPropertyEquality()
    {
        var obj1 = new { Name = "John", Age = 30 };
        var obj2 = new { Name = "John", Age = 30 };
        obj1.ShouldBeEquivalentTo(obj2);
    }
}