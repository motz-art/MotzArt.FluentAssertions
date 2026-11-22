using MotzArt.FluentAssertions.Helpers;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MotzArt.FluentAssertions;

/// <summary>
/// Assertions for code that should or shouldn't throw exceptions.
/// </summary>
/// <remarks>
/// The overloads that take <see cref="TestDelegate"/> or <see cref="AsyncTestDelegate"/> are meant to be called with
/// <c>using static MotzArt.FluentAssertions.ThrowAssertions;</c>, e.g. <c>ShouldThrow&lt;ArgumentException&gt;(() => Parse(null));</c>.
/// The extension overloads take a delegate stored in a variable.
/// </remarks>
public static class ThrowAssertions
{
    private const string AsyncCodeIsNotSupportedMessage = "ShouldNotThrow can't await async code, so its failure would go unnoticed. Use ShouldNotThrowAsync instead.";

    /// <summary>
    /// Asserts that <paramref name="code"/> throws <typeparamref name="TException"/> or an exception derived from it.
    /// </summary>
    /// <returns>The thrown exception, for chaining assertions on it.</returns>
    public static TException ShouldThrow<TException>(
        this Action code,
        NUnitString message = default,
        [CallerArgumentExpression(nameof(code))] string codeExpression = "") where TException : Exception
    {
        return ShouldThrow<TException>((TestDelegate)(() => code()), message, codeExpression);
    }

    /// <summary>
    /// Asserts that <paramref name="code"/> throws <typeparamref name="TException"/> or an exception derived from it.
    /// </summary>
    /// <returns>The thrown exception, for chaining assertions on it.</returns>
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

    /// <summary>
    /// Asserts that <paramref name="code"/> throws exactly <typeparamref name="TException"/> with the message <paramref name="exceptionMessage"/>.
    /// </summary>
    public static void ShouldThrowWithMessage<TException>(this Action code, string exceptionMessage, NUnitString message = default, [CallerArgumentExpression(nameof(code))] string codeExpression = "") where TException : Exception
    {
        Assert.That(code, Throws.TypeOf<TException>().With.Message.EqualTo(exceptionMessage), message, actualExpression: codeExpression);
    }

    /// <summary>
    /// Asserts that the task returned by <paramref name="code"/> throws <typeparamref name="TException"/> or an exception derived from it.
    /// </summary>
    /// <returns>The thrown exception, for chaining assertions on it.</returns>
    public static Task<TException> ShouldThrowAsync<TException>(this Func<Task> code, NUnitString message = default, [CallerArgumentExpression(nameof(code))] string codeExpression = "") where TException : Exception
    {
        return ShouldThrowAsync<TException>((AsyncTestDelegate)(() => code()), message, codeExpression);
    }

    /// <summary>
    /// Asserts that the task returned by <paramref name="code"/> throws <typeparamref name="TException"/> or an exception derived from it.
    /// </summary>
    /// <remarks>
    /// An <see cref="AggregateException"/> with a single <typeparamref name="TException"/> inside is unwrapped.
    /// </remarks>
    /// <returns>The thrown exception, for chaining assertions on it.</returns>
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

    /// <summary>
    /// Asserts that <paramref name="code"/> doesn't throw any exception.
    /// </summary>
    /// <remarks>
    /// The failure message shows the thrown exception, which is also kept as the inner exception of the failure.
    /// NUnit result exceptions, such as assertion failures or <see cref="Assert.Ignore(string)"/>, pass through unchanged.
    /// </remarks>
    /// <exception cref="ArgumentException">Thrown when <paramref name="code"/> is an async void lambda or method.</exception>
    public static void ShouldNotThrow(this Action code, NUnitString message = default, [CallerArgumentExpression(nameof(code))] string codeExpression = "")
    {
        EnsureNotAsyncVoid(code);

        ShouldNotThrow((TestDelegate)(() => code()), message, codeExpression);
    }

    /// <summary>
    /// Asserts that <paramref name="code"/> doesn't throw any exception.
    /// </summary>
    /// <remarks>
    /// The failure message shows the thrown exception, which is also kept as the inner exception of the failure.
    /// NUnit result exceptions, such as assertion failures or <see cref="Assert.Ignore(string)"/>, pass through unchanged.
    /// </remarks>
    /// <exception cref="ArgumentException">Thrown when <paramref name="code"/> is an async void lambda or method.</exception>
    public static void ShouldNotThrow(TestDelegate code, NUnitString message = default, [CallerArgumentExpression(nameof(code))] string codeExpression = "")
    {
        EnsureNotAsyncVoid(code);

        using var ctx = new TestExecutionContext.IsolatedContext();

        try
        {
            code();
        }
        catch (Exception exception) when (exception is not ResultStateException)
        {
            throw CreateExceptionThrownFailure(message, codeExpression, exception);
        }
    }

