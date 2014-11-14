namespace AggregateDemo.Common
{
    using System;
    using System.Threading.Tasks;

    public abstract class CommandProcessorBase : ICommandProcessor
    {
        private readonly IEventStore eventStore;

        protected CommandProcessorBase(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        /// <summary>
        /// Prozessiert den Befehl ansynchron.
        /// </summary>
        /// <param name="command">Der Befehl, der prozessiert werden soll.</param>
        /// <returns>Die asynchrone Operation.</returns>
        public abstract Task HandleAsync(ICommand command);

        /// <summary>
        /// Prozessiert die übergebenen Befehle ansynchron.
        /// </summary>
        /// <param name="aggregateId">Die Id des Aggregates.</param>
        /// <param name="commands">Die Befehle, die prozessiert werden sollen.</param>
        /// <returns>Die asynchrone Operation.</returns>
        public abstract Task HandleAsync(Guid aggregateId, ICommand[] commands);

        protected void UpdateAggregate<TAggregateRoot>(Guid aggregateId, Action<TAggregateRoot> execute) where TAggregateRoot : class, IAggregateRoot
        {
            var root = this.eventStore.Find<TAggregateRoot>(aggregateId);
            root.Lock();

            execute(root);

            this.eventStore.AppendChanges(aggregateId);
        }
    }
}