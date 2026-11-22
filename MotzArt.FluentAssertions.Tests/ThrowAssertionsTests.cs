using System.Reflection;
using static MotzArt.FluentAssertions.Tests.TestHelpers;
using static MotzArt.FluentAssertions.ThrowAssertions;

namespace MotzArt.FluentAssertions.Tests;

[TestFixture]
public class ThrowAssertionsTests
{
    private const string? TestExceptionMessage = "Test exception";
    private readonly TestDelegate willThrowNoException = () => { };
    private readonly TestDelegate throwsInvalidOperationException = () => throw new InvalidOperationException(TestExceptionMessage);
    private readonly AsyncTestDelegate willThrowNoExceptionAsync = () => Task.CompletedTask;
    private readonly AsyncTestDelegate throwsInvalidOperationExceptionAsync = () => Task.FromException(new InvalidOperationException(TestExceptionMessage));

    [Test]
    public void ShouldThrow_PassWhenExceptionIsThrown()
    {
        var message = ShouldThrow<InvalidOperationException>(throwsInvalidOperationException);

        message.ShouldBeOfType<InvalidOperationException>().Message.ShouldBe(TestExceptionMessage);
    }

    [Test]
    public void ShouldThrow_WithCatchAll_PassWhenExceptionIsThrown()
    {
        var message = ShouldThrow<Exception>(throwsInvalidOperationException);

        message.ShouldBeOfType<InvalidOperationException>().Message.ShouldBe(TestExceptionMessage);
    }

    [Test]
    public void ShouldThrow_FailWhenNothingIsThrown()
    {
        ShouldThrowAssertionException(() =>
        {
            ShouldThrow<Exception>(willThrowNoException);
        }).Message.ShouldBe($"""
                               Assert.That({nameof(willThrowNoException)}, Throws.TypeOf<Exception>())
                               Expected: <System.Exception>
                               But was:  no exception thrown
                             
                             """);
    }

    [Test]
    public void ShouldThrow_FailWhenUnexpectedExceptionIsThrown()
    {
        ShouldThrowAssertionException(() =>
        {
            ShouldThrow<ArgumentException>(throwsInvalidOperationException);
        }).Message.ShouldStartWith($"""
                                 Assert.That({nameof(throwsInvalidOperationException)}, Throws.TypeOf<ArgumentException>())
                                 Expected: <System.ArgumentException>
                                 But was:  <System.InvalidOperationException: Test exception
                               """);
    }

    [Test]
    public async Task ShouldThrowAsync_PassWhenExceptionIsThrown()
    {
        await ShouldThrowAsync<InvalidOperationException>(throwsInvalidOperationExceptionAsync);
    }

    [Test]
    public async Task ShouldThrowAsync_Fluent_PassWhenExceptionIsThrown()
    {
        Func<Task> act = () => Task.FromException(new InvalidOperationException(TestExceptionMessage));
        await act.ShouldThrowAsync<InvalidOperationException>();
    }

    [Test]
    public async Task ShouldThrowAsync_FailWhenNoExceptionIsThrown()
    {
        (await ShouldThrowAssertionExceptionAsync(async () =>
        {
            await ShouldThrowAsync<InvalidOperationException>(willThrowNoExceptionAsync);
        })).Message.ShouldBe($"""
                               Assert.That({nameof(willThrowNoExceptionAsync)}, Throws.TypeOf<InvalidOperationException>())
                               Expected: <System.InvalidOperationException>
                               But was:  no exception thrown
                             
                             """);
    }

    [Test]
    public async Task ShouldThrowAsync_FailWhenUnexpectedExceptionTypeIsThrown()
    {
        (await ShouldThrowAssertionExceptionAsync(async () =>
        {
            await ShouldThrowAsync<ArgumentException>(throwsInvalidOperationExceptionAsync);
        })).Message.ShouldStartWith($"""
                               Assert.That({nameof(throwsInvalidOperationExceptionAsync)}, Throws.TypeOf<ArgumentException>())
                               Expected: <System.ArgumentException>
                               But was:  <System.InvalidOperationException: Test exception
                             """);
    }

