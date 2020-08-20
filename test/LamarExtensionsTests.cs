using System;
using Lamar;
using Moq;
using Xunit;

namespace Mockery.Lamar.Test
{
    public class LamarExtensionsTests
    {
        public interface IMockable
        {
            void DoWork();
        }

        public class MockBehaviorCases : TheoryData<MockBehavior>
        {
            public MockBehaviorCases()
            {
                Add( MockBehavior.Loose );
                Add( MockBehavior.Strict );
            }
        }

        public class MockWithSpecificBehavior
        {
            ServiceRegistry registry = new ServiceRegistry();
            MockBehavior behavior = default;
            IMockable mocked = null;
            Mock<IMockable> method() => registry.Mock( behavior, out mocked );

            [Fact]
            public void requires_registry()
            {
                registry = null!;
                Assert.Throws<ArgumentNullException>( nameof( registry ), method );
            }

            [Theory]
            [ClassData( typeof( MockBehaviorCases ) )]
            public void returns_mock_with_specified_behavior( MockBehavior behavior )
            {
                this.behavior = behavior;
                var mock = method();
                Assert.Equal( behavior, mock.Behavior );
                Assert.Same( mock.Object, mocked );

                using var container = new Container( registry );
                var actual = container.GetInstance<IMockable>();
                Assert.Same( mock.Object, actual );
            }
        }

        public class MockWithDefaultBehavior
        {
            ServiceRegistry registry = new ServiceRegistry();
            IMockable mocked = null;
            Mock<IMockable> method() => registry.Mock( out mocked );

            [Fact]
            public void requires_registry()
            {
                registry = null!;
                Assert.Throws<ArgumentNullException>( nameof( registry ), method );
            }

            [Fact]
            public void returns_mock_with_strict_behavior()
            {
                var mock = method();
                Assert.Equal( MockBehavior.Strict, mock.Behavior );
                Assert.Same( mock.Object, mocked );

                using var container = new Container( registry );
                var actual = container.GetInstance<IMockable>();
                Assert.Same( mock.Object, actual );
            }
        }
    }
}
