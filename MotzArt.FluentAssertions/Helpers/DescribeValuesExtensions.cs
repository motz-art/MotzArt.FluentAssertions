using MotzArt.NullChecks;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace MotzArt.FluentAssertions.Helpers;

/// <summary>
/// Formats values, types and strings the way they are shown in failure messages.
/// </summary>
public static class DescribeValuesExtensions
{
    private static IFormatProvider FormatProvider => CultureInfo.InvariantCulture;

    private static readonly Dictionary<Type, string> CSharpKeywordsByType = new()
    {
        [typeof(bool)] = "bool",
        [typeof(byte)] = "byte",
        [typeof(sbyte)] = "sbyte",
        [typeof(char)] = "char",
        [typeof(short)] = "short",
        [typeof(ushort)] = "ushort",
        [typeof(int)] = "int",
        [typeof(uint)] = "uint",
        [typeof(long)] = "long",
        [typeof(ulong)] = "ulong",
        [typeof(nint)] = "nint",
        [typeof(nuint)] = "nuint",
        [typeof(float)] = "float",
        [typeof(double)] = "double",
        [typeof(decimal)] = "decimal",
        [typeof(object)] = "object",
        [typeof(string)] = "string",
        [typeof(void)] = "void",
    };

    /// <summary>
    /// Describes the items of <paramref name="span"/> starting at <paramref name="skip"/>, e.g. <c>[1, 2, 3]</c>,
    /// or <c>[..., 2, 3]</c> when items are skipped. The description is cut after about <paramref name="maxLength"/> characters.
    /// </summary>
    public static string DescribeValue<T>(this ReadOnlySpan<T> span, int skip = 0, int maxLength = 250)
    {
        if (span.Length == 0) return $"empty <ReadOnlySpan<{typeof(T).DescribeType()}>>";

        var sb = new StringBuilder();

        sb.Append('[');

        if (skip > 0)
        {
            sb.Append("...");
        }

        for (var i = skip; i < span.Length; i++)
        {
            if (i > 0) sb.Append(", ");
            var item = span[i];
            sb.Append(DescribeValue(item));

            var hasMoreItems = i < span.Length - 1;

            if (sb.Length > maxLength && hasMoreItems)
            {
                sb.Append(", ...");
                break;
            }
        }

        sb.Append(']');

        return sb.ToString();
    }

    /// <summary>
    /// Describes <paramref name="value"/>: strings as they are, characters as C# literals, numbers in invariant culture with
    /// their C# suffix, values with their own <see cref="object.ToString"/> by it, and other values as JSON.
    /// </summary>
    public static string DescribeValue<T>(this T? value)
    {
        if (value == null)
        {
            return "null";
        }

        if (value is string str) return str;
        if (value is char ch) return FormatChar(ch);

        if (value is byte b) return b.ToString(FormatProvider);
        if (value is sbyte sb) return sb.ToString(FormatProvider);
        if (value is short s) return s.ToString(FormatProvider);
        if (value is ushort us) return us.ToString(FormatProvider);
        if (value is int i) return i.ToString(FormatProvider);
        if (value is uint ui) return ui.ToString(FormatProvider) + "u";
        if (value is long l) return l.ToString(FormatProvider) + "L";
        if (value is ulong ul) return ul.ToString(FormatProvider) + "UL";

        if (value is float f) return f.ToString(FormatProvider) + "f";
        if (value is double d) return d.ToString(FormatProvider) + "d";
        if (value is decimal dec) return dec.ToString(FormatProvider) + "m";

        var hasCustomToString = HasCustomToString(value.GetType());

        if (hasCustomToString)
        {
            return value.ToString()!;
        }

        return JsonSerializer.Serialize(value);
    }

    private static string FormatChar(char ch)
    {
        switch (ch)
        {
            case '\\': return @"'\\'";
            case '\'': return @"'\''";
            case '\t': return @"'\t'";
            case '\r': return @"'\r'";
            case '\n': return @"'\n'";
            case '\0': return @"'\0'";
            default: return $"'{ch}'";
        }
    }

    /// <summary>
    /// Describes <paramref name="type"/> as in C# source, e.g. <c>List&lt;int&gt;</c>, <c>int?</c> or <c>string[]</c>.
    /// </summary>
    public static string DescribeType(this Type type)
    {
        if (type.IsNullable(out var baseType)) return baseType.DescribeType() + "?";

        if (type.IsArray)
        {
            return type.GetElementType().EnsureNotNull().DescribeType() + "[" + new string(',', type.GetArrayRank() - 1) + "]";
        }

        if (type.IsGenericType)
        {
            var parameters = type.GetGenericArguments();

            return $"{RemoveGenericArity(type.Name)}<{string.Join(", ", parameters.Select(x => x.DescribeType()))}>";
        }

        return CSharpKeywordsByType.TryGetValue(type, out var keyword) ? keyword : type.Name;
    }

    private static string RemoveGenericArity(string typeName)
    {
        var arityIndex = typeName.LastIndexOf('`');
        return arityIndex < 0 ? typeName : typeName.Substring(0, arityIndex);
    }

    private static bool IsNullable(this Type type)
    {
        return type.IsValueType && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    private static bool IsNullable(this Type type, out Type baseType)
    {
        if (type.IsNullable())
        {
            var args = type.GetGenericArguments();
            baseType = args[0];
            return true;
        }

        baseType = type;
        return false;
    }

    private static bool HasCustomToString(Type type)
    {
        var toStringOverload = type
            .GetMethod(nameof(ToString), BindingFlags.Instance | BindingFlags.Public, Type.EmptyTypes).EnsureNotNull();

        return toStringOverload.DeclaringType != typeof(object);
    }

    /// <summary>
    /// Formats <paramref name="value"/> as a quoted string with escaped quotes and backslashes, or as <c>null</c>.
    /// </summary>
    public static string FormatValue(string? value)
    {
        if (value == null)
        {
            return "null";
        }
        else
        {
            var sb = new StringBuilder();
            WriteStringValue(sb, value);
            return sb.ToString();
        }
    }
    /// <summary>
    /// Appends <paramref name="value"/> to <paramref name="sb"/> as a quoted string with escaped quotes and backslashes, or as <c>null</c>.
    /// </summary>
    public static void WriteValue(this StringBuilder sb, string? value)
    {
        if (value == null)
        {
            sb.Append("null");
        }
        else
        {
            WriteStringValue(sb, value);
        }
    }

    /// <summary>
    /// Appends the item of <paramref name="span"/> at index <paramref name="item"/> to <paramref name="sb"/>,
    /// with <c>...</c> in place of the items before and after it.
    /// </summary>
    public static void ShowAtItem<T>(this StringBuilder sb, ReadOnlySpan<T> span, int item)
    {
        if (item >= span.Length)
        {
            sb.Append("[ ... ]");
            return;
        }

        sb.Append("[ ");

        if (item > 0)
        {
            sb.Append("... , ");
        }

        sb.Append(span[item]);

        if (span.Length > item + 1)
        {
            sb.Append(", ...");
        }

        sb.Append(" ]");
    }

    private static void WriteStringValue(StringBuilder sb, string value)
    {
        sb.Append('"');

        var span = value.AsSpan();

        var index = span.IndexOfAny('"', '\\');

        while (index >= 0)
        {
            sb.Append(span.Slice(0, index));
            sb.Append('\\');
            sb.Append(span[index]);
            span = span.Slice(index + 1);
            index = span.IndexOfAny('"', '\\');
        }

        sb.Append(span);

        sb.Append('"');
    }
}