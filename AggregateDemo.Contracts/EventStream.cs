namespace AggregateDemo.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Die EventStream Klasse.
    /// </summary>
    public sealed class EventStream
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der EventStream Klasse.
        /// </summary>
        /// <param name="aggregateId">Die Id des Aggregates, zu dem der Stream gehört.</param>
        /// <param name="events">Die Domain Events.</param>
        public EventStream(Guid aggregateId, IList<IDomainEvent> events)
        {
            this.AggregateId = aggregateId;
            this.Events = new ReadOnlyCollection<IDomainEvent>(events);
        }

        /// <summary>
        /// Holt die Id des Aggregates.
        /// </summary>
        public Guid AggregateId { get; private set; }

        /// <summary>
        /// Holt die Version des Streams.
        /// </summary>
        public long Version
        {
            get
            {
                return this.Events.Max(e => e.Version);
            }
        }

        /// <summary>
        /// Holt die Auflistung der Events.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> Events { get; private set; }
    }
}