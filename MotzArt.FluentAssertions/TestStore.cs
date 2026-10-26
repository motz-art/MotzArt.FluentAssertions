using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace MotzArt.FluentAssertions;

/// <summary>
/// Stores values for the currently running NUnit test or test fixture, e.g. to share test data between setup and test methods.
/// </summary>
/// <remarks>
/// Values are kept per test or fixture, so tests running in parallel don't see each other's values.
/// A value stored in a one-time setup is visible to all tests of the fixture.
/// </remarks>
public static class TestStore
{
    private static readonly ConditionalWeakTable<ITest, ConcurrentDictionary<string, object>> _valuesByTestOrSuite = new();

    /// <summary>
    /// Stores <paramref name="value"/> under <paramref name="key"/> for the current test or fixture, replacing any previous value.
    /// </summary>
    /// <returns><paramref name="value"/>, for further use.</returns>
    public static T Set<T>(string key, T value)
    {
        GetOrAddValuesOfCurrentTestOrSuite()[key] = value!;
        return value;
    }

    /// <summary>
    /// Gets the value stored under <paramref name="key"/> for the current test or any of its parent fixtures,
    /// or stores and returns the value created by <paramref name="create"/> for the current test or fixture.
    /// </summary>
    /// <returns>The existing or created value.</returns>
    public static T GetOrCreate<T>(string key, Func<string, T> create)
    {
        if (TryGet(key, out T value))
        {
            return value;
        }

        return (T)GetOrAddValuesOfCurrentTestOrSuite().GetOrAdd(key, x => create(x)!);
    }

    /// <summary>
    /// Gets the value stored under <paramref name="key"/> for the current test or the closest of its parent fixtures.
    /// </summary>
    /// <returns><see langword="true"/> when a value was found; otherwise <see langword="false"/>.</returns>
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

    /// <summary>
    /// Removes the value stored under <paramref name="key"/> for the current test or fixture. Values of parent fixtures are kept.
    /// </summary>
    /// <returns><see langword="true"/> when a value was removed; otherwise <see langword="false"/>.</returns>
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
