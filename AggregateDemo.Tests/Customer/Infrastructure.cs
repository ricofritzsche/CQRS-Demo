namespace AggregateDemo.Tests.Customer
{
    using System;

    using AggregateDemo.Common;
    using AggregateDemo.Domain.Customer;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    public abstract class Infrastructure
    {
        protected ICommandProcessor Processor;

        protected Mock<IEventStore> EventStoreMock;

        protected const string FirstName = "Rico";

        protected const string LastName = "Fritzsche";

        protected readonly Address Address = new Address("Annaberger Str.", "240", "09125", "Chemnitz");

        internal CustomerAggregateRoot Customer;

        [TestInitialize]
        public virtual void Initialize()
        {
            this.EventStoreMock = new Mock<IEventStore>();
            this.Processor = new CommandProcessor(this.EventStoreMock.Object);

            this.Customer = new CustomerAggregateRoot(new CreateCustomerCommand(FirstName, LastName, this.Address, Guid.NewGuid()));
            this.EventStoreMock.Setup(esm => esm.Find<CustomerAggregateRoot>(this.Customer.Id)).Returns(this.Customer);
        }
    }
}