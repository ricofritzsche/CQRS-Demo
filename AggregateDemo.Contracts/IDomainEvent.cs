namespace AggregateDemo.Common
{
    using System;

    /// <summary>
    /// Die Nachrichten Schnittstelle. Typen, die diese Schnittstelle implementieren, ermöglichen die Kommunikation zwischen Komponenten.
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Holt die AggregateId des zugehörigen Aggregates.
        /// </summary>
        Guid AggregateId { get; }
        
        /// <summary>
        /// Holt die Versionsnummer.
        /// </summary>
        long Version { get; }

        /// <summary>
        /// Holt die ID des Commands, der das Ereignis ausgelöst hat.
        /// </summary>
        Guid CommandId { get; }
    }
}