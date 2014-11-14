namespace AggregateDemo.Common
{
    public interface IEventSubscriber<in TDomainEvent> where TDomainEvent : IDomainEvent
    {
        void Handle(TDomainEvent message);
    }
}