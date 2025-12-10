using MotzArt.FluentAssertions.Tests.TestData;

namespace MotzArt.FluentAssertions.Tests;

[TestFixture]
public class CommonAssertionsTests
{
    [Test]
    public void ShouldBe_ShouldPassForEqualValues()
    {
        var actual = 5;
        var expected = 5;
        actual.ShouldBe(expected);
    }

    [Test]
    public void ShouldBe_ShouldFailForNonEqualValues()
    {
        var actual = 5;
        var expected = 10;
        TestHelpers.ShouldThrowAssertionException(() => actual.ShouldBe(expected))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.EqualTo(expectedValue))
                                 Expected: 10
                                 But was:  5

                               """);
    }

    [Test]
    public void ShouldBe_WithEqualityComparer_ShouldPassForEquivalentValues()
    {
        var actual = DataProvider.DefaultTestClass;
        var expected = DataProvider.CreateTestClass(id:2, name: DataProvider.DefaultTestClassName);
        actual.ShouldBe(expected, new TestClassComparerByName());
    }

    [Test]
    public void ShouldBe_WithEqualityComparer_ShouldFailForNonEquivalentValues()
    {
        var actual = DataProvider.DefaultTestClass;
        var expected = DataProvider.AnotherTestClass;
        TestHelpers.ShouldThrowAssertionException(() => actual.ShouldBe(expected, new TestClassComparerByName()))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.EqualTo(expectedValue).Using(comparer))
                                 Expected: <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                                 But was:  <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                               
                               """);
    }

    [Test]
    public void ShouldNotBe_ShouldPassForNonEqualValues()
    {
        var actual = 5;
        var expected = 10;
        actual.ShouldNotBe(expected);
    }

    [Test]
    public void ShouldNotBe_ShouldFailForEqualValues()
    {
        var actual = 5;
        var expected = 5;
        TestHelpers.ShouldThrowAssertionException(() => actual.ShouldNotBe(expected))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.Not.EqualTo(expectedValue))
                                 Expected: not equal to 5
                                 But was:  5

                               """);
    }

    [Test]
    public void ShouldNotBe_WithEqualityComparer_ShouldPassForNonEquivalentValues()
    {
        var actual = DataProvider.DefaultTestClass;
        var expected = DataProvider.AnotherTestClass;
        actual.ShouldNotBe(expected, new TestClassComparerByName());
    }

    [Test]
    public void ShouldNotBe_WithEqualityComparer_ShouldFailForEquivalentValues()
    {
        var actual = DataProvider.DefaultTestClass;
        var expected = DataProvider.CreateTestClass(id: 2, name: DataProvider.DefaultTestClassName);
        TestHelpers.ShouldThrowAssertionException(() => actual.ShouldNotBe(expected, new TestClassComparerByName()))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.Not.EqualTo(expectedValue).Using(comparer))
                                 Expected: not equal to <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                                 But was:  <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                               
                               """);
    }

    [Test]
    public void ShouldBeSameAs_ShouldPassForSameReference()
    {
        var obj = DataProvider.DefaultTestClass;
        var result = obj.ShouldBeSameAs(obj);
        result.ShouldBe(obj);
    }

    [Test]
    public void ShouldBeSameAs_ShouldFailForDifferentReferences()
    {
        var obj1 = DataProvider.DefaultTestClass;
        var obj2 = DataProvider.CreateTestClass(id: 1, name: DataProvider.DefaultTestClassName);
        obj1.ShouldBe(obj2);

        TestHelpers.ShouldThrowAssertionException(() => obj1.ShouldBeSameAs(obj2))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(obj1)}, Is.SameAs(expectedValue))
                                 Expected: same as <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                                 But was:  <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                               
                               """);
    }

    [Test]
    public void ShouldNotBeSameAs_ShouldPassForDifferentReferences()
    {
        var obj1 = DataProvider.DefaultTestClass;
        var obj2 = DataProvider.CreateTestClass(id: 1, name: DataProvider.DefaultTestClassName);
        obj1.ShouldBe(obj2);

        obj1.ShouldNotBeSameAs(obj2);
    }

    [Test]
    public void ShouldNotBeSameAs_ShouldFailForSameReference()
    {
        var obj = DataProvider.DefaultTestClass;
        TestHelpers.ShouldThrowAssertionException(() => obj.ShouldNotBeSameAs(obj))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(obj)}, Is.Not.SameAs(expectedValue))
                                 Expected: not same as <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                                 But was:  <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                               
                               """);
    }

    [Test]
    public void ShouldNotBeNull_ShouldPassForNonNullReference()
    {
        var obj = DataProvider.DefaultTestClass;
        var result = obj.ShouldNotBeNull();
        result.ShouldBe(obj);
    }

    [Test]
    public void ShouldNotBeNull_ShouldFailForNullReference()
    {
        TestClass? obj = null;
        TestHelpers.ShouldThrowAssertionException(() => obj.ShouldNotBeNull())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(obj)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null
                               
                               """);
    }

    [Test]
    public void ShouldBeNull_ShouldPassForNullReference()
    {
        TestClass? obj = null;
        obj.ShouldBeNull();
    }

    [Test]
    public void ShouldBeNull_ShouldFailForNonNullReference()
    {
        var obj = DataProvider.DefaultTestClass;
        TestHelpers.ShouldThrowAssertionException(() => obj.ShouldBeNull())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(obj)}, Is.Null)
                                 Expected: null
                                 But was:  <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                               
                               """);
    }

    [Test]
    public void ShouldBeEquivalentTo_ShouldPassForEquivalentObjects()
    {
        var actual = DataProvider.CreateTestClass(id: 5);
        var expected = DataProvider.CreateTestClass(id: 5);
        actual.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public void ShouldBeEquivalentTo_ShouldFailForNonEquivalentObjects()
    {
        var actual = DataProvider.CreateTestClass(id: 5);
        var expected = DataProvider.CreateTestClass(id: 10);
        TestHelpers.ShouldThrowAssertionException(() => actual.ShouldBeEquivalentTo(expected))
            .Message.ShouldBe($$"""
                                  Assert.That({{nameof(actual)}}, Is.EqualTo(expectedValue).UsingPropertiesComparer())
                                  Expected: <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                                  But was:  <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                                
                                """);
    }

    [Test]
    public void ShouldNotBeEquivalentTo_ShouldPassForNonEquivalentObjects()
    {
        var actual = DataProvider.CreateTestClass(id: 5);
        var expected = DataProvider.CreateTestClass(id: 10);
        actual.ShouldNotBeEquivalentTo(expected);
    }

    [Test]
    public void ShouldNotBeEquivalentTo_ShouldFailForEquivalentObjects()
    {
        var actual = DataProvider.CreateTestClass(id: 5);
        var expected = DataProvider.CreateTestClass(id: 5);
        TestHelpers.ShouldThrowAssertionException(() => actual.ShouldNotBeEquivalentTo(expected))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.Not.EqualTo(expectedValue).UsingPropertiesComparer())
                                 Expected: not equal to <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                                 But was:  <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                               
                               """);
    }

    [Test]
    public void ShouldBeOfType_ShouldPassForCorrectType()
    {
        object? value = DataProvider.DefaultTestClass;
        var result = value.ShouldBeOfType<TestClass>();
        result.ShouldBe(DataProvider.DefaultTestClass);
    }

    [Test]
    public void ShouldBeOfType_ShouldFailForIncorrectType()
    {
        object? value = DataProvider.DefaultTestClass;
        TestHelpers.ShouldThrowAssertionException(() => value.ShouldBeOfType<string>())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(value)}, Is.InstanceOf<String>())
                                 Expected: instance of <System.String>
                                 But was:  <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                               
                               """);
    }

    [Test]
    public void ShouldBeOfType_ShouldFailForNullValue()
    {
        object? value = null;
        TestHelpers.ShouldThrowAssertionException(() => value.ShouldBeOfType<string>())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(value)}, Is.InstanceOf<String>())
                                 Expected: instance of <System.String>
                                 But was:  null
                               
                               """);
    }
    
    [Test]
    public void ShouldNotBeOfType_ShouldPassForIncorrectType()
    {
        object? value = DataProvider.DefaultTestClass;
        value.ShouldNotBeOfType<string>();
    }

    [Test]
    public void ShouldNotBeOfType_ShouldFailForCorrectType()
    {
        object? value = DataProvider.DefaultTestClass;
        TestHelpers.ShouldThrowAssertionException(() => value.ShouldNotBeOfType<TestClass>())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(value)}, Is.Not.InstanceOf<TestClass>())
                                 Expected: not instance of <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                                 But was:  <MotzArt.FluentAssertions.Tests.TestData.TestClass>
                               
                               """);
    }
}