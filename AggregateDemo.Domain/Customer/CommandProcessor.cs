namespace AggregateDemo.Domain.Customer
{
    using System;
    using System.Threading.Tasks;

    using AggregateDemo.Common;

    internal sealed class CommandProcessor : CommandProcessorBase
    {
        private readonly IEventStore eventStore;

        public CommandProcessor(IEventStore eventStore) : base(eventStore)
        {
            this.eventStore = eventStore;
        }

        public async override Task HandleAsync(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command", @"The command parameter must not be null!");
            }

            Action action = () => this.Apply((dynamic)command);
            await Task.Factory.StartNew(action);
        }
        
        public override Task HandleAsync(Guid aggregateId, ICommand[] commands)
        {
            throw new NotImplementedException();
        }

        [UsedImplicitly]
        private void Apply(CreateCustomerCommand cmd)
        {
            var customer = new CustomerAggregateRoot(cmd);
            customer.Lock();
            
            this.eventStore.AddInstance(customer);
            this.eventStore.AppendChanges(customer.Id);
        }

        [UsedImplicitly]
        private void Apply(ChangeDeliveryAddressCommand cmd)
        {
            this.UpdateAggregate<CustomerAggregateRoot>(cmd.CustomerId, a => a.ChangeDeliveryAddress(cmd));
        }

        [UsedImplicitly]
        private void Apply(ChangeCustomerNameCommand cmd)
        {
            this.UpdateAggregate<CustomerAggregateRoot>(cmd.CustomerId, a => a.ChangeCustomerName(cmd));
        }
    }
}