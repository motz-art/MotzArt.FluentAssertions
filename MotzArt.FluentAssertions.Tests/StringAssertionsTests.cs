using static MotzArt.FluentAssertions.Tests.TestHelpers;

namespace MotzArt.FluentAssertions.Tests;

[TestFixture]
public class StringAssertionsTests
{
    [Test]
    [TestCase(null, TestName = "Should pass for null string.")]
    [TestCase("")]
    public void ShouldBeNullOrEmpty_ShouldPassForNullOrEmptyString(string? actual)
    {
        var result = actual.ShouldBeNullOrEmpty();
        result.ShouldBeSameAs(actual);
    }

    [Test]
    [TestCase("Not empty")]
    [TestCase(" ")]
    public void ShouldBeNullOrEmpty_ShouldFailForNonEmptyString(string actual)
    {
        ShouldThrowAssertionException(() => actual.ShouldBeNullOrEmpty())
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Is.Null.Or.Empty)
                                  Expected: null or <empty>
                                  But was:  "{actual}"
                                
                                """);
    }

    [Test]
    public void ShouldNotBeNullOrEmpty_ShouldPassForNonEmptyString()
    {
        var actual = "Not empty";
        var result = actual.ShouldNotBeNullOrEmpty();

        result.ShouldBeSameAs(actual);
    }

    [Test]
    [TestCase(null, "null", TestName = "Should fail for null string.")]
    [TestCase("", "<string.Empty>", TestName = "Should fail for empty string.")]
    public void ShouldNotBeNullOrEmpty_ShouldFailForNullOrEmptyString(string? actual, string valueDescription)
    {
        ShouldThrowAssertionException(() => actual.ShouldNotBeNullOrEmpty())
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Is.Not.NullOrEmpty)
                                  Expected: not null and not <empty>
                                  But was:  {valueDescription}
                                
                                """);
    }

    [Test]
    public void ShouldNotBeNullOrWhiteSpace_ShouldPassForNonWhiteSpaceString()
    {
        var actual = "Not empty";
        var result = actual.ShouldNotBeNullOrWhiteSpace();

        result.ShouldBeSameAs(actual);
    }

    [Test]
    [TestCase(null, "null", TestName = "Should fail for null string.")]
    [TestCase("", "<string.Empty>", TestName = "Should fail for empty string.")]
    [TestCase("   ", "\"   \"", TestName = "Should fail for whitespace string with spaces.")]
    [TestCase("\t", "\"\t\"", TestName = "Should fail for whitespace string with tab.")]
    public void ShouldNotBeNullOrWhiteSpace_ShouldFailForNullEmptyOrWhiteSpaceString(string? actual, string valueDescription)
    {
        ShouldThrowAssertionException(() => actual.ShouldNotBeNullOrWhiteSpace())
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Is.Not.NullOrWhitespace)
                                  Expected: not null and not <empty> and not white-space
                                  But was:  {valueDescription}
                                
                                """);
    }


    [Test]
    public void ShouldBeEmpty_ShouldPassForEmptyString()
    {
        var actual = string.Empty;
        actual.ShouldBeEmpty();
    }

    [Test]
    public void ShouldBeEmpty_ShouldFailForNonEmptyString()
    {
        var actual = "Not empty";
        ShouldThrowAssertionException(() => actual.ShouldBeEmpty())
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Is.Empty)
                                  Expected: <empty>
                                  But was:  "{actual}"
                                
                                """);
    }

    [Test]
    public void ShouldNotBeEmpty_ShouldPassForNonEmptyString()
    {
        var actual = "Not empty";
        var result = actual.ShouldNotBeEmpty();
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldNotBeEmpty_ShouldFailForEmptyString()
    {
        var actual = string.Empty;
        ShouldThrowAssertionException(() => actual.ShouldNotBeEmpty())
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Is.Not.Empty)
                                  Expected: not <empty>
                                  But was:  <string.Empty>
                                
                                """);
    }

    [Test]
    public void ShouldStartWith_ShouldPassWhenStringStartsWithExpectedStart()
    {
        var actual = "Hello, World!";
        var result = actual.ShouldStartWith("Hello");
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldStartWith_ShouldFailWhenStringDoesNotStartWithExpectedStart()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldStartWith("World"))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Does.StartWith(World))
                                  Expected: String starting with "World"
                                  But was:  "Hello, World!"
                                
                                """);
    }

    [Test]
    public void ShouldStartWith_WithStringComparison_ShouldPassWhenStringStartsWithExpectedStart()
    {
        var actual = "Hello, World!";
        var result = actual.ShouldStartWith("Hello", StringComparison.Ordinal);
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldStartWith_WithStringComparisonOrdinalIgnoreCase_ShouldPassWhenStringStartsWithExpectedStart()
    {
        var actual = "Hello, World!";
        var result = actual.ShouldStartWith("hello", StringComparison.OrdinalIgnoreCase);
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldStartWith_WithStringComparison_ShouldFailWhenStringDoesNotStartWithExpectedStart()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldStartWith("World", StringComparison.OrdinalIgnoreCase))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Does.StartWith("World").Using(StringComparison.OrdinalIgnoreCase))
                                  Expected: String starting with "World"
                                  But was:  "Hello, World!"
                                
                                """);
    }

    [Test]
    public void ShouldStartWith_WithStringComparison_ShouldFailWhenStringDoesNotCase()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldStartWith("hello", StringComparison.Ordinal))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Does.StartWith("hello").Using(StringComparison.Ordinal))
                                  Expected: String starting with "hello"
                                  But was:  "Hello, World!"
                                
                                """);
    }

    [Test]
    public void ShouldEndWith_ShouldPassWhenStringEndsWithExpectedEnd()
    {
        var actual = "Hello, World!";
        var result = actual.ShouldEndWith("World!");
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldEndWith_ShouldFailWhenStringDoesNotEndWithExpectedEnd()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldEndWith("Hello"))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Does.EndWith("Hello"))
                                 Expected: String ending with "Hello"
                                 But was:  "Hello, World!"

                               """);
    }

    [Test]
    public void ShouldEndWith_WithStringComparison_ShouldPassWhenStringEndsWithExpectedEnd()
    {
        var actual = "Hello, World!";
        var result = actual.ShouldEndWith("World!", StringComparison.Ordinal);
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldEndWith_WithStringComparisonOrdinalIgnoreCase_ShouldPassWhenStringEndsWithExpectedEnd()
    {
        var actual = "Hello, World!";
        var result = actual.ShouldEndWith("world!", StringComparison.OrdinalIgnoreCase);
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldEndWith_WithStringComparison_ShouldFailWhenStringDoesNotEndWithExpectedEnd()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldEndWith("Hello", StringComparison.Ordinal))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Does.EndWith("Hello").Using(StringComparison.Ordinal))
                                  Expected: String ending with "Hello"
                                  But was:  "Hello, World!"
                                
                                """);
    }

    [Test]
    public void ShouldEndWith_WithStringComparisonOrdinal_ShouldFailWhenStringDoesNotMatchesCase()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldEndWith("world!", StringComparison.Ordinal))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Does.EndWith("world!").Using(StringComparison.Ordinal))
                                  Expected: String ending with "world!"
                                  But was:  "Hello, World!"
                                
                                """);
    }

    [Test]
    public void ShouldContain_ShouldPassWhenStringContainsExpectedSubstring()
    {
        var actual = "Hello, World!";
        var result = actual.ShouldContain("lo, Wo");
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldContain_ShouldFailWhenStringDoesNotContainExpectedSubstring()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldContain("Universe"))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Contains.Substring("Universe"))
                                  Expected: String containing "Universe"
                                  But was:  "Hello, World!"
                                
                                """);
    }

    [Test]
    public void ShouldContain_WithStringComparisonOrdinalIgnoreCase_ShouldPassWhenStringContainsExpectedSubstring()
    {
        var actual = "Hello, World!";
        var result = actual.ShouldContain("LO, wo", StringComparison.OrdinalIgnoreCase);
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldContain_WithStringComparison_ShouldFailWhenSubstringDoesNotMatchCase()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldContain("LO, wo", StringComparison.Ordinal))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Contains.Substring("LO, wo").Using(StringComparison.Ordinal))
                                  Expected: String containing "LO, wo"
                                  But was:  "Hello, World!"
                                
                                """);
    }

    [Test]
    public void ShouldContain_WithStringComparison_ShouldFailWhenSubstringDoesNotContainsExpectedSubstring()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldContain("Universe", StringComparison.OrdinalIgnoreCase))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Contains.Substring("Universe").Using(StringComparison.OrdinalIgnoreCase))
                                  Expected: String containing "Universe"
                                  But was:  "Hello, World!"
                                
                                """);
    }

    [Test]
    public void ShouldNotContain_WithStringComparison_ShouldPassWhenStringDoesNotContainUnexpectedSubstring()
    {
        var actual = "Hello, World!";
        var result = actual.ShouldNotContain("Universe", StringComparison.OrdinalIgnoreCase);
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldNotContain_WithStringComparisonOrdinalIgnoreCase_ShouldFailWhenStringContainsUnexpectedSubstring()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldNotContain("LO, wo", StringComparison.OrdinalIgnoreCase))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Does.Not.Contain("LO, wo").Using(StringComparison.OrdinalIgnoreCase))
                                  Expected: String not containing "LO, wo"
                                  But was:  "Hello, World!"
                                
                                """);
    }

    [Test]
    public void ShouldNotContain_WithStringComparison_ShouldFailWhenStringContainsUnexpectedSubstring()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldNotContain("lo, Wo", StringComparison.Ordinal))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Does.Not.Contain("lo, Wo").Using(StringComparison.Ordinal))
                                  Expected: String not containing "lo, Wo"
                                  But was:  "Hello, World!"
                                
                                """);
    }

    [Test]
    public void ShouldNotContain_ShouldPassWhenStringDoesNotContainUnexpectedSubstring()
    {
        var actual = "Hello, World!";
        var result = actual.ShouldNotContain("Universe");
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldNotContain_ShouldFailWhenStringContainsUnexpectedSubstring()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldNotContain("World"))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Does.Not.Contain("World"))
                                  Expected: not String containing "World"
                                  But was:  "Hello, World!"
                                
                                """);
    }

    [Test]
    public void ShouldBe_ShouldPassWhenStringsAreEqual()
    {
        var actual = "Hello, World!";
        var result = actual.ShouldBe("Hello, World!");
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldBe_ShouldPassWhenStringsAreNull()
    {
        string? actual = null;
        var result = actual.ShouldBe(null);
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldBe_ShouldFailWhenStringsAreNotEqual()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldBe("Hello, Universe!"))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Is.EqualTo("Hello, Universe!"))
                                  Expected string length 16 but was 13. Strings differ at index 7.
                                  Expected: "Hello, Universe!"
                                  But was:  "Hello, World!"
                                  ------------------^
                                
                                """);
    }

    [Test]
    public void ShouldBe_WithStringComparison_ShouldPassWhenStringsAreEqual()
    {
        var actual = "Hello, World!";
        var result = actual.ShouldBe("hello, world!", StringComparison.OrdinalIgnoreCase);
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldBe_WithStringComparison_ShouldFailWhenStringsAreNotEqual()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldBe("Hello, Universe!", StringComparison.OrdinalIgnoreCase))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Is.EqualTo("Hello, Universe!").Using(StringComparison.OrdinalIgnoreCase))
                                  Expected string length 16 but was 13. Strings differ at index 7.
                                  Expected: "Hello, Universe!"
                                  But was:  "Hello, World!"
                                  ------------------^
                                
                                """);
    }

    [Test]
    public void ShouldBe_WithStringComparison_ShouldFailWhenStringsDoNotMatchCase()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldBe("hello, world!", StringComparison.Ordinal))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Is.EqualTo("hello, world!").Using(StringComparison.Ordinal))
                                  String lengths are both 13. Strings differ at index 0.
                                  Expected: "hello, world!"
                                  But was:  "Hello, World!"
                                  -----------^
                                
                                """);
    }

    [Test]
    public void ShouldNotBe_ShouldPassWhenStringsAreNotEqual()
    {
        var actual = "Hello, World!";
        var result = actual.ShouldNotBe("Hello, Universe!");
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldNotBe_ShouldFailWhenStringsAreEqual()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldNotBe("Hello, World!"))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Is.Not.EqualTo("Hello, World!"))
                                  Expected: not equal to "Hello, World!"
                                  But was:  "Hello, World!"
                                
                                """);
    }

    [Test]
    public void ShouldNotBe_WithStringComparison_ShouldPassWhenStringsAreNotEqual()
    {
        var actual = "Hello, World!";
        var result = actual.ShouldNotBe("Hello, Universe!", StringComparison.OrdinalIgnoreCase);
        result.ShouldBeSameAs(actual);
    }

    [Test]
    public void ShouldNotBe_WithStringComparison_ShouldFailWhenStringsAreEqual()
    {
        var actual = "Hello, World!";
        ShouldThrowAssertionException(() => actual.ShouldNotBe("hello, world!", StringComparison.OrdinalIgnoreCase))
            .Message.ShouldBe($"""
                                  Assert.That({nameof(actual)}, Is.Not.EqualTo("hello, world!").Using(StringComparison.OrdinalIgnoreCase))
                                  Expected: not "hello, world!", using strongly typed comparer
                                  But was:  "Hello, World!"
                                
                                """);
    }
}