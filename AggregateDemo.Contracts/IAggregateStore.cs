namespace AggregateDemo.Common
{
    using System;

    /// <summary>
    /// Definiert eine Schnittstelle, die Signaturen für den Zugriff auf Aggregat Instanz anbietet.
    /// </summary>
    public interface IAggregateStore
    {
        /// <summary>
        /// Ermittelt die Aggregat Instanz mittels der übergebenen ID.
        /// </summary>
        /// <typeparam name="TAggregate">Der Typ des Aggregates, das geholt werden soll.</typeparam>
        /// <param name="aggregateId">Die Aggregat ID.</param>
        /// <returns>Die Instanz des Aggregates.</returns>
        [Pure]
        TAggregate Find<TAggregate>(Guid aggregateId) where TAggregate : class, IAggregateRoot;
    }
}