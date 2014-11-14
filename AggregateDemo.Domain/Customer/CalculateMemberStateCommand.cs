namespace AggregateDemo.Domain.Customer
{
    using System;

    using AggregateDemo.Common;

    [Serializable]
    public sealed class CalculateMemberStateCommand : ICommand
    {
        public CalculateMemberStateCommand(Guid customerId, int orderCount, Guid commandId)
        {
            this.CustomerId = customerId;
            this.OrderCount = orderCount;
            this.CommandId = commandId;
        }

        public Guid CustomerId { get; private set; }

        public int OrderCount { get; private set; }

        public Guid CommandId { get; private set; }
    }
}