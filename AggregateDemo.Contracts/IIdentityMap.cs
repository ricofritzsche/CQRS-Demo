namespace AggregateDemo.Common
{
    using System;

    /// <summary>
    /// Eine Schnittstelle, die Signaturen von Operationen anbietet, die zur Implementierung einer Identity Map erforderlich sind.
    /// </summary>
    public interface IIdentityMap
    {
        /// <summary>
        /// Versucht, eine Instanz aus dem IdentityMap Cache zu holen.
        /// </summary>
        /// <typeparam name="TAggregate">Der Typ des Aggregates.</typeparam>
        /// <param name="aggregateId">Die Id des Aggregates.</param>
        /// <param name="aggregate">Die Aggregate Instanz, im Falle der Verfügbarkeit.</param>
        /// <returns>Ein Wert, der angibt, ob die Instanz gefunden wurde.</returns>
        bool TryGetInstance<TAggregate>(Guid aggregateId, out TAggregate aggregate) where TAggregate : class, IAggregateRoot;

        /// <summary>
        /// Fügt dem IdentityMap Cache eine Aggregate Instanz hinzu.
        /// </summary>
        /// <typeparam name="TAggregate">Der Typ des Aggregates.</typeparam>
        /// <param name="aggregate">Die Aggregat Instanz, die hinzugefügt werden soll.</param>
        void AddInstance<TAggregate>(TAggregate aggregate) where TAggregate : class, IAggregateRoot;

        /// <summary>
        /// Entfernt die Instanz mit der übergebenen ID aus der Identity Map, sofern vorhanden.
        /// </summary>
        /// <param name="aggregateId">Die ID des Aggregates.</param>
        void RemoveInstance(Guid aggregateId);
    }
}