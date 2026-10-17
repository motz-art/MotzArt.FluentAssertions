using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace MotzArt.FluentAssertions;

// Values are stored per NUnit test or suite (test case, fixture, setup fixture, ...) and are visible to all its descendants.
// For example a value set in OneTimeSetUp of a fixture is visible in all tests of the fixture, a value set in a test is visible only in that test (including its SetUp and TearDown).
// Lookups go from the current test up through its parents, so parallel tests never see each other's values.
public static class TestStore
{
    private static readonly ConditionalWeakTable<ITest, ConcurrentDictionary<string, object>> _values = new();

    public static T Set<T>(string key, T value)
    {
        GetCurrentValues()[key] = value!;
        return value;
    }

    public static T GetOrCreate<T>(string key, Func<string, T> create)
    {
        if (TryGet(key, out T value))
        {
            return value;
        }

        return (T)GetCurrentValues().GetOrAdd(key, x => create(x)!);
    }

    public static bool TryGet<T>(string key, out T value)
    {
        for (ITest? test = GetCurrentTest(); test != null; test = test.Parent)
        {
            if (_values.TryGetValue(test, out var values) && values.TryGetValue(key, out var obj))
            {
                value = (T)obj;
                return true;
            }
        }

        value = default!;
        return false;
    }

    // Removes the value only from the current test or suite, values of parents are not affected.
    public static bool Remove(string key)
    {
        return _values.TryGetValue(GetCurrentTest(), out var values) && values.TryRemove(key, out _);
    }

    private static ConcurrentDictionary<string, object> GetCurrentValues()
    {
        return _values.GetValue(GetCurrentTest(), _ => new());
    }

    private static Test GetCurrentTest()
    {
        return TestExecutionContext.CurrentContext.CurrentTest;
    }
}
