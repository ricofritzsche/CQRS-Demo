namespace AggregateDemo.Events.Customer
{
    using System;

    using AggregateDemo.Common;

    [Serializable]
    public sealed class MemberStateChanged : DomainEvent
    {
        public MemberStateChanged(Guid customerId, uint memberState, long version, Guid commandId)
            : base(customerId, version, commandId)
        {
            this.MemberState = memberState;
        }

        public uint MemberState { get; private set; }
    }
}