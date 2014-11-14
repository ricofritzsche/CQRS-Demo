namespace AggregateDemo.Domain.Customer
{
    using System;

    using AggregateDemo.Common;

    [Serializable]
    public sealed class CreateCustomerCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public CreateCustomerCommand(string firstName, string lastName, Address address, Guid commandId)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address = address;
            this.CommandId = commandId;
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }
        
        public Address Address { get; private set; }

        public Guid CommandId { get; private set; }
    }
}