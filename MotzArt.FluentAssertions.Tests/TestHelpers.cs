namespace MotzArt.FluentAssertions.Tests;

public static class TestHelpers
{
    public static AssertionException ShouldThrowAssertionException(Action act)
    {
        return act.ShouldThrow<AssertionException>();
    }

    public static async Task<AssertionException> ShouldThrowAssertionExceptionAsync(Func<Task> act)
    {
        return await act.ShouldThrowAsync<AssertionException>();
    }
}