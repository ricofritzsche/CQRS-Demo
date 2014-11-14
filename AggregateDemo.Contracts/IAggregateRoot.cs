namespace AggregateDemo.Common
{
    using System;
    using System.Collections.Generic;

    public interface IAggregateRoot
    {
        Guid Id { get; }

        IReadOnlyCollection<IDomainEvent> Changes { get; }

        bool IsLocked { get; }

        void RemoveEventFromChangesCollection(IDomainEvent @event);

        long GetNextVersion();

        void Lock();
        
        void Unlock();

        void Apply<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : class, IDomainEvent;
    }
}

