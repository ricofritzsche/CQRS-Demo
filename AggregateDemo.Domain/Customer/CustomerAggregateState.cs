namespace AggregateDemo.Domain.Customer
{
    using System;

    using AggregateDemo.Common;
    using AggregateDemo.Events.Customer;

    internal sealed class CustomerAggregateState : IAggregateState
    {
        /// <summary>
        /// Konstruktor, der den Zustand des Aggregates über den Eventstream wieder herstellt, in dem alle Events der Reihe nach auf dem Aggregat angewendet werden.
        /// </summary>
        internal CustomerAggregateState(EventStream eventStream)
        {
            this.Id = eventStream.AggregateId;

            foreach (var e in eventStream.Events)
            {
                this.Mutate(e);
            }
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public Address DeliveryAddress { get; private set; }
        
        public Address BillingAddress { get; private set; }

        public MemberState MemberState { get; private set; }

        public Guid Id { get; private set; }

        public long Version { get; private set; }

        public void Mutate(IDomainEvent domainEvent)
        {
            if (domainEvent.Version == this.Version + 1)
            {
                ((dynamic)this).When((dynamic)domainEvent);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Wrong event version. The version to apply is '{2}', but the expected version is '{0}'. Aggregate ID: '{1}'.", this.Version + 1, domainEvent.AggregateId, domainEvent.Version));
            }
        }

        [UsedImplicitly]
        private void When(CustomerCreated e)
        {
            this.Version = e.Version;

            this.FirstName = e.FirstName;
            this.LastName = e.LastName;
            this.DeliveryAddress = new Address(e.Street, e.HouseNumber, e.PostalCode, e.City);
        }

        [UsedImplicitly]
        private void When(CustomerNameChanged e)
        {
            this.Version = e.Version;

            this.FirstName = e.FirstName;
            this.LastName = e.LastName;
        }

        [UsedImplicitly]
        private void When(DeliveryAddressChanged e)
        {
            this.Version = e.Version;

            this.DeliveryAddress = new Address(e.Street, e.HouseNumber, e.PostalCode, e.City);
        }

        [UsedImplicitly]
        private void When(BillingAddressChanged e)
        {
            this.Version = e.Version;

            this.BillingAddress = new Address(e.Street, e.HouseNumber, e.PostalCode, e.City);
        }
    }
}