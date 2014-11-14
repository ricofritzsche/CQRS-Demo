namespace AggregateDemo.Common
{
    using System;

    /// <summary>
    /// Kennzeichnet Befehle.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Holt die ID des Befehls.
        /// </summary>
        Guid CommandId { get; }
    }
}