namespace AggregateDemo.Common
{
    using System;

    public interface IAggregateState
    {
        Guid Id { get; }

        long Version { get; }
        
        void Mutate(IDomainEvent domainEvent); 
    }
}


