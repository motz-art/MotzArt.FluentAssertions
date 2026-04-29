using MotzArt.NullChecks;
using NUnit.Framework;
using System.Collections.Concurrent;

namespace MotzArt.FluentAssertions;

public static class TestStore
{
    public static T Set<T>(string key, T value)
    {
        GetValues().Set(key, value);
        return value;
    }

    public static T GetOrCreate<T>(string key, Func<string, T> create)
    {
        return GetValues().GetOrCreate(key, create);
    }

    public static bool TryGet<T>(string key, out T value)
    {
        return GetValues().TryGet<T>(key, out value);
    }

    public static bool Remove(string key)
    {
        return GetValues().Remove(key);
    }

    private static string? _lastId;
    private static string? _lastName;
    private static IKeyValueStore? _store;
    private static ConcurrentDictionary<string, object>? _testValues;

    public static List<(string name, ConcurrentDictionary<string, object> values)>? _stack;

    private static IKeyValueStore GetValues()
    {
        var testAdapter = TestContext.CurrentContext.Test;

        var id = testAdapter.ID.ShouldNotBeNullOrWhiteSpace();

        if (_store != null && _lastId == id)
        {
            return _store;
        }

        _lastId = id;

        var parent = testAdapter.Parent.EnsureNotNull();
        var test = parent.Tests.First(x => x.Id == testAdapter.ID);
        var type = test.TestType;

        // Values: TestSuite,SetUpFixture,TestFixture,ParameterizedMethod

        var name = type switch
        {
            "SetUpFixture" => testAdapter.Namespace,
            "TestFixture" => testAdapter.ClassName,
            _ => testAdapter.FullName + ":" + testAdapter.ID,
        };

        name.EnsureNotNull();

        if (_lastName != null && name.StartsWith(_lastName + ".", StringComparison.Ordinal))
        {
            if (_testValues.Count > 0)
            {
                _stack ??= new();
                _stack.Add((_lastName, _testValues.EnsureNotNull()));
                _testValues = new();
            }
        }
        else if (_stack != null && _stack.Count > 0)
        {
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                var item = _stack[i];
                if (name.StartsWith(item.name, StringComparison.Ordinal))
                {
                    if (name.Length == item.name.Length)
                    {
                        _testValues = item.values;
                        _stack.RemoveAt(i);
                        goto end;
                    }
                    else
                    {
                        break;
                    }
                }
                _stack.RemoveAt(i);
            }
        }

        _lastName = name;

        if (_testValues == null) _testValues = new();
        else _testValues.Clear();

    end:
        if (_stack != null && _stack.Count > 0)
        {
            _store = new StackStore(_testValues, _stack);
        }
        else
        {
            _store = new DictionaryWrapperStore(_testValues);
        }

        return _store;

    }

    private interface IKeyValueStore
    {
        T Set<T>(string key, T value);
        T GetOrCreate<T>(string key, Func<string, T> create);
        bool TryGet<T>(string key, out T value);
        bool Remove(string key);
    }

    private sealed class DictionaryWrapperStore(ConcurrentDictionary<string, object> dic) : IKeyValueStore
    {
        public T Set<T>(string key, T value)
        {
            dic[key] = value!;
            return value;
        }
        public T GetOrCreate<T>(string key, Func<string, T> create)
        {
            return (T)dic.GetOrAdd(key, create);
        }
        public bool TryGet<T>(string key, out T value)
        {
            var result = dic.TryGetValue(key, out var obj);
            value = (T)obj!;
            return result;
        }
        public bool Remove(string key)
        {
            return dic.Remove(key, out _);
        }
    }

    private sealed class StackStore(ConcurrentDictionary<string, object> dic, List<(string name, ConcurrentDictionary<string, object> values)> stack) : IKeyValueStore
    {
        public T Set<T>(string key, T value)
        {
            dic[key] = value!;
            return value;
        }
        public T GetOrCreate<T>(string key, Func<string, T> create)
        {
            if (TryGet(key, out T value))
            {
                return value;
            }
            return (T)dic.GetOrAdd(key, create);
        }

        public bool TryGet<T>(string key, out T value)
        {
            if (dic.TryGetValue(key, out var obj))
            {
                value = (T)obj;
                return true;
            }

            foreach (var (_, values) in stack.AsEnumerable().Reverse())
            {
                if (values.TryGetValue(key, out obj))
                {
                    value = (T)obj;
                    return true;
                }
            }

            value = default!;
            return false;
        }

        public bool Remove(string key)
        {
            return dic.Remove(key, out _);
        }
    }
}
