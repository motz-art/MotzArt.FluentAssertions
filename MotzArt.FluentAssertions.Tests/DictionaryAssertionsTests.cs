using static MotzArt.FluentAssertions.Tests.TestHelpers;

namespace MotzArt.FluentAssertions.Tests;

[TestFixture]
public class DictionaryAssertionsTests
{
    private readonly Dictionary<string, int> ages = new() { ["Alice"] = 30, ["Bob"] = 25 };

    [Test]
    public void ShouldContainKey_ShouldReturnValueOfKey()
    {
        ages.ShouldContainKey("Bob").ShouldBe(25);
    }

    [Test]
    public void ShouldContainKey_ShouldFailWhenKeyIsMissing()
    {
        ShouldThrowAssertionException(() => ages.ShouldContainKey("Carol"))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(ages)}, Does.ContainKey("Carol"))
                                 Expected: dictionary containing key Carol
                                 But was:  keys [Alice, Bob]

                               """);
    }

    [Test]
    public void ShouldContainKey_ShouldFailForNullKey()
    {
        string missingKey = null!;
        ShouldThrowAssertionException(() => ages.ShouldContainKey(missingKey))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(ages)}, Does.ContainKey({nameof(missingKey)}))
                                 Expected: dictionary containing key null
                                 But was:  keys [Alice, Bob]

                               """);
    }

    [Test]
    public void ShouldNotContainKey_ShouldPassForNullKey()
    {
        string missingKey = null!;
        ages.ShouldNotContainKey(missingKey).ShouldBeSameAs(ages);
    }

    [Test]
    public void ShouldContainKey_ShouldFailForEmptyDictionary()
    {
        var empty = new Dictionary<string, int>();
        ShouldThrowAssertionException(() => empty.ShouldContainKey("Carol"))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(empty)}, Does.ContainKey("Carol"))
                                 Expected: dictionary containing key Carol
                                 But was:  keys <empty>

                               """);
    }

    [Test]
    public void ShouldContainKey_ShouldFailForNull()
    {
        Dictionary<string, int>? dictionary = null;
        ShouldThrowAssertionException(() => dictionary.ShouldContainKey("Carol"))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(dictionary)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }

    [Test]
    public void ShouldContainKey_ShouldIncludeCustomMessage()
    {
        ShouldThrowAssertionException(() => ages.ShouldContainKey("Carol", "Custom message"))
            .Message.FirstLine(trim: true).ShouldBe("Custom message");
    }

    [Test]
    public void ShouldNotContainKey_ShouldPassAndReturnSourceWhenKeyIsMissing()
    {
        ages.ShouldNotContainKey("Carol").ShouldBeSameAs(ages);
    }

    [Test]
    public void ShouldNotContainKey_ShouldFailWhenKeyIsPresent()
    {
        ShouldThrowAssertionException(() => ages.ShouldNotContainKey("Alice"))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(ages)}, Does.Not.ContainKey("Alice"))
                                 Expected: dictionary not containing key Alice
                                 But was:  keys [Alice, Bob]

                               """);
    }

    [Test]
    public void ShouldNotContainKey_ShouldFailForNull()
    {
        Dictionary<string, int>? dictionary = null;
        ShouldThrowAssertionException(() => dictionary.ShouldNotContainKey("Carol"))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(dictionary)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }

    [Test]
    public void ShouldContainValue_ShouldPassAndReturnSourceWhenValueIsPresent()
    {
        ages.ShouldContainValue(30).ShouldBeSameAs(ages);
    }

    [Test]
    public void ShouldContainValue_ShouldFailWhenValueIsMissing()
    {
        ShouldThrowAssertionException(() => ages.ShouldContainValue(40))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(ages)}, Does.ContainValue(40))
                                 Expected: dictionary containing value 40
                                 But was:  values [30, 25]

                               """);
    }

    [Test]
    public void ShouldContainValue_ShouldIncludeCustomMessage()
    {
        ShouldThrowAssertionException(() => ages.ShouldContainValue(40, "Custom message"))
            .Message.FirstLine(trim: true).ShouldBe("Custom message");
    }

    [Test]
    public void ShouldNotContainValue_ShouldPassAndReturnSourceWhenValueIsMissing()
    {
        ages.ShouldNotContainValue(40).ShouldBeSameAs(ages);
    }

    [Test]
    public void ShouldNotContainValue_ShouldFailWhenValueIsPresent()
    {
        ShouldThrowAssertionException(() => ages.ShouldNotContainValue(30))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(ages)}, Does.Not.ContainValue(30))
                                 Expected: dictionary not containing value 30
                                 But was:  values [30, 25]

                               """);
    }

    [Test]
    public void DictionaryOverloads_ShouldReturnDictionaryForChaining()
    {
        Dictionary<string, int> result = ages
            .ShouldNotContainKey("Carol")
            .ShouldContainValue(30)
            .ShouldNotContainValue(40);

        result.ShouldBeSameAs(ages);
    }

    [Test]
    public void Overloads_ShouldAcceptSortedDictionaryWithoutCast()
    {
        var sortedAges = new SortedDictionary<string, int>(ages);

        IReadOnlyDictionary<string, int> result = sortedAges
            .ShouldNotContainKey("Carol")
            .ShouldContainValue(30)
            .ShouldNotContainValue(40);

        result.ShouldBeSameAs(sortedAges);
        sortedAges.ShouldContainKey("Bob").ShouldBe(25);
    }

