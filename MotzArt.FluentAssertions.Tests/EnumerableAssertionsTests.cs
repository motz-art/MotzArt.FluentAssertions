using static MotzArt.FluentAssertions.Tests.TestHelpers;
using MotzArt.FluentAssertions.Tests.TestData;

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
                                 But was:  more then one matching items < 1, 2 >

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
                                 But was:  more then one matching items < 2, 3 >

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
    public void ShouldContain_ShouldPassWhenSequenceHasMatchingItems()
    {
        var array = new[] { 1, 2, 3 };
        array.ShouldContain(x => x % 2 == 0);
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
    public void ShouldNotContain_ShouldPassWhenSequenceHasNoMatchingItems()
    {
        var array = new[] { 1, 3, 5 };
        array.ShouldNotContain(x => x % 2 == 0);
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
    public void ShouldBeSequenceEqualTo_ShouldPassForEqualSequences()
    {
        var enumerable = Enumerable.Range(1, 3);
        var result = enumerable.ShouldBeSequenceEqualTo([1, 2, 3]);
        result.ShouldBeSameAs(enumerable);
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
}