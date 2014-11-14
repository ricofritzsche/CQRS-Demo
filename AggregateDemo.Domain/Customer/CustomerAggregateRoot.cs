namespace AggregateDemo.Domain.Customer
{
    using System;
    using System.Collections.Generic;

    using AggregateDemo.Common;
    using AggregateDemo.Events.Customer;

    internal sealed class CustomerAggregateRoot : AggregateRoot<CustomerAggregateState>
    {
        /// <summary>
        /// Konstruktor zur Erstellung eines neuen Customer-Aggregates.
        /// </summary>
        public CustomerAggregateRoot([NotNull] CreateCustomerCommand cmd) : base(new CustomerAggregateState(new EventStream(Guid.NewGuid(), new List<IDomainEvent>())))
        {
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }

            if (string.IsNullOrWhiteSpace(cmd.FirstName))
            {
                throw new ArgumentNullException(cmd.FirstName);
            }

            if (string.IsNullOrWhiteSpace(cmd.LastName))
            {
                throw new ArgumentNullException(cmd.LastName);
            }

            this.Apply(new CustomerCreated(this.Id, cmd.FirstName, cmd.LastName, cmd.Address.Street, cmd.Address.HouseNumber, cmd.Address.PostalCode, cmd.Address.City, this.GetNextVersion(), cmd.CommandId));
        }

        /// <summary>
        /// Konstruktor zur Aggregat-Wiederherstellung aus dem EventStream.
        /// </summary>
        public CustomerAggregateRoot(EventStream eventStream)
            : base(new CustomerAggregateState(eventStream))
        {
        }

        public void ChangeCustomerName(ChangeCustomerNameCommand cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd.FirstName))
            {
                throw new ArgumentNullException(cmd.FirstName);
            }

            if (string.IsNullOrWhiteSpace(cmd.LastName))
            {
                throw new ArgumentNullException(cmd.LastName);
            }

            if (this.State.FirstName.Equals(cmd.FirstName) && this.State.LastName.Equals(cmd.LastName))
            {
                return;
            }

            this.Apply(new CustomerNameChanged(this.Id, cmd.FirstName, cmd.LastName, this.GetNextVersion(), cmd.CommandId));
        }

        public void ChangeDeliveryAddress(ChangeDeliveryAddressCommand cmd)
        {
            if (this.State.DeliveryAddress.Equals(cmd.Address))
            {
                return;
            }

            this.Apply(new DeliveryAddressChanged(this.Id, cmd.Address.Street, cmd.Address.HouseNumber, cmd.Address.PostalCode, cmd.Address.City, this.GetNextVersion(), cmd.CommandId));
        }

        public void ChangeDeliveryAddress(ChangeBillingAddressCommand cmd)
        {
            if (this.State.DeliveryAddress.Equals(cmd.Address))
            {
                return;
            }

            this.Apply(new BillingAddressChanged(this.Id, cmd.Address.Street, cmd.Address.HouseNumber, cmd.Address.PostalCode, cmd.Address.City, this.GetNextVersion(), cmd.CommandId));
        }

        public void CalculateMemberState(CalculateMemberStateCommand cmd)
        {
            var memberState = MemberState.Basic;

            if (cmd.OrderCount >= 5)
            {
                memberState = MemberState.Silver;
            }
            else if (cmd.OrderCount > 10)
            {
                memberState = MemberState.Gold;
            }

            if (this.State.MemberState.Equals(memberState))
            {
                return;
            }

            this.Apply(new MemberStateChanged(this.Id, (uint)memberState, this.GetNextVersion(), cmd.CommandId));
        }
    }
}