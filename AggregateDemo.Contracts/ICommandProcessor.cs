namespace AggregateDemo.Common
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Öffentliche Schnittstellen für den Zugriff auf eine Domäne.
    /// </summary>
    public interface ICommandProcessor
    {
        /// <summary>
        /// Prozessiert den Befehl ansynchron.
        /// </summary>
        /// <param name="command">Der Befehl, der prozessiert werden soll.</param>
        /// <returns>Die asynchrone Operation.</returns>
        Task HandleAsync([NotNull]ICommand command);

        /// <summary>
        /// Prozessiert die übergebenen Befehle ansynchron.
        /// </summary>
        /// <param name="aggregateId">Die Id des Aggregates.</param>
        /// <param name="commands">Die Befehle, die prozessiert werden sollen.</param>
        /// <returns>Die asynchrone Operation.</returns>
        Task HandleAsync(Guid aggregateId, [NotNull]ICommand[] commands);
    }
}