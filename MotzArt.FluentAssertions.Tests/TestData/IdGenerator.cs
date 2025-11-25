namespace MotzArt.FluentAssertions.Tests.TestData;

public static class IdGenerator
{
    private const int StartValue = 1229;
    private const int Inc = 30;

    private static int _id = StartValue;
    private static string? _lastTestName;

    public static int NextId()
    {
        var testFullName = TestContext.CurrentContext.Test.FullName;
        if (_lastTestName != testFullName)
        {
            _lastTestName = testFullName;
            _id = StartValue;
        }

        var result = _id;
        _id += Inc;

        return result;
    }
}