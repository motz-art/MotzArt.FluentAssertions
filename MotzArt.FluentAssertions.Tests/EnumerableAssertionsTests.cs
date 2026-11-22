using static MotzArt.FluentAssertions.Tests.TestHelpers;
using static MotzArt.FluentAssertions.ThrowAssertions;
using MotzArt.FluentAssertions.Tests.TestData;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace MotzArt.FluentAssertions.Tests;

[TestFixture]
public class EnumerableAssertionsTests
{
    [Test]
    public void ShouldBeEmpty_ShouldPassForEmptyArray()
    {
        var array = Array.Empty<int>();
        var result = array.ShouldBeEmpty();
        result.ShouldBeSameAs(array);
    }

    [Test]
    public void ShouldBeEmpty_ShouldPassForEmptyEnumerable()
    {
        var enumerable = Enumerable.Empty<int>();
        enumerable.ShouldBeEmpty();
    }

    [Test]
    public void ShouldBeEmpty_WithNonEmptyArray_ShouldFailForNull()
    {
        int[]? array = null;
        ShouldThrowAssertionException(() => array.ShouldBeEmpty())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(array)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }

    [Test]
    public void ShouldBeEmpty_WithNonEmptyArray_ShouldFailForNonEmptyArray()
    {
        var array = new[] { 1, 2, 3 };
        ShouldThrowAssertionException(() => array.ShouldBeEmpty())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(array)}, Is.Empty)
                                 Expected: <empty>
                                 But was:  < 1, 2, 3 >
                               
                               """);
    }

    [Test]
    public void ShouldBeEmpty_WithNonEmptyArray_ShouldFailForNonEmptyEnumerable()
    {
        var enumerable = Enumerable.Repeat(1, 1);
        ShouldThrowAssertionException(() => enumerable.ShouldBeEmpty())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Is.Empty)
                                 Expected: <empty>
                                 But was:  < 1 >
                               
                               """);
    }

    [Test]
    public void ShouldBeNullOrEmpty_ShouldPassForNullEnumerable()
    {
        IEnumerable<int>? enumerable = null;
        var result = enumerable.ShouldBeNullOrEmpty();
        // ReSharper disable once PossibleMultipleEnumeration
        result.ShouldBeSameAs(enumerable);
    }

    [Test]
    public void ShouldBeNullOrEmpty_ShouldPassForEmptyEnumerable()
    {
        var enumerable = Enumerable.Empty<int>();
        var result = enumerable.ShouldBeNullOrEmpty();
        result.ShouldBeSameAs(enumerable);
    }

    [Test]
    public void ShouldBeNullOrEmpty_WithNonEmptyEnumerable_ShouldFail()
    {
        var enumerable = Enumerable.Repeat(1, 1);
        ShouldThrowAssertionException(() => enumerable.ShouldBeNullOrEmpty())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Is.Null.Or.Empty)
                                 Expected: null or <empty>
                                 But was:  < 1 >
                               
                               """);
    }

    [Test]
    public void ShouldNotBeNullOrEmpty_ShouldPassForNonEmptyEnumerable()
    {
        var enumerable = Enumerable.Repeat(1, 1);
        var result = enumerable.ShouldNotBeNullOrEmpty();
        result.ShouldBeSameAs(enumerable);
    }

    [Test]
    public void ShouldNotBeNullOrEmpty_ShouldFailForNullEnumerable()
    {
        IEnumerable<int>? enumerable = null;
        ShouldThrowAssertionException(() => enumerable.ShouldNotBeNullOrEmpty())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Is.Not.Empty)
                                 Expected: not null and not <empty>
                                 But was:  null
                               
                               """);
    }

    [Test]
    public void ShouldNotBeNullOrEmpty_ShouldFailForEmptyEnumerable()
    {
        var enumerable = Enumerable.Empty<int>();
        ShouldThrowAssertionException(() => enumerable.ShouldNotBeNullOrEmpty())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Is.Not.Empty)
                                 Expected: not null and not <empty>
                                 But was:  <empty>
                               
                               """);
    }

    [Test]
    public void ShouldNotBeEmpty_ShouldPassForNonEmptyEnumerable()
    {
        var enumerable = Enumerable.Repeat(1, 1);
        var result = enumerable.ShouldNotBeEmpty();
        result.ShouldBeSameAs(enumerable);
    }

    [Test]
    public void ShouldNotBeEmpty_ShouldFailForNullEnumerable()
    {
        IEnumerable<int>? enumerable = null;
        ShouldThrowAssertionException(() => enumerable.ShouldNotBeEmpty())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Is.Not.Empty)
                                 Expected: not null and not <empty>
                                 But was:  null
                               
                               """);
    }

    [Test]
    public void ShouldNotBeEmpty_ShouldFailForEmptyEnumerable()
    {
        var enumerable = Enumerable.Empty<int>();
        ShouldThrowAssertionException(() => enumerable.ShouldNotBeEmpty())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Is.Not.Empty)
                                 Expected: not null and not <empty>
                                 But was:  <empty>
                               
                               """);
    }

    [Test]
    public void ShouldHaveCount_ShouldPassWhenCountMatches()
    {
        int[] array = [1, 2, 3, 4, 5];
        var result = array.ShouldHaveCount(5);
        result.ShouldBeSameAs(array);
    }

    [Test]
    public void ShouldHaveCount_ShouldFailWhenCountNotMatches()
    {
        var enumerable = Enumerable.Empty<int>();
        ShouldThrowAssertionException(() => enumerable.ShouldHaveCount(1))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Has.Exactly(1).Items)
                                 Expected: exactly one item
                                 But was:  no matching items

                               """);
    }

    [Test]
    public void ShouldHaveCount_ShouldFailForNullSource()
    {
        IEnumerable<int>? enumerable = null;
        ShouldThrowAssertionException(() => enumerable.ShouldHaveCount(10))
            .Message.FirstLine(trim: true).ShouldBe($"Assert.That({nameof(enumerable)}, Is.Not.Null)");
    }

    [Test]
    public void ShouldHaveSingle_WithArray_ShouldPassForArrayWithOneItem()
    {
        var item = "The Item";
        var array = new[] { item };
        var result = array.ShouldHaveSingle();
        result.ShouldBeSameAs(item);
    }

    [Test]
    public void ShouldHaveSingle_WithArray_ShouldFailForEmptyArray()
    {
        var array = Array.Empty<int>();
        ShouldThrowAssertionException(() => array.ShouldHaveSingle())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(array)}, Has.Exactly(1).Items)
                                 Expected: exactly one item
                                 But was:  no matching items

                               """);
    }

    [Test]
    public void ShouldHaveSingle_WithArray_ShouldFailForArrayWithMultipleItems()
    {
        var array = new[] { 1, 2 };
        ShouldThrowAssertionException(() => array.ShouldHaveSingle())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(array)}, Has.Exactly(1).Items)
                                 Expected: exactly one item
                                 But was:  2 matching items < 1, 2 >

                               """);
    }

    [Test]
    public void ShouldHaveSingle_WithArray_ShouldFailForNull()
    {
        int[]? array = null;
        ShouldThrowAssertionException(() => array.ShouldHaveSingle())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(array)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }

    [Test]
    public void ShouldHaveSingle_WithEnumerable_ShouldPassForEnumerableWithOneItem()
    {
        var item = "The Item";
        var enumerable = Enumerable.Repeat(item, 1);
        var result = enumerable.ShouldHaveSingle();
        result.ShouldBeSameAs(item);
    }

    [Test]
    public void ShouldHaveSingle_WithEnumerable_ShouldFailForEmptyEnumerable()
    {
        var enumerable = Enumerable.Empty<int>();
        ShouldThrowAssertionException(() => enumerable.ShouldHaveSingle())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Has.Exactly(1).Items)
                                 Expected: exactly one item
                                 But was:  no matching items

                               """);
    }

    [Test]
    public void ShouldHaveSingle_WithEnumerable_ShouldFailForEnumerableWithMultipleItems()
    {
        var enumerable = Enumerable.Range(1, 10);
        ShouldThrowAssertionException(() => enumerable.ShouldHaveSingle())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Has.Exactly(1).Items)
                                 Expected: exactly one item
                                 But was:  more than one matching items < 1, 2 >

                               """);
    }

    [Test]
    public void ShouldHaveSingle_WithEnumerable_ShouldFailForNull()
    {
        IEnumerable<int>? enumerable = null;
        ShouldThrowAssertionException(() => enumerable.ShouldHaveSingle())
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }

    [Test]
    public void ShouldHaveSingle_WithEnumerable_ShouldIncludeCustomMessage()
    {
        var enumerable = Enumerable.Range(1, 10);
        ShouldThrowAssertionException(() => enumerable.ShouldHaveSingle("Custom message"))
            .Message.FirstLine(trim: true).ShouldBe("Custom message");
    }

    [Test]
    public void ShouldHaveSingle_WithPredicate_ShouldPassForArrayWithOneMatchingItem()
    {
        var item = "The Item";
        var array = new[] { "a", "b", item, "C" };
        var result = array.ShouldHaveSingle(x => x.Length > 3);
        result.ShouldBeSameAs(item);
    }

    [Test]
    public void ShouldHaveSingle_WithPredicate_ShouldFailNoMatchingItemsFound()
    {
        var enumerable = Enumerable.Empty<int>();
        ShouldThrowAssertionException(() => enumerable.ShouldHaveSingle(x => x > 0))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Has.Exactly(1).Item.Matching(x => x > 0))
                                 Expected: exactly one item
                                 But was:  no matching items

                               """);
    }

    [Test]
    public void ShouldHaveSingle_WithPredicate_ShouldFailForEnumerableWithMultipleItems()
    {
        var enumerable = Enumerable.Range(1, 10);
        ShouldThrowAssertionException(() => enumerable.ShouldHaveSingle(x => x > 1))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Has.Exactly(1).Item.Matching(x => x > 1))
                                 Expected: exactly one item
                                 But was:  more than one matching items < 2, 3 >

                               """);
    }

    [Test]
    public void ShouldHaveSingle_WithPredicate_ShouldFailForNull()
    {
        IEnumerable<int>? enumerable = null;
        ShouldThrowAssertionException(() => enumerable.ShouldHaveSingle(x => x > 0))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }

    [Test]
    public void ShouldHaveSingle_WithPredicate_ShouldIncludeCustomMessage()
    {
        var enumerable = Enumerable.Range(1, 10);
        ShouldThrowAssertionException(() => enumerable.ShouldHaveSingle(x => x > 1, "Custom message"))
            .Message.FirstLine(trim: true).ShouldBe("Custom message");
    }

    [Test]
    public void ShouldContain_ShouldPassWhenSequenceHasMatchingItems()
    {
        var array = new[] { 1, 2, 3 };
        var result = array.ShouldContain(x => x % 2 == 0);
        result.ShouldBeSameAs(array);
    }

    [Test]
    public void ShouldContain_ShouldFailWhenNoMatchingItemsFound()
    {
        var enumerable = Enumerable.Empty<int>();
        ShouldThrowAssertionException(() => enumerable.ShouldContain(x => x > 0))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Has.Items.Matching(x => x > 0))
                                 Expected: has matching items
                                 But was:  no matching items

                               """);
    }

    [Test]
    public void ShouldContain_ShouldFailForNull()
    {
        IEnumerable<int>? enumerable = null;
        ShouldThrowAssertionException(() => enumerable.ShouldContain(x => x > 0))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }

    [Test]
    public void ShouldContain_ShouldIncludeCustomMessage()
    {
        var enumerable = Enumerable.Empty<int>();
        ShouldThrowAssertionException(() => enumerable.ShouldContain(x => x > 0, "Custom message"))
            .Message.FirstLine(trim: true).ShouldBe("Custom message");
    }

    [Test]
    public void ShouldNotContain_ShouldPassWhenSequenceHasNoMatchingItems()
    {
        var array = new[] { 1, 3, 5 };
        var result = array.ShouldNotContain(x => x % 2 == 0);
        result.ShouldBeSameAs(array);
    }


    [Test]
    public void ShouldNotContain_ShouldFailWhenMatchingItemFound()
    {
        var enumerable = Enumerable.Range(1, 10);
        ShouldThrowAssertionException(() => enumerable.ShouldNotContain(x => x > 0))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Has.No.Items.Matching(x => x > 0))
                                 Expected: has no matching items
                                 But was:  matching item < 1 >

                               """);
    }

    [Test]
    public void ShouldNotContain_ShouldFailForNull()
    {
        IEnumerable<int>? enumerable = null;
        ShouldThrowAssertionException(() => enumerable.ShouldNotContain(x => x > 0))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }

    [Test]
    public void ShouldNotContain_ShouldIncludeCustomMessage()
    {
        var enumerable = Enumerable.Range(1, 10);
        ShouldThrowAssertionException(() => enumerable.ShouldNotContain(x => x > 0, "Custom message"))
            .Message.FirstLine(trim: true).ShouldBe("Custom message");
    }

    [Test]
    public void ShouldContainItem_ShouldPassAndReturnSourceWhenItemIsPresent()
    {
        var list = new List<int> { 1, 2, 3 };
        List<int> result = list.ShouldContain(2);
        result.ShouldBeSameAs(list);
    }

    [Test]
    public void ShouldContainItem_ShouldPassForItemOfBaseType()
    {
        var items = new List<object> { 1, "two" };
        items.ShouldContain("two");
    }

    [Test]
    public void ShouldContainItem_ShouldFailWhenItemIsMissing()
    {
        var list = new List<int> { 1, 2, 3 };
        ShouldThrowAssertionException(() => list.ShouldContain(4))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(list)}, Has.Member(4))
                                 Expected: collection containing 4
                                 But was:  [1, 2, 3]

                               """);
    }

    [Test]
    public void ShouldContainItem_ShouldFailForEmptyCollection()
    {
        var array = Array.Empty<int>();
        ShouldThrowAssertionException(() => array.ShouldContain(1))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(array)}, Has.Member(1))
                                 Expected: collection containing 1
                                 But was:  <empty>

                               """);
    }

    [Test]
    public void ShouldContainItem_ShouldFailForNull()
    {
        int[]? array = null;
        ShouldThrowAssertionException(() => array.ShouldContain(1))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(array)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }

    [Test]
    public void ShouldContainItem_ShouldIncludeCustomMessage()
    {
        var array = new[] { 1, 2, 3 };
        ShouldThrowAssertionException(() => array.ShouldContain(4, "Custom message"))
            .Message.FirstLine(trim: true).ShouldBe("Custom message");
    }

    [Test]
    public void ShouldContainItem_WithComparer_ShouldPassWhenComparerFindsItem()
    {
        var names = new[] { "alice", "bob" };
        var result = names.ShouldContain("BOB", StringComparer.OrdinalIgnoreCase);
        result.ShouldBeSameAs(names);
    }

    [Test]
    public void ShouldContainItem_WithComparer_ShouldFailWhenComparerFindsNoItem()
    {
        var names = new[] { "alice", "bob" };
        ShouldThrowAssertionException(() => names.ShouldContain("BOB", StringComparer.Ordinal))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(names)}, Has.Member("BOB").Using(StringComparer.Ordinal))
                                 Expected: collection containing BOB
                                 But was:  [alice, bob]

                               """);
    }

    [Test]
    public void ShouldNotContainItem_ShouldPassAndReturnSourceWhenItemIsAbsent()
    {
        var list = new List<int> { 1, 2, 3 };
        List<int> result = list.ShouldNotContain(4);
        result.ShouldBeSameAs(list);
    }

    [Test]
    public void ShouldNotContainItem_ShouldPassForAbsentItemOfDerivedType()
    {
        var items = new List<object> { 1, "two" };
        items.ShouldNotContain("three");
    }

    [Test]
    public void ShouldNotContainItem_ShouldFailWhenItemIsPresent()
    {
        var list = new List<int> { 1, 2, 3 };
        ShouldThrowAssertionException(() => list.ShouldNotContain(2))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(list)}, Has.No.Member(2))
                                 Expected: collection not containing 2
                                 But was:  [1, 2, 3]

                               """);
    }

    [Test]
    public void ShouldNotContainItem_ShouldFailForNull()
    {
        int[]? array = null;
        ShouldThrowAssertionException(() => array.ShouldNotContain(1))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(array)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }

    [Test]
    public void ShouldNotContainItem_ShouldIncludeCustomMessage()
    {
        var array = new[] { 1, 2, 3 };
        ShouldThrowAssertionException(() => array.ShouldNotContain(1, "Custom message"))
            .Message.FirstLine(trim: true).ShouldBe("Custom message");
    }

    [Test]
    public void ShouldNotContainItem_WithComparer_ShouldFailWhenComparerFindsItem()
    {
        var names = new[] { "alice", "bob" };
        ShouldThrowAssertionException(() => names.ShouldNotContain("BOB", StringComparer.OrdinalIgnoreCase))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(names)}, Has.No.Member("BOB").Using(StringComparer.OrdinalIgnoreCase))
                                 Expected: collection not containing BOB
                                 But was:  [alice, bob]

                               """);
    }

    [Test]
    public void ShouldNotContainItem_WithComparer_ShouldPassWhenComparerFindsNoItem()
    {
        var names = new[] { "alice", "bob" };
        names.ShouldNotContain("BOB", StringComparer.Ordinal);
    }

    [Test]
    public void ShouldAllSatisfy_ShouldPassAndReturnSourceWhenAllItemsSatisfyAssertion()
    {
        var list = new List<int> { 1, 2, 3 };
        var result = list.ShouldAllSatisfy(x => x.ShouldBeGreaterThan(0));
        result.ShouldBeSameAs(list);
    }

    [Test]
    public void ShouldAllSatisfy_WithTypedLambda_ShouldPreserveCollectionType()
    {
        var list = new List<int> { 1, 2, 3 };
        List<int> result = list.ShouldAllSatisfy((int x) => x.ShouldBeGreaterThan(0));
        result.ShouldBeSameAs(list);
    }

    [Test]
    public void ShouldAllSatisfy_ShouldPassForEmptyCollection()
    {
        var array = Array.Empty<int>();
        array.ShouldAllSatisfy(x => x.ShouldBeGreaterThan(0));
    }

    [Test]
    public void ShouldAllSatisfy_ShouldFailListingEachItemThatFails()
    {
        var numbers = new List<int> { 1, 2, 3 };
        ShouldThrowAssertionException(() => numbers.ShouldAllSatisfy(x => x.ShouldBeLessThan(2)))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(numbers)}, Has.All.Satisfying(x => x.ShouldBeLessThan(2)))
                                 2 of 3 items don't satisfy the assertion.
                                 [1] 2:
                                   Assert.That(x, Is.LessThan(2))
                                   Expected: less than 2
                                   But was:  2
                                 [2] 3:
                                   Assert.That(x, Is.LessThan(2))
                                   Expected: less than 2
                                   But was:  3

                               """);
    }

    [Test]
    public void ShouldAllSatisfy_ShouldReportExceptionsThrownByAssertion()
    {
        var numbers = new[] { 1, 2 };
        ShouldThrowAssertionException(() => numbers.ShouldAllSatisfy(x => ThrowIfEven(x)))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(numbers)}, Has.All.Satisfying(x => ThrowIfEven(x)))
                                 1 of 2 items don't satisfy the assertion.
                                 [1] 2:
                                   <System.InvalidOperationException: 2 is even.>

                               """);
    }

    private static void ThrowIfEven(int number)
    {
        if (number % 2 == 0) throw new InvalidOperationException($"{number} is even.");
    }

    [Test]
    public void ShouldAllSatisfy_ShouldLetIgnoreExceptionThrough()
    {
        var numbers = new[] { 1, 2 };
        ShouldThrow<IgnoreException>(() => numbers.ShouldAllSatisfy(x => IgnoreIfEven(x)))
            .Message.ShouldBe("2 is even.");
    }

    private static void IgnoreIfEven(int number)
    {
        if (number % 2 == 0) Assert.Ignore($"{number} is even.");
    }

    [Test]
    public void ShouldAllSatisfy_ShouldReportOnlyFirstTenFailures()
    {
        var numbers = Enumerable.Range(1, 12).ToArray();
        var message = ShouldThrowAssertionException(() => numbers.ShouldAllSatisfy(x => x.ShouldBe(0))).Message;

        message.ShouldContain("12 of 12 items don't satisfy the assertion.");
        message.ShouldContain("[9] 10:");
        message.ShouldNotContain("[10] 11:");
        message.ShouldContain("... and 2 more.");
    }

    [Test]
    public void ShouldAllSatisfy_ShouldReportItemFailuresInsideAssertMultiple()
    {
        var numbers = new[] { 1, 2 };
        var exception = ShouldThrow<MultipleAssertException>(() => Assert.Multiple(() => numbers.ShouldAllSatisfy(x => x.ShouldBe(1))));
        exception.Message.ShouldContain("1 of 2 items don't satisfy the assertion.");
    }

    [Test]
    public void ShouldAllSatisfy_ShouldFailForNull()
    {
        int[]? array = null;
        ShouldThrowAssertionException(() => array.ShouldAllSatisfy(x => x.ShouldBe(1)))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(array)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }

    [Test]
    public void ShouldAllSatisfy_ShouldIncludeCustomMessage()
    {
        var array = new[] { 1, 2 };
        ShouldThrowAssertionException(() => array.ShouldAllSatisfy(x => x.ShouldBe(1), "Custom message"))
            .Message.FirstLine(trim: true).ShouldBe("Custom message");
    }

    [Test]
    public void ShouldBeSequenceEqualTo_ShouldPassForEqualSequences()
    {
        var enumerable = Enumerable.Range(1, 3);
        var result = enumerable.ShouldBeSequenceEqualTo([1, 2, 3]);
        result.ShouldBeSameAs(enumerable);
    }

    [Test]
    public void ShouldBeSequenceEqualTo_ShouldPreserveCollectionType()
    {
        var list = new List<int> { 1, 2, 3 };

        List<int> result = list.ShouldBeSequenceEqualTo([1, 2, 3]);
        result.ShouldBeSameAs(list);

        List<int> emptyList = [];
        List<int> emptyResult = emptyList.ShouldBeSequenceEqualTo([], EqualityComparer<int>.Default);
        emptyResult.ShouldBeSameAs(emptyList);

        List<int> resultWithComparer = list.ShouldBeSequenceEqualTo([1, 2, 3], EqualityComparer<int>.Default);
        resultWithComparer.ShouldBeSameAs(list);

        List<int> resultWithPredicate = list.ShouldBeSequenceEqualTo(["1", "2", "3"], (int a, string e) => a.ToString() == e);
        resultWithPredicate.ShouldBeSameAs(list);
    }

    [Test]
    public void ShouldBeSequenceEqualTo_ShouldPassForEmptySequences()
    {
        Enumerable.Empty<int>().ShouldBeSequenceEqualTo([]);
    }

    [Test]
    public void ShouldBeSequenceEqualTo_ShouldFailWhenItemsDiffer()
    {
        var actual = new[] { 1, 2, 3 };
        var expected = new[] { 1, 5, 3 };
        ShouldThrowAssertionException(() => actual.ShouldBeSequenceEqualTo(expected))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.EqualTo({nameof(expected)}))
                                 Sequence lengths are both 3. Sequences differ at index 1.
                                 Expected: [..., 5, 3]
                                 But was:  [..., 2, 3]

                               """);
    }

    [Test]
    public void ShouldBeSequenceEqualTo_ShouldFailWhenActualIsLonger()
    {
        var actual = new[] { 1, 2, 3, 4 };
        var expected = new[] { 1, 2, 3 };
        ShouldThrowAssertionException(() => actual.ShouldBeSequenceEqualTo(expected))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.EqualTo({nameof(expected)}))
                                 Expected sequence length 3 but was 4. Sequences differ at index 3.
                                 Expected: [...]
                                 But was:  [..., 4]

                               """);
    }

    [Test]
    public void ShouldBeSequenceEqualTo_ShouldFailWhenActualIsShorter()
    {
        var actual = new[] { 1, 2 };
        var expected = new[] { 1, 2, 3 };
        ShouldThrowAssertionException(() => actual.ShouldBeSequenceEqualTo(expected))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.EqualTo({nameof(expected)}))
                                 Expected sequence length 3 but was 2. Sequences differ at index 2.
                                 Expected: [..., 3]
                                 But was:  [...]

                               """);
    }

    [Test]
    public void ShouldBeSequenceEqualTo_ShouldIncludeCustomMessage()
    {
        var actual = new[] { 1 };
        var expected = new[] { 2 };
        ShouldThrowAssertionException(() => actual.ShouldBeSequenceEqualTo(expected, "Custom message"))
            .Message.FirstLine(trim: true).ShouldBe("Custom message");
    }

    [Test]
    public void ShouldBeSequenceEqualTo_ShouldFailForNull()
    {
        IEnumerable<int>? enumerable = null;
        ShouldThrowAssertionException(() => enumerable.ShouldBeSequenceEqualTo([1]))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(enumerable)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }

    [Test]
    public void ShouldBeSequenceEqualTo_WithComparer_ShouldPassWhenComparerMatches()
    {
        var actual = new[] { new TestClass { Id = 1, Name = "a" }, new TestClass { Id = 2, Name = "b" } };
        var expected = new[] { new TestClass { Id = 10, Name = "a" }, new TestClass { Id = 20, Name = "b" } };
        actual.ShouldBeSequenceEqualTo(expected, new TestClassComparerByName());
    }

    [Test]
    public void ShouldBeSequenceEqualTo_WithComparer_ShouldFailWhenComparerDoesNotMatch()
    {
        var actual = new[] { "a", "b" };
        var expected = new[] { "A", "C" };
        ShouldThrowAssertionException(() => actual.ShouldBeSequenceEqualTo(expected, StringComparer.OrdinalIgnoreCase))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.EqualTo({nameof(expected)}).Using(StringComparer.OrdinalIgnoreCase))
                                 Sequence lengths are both 2. Sequences differ at index 1.
                                 Expected: [..., C]
                                 But was:  [..., b]

                               """);
    }

    [Test]
    public void ShouldBeSequenceEqualTo_WithPredicate_ShouldPassForMatchingItems()
    {
        var actual = new[] { new TestClass { Id = 1 }, new TestClass { Id = 2 } };
        actual.ShouldBeSequenceEqualTo([1, 2], (a, e) => a.Id == e);
    }

    [Test]
    public void ShouldBeSequenceEqualTo_WithPredicate_ShouldFailForNotMatchingItems()
    {
        var actual = new[] { 1, 2, 3 };
        var expected = new[] { "1", "2", "4" };
        ShouldThrowAssertionException(() => actual.ShouldBeSequenceEqualTo(expected, (a, e) => a.ToString() == e))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.EqualTo({nameof(expected)}).Using((a, e) => a.ToString() == e))
                                 Sequence lengths are both 3. Sequences differ at index 2.
                                 Expected: [..., 4]
                                 But was:  [..., 3]

                               """);
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldPassForSameItemsInDifferentOrder()
    {
        var set = new HashSet<int> { 1, 2, 3 };
        HashSet<int> result = set.ShouldBeSetEqualTo([3, 1, 2]);
        result.ShouldBeSameAs(set);
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldIgnoreDuplicatesInExpected()
    {
        var set = new HashSet<int> { 1, 2 };
        set.ShouldBeSetEqualTo([2, 1, 2, 1]);
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldPassForEmptySets()
    {
        new HashSet<int>().ShouldBeSetEqualTo([]);
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldUseComparersOfSets()
    {
        var actual = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "a", "b" };
        var expected = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "B", "A" };
        actual.ShouldBeSetEqualTo(expected);
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldCompareSequenceWithComparerOfActualSet()
    {
        var actual = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "A" };
        actual.ShouldBeSetEqualTo(["a"]);
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldCompareSequenceWithComparerOfExpectedSet()
    {
        var actual = new[] { "a", "A" };
        var expected = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "A" };
        actual.ShouldBeSetEqualTo(expected);
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldCompareSequenceWithComparerOfActualFrozenSet()
    {
        var actual = new[] { "A" }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);
        actual.ShouldBeSetEqualTo(["a"]);
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldCompareSequenceWithComparerOfActualImmutableSet()
    {
        var actual = ImmutableHashSet.Create(StringComparer.OrdinalIgnoreCase, "A");
        actual.ShouldBeSetEqualTo(["a"]);
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldUseComparerOfActualWhenBothSetsHaveComparers()
    {
        var actual = new HashSet<string>(StringComparer.Ordinal) { "a", "A" };
        var expected = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "A" };
        ShouldThrowAssertionException(() => actual.ShouldBeSetEqualTo(expected))
            .Message.ShouldContain("Extra items: [a]");
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldFailWhenItemsAreMissing()
    {
        var actual = new HashSet<int> { 1, 2 };
        var expected = new[] { 1, 2, 3 };
        ShouldThrowAssertionException(() => actual.ShouldBeSetEqualTo(expected))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.SetEqualTo({nameof(expected)}))
                                 Missing items: [3]
                                 Expected: [1, 2, 3]
                                 But was:  [1, 2]

                               """);
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldFailWhenThereAreExtraItems()
    {
        var actual = new HashSet<int> { 1, 2, 3 };
        var expected = new[] { 1, 2 };
        ShouldThrowAssertionException(() => actual.ShouldBeSetEqualTo(expected))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.SetEqualTo({nameof(expected)}))
                                 Extra items: [3]
                                 Expected: [1, 2]
                                 But was:  [1, 2, 3]

                               """);
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldFailWhenItemsAreMissingAndExtra()
    {
        var actual = new HashSet<int> { 1, 2, 4 };
        var expected = new[] { 1, 2, 3 };
        ShouldThrowAssertionException(() => actual.ShouldBeSetEqualTo(expected))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.SetEqualTo({nameof(expected)}))
                                 Missing items: [3]
                                 Extra items: [4]
                                 Expected: [1, 2, 3]
                                 But was:  [1, 2, 4]

                               """);
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldFailWhenActualIsEmpty()
    {
        var actual = new HashSet<int>();
        var expected = new[] { 1 };
        ShouldThrowAssertionException(() => actual.ShouldBeSetEqualTo(expected))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.SetEqualTo({nameof(expected)}))
                                 Missing items: [1]
                                 Expected: [1]
                                 But was:  <empty>

                               """);
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldIncludeCustomMessage()
    {
        var actual = new HashSet<int> { 1 };
        ShouldThrowAssertionException(() => actual.ShouldBeSetEqualTo([2], "Custom message"))
            .Message.FirstLine(trim: true).ShouldBe("Custom message");
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldFailForNull()
    {
        HashSet<int>? set = null;
        ShouldThrowAssertionException(() => set.ShouldBeSetEqualTo([1]))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(set)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }

    [Test]
    public void ShouldBeSetEqualTo_WithComparer_ShouldPassWhenComparerMatches()
    {
        var set = new HashSet<string> { "a", "b" };
        HashSet<string> result = set.ShouldBeSetEqualTo(["B", "A"], StringComparer.OrdinalIgnoreCase);
        result.ShouldBeSameAs(set);
    }

    [Test]
    public void ShouldBeSetEqualTo_WithComparer_ShouldFailWhenComparerDoesNotMatch()
    {
        var actual = new HashSet<string> { "a", "b" };
        var expected = new[] { "A", "C" };
        ShouldThrowAssertionException(() => actual.ShouldBeSetEqualTo(expected, StringComparer.OrdinalIgnoreCase))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.SetEqualTo({nameof(expected)}).Using(StringComparer.OrdinalIgnoreCase))
                                 Missing items: [C]
                                 Extra items: [b]
                                 Expected: [A, C]
                                 But was:  [a, b]

                               """);
    }

    [Test]
    public void ShouldBeMultisetEqualTo_ShouldPassForSameItemsInDifferentOrder()
    {
        var list = new List<int> { 1, 2, 2, 3 };
        List<int> result = list.ShouldBeMultisetEqualTo([2, 3, 1, 2]);
        result.ShouldBeSameAs(list);
    }

    [Test]
    public void ShouldBeMultisetEqualTo_ShouldPassForEmptyCollections()
    {
        new List<int>().ShouldBeMultisetEqualTo([]);
    }

    [Test]
    public void ShouldBeMultisetEqualTo_ShouldPassForNullItems()
    {
        var actual = new[] { null, "a", null };
        actual.ShouldBeMultisetEqualTo(["a", null, null]);
    }

    [Test]
    public void ShouldBeMultisetEqualTo_WithComparer_ShouldPassForNullItems()
    {
        var actual = new[] { null, "A" };
        actual.ShouldBeMultisetEqualTo(["a", null], StringComparer.OrdinalIgnoreCase);
    }

    [Test]
    [MaxTime(5000)]
    public void ShouldBeMultisetEqualTo_ShouldPassForLargeCollectionsInReverseOrderWithinFiveSeconds()
    {
        var actual = Enumerable.Range(0, 100_000).ToArray();
        actual.ShouldBeMultisetEqualTo(actual.Reverse());
    }

    [Test]
    public void ShouldBeMultisetEqualTo_ShouldListMissingItemsInExpectedOrder()
    {
        var actual = new[] { 1 };
        var expected = new[] { 3, 1, 2, 3 };
        ShouldThrowAssertionException(() => actual.ShouldBeMultisetEqualTo(expected))
            .Message.ShouldContain("Missing items: [3, 2, 3]");
    }

    [Test]
    public void ShouldBeMultisetEqualTo_ShouldFailWhenDuplicateIsMissing()
    {
        var actual = new[] { 1, 2 };
        var expected = new[] { 2, 1, 2 };
        ShouldThrowAssertionException(() => actual.ShouldBeMultisetEqualTo(expected))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.EquivalentTo({nameof(expected)}))
                                 Missing items: [2]
                                 Expected: [2, 1, 2]
                                 But was:  [1, 2]

                               """);
    }

    [Test]
    public void ShouldBeMultisetEqualTo_ShouldFailWhenThereIsAnExtraDuplicate()
    {
        var actual = new[] { 1, 1, 2 };
        var expected = new[] { 2, 1 };
        ShouldThrowAssertionException(() => actual.ShouldBeMultisetEqualTo(expected))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.EquivalentTo({nameof(expected)}))
                                 Extra items: [1]
                                 Expected: [2, 1]
                                 But was:  [1, 1, 2]

                               """);
    }

    [Test]
    public void ShouldBeMultisetEqualTo_ShouldFailWhenItemsAreMissingAndExtra()
    {
        var actual = new[] { 1, 2, 4 };
        var expected = new[] { 3, 2, 1 };
        ShouldThrowAssertionException(() => actual.ShouldBeMultisetEqualTo(expected))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.EquivalentTo({nameof(expected)}))
                                 Missing items: [3]
                                 Extra items: [4]
                                 Expected: [3, 2, 1]
                                 But was:  [1, 2, 4]

                               """);
    }

    [Test]
    public void ShouldBeMultisetEqualTo_ShouldFailWhenActualIsEmpty()
    {
        var actual = new List<int>();
        var expected = new[] { 1 };
        ShouldThrowAssertionException(() => actual.ShouldBeMultisetEqualTo(expected))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.EquivalentTo({nameof(expected)}))
                                 Missing items: [1]
                                 Expected: [1]
                                 But was:  <empty>

                               """);
    }

    [Test]
    public void ShouldBeMultisetEqualTo_ShouldIncludeCustomMessage()
    {
        var actual = new[] { 1 };
        ShouldThrowAssertionException(() => actual.ShouldBeMultisetEqualTo([2], "Custom message"))
            .Message.FirstLine(trim: true).ShouldBe("Custom message");
    }

    [Test]
    public void ShouldBeMultisetEqualTo_ShouldFailForNull()
    {
        int[]? items = null;
        ShouldThrowAssertionException(() => items.ShouldBeMultisetEqualTo([1]))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(items)}, Is.Not.Null)
                                 Expected: not null
                                 But was:  null

                               """);
    }

    [Test]
    public void ShouldBeMultisetEqualTo_WithComparer_ShouldPassWhenComparerMatches()
    {
        var list = new List<string> { "a", "b", "a" };
        List<string> result = list.ShouldBeMultisetEqualTo(["A", "B", "A"], StringComparer.OrdinalIgnoreCase);
        result.ShouldBeSameAs(list);
    }

    [Test]
    public void ShouldBeMultisetEqualTo_WithComparer_ShouldFailWhenComparerDoesNotMatch()
    {
        var actual = new[] { "a", "b" };
        var expected = new[] { "A", "C" };
        ShouldThrowAssertionException(() => actual.ShouldBeMultisetEqualTo(expected, StringComparer.OrdinalIgnoreCase))
            .Message.ShouldBe($"""
                                 Assert.That({nameof(actual)}, Is.EquivalentTo({nameof(expected)}).Using(StringComparer.OrdinalIgnoreCase))
                                 Missing items: [C]
                                 Extra items: [b]
                                 Expected: [A, C]
                                 But was:  [a, b]

                               """);
    }

    [Test]
    public void ShouldBeSequenceEqualTo_ShouldThrowArgumentNullExceptionForNullExpected()
    {
        int[] nullSequence = null!;
        ShouldThrow<ArgumentNullException>(() => new[] { 1 }.ShouldBeSequenceEqualTo(nullSequence))
            .ParamName.ShouldBe("expected");
    }

    [Test]
    public void ShouldBeSetEqualTo_ShouldThrowArgumentNullExceptionForNullExpected()
    {
        int[] nullSequence = null!;
        ShouldThrow<ArgumentNullException>(() => new[] { 1 }.ShouldBeSetEqualTo(nullSequence))
            .ParamName.ShouldBe("expected");
    }

    [Test]
    public void ShouldBeMultisetEqualTo_ShouldThrowArgumentNullExceptionForNullExpected()
    {
        int[] nullSequence = null!;
        ShouldThrow<ArgumentNullException>(() => new[] { 1 }.ShouldBeMultisetEqualTo(nullSequence))
            .ParamName.ShouldBe("expected");
    }
}