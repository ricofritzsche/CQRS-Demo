namespace AggregateDemo.Events.Customer
{
    using System;

    using AggregateDemo.Common;

    [Serializable]
    public sealed class BillingAddressChanged : DomainEvent
    {
        public BillingAddressChanged(Guid customerId, string street, string houseNumber, string postalCode, string city, long version, Guid commandId)
            : base(customerId, version, commandId)
        {
            this.Street = street;
            this.HouseNumber = houseNumber;
            this.PostalCode = postalCode;
            this.City = city;
        }

        public string Street { get; private set; }

        public string HouseNumber { get; private set; }

        public string PostalCode { get; private set; }

        public string City { get; private set; }
    }
}