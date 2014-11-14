namespace AggregateDemo.Events.Customer
{
    using System;

    using AggregateDemo.Common;

    [Serializable]
    public sealed class CustomerCreated : DomainEvent
    {
        public CustomerCreated(Guid customerId, string firstName, string lastName, string street, string houseNumber, string postalCode, string city, long version, Guid commandId)
            : base(customerId, version, commandId)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Street = street;
            this.HouseNumber = houseNumber;
            this.PostalCode = postalCode;
            this.City = city;
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Street { get; private set; }

        public string HouseNumber { get; private set; }

        public string PostalCode { get; private set; }

        public string City { get; private set; }
    }
}