namespace AggregateDemo.Events.Order
{
    using System;

    using AggregateDemo.Common;

    public class OrderProcessed : DomainEvent
    {
        public Guid CustomerId { get; set; }

        public OrderProcessed(Guid orderId, Guid customerId, string orderNumber, long version, Guid commandId)
            : base(orderId, version, commandId)
       {
           this.CustomerId = customerId;
       }
    }
}