using MotzArt.FluentAssertions.Helpers;
using System.Text;

namespace MotzArt.FluentAssertions.Tests.Helpers;

[TestFixture]
public class DescribeValuesExtensionsTests
{
    [Test]
    public void DescribeValue_ShouldDescribeNullAsNull()
    {
        ((object?)null).DescribeValue().ShouldBe("null");
    }

    [Test]
    public void DescribeValue_ShouldDescribeNullNullableAsNull()
    {
        ((int?)null).DescribeValue().ShouldBe("null");
    }

    [Test]
    public void DescribeValue_ShouldDescribeStringAsItIs()
    {
        "some \"text\"".DescribeValue().ShouldBe("some \"text\"");
    }

    [TestCase('a', "'a'")]
    [TestCase('\\', @"'\\'")]
    [TestCase('\'', @"'\''")]
    [TestCase('\t', @"'\t'")]
    [TestCase('\r', @"'\r'")]
    [TestCase('\n', @"'\n'")]
    [TestCase('\0', @"'\0'")]
    public void DescribeValue_ShouldDescribeCharAsCSharpLiteral(char value, string expected)
    {
        value.DescribeValue().ShouldBe(expected);
    }

    public static IEnumerable<TestCaseData> Numbers =>
    [
        new TestCaseData((byte)1, "1"),
        new TestCaseData((sbyte)-1, "-1"),
        new TestCaseData((short)-2, "-2"),
        new TestCaseData((ushort)2, "2"),
        new TestCaseData(-3, "-3"),
        new TestCaseData(3u, "3u"),
        new TestCaseData(-4L, "-4L"),
        new TestCaseData(4UL, "4UL"),
        new TestCaseData(1.5f, "1.5f"),
        new TestCaseData(-2.5d, "-2.5d"),
        new TestCaseData(3.25m, "3.25m"),
    ];

    [TestCaseSource(nameof(Numbers))]
    public void DescribeValue_ShouldDescribeNumberWithCSharpSuffix(object value, string expected)
    {
        value.DescribeValue().ShouldBe(expected);
    }

    [Test]
    [SetCulture("de-DE")]
    public void DescribeValue_ShouldDescribeNumberInInvariantCulture()
    {
        1.5d.DescribeValue().ShouldBe("1.5d");
    }

    [Test]
    public void DescribeValue_ShouldUseOverriddenToString()
    {
        new Point(1, 2).DescribeValue().ShouldBe("Point { X = 1, Y = 2 }");
    }

    [Test]
    public void DescribeValue_ShouldDescribeValueWithoutOverriddenToStringAsJson()
    {
        new PlainValue { Name = "a", Count = 1 }.DescribeValue().ShouldBe("""{"Name":"a","Count":1}""");
    }

    [Test]
    public void DescribeValue_OfSpan_ShouldDescribeEmptySpanWithItsType()
    {
        ReadOnlySpan<int>.Empty.DescribeValue().ShouldBe("empty <ReadOnlySpan<Int32>>");
    }

    [Test]
    public void DescribeValue_OfSpan_ShouldDescribeAllItems()
    {
        new ReadOnlySpan<int>([1, 2, 3]).DescribeValue().ShouldBe("[1, 2, 3]");
    }

    [Test]
    public void DescribeValue_OfSpan_ShouldReplaceSkippedItemsWithEllipsis()
    {
        new ReadOnlySpan<int>([1, 2, 3]).DescribeValue(skip: 1).ShouldBe("[..., 2, 3]");
    }

    [Test]
    public void DescribeValue_OfSpan_ShouldCutItemsAfterMaxLength()
    {
        new ReadOnlySpan<string>(["aaa", "bbb", "ccc"]).DescribeValue(maxLength: 5).ShouldBe("[aaa, bbb, ...]");
    }

    [Test]
    public void DescribeValue_OfSpan_ShouldNotAddEllipsisWhenLastItemExceedsMaxLength()
    {
        new ReadOnlySpan<string>(["aaa", "bbb"]).DescribeValue(maxLength: 5).ShouldBe("[aaa, bbb]");
    }

    [TestCase(typeof(int), "Int32")]
    [TestCase(typeof(int?), "Int32?")]
    [TestCase(typeof(int[]), "Int32[]")]
    [TestCase(typeof(int[][]), "Int32[][]")]
    [TestCase(typeof(int[,]), "Int32[,]")]
    [TestCase(typeof(int?[]), "Int32?[]")]
    [TestCase(typeof(List<int>), "List<Int32>")]
    [TestCase(typeof(Dictionary<string, List<int?>>), "Dictionary<String, List<Int32?>>")]
    [TestCase(typeof(Dictionary<int, string>.KeyCollection), "KeyCollection<Int32, String>")]
    public void DescribeType_ShouldDescribeTypeAsInCSharpSource(Type type, string expected)
    {
        type.DescribeType().ShouldBe(expected);
    }

    [TestCase(null, "null")]
    [TestCase("", "\"\"")]
    [TestCase("text", "\"text\"")]
    [TestCase("say \"hi\"", "\"say \\\"hi\\\"\"")]
    [TestCase(@"C:\dir\", "\"C:\\\\dir\\\\\"")]
    public void FormatValue_ShouldQuoteAndEscapeString(string? value, string expected)
    {
        DescribeValuesExtensions.FormatValue(value).ShouldBe(expected);
    }

    [TestCase(null, "null")]
    [TestCase("say \"hi\"", "\"say \\\"hi\\\"\"")]
    public void WriteValue_ShouldAppendQuotedAndEscapedString(string? value, string expected)
    {
        var sb = new StringBuilder("value: ");

        sb.WriteValue(value);

        sb.ToString().ShouldBe("value: " + expected);
    }

    [TestCase(0, "[ 1, ... ]")]
    [TestCase(1, "[ ... , 2, ... ]")]
    [TestCase(2, "[ ... , 3 ]")]
    [TestCase(3, "[ ... ]")]
    public void ShowAtItem_ShouldShowOnlyItemAtIndex(int index, string expected)
    {
        var sb = new StringBuilder();

        sb.ShowAtItem(new ReadOnlySpan<int>([1, 2, 3]), index);

        sb.ToString().ShouldBe(expected);
    }

    [Test]
    public void ShowAtItem_ShouldShowSingleItemWithoutEllipsis()
    {
        var sb = new StringBuilder();

        sb.ShowAtItem(new ReadOnlySpan<int>([1]), 0);

        sb.ToString().ShouldBe("[ 1 ]");
    }

    private sealed record Point(int X, int Y);

    private sealed class PlainValue
    {
        public string? Name { get; init; }

        public int Count { get; init; }
    }
}
