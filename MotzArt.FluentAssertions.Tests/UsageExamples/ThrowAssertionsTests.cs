using MotzArt.FluentAssertions;

// Recommended use of ThrowAssertions.
using static MotzArt.FluentAssertions.ThrowAssertions;

[TestFixture]
public class ThrowAssertionsTests
{
    [Test]
    public void ShouldThrow_AssertSyncException()
    {
        void TestMethod(string message)
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
}

[TestFixture]
public class MiscTests
{
    [Test]
    public void ChainAssertions()
    {
        var user = new { Name = "Alice", Age = 30 };
        
        user
            .ShouldNotBeNull()
            .ShouldBeOfType<dynamic>();

        var list = new[] { 1, 2, 3 };
        list
            .ShouldNotBeEmpty()
            .ShouldHaveCount(3);

        "hello world"
            .ShouldNotBeNullOrEmpty()
            .ShouldStartWith("hello")
            .ShouldEndWith("world")
            .ShouldContain("lo wo");
    }

    [Test]
    [Ignore("Test is designed to fail to demonstrate custom message functionality.")]
    public void WithCustomMessage()
    {
        var value = 42; 
        value.ShouldBe(43, message: "Expected the answer to be 43");

        //  Expected the answer to be 43
        //  Assert.That(value, Is.EqualTo(expectedValue))
        //  Expected: 43
        //  But was:  42
    }
}