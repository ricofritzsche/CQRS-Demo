namespace AggregateDemo.Events.Customer
{
    using System;

    using AggregateDemo.Common;

    [Serializable]
    public sealed class CustomerNameChanged : DomainEvent
    {
        public CustomerNameChanged(Guid customerId, string firstName, string lastName, long version, Guid commandId)
            : base(customerId, version, commandId)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }
    }
}