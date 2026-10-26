// Recommended use of ThrowAssertions.
using static MotzArt.FluentAssertions.ThrowAssertions;

namespace MotzArt.FluentAssertions.Tests.UsageExamples;

[TestFixture]
public class ThrowAssertionsTests
{
    [Test]
    public void ShouldThrow_AssertSyncException()
    {
        void TestMethod(string? message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            // Do something...
        }

        // Recommended use of ThrowAssertions with `using static MotzArt.FluentAssertions.ThrowAssertions;`
        var exception = ShouldThrow<ArgumentNullException>(() => TestMethod(null));
        exception.ParamName.ShouldBe("message");
    }

    [Test]
    public void ShouldThrowWithMessage_AssertExceptionMessage()
    {
        // Alternative use through Action
        Action act = () => throw new InvalidOperationException("Operation failed");
        act.ShouldThrowWithMessage<InvalidOperationException>("Operation failed");
    }

    [Test]
    public async Task ShouldThrowAsync_AssertAsyncException()
    {
        Func<Task> action = async () =>
        {
            await Task.Delay(10);
            throw new TimeoutException();
        };

        var ex = await action.ShouldThrowAsync<TimeoutException>();
        ex.ShouldNotBeNull();
    }

    [Test]
    public async Task ShouldThrowAsync_UnwrapsAggregateException()
    {
        Func<Task> action = async () =>
        {
            await Task.Delay(10);
            throw new InvalidOperationException("Async error");
        };

        var ex = await action.ShouldThrowAsync<InvalidOperationException>();
        ex.Message.ShouldBe("Async error");
    }

    [Test]
    public void ShouldNotThrow_AssertNoException()
    {
        var numbers = new List<int>();

        ShouldNotThrow(() => numbers.Add(1));

        Func<int> sum = () => numbers.Sum();
        sum.ShouldNotThrow().ShouldBe(1);
    }

    [Test]
    public async Task ShouldNotThrowAsync_AssertNoAsyncException()
    {
        Func<Task<int>> action = async () =>
        {
            await Task.Delay(10);
            return 42;
        };

        var result = await action.ShouldNotThrowAsync();
        result.ShouldBe(42);
    }
}