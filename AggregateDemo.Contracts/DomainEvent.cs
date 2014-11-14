namespace AggregateDemo.Common
{
    using System;

    /// <summary>
    /// Die Basisklasse für Fachereignisse.
    /// </summary>
    [Serializable]
    public abstract class DomainEvent : IDomainEvent
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der DomainEvent Klasse.
        /// </summary>
        /// <param name="aggregateId">Die Id des Aggregates.</param>
        /// <param name="version">Die Version des Events.</param>
        protected DomainEvent(Guid aggregateId, long version) : this(aggregateId, version, Guid.Empty)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der DomainEvent Klasse.
        /// </summary>
        /// <param name="aggregateId">Die Id des Aggregates.</param>
        /// <param name="version">Die Version des Events.</param>
        /// <param name="commandId">Die ID des Commands, der das Ereignis auslöste.</param>
        protected DomainEvent(Guid aggregateId, long version, Guid commandId)
        {
            this.AggregateId = aggregateId;
            this.Version = version;
            this.CommandId = commandId;
        }
        
        /// <summary>
        /// Holt die AggregateId des zugehörigen Aggregates.
        /// </summary>
        public Guid AggregateId { get; private set; }
        
        /// <summary>
        /// Holt die Versionsnummer.
        /// </summary>
        public long Version { get; private set; }

        /// <summary>
        /// Holt die ID des Commands, der das Ereignis ausgelöst hat.
        /// </summary>
        public Guid CommandId { get; private set; }
    }
}