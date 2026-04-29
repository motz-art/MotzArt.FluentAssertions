namespace MotzArt.FluentAssertions.Tests.StoreTests
{
    [SetUpFixture]
    public class NameSpaceFixtures
    {
        private const string FixtureScopeValue = $"Value From {nameof(NameSpaceFixtures)}";
        private const string OneTimeSetupName = "OneTimeFixtureSetUp";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            TestStore.TryGet(OneTimeSetupName, out string _).ShouldBeFalse();
            TestStore.Set(OneTimeSetupName, FixtureScopeValue);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestStore.TryGet(OneTimeSetupName, out string value).ShouldBeTrue();
            value.ShouldBe(FixtureScopeValue);
            TestStore.Remove(OneTimeSetupName);
        }
    }

    [TestFixture]
    public class TestStoreTests
    {
        private const string FixtureScopeValue = $"Value From {nameof(TestStoreTests)}";
        private const string OneTimeSetupName = "OneTimeSetUp";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            TestStore.TryGet(OneTimeSetupName, out string _).ShouldBeFalse();
            TestStore.Set(OneTimeSetupName, FixtureScopeValue);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestStore.TryGet(OneTimeSetupName, out string value).ShouldBeTrue();
            value.ShouldBe(FixtureScopeValue);
            TestStore.Remove(OneTimeSetupName);
        }

        const string Key1 = "Test1Key";
        const string Key2 = "Test2Key";

        [SetUp]
        public void Setup()
        {
            TestStore.TryGet(Key1, out string _).ShouldBeFalse();
            TestStore.TryGet(Key2, out string _).ShouldBeFalse();
        }

        [Test]
        [TestCase("value1")]
        [TestCase("value2")]
        [TestCase("value3")]
        public void Test1(string value)
        {
            TestStore.TryGet(Key1, out string _).ShouldBeFalse();
            TestStore.TryGet(Key2, out string _).ShouldBeFalse();

            TestStore.Set(Key1, value);

            TestStore.TryGet(Key1, out string storedValue).ShouldBeTrue();
            storedValue.ShouldBe(value);
        }

        public static List<TestParam> TestParams =>
        [
            new TestParam {Name = "Name 1"},
            new TestParam {Name = "Name 2"},
            new TestParam {Name = "Name 3"}
        ];

        [Test]
        [TestCaseSource(nameof(TestParams))]
        public void Test2(TestParam param)
        {
            TestStore.TryGet(Key1, out string _).ShouldBeFalse();
            TestStore.TryGet(Key2, out TestParam _).ShouldBeFalse();

            TestStore.Set(Key2, param);

            TestStore.TryGet(Key2, out TestParam value2).ShouldBeTrue();
            value2.ShouldBe(param);
        }

        [TearDown]
        public void TearDown()
        {
            (TestStore.TryGet(Key1, out object value) || TestStore.TryGet(Key2, out value)).ShouldBeTrue();
            value.ShouldNotBeNull();
        }

        public class TestParam
        {
            public string Name { get; set; }
        }
    }

    namespace MotzArt.FluentAssertions.Tests.StoreTests.SubNameSpace
    {
        [SetUpFixture]
        public class NameSpaceFixtures
        {
            private const string FixtureScopeValue = $"Value From {nameof(NameSpaceFixtures)}";
            private const string OneTimeSetupName = "OneTimeSubFixtureSetUp";

            [OneTimeSetUp]
            public void OneTimeSetUp()
            {
                TestStore.TryGet(OneTimeSetupName, out string _).ShouldBeFalse();
                TestStore.Set(OneTimeSetupName, FixtureScopeValue);
            }

            [OneTimeTearDown]
            public void OneTimeTearDown()
            {
                TestStore.TryGet(OneTimeSetupName, out string value).ShouldBeTrue();
                value.ShouldBe(FixtureScopeValue);
                TestStore.Remove(OneTimeSetupName);
            }
        }

        [TestFixture]
        public class SubNamespaceTest
        {
            private const string FixtureScopeValue = $"Value From {nameof(SubNamespaceTest)}";
            private const string OneTimeSetupName = "OneTimeSubNamespaceTest";

            [OneTimeSetUp]
            public void OneTimeSetUp()
            {
                TestStore.TryGet(OneTimeSetupName, out string _).ShouldBeFalse();
                TestStore.Set(OneTimeSetupName, FixtureScopeValue);
            }

            [OneTimeTearDown]
            public void OneTimeTearDown()
            {
                TestStore.TryGet(OneTimeSetupName, out string value).ShouldBeTrue();
                value.ShouldBe(FixtureScopeValue);
                TestStore.Remove(OneTimeSetupName);
            }

            [Test]
            public void SubNamespaceTest1()
            {
                TestStore.TryGet(OneTimeSetupName, out string value).ShouldBeTrue();
                value.ShouldBe(FixtureScopeValue);
            }
        }
    }
}