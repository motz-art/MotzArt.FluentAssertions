namespace MotzArt.FluentAssertions.Tests.StoreTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ParallelTestStoreTests
    {
        private const string FixtureKey = "ParallelFixtureKey";
        private const string FixtureValue = $"Value From {nameof(ParallelTestStoreTests)}";
        private const string TestKey = "ParallelTestKey";

        private const int MaxTestCount = 8;

        private static readonly int TestCount = Math.Min(MaxTestCount, Math.Max(Environment.ProcessorCount, 2));

        private static readonly CountdownEvent AllTestsStarted = new(TestCount);

        private static readonly TimeSpan OverlapTimeout = TimeSpan.FromSeconds(10);

        public static IEnumerable<int> TestIndexes => Enumerable.Range(0, TestCount);

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            TestStore.Set(FixtureKey, FixtureValue);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestStore.TryGet(FixtureKey, out string value).ShouldBeTrue();
            value.ShouldBe(FixtureValue);
            TestStore.TryGet(TestKey, out string _).ShouldBeFalse();
        }

        [Test]
        [TestCaseSource(nameof(TestIndexes))]
        public void ValuesAreIsolatedBetweenParallelTests(int index)
        {
            var value = $"Value {index}";

            TestStore.TryGet(TestKey, out string _).ShouldBeFalse();
            TestStore.Set(TestKey, value);

            WaitForOtherTestsToSetTheirValues();

            TestStore.TryGet(TestKey, out string storedValue).ShouldBeTrue();
            storedValue.ShouldBe(value);

            TestStore.TryGet(FixtureKey, out string fixtureValue).ShouldBeTrue();
            fixtureValue.ShouldBe(FixtureValue);
        }

        private static void WaitForOtherTestsToSetTheirValues()
        {
            AllTestsStarted.Signal();
            AllTestsStarted.Wait(OverlapTimeout).ShouldBeTrue($"Expected all {TestCount} test cases to run at the same time.");
        }
    }
}
