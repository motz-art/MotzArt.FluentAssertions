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
}