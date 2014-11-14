namespace AggregateDemo.Common
{
    using System;

    /// <summary>
    /// Die Schnittstelle des EventStore.
    /// </summary>
    public interface IEventStore : IIdentityMap, IAggregateStore
    {
        /// <summary>
        /// Lädt den EventStream zur übergebenen Aggregat ID.
        /// </summary>
        /// <param name="aggregateId">Die Aggregat ID, für die der EventStream geholt werden soll.</param>
        /// <returns>Eine Instanz des EventStream.</returns>
        EventStream LoadEventStream(Guid aggregateId);

        /// <summary>
        /// Fügt dem EventStream alle unpersistierten Events des angegebenen Aggregates hinzu.
        /// </summary>
        /// <param name="aggregateId">Die ID des Aggregates, dessen Änderungen übernommen werden sollen.</param>
        void AppendChanges(Guid aggregateId);
    }
}