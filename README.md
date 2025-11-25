# MotzArt.FluentAssertions

A small fluent assertion library for `NUnit` that provides intuitive, chainable assertion methods.

Write expressive, readable test code with detailed failure messages that pinpoint assertion failures.

## Features

- **Fluent API** - Chain assertions naturally with intuitive method names
- **Nullabile ready** - Library is designed with nullabile static analysis in mind
- **Detailed diagnostics** - Failure messages show expected vs. actual with helpful context
- **Caller expression tracking** - Error messages display the code that was asserted
- **Multiple assertion domains** - Common, enumerable, string, numeric, boolean, exception, and span assertions
- **NUnit integration** - Seamless integration with NUnit testing framework

## Installation

Install `MotzArt.FluentAssertions` NuGet package.

## Usage Examples

### Common Assertions

Assert equality, reference equality, type, and `null` checks:

``` C#
using NUnit.Framework;
using MotzArt.FluentAssertions;

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
```


### Enumerable Assertions

Assert collection state and contents:

``` C#
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
```


### String Assertions

Assert string content and format:

``` C#
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
```


### ReadOnlySpan Assertions

Assert span equality and emptiness with detailed diffs:

``` C#
[TestFixture]
public class ReadOnlySpanAssertionsTests
{
    [Test]
    public void CheckSpanIsEmpty()
    {
        ReadOnlySpan<int> span = Array.Empty<int>(); 
        span.ShouldBeEmpty();
    }

    [Test]
    public void CheckSpanIsNotEmpty()
    {
        ReadOnlySpan<int> span = [1, 2, 3];
        span.ShouldNotBeEmpty();
    }

    [Test]
    public void CompareSpansByContent()
    {
        ReadOnlySpan<int> actual = [1, 2, 3];
        ReadOnlySpan<int> expected = [1, 2, 3];
        actual.ShouldBe(expected);
    }
}
```


### Boolean Assertions

Assert true and false conditions:

``` C#
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
```


### Number Assertions

Assert numeric comparisons:

``` C#
[TestFixture]
public class NumberAssertionsTests
{
    [Test] public void ShouldBeGreaterThan_AssertGreaterValue() { int value = 10; value.ShouldBeGreaterThan(5); }
    [Test]
    public void ShouldBeGreaterThanOrEqual_AssertGreaterOrEqual()
    {
        int value = 10;
        value.ShouldBeGreaterThanOrEqual(10);
    }

    [Test]
    public void ShouldBeLessThan_AssertLesserValue()
    {
        decimal value = 5m;
        value.ShouldBeLessThan(10);
    }

    [Test]
    public void ShouldBeLessThanOrEqual_AssertLesserOrEqual()
    {
        double value = 10.0;
        value.ShouldBeLessThanOrEqual(10);
    }
}
```


### Exception Assertions

Assert that code throws expected exceptions:

``` C#
// Recommended use of ThrowAssertions.
using static MotzArt.FluentAssertions.ThrowAssertions;

[TestFixture]
public class ThrowAssertionsTests
{
    [Test]
    public void ShouldThrow_AssertSyncException()
    {
        void TestMethod(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            // Do something...
        }

        // Recommended use of ThrowAssertions with `using static MotzArt.FluentAssertions.ThrowAssertions;`
        var exception = ShouldThrow<ArgumentNullException>(() => TestMethod(null));
        exception.ParamName.ShouldBe("message");
    }

    [Test]
    public void ShouldThrowWithMessage_AssertExceptionMessage()
    {
        // Alternative use through Action
        Action act = () => throw new InvalidOperationException("Operation failed");
        act.ShouldThrowWithMessage<InvalidOperationException>("Operation failed");
    }

    [Test]
    public async Task ShouldThrowAsync_AssertAsyncException()
    {
        Func<Task> action = async () =>
        {
            await Task.Delay(10);
            throw new TimeoutException();
        };

        var ex = await action.ShouldThrowAsync<TimeoutException>();
        ex.ShouldNotBeNull();
    }

    [Test]
    public async Task ShouldThrowAsync_UnwrapsAggregateException()
    {
        Func<Task> action = async () =>
        {
            await Task.Delay(10);
            throw new InvalidOperationException("Async error");
        };

        var ex = await action.ShouldThrowAsync<InvalidOperationException>();
        ex.Message.ShouldBe("Async error");
    }
}
```


## Fluent Chaining

Many assertions return the value being asserted, enabling fluent chaining:

``` C#
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
```


## Custom Messages

All assertions support optional custom messages:

``` C#
    [Test]
    public void WithCustomMessage()
    {
        var value = 42; 
        value.ShouldBe(43, message: "Expected the answer to be 43");

        //  Expected the answer to be 43
        //  Assert.That(value, Is.EqualTo(expectedValue))
        //  Expected: 43
        //  But was:  42
    }
```


## License

This project is licensed under the MIT License - see the LICENSE.txt file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