#if NET9_0_OR_GREATER
    [Test]
    public void IDictionaryOverloads_ShouldPassAndReturnSourceForChaining()
    {
        IDictionary<string, int> dictionary = ages;

        IDictionary<string, int> result = dictionary
            .ShouldNotContainKey("Carol")
            .ShouldContainValue(30)
            .ShouldNotContainValue(40);

        result.ShouldBeSameAs(dictionary);
        dictionary.ShouldContainKey("Bob").ShouldBe(25);
    }

    [Test]
    public void IDictionaryShouldContainKey_ShouldFailWhenKeyIsMissing()
    {
        IDictionary<string, int> dictionary = ages;
        ShouldThrowAssertionException(() => dictionary.ShouldContainKey("Carol"))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(dictionary)}, Does.ContainKey("Carol"))
                                 Expected: dictionary containing key Carol
                                 But was:  keys [Alice, Bob]

                               """);
    }

    [Test]
    public void IDictionaryShouldNotContainKey_ShouldFailWhenKeyIsPresent()
    {
        IDictionary<string, int> dictionary = ages;
        ShouldThrowAssertionException(() => dictionary.ShouldNotContainKey("Alice"))
            .Message.ShouldStartWith($"  Assert.That({nameof(dictionary)}, Does.Not.ContainKey(\"Alice\"))");
    }

    [Test]
    public void IDictionaryShouldContainValue_ShouldFailWhenValueIsMissing()
    {
        IDictionary<string, int> dictionary = ages;
        ShouldThrowAssertionException(() => dictionary.ShouldContainValue(40))
            .Message.ShouldStartWith($"  Assert.That({nameof(dictionary)}, Does.ContainValue(40))");
    }

    [Test]
    public void IDictionaryShouldNotContainValue_ShouldFailWhenValueIsPresent()
    {
        IDictionary<string, int> dictionary = ages;
        ShouldThrowAssertionException(() => dictionary.ShouldNotContainValue(30))
            .Message.ShouldStartWith($"  Assert.That({nameof(dictionary)}, Does.Not.ContainValue(30))");
    }

    [Test]
    public void IDictionaryOverloads_ShouldFailForNull()
    {
        IDictionary<string, int>? dictionary = null;
        ShouldThrowAssertionException(() => dictionary.ShouldContainKey("Carol"))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(dictionary)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }
#endif
}
