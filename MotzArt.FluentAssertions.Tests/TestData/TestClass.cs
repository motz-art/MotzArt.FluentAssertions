namespace MotzArt.FluentAssertions.Tests.TestData;

class TestClass : IEquatable<TestClass>
{
    public int Id { get; init; }
    public string? Name { get; init; } = string.Empty;

    public bool Equals(TestClass? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((TestClass) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name);
    }

    public static bool operator ==(TestClass? left, TestClass? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TestClass? left, TestClass? right)
    {
        return !Equals(left, right);
    }
}