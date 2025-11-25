using System.Diagnostics.CodeAnalysis;
using System.Text;
using MotzArt.FluentStrings;
using NUnit.Framework;

namespace MotzArt.FluentAssertions.Helpers;

internal class MessageWriter
{
    private const int IndentSize = 2;
    private readonly StringBuilder sb = new();

    public void WriteMessageIfNotEmpty(NUnitString message)
    {
        var messageText = message.ToString();

        if (messageText.HasValue())
            WriteLine(messageText);
    }

    public void WriteAssertThat(string expression, string constraintExpression)
    {
        Write("Assert.That(");
        Write(expression);
        Write(", ");
        Write(constraintExpression);
        WriteLine(")");
    }

    public void WriteExpected(string expected)
    {
        WriteExpected();
        WriteLine(expected);
    }

    public void WriteExpected()
    {
        Write("Expected: ");
    }

    public void WriteActual(string actual)
    {
        WriteActual();
        WriteLine(actual);
    }

    public void WriteActual()
    {
        Write("But was:  ");
    }

    public void WriteLine(string text)
    {
        EnsureIndent();
        sb.AppendLine(text);
    }

    public void Write(string text)
    {
        EnsureIndent();
        sb.Append(text);
    }

    public void WriteLine()
    {
        sb.AppendLine();
    }

    public void WriteValue(string? value)
    {
        sb.WriteValue(value);
    }

    public override string ToString() => sb.ToString();

    [DoesNotReturn]
    public void ReportFailure()
    {
        Assert.Fail(ToString());
        throw new InvalidOperationException("UPS! This should never happened because of Assert.Fail call above.");
    }

    private void EnsureIndent()
    {
        if (sb.Length == 0 || sb[^1] == '\n')
        {
            for (int i = 0; i < IndentSize; i++)
            {
                sb.Append(' ');
            }
        }
    }

    [DoesNotReturn]
    public static string Report(
        NUnitString message,
        string actualExpression,
        string constraintExpression,
        string expected,
        string actual)
    {
        var messageText = Create(message, actualExpression, constraintExpression, expected, actual);
        Assert.Fail(messageText);
        throw new InvalidOperationException("UPS! This should never happened because of Assert.Fail call above.");
    }

    public static string Create(
        NUnitString message, 
        string actualExpression, 
        string constraintExpression,
        string expected, 
        string actual)
    {
        var writer = new MessageWriter();

        writer.WriteMessageIfNotEmpty(message);
        writer.WriteAssertThat(actualExpression, constraintExpression);
        writer.WriteExpected(expected);
        writer.WriteActual(actual);

        return writer.ToString();
    }
}