    /// <summary>
    /// Asserts that <paramref name="code"/> doesn't throw any exception.
    /// </summary>
    /// <remarks>
    /// The failure message shows the thrown exception, which is also kept as the inner exception of the failure.
    /// NUnit result exceptions, such as assertion failures or <see cref="Assert.Ignore(string)"/>, pass through unchanged.
    /// </remarks>
    /// <returns>The result of <paramref name="code"/>, for chaining assertions on it.</returns>
    public static T ShouldNotThrow<T>(this Func<T> code, NUnitString message = default, [CallerArgumentExpression(nameof(code))] string codeExpression = "")
    {
        using var ctx = new TestExecutionContext.IsolatedContext();

        try
        {
            return code();
        }
        catch (Exception exception) when (exception is not ResultStateException)
        {
            throw CreateExceptionThrownFailure(message, codeExpression, exception);
        }
    }

    /// <summary>
    /// Not supported: the task returned by <paramref name="code"/> wouldn't be awaited, so its failure would go unnoticed.
    /// Use <see cref="ShouldNotThrowAsync(Func{Task}, NUnitString, string)"/> instead.
    /// </summary>
    /// <exception cref="NotSupportedException">Always thrown.</exception>
    [Obsolete(AsyncCodeIsNotSupportedMessage, error: true)]
    public static void ShouldNotThrow(this Func<Task> code, NUnitString message = default, [CallerArgumentExpression(nameof(code))] string codeExpression = "")
    {
        throw new NotSupportedException(AsyncCodeIsNotSupportedMessage);
    }

    /// <summary>
    /// Not supported: the task returned by <paramref name="code"/> wouldn't be awaited, so its failure would go unnoticed.
    /// Use <see cref="ShouldNotThrowAsync{T}(Func{Task{T}}, NUnitString, string)"/> instead.
    /// </summary>
    /// <exception cref="NotSupportedException">Always thrown.</exception>
    [Obsolete(AsyncCodeIsNotSupportedMessage, error: true)]
    public static T ShouldNotThrow<T>(this Func<Task<T>> code, NUnitString message = default, [CallerArgumentExpression(nameof(code))] string codeExpression = "")
    {
        throw new NotSupportedException(AsyncCodeIsNotSupportedMessage);
    }

    /// <summary>
    /// Asserts that <paramref name="code"/> and the task it returns don't throw any exception.
    /// </summary>
    /// <remarks>
    /// The failure message shows the thrown exception, which is also kept as the inner exception of the failure.
    /// NUnit result exceptions, such as assertion failures or <see cref="Assert.Ignore(string)"/>, pass through unchanged.
    /// </remarks>
    public static Task ShouldNotThrowAsync(this Func<Task> code, NUnitString message = default, [CallerArgumentExpression(nameof(code))] string codeExpression = "")
    {
        return ShouldNotThrowAsync((AsyncTestDelegate)(() => code()), message, codeExpression);
    }

    /// <summary>
    /// Asserts that <paramref name="code"/> and the task it returns don't throw any exception.
    /// </summary>
    /// <remarks>
    /// The failure message shows the thrown exception, which is also kept as the inner exception of the failure.
    /// NUnit result exceptions, such as assertion failures or <see cref="Assert.Ignore(string)"/>, pass through unchanged.
    /// </remarks>
    public static async Task ShouldNotThrowAsync(AsyncTestDelegate code, NUnitString message = default, [CallerArgumentExpression(nameof(code))] string codeExpression = "")
    {
        using var ctx = new TestExecutionContext.IsolatedContext();

        try
        {
            await code();
        }
        catch (Exception exception) when (exception is not ResultStateException)
        {
            throw CreateExceptionThrownFailure(message, codeExpression, exception);
        }
    }

    /// <summary>
    /// Asserts that <paramref name="code"/> and the task it returns don't throw any exception.
    /// </summary>
    /// <remarks>
    /// The failure message shows the thrown exception, which is also kept as the inner exception of the failure.
    /// NUnit result exceptions, such as assertion failures or <see cref="Assert.Ignore(string)"/>, pass through unchanged.
    /// </remarks>
    /// <returns>The result of the task, for chaining assertions on it.</returns>
    public static async Task<T> ShouldNotThrowAsync<T>(this Func<Task<T>> code, NUnitString message = default, [CallerArgumentExpression(nameof(code))] string codeExpression = "")
    {
        using var ctx = new TestExecutionContext.IsolatedContext();

        try
        {
            return await code();
        }
        catch (Exception exception) when (exception is not ResultStateException)
        {
            throw CreateExceptionThrownFailure(message, codeExpression, exception);
        }
    }

    private static void EnsureNotAsyncVoid(Delegate code)
    {
        if (code.Method.IsDefined(typeof(AsyncStateMachineAttribute), inherit: false))
        {
            throw new ArgumentException(AsyncCodeIsNotSupportedMessage, nameof(code));
        }
    }

    private static AssertionException CreateExceptionThrownFailure(NUnitString message, string codeExpression, Exception exception)
    {
        var failureMessage = MessageWriter.Create(message, codeExpression, "Throws.Nothing", "no exception", $"<{exception}>");
        return new AssertionException(failureMessage, exception);
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