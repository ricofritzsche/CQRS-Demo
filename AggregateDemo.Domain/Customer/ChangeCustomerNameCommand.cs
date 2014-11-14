namespace AggregateDemo.Domain.Customer
{
    using System;

    using AggregateDemo.Common;

    [Serializable]
    public sealed class ChangeCustomerNameCommand : ICommand
    {
        public ChangeCustomerNameCommand(Guid customerId, string firstName, string lastName, Guid commandId)
        {
            this.CustomerId = customerId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CommandId = commandId;
        }

        public Guid CustomerId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public Guid CommandId { get; private set; }
    }
}