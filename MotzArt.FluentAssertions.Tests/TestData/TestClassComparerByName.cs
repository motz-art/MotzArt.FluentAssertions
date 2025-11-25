namespace MotzArt.FluentAssertions.Tests.TestData;

class TestClassComparerByName : IEqualityComparer<TestClass>
{
    public bool Equals(TestClass? x, TestClass? y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;
        return x.Name == y.Name;
    }

    public int GetHashCode(TestClass obj)
    {
        return obj.Name?.GetHashCode() ?? 0;
    }
}