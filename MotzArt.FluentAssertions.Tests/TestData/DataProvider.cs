namespace MotzArt.FluentAssertions.Tests.TestData;

class DataProvider
{
    public const string DefaultTestClassName = "Default";
    
    private static TestClass? _defaultTestClass;
    public static TestClass DefaultTestClass => _defaultTestClass ??= new TestClass { Id = 1, Name = DefaultTestClassName };
    
    private static TestClass? _anotherTestClass;
    public static TestClass AnotherTestClass => _anotherTestClass ??= new TestClass { Id = 2, Name = "Another" };

    public static TestClass CreateTestClass(string name = DefaultTestClassName)
    {
        return CreateTestClass(IdGenerator.NextId(), name);
    }

    public static TestClass CreateTestClass(int id, string name = DefaultTestClassName)
    {
        return new TestClass { Id = id, Name = name };
    }
}