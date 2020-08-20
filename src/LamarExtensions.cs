using System;
using Lamar;

namespace Moq
{
    /// <summary>
    /// Extension methods for mocking within Lamar.
    /// </summary>

    public static class LamarExtensions
    {
        /// <summary>
        /// Registers a mocked instance of the specified type with the container.
        /// </summary>
        /// <typeparam name="T">Type to mock.</typeparam>
        /// <param name="registry">Lamar service registry.</param>
        /// <param name="behavior">Mock behavior.</param>
        /// <param name="mocked">Mocked instance.</param>
        /// <returns>The mock setup.</returns>

        public static Mock<T> Mock<T>( this ServiceRegistry registry, MockBehavior behavior, out T mocked ) where T : class
        {
            if ( registry == null ) throw new ArgumentNullException( nameof( registry ) );

            var mock = Mockery.Of( behavior, out mocked );
            registry.For<T>().Add( mocked );

            return mock;
        }

        /// <summary>
        /// Registers a strictly-mocked instance of the specified type with the container.
        /// </summary>
        /// <typeparam name="T">Type to mock.</typeparam>
        /// <param name="registry">Lamar service registry.</param>
        /// <param name="mocked">Mocked instance.</param>
        /// <returns>The mock setup.</returns>

        public static Mock<T> Mock<T>( this ServiceRegistry registry, out T mocked ) where T : class =>
            registry.Mock( MockBehavior.Strict, out mocked );
    }
}
