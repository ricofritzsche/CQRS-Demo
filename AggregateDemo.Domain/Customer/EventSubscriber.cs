namespace AggregateDemo.Domain.Customer
{
    using System;

    using AggregateDemo.Common;
    using AggregateDemo.Events.Order;

    internal sealed class EventSubscriber : IEventSubscriber<OrderProcessed>
    {
        private readonly ICommandProcessor commandProcessor;

        private readonly IOrderQueryService orderQueryService;

        public EventSubscriber(ICommandProcessor commandProcessor, IOrderQueryService orderQueryService)
        {
            this.commandProcessor = commandProcessor;
            this.orderQueryService = orderQueryService;
        }

        public void Handle(OrderProcessed message)
        {
            var orderCount = this.orderQueryService.GetOrdersProcessedCount(message.CustomerId);
            this.commandProcessor.HandleAsync(new CalculateMemberStateCommand(message.CustomerId, orderCount, Guid.NewGuid()));
        }
    }

    internal interface IOrderQueryService
    {
        int GetOrdersProcessedCount(Guid customerId);
    }
}