    [Test]
    public async Task ShouldThrowAsync_ShouldUnwrapAggregateExceptionWithSingleExpectedException()
    {
        AsyncTestDelegate throwsAggregateException = () => Task.FromException(new AggregateException(new InvalidOperationException(TestExceptionMessage)));

        var exception = await ShouldThrowAsync<InvalidOperationException>(throwsAggregateException);

        exception.Message.ShouldBe(TestExceptionMessage);
    }

    [Test]
    public async Task ShouldThrowAsync_FailWhenAggregateExceptionHasSeveralInnerExceptions()
    {
        AsyncTestDelegate throwsAggregateException = () => Task.FromException(new AggregateException(new InvalidOperationException(), new InvalidOperationException()));

        (await ShouldThrowAssertionExceptionAsync(async () =>
        {
            await ShouldThrowAsync<InvalidOperationException>(throwsAggregateException);
        })).Message.ShouldStartWith($"""
                               Assert.That({nameof(throwsAggregateException)}, Throws.TypeOf<InvalidOperationException>())
                               Expected: <System.InvalidOperationException>
                               But was:  <System.AggregateException:
                             """);
    }

    [Test]
    public void ShouldNotThrow_PassWhenNothingIsThrown()
    {
        ShouldNotThrow(willThrowNoException);
    }

    [Test]
    public void ShouldNotThrow_FailWhenExceptionIsThrown()
    {
        ShouldThrowAssertionException(() =>
        {
            ShouldNotThrow(throwsInvalidOperationException);
        }).Message.ShouldStartWith($"""
                                 Assert.That({nameof(throwsInvalidOperationException)}, Throws.Nothing)
                                 Expected: no exception
                                 But was:  <System.InvalidOperationException: Test exception
                               """);
    }

    [Test]
    public void ShouldNotThrow_KeepThrownExceptionAsInnerException()
    {
        var assertionException = ShouldThrowAssertionException(() => ShouldNotThrow(throwsInvalidOperationException));

        assertionException.InnerException.ShouldBeOfType<InvalidOperationException>().Message.ShouldBe(TestExceptionMessage);
    }

    [Test]
    public void ShouldNotThrow_IncludeCustomMessage()
    {
        ShouldThrowAssertionException(() => ShouldNotThrow(throwsInvalidOperationException, "Custom message"))
            .Message.FirstLine(trim: true).ShouldBe("Custom message");
    }

    [Test]
    public void ShouldNotThrow_LetIgnoreExceptionThrough()
    {
        ShouldThrow<IgnoreException>(() => ShouldNotThrow(() => Assert.Ignore("Not applicable")))
            .Message.ShouldBe("Not applicable");
    }

    [Test]
    public void ShouldNotThrow_LetNestedAssertionFailureThrough()
    {
        ShouldThrowAssertionException(() => ShouldNotThrow(() => 1.ShouldBe(2)))
            .Message.ShouldStartWith("  Assert.That(1, Is.EqualTo(");
    }

    [Test]
    public void ShouldNotThrow_WithFunc_LetInconclusiveExceptionThrough()
    {
        Func<int> calculate = () =>
        {
            Assert.Inconclusive("Can't calculate");
            return 42;
        };

        ShouldThrow<InconclusiveException>(() => calculate.ShouldNotThrow())
            .Message.ShouldBe("Can't calculate");
    }

    [Test]
    public async Task ShouldNotThrowAsync_LetIgnoreExceptionThrough()
    {
        AsyncTestDelegate ignore = async () =>
        {
            await Task.Yield();
            Assert.Ignore("Not applicable");
        };

        (await ShouldThrowAsync<IgnoreException>(() => ShouldNotThrowAsync(ignore)))
            .Message.ShouldBe("Not applicable");
    }

    [Test]
    public async Task ShouldNotThrowAsync_WithResult_LetSuccessExceptionThrough()
    {
        Func<Task<int>> calculate = async () =>
        {
            await Task.Yield();
            Assert.Pass("Already passed");
            return 42;
        };

        (await ShouldThrowAsync<SuccessException>(() => calculate.ShouldNotThrowAsync()))
            .Message.ShouldBe("Already passed");
    }

