namespace AggregateDemo.Domain.Customer
{
    using AggregateDemo.Common;

    using Microsoft.Practices.Unity;

    internal sealed class ContainerRegistrations : UnityContainerExtension
    {
        /// <summary>
        /// Initial the container with this extension's functionality.
        /// </summary>
        /// <remarks>
        /// When overridden in a derived class, this method will modify the given
        ///             <see cref="T:Microsoft.Practices.Unity.ExtensionContext"/> by adding strategies, policies, etc. to
        ///             install it's functions into the container.
        /// </remarks>
        protected override void Initialize()
        {
            this.Container.RegisterType<ICommandProcessor, CommandProcessor>("CustomerCommandProcessor", new ContainerControlledLifetimeManager());
        }
    }
}