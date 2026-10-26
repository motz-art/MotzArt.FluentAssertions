using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace MotzArt.FluentAssertions;

public static class TestStore
{
    private static readonly ConditionalWeakTable<ITest, ConcurrentDictionary<string, object>> _valuesByTestOrSuite = new();

    public static T Set<T>(string key, T value)
    {
        GetOrAddValuesOfCurrentTestOrSuite()[key] = value!;
        return value;
    }

    public static T GetOrCreate<T>(string key, Func<string, T> create)
    {
        if (TryGet(key, out T value))
        {
            return value;
        }

        return (T)GetOrAddValuesOfCurrentTestOrSuite().GetOrAdd(key, x => create(x)!);
    }

    public static bool TryGet<T>(string key, out T value)
    {
        foreach (var testOrSuite in GetCurrentTestOrSuiteAndItsParents())
        {
            if (TryGetValueOfTestOrSuite(testOrSuite, key, out value))
            {
                return true;
            }
        }

        value = default!;
        return false;
    }

    public static bool Remove(string key)
    {
        return _valuesByTestOrSuite.TryGetValue(GetCurrentTestOrSuite(), out var values) && values.TryRemove(key, out _);
    }

    private static bool TryGetValueOfTestOrSuite<T>(ITest testOrSuite, string key, out T value)
    {
        if (_valuesByTestOrSuite.TryGetValue(testOrSuite, out var values) && values.TryGetValue(key, out var storedValue))
        {
            value = (T)storedValue;
            return true;
        }

        value = default!;
        return false;
    }

    private static IEnumerable<ITest> GetCurrentTestOrSuiteAndItsParents()
    {
        for (ITest? testOrSuite = GetCurrentTestOrSuite(); testOrSuite != null; testOrSuite = testOrSuite.Parent)
        {
            yield return testOrSuite;
        }
    }

    private static ConcurrentDictionary<string, object> GetOrAddValuesOfCurrentTestOrSuite()
    {
        return _valuesByTestOrSuite.GetValue(GetCurrentTestOrSuite(), _ => new());
    }

    private static Test GetCurrentTestOrSuite()
    {
        return TestExecutionContext.CurrentContext.CurrentTest;
    }
}
