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