namespace AggregateDemo.Tests.Customer
{
    using System;
    using System.Linq;

    using AggregateDemo.Domain.Customer;
    using AggregateDemo.Events.Customer;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WhenChangeCustomerName : Infrastructure
    {
        [TestMethod]
        public void MustChangeNames()
        {
            const string NewFirstName = "Max";
            const string NewLastName = "Mustermann";

            this.Customer.ChangeCustomerName(new ChangeCustomerNameCommand(this.Customer.Id, NewFirstName, NewLastName, Guid.NewGuid()));

            this.Customer.State.FirstName.Should().Be(NewFirstName);
            this.Customer.State.LastName.Should().Be(NewLastName);
        }

        [TestMethod]
        public void MustCreateCustomerNameChangedEvent()
        {
            const string NewFirstName = "Max";
            const string NewLastName = "Mustermann";

            this.Customer.ChangeCustomerName(new ChangeCustomerNameCommand(this.Customer.Id, NewFirstName, NewLastName, Guid.NewGuid()));

            this.Customer.Changes.OfType<CustomerNameChanged>().Should().HaveCount(1);
        }
        
        [TestMethod]
        public void ShouldDoNothingIfNamesNotChanged()
        {
            var version = this.Customer.State.Version;

            this.Customer.ChangeCustomerName(new ChangeCustomerNameCommand(this.Customer.Id, this.Customer.State.FirstName, this.Customer.State.LastName, Guid.NewGuid()));

            this.Customer.Changes.OfType<CustomerNameChanged>().Should().HaveCount(0);
            this.Customer.State.Version.Should().Be(version);
        }
    }
}