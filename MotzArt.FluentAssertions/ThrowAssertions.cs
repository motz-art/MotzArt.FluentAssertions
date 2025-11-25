using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using MotzArt.FluentAssertions.Helpers;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace MotzArt.FluentAssertions;

public static class ThrowAssertions
{
    public static TException ShouldThrow<TException>(
        this Action code,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(code))] string codeExpression = "") where TException : Exception
    {
        return ShouldThrow<TException>((TestDelegate) (() => code()), message, codeExpression);
    }

    public static TException ShouldThrow<TException>(
        TestDelegate code,
        NUnitString message = default, 
        [CallerArgumentExpression(nameof(code))] string codeExpression = "") where TException : Exception
    {
        using var ctx = new TestExecutionContext.IsolatedContext();

        try
        {
            code();
        }
        catch (Exception exception)
        {
            if (exception is TException targetException)
                return targetException;

            ReportUnexpectedException<TException>(message, codeExpression, exception);
        }

        throw CreateNoExceptionThrownException<TException>(message, codeExpression);
    }

    public static void ShouldThrowWithMessage<TException>(this Action code, string exceptionMessage, NUnitString message = default, [CallerArgumentExpression(nameof(code))] string codeExpression = "") where TException : Exception
    {
        Assert.That(code, Throws.TypeOf<TException>().With.Message.EqualTo(exceptionMessage), message, actualExpression: codeExpression);
    }

    public static Task<TException> ShouldThrowAsync<TException>(this Func<Task> code, NUnitString message = default, [CallerArgumentExpression(nameof(code))] string codeExpression = "") where TException : Exception
    {
        return ShouldThrowAsync<TException>((AsyncTestDelegate)(() => code()), message, codeExpression);
    }

    public static async Task<TException> ShouldThrowAsync<TException>(AsyncTestDelegate code, NUnitString message = default, [CallerArgumentExpression(nameof(code))] string codeExpression = "") where TException : Exception
    {
        using var ctx = new TestExecutionContext.IsolatedContext();

        try
        {
            await code();
        }
        catch (Exception exception)
        {
            if (exception is TException targetException)
                return targetException;

            if (exception is AggregateException aggregateException)
            {
                if (aggregateException.InnerExceptions.Count == 1 &&
                    aggregateException.InnerExceptions[0] is TException tException)
                {
                    return tException;
                }
            }

            ReportUnexpectedException<TException>(message, codeExpression, exception);
        }

        throw CreateNoExceptionThrownException<TException>(message, codeExpression);
    }

    private static AssertionException CreateNoExceptionThrownException<TException>(NUnitString message, string codeExpression)
        where TException : Exception
    {
        return new AssertionException(GetFailureMessage(message, codeExpression, typeof(TException), "no exception thrown"));
    }

    [DoesNotReturn]
    private static void ReportUnexpectedException<TException>(NUnitString message, string codeExpression,
        Exception exception) where TException : Exception
    {
        throw new AssertionException(GetFailureMessage(message, codeExpression, typeof(TException), $"<{exception}>"), exception);
    }

    private static string GetFailureMessage(NUnitString message, string codeExpression, Type exceptionType, string actual)
    {
        return MessageWriter.Create(message,
            codeExpression, $"Throws.TypeOf<{exceptionType.DescribeType()}>()",
            $"<{exceptionType.FullName}>",
            actual);
    }
}