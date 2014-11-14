namespace AggregateDemo.Domain.Order
{
    using System.Collections.Generic;

    public class Order
    {
        public IReadOnlyCollection<OrderLine> Lines { get; set; } 
    }
}