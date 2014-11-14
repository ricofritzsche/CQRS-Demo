namespace AggregateDemo.Domain.Customer
{
    using System;

    using AggregateDemo.Common;

    [Serializable]
    public sealed class ChangeDeliveryAddressCommand : ICommand
    {
        public ChangeDeliveryAddressCommand(Guid customerId, Address address, Guid commandId)
        {
            this.CommandId = commandId;
            this.CustomerId = customerId;
            this.Address = address;
        }

        public Guid CustomerId { get; private set; }

        public Address Address { get; private set; }

        public Guid CommandId { get; private set; }
    }
}