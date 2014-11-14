namespace AggregateDemo.Tests.Customer
{
    using System;

    using AggregateDemo.Domain.Customer;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class WhenCreateCustomer : Infrastructure
    {
        [TestMethod]
        public void NewAggregateMustBeAttachedToTheEventStore()
        {
            var task = this.Processor.HandleAsync(new CreateCustomerCommand(FirstName, LastName, this.Address, Guid.NewGuid()));
            task.Wait();

            this.EventStoreMock.Verify(esm => esm.AddInstance(It.IsAny<CustomerAggregateRoot>()), Times.Once);
        }

        [TestMethod]
        public void AppendChangesMustBeCalledOnce()
        {
            var task = this.Processor.HandleAsync(new CreateCustomerCommand(FirstName, LastName, this.Address, Guid.NewGuid()));
            task.Wait();

            this.EventStoreMock.Verify(esm => esm.AppendChanges(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        public void NewAggregateMustContainSubmittedValues()
        {
            var task = this.Processor.HandleAsync(new CreateCustomerCommand(FirstName, LastName, this.Address, Guid.NewGuid()));
            task.Wait();

            this.EventStoreMock.Verify(
                esm =>
                esm.AddInstance(
                    It.Is<CustomerAggregateRoot>(
                        c =>
                        c.State.FirstName == FirstName && c.State.LastName == LastName
                        && c.State.DeliveryAddress.Equals(this.Address))),
                Times.Once);
        }
        
        [TestMethod]
        public void NewAggregateMustBeVersion1()
        {
            var task = this.Processor.HandleAsync(new CreateCustomerCommand(FirstName, LastName, this.Address, Guid.NewGuid()));
            task.Wait();

            this.EventStoreMock.Verify(esm => esm.AddInstance(It.Is<CustomerAggregateRoot>(c => c.State.Version.Equals(1))), Times.Once);
        }
        
        [TestMethod]
        public void MustThrowAnArgumentNullExceptionWhenFirstNameNotGiven()
        {
            var task = this.Processor.HandleAsync(new CreateCustomerCommand(null, LastName, this.Address, Guid.NewGuid()));
            Action action = task.Wait;
            action.ShouldThrow<ArgumentNullException>();
        }
        
        [TestMethod]
        public void MustThrowAnArgumentNullExceptionWhenLastNameNotGiven()
        {
            var task = this.Processor.HandleAsync(new CreateCustomerCommand(FirstName, null, this.Address, Guid.NewGuid()));
            Action action = task.Wait;
            action.ShouldThrow<ArgumentNullException>();
        }
    }
}
