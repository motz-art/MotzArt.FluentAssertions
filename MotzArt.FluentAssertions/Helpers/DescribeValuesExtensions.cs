using System.Reflection;
using System.Text;
using System.Text.Json;
using MotzArt.NullChecks;

namespace MotzArt.FluentAssertions.Helpers;

public static class DescribeValuesExtensions
{
    public static string DescribeValue<T>(this ReadOnlySpan<T> span, int skip = 0, int maxLength = 250)
    {
        if (span.Length == 0) return $"empty <ReadOnlySpan<{typeof(T).DescribeType()}>>";

        var sb = new StringBuilder();

        sb.Append("[");

        if (skip > 0)
        {
            sb.Append("...");
        }

        for (var i = skip; i < span.Length; i++)
        {
            if (i > 0) sb.Append(", ");
            var item = span[i];
            sb.Append(DescribeValue(item));

            if (sb.Length > maxLength)
            {
                sb.Append(", ...");
                break;
            }
        }

        sb.Append("]");

        return sb.ToString();
    }

    public static string DescribeValue<T>(this T? value)
    {
        if (value == null)
        {
            return "null";
        }

        if (value is string str) return str;
        if (value is char ch) return FormatChar(ch);

        if (value is byte b) return b.ToString();
        if (value is sbyte sb) return sb.ToString();
        if (value is short s) return s.ToString();
        if (value is ushort us) return us.ToString();
        if (value is int i) return i.ToString();
        if (value is uint ui) return ui.ToString();
        if (value is long l) return l.ToString();
        if (value is ulong ul) return ul.ToString();

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

    public static string DescribeType(this Type type)
    {
        if (type.IsNullable(out var baseType)) return baseType.DescribeType() + "?";

        if (type.IsArray)
        {
            return type.GetElementType().EnsureNotNull().DescribeType() + string.Join("", Enumerable.Repeat("[]", type.GetArrayRank()));
        }

        if (type.IsGenericType)
        {
            var parameters = type.GetGenericArguments();
            var typeName = type.Name;
            typeName = typeName.Substring(0, typeName.LastIndexOf('`'));

            return $"{typeName}<{string.Join(", ", parameters.Select(x => x.DescribeType()))}>";
        }

        return type.Name;
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