    [Test]
    public void ShouldNotThrow_Fluent_PassWhenNothingIsThrown()
    {
        Action act = () => { };
        act.ShouldNotThrow();
    }

    [Test]
    public void ShouldNotThrow_WithFunc_ReturnResult()
    {
        Func<int> calculate = () => 42;
        calculate.ShouldNotThrow().ShouldBe(42);
    }

    [Test]
    public void ShouldNotThrow_WithFunc_FailWhenExceptionIsThrown()
    {
        Func<int> calculate = () => throw new InvalidOperationException(TestExceptionMessage);
        ShouldThrowAssertionException(() => calculate.ShouldNotThrow())
            .Message.ShouldStartWith($"""
                                 Assert.That({nameof(calculate)}, Throws.Nothing)
                                 Expected: no exception
                                 But was:  <System.InvalidOperationException: Test exception
                               """);
    }

    [Test]
    public void ShouldNotThrow_RejectAsyncVoidLambda()
    {
        var exception = ShouldThrow<ArgumentException>(() => ShouldNotThrow(async () => await Task.Yield()));

        exception.Message.ShouldStartWith("ShouldNotThrow can't await async code");
    }

    [Test]
    public void ShouldNotThrow_Fluent_RejectAsyncVoidAction()
    {
        Action act = async () => await Task.Yield();

        var exception = ShouldThrow<ArgumentException>(() => act.ShouldNotThrow());

        exception.Message.ShouldStartWith("ShouldNotThrow can't await async code");
    }

    [Test]
    public void ShouldNotThrow_OverloadsForTaskReturningCode_AreObsoleteAsErrors()
    {
        var taskReturningOverloads = typeof(ThrowAssertions).GetMethods()
            .Where(x => x.Name == nameof(ShouldNotThrow) && ReturnsTask(x.GetParameters()[0].ParameterType))
            .ToArray();

        taskReturningOverloads.Length.ShouldBe(2);
        taskReturningOverloads.ShouldAllSatisfy(x => x.GetCustomAttribute<ObsoleteAttribute>().ShouldNotBeNull().IsError.ShouldBeTrue());
    }

    private static bool ReturnsTask(Type delegateType)
    {
        return typeof(Task).IsAssignableFrom(delegateType.GetMethod(nameof(Action.Invoke))!.ReturnType);
    }

    [Test]
    public async Task ShouldNotThrowAsync_PassWhenNothingIsThrown()
    {
        await ShouldNotThrowAsync(willThrowNoExceptionAsync);
    }

    [Test]
    public async Task ShouldNotThrowAsync_FailWhenExceptionIsThrown()
    {
        (await ShouldThrowAssertionExceptionAsync(async () =>
        {
            await ShouldNotThrowAsync(throwsInvalidOperationExceptionAsync);
        })).Message.ShouldStartWith($"""
                               Assert.That({nameof(throwsInvalidOperationExceptionAsync)}, Throws.Nothing)
                               Expected: no exception
                               But was:  <System.InvalidOperationException: Test exception
                             """);
    }

    [Test]
    public async Task ShouldNotThrowAsync_Fluent_PassWhenNothingIsThrown()
    {
        Func<Task> act = () => Task.Delay(1);
        await act.ShouldNotThrowAsync();
    }

    [Test]
    public async Task ShouldNotThrowAsync_WithResult_ReturnResult()
    {
        Func<Task<int>> calculate = () => Task.FromResult(42);
        (await calculate.ShouldNotThrowAsync()).ShouldBe(42);
    }

    [Test]
    public async Task ShouldNotThrowAsync_WithResult_FailWhenExceptionIsThrown()
    {
        Func<Task<int>> calculate = () => Task.FromException<int>(new InvalidOperationException(TestExceptionMessage));
        (await ShouldThrowAssertionExceptionAsync(() => calculate.ShouldNotThrowAsync()))
            .Message.ShouldStartWith($"""
                               Assert.That({nameof(calculate)}, Throws.Nothing)
                               Expected: no exception
                               But was:  <System.InvalidOperationException: Test exception
                             """);
    }
}