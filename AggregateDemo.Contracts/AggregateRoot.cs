namespace AggregateDemo.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Transactions;

    /// <summary>
    /// Die Aggregate Root Basisklasse.
    /// </summary>
    /// <typeparam name="TAggregateState">Der Typ des Aggregatzustandsobjektes.</typeparam>
    public abstract class AggregateRoot<TAggregateState> : IAggregateRoot, IEnlistmentNotification where TAggregateState : class, IAggregateState
    {
        private Transaction currentTransaction;

        private IList<IDomainEvent> aggregateChanges = new List<IDomainEvent>();

        private IList<IDomainEvent> tempCollection = new List<IDomainEvent>();

        private readonly TransactionLock transactionLock = new TransactionLock();

        private readonly List<int> locks = new List<int>();

        /// <summary>
        /// Initialisiert eine neue Instanz der AggregateRoot Klasse.
        /// </summary>
        protected AggregateRoot(TAggregateState aggregateState)
        {
            this.State = aggregateState;
        }

        /// <summary>
        /// Holt die eindeutige Kennung des Aggregates.
        /// </summary>
        public Guid Id
        {
            get
            {
                return this.State.Id;
            }
        }

        /// <summary>
        /// Holt den Zustand des Aggregates.
        /// </summary>
        public TAggregateState State { get; private set; }

        /// <summary>
        /// Holt die Auflistung der Änderungen am Aggregat.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> Changes
        {
            get
            {
                return new ReadOnlyCollection<IDomainEvent>(this.aggregateChanges);
            }
        }

        /// <summary>
        /// Holt einen Wert, der angibt, ob das Aggregat gesperrt ist.
        /// </summary>
        public bool IsLocked
        {
            get
            {
                return this.locks.Any();
            }
        }
        
        /// <summary>
        /// Entfernt die angegebene Event Instanz aus der internen Auflistung noch nicht persistierter Ereignisse.
        /// </summary>
        /// <param name="event">Die Instanz, die entfernt werden soll.</param>
        public void RemoveEventFromChangesCollection(IDomainEvent @event)
        {
            if (Transaction.Current == null)
            {
                // Führt die Aktion ohne eine Transaktion auf der Originalliste aus!
                if (this.aggregateChanges.Contains(@event))
                {
                    this.aggregateChanges.Remove(@event);
                }
            }
            else
            {
                this.transactionLock.Lock();

                // Fügt die Aggregat Instanz der aktuellen Transaktion hinzu.
                if (this.currentTransaction == null)
                {
                    this.currentTransaction = Transaction.Current;
                    this.currentTransaction.EnlistVolatile(this, EnlistmentOptions.None);

                    // Kopiert die Auflistung der Änderungen in eine temporäre Auflistung, um einen Rollback zu ermöglichen.
                    this.tempCollection = new List<IDomainEvent>(this.aggregateChanges);
                }

                if (this.tempCollection.Contains(@event))
                {
                    this.tempCollection.Remove(@event);
                }
            }
        }

        /// <summary>
        /// Holt die nächste verfügbare Versionsnummer.
        /// </summary>
        /// <returns>Die Nummer.</returns>
        public long GetNextVersion()
        {
            return this.State.Version + 1;
        }

        /// <summary>
        /// Sperrt das Aggregat.
        /// </summary>
        public void Lock()
        {
            var contextId = Thread.CurrentThread.ManagedThreadId;
            this.locks.Add(contextId);
        }

        /// <summary>
        /// Entsperrt das Aggregat.
        /// </summary>
        public void Unlock()
        {
            var contextId = Thread.CurrentThread.ManagedThreadId;
            if (this.locks.Contains(contextId))
            {
                this.locks.Remove(contextId);
            }
        }

        /// <summary>
        /// Wendet das Domain Event auf dem Zustandsobjekt an.
        /// </summary>
        /// <typeparam name="TDomainEvent">Der Typparameter.</typeparam>
        /// <param name="domainEvent">Der Domain Event, das angewendet werden soll.</param>
        public void Apply<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : class, IDomainEvent
        {
            if (!this.transactionLock.Locked)
            {
                if (domainEvent.Version > this.State.Version)
                {
                    this.State.Mutate(domainEvent);
                    this.aggregateChanges.Add(domainEvent);
                }    
            }
            else
            {
                throw new TransactionException("Aggregate is locked!");
            }
        }

        /// <summary>
        /// Notifies an enlisted object that a transaction is being prepared for commitment.
        /// </summary>
        /// <param name="preparingEnlistment">A <see cref="T:System.Transactions.PreparingEnlistment"/> object used to send a response to the transaction manager.</param>
        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            preparingEnlistment.Prepared();
        }

        /// <summary>
        /// Notifies an enlisted object that a transaction is being committed.
        /// </summary>
        /// <param name="enlistment">An <see cref="T:System.Transactions.Enlistment"/> object used to send a response to the transaction manager.</param>
        public void Commit(Enlistment enlistment)
        {
            this.aggregateChanges = this.tempCollection;
            this.currentTransaction = null;
            this.tempCollection = new List<IDomainEvent>();

            this.transactionLock.Unlock();
            enlistment.Done();
        }

        /// <summary>
        /// Notifies an enlisted object that a transaction is being rolled back (aborted).
        /// </summary>
        /// <param name="enlistment">A <see cref="T:System.Transactions.Enlistment"/> object used to send a response to the transaction manager.</param>
        public void Rollback(Enlistment enlistment)
        {
            this.currentTransaction = null;
            this.tempCollection = new List<IDomainEvent>();

            this.transactionLock.Unlock();
            enlistment.Done();
        }

        /// <summary>
        /// Notifies an enlisted object that the status of a transaction is in doubt.
        /// </summary>
        /// <param name="enlistment">An <see cref="T:System.Transactions.Enlistment"/> object used to send a response to the transaction manager.</param>
        public void InDoubt(Enlistment enlistment)
        {
            this.transactionLock.Unlock();
            enlistment.Done();
        }
    }
}